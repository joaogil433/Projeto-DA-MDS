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
        private ListaCompra _listaCompra;
        private Orcamento _orcamento;
        private IshoppingContext _db;

        // Construtor vazio — usado APENAS pelo Designer do Visual Studio
        // NÃO inicializar BD aqui para não rebentar o designer!
        public FormModoCompra()
        {
            InitializeComponent();
        }

        // Construtor real — usado pela aplicação
        public FormModoCompra(ListaCompra listaCompra)
        {
            InitializeComponent();
            _listaCompra = listaCompra;
            _db = new IshoppingContext();
        }

        private void FormModoCompra_Load(object sender, EventArgs e)
        {
            // Garante que o _db está inicializado (segurança extra)
            if (_db == null)
                _db = new IshoppingContext();

            // Garante que a lista está inicializada
            if (_listaCompra == null)
                _listaCompra = new ListaCompra { Itens = new List<ItemCompra>() };

            // 1. Carrega o orçamento
            CarregarOrcamento();
            if (_orcamento == null)
            {
                _orcamento = new Orcamento
                {
                    ValorMaximo = 50.00m,
                    Mes = DateTime.Now.Month,
                    Ano = DateTime.Now.Year
                };
            }

            // 2. Carrega os artigos para a ComboBox
            try
            {
                CarregarArtigos();
            }
            catch
            {
                /* Ignora se a tabela física não existir */
            }

            // Se a ComboBox continuar vazia, injeta artigos fictícios para testes
            if (cmbArtigo.DataSource == null || cmbArtigo.Items.Count == 0)
            {
                var artigosFicticios = new List<Artigo>
                {
                    new Artigo { Id = 10, Nome = "Arroz Agulha (Teste)" },
                    new Artigo { Id = 11, Nome = "Massa Esparguete (Teste)" },
                    new Artigo { Id = 12, Nome = "Feijão Preto (Teste)" }
                };

                cmbArtigo.DataSource = artigosFicticios;
                cmbArtigo.DisplayMember = "Nome";
                cmbArtigo.ValueMember = "Id";
                cmbArtigo.SelectedIndex = -1;
            }

            // 3. Garante que a coleção de itens está inicializada
            if (_listaCompra.Itens == null)
                _listaCompra.Itens = new List<ItemCompra>();

            // 4. Injeta itens simulados se a lista estiver vazia
            if (_listaCompra.Itens.Count == 0)
            {
                var item1 = new ItemPrevisto
                {
                    Id = 1,
                    Artigo = new Artigo { Nome = "Leite Meio Gordo" },
                    QuantidadePrevista = 3,
                    QuantidadeAdquirida = 0,
                    PrecoUnitario = 0.95m
                };

                var item2 = new ItemPrevisto
                {
                    Id = 2,
                    Artigo = new Artigo { Nome = "Chocolate Milka" },
                    QuantidadePrevista = 1,
                    QuantidadeAdquirida = 1,
                    PrecoUnitario = 2.50m
                };

                _listaCompra.Itens.Add(item1);
                _listaCompra.Itens.Add(item2);
            }

            // 5. Desenha tudo no ecrã
            PopularTabelaManual();
            AtualizarOrcamento();
        }

        // Desenha os itens na dgvItens
        private void PopularTabelaManual()
        {
            dgvItens.Rows.Clear();
            foreach (var item in _listaCompra.Itens)
            {
                bool isPrevisto = item is ItemPrevisto;
                var prev = item as ItemPrevisto;
                var naoPrev = item as ItemNaoPrevisto;

                dgvItens.Rows.Add(
                    item.Id,
                    item.Artigo?.Nome ?? "Artigo Geral",
                    isPrevisto ? prev.QuantidadePrevista.ToString() : "-",
                    item.QuantidadeAdquirida,
                    item.PrecoUnitario,
                    isPrevisto ? "Previsto" : "Não Previsto",
                    naoPrev?.Observacoes ?? ""
                );
            }
        }

        // ── ORÇAMENTO ────────────────────────────────────────────────────────
        private void CarregarOrcamento()
        {
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;

            _orcamento = _db.Orcamentos
                .FirstOrDefault(o => o.Mes == mes && o.Ano == ano);

            AtualizarOrcamento();
        }

        private void AtualizarOrcamento()
        {
            decimal orcamentoMax = _orcamento?.ValorMaximo ?? 0;
            decimal totalGasto = CalcularTotalGasto();
            decimal disponivel = orcamentoMax - totalGasto;

            lblOrcamentoMax.Text = $"Orçamento: {orcamentoMax:C2}";
            lblTotalGasto.Text = $"Total gasto: {totalGasto:C2}";
            lblDisponivel.Text = $"Disponível: {disponivel:C2}";

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

        private decimal CalcularTotalGasto()
        {
            return _listaCompra.Itens
                .Where(i => i.QuantidadeAdquirida > 0)
                .Sum(i => i.QuantidadeAdquirida * i.PrecoUnitario);
        }

        // ── CARREGAR ITENS DA BD ─────────────────────────────────────────────
        private void CarregarItens()
        {
            var listaBD = _db.ListasCompras
                .Include("Itens.Artigo")
                .FirstOrDefault(l => l.Id == _listaCompra.Id);

            if (listaBD != null)
                _listaCompra = listaBD;

            PopularTabelaManual();
            AtualizarOrcamento();
        }

        // ── EDIÇÃO NA TABELA ─────────────────────────────────────────────────
        private void dgvItens_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int itemId = (int)dgvItens.Rows[e.RowIndex].Cells["colId"].Value;
            var item = _db.ItensCompra.Find(itemId);
            if (item == null) return;

            if (e.ColumnIndex == dgvItens.Columns["colQtdAdquirida"].Index)
            {
                if (int.TryParse(dgvItens.Rows[e.RowIndex].Cells["colQtdAdquirida"].Value?.ToString(), out int qtd))
                    item.QuantidadeAdquirida = qtd;
            }
            else if (e.ColumnIndex == dgvItens.Columns["colPreco"].Index)
            {
                if (decimal.TryParse(dgvItens.Rows[e.RowIndex].Cells["colPreco"].Value?.ToString(), out decimal preco))
                    item.PrecoUnitario = preco;
            }

            _db.SaveChanges();
            AtualizarOrcamento();
        }

        // ── ADICIONAR ITEM NÃO PREVISTO ──────────────────────────────────────
        private void btnAdicionarNaoPrevisto_Click(object sender, EventArgs e)
        {
            if (cmbArtigo.SelectedItem == null)
            {
                MessageBox.Show("Seleciona um artigo.");
                return;
            }

            var artigo = (Artigo)cmbArtigo.SelectedItem;

            int utilizadorIdAtual = 1;
            if (SessaoUtilizador.Atual != null)
                utilizadorIdAtual = SessaoUtilizador.Atual.Id;

            var novoItem = new ItemNaoPrevisto
            {
                ListaCompraId = _listaCompra.Id,
                ArtigoId = artigo.Id,
                Artigo = artigo,
                QuantidadeAdquirida = 1,
                PrecoUnitario = 0,
                Observacoes = tbObservacoes.Text,
                UtilizadorId = utilizadorIdAtual
            };

            if (_listaCompra.Itens == null)
                _listaCompra.Itens = new List<ItemCompra>();

            _listaCompra.Itens.Add(novoItem);

            try
            {
                _db.ItensCompra.Add(novoItem);
                _db.SaveChanges();
            }
            catch
            {
                /* Ignora em modo de teste isolado */
            }

            tbObservacoes.Clear();
            cmbArtigo.SelectedIndex = -1;

            PopularTabelaManual();
            AtualizarOrcamento();
        }

        private void CarregarArtigos()
        {
            var artigos = _db.Artigos.OrderBy(a => a.Nome).ToList();
            cmbArtigo.DataSource = artigos;
            cmbArtigo.DisplayMember = "Nome";
            cmbArtigo.ValueMember = "Id";
            cmbArtigo.SelectedIndex = -1;
        }

        // ── FECHAR COMPRA ────────────────────────────────────────────────────
        private void btnFecharCompra_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show(
                "Tens a certeza que queres fechar esta compra?",
                "Fechar Compra",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmacao != DialogResult.Yes) return;

            var lista = _db.ListasCompras.Find(_listaCompra.Id);
            lista.Estado = "Fechada";
            lista.DataFecho = DateTime.Now;
            lista.UtilizadorAlterouId = SessaoUtilizador.Atual.Id;
            lista.DataAlteracao = DateTime.Now;

            _db.SaveChanges();

            MessageBox.Show($"Compra fechada em {lista.DataFecho:dd/MM/yyyy HH:mm} por {SessaoUtilizador.Atual.Username}.");
            this.Close();
        }

        // ── FECHAR FORMULÁRIO ────────────────────────────────────────────────
        private void FormModoCompra_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db?.Dispose();
        }
    }
}