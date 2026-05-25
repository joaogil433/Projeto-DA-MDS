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

        // Id do tipo selecionado (0 = modo criação)
        private int idSelecionado;
        public FormGestaoTipoArtigo()
        {
            InitializeComponent();
            tipoCtrl = new TipoArtigoController();
            idSelecionado = 0;

            btnNovo.Click += new EventHandler(btnNovo_Click);
            btnEditar.Click += new EventHandler(btnEditar_Click);
            btnEliminar.Click += new EventHandler(btnEliminar_Click);
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);
            dataGridViewTipos.SelectionChanged += new EventHandler(dataGridViewTipos_SelectionChanged);
            dataGridViewTipos.DoubleClick += new EventHandler(dataGridViewTipos_CellDoubleClick);

            CarregarTipos();
            ModoLeitura();

        }

        // Carrega todos os tipos na grelha
        private void CarregarTipos()
        {
            try
            {
                List<TipoArtigo> lista = tipoCtrl.GetAll();

                dataGridViewTipos.DataSource = null;
                dataGridViewTipos.AutoGenerateColumns = false;
                dataGridViewTipos.Columns.Clear();

                // Id escondido — necessário para identificar o registo no CRUD
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // Nome do tipo de artigo
                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Tipo de Artigo";
                colNome.FillWeight = 300;

                dataGridViewTipos.Columns.Add(colId);
                dataGridViewTipos.Columns.Add(colNome);
                dataGridViewTipos.DataSource = lista;
                dataGridViewTipos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                lbContador.Text = lista.Count + " tipo(s)";
                AtualizarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tipos de artigo: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Mostra o painel de formulário em modo criação
        private void ModoNovo()
        {
            idSelecionado = 0;
            txtNome.Clear();
            lbStatus.Text = "";
            panelFormulário.Visible = true;
            txtNome.Focus();
        }

        // Preenche o painel de formulário com os dados do tipo selecionado
        private void ModoEdicao()
        {
            if (dataGridViewTipos.SelectedRows.Count == 0) return;

            try
            {
                idSelecionado = (int)dataGridViewTipos.SelectedRows[0].Cells["Id"].Value;
                TipoArtigo tipo = tipoCtrl.GetById(idSelecionado);
                if (tipo == null) return;

                txtNome.Text = tipo.Nome;
                lbStatus.Text = "";
                panelFormulário.Visible = true;
                txtNome.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir edição: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Esconde o painel de formulário e limpa a seleção
        private void ModoLeitura()
        {
            idSelecionado = 0;
            panelFormulário.Visible = false;
        }

        // Ativa/desativa botões consoante haja ou não seleção na grelha
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
                "Eliminar o tipo de artigo \"" + nomeTipo + "\"?\n\nNão é possível eliminar se existirem artigos associados.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    string mensagem = "";
                    bool sucesso = tipoCtrl.Delete(id, out mensagem);

                    if (sucesso)
                    {
                        ModoLeitura();
                        CarregarTipos();
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensagem = "";
            bool sucesso = false;

            try
            {
                if (idSelecionado == 0)
                {
                    sucesso = tipoCtrl.Add(txtNome.Text, out mensagem);
                }
                else
                {
                    sucesso = tipoCtrl.Update(idSelecionado, txtNome.Text, out mensagem);
                }

                if (sucesso)
                {
                    ModoLeitura();
                    CarregarTipos();
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lbStatus.Text = mensagem;
                    lbStatus.ForeColor = Color.DarkRed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura() ;
        }

        private void dataGridViewTipos_CellDoubleClick(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        private void dataGridViewTipos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }
    }
}
