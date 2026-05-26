// Responsabilidade: listar todas as compras com filtro por estado e aceder à criação/edição
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS.Views
{
    public partial class FormPlaneamentoCompras : Form
    {
        // Controller que trata a lógica das listas de compras
        private ListaCompraController listaCtrl;

        public FormPlaneamentoCompras()
        {
            InitializeComponent();

            listaCtrl = new ListaCompraController();

            // Preenche o ComboBox de filtro com as opções de estado
            cmbFiltroEstado.Items.Add("Todos");   // Mostra todas as listas
            cmbFiltroEstado.Items.Add("Aberta");  // Só as listas em aberto
            cmbFiltroEstado.Items.Add("Fechada"); // Só as listas fechadas

            // Seleciona "Todos" por omissão
            cmbFiltroEstado.SelectedIndex = 0;

            // Carrega todas as compras na grelha
            CarregarCompras();
        }

        // Carrega as compras na grelha aplicando o filtro selecionado
        private void CarregarCompras()
        {
            try
            {
                // Lê o estado selecionado no filtro
                string filtro = cmbFiltroEstado.SelectedItem.ToString();
                List<ListaCompra> lista;

                // Decide qual método do controller usar conforme o filtro
                if (filtro == "Todos")
                {
                    lista = listaCtrl.GetAll();
                }
                else
                {
                    // Passa "Aberta" ou "Fechada" ao controller
                    lista = listaCtrl.GetByEstado(filtro);
                }

                // Limpa e define as colunas manualmente
                dgvCompras.DataSource = null;
                dgvCompras.AutoGenerateColumns = false;
                dgvCompras.Columns.Clear();

                // Id escondido — necessário para identificar a linha no CRUD
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "Id";
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // Nome da compra
                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.Name = "Nome";
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Nome da Compra";
                colNome.FillWeight = 200;

                // Estado ("Aberta" ou "Fechada")
                DataGridViewTextBoxColumn colEstado = new DataGridViewTextBoxColumn();
                colEstado.Name = "Estado";
                colEstado.DataPropertyName = "Estado";
                colEstado.HeaderText = "Estado";
                colEstado.FillWeight = 80;

                // Data de criação com formato legível
                DataGridViewTextBoxColumn colCriacao = new DataGridViewTextBoxColumn();
                colCriacao.DataPropertyName = "DataCriacao";
                colCriacao.HeaderText = "Criado Em";
                colCriacao.FillWeight = 130;
                colCriacao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // Criador — preenchido manualmente a partir de UtilizadorCriou.Nome
                DataGridViewTextBoxColumn colCriador = new DataGridViewTextBoxColumn();
                colCriador.Name = "colCriador";
                colCriador.HeaderText = "Criado Por";
                colCriador.FillWeight = 120;

                // Data de alteração — nullable (pode ser null)
                DataGridViewTextBoxColumn colAlteracao = new DataGridViewTextBoxColumn();
                colAlteracao.DataPropertyName = "DataAlteracao";
                colAlteracao.HeaderText = "Alterado Em";
                colAlteracao.FillWeight = 130;
                colAlteracao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // Alterador — preenchido manualmente
                DataGridViewTextBoxColumn colAlterador = new DataGridViewTextBoxColumn();
                colAlterador.Name = "colAlterador";
                colAlterador.HeaderText = "Alterado Por";
                colAlterador.FillWeight = 120;

                // Adiciona todas as colunas
                dgvCompras.Columns.Add(colId);
                dgvCompras.Columns.Add(colNome);
                dgvCompras.Columns.Add(colEstado);
                dgvCompras.Columns.Add(colCriacao);
                dgvCompras.Columns.Add(colCriador);
                dgvCompras.Columns.Add(colAlteracao);
                dgvCompras.Columns.Add(colAlterador);
                // Liga os dados à grelha
                dgvCompras.DataSource = lista;
                dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // Preenche as colunas calculadas e aplica estilos visuais
                for (int i = 0; i < lista.Count; i++)
                {
                    ListaCompra c = lista[i];

                    // UtilizadorCriou é o nome da propriedade de navegação no modelo ListaCompra
                    if (c.UtilizadorCriou != null)
                    {
                        dgvCompras.Rows[i].Cells["colCriador"].Value = c.UtilizadorCriou.Nome;
                    }
                    else
                    {
                        dgvCompras.Rows[i].Cells["colCriador"].Value = "—";
                    }

                    // UtilizadorAlterou é o nome da propriedade de navegação no modelo ListaCompra
                    if (c.UtilizadorAlterou != null)
                    {
                        dgvCompras.Rows[i].Cells["colAlterador"].Value = c.UtilizadorAlterou.Nome;
                    }
                    else
                    {
                        dgvCompras.Rows[i].Cells["colAlterador"].Value = "—";
                    }

                    // Compras fechadas aparecem a cinzento para distinguir visualmente
                    if (c.Estado == "Fechada")
                    {
                        dgvCompras.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }

                // Mostra o total de compras na label
                lblContador.Text = lista.Count + " compra(s)";
                AtualizarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Atualiza os botões e o texto do botão Editar/Ver consoante o estado da seleção
        private void AtualizarBotoes()
        {
            bool temSelecao = dgvCompras.SelectedRows.Count > 0;
            btnEditarVer.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;

            if (temSelecao)
            {
                string estado = dgvCompras.SelectedRows[0].Cells["Estado"].Value.ToString();

                // Se a compra está fechada, o botão só permite visualizar (sem editar)
                if (estado == "Fechada")
                {
                    btnEditarVer.Text = "Ver";
                }
                else
                {
                    btnEditarVer.Text = "Editar";
                }
            }
        }

        // Botão "Filtrar" — recarrega a grelha com o filtro atual
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarCompras();
        }

        // Quando a seleção muda, atualiza os botões
        private void dgvCompras_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // Duplo clique abre o formulário de edição/visualização
        private void dgvCompras_DoubleClick(object sender, EventArgs e)
        {
            AbrirEdicao();
        }

        // Botão "Nova" — abre o form de criação (listaId = 0 significa nova lista)
        private void btnNova_Click(object sender, EventArgs e)
        {
            FormCriacaoEdicaoCompra form = new FormCriacaoEdicaoCompra(0);

            // Quando o form de criação fechar, recarrega a grelha automaticamente
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        // Botão "Editar/Ver" — abre o form com a lista selecionada
        private void btnEditarVer_Click(object sender, EventArgs e)
        {
            AbrirEdicao();
        }

        // Método partilhado para abrir o form de edição com a lista selecionada
        private void AbrirEdicao()
        {
            if (dgvCompras.SelectedRows.Count == 0)
            {
                return;
            }

            // Lê o Id da linha selecionada
            int id = (int)dgvCompras.SelectedRows[0].Cells["Id"].Value;

            // Passa o Id ao form — o form decide se é modo edição ou leitura consoante o estado
            FormCriacaoEdicaoCompra form = new FormCriacaoEdicaoCompra(id);
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        // Quando um sub-form fecha, recarrega a grelha para mostrar alterações
        private void SubForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CarregarCompras();
        }

        // Botão "Eliminar" — verifica regras e pede confirmação
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCompras.SelectedRows.Count == 0)
            {
                return;
            }

            int id = (int)dgvCompras.SelectedRows[0].Cells["Id"].Value;
            string nomeCompra = dgvCompras.SelectedRows[0].Cells["Nome"].Value.ToString();
            string estado = dgvCompras.SelectedRows[0].Cells["Estado"].Value.ToString();

            // Regra de negócio: não é possível eliminar compras fechadas
            if (estado == "Fechada")
            {
                MessageBox.Show("Não é possível eliminar uma compra fechada.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirmação antes de eliminar
            DialogResult confirmacao = MessageBox.Show(
                "Eliminar a compra \"" + nomeCompra + "\" e todos os seus itens?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    string mensagem = "";
                    bool sucesso = listaCtrl.Delete(id, out mensagem);

                    if (sucesso)
                    {
                        CarregarCompras();
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado: " + ex.Message,
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
