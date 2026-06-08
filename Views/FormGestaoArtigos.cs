// FormGestaoArtigos.cs
// Responsabilidade: interface gráfica para o CRUD de Artigos
// Permite criar, editar e eliminar artigos, com filtro por Tipo de Artigo

using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Projeto_DA_MDS.Views
{
    public partial class FormGestaoArtigos : Form
    {
        // controller que trata toda a lógica de artigos
        private ArtigoController artigoCtrl;

        // Id do artigo selecionado (0 = modo criação, >0 = modo edição)
        private int idSelecionado;

        // guarda a lista atual para o DataBindingComplete poder aceder
        private List<Artigo> _listaAtual;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        public FormGestaoArtigos()
        {
            InitializeComponent();
            artigoCtrl = new ArtigoController();
            idSelecionado = 0;

            btnNovo.Click += new EventHandler(btnNovo_Click);
            btnEditar.Click += new EventHandler(btnEditar_Click);
            btnEliminar.Click += new EventHandler(btnEliminar_Click);
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);
            btnFiltrar.Click += new EventHandler(btnFiltrar_Click);
            dgvArtigos.SelectionChanged += new EventHandler(dgvArtigos_SelectionChanged);
            dgvArtigos.DoubleClick += new EventHandler(dgvArtigos_DoubleClick);

            // liga o DataBindingComplete — dispara quando o WinForms termina de criar
            // as linhas da grelha, garantindo que as células já existem antes de
            // tentarmos preencher colTipoArtigo manualmente
            dgvArtigos.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgvArtigos_DataBindingComplete);

            CarregarTiposArtigo();
            CarregarArtigos();
            ModoLeitura();
        }

        // ── CARREGAR TIPOS ────────────────────────────────────────────────────

        // preenche os dois ComboBoxes de tipo diretamente pela BD
        private void CarregarTiposArtigo()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    List<TipoArtigo> tipos = db.TiposArtigo.OrderBy(t => t.Nome).ToList();

                    // ComboBox do filtro — inclui opção "Todos" (Id=0)
                    cmbFiltroTipo.Items.Clear();
                    cmbFiltroTipo.Items.Add(new ComboItem(0, "— Todos —"));
                    foreach (TipoArtigo t in tipos)
                        cmbFiltroTipo.Items.Add(new ComboItem(t.Id, t.Nome));
                    cmbFiltroTipo.DisplayMember = "Nome";
                    cmbFiltroTipo.SelectedIndex = 0;

                    // ComboBox do formulário — apenas os tipos reais
                    cmbTipoArtigo.Items.Clear();
                    foreach (TipoArtigo t in tipos)
                        cmbTipoArtigo.Items.Add(new ComboItem(t.Id, t.Nome));
                    cmbTipoArtigo.DisplayMember = "Nome";
                    if (cmbTipoArtigo.Items.Count > 0)
                        cmbTipoArtigo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tipos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── CARREGAR GRELHA ───────────────────────────────────────────────────

        // vai à BD buscar os artigos (com ou sem filtro) e preenche a grelha
        // o preenchimento de colTipoArtigo é feito no DataBindingComplete
        private void CarregarArtigos()
        {
            try
            {
                ComboItem filtro = cmbFiltroTipo.SelectedItem as ComboItem;

                // se filtro for "Todos" (Id=0) carrega todos, senão filtra pelo tipo
                if (filtro == null || filtro.Id == 0)
                    _listaAtual = artigoCtrl.GetAll();
                else
                    _listaAtual = artigoCtrl.GetByTipo(filtro.Id);

                dgvArtigos.DataSource = null;
                dgvArtigos.AutoGenerateColumns = false;
                dgvArtigos.Columns.Clear();

                // coluna Id — escondida, usada para identificar o registo
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "colId";
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // coluna Nome — nome do artigo
                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.Name = "colNome";
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Artigo";
                colNome.FillWeight = 200;

                // coluna Tipo — preenchida manualmente no DataBindingComplete
                // porque é uma propriedade de navegação (Artigo.Tipo.Nome)
                DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn();
                colTipo.Name = "colTipoArtigo";
                colTipo.HeaderText = "Tipo de Artigo";
                colTipo.FillWeight = 150;

                dgvArtigos.Columns.Add(colId);
                dgvArtigos.Columns.Add(colNome);
                dgvArtigos.Columns.Add(colTipo);

                // atribui os dados — o preenchimento de colTipoArtigo é feito
                // no DataBindingComplete para garantir que as linhas já existem
                dgvArtigos.DataSource = _listaAtual;
                dgvArtigos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                AtualizarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar artigos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── DATA BINDING COMPLETE ─────────────────────────────────────────────

        // dispara quando o WinForms termina de criar todas as linhas da grelha
        // é aqui que preenchemos colTipoArtigo com o nome do tipo
        private void dgvArtigos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (_listaAtual == null) return;

            for (int i = 0; i < _listaAtual.Count; i++)
            {
                dgvArtigos.Rows[i].Cells["colTipoArtigo"].Value =
                    _listaAtual[i].Tipo != null ? _listaAtual[i].Tipo.Nome : "—";
            }
        }

        // ── MODOS DO FORMULÁRIO ───────────────────────────────────────────────

        // prepara o form para criar um novo artigo
        private void ModoNovo()
        {
            idSelecionado = 0;
            txtNome.Clear();
            if (cmbTipoArtigo.Items.Count > 0)
                cmbTipoArtigo.SelectedIndex = 0;
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.DarkRed;
            panelFormulario.Visible = true;
            txtNome.Focus();
        }

        // prepara o form para editar o artigo selecionado na grelha
        private void ModoEdicao()
        {
            if (dgvArtigos.SelectedRows.Count == 0) return;

            try
            {
                // lê o Id da coluna escondida "colId"
                idSelecionado = (int)dgvArtigos.SelectedRows[0].Cells["colId"].Value;

                Artigo artigo = artigoCtrl.GetById(idSelecionado);
                if (artigo == null) return;

                txtNome.Text = artigo.Nome;

                // seleciona o tipo correto no ComboBox pelo Id
                foreach (ComboItem item in cmbTipoArtigo.Items)
                {
                    if (item.Id == artigo.TipoArtigoId)
                    {
                        cmbTipoArtigo.SelectedItem = item;
                        break;
                    }
                }

                lblStatus.Text = "";
                panelFormulario.Visible = true;
                txtNome.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir edição: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // volta ao modo leitura — esconde o painel de formulário
        private void ModoLeitura()
        {
            idSelecionado = 0;
            panelFormulario.Visible = false;
        }

        // ── BOTÕES ────────────────────────────────────────────────────────────

        // ativa/desativa os botões Editar e Eliminar conforme a seleção
        private void AtualizarBotoes()
        {
            bool temSelecao = dgvArtigos.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        // botão "Filtrar" — recarrega a grelha com o filtro atual
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarArtigos();
        }

        // botão "Novo" — abre o painel em modo criação
        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        // botão "Editar" — abre o painel com os dados do artigo selecionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // botão "Cancelar" — volta ao modo leitura sem guardar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        // botão "Guardar" — cria ou atualiza o artigo
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbTipoArtigo.SelectedItem == null)
            {
                lblStatus.Text = "Seleciona um Tipo de Artigo.";
                return;
            }

            ComboItem tipoSelecionado = (ComboItem)cmbTipoArtigo.SelectedItem;
            string mensagem = "";
            bool sucesso = false;

            try
            {
                // idSelecionado == 0 → criação; >0 → atualização
                if (idSelecionado == 0)
                    sucesso = artigoCtrl.Add(txtNome.Text, tipoSelecionado.Id, out mensagem);
                else
                    sucesso = artigoCtrl.Update(idSelecionado, txtNome.Text, tipoSelecionado.Id, out mensagem);

                if (sucesso)
                {
                    ModoLeitura();
                    CarregarArtigos();
                    MessageBox.Show(mensagem, "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // falha por nome vazio ou duplicado — mostra a mensagem do controller
                    lblStatus.Text = mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // botão "Eliminar" — pede confirmação e elimina o artigo selecionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArtigos.SelectedRows.Count == 0) return;

            int id = (int)dgvArtigos.SelectedRows[0].Cells["colId"].Value;
            string nomeArtigo = dgvArtigos.SelectedRows[0].Cells["colNome"].Value.ToString();

            DialogResult confirmacao = MessageBox.Show(
                "Eliminar o artigo \"" + nomeArtigo + "\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    string mensagem = "";
                    bool sucesso = artigoCtrl.Delete(id, out mensagem);

                    if (sucesso)
                    {
                        ModoLeitura();
                        CarregarArtigos();
                        MessageBox.Show(mensagem, "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // falha por artigo associado a compras
                        MessageBox.Show(mensagem, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado: " + ex.Message,
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ── EVENTOS DA GRELHA ─────────────────────────────────────────────────

        // seleção mudou — atualiza botões
        private void dgvArtigos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // duplo clique — abre modo de edição
        private void dgvArtigos_DoubleClick(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // botão "Voltar" — fecha o form
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // evento vazio — mantido para o Designer não perder a ligação
        private void dgvArtigos_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // ── CLASSE AUXILIAR ───────────────────────────────────────────────────

        // ComboItem — guarda Id e Nome para os ComboBoxes de tipo
        private class ComboItem
        {
            private int _id;
            private string _nome;

            public int Id { get { return _id; } set { _id = value; } }
            public string Nome { get { return _nome; } set { _nome = value; } }

            public ComboItem(int id, string nome) { _id = id; _nome = nome; }

            // ToString usado pelo ComboBox para mostrar o texto de cada opção
            public override string ToString() { return _nome; }
        }
    }
}