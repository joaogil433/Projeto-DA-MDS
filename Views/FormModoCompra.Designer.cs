namespace Projeto_DA_MDS.Views
{
    partial class FormModoCompra
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dgvItens;
        private System.Windows.Forms.Label lblOrcamentoMax;
        private System.Windows.Forms.Label lblTotalGasto;
        private System.Windows.Forms.Label lblDisponivel;
        private System.Windows.Forms.Label lblAlerta;
        private System.Windows.Forms.ComboBox cmbArtigo;
        private System.Windows.Forms.TextBox tbObservacoes;
        private System.Windows.Forms.Button btnAdicionarNaoPrevisto;
        private System.Windows.Forms.Button btnFecharCompra;
        private System.Windows.Forms.Label lblTituloCompra;
        private System.Windows.Forms.GroupBox gbNaoPrevisto;
        private System.Windows.Forms.Label lblArtigo;
        private System.Windows.Forms.Label lblObservacoes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.lblOrcamentoMax = new System.Windows.Forms.Label();
            this.lblTotalGasto = new System.Windows.Forms.Label();
            this.lblDisponivel = new System.Windows.Forms.Label();
            this.lblAlerta = new System.Windows.Forms.Label();
            this.cmbArtigo = new System.Windows.Forms.ComboBox();
            this.tbObservacoes = new System.Windows.Forms.TextBox();
            this.btnAdicionarNaoPrevisto = new System.Windows.Forms.Button();
            this.btnFecharCompra = new System.Windows.Forms.Button();
            this.lblTituloCompra = new System.Windows.Forms.Label();
            this.gbNaoPrevisto = new System.Windows.Forms.GroupBox();
            this.lblArtigo = new System.Windows.Forms.Label();
            this.lblObservacoes = new System.Windows.Forms.Label();

            // dgvItens
            this.dgvItens.Location = new System.Drawing.Point(12, 60);
            this.dgvItens.Size = new System.Drawing.Size(760, 280);
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItens_CellEndEdit);
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", Visible = false });
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colArtigo", HeaderText = "Artigo", Width = 180, ReadOnly = true });
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colQtdPrevista", HeaderText = "Qtd Prevista", Width = 90, ReadOnly = true });
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colQtdAdquirida", HeaderText = "Qtd Adquirida", Width = 100 });
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colPreco", HeaderText = "Preço Unit. (€)", Width = 110 });
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colTipo", HeaderText = "Tipo", Width = 100, ReadOnly = true });
            this.dgvItens.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colObs", HeaderText = "Observações", Width = 160, ReadOnly = true });

            // Labels orçamento
            this.lblOrcamentoMax.Location = new System.Drawing.Point(12, 355);
            this.lblOrcamentoMax.Size = new System.Drawing.Size(200, 20);

            this.lblTotalGasto.Location = new System.Drawing.Point(220, 355);
            this.lblTotalGasto.Size = new System.Drawing.Size(200, 20);

            this.lblDisponivel.Location = new System.Drawing.Point(430, 355);
            this.lblDisponivel.Size = new System.Drawing.Size(200, 20);
            this.lblDisponivel.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);

            this.lblAlerta.Location = new System.Drawing.Point(640, 350);
            this.lblAlerta.Size = new System.Drawing.Size(130, 30);
            this.lblAlerta.Text = "⚠ Orçamento ultrapassado!";
            this.lblAlerta.ForeColor = System.Drawing.Color.Red;
            this.lblAlerta.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
            this.lblAlerta.Visible = false;

            // GroupBox Não Previsto
            this.gbNaoPrevisto.Text = "Adicionar Item Não Previsto";
            this.gbNaoPrevisto.Location = new System.Drawing.Point(12, 390);
            this.gbNaoPrevisto.Size = new System.Drawing.Size(760, 90);

            this.lblArtigo.Text = "Artigo:";
            this.lblArtigo.Location = new System.Drawing.Point(10, 25);
            this.lblArtigo.Size = new System.Drawing.Size(50, 20);

            this.cmbArtigo.Location = new System.Drawing.Point(65, 22);
            this.cmbArtigo.Size = new System.Drawing.Size(200, 23);

            this.lblObservacoes.Text = "Observações:";
            this.lblObservacoes.Location = new System.Drawing.Point(280, 25);
            this.lblObservacoes.Size = new System.Drawing.Size(80, 20);

            this.tbObservacoes.Location = new System.Drawing.Point(365, 22);
            this.tbObservacoes.Size = new System.Drawing.Size(200, 23);

            this.btnAdicionarNaoPrevisto.Text = "Adicionar";
            this.btnAdicionarNaoPrevisto.Location = new System.Drawing.Point(580, 20);
            this.btnAdicionarNaoPrevisto.Size = new System.Drawing.Size(80, 28);
            this.btnAdicionarNaoPrevisto.Click += new System.EventHandler(this.btnAdicionarNaoPrevisto_Click);

            this.gbNaoPrevisto.Controls.Add(this.lblArtigo);
            this.gbNaoPrevisto.Controls.Add(this.cmbArtigo);
            this.gbNaoPrevisto.Controls.Add(this.lblObservacoes);
            this.gbNaoPrevisto.Controls.Add(this.tbObservacoes);
            this.gbNaoPrevisto.Controls.Add(this.btnAdicionarNaoPrevisto);

            // Título
            this.lblTituloCompra.Location = new System.Drawing.Point(12, 15);
            this.lblTituloCompra.Size = new System.Drawing.Size(500, 30);
            this.lblTituloCompra.Font = new System.Drawing.Font("Arial", 13, System.Drawing.FontStyle.Bold);
            this.lblTituloCompra.Text = "Modo Compra";

            // Botão Fechar Compra
            this.btnFecharCompra.Text = "Fechar Compra";
            this.btnFecharCompra.Location = new System.Drawing.Point(630, 495);
            this.btnFecharCompra.Size = new System.Drawing.Size(140, 35);
            this.btnFecharCompra.BackColor = System.Drawing.Color.DarkRed;
            this.btnFecharCompra.ForeColor = System.Drawing.Color.White;
            this.btnFecharCompra.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            this.btnFecharCompra.Click += new System.EventHandler(this.btnFecharCompra_Click);

            // Form
            this.Text = "Modo Compra";
            this.Size = new System.Drawing.Size(800, 580);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormModoCompra_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModoCompra_FormClosing);

            this.Controls.Add(this.lblTituloCompra);
            this.Controls.Add(this.dgvItens);
            this.Controls.Add(this.lblOrcamentoMax);
            this.Controls.Add(this.lblTotalGasto);
            this.Controls.Add(this.lblDisponivel);
            this.Controls.Add(this.lblAlerta);
            this.Controls.Add(this.gbNaoPrevisto);
            this.Controls.Add(this.btnFecharCompra);
        }
    }
}