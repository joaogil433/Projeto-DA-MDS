// FormModoCompra.cs
// Responsabilidade: interface gráfica para a execução de uma compra (US4)
// Permite ao utilizador preencher quantidades adquiridas e preços reais,
// adicionar itens não previstos e fechar a compra com data/hora e utilizador.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS.Views
{
    public partial class FormModoCompra : Form
    {
        // Lista de compras que está a ser executada
        private ListaCompra _listaCompra;

        // Orçamento do mês atual — usado para calcular o disponível e mostrar alertas
        private Orcamento _orcamento;

        // Contexto da BD — uma instância partilhada durante a vida do form
        private IshoppingContext _db;

        // ── CONSTRUTORES ──────────────────────────────────────────────────────

        // Construtor vazio — exigido pelo Designer do Visual Studio
        // NÃO deve aceder à BD para não rebentar o Designer
        public FormModoCompra()
        {
            InitializeComponent();
        }

        // Construtor real — chamado pelo FormPrincipal ao abrir o modo compra
        public FormModoCompra(ListaCompra listaCompra)
        {
            InitializeComponent();
            _listaCompra = listaCompra;
            _db = new IshoppingContext();
        }

        // ── LOAD ──────────────────────────────────────────────────────────────

        private void FormModoCompra_Load(object sender, EventArgs e)
        {
            // Segurança: garante que o contexto está inicializado
            if (_db == null)
                _db = new IshoppingContext();

            // Segurança: garante que a lista tem uma coleção de itens válida
            if (_listaCompra == null)
                _listaCompra = new ListaCompra { Itens = new List<ItemCompra>() };

            if (_listaCompra.Itens == null)
                _listaCompra.Itens = new List<ItemCompra>();

            // 1. Recarrega a lista da BD com os itens e artigos (garante dados frescos)
            CarregarListaDaBD();

            // 2. Carrega o orçamento do mês atual
            CarregarOrcamento();

            // 3. Carrega os artigos para o ComboBox de itens não previstos
            CarregarArtigos();

            // 4. Preenche a tabela com os itens da lista
            PopularTabela();

            // 5. Atualiza os labels do orçamento
            AtualizarOrcamento();
        }

        // ── CARREGAR LISTA DA BD ──────────────────────────────────────────────

        // Recarrega a lista completa da BD com todos os itens e artigos
        // Garante que os dados em memória estão sincronizados com a BD
        private void CarregarListaDaBD()
        {
            var listaBD = _db.ListasCompras
                .Include("Itens.Artigo")
                .FirstOrDefault(l => l.Id == _listaCompra.Id);

            if (listaBD != null)
                _listaCompra = listaBD;
        }

        // ── TABELA DE ITENS ───────────────────────────────────────────────────

        // Limpa e redesenha a grelha com todos os itens da lista atual
        private void PopularTabela()
        {
            dgvItens.Rows.Clear();

            foreach (var item in _listaCompra.Itens)
            {
                bool isPrevisto = item is ItemPrevisto;
                var prev    = item as ItemPrevisto;
                var naoPrev = item as ItemNaoPrevisto;

                dgvItens.Rows.Add(
                    item.Id,
                    item.Artigo?.Nome ?? "—",
                    isPrevisto ? prev.QuantidadePrevista.ToString() : "—",
                    item.QuantidadeAdquirida,
                    item.PrecoUnitario,
                    isPrevisto ? "Previsto" : "Não Previsto",
                    naoPrev?.Observacoes ?? ""
                );
            }
        }

        // ── ORÇAMENTO ─────────────────────────────────────────────────────────

        // Carrega o orçamento do mês/ano atual da BD
        private void CarregarOrcamento()
        {
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;

            _orcamento = _db.Orcamentos
                .FirstOrDefault(o => o.Mes == mes && o.Ano == ano);
        }

        // Atualiza os labels do orçamento calculando o total a partir da BD
        // Usa a BD em vez da memória para garantir valores atualizados após cada edição
        private void AtualizarOrcamento()
        {
            decimal orcamentoMax = _orcamento?.ValorMaximo ?? 0;

            // Calcula o total gasto diretamente da BD (valores atualizados)
            decimal totalGasto = CalcularTotalGastoDaBD();
            decimal disponivel = orcamentoMax - totalGasto;

            // Atualiza também os itens em memória para ficarem consistentes
            CarregarListaDaBD();

            lblOrcamentoMax.Text = $"Orçamento: {orcamentoMax:C2}";
            lblTotalGasto.Text   = $"Total gasto: {totalGasto:C2}";
            lblDisponivel.Text   = $"Disponível: {disponivel:C2}";

            // Alerta visual se o orçamento foi ultrapassado
            if (orcamentoMax > 0 && totalGasto > orcamentoMax)
            {
                lblDisponivel.ForeColor = Color.Red;
                lblAlerta.Visible = true;
            }
            else
            {
                lblDisponivel.ForeColor = Color.Green;
                lblAlerta.Visible = false;
            }
        }

        // Calcula o total gasto indo diretamente à BD — evita usar dados desatualizados em memória
        private decimal CalcularTotalGastoDaBD()
        {
            return _db.ItensCompra
                .Where(i => i.ListaCompraId == _listaCompra.Id && i.QuantidadeAdquirida > 0)
                .Sum(i => (decimal?)(i.QuantidadeAdquirida * i.PrecoUnitario)) ?? 0m;
        }

        // ── ARTIGOS PARA O COMBO ──────────────────────────────────────────────

        // Carrega todos os artigos da BD para o ComboBox de itens não previstos
        private void CarregarArtigos()
        {
            try
            {
                var artigos = _db.Artigos.OrderBy(a => a.Nome).ToList();
                cmbArtigo.DataSource    = artigos;
                cmbArtigo.DisplayMember = "Nome";
                cmbArtigo.ValueMember   = "Id";
                cmbArtigo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar artigos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── EDIÇÃO DIRETA NA TABELA ───────────────────────────────────────────

        // Disparado quando o utilizador termina de editar uma célula
        // Guarda a alteração na BD e atualiza os labels do orçamento em tempo real
        private void dgvItens_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var idCell = dgvItens.Rows[e.RowIndex].Cells["colId"].Value;
            if (idCell == null) return;

            int itemId = (int)idCell;
            if (itemId <= 0) return;

            var item = _db.ItensCompra.Find(itemId);
            if (item == null) return;

            if (e.ColumnIndex == dgvItens.Columns["colQtdAdquirida"].Index)
            {
                if (int.TryParse(
                    dgvItens.Rows[e.RowIndex].Cells["colQtdAdquirida"].Value?.ToString(),
                    out int qtd))
                    item.QuantidadeAdquirida = qtd;
            }
            else if (e.ColumnIndex == dgvItens.Columns["colPreco"].Index)
            {
                string precoStr = dgvItens.Rows[e.RowIndex].Cells["colPreco"].Value?.ToString()
                    ?.Replace(",", ".");

                if (decimal.TryParse(precoStr,
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal preco))
                    item.PrecoUnitario = preco;
            }

            _db.SaveChanges();

            // Adia o redesenho para depois do evento CellEndEdit terminar completamente
            // Evita o erro "reentrant call to SetCurrentCellAddressCore"
            this.BeginInvoke(new Action(() =>
            {
                CarregarListaDaBD();
                PopularTabela();
                AtualizarOrcamento();
            }));
        }

        // ── ADICIONAR ITEM NÃO PREVISTO ───────────────────────────────────────

        // Cria um ItemNaoPrevisto e guarda na BD
        // Observações são obrigatórias (critério de aceitação da US4)
        private void btnAdicionarNaoPrevisto_Click(object sender, EventArgs e)
        {
            // Validação: tem de estar um artigo selecionado
            if (cmbArtigo.SelectedItem == null)
            {
                MessageBox.Show("Seleciona um artigo antes de adicionar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validação: observações são obrigatórias para itens não previstos
            if (string.IsNullOrWhiteSpace(tbObservacoes.Text))
            {
                MessageBox.Show("O campo de observações é obrigatório para itens não previstos.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var artigo = (Artigo)cmbArtigo.SelectedItem;
            int utilizadorId = Sessao.UtilizadorAtual?.Id ?? 1;

            var novoItem = new ItemNaoPrevisto
            {
                ListaCompraId       = _listaCompra.Id,
                ArtigoId            = artigo.Id,
                QuantidadeAdquirida = 1,
                PrecoUnitario       = 0,
                Observacoes         = tbObservacoes.Text.Trim(),
                UtilizadorId        = utilizadorId
            };

            try
            {
                _db.ItensCompra.Add(novoItem);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao guardar item: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Limpa os campos e atualiza a tabela
            tbObservacoes.Clear();
            cmbArtigo.SelectedIndex = -1;

            CarregarListaDaBD();
            PopularTabela();
            AtualizarOrcamento();
        }

        // ── FECHAR COMPRA ─────────────────────────────────────────────────────

        // Fecha a compra — regista data/hora e utilizador, muda estado para "Fechada"
        private void btnFecharCompra_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show(
                "Tens a certeza que queres fechar esta compra?\nNão poderás fazer mais alterações.",
                "Fechar Compra",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            try
            {
                var lista = _db.ListasCompras.Find(_listaCompra.Id);

                if (lista == null)
                {
                    MessageBox.Show("Erro: lista não encontrada.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Atualiza o estado e regista os dados de fecho
                lista.Estado              = "Fechada";
                lista.DataFecho           = DateTime.Now;
                lista.UtilizadorAlterouId = Sessao.UtilizadorAtual.Id;
                lista.DataAlteracao       = DateTime.Now;

                _db.SaveChanges();

                MessageBox.Show(
                    $"Compra fechada com sucesso!\nData: {lista.DataFecho:dd/MM/yyyy HH:mm}\nUtilizador: {Sessao.UtilizadorAtual.Username}",
                    "Compra Fechada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fechar compra: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── FECHAR FORMULÁRIO ─────────────────────────────────────────────────

        // Liberta o contexto da BD ao fechar o form
        private void FormModoCompra_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db?.Dispose();
        }
    }
}