using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Projeto_DA_MDS.Views
{
    public partial class FormPrincipal : Form
    {
        private ListaCompraController listaCtrl;

        public FormPrincipal()
        {
            InitializeComponent();
            listaCtrl = new ListaCompraController();
            lblUtilizador.Text = "Utilizador: " + (Sessao.UtilizadorAtual?.Nome ?? "—");
            dgvComprasAbertas.DoubleClick += new EventHandler(dgvComprasAbertas_DoubleClick);
            this.Load += (s, e) => CarregarComprasAbertas();
        }

        private void CarregarComprasAbertas()
        {
            try
            {
                List<ListaCompra> compras = listaCtrl.GetAbertas();

                dgvComprasAbertas.DataSource = null;
                dgvComprasAbertas.AutoGenerateColumns = false;
                dgvComprasAbertas.Columns.Clear();

                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Nome da Compra";
                colNome.FillWeight = 300;

                DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn();
                colData.DataPropertyName = "DataCriacao";
                colData.HeaderText = "Data Criação";
                colData.FillWeight = 160;
                colData.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                DataGridViewTextBoxColumn colCriador = new DataGridViewTextBoxColumn();
                colCriador.Name = "colCriador";
                colCriador.HeaderText = "Criado Por";
                colCriador.FillWeight = 180;

                dgvComprasAbertas.Columns.Add(colId);
                dgvComprasAbertas.Columns.Add(colNome);
                dgvComprasAbertas.Columns.Add(colData);
                dgvComprasAbertas.Columns.Add(colCriador);
                dgvComprasAbertas.DataSource = compras;
                dgvComprasAbertas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                for (int i = 0; i < compras.Count; i++)
                {
                    dgvComprasAbertas.Rows[i].Cells["colCriador"].Value =
                        compras[i].UtilizadorCriou != null ? compras[i].UtilizadorCriou.Nome : "—";
                }

                lblTitulo.Text = "Compras em Aberto (" + compras.Count + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirModoCompra()
        {
            if (dgvComprasAbertas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleciona uma compra para abrir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvComprasAbertas.SelectedRows[0].Cells["Id"].Value;

            // TODO: descomentar quando o FormModoCompra (Pessoa 3 - Rafael) estiver criado
            // FormModoCompra form = new FormModoCompra(id);
            // form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            // form.ShowDialog();

            MessageBox.Show("Modo Compra ainda não implementado (Pessoa 3).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SubForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CarregarComprasAbertas();
        }

        // Menu "Gestão > Utilizadores" — TODO: descomentar quando FormGestaoUtilizadores (Pessoa 1) estiver criado
        private void utilizadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // FormGestaoUtilizadores form = new FormGestaoUtilizadores();
            // form.ShowDialog();
            MessageBox.Show("Gestão de Utilizadores ainda não implementada (Pessoa 1).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Menu "Gestão > Artigos"
        private void artigosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGestaoArtigos form = new FormGestaoArtigos();
            form.ShowDialog();
        }

        // Menu "Gestão > Orçamentos"
        private void orcamentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGestaoOrcamentos form = new FormGestaoOrcamentos();
            form.ShowDialog();
        }

        // Menu "Gestão > Planeamento de Compras"
        private void planeamentoDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormPlaneamentoCompras form = new FormPlaneamentoCompras();
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        // Menu "Estatísticas" — TODO: descomentar quando FormEstatisticas (Pessoa 3) estiver criado
        private void estatisticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // FormEstatisticas form = new FormEstatisticas();
            // form.ShowDialog();
            MessageBox.Show("Estatísticas ainda não implementadas (Pessoa 3).", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Menu "Sair"
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dgvComprasAbertas_DoubleClick(object sender, EventArgs e)
        {
            AbrirModoCompra();
        }

        private void btnAbrirModoCompra_Click(object sender, EventArgs e)
        {
            AbrirModoCompra();
        }
    }
}
