namespace Projeto_DA_MDS.Views
{
    partial class FormEstatisticas
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnExportarCSV;

        // Tab 1
        private System.Windows.Forms.DataGridView dgvOrcamentoVsTotal;
        private System.Windows.Forms.DataGridView dgvComprasFechadas;
        private System.Windows.Forms.Label lblTituloOrcamento;
        private System.Windows.Forms.Label lblTituloCompras;

        // Tab 2
        private System.Windows.Forms.Label lblMediaGastos;
        private System.Windows.Forms.Label lblSugestaoOrcamento;
        private System.Windows.Forms.Label lblOrcamentoAtual;
        private System.Windows.Forms.DataGridView dgvSugestaoSemana;
        private System.Windows.Forms.Label lblTituloSugestao;
        private System.Windows.Forms.Label lblTituloSemana;
        private System.Windows.Forms.Label lblSemSugestao;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvOrcamentoVsTotal = new System.Windows.Forms.DataGridView();
            this.dgvComprasFechadas = new System.Windows.Forms.DataGridView();
            this.lblTituloOrcamento = new System.Windows.Forms.Label();
            this.lblTituloCompras = new System.Windows.Forms.Label();
            this.lblMediaGastos = new System.Windows.Forms.Label();
            this.lblSugestaoOrcamento = new System.Windows.Forms.Label();
            this.lblOrcamentoAtual = new System.Windows.Forms.Label();
            this.dgvSugestaoSemana = new System.Windows.Forms.DataGridView();
            this.lblTituloSugestao = new System.Windows.Forms.Label();
            this.lblTituloSemana = new System.Windows.Forms.Label();
            this.lblSemSugestao = new System.Windows.Forms.Label();

