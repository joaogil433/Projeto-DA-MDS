// FormGestaoUtilizadores.cs
// Responsabilidade: interface gráfica para o CRUD de Utilizadores (US8)
// Permite criar, editar e eliminar utilizadores do sistema

using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Projeto_DA_MDS.Views
{
    public partial class FormGestaoUtilizadores : Form
    {
        // controller que trata toda a lógica de utilizadores
        private UtilizadorController utilizadorCtrl;

        // Id do utilizador selecionado (0 = modo criação, >0 = modo edição)
        private int idSelecionado;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        public FormGestaoUtilizadores()
        {
            InitializeComponent();
            utilizadorCtrl = new UtilizadorController();
            idSelecionado = 0;

            CarregarUtilizadores();
            ModoLeitura();
        }

        // ── CARREGAR GRELHA ───────────────────────────────────────────────────

        // vai à BD buscar todos os utilizadores e preenche a grelha
        // usa um objeto anónimo para resolver os nomes de CriadoPor e AlteradoPor
        // a partir da própria lista — evita propriedades de navegação
        private void CarregarUtilizadores()
        {
            List<Utilizador> todos = utilizadorCtrl.GetAll();

            // cria objetos anónimos com os campos a mostrar na grelha
            // resolve o nome do criador e do alterador a partir da mesma lista
            var dadosGrid = todos.Select(u => new
            {
                Id = u.Id,
                Nome = u.Nome,
                Username = u.Username,
                CriadoPor = u.CriadoPorId.HasValue
                    ? (todos.FirstOrDefault(x => x.Id == u.CriadoPorId.Value)?.Nome ?? "(desconhecido)")
                    : "(sistema)",
                AlteradoPor = u.AlteradoPorId.HasValue
                    ? (todos.FirstOrDefault(x => x.Id == u.AlteradoPorId.Value)?.Nome ?? "(desconhecido)")
                    : "—"
            }).ToList();

            dataGridViewUtilizadores.DataSource = null;
            dataGridViewUtilizadores.AutoGenerateColumns = false;
            dataGridViewUtilizadores.Columns.Clear();

            // coluna Id — escondida, usada para identificar o registo selecionado
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "colId";               // Name para aceder com Cells["colId"]
            colId.DataPropertyName = "Id";      // mapeia ao campo Id do objeto anónimo
            colId.HeaderText = "ID";
            colId.Visible = false;

            // coluna Nome
            DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
            colNome.Name = "colNome";            // Name para aceder com Cells["colNome"]
            colNome.DataPropertyName = "Nome";
            colNome.HeaderText = "Nome";

            // coluna Username
            DataGridViewTextBoxColumn colUsername = new DataGridViewTextBoxColumn();
            colUsername.DataPropertyName = "Username";
            colUsername.HeaderText = "Username";

            // coluna Criado Por — preenchida pelo DataBinding via objeto anónimo
            DataGridViewTextBoxColumn colCriadoPor = new DataGridViewTextBoxColumn();
            colCriadoPor.DataPropertyName = "CriadoPor";
            colCriadoPor.HeaderText = "Criado por";

            // coluna Alterado Por — preenchida pelo DataBinding via objeto anónimo
            DataGridViewTextBoxColumn colAlteradoPor = new DataGridViewTextBoxColumn();
            colAlteradoPor.DataPropertyName = "AlteradoPor";
            colAlteradoPor.HeaderText = "Alterado por";

            dataGridViewUtilizadores.Columns.Add(colId);
            dataGridViewUtilizadores.Columns.Add(colNome);
            dataGridViewUtilizadores.Columns.Add(colUsername);
            dataGridViewUtilizadores.Columns.Add(colCriadoPor);
            dataGridViewUtilizadores.Columns.Add(colAlteradoPor);
            dataGridViewUtilizadores.DataSource = dadosGrid;
            dataGridViewUtilizadores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lbContador.Text = todos.Count + " utilizador(es)";
            AtualizarBotoes();
        }

        // ── MODOS DO FORMULÁRIO ───────────────────────────────────────────────

        // prepara o form para criar um novo utilizador
        private void ModoNovo()
        {
            idSelecionado = 0;
            txtNome.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            panel1.Visible = true;
            txtNome.Focus();
        }

        // prepara o form para editar o utilizador selecionado na grelha
        private void ModoEdicao()
        {
            if (dataGridViewUtilizadores.SelectedRows.Count == 0) return;

            // lê o Id da coluna escondida "colId"
            idSelecionado = (int)dataGridViewUtilizadores.SelectedRows[0].Cells["colId"].Value;

            // vai à BD buscar os dados atuais do utilizador
            Utilizador utilizador = utilizadorCtrl.GetById(idSelecionado);
            if (utilizador == null) return;

            txtNome.Text = utilizador.Nome;
            txtUsername.Text = utilizador.Username;
            txtPassword.Clear(); // password não é mostrada — só preenchida se quiser alterar
            panel1.Visible = true;
            txtNome.Focus();
        }

        // volta ao modo leitura — esconde o painel de formulário
        private void ModoLeitura()
        {
            idSelecionado = 0;
            panel1.Visible = false;
        }

        // ativa/desativa os botões Editar e Eliminar conforme a seleção
        private void AtualizarBotoes()
        {
            bool temSelecao = dataGridViewUtilizadores.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        // ── EVENTOS ───────────────────────────────────────────────────────────

        // seleção na grelha mudou — atualiza botões
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // botão "Cancelar" — volta ao modo leitura sem guardar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        // botão "Guardar" — cria ou atualiza o utilizador
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool sucesso;

            // idSelecionado == 0 → criação; >0 → atualização
            if (idSelecionado == 0)
                sucesso = utilizadorCtrl.Add(txtNome.Text, txtUsername.Text, txtPassword.Text);
            else
                sucesso = utilizadorCtrl.Update(idSelecionado, txtNome.Text, txtUsername.Text, txtPassword.Text);

            if (sucesso)
            {
                ModoLeitura();
                CarregarUtilizadores();
                MessageBox.Show("Guardado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // falha por campos vazios ou username duplicado
                MessageBox.Show("Não foi possível guardar. Verifique se todos os campos estão preenchidos " +
                    "e se o username não está em uso.",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // botão "Novo" — abre o painel em modo criação
        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        // botão "Editar" — abre o painel com os dados do utilizador selecionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // botão "Eliminar" — pede confirmação e elimina o utilizador selecionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUtilizadores.SelectedRows.Count == 0) return;

            // lê o Id e o nome usando os Names definidos nas colunas
            int id = (int)dataGridViewUtilizadores.SelectedRows[0].Cells["colId"].Value;
            string nomeUtilizador = dataGridViewUtilizadores.SelectedRows[0].Cells["colNome"].Value.ToString();

            DialogResult confirmacao = MessageBox.Show(
                "Eliminar o utilizador \"" + nomeUtilizador + "\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                bool sucesso = utilizadorCtrl.Delete(id);

                if (sucesso)
                {
                    ModoLeitura();
                    CarregarUtilizadores();
                    MessageBox.Show("Utilizador eliminado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // falha por dados associados ou por ser o utilizador com sessão iniciada
                    MessageBox.Show("Não foi possível eliminar. O utilizador pode ter dados associados " +
                        "ou ser o utilizador com sessão iniciada.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // eventos vazios — mantidos para o Designer não perder a ligação
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtNome_TextChanged(object sender, EventArgs e) { }
    }
}