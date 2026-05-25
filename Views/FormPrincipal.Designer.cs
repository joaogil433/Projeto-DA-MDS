namespace Projeto_DA_MDS.Views
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gestãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utilizadoresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.artigosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orçamentosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.planeamentoDeComprasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.estatísticasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblUtilizador = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvComprasAbertas = new System.Windows.Forms.DataGridView();
            this.btnAbrirModoCompra = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprasAbertas)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestãoToolStripMenuItem,
            this.estatísticasToolStripMenuItem,
            this.sairToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(496, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gestãoToolStripMenuItem
            // 
            this.gestãoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.utilizadoresToolStripMenuItem,
            this.artigosToolStripMenuItem,
            this.orçamentosToolStripMenuItem,
            this.toolStripMenuItem2,
            this.planeamentoDeComprasToolStripMenuItem});
            this.gestãoToolStripMenuItem.Name = "gestãoToolStripMenuItem";
            this.gestãoToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.gestãoToolStripMenuItem.Text = "Gestão ";
            // 
            // utilizadoresToolStripMenuItem
            // 
            this.utilizadoresToolStripMenuItem.Name = "utilizadoresToolStripMenuItem";
            this.utilizadoresToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.utilizadoresToolStripMenuItem.Text = "Utilizadores";
            this.utilizadoresToolStripMenuItem.Click += new System.EventHandler(this.utilizadoresToolStripMenuItem_Click);
            // 
            // artigosToolStripMenuItem
            // 
            this.artigosToolStripMenuItem.Name = "artigosToolStripMenuItem";
            this.artigosToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.artigosToolStripMenuItem.Text = "Artigos";
            this.artigosToolStripMenuItem.Click += new System.EventHandler(this.artigosToolStripMenuItem_Click);
            // 
            // orçamentosToolStripMenuItem
            // 
            this.orçamentosToolStripMenuItem.Name = "orçamentosToolStripMenuItem";
            this.orçamentosToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.orçamentosToolStripMenuItem.Text = "Orçamentos";
            this.orçamentosToolStripMenuItem.Click += new System.EventHandler(this.orcamentosToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(263, 26);
            this.toolStripMenuItem2.Text = "─────────────";
            // 
            // planeamentoDeComprasToolStripMenuItem
            // 
            this.planeamentoDeComprasToolStripMenuItem.Name = "planeamentoDeComprasToolStripMenuItem";
            this.planeamentoDeComprasToolStripMenuItem.Size = new System.Drawing.Size(263, 26);
            this.planeamentoDeComprasToolStripMenuItem.Text = "Planeamento de Compras";
            this.planeamentoDeComprasToolStripMenuItem.Click += new System.EventHandler(this.planeamentoDeComprasToolStripMenuItem_Click);
            // 
            // estatísticasToolStripMenuItem
            // 
            this.estatísticasToolStripMenuItem.Name = "estatísticasToolStripMenuItem";
            this.estatísticasToolStripMenuItem.Size = new System.Drawing.Size(99, 24);
            this.estatísticasToolStripMenuItem.Text = "Estatísticas ";
            this.estatísticasToolStripMenuItem.Click += new System.EventHandler(this.estatisticasToolStripMenuItem_Click);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(52, 24);
            this.sairToolStripMenuItem.Text = "Sair ";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // lblUtilizador
            // 
            this.lblUtilizador.AutoSize = true;
            this.lblUtilizador.Location = new System.Drawing.Point(34, 87);
            this.lblUtilizador.Name = "lblUtilizador";
            this.lblUtilizador.Size = new System.Drawing.Size(0, 16);
            this.lblUtilizador.TabIndex = 4;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Location = new System.Drawing.Point(34, 134);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(127, 16);
            this.lblTitulo.TabIndex = 5;
            this.lblTitulo.Text = "Compras em Aberto";
            // 
            // dgvComprasAbertas
            // 
            this.dgvComprasAbertas.AllowUserToAddRows = false;
            this.dgvComprasAbertas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComprasAbertas.Location = new System.Drawing.Point(37, 183);
            this.dgvComprasAbertas.MultiSelect = false;
            this.dgvComprasAbertas.Name = "dgvComprasAbertas";
            this.dgvComprasAbertas.ReadOnly = true;
            this.dgvComprasAbertas.RowHeadersWidth = 51;
            this.dgvComprasAbertas.RowTemplate.Height = 24;
            this.dgvComprasAbertas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComprasAbertas.Size = new System.Drawing.Size(386, 153);
            this.dgvComprasAbertas.TabIndex = 6;
            // 
            // btnAbrirModoCompra
            // 
            this.btnAbrirModoCompra.Location = new System.Drawing.Point(36, 369);
            this.btnAbrirModoCompra.Name = "btnAbrirModoCompra";
            this.btnAbrirModoCompra.Size = new System.Drawing.Size(151, 38);
            this.btnAbrirModoCompra.TabIndex = 7;
            this.btnAbrirModoCompra.Text = "Abrir Modo Compra";
            this.btnAbrirModoCompra.UseVisualStyleBackColor = false;
            this.btnAbrirModoCompra.Click += new System.EventHandler(this.btnAbrirModoCompra_Click);
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 450);
            this.Controls.Add(this.btnAbrirModoCompra);
            this.Controls.Add(this.dgvComprasAbertas);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblUtilizador);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPrincipal";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComprasAbertas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gestãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem utilizadoresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem artigosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orçamentosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem planeamentoDeComprasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem estatísticasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.Label lblUtilizador;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvComprasAbertas;
        private System.Windows.Forms.Button btnAbrirModoCompra;
    }
}