            // ── TAB CONTROL ──────────────────────────────────────────────────
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Size = new System.Drawing.Size(860, 560);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);

            this.tabPage1.Text = "Orçamento & Compras";
            this.tabPage2.Text = "Sugestões";

            // ── TAB 1 ─────────────────────────────────────────────────────────

            // Título Orçamento vs Total
            this.lblTituloOrcamento.Text = "Orçamento vs Total Gasto por Mês";
            this.lblTituloOrcamento.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblTituloOrcamento.Location = new System.Drawing.Point(10, 10);
            this.lblTituloOrcamento.Size = new System.Drawing.Size(400, 22);

            // DataGridView Orçamento vs Total
            this.dgvOrcamentoVsTotal.Location = new System.Drawing.Point(10, 38);
            this.dgvOrcamentoVsTotal.Size = new System.Drawing.Size(820, 180);
            this.dgvOrcamentoVsTotal.AllowUserToAddRows = false;
            this.dgvOrcamentoVsTotal.ReadOnly = true;
            this.dgvOrcamentoVsTotal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrcamentoVsTotal.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colMes", HeaderText = "Mês/Ano" });
            this.dgvOrcamentoVsTotal.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colOrcMax", HeaderText = "Orçamento Máx." });
            this.dgvOrcamentoVsTotal.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colTotalGasto", HeaderText = "Total Gasto" });
            this.dgvOrcamentoVsTotal.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colDiferenca", HeaderText = "Diferença" });
            this.dgvOrcamentoVsTotal.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado" });

            // Título Compras Fechadas
            this.lblTituloCompras.Text = "Compras Fechadas — % Previstos vs Não Previstos";
            this.lblTituloCompras.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblTituloCompras.Location = new System.Drawing.Point(10, 230);
            this.lblTituloCompras.Size = new System.Drawing.Size(500, 22);

            // DataGridView Compras Fechadas
            this.dgvComprasFechadas.Location = new System.Drawing.Point(10, 258);
            this.dgvComprasFechadas.Size = new System.Drawing.Size(820, 210);
            this.dgvComprasFechadas.AllowUserToAddRows = false;
            this.dgvComprasFechadas.ReadOnly = true;
            this.dgvComprasFechadas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colNome", HeaderText = "Lista" });
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colDataFecho", HeaderText = "Data Fecho" });
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colTotal", HeaderText = "Total Itens" });
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colPrevistos", HeaderText = "Previstos" });
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colNaoPrevistos", HeaderText = "Não Previstos" });
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colPctPrev", HeaderText = "% Prev." });
            this.dgvComprasFechadas.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colPctNaoPrev", HeaderText = "% N. Prev." });

            this.tabPage1.Controls.Add(this.lblTituloOrcamento);
            this.tabPage1.Controls.Add(this.dgvOrcamentoVsTotal);
            this.tabPage1.Controls.Add(this.lblTituloCompras);
            this.tabPage1.Controls.Add(this.dgvComprasFechadas);

            // ── TAB 2 ─────────────────────────────────────────────────────────

            // Sugestão Orçamento
            this.lblTituloSugestao.Text = "Sugestão de Orçamento para o Próximo Mês";
            this.lblTituloSugestao.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblTituloSugestao.Location = new System.Drawing.Point(10, 10);
            this.lblTituloSugestao.Size = new System.Drawing.Size(500, 22);

            this.lblMediaGastos.Location = new System.Drawing.Point(10, 42);
            this.lblMediaGastos.Size = new System.Drawing.Size(500, 22);
            this.lblMediaGastos.Text = "A calcular...";

            this.lblOrcamentoAtual.Location = new System.Drawing.Point(10, 68);
            this.lblOrcamentoAtual.Size = new System.Drawing.Size(500, 22);
            this.lblOrcamentoAtual.Text = "";

            this.lblSugestaoOrcamento.Location = new System.Drawing.Point(10, 94);
            this.lblSugestaoOrcamento.Size = new System.Drawing.Size(600, 22);
            this.lblSugestaoOrcamento.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            this.lblSugestaoOrcamento.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblSugestaoOrcamento.Text = "";

            // Sugestão Lista por Semana
            this.lblTituloSemana.Text = "Sugestão de Lista por Semana do Mês (baseado em compras anteriores)";
            this.lblTituloSemana.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            this.lblTituloSemana.Location = new System.Drawing.Point(10, 130);
            this.lblTituloSemana.Size = new System.Drawing.Size(700, 22);

            this.dgvSugestaoSemana.Location = new System.Drawing.Point(10, 158);
            this.dgvSugestaoSemana.Size = new System.Drawing.Size(820, 300);
            this.dgvSugestaoSemana.AllowUserToAddRows = false;
            this.dgvSugestaoSemana.ReadOnly = true;
            this.dgvSugestaoSemana.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSugestaoSemana.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colSemana", HeaderText = "Semana" });
            this.dgvSugestaoSemana.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colArtigo", HeaderText = "Artigo" });
            this.dgvSugestaoSemana.Columns.Add(new System.Windows.Forms.DataGridViewTextBoxColumn { Name = "colQtd", HeaderText = "Qtd Habitual" });

            this.lblSemSugestao.Text = "Sem dados suficientes para sugestões.";
            this.lblSemSugestao.Location = new System.Drawing.Point(10, 465);
            this.lblSemSugestao.Size = new System.Drawing.Size(400, 22);
            this.lblSemSugestao.ForeColor = System.Drawing.Color.Gray;
            this.lblSemSugestao.Visible = false;

            this.tabPage2.Controls.Add(this.lblTituloSugestao);
            this.tabPage2.Controls.Add(this.lblMediaGastos);
            this.tabPage2.Controls.Add(this.lblOrcamentoAtual);
            this.tabPage2.Controls.Add(this.lblSugestaoOrcamento);
            this.tabPage2.Controls.Add(this.lblTituloSemana);
            this.tabPage2.Controls.Add(this.dgvSugestaoSemana);
            this.tabPage2.Controls.Add(this.lblSemSugestao);

            // ── FORM ──────────────────────────────────────────────────────────
            this.Text = "Estatísticas";
            this.Size = new System.Drawing.Size(880, 650);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormEstatisticas_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEstatisticas_FormClosing);

            this.btnExportarCSV = new System.Windows.Forms.Button();
            this.btnExportarCSV.Text = "Exportar CSV";
            this.btnExportarCSV.Location = new System.Drawing.Point(700, 565);
            this.btnExportarCSV.Size = new System.Drawing.Size(130, 35);
            this.btnExportarCSV.BackColor = System.Drawing.Color.DarkGreen;
            this.btnExportarCSV.ForeColor = System.Drawing.Color.White;
            this.btnExportarCSV.Font = new System.Drawing.Font("Arial", 9, System.Drawing.FontStyle.Bold);
            this.btnExportarCSV.Click += new System.EventHandler(this.btnExportarCSV_Click);
            this.Controls.Add(this.btnExportarCSV);

            this.Controls.Add(this.tabControl);
        }
    }
}