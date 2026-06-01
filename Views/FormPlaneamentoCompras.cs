using System;
using System.Collections.Generic;
using System.Drawing;              // acesso a Color (para colorir linhas fechadas)
using System.Windows.Forms;
using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS.Views
{
    public partial class FormPlaneamentoCompras : Form // lista todas as compras com filtro e CRUD
    {
        private ListaCompraController listaCtrl; // controller para operações de listas de compras

        public FormPlaneamentoCompras()
        {
            InitializeComponent();           // cria os controlos definidos no Designer
            listaCtrl = new ListaCompraController(); // instancia o controller

            cmbFiltroEstado.Items.Clear();   // ← limpa os 3 que o Designer já adicionou
            cmbFiltroEstado.Items.Add("Todos");
            cmbFiltroEstado.Items.Add("Aberta");
            cmbFiltroEstado.Items.Add("Fechada");
            cmbFiltroEstado.SelectedIndex = 0;

            CarregarCompras(); // preenche a grelha ao abrir o form
        }

        private void CarregarCompras() // lê as compras da BD e preenche a grelha conforme o filtro
        {
            try
            {
                string filtro = cmbFiltroEstado.SelectedItem.ToString(); // lê o texto selecionado no ComboBox
                List<ListaCompra> lista; // vai guardar a lista de compras a mostrar

                if (filtro == "Todos") // sem filtro — busca tudo
                {
                    lista = listaCtrl.GetAll(); // vai à BD buscar todas as compras
                }
                else // com filtro — "Aberta" ou "Fechada"
                {
                    lista = listaCtrl.GetByEstado(filtro); // vai à BD buscar só as do estado escolhido
                }

                dgvCompras.DataSource = null;           // limpa a fonte anterior
                dgvCompras.AutoGenerateColumns = false; // desativa criação automática de colunas
                dgvCompras.Columns.Clear();             // remove colunas existentes

                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "Id";               // chave para Cells["Id"]
                colId.DataPropertyName = "Id";   // mapeia ao campo Id do modelo
                colId.HeaderText = "ID";
                colId.Visible = false;           // escondida — só usada internamente

                DataGridViewTextBoxColumn colNome = new DataGridViewTextBoxColumn();
                colNome.Name = "Nome";                 // chave para Cells["Nome"]
                colNome.DataPropertyName = "Nome";     // mapeia ao campo Nome do modelo
                colNome.HeaderText = "Nome da Compra";
                colNome.FillWeight = 200;

                DataGridViewTextBoxColumn colEstado = new DataGridViewTextBoxColumn();
                colEstado.Name = "Estado";             // chave para Cells["Estado"]
                colEstado.DataPropertyName = "Estado"; // mapeia ao campo Estado ("Aberta" ou "Fechada")
                colEstado.HeaderText = "Estado";
                colEstado.FillWeight = 80;

                DataGridViewTextBoxColumn colCriacao = new DataGridViewTextBoxColumn();
                colCriacao.DataPropertyName = "DataCriacao"; // mapeia à data de criação
                colCriacao.HeaderText = "Criado Em";
                colCriacao.FillWeight = 130;
                colCriacao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"; // formato de data legível

                DataGridViewTextBoxColumn colCriador = new DataGridViewTextBoxColumn();
                colCriador.Name = "colCriador";  // chave para preencher manualmente abaixo
                colCriador.HeaderText = "Criado Por";
                colCriador.FillWeight = 120;     // sem DataPropertyName — preenchido manualmente

                DataGridViewTextBoxColumn colAlteracao = new DataGridViewTextBoxColumn();
                colAlteracao.DataPropertyName = "DataAlteracao"; // mapeia à data de alteração (nullable)
                colAlteracao.HeaderText = "Alterado Em";
                colAlteracao.FillWeight = 130;
                colAlteracao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                DataGridViewTextBoxColumn colAlterador = new DataGridViewTextBoxColumn();
                colAlterador.Name = "colAlterador"; // chave para preencher manualmente abaixo
                colAlterador.HeaderText = "Alterado Por";
                colAlterador.FillWeight = 120;

                dgvCompras.Columns.Add(colId);         // adiciona colunas pela ordem que vão aparecer
                dgvCompras.Columns.Add(colNome);
                dgvCompras.Columns.Add(colEstado);
                dgvCompras.Columns.Add(colCriacao);
                dgvCompras.Columns.Add(colCriador);
                dgvCompras.Columns.Add(colAlteracao);
                dgvCompras.Columns.Add(colAlterador);
                dgvCompras.DataSource = lista;         // liga os dados à grelha
                dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // larguras fixas

                for (int i = 0; i < lista.Count; i++) // percorre cada linha para preencher colunas manuais
                {
                    ListaCompra c = lista[i]; // atalho para o item atual

                    dgvCompras.Rows[i].Cells["colCriador"].Value =
                        c.UtilizadorCriou != null ? c.UtilizadorCriou.Nome : "—";
                    // preenche o nome do criador; "—" se a propriedade de navegação for null

                    dgvCompras.Rows[i].Cells["colAlterador"].Value =
                        c.UtilizadorAlterou != null ? c.UtilizadorAlterou.Nome : "—";
                    // preenche o nome do último a alterar

                    if (c.Estado == "Fechada") // diferencia visualmente compras fechadas
                    {
                        dgvCompras.Rows[i].DefaultCellStyle.ForeColor = Color.Gray; // texto cinzento
                    }
                }

                lblContador.Text = lista.Count + " compra(s)"; // mostra o total de compras na label
                AtualizarBotoes(); // ativa/desativa botões conforme a seleção atual
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar compras: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarBotoes() // atualiza o estado dos botões conforme a linha selecionada
        {
            bool temSelecao = dgvCompras.SelectedRows.Count > 0; // há linha selecionada?
            btnEditarVer.Enabled = temSelecao; // só ativa se houver seleção
            btnEliminar.Enabled = temSelecao;

            if (temSelecao) // se há seleção, ajusta o texto do botão Editar/Ver
            {
                string estado = dgvCompras.SelectedRows[0].Cells["Estado"].Value.ToString(); // lê o estado da linha

                if (estado == "Fechada") // compra fechada — só pode ver, não editar
                {
                    btnEditarVer.Text = "Ver"; // muda o texto do botão
                }
                else // compra aberta — pode editar
                {
                    btnEditarVer.Text = "Editar";
                }
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e) // botão "Filtrar"
        {
            CarregarCompras(); // recarrega a grelha aplicando o filtro atual do ComboBox
        }

        private void dgvCompras_SelectionChanged(object sender, EventArgs e) // seleção na grelha mudou
        {
            AtualizarBotoes(); // atualiza os botões conforme o que está selecionado
        }

        private void dgvCompras_DoubleClick(object sender, EventArgs e) // double-click na grelha
        {
            AbrirEdicao(); // abre o form de edição para a linha selecionada
        }

        private void btnNova_Click(object sender, EventArgs e) // botão "Nova"
        {
            FormCriacaoEdicaoCompra form = new FormCriacaoEdicaoCompra(0);
            // passa 0 como listaId — o form interpreta 0 como "criar nova lista"
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            // quando fechar, recarrega a grelha para mostrar a nova compra
            form.ShowDialog();
        }

        private void btnEditarVer_Click(object sender, EventArgs e) // botão "Editar" ou "Ver"
        {
            AbrirEdicao(); // delega no método partilhado
        }

        private void AbrirEdicao() // método partilhado — abre o form de edição/visualização
        {
            if (dgvCompras.SelectedRows.Count == 0) // proteção: sem seleção não faz nada
            {
                return;
            }

            int id = (int)dgvCompras.SelectedRows[0].Cells["Id"].Value;
            // lê o Id da linha selecionada (coluna invisível)

            FormCriacaoEdicaoCompra form = new FormCriacaoEdicaoCompra(id);
            // passa o Id — o form vai à BD buscar os dados e decide se é edição ou leitura
            form.FormClosed += new FormClosedEventHandler(SubForm_FormClosed);
            form.ShowDialog();
        }

        private void SubForm_FormClosed(object sender, FormClosedEventArgs e) // sub-form fechou
        {
            CarregarCompras(); // recarrega a grelha para mostrar eventuais alterações
        }

        private void btnEliminar_Click(object sender, EventArgs e) // botão "Eliminar"
        {
            if (dgvCompras.SelectedRows.Count == 0) // proteção: sem seleção não faz nada
            {
                return;
            }

            int id = (int)dgvCompras.SelectedRows[0].Cells["Id"].Value; // Id da linha selecionada
            string nomeCompra = dgvCompras.SelectedRows[0].Cells["Nome"].Value.ToString(); // nome para a confirmação
            string estado = dgvCompras.SelectedRows[0].Cells["Estado"].Value.ToString();   // estado atual

            if (estado == "Fechada") // regra de negócio: compras fechadas não podem ser eliminadas
            {
                MessageBox.Show("Não é possível eliminar uma compra fechada.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // sai sem eliminar
            }

            DialogResult confirmacao = MessageBox.Show(
                "Eliminar a compra \"" + nomeCompra + "\" e todos os seus itens?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            // mostra caixa de confirmação com botões "Sim" e "Não"

            if (confirmacao == DialogResult.Yes) // utilizador confirmou a eliminação
            {
                try
                {
                    string mensagem = "";
                    bool sucesso = listaCtrl.Delete(id, out mensagem);
                    // tenta eliminar na BD; out mensagem recebe o texto de resultado do controller

                    if (sucesso)
                    {
                        CarregarCompras(); // recarrega a grelha após eliminar
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