// FormCriacaoEdicaoCompra.cs
// Responsabilidade: interface gráfica para criar ou editar uma lista de compras (US3)
// Permite definir o nome da compra e gerir os itens previstos (com Tipo, Artigo e Quantidade)
// Listas fechadas abrem em modo só leitura — não permitem alterações

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
        // Controllers usados pelo form
        private ListaCompraController listaCtrl;
        private ArtigoController artigoCtrl;

        // Id da lista a editar (0 = criar nova lista)
        private int listaId;

        // Flag que indica se a lista está fechada — bloqueia edições
        private bool soLeitura;

        // ── CONSTRUTOR ────────────────────────────────────────────────────────

        // Recebe o Id da lista. Se for 0 → criação de nova; se >0 → edição/visualização
        public FormCriacaoEdicaoCompra(int listaId)
        {
            InitializeComponent();

            this.listaId = listaId;
            listaCtrl = new ListaCompraController();
            artigoCtrl = new ArtigoController();
            soLeitura = false;

            // Regista os eventos dos botões e do ComboBox
            btnGuardar.Click += new EventHandler(btnGuardar_Click);
            btnAdicionarItem.Click += new EventHandler(btnAdicionarItem_Click);
            btnRemoverItem.Click += new EventHandler(btnRemoverItem_Click);
            btnFechar.Click += new EventHandler(btnFechar_Click);
            cmbTipoArtigo.SelectedIndexChanged += new EventHandler(cmbTipoArtigo_SelectedIndexChanged);

            // Carrega os tipos de artigo no ComboBox
            CarregarTiposArtigo();

            // Carrega os dados da lista (ou prepara form vazio se for nova)
            CarregarDados();
        }

        // ── CARREGAR TIPOS DE ARTIGO ──────────────────────────────────────────

        // Carrega os tipos de artigo diretamente pela BD (sem usar TipoArtigoController)
        // Adiciona uma opção "— Seleciona Tipo —" no início para forçar escolha consciente
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

                    // Limpa o ComboBox de artigos — vai ser preenchido após selecionar tipo
                    cmbArtigo.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tipos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── CARREGAR DADOS DA LISTA ───────────────────────────────────────────

        // Carrega os dados da lista para o form. Diferencia 3 cenários:
        //   1. listaId == 0 → criação de nova lista (campos vazios)
        //   2. listaId > 0 + Estado "Aberta" → modo edição completa
        //   3. listaId > 0 + Estado "Fechada" → modo só leitura
        private void CarregarDados()
        {
            // Cenário 1: criação de nova lista
            if (listaId == 0)
            {
                txtNome.Clear();
                lblEstado.Text = "[ Nova ]";
                lblEstado.ForeColor = Color.SteelBlue;
                lblInfo.Text = "Criado por: " + (Sessao.UtilizadorAtual?.Nome ?? "—") +
                               "  |  " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                return;
            }

            try
            {
                // Vai à BD buscar a lista com todos os dados relacionados
                ListaCompra lista = listaCtrl.GetById(listaId);

                if (lista == null)
                {
                    MessageBox.Show("Lista não encontrada.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Preenche os campos do form com os dados da lista
                txtNome.Text = lista.Nome;
                lblEstado.Text = lista.Estado == "Fechada" ? "FECHADA" : "ABERTA";
                lblEstado.ForeColor = lista.Estado == "Fechada" ? Color.Gray : Color.DarkGreen;

                // Monta a label de auditoria com criador, alterador e datas
                string nomeCriador = lista.UtilizadorCriou != null ? lista.UtilizadorCriou.Nome : "—";
                string nomeAlterador = lista.UtilizadorAlterou != null ? lista.UtilizadorAlterou.Nome : "—";
                string dataAlt = lista.DataAlteracao.HasValue
                    ? lista.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm")
                    : "—";
                lblInfo.Text = "Criado: " + nomeCriador + " em " + lista.DataCriacao.ToString("dd/MM/yyyy HH:mm") +
                               "  |  Alterado: " + nomeAlterador + " em " + dataAlt;

                // Se a lista está fechada, bloqueia todas as edições
                soLeitura = lista.Estado == "Fechada";
                txtNome.ReadOnly = soLeitura;
                btnGuardar.Enabled = !soLeitura;
                btnAdicionarItem.Enabled = !soLeitura;
                btnRemoverItem.Enabled = !soLeitura;
                panelAddItem.Enabled = !soLeitura;

                // Carrega os itens previstos na grelha
                CarregarItens();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── CARREGAR ITENS NA GRELHA ──────────────────────────────────────────

        // Preenche a dgvItens com os itens previstos da lista atual
        // IMPORTANTE: usamos preenchimento 100% manual (sem DataSource) para evitar
        //             conflitos entre o data binding e a atribuição manual das propriedades
        //             de navegação (Artigo.Nome e Artigo.Tipo.Nome)
        private void CarregarItens()
        {
            if (listaId == 0) return;

            try
            {
                // Vai à BD buscar os itens previstos com Artigo e Tipo já carregados
                List<ItemPrevisto> itens = listaCtrl.GetItensPrevistos(listaId);

                // Reseta a grelha — sem DataSource, sem AutoGenerateColumns
                dgvItens.DataSource = null;
                dgvItens.AutoGenerateColumns = false;
                dgvItens.Rows.Clear();
                dgvItens.Columns.Clear();
                dgvItens.AllowUserToAddRows = false;
                dgvItens.ReadOnly = true;

                // Coluna Id — escondida, usada apenas internamente para Remover Item
                DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
                colId.Name = "colId";
                colId.HeaderText = "ID";
                colId.Visible = false;

                // Coluna Tipo de Artigo — preenchida manualmente (propriedade de navegação)
                DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn();
                colTipo.Name = "colTipoArtigo";
                colTipo.HeaderText = "Tipo de Artigo";
                colTipo.FillWeight = 160;

                // Coluna Artigo — preenchida manualmente (propriedade de navegação)
                DataGridViewTextBoxColumn colArtigo = new DataGridViewTextBoxColumn();
                colArtigo.Name = "colArtigo";
                colArtigo.HeaderText = "Artigo";
                colArtigo.FillWeight = 200;

                // Coluna Quantidade Prevista — preenchida manualmente
                DataGridViewTextBoxColumn colQtdPrevista = new DataGridViewTextBoxColumn();
                colQtdPrevista.Name = "colQtdPrevista";
                colQtdPrevista.HeaderText = "Qtd Prevista";
                colQtdPrevista.FillWeight = 100;

                // Coluna Quantidade Adquirida — preenchida manualmente
                DataGridViewTextBoxColumn colQtdAdq = new DataGridViewTextBoxColumn();
                colQtdAdq.Name = "colQtdAdquirida";
                colQtdAdq.HeaderText = "Qtd Adquirida";
                colQtdAdq.FillWeight = 100;

                // Adiciona as colunas pela ordem que vão aparecer na grelha
                dgvItens.Columns.Add(colId);
                dgvItens.Columns.Add(colTipo);
                dgvItens.Columns.Add(colArtigo);
                dgvItens.Columns.Add(colQtdPrevista);
                dgvItens.Columns.Add(colQtdAdq);
                dgvItens.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Adiciona uma linha por cada item, preenchendo TUDO manualmente
                // Isto evita o problema de o data binding sobrescrever as células
                foreach (ItemPrevisto item in itens)
                {
                    string nomeArtigo = item.Artigo != null ? item.Artigo.Nome : "—";
                    string nomeTipo = item.Artigo != null && item.Artigo.Tipo != null
                        ? item.Artigo.Tipo.Nome
                        : "—";

                    dgvItens.Rows.Add(
                        item.Id,                      // colId (escondida)
                        nomeTipo,                     // colTipoArtigo
                        nomeArtigo,                   // colArtigo
                        item.QuantidadePrevista,      // colQtdPrevista
                        item.QuantidadeAdquirida      // colQtdAdquirida
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar itens: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── FILTRO DE ARTIGOS POR TIPO ────────────────────────────────────────

        // Quando o tipo no ComboBox muda, recarrega o ComboBox de artigos
        // com apenas os artigos desse tipo (filtro em cascata)
        private void cmbTipoArtigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboItem tipo = cmbTipoArtigo.SelectedItem as ComboItem;
            cmbArtigo.Items.Clear();

            // Se selecionou a opção "— Seleciona Tipo —" (Id=0), não carrega nada
            if (tipo == null || tipo.Id == 0) return;

            try
            {
                // Vai à BD buscar apenas os artigos deste tipo
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
                MessageBox.Show("Erro ao filtrar artigos: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── BOTÕES ────────────────────────────────────────────────────────────

        // Botão "Guardar" — cria nova lista ou atualiza o nome de uma existente
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Proteção: tem de existir um utilizador autenticado
            if (Sessao.UtilizadorAtual == null)
            {
                MessageBox.Show("Utilizador não autenticado. Por favor, faça login.",
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int userId = Sessao.UtilizadorAtual.Id;
            string mensagem = "";
            bool sucesso = false;

            try
            {
                // Cenário 1: criação de nova lista
                if (listaId == 0)
                {
                    sucesso = listaCtrl.Add(txtNome.Text, userId, out mensagem);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();   // fecha o form após criar — utilizador volta ao planeamento
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Cenário 2: edição de lista existente
                else
                {
                    sucesso = listaCtrl.Update(listaId, txtNome.Text, userId, out mensagem);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro inesperado: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botão "Adicionar Item" — adiciona um novo item previsto à lista
        private void btnAdicionarItem_Click(object sender, EventArgs e)
        {
            // Não pode adicionar itens a uma lista que ainda não foi guardada
            if (listaId == 0)
            {
                lblAddInfo.Text = "Guarda primeiro a compra antes de adicionar itens.";
                return;
            }

            // Validação: tem de estar um artigo selecionado
            ComboItem artigo = cmbArtigo.SelectedItem as ComboItem;
            if (artigo == null || artigo.Id == 0)
            {
                lblAddInfo.Text = "Seleciona um Tipo e um Artigo.";
                return;
            }

            try
            {
                string mensagem = "";
                bool sucesso = listaCtrl.AddItemPrevisto(
                    listaId, artigo.Id, (int)nudQuantidade.Value, out mensagem);

                if (sucesso)
                {
                    lblAddInfo.Text = "";
                    CarregarItens();   // recarrega a grelha para mostrar o novo item
                }
                else
                {
                    // Pode falhar por artigo já adicionado ou lista fechada
                    lblAddInfo.Text = mensagem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar item: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botão "Remover Item" — pede confirmação e remove o item selecionado
        private void btnRemoverItem_Click(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count == 0) return;

            // Lê o Id da linha selecionada através da coluna escondida "colId"
            int id = (int)dgvItens.SelectedRows[0].Cells["colId"].Value;
            string nomeArtigo = dgvItens.SelectedRows[0].Cells["colArtigo"].Value.ToString();

            // Pede confirmação antes de remover
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
                        CarregarItens();   // recarrega a grelha após remover
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro inesperado: " + ex.Message,
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Botão "Fechar" — fecha o form sem guardar
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ── EVENTOS VAZIOS ────────────────────────────────────────────────────

        // Eventos vazios — mantidos para o Designer não perder a ligação ao evento
        private void btnRemoverItem_Click_1(object sender, EventArgs e) { }
        private void dgvItens_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // ── CLASSE AUXILIAR ───────────────────────────────────────────────────

        // ComboItem — classe auxiliar para os ComboBoxes
        // Guarda o Id (para operações na BD) e o Nome (para mostrar ao utilizador)
        private class ComboItem
        {
            private int _id;
            private string _nome;

            public int Id { get { return _id; } set { _id = value; } }
            public string Nome { get { return _nome; } set { _nome = value; } }

            public ComboItem(int id, string nome) { _id = id; _nome = nome; }

            // ToString é usado pelo ComboBox para mostrar o texto de cada opção
            public override string ToString() { return _nome; }
        }
    }
}