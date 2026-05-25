// Responsabilidade: interface gráfica para o CRUD de Orçamentos mensais
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Projeto_DA_MDS.Controllers;
using Projeto_DA_MDS.Models;

namespace Projeto_DA_MDS.Views
{
    public partial class FormGestaoOrcamentos : Form
    {
        // Controller que trata toda a lógica de orçamentos
        private OrcamentoController orcamentoCtrl;

        // Id do orçamento selecionado (0 = modo criação)
        private int idSelecionado;

        public FormGestaoOrcamentos()
        {
            // Inicializa os controlos do Designer
            InitializeComponent();

            orcamentoCtrl = new OrcamentoController();
            idSelecionado = 0;

            btnNovo.Click += new EventHandler(btnNovo_Click);
            btnEditar.Click += new EventHandler(btnEditar_Click);
            btnEliminar.Click += new EventHandler(btnEliminar_Click);
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnCancelar.Click += new EventHandler(btnCancelar_Click);
            dgvOrcamentos.SelectionChanged += new EventHandler(dgvOrcamentos_SelectionChanged);
            dgvOrcamentos.DoubleClick += new EventHandler(dgvOrcamentos_DoubleClick);

            // Carrega os orçamentos existentes na grelha
            CarregarOrcamentos();

            // Esconde o painel de formulário por omissão
            ModoLeitura();
        }

