// FormGestaoTipoArtigo.cs
// Responsabilidade: interface gráfica para o CRUD de Tipos de Artigo
// Permite criar, editar e eliminar categorias de artigos (ex: Mercearia, Limpeza)

using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_DA_MDS.Views
{
    public partial class FormGestaoTipoArtigo : Form
    {
        // Controller que trata toda a lógica de tipos de artigo
        private TipoArtigoController tipoCtrl;

        // Id do tipo selecionado na grelha (0 = modo criação, >0 = modo edição)
        private int idSelecionado;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        public FormGestaoTipoArtigo()
        {
            InitializeComponent();

            tipoCtrl = new TipoArtigoController();
            idSelecionado = 0;

            // Carrega os tipos existentes na grelha ao abrir o form
            CarregarTipos();

            // Começa em modo leitura — painel de formulário escondido
            ModoLeitura();
        }

        // ── CARREGAR GRELHA ───────────────────────────────────────────────────

        // Vai à BD buscar todos os tipos de artigo e preenche a grelha
        private void CarregarTipos()
        {
            List<TipoArtigo> lista = tipoCtrl.GetAll();

            // Limpa a fonte anterior e define as colunas manualmente
            dataGridViewTipos.DataSource = null;
            dataGridViewTipos.AutoGenerateColumns = false;
            dataGridViewTipos.Columns.Clear();

            // Coluna Id — escondida, usada internamente para identificar o registo
            // Name: nome para aceder com Cells["colId"]
            // DataPropertyName: campo do modelo TipoArtigo que vai preencher esta coluna
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "colId";
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Visible = false;

            // Coluna Nome — visível, mostra o nome do tipo de artigo
            DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
            colNome.Name = "colNome";
            colNome.DataPropertyName = "Nome";
            colNome.HeaderText = "Tipo de Artigo";

            // Adiciona as colunas e liga a fonte de dados
            dataGridViewTipos.Columns.Add(colId);
            dataGridViewTipos.Columns.Add(colNome);
            dataGridViewTipos.DataSource = lista;
            dataGridViewTipos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Atualiza o contador de registos
            lbContador.Text = lista.Count + " tipo(s)";

            // Atualiza os botões Editar/Eliminar conforme a seleção
            AtualizarBotoes();
        }

        // ── MODOS DO FORMULÁRIO ───────────────────────────────────────────────

        // Prepara o form para criar um novo tipo de artigo
        private void ModoNovo()
        {
            idSelecionado = 0;       // 0 indica que é uma criação
            txtNome.Clear();
            panelFormulário.Visible = true;
            txtNome.Focus();
        }

        // Prepara o form para editar o tipo de artigo selecionado na grelha
        private void ModoEdicao()
        {
            if (dataGridViewTipos.SelectedRows.Count == 0) return;

            // Lê o Id da linha selecionada através da coluna escondida "colId"
            idSelecionado = (int)dataGridViewTipos.SelectedRows[0].Cells["colId"].Value;

            // Vai à BD buscar os dados atuais do tipo selecionado
            TipoArtigo tipo = tipoCtrl.GetById(idSelecionado);
            if (tipo == null) return;

            // Preenche o campo de texto com o nome atual
            txtNome.Text = tipo.Nome;
            panelFormulário.Visible = true;
            txtNome.Focus();
        }

        // Volta ao modo leitura — esconde o painel de formulário
        private void ModoLeitura()
        {
            idSelecionado = 0;
            panelFormulário.Visible = false;
        }

        // ── BOTÕES ────────────────────────────────────────────────────────────

        // Ativa ou desativa os botões Editar e Eliminar
        // consoante existe ou não uma linha selecionada na grelha
        private void AtualizarBotoes()
        {
            bool temSelecao = dataGridViewTipos.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        // Botão "Novo" — abre o painel em modo criação
        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        // Botão "Editar" — abre o painel com os dados do tipo selecionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // Botão "Eliminar" — pede confirmação e elimina o tipo selecionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewTipos.SelectedRows.Count == 0) return;

            // Lê o Id e o nome da linha selecionada para a mensagem de confirmação
            int id = (int)dataGridViewTipos.SelectedRows[0].Cells["colId"].Value;
            string nomeTipo = dataGridViewTipos.SelectedRows[0].Cells["colNome"].Value.ToString();

            // Pede confirmação antes de eliminar
            DialogResult confirmacao = MessageBox.Show(
                "Eliminar o tipo de artigo \"" + nomeTipo + "\"?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                bool sucesso = tipoCtrl.Delete(id);

                if (sucesso)
                {
                    ModoLeitura();
                    CarregarTipos();
                    MessageBox.Show("Tipo de artigo eliminado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Falha normalmente por existirem artigos associados a este tipo
                    MessageBox.Show("Não foi possível eliminar. Podem existir artigos associados a este tipo.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Botão "Guardar" — cria ou atualiza o tipo de artigo
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool sucesso;

            // Se idSelecionado == 0 é uma criação, caso contrário é uma atualização
            if (idSelecionado == 0)
                sucesso = tipoCtrl.Add(txtNome.Text);
            else
                sucesso = tipoCtrl.Update(idSelecionado, txtNome.Text);

            if (sucesso)
            {
                ModoLeitura();
                CarregarTipos();
                MessageBox.Show("Guardado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Falha normalmente por nome vazio ou duplicado
                MessageBox.Show("Não foi possível guardar. Verifique se o nome está preenchido " +
                    "e não está duplicado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botão "Cancelar" — volta ao modo leitura sem guardar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        // ── EVENTOS DA GRELHA ─────────────────────────────────────────────────

        // Duplo clique numa linha da grelha abre o modo de edição
        private void dataGridViewTipos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ModoEdicao();
        }

        // Quando a seleção na grelha muda, atualiza o estado dos botões
        private void dataGridViewTipos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }
    }
}