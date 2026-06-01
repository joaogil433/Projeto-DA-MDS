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
        private TipoArtigoController tipoCtrl;
        private int idSelecionado;

        public FormGestaoTipoArtigo()
        {
            InitializeComponent();
            tipoCtrl = new TipoArtigoController();
            idSelecionado = 0;

            CarregarTipos();
            ModoLeitura();
        }

        private void CarregarTipos()
        {
            List<TipoArtigo> lista = tipoCtrl.GetAll();

            dataGridViewTipos.DataSource = null;
            dataGridViewTipos.AutoGenerateColumns = false;
            dataGridViewTipos.Columns.Clear();

            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Visible = false;

            DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
            colNome.DataPropertyName = "Nome";
            colNome.HeaderText = "Tipo de Artigo";

            dataGridViewTipos.Columns.Add(colId);
            dataGridViewTipos.Columns.Add(colNome);
            dataGridViewTipos.DataSource = lista;
            dataGridViewTipos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lbContador.Text = lista.Count + " tipo(s)";
            AtualizarBotoes();
        }

        private void ModoNovo()
        {
            idSelecionado = 0;
            txtNome.Clear();
            panelFormulário.Visible = true;
            txtNome.Focus();
        }

        private void ModoEdicao()
        {
            if (dataGridViewTipos.SelectedRows.Count == 0) return;

            idSelecionado = (int)dataGridViewTipos.SelectedRows[0].Cells["Id"].Value;
            TipoArtigo tipo = tipoCtrl.GetById(idSelecionado);
            if (tipo == null) return;

            txtNome.Text = tipo.Nome;
            panelFormulário.Visible = true;
            txtNome.Focus();
        }

        private void ModoLeitura()
        {
            idSelecionado = 0;
            panelFormulário.Visible = false;
        }

        private void AtualizarBotoes()
        {
            bool temSelecao = dataGridViewTipos.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
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
            if (dataGridViewTipos.SelectedRows.Count == 0) return;

            int id = (int)dataGridViewTipos.SelectedRows[0].Cells["Id"].Value;
            string nomeTipo = dataGridViewTipos.SelectedRows[0].Cells[1].Value.ToString();

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
                    MessageBox.Show("Não foi possível eliminar. Podem existir artigos associados a este tipo.",
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            bool sucesso;

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
                MessageBox.Show("Não foi possível guardar. Verifique se o nome está preenchido " +
                    "e não está duplicado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        private void dataGridViewTipos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ModoEdicao();
        }

        private void dataGridViewTipos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }
    }
}
