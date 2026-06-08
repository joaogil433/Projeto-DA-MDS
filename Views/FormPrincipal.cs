// FormPrincipal.cs
// Responsabilidade: ecrã principal da aplicação
// Mostra as compras em aberto e dá acesso a todas as funcionalidades via menu

using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Projeto_DA_MDS.Views
{
    public partial class FormPrincipal : Form
    {
        // controller para operações de listas de compras
        private ListaCompraController listaCtrl;

        // guarda as compras abertas para o DataBindingComplete poder aceder
        private List<ListaCompra> _comprasAbertas;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        public FormPrincipal()
        {
            InitializeComponent();
            listaCtrl = new ListaCompraController();

            // mostra o nome do utilizador com sessão iniciada
            lblUtilizador.Text = "Utilizador: " + (Sessao.UtilizadorAtual?.Nome ?? "—");

            // liga o duplo clique na grelha ao método AbrirModoCompra
            dgvComprasAbertas.DoubleClick += new EventHandler(dgvComprasAbertas_DoubleClick);

            // liga o DataBindingComplete — dispara quando o WinForms termina de criar
            // as linhas da grelha, garantindo que as células já existem antes de
            // tentarmos preencher colCriador manualmente
            dgvComprasAbertas.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgvComprasAbertas_DataBindingComplete);

            // carrega as compras abertas quando o form abre
            this.Load += (s, e) => CarregarComprasAbertas();
        }

        // ── CARREGAR GRELHA ───────────────────────────────────────────────────

        // vai à BD buscar as compras abertas e preenche a grelha
        private void CarregarComprasAbertas()
        {
            try
            {
                dgvComprasAbertas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvComprasAbertas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                // guarda na variável de instância para o DataBindingComplete poder aceder
                _comprasAbertas = listaCtrl.GetAbertas();

                dgvComprasAbertas.DataSource = null;
                dgvComprasAbertas.AutoGenerateColumns = false;
                dgvComprasAbertas.Columns.Clear();

                // coluna Id — escondida, usada para identificar a linha selecionada
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "Id";
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // coluna Nome — nome da compra
                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.Name = "colNome";
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Nome da Compra";
                colNome.FillWeight = 300;

                // coluna Data Criação — preenchida automaticamente pelo DataBinding
                DataGridViewTextBoxColumn colData = new DataGridViewTextBoxColumn();
                colData.DataPropertyName = "DataCriacao";
                colData.HeaderText = "Data Criação";
                colData.FillWeight = 160;
                colData.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // coluna Criado Por — preenchida manualmente no DataBindingComplete
                // porque é uma propriedade de navegação (UtilizadorCriou.Nome)
                DataGridViewTextBoxColumn colCriador = new DataGridViewTextBoxColumn();
                colCriador.Name = "colCriador";
                colCriador.HeaderText = "Criado Por";
                colCriador.FillWeight = 180;

                dgvComprasAbertas.Columns.Add(colId);
                dgvComprasAbertas.Columns.Add(colNome);
                dgvComprasAbertas.Columns.Add(colData);
                dgvComprasAbertas.Columns.Add(colCriador);

                // atribui os dados — o preenchimento de colCriador é feito no
                // DataBindingComplete para garantir que as linhas já existem
                dgvComprasAbertas.DataSource = _comprasAbertas;
                dgvComprasAbertas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lblTitulo.Text = "Compras em Aberto (" + _comprasAbertas.Count + ")";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── DATA BINDING COMPLETE ─────────────────────────────────────────────

        // dispara quando o WinForms termina de criar todas as linhas da grelha
        // é aqui que preenchemos colCriador com o nome do utilizador
        private void dgvComprasAbertas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (_comprasAbertas == null) return;

            for (int i = 0; i < _comprasAbertas.Count; i++)
            {
                dgvComprasAbertas.Rows[i].Cells["colCriador"].Value =
                    _comprasAbertas[i].UtilizadorCriou != null
                    ? _comprasAbertas[i].UtilizadorCriou.Nome
                    : "—";
            }
        }

        // ── MODO COMPRA ───────────────────────────────────────────────────────

        // abre o FormModoCompra para a compra selecionada na grelha
        private void AbrirModoCompra()
        {
            if (dgvComprasAbertas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleciona uma compra para abrir.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvComprasAbertas.SelectedRows[0].Cells["Id"].Value;

            // vai à BD buscar a lista completa com todos os itens e artigos
            ListaCompra lista = listaCtrl.GetById(id);
            FormModoCompra form = new FormModoCompra(lista);
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        // quando um sub-form fecha, recarrega as compras abertas
        private void SubForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CarregarComprasAbertas();
        }

        // ── MENU ──────────────────────────────────────────────────────────────

        // Menu "Gestão > Utilizadores"
        private void utilizadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGestaoUtilizadores form = new FormGestaoUtilizadores();
            form.ShowDialog();
        }

        // Menu "Gestão > Tipo de Artigo"
        private void tipoDeArtigoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGestaoTipoArtigo form = new FormGestaoTipoArtigo();
            form.ShowDialog();
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

        // Menu "Estatísticas"
        private void estatisticasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormEstatisticas form = new FormEstatisticas();
            form.ShowDialog();
        }

        // Menu "Sair"
        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // ── EVENTOS DA GRELHA ─────────────────────────────────────────────────

        // duplo clique na grelha — abre o modo compra
        private void dgvComprasAbertas_DoubleClick(object sender, EventArgs e)
        {
            AbrirModoCompra();
        }

        // botão "Abrir Modo Compra"
        private void btnAbrirModoCompra_Click(object sender, EventArgs e)
        {
            AbrirModoCompra();
        }

        // eventos vazios — mantidos para o Designer não perder a ligação
        private void toolStripMenuItem2_Click(object sender, EventArgs e) { }
        private void dgvComprasAbertas_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}