using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS.Views
{
    public partial class FormEstatisticas : Form
    {
        private IshoppingContext _db;

        public FormEstatisticas()
        {
            InitializeComponent();
            _db = new IshoppingContext();
        }

        private void FormEstatisticas_Load(object sender, EventArgs e)
        {
            CarregarTab1();
            CarregarTab2();
        }

        // ── TAB 1 ────────────────────────────────────────────────────────────

        private void CarregarTab1()
        {
            CarregarOrcamentoVsTotal();
            CarregarComprasFechadas();
        }

        private void CarregarOrcamentoVsTotal()
        {
            dgvOrcamentoVsTotal.Rows.Clear();

            var orcamentos = _db.Orcamentos
                .OrderByDescending(o => o.Ano)
                .ThenByDescending(o => o.Mes)
                .ToList();

            var comprasFechadas = _db.ListasCompras
                .Include("Itens")
                .Where(l => l.Estado == "Fechada" && l.DataFecho != null)
                .ToList();

            foreach (var orc in orcamentos)
            {
                // Total gasto nas compras fechadas desse mês/ano
                decimal totalGasto = comprasFechadas
                    .Where(l => l.DataFecho.Value.Month == orc.Mes
                             && l.DataFecho.Value.Year == orc.Ano)
                    .SelectMany(l => l.Itens)
                    .Sum(i => i.QuantidadeAdquirida * i.PrecoUnitario);

                decimal diferenca = orc.ValorMaximo - totalGasto;
                string estado = totalGasto > orc.ValorMaximo ? "⚠ Ultrapassado" : "✔ Dentro";

                int rowIdx = dgvOrcamentoVsTotal.Rows.Add(
                    $"{orc.Mes:D2}/{orc.Ano}",
                    orc.ValorMaximo.ToString("C2"),
                    totalGasto.ToString("C2"),
                    diferenca.ToString("C2"),
                    estado
                );

                // Alerta visual a vermelho se ultrapassou
                if (totalGasto > orc.ValorMaximo)
                    dgvOrcamentoVsTotal.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.Red;
                else
                    dgvOrcamentoVsTotal.Rows[rowIdx].DefaultCellStyle.ForeColor = Color.Green;
            }
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            var compras = _db.ListasCompras
                .Include("Itens.Artigo")
                .Include("UtilizadorCriou")
                .Where(l => l.Estado == "Fechada")
                .ToList();

            if (compras.Count == 0)
            {
                MessageBox.Show("Não existem compras fechadas para exportar.");
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = $"compras_fechadas_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Title = "Exportar Compras Fechadas"
            };

            if (saveDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (var writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Cabeçalho
                    writer.WriteLine("Lista;Data Criação;Data Fecho;Estado;Criado Por;Artigo;Tipo Item;Qtd Prevista;Qtd Adquirida;Preço Unitário;Total Item;Observações");

                    foreach (var compra in compras)
                    {
                        foreach (var item in compra.Itens)
                        {
                            var prev = item as ItemPrevisto;
                            var naoPrev = item as ItemNaoPrevisto;

                            string tipoItem = prev != null ? "Previsto" : "Não Previsto";
                            string qtdPrevista = prev != null ? prev.QuantidadePrevista.ToString() : "-";
                            string observacoes = naoPrev?.Observacoes ?? "";
                            decimal totalItem = item.QuantidadeAdquirida * item.PrecoUnitario;

                            writer.WriteLine(
                                $"{compra.Nome};" +
                                $"{compra.DataCriacao:dd/MM/yyyy HH:mm};" +
                                $"{compra.DataFecho:dd/MM/yyyy HH:mm};" +
                                $"{compra.Estado};" +
                                $"{compra.UtilizadorCriou?.Username ?? "-"};" +
                                $"{item.Artigo?.Nome ?? "-"};" +
                                $"{tipoItem};" +
                                $"{qtdPrevista};" +
                                $"{item.QuantidadeAdquirida};" +
                                $"{item.PrecoUnitario:F2};" +
                                $"{totalItem:F2};" +
                                $"{observacoes}"
                            );
                        }
                    }
                }

                MessageBox.Show($"Exportação concluída!\n{saveDialog.FileName}", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao exportar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarComprasFechadas()
        {
            dgvComprasFechadas.Rows.Clear();

            var compras = _db.ListasCompras
                .Include("Itens")
                .Where(l => l.Estado == "Fechada")
                .ToList();

            foreach (var compra in compras)
            {
                int totalItens = compra.Itens.Count;
                int previstos = compra.Itens.Count(i => i is ItemPrevisto);
                int naoPrevistos = compra.Itens.Count(i => i is ItemNaoPrevisto);

                double pctPrev = totalItens > 0 ? (double)previstos / totalItens * 100 : 0;
                double pctNaoPrev = totalItens > 0 ? (double)naoPrevistos / totalItens * 100 : 0;

                dgvComprasFechadas.Rows.Add(
                    compra.Nome,
                    compra.DataFecho?.ToString("dd/MM/yyyy HH:mm") ?? "-",
                    totalItens,
                    previstos,
                    naoPrevistos,
                    $"{pctPrev:F1}%",
                    $"{pctNaoPrev:F1}%"
                );
            }
        }

        // ── TAB 2 ────────────────────────────────────────────────────────────

        private void CarregarTab2()
        {
            CarregarSugestaoOrcamento();
            CarregarSugestaoListaSemana();
        }

        private void CarregarSugestaoOrcamento()
        {
            // Calcula média dos gastos dos últimos 3 meses
            var hoje = DateTime.Now;
            var comprasFechadas = _db.ListasCompras
                .Include("Itens")
                .Where(l => l.Estado == "Fechada" && l.DataFecho != null)
                .ToList();

            var ultimos3Meses = new List<(int Mes, int Ano)>
            {
                (hoje.AddMonths(-1).Month, hoje.AddMonths(-1).Year),
                (hoje.AddMonths(-2).Month, hoje.AddMonths(-2).Year),
                (hoje.AddMonths(-3).Month, hoje.AddMonths(-3).Year)
            };

            var gastosPorMes = ultimos3Meses.Select(m =>
            {
                decimal gasto = comprasFechadas
                    .Where(l => l.DataFecho.Value.Month == m.Mes
                             && l.DataFecho.Value.Year == m.Ano)
                    .SelectMany(l => l.Itens)
                    .Sum(i => i.QuantidadeAdquirida * i.PrecoUnitario);
                return gasto;
            }).ToList();

            decimal media = gastosPorMes.Count > 0 ? gastosPorMes.Average() : 0;
            decimal sugestao = Math.Ceiling(media * 1.10m); // +10% de margem

            var orcamentoAtual = _db.Orcamentos
                .FirstOrDefault(o => o.Mes == hoje.Month && o.Ano == hoje.Year);

            lblMediaGastos.Text = $"Média dos últimos 3 meses: {media:C2}";
            lblSugestaoOrcamento.Text = $"Sugestão para {hoje:MMMM yyyy}: {sugestao:C2} (+10% margem)";

            if (orcamentoAtual != null)
                lblOrcamentoAtual.Text = $"Orçamento atual definido: {orcamentoAtual.ValorMaximo:C2}";
            else
                lblOrcamentoAtual.Text = "Orçamento atual: não definido para este mês";
        }

        private void CarregarSugestaoListaSemana()
        {
            dgvSugestaoSemana.Rows.Clear();

            // Agrupa compras fechadas por semana do mês
            var compras = _db.ListasCompras
                .Include("Itens.Artigo")
                .Where(l => l.Estado == "Fechada" && l.DataFecho != null)
                .ToList();

            // Artigos mais comprados por semana (1ª, 2ª, 3ª, 4ª semana)
            var itensPorSemana = compras
                .SelectMany(l => l.Itens.Select(i => new
                {
                    Semana = (l.DataFecho.Value.Day - 1) / 7 + 1,
                    Artigo = i.Artigo?.Nome ?? "Desconhecido",
                    i.QuantidadeAdquirida
                }))
                .GroupBy(x => new { x.Semana, x.Artigo })
                .Select(g => new
                {
                    g.Key.Semana,
                    g.Key.Artigo,
                    TotalQtd = g.Sum(x => x.QuantidadeAdquirida)
                })
                .OrderBy(x => x.Semana)
                .ThenByDescending(x => x.TotalQtd)
                .ToList();

            foreach (var item in itensPorSemana)
            {
                dgvSugestaoSemana.Rows.Add(
                    $"Semana {item.Semana}",
                    item.Artigo,
                    item.TotalQtd
                );
            }

            if (itensPorSemana.Count == 0)
                lblSemSugestao.Visible = true;
            else
                lblSemSugestao.Visible = false;
        }

        private void FormEstatisticas_FormClosing(object sender, FormClosingEventArgs e)
        {
            _db?.Dispose();
        }
    }
}