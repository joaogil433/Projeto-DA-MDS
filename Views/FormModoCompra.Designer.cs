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
        private System.Windows.Forms.Panel panelTopo;

        // Colunas do DataGridView declaradas como campos
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArtigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtdPrevista;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQtdAdquirida;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPreco;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObs;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvItens = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArtigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtdPrevista = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQtdAdquirida = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPreco = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObs = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.panelTopo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.gbNaoPrevisto.SuspendLayout();
            this.panelTopo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            this.dgvItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colArtigo,
            this.colQtdPrevista,
            this.colQtdAdquirida,
            this.colPreco,
            this.colTipo,
            this.colObs});
            this.dgvItens.Location = new System.Drawing.Point(12, 85);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.Size = new System.Drawing.Size(760, 260);
            this.dgvItens.TabIndex = 1;
            this.dgvItens.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItens_CellEndEdit);
            // 
            // colId
            // 
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colArtigo
            // 
            this.colArtigo.HeaderText = "Artigo";
            this.colArtigo.Name = "colArtigo";
            this.colArtigo.ReadOnly = true;
            this.colArtigo.Width = 180;
            // 
            // colQtdPrevista
            // 
            this.colQtdPrevista.HeaderText = "Qtd Prevista";
            this.colQtdPrevista.Name = "colQtdPrevista";
            this.colQtdPrevista.ReadOnly = true;
            this.colQtdPrevista.Width = 90;
            // 
            // colQtdAdquirida
            // 
            this.colQtdAdquirida.HeaderText = "Qtd Adquirida";
            this.colQtdAdquirida.Name = "colQtdAdquirida";
            // 
            // colPreco
            // 
            this.colPreco.HeaderText = "Preço Unit. (€)";
            this.colPreco.Name = "colPreco";
            this.colPreco.Width = 110;
            // 
            // colTipo
            // 
            this.colTipo.HeaderText = "Tipo";
            this.colTipo.Name = "colTipo";
            this.colTipo.ReadOnly = true;
            // 
            // colObs
            // 
            this.colObs.HeaderText = "Observações";
            this.colObs.Name = "colObs";
            this.colObs.ReadOnly = true;
            this.colObs.Width = 160;
            // 
            // lblOrcamentoMax
            // 
            this.lblOrcamentoMax.Location = new System.Drawing.Point(12, 360);
            this.lblOrcamentoMax.Name = "lblOrcamentoMax";
            this.lblOrcamentoMax.Size = new System.Drawing.Size(200, 20);
            this.lblOrcamentoMax.TabIndex = 2;
            // 
            // lblTotalGasto
            // 
            this.lblTotalGasto.Location = new System.Drawing.Point(220, 360);
            this.lblTotalGasto.Name = "lblTotalGasto";
            this.lblTotalGasto.Size = new System.Drawing.Size(200, 20);
            this.lblTotalGasto.TabIndex = 3;
            // 
            // lblDisponivel
            // 
            this.lblDisponivel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lblDisponivel.Location = new System.Drawing.Point(430, 360);
            this.lblDisponivel.Name = "lblDisponivel";
            this.lblDisponivel.Size = new System.Drawing.Size(200, 20);
            this.lblDisponivel.TabIndex = 4;
            // 
            // lblAlerta
            // 
            this.lblAlerta.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.lblAlerta.ForeColor = System.Drawing.Color.Red;
            this.lblAlerta.Location = new System.Drawing.Point(640, 355);
            this.lblAlerta.Name = "lblAlerta";
            this.lblAlerta.Size = new System.Drawing.Size(130, 30);
            this.lblAlerta.TabIndex = 5;
            this.lblAlerta.Text = "⚠ Orçamento ultrapassado!";
            this.lblAlerta.Visible = false;
            // 
            // cmbArtigo
            // 
            this.cmbArtigo.Location = new System.Drawing.Point(65, 27);
            this.cmbArtigo.Name = "cmbArtigo";
            this.cmbArtigo.Size = new System.Drawing.Size(200, 21);
            this.cmbArtigo.TabIndex = 1;
            // 
            // tbObservacoes
            // 
            this.tbObservacoes.Location = new System.Drawing.Point(366, 28);
            this.tbObservacoes.Name = "tbObservacoes";
            this.tbObservacoes.Size = new System.Drawing.Size(200, 20);
            this.tbObservacoes.TabIndex = 3;
            // 
            // btnAdicionarNaoPrevisto
            // 
            this.btnAdicionarNaoPrevisto.Location = new System.Drawing.Point(631, 27);
            this.btnAdicionarNaoPrevisto.Name = "btnAdicionarNaoPrevisto";
            this.btnAdicionarNaoPrevisto.Size = new System.Drawing.Size(80, 28);
            this.btnAdicionarNaoPrevisto.TabIndex = 4;
            this.btnAdicionarNaoPrevisto.Text = "Adicionar";
            this.btnAdicionarNaoPrevisto.Click += new System.EventHandler(this.btnAdicionarNaoPrevisto_Click);
            // 
            // btnFecharCompra
            // 
            this.btnFecharCompra.BackColor = System.Drawing.Color.DarkRed;
            this.btnFecharCompra.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.btnFecharCompra.ForeColor = System.Drawing.Color.White;
            this.btnFecharCompra.Location = new System.Drawing.Point(630, 495);
            this.btnFecharCompra.Name = "btnFecharCompra";
            this.btnFecharCompra.Size = new System.Drawing.Size(140, 35);
            this.btnFecharCompra.TabIndex = 7;
            this.btnFecharCompra.Text = "Fechar Compra";
            this.btnFecharCompra.UseVisualStyleBackColor = false;
            this.btnFecharCompra.Click += new System.EventHandler(this.btnFecharCompra_Click);
            // 
            // lblTituloCompra
            // 
            this.lblTituloCompra.AutoSize = true;
            this.lblTituloCompra.Font = new System.Drawing.Font("Segoe UI", 13.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTituloCompra.ForeColor = System.Drawing.Color.White;
            this.lblTituloCompra.Location = new System.Drawing.Point(320, 22);
            this.lblTituloCompra.Name = "lblTituloCompra";
            this.lblTituloCompra.Size = new System.Drawing.Size(137, 25);
            this.lblTituloCompra.TabIndex = 0;
            this.lblTituloCompra.Text = "Modo Compra";
            // 
            // gbNaoPrevisto
            // 
            this.gbNaoPrevisto.Controls.Add(this.lblArtigo);
            this.gbNaoPrevisto.Controls.Add(this.cmbArtigo);
            this.gbNaoPrevisto.Controls.Add(this.lblObservacoes);
            this.gbNaoPrevisto.Controls.Add(this.tbObservacoes);
            this.gbNaoPrevisto.Controls.Add(this.btnAdicionarNaoPrevisto);
            this.gbNaoPrevisto.Location = new System.Drawing.Point(12, 395);
            this.gbNaoPrevisto.Name = "gbNaoPrevisto";
            this.gbNaoPrevisto.Size = new System.Drawing.Size(760, 90);
            this.gbNaoPrevisto.TabIndex = 6;
            this.gbNaoPrevisto.TabStop = false;
            this.gbNaoPrevisto.Text = "Adicionar Item Não Previsto";
            // 
            // lblArtigo
            // 
            this.lblArtigo.Location = new System.Drawing.Point(10, 30);
            this.lblArtigo.Name = "lblArtigo";
            this.lblArtigo.Size = new System.Drawing.Size(50, 20);
            this.lblArtigo.TabIndex = 0;
            this.lblArtigo.Text = "Artigo:";
            // 
            // lblObservacoes
            // 
            this.lblObservacoes.Location = new System.Drawing.Point(280, 30);
            this.lblObservacoes.Name = "lblObservacoes";
            this.lblObservacoes.Size = new System.Drawing.Size(80, 20);
            this.lblObservacoes.TabIndex = 2;
            this.lblObservacoes.Text = "Observações:";
            // 
            // panelTopo
            // 
            this.panelTopo.BackColor = System.Drawing.Color.SteelBlue;
            this.panelTopo.Controls.Add(this.lblTituloCompra);
            this.panelTopo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopo.Location = new System.Drawing.Point(0, 0);
            this.panelTopo.Name = "panelTopo";
            this.panelTopo.Size = new System.Drawing.Size(784, 70);
            this.panelTopo.TabIndex = 0;
            // 
            // FormModoCompra
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 541);
            this.Controls.Add(this.panelTopo);
            this.Controls.Add(this.dgvItens);
            this.Controls.Add(this.lblOrcamentoMax);
            this.Controls.Add(this.lblTotalGasto);
            this.Controls.Add(this.lblDisponivel);
            this.Controls.Add(this.lblAlerta);
            this.Controls.Add(this.gbNaoPrevisto);
            this.Controls.Add(this.btnFecharCompra);
            this.Name = "FormModoCompra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modo Compra";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormModoCompra_FormClosing);
            this.Load += new System.EventHandler(this.FormModoCompra_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.gbNaoPrevisto.ResumeLayout(false);
            this.gbNaoPrevisto.PerformLayout();
            this.panelTopo.ResumeLayout(false);
            this.panelTopo.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}