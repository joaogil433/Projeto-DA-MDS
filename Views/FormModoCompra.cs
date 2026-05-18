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

        public FormModoCompra(ListaCompra listaCompra)
        {
            InitializeComponent();
            _listaCompra = listaCompra;
            _db = new IshoppingContext();
        }

        private void FormModoCompra_Load(object sender, EventArgs e)
        {
            CarregarOrcamento();
            CarregarItens();
        }

        // ── ORÇAMENTO ────────────────────────────────────────────
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

        // ── CARREGAR ITENS ───────────────────────────────────────
        private void CarregarItens()
        {
            // Recarrega a lista da DB para ter dados frescos
            _listaCompra = _db.ListasCompras
                .Include("Itens.Artigo")
                .FirstOrDefault(l => l.Id == _listaCompra.Id);

            dgvItens.Rows.Clear();

            foreach (var item in _listaCompra.Itens)
            {
                bool isPrevisto = item is ItemPrevisto;
                var prev = item as ItemPrevisto;
                var naoPrev = item as ItemNaoPrevisto;

                dgvItens.Rows.Add(
                    item.Id,
                    item.Artigo.Nome,
                    isPrevisto ? prev.QuantidadePrevista.ToString() : "-",
                    item.QuantidadeAdquirida,
                    item.PrecoUnitario,
                    isPrevisto ? "Previsto" : "Não Previsto",
                    naoPrev?.Observacoes ?? ""
                );
            }

            AtualizarOrcamento();
        }

        // ── MARCAR ITEM COMO ADQUIRIDO ───────────────────────────
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

        // ── ADICIONAR ITEM NÃO PREVISTO ──────────────────────────
        private void btnAdicionarNaoPrevisto_Click(object sender, EventArgs e)
        {
            if (cmbArtigo.SelectedItem == null)
            {
                MessageBox.Show("Seleciona um artigo.");
                return;
            }

            var artigo = (Artigo)cmbArtigo.SelectedItem;

            var novoItem = new ItemNaoPrevisto
            {
                ListaCompraId = _listaCompra.Id,
                ArtigoId = artigo.Id,
                QuantidadeAdquirida = 0,
                PrecoUnitario = 0,
                Observacoes = tbObservacoes.Text,
                UtilizadorId = SessaoUtilizador.Atual.Id
            };

            _db.ItensCompra.Add(novoItem);
            _db.SaveChanges();

            tbObservacoes.Clear();
            cmbArtigo.SelectedIndex = -1;
            CarregarItens();
        }

        private void CarregarArtigos()
        {
            var artigos = _db.Artigos.OrderBy(a => a.Nome).ToList();
            cmbArtigo.DataSource = artigos;
            cmbArtigo.DisplayMember = "Nome";
            cmbArtigo.ValueMember = "Id";
            cmbArtigo.SelectedIndex = -1;
        }

        // ── FECHAR COMPRA ────────────────────────────────────────
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

        // ── FECHAR FORMULÁRIO ────────────────────────────────────
        private void FormModoCompra_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db?.Dispose();
        }
    }
}