// FormModoCompra.cs
// Responsabilidade: interface gráfica para a execução de uma compra (US4)
// Permite ao utilizador registar preços reais, quantidades adquiridas,
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
        // Lista de compras que está a ser executada neste momento
        private ListaCompra _listaCompra;

        // Orçamento do mês atual — usado para calcular o disponível e mostrar alertas
        private Orcamento _orcamento;

        // Contexto da base de dados — uma única instância partilhada durante a vida do form
        // (seguro porque o form é modal e de curta duração)
        private IshoppingContext _db;

        // ── CONSTRUTORES ─────────────────────────────────────────────────────

        // Construtor vazio — exigido pelo Designer do Visual Studio para pré-visualizar o form
        // NÃO deve fazer nada com a BD, caso contrário o Designer rebenta ao carregar
        public FormModoCompra()
        {
            InitializeComponent();
        }

        // Construtor real — chamado pela aplicação (ex: FormPrincipal ao abrir modo compra)
        // Recebe a lista de compras que já foi carregada da BD com os seus itens
        public FormModoCompra(ListaCompra listaCompra)
        {
            InitializeComponent();
            _listaCompra = listaCompra;           // guarda a referência da lista
            _db = new IshoppingContext();         // abre a ligação à BD
        }

        // ── LOAD ──────────────────────────────────────────────────────────────

        private void FormModoCompra_Load(object sender, EventArgs e)
        {
            // Segurança extra: se o construtor vazio foi usado (Designer),
            // inicializa o contexto antes de tentar aceder à BD
            if (_db == null)
                _db = new IshoppingContext();

            // Segurança extra: garante que a lista tem uma coleção de itens válida
            // para evitar NullReferenceException nas operações seguintes
            if (_listaCompra == null)
                _listaCompra = new ListaCompra { Itens = new List<ItemCompra>() };

            if (_listaCompra.Itens == null)
                _listaCompra.Itens = new List<ItemCompra>();

            // 1. Carrega o orçamento do mês atual da BD
            //    Se não existir orçamento definido, _orcamento fica null e
            //    os labels mostrarão 0,00€ (sem alertas ativos)
            CarregarOrcamento();

            // 2. Carrega os artigos da BD para a ComboBox "Adicionar Item Não Previsto"
            //    Se a BD ainda não tiver artigos, a ComboBox ficará vazia
            //    e o utilizador não conseguirá adicionar itens não previstos
            CarregarArtigos();

            // 3. Preenche a tabela com os itens já existentes na lista
            PopularTabelaManual();

            // 4. Atualiza os labels do orçamento (máximo, gasto, disponível, alerta)
            AtualizarOrcamento();
        }

        // ── TABELA DE ITENS ───────────────────────────────────────────────────

        // Limpa e redesenha a grelha dgvItens com todos os itens da lista atual
        // Distingue visualmente ItemPrevisto de ItemNaoPrevisto
        private void PopularTabelaManual()
        {
            dgvItens.Rows.Clear();

            foreach (var item in _listaCompra.Itens)
            {
                // Verifica o tipo concreto do item (herança: ItemPrevisto ou ItemNaoPrevisto)
                bool isPrevisto = item is ItemPrevisto;
                var prev = item as ItemPrevisto;
                var naoPrev = item as ItemNaoPrevisto;

                // Adiciona uma linha na grelha com todos os dados do item
                dgvItens.Rows.Add(
                    item.Id,                                                  // colId (hidden)
                    item.Artigo?.Nome ?? "—",                                 // colArtigo
                    isPrevisto ? prev.QuantidadePrevista.ToString() : "—",    // colQtdPrevista
                    item.QuantidadeAdquirida,                                  // colQtdAdquirida (editável)
                    item.PrecoUnitario,                                        // colPreco (editável)
                    isPrevisto ? "Previsto" : "Não Previsto",                  // colTipo
                    naoPrev?.Observacoes ?? ""                                 // colObservacoes
                );
            }
        }

        // ── ORÇAMENTO ────────────────────────────────────────────────────────

        // Vai à BD buscar o orçamento definido para o mês e ano atuais
        // Chama AtualizarOrcamento() para refletir o valor nos labels
        private void CarregarOrcamento()
        {
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;

            // Tenta encontrar um orçamento para o mês corrente
            _orcamento = _db.Orcamentos
                .FirstOrDefault(o => o.Mes == mes && o.Ano == ano);

            // Atualiza os labels mesmo que não haja orçamento (mostrarão 0,00€)
            AtualizarOrcamento();
        }

        // Recalcula e atualiza os três labels de orçamento no topo do form:
        // - Orçamento máximo definido
        // - Total já gasto nesta lista
        // - Valor disponível (pode ser negativo se ultrapassado)
        // Também controla a visibilidade do label de alerta vermelho
        private void AtualizarOrcamento()
        {
            decimal orcamentoMax = _orcamento?.ValorMaximo ?? 0;  // 0 se não há orçamento definido
            decimal totalGasto = CalcularTotalGasto();
            decimal disponivel = orcamentoMax - totalGasto;

            // Atualiza os textos dos labels
            lblOrcamentoMax.Text = $"Orçamento: {orcamentoMax:C2}";
            lblTotalGasto.Text = $"Total gasto: {totalGasto:C2}";
            lblDisponivel.Text = $"Disponível: {disponivel:C2}";

            // Alerta visual: vermelho e label visível se o orçamento foi ultrapassado
            if (orcamentoMax > 0 && totalGasto > orcamentoMax)
            {
                lblDisponivel.ForeColor = Color.Red;
                lblAlerta.Visible = true;   // label com aviso de orçamento ultrapassado
            }
            else
            {
                lblDisponivel.ForeColor = Color.Green;
                lblAlerta.Visible = false;
            }
        }

        // Calcula o total gasto somando (quantidade adquirida × preço unitário)
        // de todos os itens que já têm quantidade adquirida > 0
        private decimal CalcularTotalGasto()
        {
            return _listaCompra.Itens
                .Where(i => i.QuantidadeAdquirida > 0)
                .Sum(i => i.QuantidadeAdquirida * i.PrecoUnitario);
        }

        // ── ARTIGOS PARA O COMBO ──────────────────────────────────────────────

        // Carrega todos os artigos existentes na BD para a ComboBox de itens não previstos
        // Os artigos são ordenados por nome para facilitar a seleção
        private void CarregarArtigos()
        {
            try
            {
                // Carrega os artigos diretamente do DbContext, ordenados por nome
                var artigos = _db.Artigos.OrderBy(a => a.Nome).ToList();

                cmbArtigo.DataSource = artigos;
                cmbArtigo.DisplayMember = "Nome";   // texto visível no combo
                cmbArtigo.ValueMember = "Id";     // valor interno usado para guardar na BD
                cmbArtigo.SelectedIndex = -1;       // começa sem nenhum selecionado
            }
            catch (Exception ex)
            {
                // Mostra o erro ao utilizador — não silencia para não esconder problemas reais
                MessageBox.Show("Erro ao carregar artigos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── RECARREGAR ITENS DA BD ────────────────────────────────────────────

        // Recarrega a lista completa da BD (incluindo itens e artigos relacionados)
        // Chamada após guardar alterações para garantir que a grelha está sincronizada
        private void CarregarItens()
        {
            var listaBD = _db.ListasCompras
                .Include("Itens.Artigo")  // carrega os artigos relacionados com os itens
                .FirstOrDefault(l => l.Id == _listaCompra.Id);

            // Substitui a referência local pela versão atualizada da BD
            if (listaBD != null)
                _listaCompra = listaBD;

            // Redesenha a tabela e atualiza os valores do orçamento
            PopularTabelaManual();
            AtualizarOrcamento();
        }

        // ── EDIÇÃO DIRETA NA TABELA ───────────────────────────────────────────

        // Evento disparado quando o utilizador termina de editar uma célula da grelha
        // Apenas as colunas "colQtdAdquirida" e "colPreco" são editáveis
        // Guarda a alteração imediatamente na BD e atualiza o orçamento em tempo real
        private void dgvItens_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Ignora linhas inválidas (ex: cabeçalho)
            if (e.RowIndex < 0) return;

            // Lê o Id do item a partir da coluna escondida "colId"
            int itemId = (int)dgvItens.Rows[e.RowIndex].Cells["colId"].Value;

            // Vai à BD buscar o item pelo Id
            var item = _db.ItensCompra.Find(itemId);
            if (item == null) return;

            // Verifica qual coluna foi editada e atualiza o campo correspondente
            if (e.ColumnIndex == dgvItens.Columns["colQtdAdquirida"].Index)
            {
                // Tenta converter o valor introduzido para inteiro
                if (int.TryParse(dgvItens.Rows[e.RowIndex].Cells["colQtdAdquirida"].Value?.ToString(), out int qtd))
                    item.QuantidadeAdquirida = qtd;
            }
            else if (e.ColumnIndex == dgvItens.Columns["colPreco"].Index)
            {
                // Tenta converter o valor introduzido para decimal
                if (decimal.TryParse(dgvItens.Rows[e.RowIndex].Cells["colPreco"].Value?.ToString(), out decimal preco))
                    item.PrecoUnitario = preco;
            }

            // Guarda as alterações na BD imediatamente (atualização em tempo real — critério US4)
            _db.SaveChanges();

            // Recalcula e atualiza os labels do orçamento após cada alteração
            AtualizarOrcamento();
        }

        // ── ADICIONAR ITEM NÃO PREVISTO ───────────────────────────────────────

        // Botão "Adicionar Item Não Previsto"
        // Valida os campos, cria um ItemNaoPrevisto e guarda na BD
        // O campo Observacoes é obrigatório (critério de aceitação da US4)
        private void btnAdicionarNaoPrevisto_Click(object sender, EventArgs e)
        {
            // Valida se um artigo foi selecionado
            if (cmbArtigo.SelectedItem == null)
            {
                MessageBox.Show("Seleciona um artigo antes de adicionar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Valida se o campo de observações foi preenchido (obrigatório por critério US4)
            if (string.IsNullOrWhiteSpace(tbObservacoes.Text))
            {
                MessageBox.Show("O campo de observações é obrigatório para itens não previstos.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lê o artigo selecionado no ComboBox
            var artigo = (Artigo)cmbArtigo.SelectedItem;

            // Obtém o Id do utilizador com sessão iniciada
            int utilizadorIdAtual = Sessao.UtilizadorAtual.Id;

            // Cria o novo item não previsto com os dados introduzidos
            var novoItem = new ItemNaoPrevisto
            {
                ListaCompraId = _listaCompra.Id,    // associa à lista atual
                ArtigoId = artigo.Id,          // artigo selecionado
                Artigo = artigo,             // referência ao objeto (para PopularTabelaManual)
                QuantidadeAdquirida = 1,                 // começa com quantidade 1 (editável na grelha)
                PrecoUnitario = 0,                  // começa com preço 0 (editável na grelha)
                Observacoes = tbObservacoes.Text.Trim(),  // observação obrigatória
                UtilizadorId = utilizadorIdAtual   // registo de quem adicionou
            };

            // Garante que a coleção de itens está inicializada antes de adicionar
            if (_listaCompra.Itens == null)
                _listaCompra.Itens = new List<ItemCompra>();

            // Adiciona à coleção local (para a tabela refletir imediatamente)
            _listaCompra.Itens.Add(novoItem);

            // Persiste o novo item na BD
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

            // Limpa os campos do formulário de adição para o próximo item
            tbObservacoes.Clear();
            cmbArtigo.SelectedIndex = -1;

            // Atualiza a tabela e o painel do orçamento
            PopularTabelaManual();
            AtualizarOrcamento();
        }

        // ── FECHAR COMPRA ─────────────────────────────────────────────────────

        // Botão "Fechar Compra"
        // Pede confirmação, altera o estado para "Fechada",
        // regista data/hora e utilizador responsável (critério US4) e fecha o form
        private void btnFecharCompra_Click(object sender, EventArgs e)
        {
            // Pede confirmação antes de fechar — operação irreversível
            var confirmacao = MessageBox.Show(
                "Tens a certeza que queres fechar esta compra?\nNão poderás fazer mais alterações.",
                "Fechar Compra",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            try
            {
                // Carrega a lista da BD para garantir que estamos a alterar o registo atual
                var lista = _db.ListasCompras.Find(_listaCompra.Id);

                if (lista == null)
                {
                    MessageBox.Show("Erro: lista não encontrada na base de dados.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Atualiza o estado e regista os dados de fecho (critério US4)
                lista.Estado = "Fechada";
                lista.DataFecho = DateTime.Now;                    // data e hora do fecho
                lista.UtilizadorAlterouId = Sessao.UtilizadorAtual.Id;   // utilizador responsável
                lista.DataAlteracao = DateTime.Now;

                _db.SaveChanges();

                // Informa o utilizador com o resumo do fecho
                MessageBox.Show(
                    $"Compra fechada com sucesso!\nData: {lista.DataFecho:dd/MM/yyyy HH:mm}\nUtilizador: {Sessao.UtilizadorAtual.Username}",
                    "Compra Fechada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                // Fecha o formulário — o FormPrincipal irá recarregar a lista de compras abertas
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fechar compra: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── FECHAR FORMULÁRIO ─────────────────────────────────────────────────

        // Evento disparado quando o form é fechado (pelo X ou pelo btnFecharCompra)
        // Liberta o contexto da BD para evitar fugas de memória
        private void FormModoCompra_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db?.Dispose();
        }
    }
}