using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Projeto_DA_MDS.Views
{
    public partial class FormCriacaoEdicaoCompra : Form
    {
        private ListaCompraController listaCtrl;
        private ArtigoController artigoCtrl;
        private int listaId;
        private bool soLeitura;

        public FormCriacaoEdicaoCompra(int listaId)
        {
            InitializeComponent();
            this.listaId = listaId;
            listaCtrl = new ListaCompraController();
            artigoCtrl = new ArtigoController();
            soLeitura = false;
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnAdicionarItem.Click += new EventHandler(btnAdicionarItem_Click);
            btnRemoverItem.Click += new EventHandler(btnRemoverItem_Click);
            btnFechar.Click += new EventHandler(btnFechar_Click);
            cmbTipoArtigo.SelectedIndexChanged += new EventHandler(cmbTipoArtigo_SelectedIndexChanged);
            CarregarTiposArtigo();
            CarregarDados();
        }

        // Carrega os tipos de artigo diretamente pela BD (sem TipoArtigoController)
        private void CarregarTiposArtigo()
        {
            try
            {
                using (var db = new IshoppingContext())
                {
                    List<TipoArtigo> tipos = db.TiposArtigo.OrderBy(t => t.Nome).ToList();

                    cmbTipoArtigo.Items.Clear();
                    cmbTipoArtigo.Items.Add(new ComboItem(0, "— Seleciona Tipo —"));
                    foreach (TipoArtigo t in tipos)
                    {
                        cmbTipoArtigo.Items.Add(new ComboItem(t.Id, t.Nome));
                    }
                    cmbTipoArtigo.DisplayMember = "Nome";
                    cmbTipoArtigo.SelectedIndex = 0;
                    cmbArtigo.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tipos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarDados()
        {
            if (listaId == 0)
            {
                txtNome.Clear();
                lblEstado.Text = "[ Nova ]";
                lblEstado.ForeColor = Color.SteelBlue;
                var nomeUtilizador = Sessao.UtilizadorAtual?.Nome ?? "—";
                lblInfo.Text = "Criado por: " + (Sessao.UtilizadorAtual?.Nome ?? "—") + "  |  " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                return;
            }

            try
            {
                ListaCompra lista = listaCtrl.GetById(listaId);

                if (lista == null)
                {
                    MessageBox.Show("Lista não encontrada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                txtNome.Text = lista.Nome;
                lblEstado.Text = lista.Estado == "Fechada" ? "FECHADA" : "ABERTA";
                lblEstado.ForeColor = lista.Estado == "Fechada" ? Color.Gray : Color.DarkGreen;

                string nomeCriador = lista.UtilizadorCriou != null ? lista.UtilizadorCriou.Nome : "—";
                string nomeAlterador = lista.UtilizadorAlterou != null ? lista.UtilizadorAlterou.Nome : "—";
                string dataAlt = lista.DataAlteracao.HasValue ? lista.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm") : "—";
                lblInfo.Text = "Criado: " + nomeCriador + " em " + lista.DataCriacao.ToString("dd/MM/yyyy HH:mm") +
                               "  |  Alterado: " + nomeAlterador + " em " + dataAlt;

                soLeitura = lista.Estado == "Fechada";
                txtNome.ReadOnly = soLeitura;
                btnGuardar.Enabled = !soLeitura;
                btnAdicionarItem.Enabled = !soLeitura;
                btnRemoverItem.Enabled = !soLeitura;
                panelAddItem.Enabled = !soLeitura;

                CarregarItens();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarItens()
        {
            if (listaId == 0) return;

            try
            {
                List<ItemPrevisto> itens = listaCtrl.GetItensPrevistos(listaId);

                dgvItens.DataSource = null;
                dgvItens.AutoGenerateColumns = false;
                dgvItens.Columns.Clear();

                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn();
                colTipo.Name = "colTipoArtigo";
                colTipo.HeaderText = "Tipo de Artigo";
                colTipo.FillWeight = 160;

                DataGridViewTextBoxColumn colArtigo = new DataGridViewTextBoxColumn();
                colArtigo.Name = "colArtigo";
                colArtigo.HeaderText = "Artigo";
                colArtigo.FillWeight = 200;

                DataGridViewTextBoxColumn colQtdPrevista = new DataGridViewTextBoxColumn();
                colQtdPrevista.DataPropertyName = "QuantidadePrevista";
                colQtdPrevista.HeaderText = "Qtd Prevista";
                colQtdPrevista.FillWeight = 100;

                DataGridViewTextBoxColumn colQtdAdq = new DataGridViewTextBoxColumn();
                colQtdAdq.DataPropertyName = "QuantidadeAdquirida";
                colQtdAdq.HeaderText = "Qtd Adquirida";
                colQtdAdq.FillWeight = 100;

                dgvItens.Columns.Add(colId);
                dgvItens.Columns.Add(colTipo);
                dgvItens.Columns.Add(colArtigo);
                dgvItens.Columns.Add(colQtdPrevista);
                dgvItens.Columns.Add(colQtdAdq);
                dgvItens.DataSource = itens;
                dgvItens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                for (int i = 0; i < itens.Count; i++)
                {
                    if (itens[i].Artigo != null)
                    {
                        dgvItens.Rows[i].Cells["colArtigo"].Value = itens[i].Artigo.Nome;
                        dgvItens.Rows[i].Cells["colTipoArtigo"].Value = itens[i].Artigo.Tipo != null ? itens[i].Artigo.Tipo.Nome : "—";
                    }
                    else
                    {
                        dgvItens.Rows[i].Cells["colArtigo"].Value = "—";
                        dgvItens.Rows[i].Cells["colTipoArtigo"].Value = "—";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar itens: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbTipoArtigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem tipo = cmbTipoArtigo.SelectedItem as ComboItem;
            cmbArtigo.Items.Clear();
            if (tipo == null || tipo.Id == 0) return;

            try
            {
                List<Artigo> artigos = artigoCtrl.GetByTipo(tipo.Id);
                foreach (Artigo a in artigos)
                {
                    cmbArtigo.Items.Add(new ComboItem(a.Id, a.Nome));
                }
                cmbArtigo.DisplayMember = "Nome";
                if (cmbArtigo.Items.Count > 0) cmbArtigo.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao filtrar artigos: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Sessao.UtilizadorAtual == null)
            {
                MessageBox.Show("Utilizador não autenticado. Por favor, faça login.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int userId = Sessao.UtilizadorAtual.Id;
            string mensagem = "";
            bool sucesso = false;

            try
            {
                if (listaId == 0)
                {
                    sucesso = listaCtrl.Add(txtNome.Text, userId, out mensagem);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    sucesso = listaCtrl.Update(listaId, txtNome.Text, userId, out mensagem);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            if (listaId == 0)
            {
                lblAddInfo.Text = "Guarda primeiro a compra antes de adicionar itens.";
                return;
            }

            ComboItem artigo = cmbArtigo.SelectedItem as ComboItem;
            if (artigo == null || artigo.Id == 0)
            {
                lblAddInfo.Text = "Seleciona um Tipo e um Artigo.";
                return;
            }

            try
            {
                string mensagem = "";
                bool sucesso = listaCtrl.AddItemPrevisto(listaId, artigo.Id, (int)nudQuantidade.Value, out mensagem);

                if (sucesso)
                {
                    lblAddInfo.Text = "";
                    CarregarItens();
                }
                else
                {
                    lblAddInfo.Text = mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar item: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoverItem_Click(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count == 0) return;

            int id = (int)dgvItens.SelectedRows[0].Cells[0].Value;
            string nomeArtigo = dgvItens.SelectedRows[0].Cells["colArtigo"].Value.ToString();

            DialogResult conf = MessageBox.Show(
                "Remover o item \"" + nomeArtigo + "\" da lista?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (conf == DialogResult.Yes)
            {
                try
                {
                    string mensagem = "";
                    bool sucesso = listaCtrl.RemoveItemPrevisto(id, out mensagem);
                    if (sucesso)
                    {
                        CarregarItens();
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

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void btnRemoverItem_Click_1(object sender, EventArgs e)
        {

        }

        private void dgvItens_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
