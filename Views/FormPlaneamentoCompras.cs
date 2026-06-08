// FormPlaneamentoCompras.cs
// Responsabilidade: interface gráfica para listar, filtrar, criar, editar e eliminar compras (US3)

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
        // controller para todas as operações de listas de compras com a BD
        private ListaCompraController listaCtrl;

        // guarda a lista atual para o DataBindingComplete poder aceder aos dados
        private List<ListaCompra> _listaAtual;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        public FormPlaneamentoCompras()
        {
            InitializeComponent();
            listaCtrl = new ListaCompraController();

            // preenche o ComboBox de filtro com as 3 opções
            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add("Todos");
            cmbFiltroEstado.Items.Add("Aberta");
            cmbFiltroEstado.Items.Add("Fechada");
            cmbFiltroEstado.SelectedIndex = 0;

            /* liga o evento DataBindingComplete — dispara quando o WinForms termina
            de criar as linhas da grelha, garantindo que as células já existem
            antes de tentarmos preencher colCriador e colAlterador manualmente */
            dgvCompras.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dgvCompras_DataBindingComplete);

            CarregarCompras();
        }

        // ── CARREGAR GRELHA ───────────────────────────────────────────────────

        private void CarregarCompras()
        {
            try
            {
                string filtro = cmbFiltroEstado.SelectedItem.ToString();

                // vai à BD buscar as compras conforme o filtro selecionado
                if (filtro == "Todos")
                    _listaAtual = listaCtrl.GetAll();
                else
                    _listaAtual = listaCtrl.GetByEstado(filtro);

                // limpa a grelha e define as colunas manualmente
                dgvCompras.DataSource = null;
                dgvCompras.AutoGenerateColumns = false;
                dgvCompras.Columns.Clear();

                // coluna Id — escondida, usada para saber qual linha está selecionada
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "Id";
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // coluna Nome — nome da compra
                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.Name = "Nome";
                colNome.DataPropertyName = "Nome";
                colNome.HeaderText = "Nome da Compra";
                colNome.FillWeight = 200;

                // coluna Estado — "Aberta" ou "Fechada"
                DataGridViewTextBoxColumn colEstado = new DataGridViewTextBoxColumn();
                colEstado.Name = "Estado";
                colEstado.DataPropertyName = "Estado";
                colEstado.HeaderText = "Estado";
                colEstado.FillWeight = 80;

                // coluna Data Criação — preenchida automaticamente pelo DataBinding
                DataGridViewTextBoxColumn colCriacao = new DataGridViewTextBoxColumn();
                colCriacao.DataPropertyName = "DataCriacao";
                colCriacao.HeaderText = "Criado Em";
                colCriacao.FillWeight = 130;
                colCriacao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // coluna Criado Por — preenchida manualmente no DataBindingComplete
                // porque é uma propriedade de navegação (UtilizadorCriou.Nome)
                DataGridViewTextBoxColumn colCriador = new DataGridViewTextBoxColumn();
                colCriador.Name = "colCriador";
                colCriador.HeaderText = "Criado Por";
                colCriador.FillWeight = 120;

                // coluna Data Alteração — preenchida automaticamente
                DataGridViewTextBoxColumn colAlteracao = new DataGridViewTextBoxColumn();
                colAlteracao.DataPropertyName = "DataAlteracao";
                colAlteracao.HeaderText = "Alterado Em";
                colAlteracao.FillWeight = 130;
                colAlteracao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // coluna Alterado Por — preenchida manualmente no DataBindingComplete
                DataGridViewTextBoxColumn colAlterador = new DataGridViewTextBoxColumn();
                colAlterador.Name = "colAlterador";
                colAlterador.HeaderText = "Alterado Por";
                colAlterador.FillWeight = 120;

                dgvCompras.Columns.Add(colId);
                dgvCompras.Columns.Add(colNome);
                dgvCompras.Columns.Add(colEstado);
                dgvCompras.Columns.Add(colCriacao);
                dgvCompras.Columns.Add(colCriador);
                dgvCompras.Columns.Add(colAlteracao);
                dgvCompras.Columns.Add(colAlterador);
                dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // atribui os dados — o WinForms cria as linhas de forma assíncrona
                // o preenchimento de colCriador e colAlterador é feito no DataBindingComplete
                dgvCompras.DataSource = _listaAtual;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── DATA BINDING COMPLETE ─────────────────────────────────────────────

        // Dispara quando o WinForms termina de criar todas as linhas da grelha
        // É aqui que preenchemos as colunas manuais (Criador, Alterador) e a cor das linhas
        // Fazer isto no CarregarCompras() causava bug: as linhas ainda não existiam quando
        // tentávamos escrever nas células
        private void dgvCompras_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (_listaAtual == null) return;

            for (int i = 0; i < _listaAtual.Count; i++)
            {
                ListaCompra c = _listaAtual[i];

                // preenche o nome do criador — "—" se não tiver utilizador associado
                dgvCompras.Rows[i].Cells["colCriador"].Value =
                    c.UtilizadorCriou != null ? c.UtilizadorCriou.Nome : "—";

                // preenche o nome do último utilizador a alterar
                dgvCompras.Rows[i].Cells["colAlterador"].Value =
                    c.UtilizadorAlterou != null ? c.UtilizadorAlterou.Nome : "—";

                // compras fechadas aparecem a cinzento para distinguir visualmente
                if (c.Estado == "Fechada")
                    dgvCompras.Rows[i].DefaultCellStyle.ForeColor = Color.Gray;
            }

            lblContador.Text = _listaAtual.Count + " compra(s)";
            AtualizarBotoes();
        }

        // ── BOTÕES ────────────────────────────────────────────────────────────

        // ativa/desativa botões e muda texto "Editar"/"Ver" conforme a seleção
        private void AtualizarBotoes()
        {
            bool temSelecao = dgvCompras.SelectedRows.Count > 0;
            btnEditarVer.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;

            if (temSelecao)
            {
                string estado = dgvCompras.SelectedRows[0].Cells["Estado"].Value.ToString();
                btnEditarVer.Text = estado == "Fechada" ? "Ver" : "Editar";
            }
        }

        // botão "Filtrar" — recarrega com o filtro atual
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarCompras();
        }

        // seleção na grelha mudou — atualiza botões
        private void dgvCompras_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // duplo clique numa linha — abre edição
        private void dgvCompras_DoubleClick(object sender, EventArgs e)
        {
            AbrirEdicao();
        }

        // botão "Nova" — abre o form com listaId=0 (criação de nova compra)
        private void btnNova_Click(object sender, EventArgs e)
        {
            FormCriacaoEdicaoCompra form = new FormCriacaoEdicaoCompra(0);
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        // botão "Editar"/"Ver" — abre edição para a linha selecionada
        private void btnEditarVer_Click(object sender, EventArgs e)
        {
            AbrirEdicao();
        }

        // método partilhado — lê o Id da linha selecionada e abre o FormCriacaoEdicaoCompra
        private void AbrirEdicao()
        {
            if (dgvCompras.SelectedRows.Count == 0) return;

            int id = (int)dgvCompras.SelectedRows[0].Cells["Id"].Value;
            FormCriacaoEdicaoCompra form = new FormCriacaoEdicaoCompra(id);
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        // quando o sub-form fecha, recarrega a grelha para mostrar eventuais alterações
        private void SubForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CarregarCompras();
        }

        // botão "Eliminar" — confirma e elimina a compra selecionada
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCompras.SelectedRows.Count == 0) return;

            int id = (int)dgvCompras.SelectedRows[0].Cells["Id"].Value;
            string nomeCompra = dgvCompras.SelectedRows[0].Cells["Nome"].Value.ToString();
            string estado = dgvCompras.SelectedRows[0].Cells["Estado"].Value.ToString();

            // regra de negócio: compras fechadas não podem ser eliminadas
            if (estado == "Fechada")
            {
                MessageBox.Show("Não é possível eliminar uma compra fechada.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacao = MessageBox.Show(
                "Eliminar a compra \"" + nomeCompra + "\" e todos os seus itens?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                    MessageBox.Show("Erro inesperado: " + ex.Message, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}