        // Carrega todos os orçamentos na grelha com colunas personalizadas
        private void CarregarOrcamentos()
        {
            try
            {
                // Obtém todos os orçamentos ordenados do mais recente
                List<Orcamento> lista = orcamentoCtrl.GetAll();

                // Limpa a fonte anterior e define as colunas manualmente
                dgvOrcamentos.DataSource = null;
                dgvOrcamentos.AutoGenerateColumns = false;
                dgvOrcamentos.Columns.Clear();

                // Coluna Id — escondida, usada para identificar o registo no CRUD
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.DataPropertyName = "Id";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // Coluna Mês — preenchida manualmente abaixo (não mapeia diretamente)
                DataGridViewTextBoxColumn colMes = new DataGridViewTextBoxColumn();
                colMes.Name = "colMes";
                colMes.HeaderText = "Mês";
                colMes.FillWeight = 60;

                // Coluna Ano — preenchida manualmente abaixo
                DataGridViewTextBoxColumn colAno = new DataGridViewTextBoxColumn();
                colAno.Name = "colAno";
                colAno.HeaderText = "Ano";
                colAno.FillWeight = 70;

                // Coluna Valor — ValorMaximo é o nome correto no modelo Orcamento
                DataGridViewTextBoxColumn colValor = new DataGridViewTextBoxColumn();
                colValor.DataPropertyName = "ValorMaximo";
                colValor.HeaderText = "Orçamento (€)";
                colValor.FillWeight = 120;
                colValor.DefaultCellStyle.Format = "C2"; // Formato monetário com 2 casas decimais

                // Coluna criador — preenchida manualmente a partir da propriedade de navegação
                DataGridViewTextBoxColumn colCriador = new DataGridViewTextBoxColumn();
                colCriador.Name = "colCriador";
                colCriador.HeaderText = "Criado Por";
                colCriador.FillWeight = 130;

                // Coluna data criação — mapeada diretamente com formato de data/hora
                DataGridViewTextBoxColumn colCriacao = new DataGridViewTextBoxColumn();
                colCriacao.DataPropertyName = "DataCriacao";
                colCriacao.HeaderText = "Data Criação";
                colCriacao.FillWeight = 120;
                colCriacao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // Coluna alterador — preenchida manualmente
                DataGridViewTextBoxColumn colAlterador = new DataGridViewTextBoxColumn();
                colAlterador.Name = "colAlterador";
                colAlterador.HeaderText = "Alterado Por";
                colAlterador.FillWeight = 130;

                // Coluna data alteração
                DataGridViewTextBoxColumn colAlteracao = new DataGridViewTextBoxColumn();
                colAlteracao.DataPropertyName = "DataAlteracao";
                colAlteracao.HeaderText = "Data Alteração";
                colAlteracao.FillWeight = 120;
                colAlteracao.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

                // Adiciona todas as colunas à grelha
                dgvOrcamentos.Columns.Add(colId);
                dgvOrcamentos.Columns.Add(colMes);
                dgvOrcamentos.Columns.Add(colAno);
                dgvOrcamentos.Columns.Add(colValor);
                dgvOrcamentos.Columns.Add(colCriador);
                dgvOrcamentos.Columns.Add(colCriacao);
                dgvOrcamentos.Columns.Add(colAlterador);
                dgvOrcamentos.Columns.Add(colAlteracao);
                // Liga a lista à grelha
                dgvOrcamentos.DataSource = lista;
                dgvOrcamentos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Preenche as colunas calculadas manualmente (propriedades de navegação)
                for (int i = 0; i < lista.Count; i++)
                {
                    Orcamento o = lista[i];

                    // D2 garante sempre dois dígitos (ex: "05" em vez de "5")
                    dgvOrcamentos.Rows[i].Cells["colMes"].Value = o.Mes.ToString("D2");
                    dgvOrcamentos.Rows[i].Cells["colAno"].Value = o.Ano;

                    // UtilizadorCriou é o nome da propriedade de navegação no modelo Orcamento
                    if (o.UtilizadorCriou != null)
                    {
                        dgvOrcamentos.Rows[i].Cells["colCriador"].Value = o.UtilizadorCriou.Nome;
                    }
                    else
                    {
                        dgvOrcamentos.Rows[i].Cells["colCriador"].Value = "—";
                    }

                    // UtilizadorAlterou é o nome da propriedade de navegação no modelo Orcamento
                    if (o.UtilizadorAlterou != null)
                    {
                        dgvOrcamentos.Rows[i].Cells["colAlterador"].Value = o.UtilizadorAlterou.Nome;
                    }
                    else
                    {
                        dgvOrcamentos.Rows[i].Cells["colAlterador"].Value = "—";
                    }
                }

                AtualizarBotoes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar orçamentos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Prepara o form para criar um novo orçamento
        private void ModoNovo()
        {
            idSelecionado = 0;

            // Pré-preenche com o mês e ano atuais para facilitar
            nudMes.Value = DateTime.Now.Month;
            nudAno.Value = DateTime.Now.Year;
            nudValor.Value = 0;

            // Limpa as labels de informação do criador/alterador
            lblCriadoPor.Text = "";
            lblAlteradoPor.Text = "";
            lblStatus.Text = "";

            MostrarPainelFormulario(true);
            nudValor.Focus(); // Coloca o cursor no campo de valor
        }

        // Prepara o form para editar o orçamento selecionado
        private void ModoEdicao()
        {
            if (dgvOrcamentos.SelectedRows.Count == 0)
            {
                return;
            }

            try
            {
                // Lê o Id da linha selecionada
                idSelecionado = (int)dgvOrcamentos.SelectedRows[0].Cells["Id"].Value;

                // Obtém o orçamento completo da BD (com os utilizadores carregados)
                Orcamento orc = orcamentoCtrl.GetById(idSelecionado);

                if (orc == null)
                {
                    return;
                }

                // Preenche os campos do formulário com os valores atuais
                nudMes.Value = orc.Mes;
                nudAno.Value = orc.Ano;
                nudValor.Value = orc.ValorMaximo; // ValorMaximo é o nome correto no modelo

                // Mostra informação do criador — ternário para evitar NullReferenceException
                string nomeCriador = orc.UtilizadorCriou != null ? orc.UtilizadorCriou.Nome : "—";
                lblCriadoPor.Text = "Criado por: " + nomeCriador +
                                    " em " + orc.DataCriacao.ToString("dd/MM/yyyy HH:mm");

                // Mostra informação do último utilizador a alterar
                string nomeAlterador = orc.UtilizadorAlterou != null ? orc.UtilizadorAlterou.Nome : "—";
                lblAlteradoPor.Text = "Alterado por: " + nomeAlterador +
                                      " em " + (orc.DataAlteracao.HasValue ?
                                      orc.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm") : "—");

                lblStatus.Text = "";
                MostrarPainelFormulario(true);
                nudValor.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao abrir edição: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Volta ao modo leitura — esconde o painel de formulário
        private void ModoLeitura()
        {
            idSelecionado = 0;
            MostrarPainelFormulario(false);
        }

        // Controla a visibilidade do painel de formulário
        private void MostrarPainelFormulario(bool mostrar)
        {
            panelFormulario.Visible = mostrar;
        }

        // Ativa/desativa botões conforme a seleção na grelha
        private void AtualizarBotoes()
        {
            bool temSelecao = dgvOrcamentos.SelectedRows.Count > 0;
            btnEditar.Enabled = temSelecao;
            btnEliminar.Enabled = temSelecao;
        }

        // Quando a seleção na grelha muda, atualiza os botões
        private void dgvOrcamentos_SelectionChanged(object sender, EventArgs e)
        {
            AtualizarBotoes();
        }

        // Duplo clique na grelha abre o modo de edição
        private void dgvOrcamentos_DoubleClick(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // Botão "Novo"
        private void btnNovo_Click(object sender, EventArgs e)
        {
            ModoNovo();
        }

        // Botão "Editar"
        private void btnEditar_Click(object sender, EventArgs e)
        {
            ModoEdicao();
        }

        // Botão "Cancelar"
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoLeitura();
        }

        // Botão "Guardar" — cria ou atualiza o orçamento
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Lê os valores dos campos numéricos
            int mes = (int)nudMes.Value;
            int ano = (int)nudAno.Value;
            decimal valor = nudValor.Value;

            // Obtém o Id do utilizador logado através da classe Sessao
            int userId = Sessao.UtilizadorAtual.Id;
            string mensagem = "";
            bool sucesso = false;

            try
            {
                // Decide se é criação ou atualização com base em idSelecionado
                if (idSelecionado == 0)
                {
                    sucesso = orcamentoCtrl.Add(valor, mes, ano, userId, out mensagem);
                }
                else
                {
                    sucesso = orcamentoCtrl.Update(idSelecionado, valor, mes, ano, userId, out mensagem);
                }

                if (sucesso)
                {
                    ModoLeitura();
                    CarregarOrcamentos();
                    MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Mostra o erro na label sem fechar o formulário
                    lblStatus.Text = mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botão "Eliminar" — pede confirmação e elimina
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvOrcamentos.SelectedRows.Count == 0)
            {
                return;
            }

            int id = (int)dgvOrcamentos.SelectedRows[0].Cells["Id"].Value;

            // Lê os valores de mês e ano para a mensagem de confirmação
            object mes = dgvOrcamentos.SelectedRows[0].Cells["colMes"].Value;
            object ano = dgvOrcamentos.SelectedRows[0].Cells["colAno"].Value;

            DialogResult confirmacao = MessageBox.Show(
                "Eliminar orçamento de " + mes + "/" + ano + "?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacao == DialogResult.Yes)
            {
                try
                {
                    string mensagem = "";
                    bool sucesso = orcamentoCtrl.Delete(id, out mensagem);

                    if (sucesso)
                    {
                        ModoLeitura();
                        CarregarOrcamentos();
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
