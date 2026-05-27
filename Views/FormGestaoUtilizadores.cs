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
    public partial class FormGestaoUtilizadores : Form
    {
        private UtilizadorController utilizadorCtrl;
        private int idSelecionado;
        public FormGestaoUtilizadores()
        {
            InitializeComponent();
            utilizadorCtrl = new UtilizadorController();
            idSelecionado = 0;

            CarregarUtilizadores();
            ModoLeitura();
        }

        private void CarregarUtilizadores()
        {
            List<Utilizador> lista = utilizadorCtrl.GetAll();

            dataGridViewUtilizadores.DataSource = null;
            dataGridViewUtilizadores.AutoGenerateColumns = false;
            dataGridViewUtilizadores.Columns.Clear();

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "Id";
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Visible = false;

            DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
            colNome.DataPropertyName = "Nome";
            colNome.HeaderText = "Nome";

            DataGridViewTextBoxColumn colUsername = new DataGridViewTextBoxColumn();
            colUsername.DataPropertyName = "Username";
            colUsername.HeaderText = "Username";

            dataGridViewUtilizadores.Columns.Add(colId);
            dataGridViewUtilizadores.Columns.Add(colNome);
            dataGridViewUtilizadores.Columns.Add(colUsername);
            dataGridViewUtilizadores.DataSource = lista;
            dataGridViewUtilizadores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lbContador.Text = lista.Count + " utilizador(es)";
            AtualizarBotoes();
        }

        private void ModoNovo()
        {
            idSelecionado = 0;
            txtNome.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
            panel1.Visible = true;
            txtNome.Focus();
        }

        private void ModoEdicao()
        {
            if (dataGridViewUtilizadores.SelectedRows.Count == 0) return;

            idSelecionado = (int)dataGridViewUtilizadores.SelectedRows[0].Cells["Id"].Value;
            Utilizador utilizador = utilizadorCtrl.GetById(idSelecionado);
            if (utilizador == null) return;

            txtNome.Text = utilizador.Nome;
            txtUsername.Text = utilizador.Username;
            txtPassword.Clear();
            panel1.Visible = true;
            txtNome.Focus();
        }

        private void ModoLeitura()
        {
            idSelecionado = 0;
            panel1.Visible = false;
        }

        private void AtualizarBotoes()
        {
            bool temSelecao = dataGridViewUtilizadores.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool sucesso;

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
                MessageBox.Show("Não foi possível guardar. Verifique se todos os campos estão preenchidos " +
                    "e se o username não está em uso.",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUtilizadores.SelectedRows.Count == 0) return;

            int id = (int)dataGridViewUtilizadores.SelectedRows[0].Cells["Id"].Value;
            string nomeUtilizador = dataGridViewUtilizadores.SelectedRows[0].Cells[1].Value.ToString();

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
                    MessageBox.Show("Não foi possível eliminar. O utilizador pode ter dados associados " +
                        "ou ser o utilizador com sessão iniciada.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
