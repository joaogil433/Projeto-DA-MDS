namespace Projeto_DA_MDS.Views
{
    partial class FormGestaoOrcamentos
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
            this.panelFormulario = new System.Windows.Forms.Panel();
            this.labelGestaoOrcamentos = new System.Windows.Forms.Label();
            this.dgvOrcamentos = new System.Windows.Forms.DataGridView();
            this.labelMes = new System.Windows.Forms.Label();
            this.labelAno = new System.Windows.Forms.Label();
            this.labelValorMaximo = new System.Windows.Forms.Label();
            this.nudMes = new System.Windows.Forms.NumericUpDown();
            this.nudAno = new System.Windows.Forms.NumericUpDown();
            this.nudValor = new System.Windows.Forms.NumericUpDown();
            this.lblCriadoPor = new System.Windows.Forms.Label();
            this.lblAlteradoPor = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnNovo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.panelFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrcamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValor)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFormulario
            // 
            this.panelFormulario.BackColor = System.Drawing.Color.SteelBlue;
            this.panelFormulario.Controls.Add(this.labelGestaoOrcamentos);
            this.panelFormulario.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFormulario.Location = new System.Drawing.Point(0, 0);
            this.panelFormulario.Name = "panelFormulario";
            this.panelFormulario.Size = new System.Drawing.Size(800, 70);
            this.panelFormulario.TabIndex = 1;
            // 
            // labelGestaoOrcamentos
            // 
            this.labelGestaoOrcamentos.AutoSize = true;
            this.labelGestaoOrcamentos.Font = new System.Drawing.Font("Segoe UI", 13.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGestaoOrcamentos.ForeColor = System.Drawing.Color.White;
            this.labelGestaoOrcamentos.Location = new System.Drawing.Point(278, 20);
            this.labelGestaoOrcamentos.Name = "labelGestaoOrcamentos";
            this.labelGestaoOrcamentos.Size = new System.Drawing.Size(244, 30);
            this.labelGestaoOrcamentos.TabIndex = 1;
            this.labelGestaoOrcamentos.Text = "Gestão de Orçamentos";
            this.labelGestaoOrcamentos.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // dgvOrcamentos
            // 
            this.dgvOrcamentos.AllowUserToAddRows = false;
            this.dgvOrcamentos.ColumnHeadersHeight = 29;
            this.dgvOrcamentos.Location = new System.Drawing.Point(121, 84);
            this.dgvOrcamentos.MultiSelect = false;
            this.dgvOrcamentos.Name = "dgvOrcamentos";
            this.dgvOrcamentos.ReadOnly = true;
            this.dgvOrcamentos.RowHeadersWidth = 51;
            this.dgvOrcamentos.RowTemplate.Height = 24;
            this.dgvOrcamentos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrcamentos.Size = new System.Drawing.Size(486, 232);
            this.dgvOrcamentos.TabIndex = 2;
            // 
            // labelMes
            // 
            this.labelMes.AutoSize = true;
            this.labelMes.Location = new System.Drawing.Point(198, 336);
            this.labelMes.Name = "labelMes";
            this.labelMes.Size = new System.Drawing.Size(33, 16);
            this.labelMes.TabIndex = 3;
            this.labelMes.Text = "Mês";
            // 
            // labelAno
            // 
            this.labelAno.AutoSize = true;
            this.labelAno.Location = new System.Drawing.Point(198, 371);
            this.labelAno.Name = "labelAno";
            this.labelAno.Size = new System.Drawing.Size(31, 16);
            this.labelAno.TabIndex = 4;
            this.labelAno.Text = "Ano";
            // 
            // labelValorMaximo
            // 
            this.labelValorMaximo.AutoSize = true;
            this.labelValorMaximo.Location = new System.Drawing.Point(174, 407);
            this.labelValorMaximo.Name = "labelValorMaximo";
            this.labelValorMaximo.Size = new System.Drawing.Size(57, 16);
            this.labelValorMaximo.TabIndex = 5;
            this.labelValorMaximo.Text = "Valor (€)";
            // 
            // nudMes
            // 
            this.nudMes.Location = new System.Drawing.Point(247, 334);
            this.nudMes.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudMes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMes.Name = "nudMes";
            this.nudMes.Size = new System.Drawing.Size(120, 22);
            this.nudMes.TabIndex = 6;
            this.nudMes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudAno
            // 
            this.nudAno.Location = new System.Drawing.Point(247, 369);
            this.nudAno.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.nudAno.Minimum = new decimal(new int[] {
            2026,
            0,
            0,
            0});
            this.nudAno.Name = "nudAno";
            this.nudAno.Size = new System.Drawing.Size(120, 22);
            this.nudAno.TabIndex = 7;
            this.nudAno.Value = new decimal(new int[] {
            2026,
            0,
            0,
            0});
            // 
            // nudValor
            // 
            this.nudValor.DecimalPlaces = 2;
            this.nudValor.Location = new System.Drawing.Point(247, 405);
            this.nudValor.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudValor.Name = "nudValor";
            this.nudValor.Size = new System.Drawing.Size(120, 22);
            this.nudValor.TabIndex = 8;
            // 
            // lblCriadoPor
            // 
            this.lblCriadoPor.AutoSize = true;
            this.lblCriadoPor.Location = new System.Drawing.Point(522, 371);
            this.lblCriadoPor.Name = "lblCriadoPor";
            this.lblCriadoPor.Size = new System.Drawing.Size(0, 16);
            this.lblCriadoPor.TabIndex = 9;
            // 
            // lblAlteradoPor
            // 
            this.lblAlteradoPor.AutoSize = true;
            this.lblAlteradoPor.Location = new System.Drawing.Point(522, 340);
            this.lblAlteradoPor.Name = "lblAlteradoPor";
            this.lblAlteradoPor.Size = new System.Drawing.Size(0, 16);
            this.lblAlteradoPor.TabIndex = 10;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(522, 407);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 11;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(400, 349);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(81, 27);
            this.btnGuardar.TabIndex = 12;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(400, 382);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(81, 27);
            this.btnCancelar.TabIndex = 13;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnNovo
            // 
            this.btnNovo.Location = new System.Drawing.Point(636, 137);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(81, 27);
            this.btnNovo.TabIndex = 14;
            this.btnNovo.Text = "Novo";
            this.btnNovo.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(636, 185);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(81, 27);
            this.btnEditar.TabIndex = 15;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(636, 240);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(81, 27);
            this.btnEliminar.TabIndex = 16;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // FormGestaoOrcamentos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNovo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblAlteradoPor);
            this.Controls.Add(this.lblCriadoPor);
            this.Controls.Add(this.nudValor);
            this.Controls.Add(this.nudAno);
            this.Controls.Add(this.nudMes);
            this.Controls.Add(this.labelValorMaximo);
            this.Controls.Add(this.labelAno);
            this.Controls.Add(this.labelMes);
            this.Controls.Add(this.dgvOrcamentos);
            this.Controls.Add(this.panelFormulario);
            this.Name = "FormGestaoOrcamentos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormGestaoOrcamentos";
            this.panelFormulario.ResumeLayout(false);
            this.panelFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrcamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudValor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFormulario;
        private System.Windows.Forms.Label labelGestaoOrcamentos;
        private System.Windows.Forms.DataGridView dgvOrcamentos;
        private System.Windows.Forms.Label labelMes;
        private System.Windows.Forms.Label labelAno;
        private System.Windows.Forms.Label labelValorMaximo;
        private System.Windows.Forms.NumericUpDown nudMes;
        private System.Windows.Forms.NumericUpDown nudAno;
        private System.Windows.Forms.NumericUpDown nudValor;
        private System.Windows.Forms.Label lblCriadoPor;
        private System.Windows.Forms.Label lblAlteradoPor;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnNovo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
    }
}