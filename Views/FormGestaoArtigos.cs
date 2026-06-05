// FormGestaoArtigos.cs
// Responsabilidade: interface gráfica para o CRUD de Artigos
// Permite criar, editar e eliminar artigos, com filtro por Tipo de Artigo
// O painel de formulário é escondido em modo leitura e mostrado em modo criação/edição

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
        // Controller que trata toda a lógica de artigos
        private ArtigoController artigoCtrl;

        // Id do artigo selecionado na grelha (0 = modo criação, >0 = modo edição)
        private int idSelecionado;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        public FormGestaoArtigos()
        {
            InitializeComponent();

            artigoCtrl = new ArtigoController();
            idSelecionado = 0;

            // Regista os eventos dos botões e da grelha
            btnNovo.Click += new EventHandler(btnNovo_Click);
            btnEditar.Click += new EventHandler(btnEditar_Click);
            btnEliminar.Click += new EventHandler(btnEliminar_Click);
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);
            btnFiltrar.Click += new EventHandler(btnFiltrar_Click);
            dgvArtigos.SelectionChanged += new EventHandler(dgvArtigos_SelectionChanged);
            dgvArtigos.DoubleClick += new EventHandler(dgvArtigos_DoubleClick);

            // Carrega os tipos de artigo nos dois ComboBoxes (filtro e formulário)
            CarregarTiposArtigo();

            // Carrega os artigos na grelha
            CarregarArtigos();

            // Começa em modo leitura — painel de formulário escondido
            ModoLeitura();
        }

        // ── CARREGAR TIPOS ────────────────────────────────────────────────────

        // Preenche os dois ComboBoxes de tipo diretamente pela BD
        // cmbFiltroTipo — para filtrar a grelha (inclui opção "Todos")
        // cmbTipoArtigo — para selecionar o tipo ao criar/editar um artigo
        private void CarregarTiposArtigo()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    // Carrega todos os tipos ordenados por nome
                    List<TipoArtigo> tipos = db.TiposArtigo.OrderBy(t => t.Nome).ToList();

                    // ComboBox do filtro — começa com a opção "Todos" (Id=0)
                    cmbFiltroTipo.Items.Clear();
                    cmbFiltroTipo.Items.Add(new ComboItem(0, "— Todos —"));
                    foreach (TipoArtigo t in tipos)
                    {
                        cmbFiltroTipo.Items.Add(new ComboItem(t.Id, t.Nome));
                    }
                    cmbFiltroTipo.DisplayMember = "Nome";
                    cmbFiltroTipo.SelectedIndex = 0; // começa em "Todos"

                    // ComboBox do formulário — apenas os tipos reais, sem opção "Todos"
                    cmbTipoArtigo.Items.Clear();
                    foreach (TipoArtigo t in tipos)
                    {
                        cmbTipoArtigo.Items.Add(new ComboItem(t.Id, t.Nome));
                    }
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

        // Vai à BD buscar os artigos (com ou sem filtro de tipo) e preenche a grelha
        private void CarregarArtigos()
        {
            try
            {
                List<Artigo> lista;
                ComboItem filtro = cmbFiltroTipo.SelectedItem as ComboItem;

                // Se o filtro for "Todos" (Id=0) ou null, carrega todos os artigos
                if (filtro == null || filtro.Id == 0)
                    lista = artigoCtrl.GetAll();
                else
                    lista = artigoCtrl.GetByTipo(filtro.Id); // filtra pelo tipo selecionado

                // Limpa a fonte anterior e define as colunas manualmente
                dgvArtigos.DataSource = null;
                dgvArtigos.AutoGenerateColumns = false;
                dgvArtigos.Columns.Clear();

                // Coluna Id — escondida, usada internamente para identificar o registo
                // Name: nome para aceder com Cells["colId"]
                // DataPropertyName: campo do modelo Artigo que preenche esta coluna
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "colId";
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // Coluna Nome — visível, mostra o nome do artigo
                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.Name = "colNome";
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Artigo";
                colNome.FillWeight = 200;

                // Coluna Tipo de Artigo — preenchida manualmente no loop abaixo
                // (não tem DataPropertyName porque é uma propriedade de navegação)
                DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn();
                colTipo.Name = "colTipoArtigo";
                colTipo.HeaderText = "Tipo de Artigo";
                colTipo.FillWeight = 150;

                // Adiciona as colunas e liga a fonte de dados
                dgvArtigos.Columns.Add(colId);
                dgvArtigos.Columns.Add(colNome);
                dgvArtigos.Columns.Add(colTipo);
                dgvArtigos.DataSource = lista;
                dgvArtigos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Preenche a coluna Tipo manualmente para cada linha
                // (o DataBinding não consegue preencher propriedades de navegação aninhadas)
                for (int i = 0; i < lista.Count; i++)
                {
                    dgvArtigos.Rows[i].Cells["colTipoArtigo"].Value =
                        lista[i].Tipo != null ? lista[i].Tipo.Nome : "—";
                }

                // Atualiza os botões Editar/Eliminar conforme a seleção
                AtualizarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar artigos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── MODOS DO FORMULÁRIO ───────────────────────────────────────────────

        // Prepara o form para criar um novo artigo
        private void ModoNovo()
        {
            idSelecionado = 0;   // 0 indica que é uma criação
            txtNome.Clear();
            if (cmbTipoArtigo.Items.Count > 0)
                cmbTipoArtigo.SelectedIndex = 0;
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.DarkRed;
            panelFormulario.Visible = true;
            txtNome.Focus();
        }

        // Prepara o form para editar o artigo selecionado na grelha
        private void ModoEdicao()
        {
            if (dgvArtigos.SelectedRows.Count == 0) return;

            try
            {
                // Lê o Id da linha selecionada através da coluna escondida "colId"
                idSelecionado = (int)dgvArtigos.SelectedRows[0].Cells["colId"].Value;

                // Vai à BD buscar os dados atuais do artigo selecionado
                Artigo artigo = artigoCtrl.GetById(idSelecionado);
                if (artigo == null) return;

                // Preenche o campo de nome com o valor atual
                txtNome.Text = artigo.Nome;

                // Seleciona o tipo correto no ComboBox comparando pelo Id
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

        // Volta ao modo leitura — esconde o painel de formulário
        private void ModoLeitura()
        {
            idSelecionado = 0;
            panelFormulario.Visible = false;
        }

        // ── BOTÕES ────────────────────────────────────────────────────────────

        // Ativa ou desativa os botões Editar e Eliminar
        // consoante existe ou não uma linha selecionada na grelha
        private void AtualizarBotoes()
        {
            bool temSelecao = dgvArtigos.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        // Botão "Filtrar" — recarrega a grelha com o filtro de tipo selecionado
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarArtigos();
        }

        // Botão "Novo" — abre o painel em modo criação
        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        // Botão "Editar" — abre o painel com os dados do artigo selecionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // Botão "Cancelar" — volta ao modo leitura sem guardar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        // Botão "Guardar" — cria ou atualiza o artigo
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Validação: tem de estar um tipo selecionado
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
                // Se idSelecionado == 0 é uma criação, caso contrário é uma atualização
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
                    // Mostra a mensagem de erro devolvida pelo controller (ex: nome duplicado)
                    lblStatus.Text = mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botão "Eliminar" — pede confirmação e elimina o artigo selecionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArtigos.SelectedRows.Count == 0) return;

            // Lê o Id e o nome da linha selecionada para a mensagem de confirmação
            int id = (int)dgvArtigos.SelectedRows[0].Cells["colId"].Value;
            string nomeArtigo = dgvArtigos.SelectedRows[0].Cells["colNome"].Value.ToString();

            // Pede confirmação antes de eliminar
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
                        // Falha normalmente por artigo associado a compras
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

        // Quando a seleção na grelha muda, atualiza o estado dos botões
        private void dgvArtigos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // Duplo clique numa linha da grelha abre o modo de edição
        private void dgvArtigos_DoubleClick(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // Botão "Voltar" — fecha o formulário e volta ao form anterior
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Evento vazio — necessário para o Designer não perder a ligação ao evento
        private void dgvArtigos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        // ── CLASSE AUXILIAR ───────────────────────────────────────────────────

        // ComboItem — classe auxiliar para os ComboBoxes de tipo de artigo
        // Guarda o Id (para operações na BD) e o Nome (para mostrar ao utilizador)
        private class ComboItem
        {
            private int _id;
            private string _nome;

            public int Id { get { return _id; } set { _id = value; } }
            public string Nome { get { return _nome; } set { _nome = value; } }

            public ComboItem(int id, string nome) { _id = id; _nome = nome; }

            // ToString é usado pelo ComboBox para mostrar o texto de cada opção
            public override string ToString() { return _nome; }
        }
    }
}