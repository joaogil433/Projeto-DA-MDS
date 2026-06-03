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
        private ArtigoController artigoCtrl;
        private int idSelecionado;

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
            CarregarTiposArtigo();
            CarregarArtigos();
            ModoLeitura();
        }

        // Preenche os dois ComboBoxes de tipo diretamente pela BD (sem TipoArtigoController)
        private void CarregarTiposArtigo()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    List<TipoArtigo> tipos = db.TiposArtigo.OrderBy(t => t.Nome).ToList();

                    cmbFiltroTipo.Items.Clear();
                    cmbFiltroTipo.Items.Add(new ComboItem(0, "— Todos —"));
                    foreach (TipoArtigo t in tipos)
                    {
                        cmbFiltroTipo.Items.Add(new ComboItem(t.Id, t.Nome));
                    }
                    cmbFiltroTipo.DisplayMember = "Nome";
                    cmbFiltroTipo.SelectedIndex = 0;

                    cmbTipoArtigo.Items.Clear();
                    foreach (TipoArtigo t in tipos)
                    {
                        cmbTipoArtigo.Items.Add(new ComboItem(t.Id, t.Nome));
                    }
                    cmbTipoArtigo.DisplayMember = "Nome";
                    if (cmbTipoArtigo.Items.Count > 0)
                    {
                        cmbTipoArtigo.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tipos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarArtigos()
        {
            try
            {
                List<Artigo> lista;
                ComboItem filtro = cmbFiltroTipo.SelectedItem as ComboItem;

                if (filtro == null || filtro.Id == 0)
                {
                    lista = artigoCtrl.GetAll();
                }
                else
                {
                    lista = artigoCtrl.GetByTipo(filtro.Id);
                }

                dgvArtigos.DataSource = null;
                dgvArtigos.AutoGenerateColumns = false;
                dgvArtigos.Columns.Clear();

                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Artigo";
                colNome.FillWeight = 200;

                DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn();
                colTipo.Name = "colTipoArtigo";
                colTipo.HeaderText = "Tipo de Artigo";
                colTipo.FillWeight = 150;

                dgvArtigos.Columns.Add(colId);
                dgvArtigos.Columns.Add(colNome);
                dgvArtigos.Columns.Add(colTipo);
                dgvArtigos.DataSource = lista;
                dgvArtigos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i].Tipo != null)
                    {
                        dgvArtigos.Rows[i].Cells["colTipoArtigo"].Value = lista[i].Tipo.Nome;
                    }
                    else
                    {
                        dgvArtigos.Rows[i].Cells["colTipoArtigo"].Value = "—";
                    }
                }

                AtualizarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar artigos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModoNovo()
        {
            idSelecionado = 0;
            txtNome.Clear();
            if (cmbTipoArtigo.Items.Count > 0) cmbTipoArtigo.SelectedIndex = 0;
            lblStatus.Text = "";
            lblStatus.ForeColor = Color.DarkRed;
            panelFormulario.Visible = true;
            txtNome.Focus();
        }

        private void ModoEdicao()
        {
            if (dgvArtigos.SelectedRows.Count == 0) return;

            try
            {
                idSelecionado = (int)dgvArtigos.SelectedRows[0].Cells["Id"].Value;
                Artigo artigo = artigoCtrl.GetById(idSelecionado);
                if (artigo == null) return;

                txtNome.Text = artigo.Nome;
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
                MessageBox.Show("Erro ao abrir edição: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModoLeitura()
        {
            idSelecionado = 0;
            panelFormulario.Visible = false;
        }

        private void AtualizarBotoes()
        {
            bool temSelecao = dgvArtigos.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarArtigos();
        }

        private void dgvArtigos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        private void dgvArtigos_DoubleClick(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

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
                if (idSelecionado == 0)
                {
                    sucesso = artigoCtrl.Add(txtNome.Text, tipoSelecionado.Id, out mensagem);
                }
                else
                {
                    sucesso = artigoCtrl.Update(idSelecionado, txtNome.Text, tipoSelecionado.Id, out mensagem);
                }

                if (sucesso)
                {
                    ModoLeitura();
                    CarregarArtigos();
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArtigos.SelectedRows.Count == 0) return;

            int id = (int)dgvArtigos.SelectedRows[0].Cells["Id"].Value;
            string nomeArtigo = dgvArtigos.SelectedRows[0].Cells["Nome"].Value.ToString();

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

        private class ComboItem
        {
            private int _id;
            private string _nome;

            public int Id { get { return _id; } set { _id = value; } }
            public string Nome { get { return _nome; } set { _nome = value; } }

            public ComboItem(int id, string nome) { _id = id; _nome = nome; }

            public override string ToString() { return _nome; }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvArtigos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
