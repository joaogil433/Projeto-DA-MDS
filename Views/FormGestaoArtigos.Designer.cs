namespace Projeto_DA_MDS.Views
{
    partial class FormGestaoArtigos
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
            this.labelGestaoArtigos = new System.Windows.Forms.Label();
            this.labelFiltroTipo = new System.Windows.Forms.Label();
            this.cmbFiltroTipo = new System.Windows.Forms.ComboBox();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.dgvArtigos = new System.Windows.Forms.DataGridView();
            this.btnNovo = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.cmbTipoArtigo = new System.Windows.Forms.ComboBox();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelFormularioGestaoArtigos = new System.Windows.Forms.Panel();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.panelFormulario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArtigos)).BeginInit();
            this.panelFormularioGestaoArtigos.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFormulario
            // 
            this.panelFormulario.BackColor = System.Drawing.Color.SteelBlue;
            this.panelFormulario.Controls.Add(this.labelGestaoArtigos);
            this.panelFormulario.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFormulario.Location = new System.Drawing.Point(0, 0);
            this.panelFormulario.Name = "panelFormulario";
            this.panelFormulario.Size = new System.Drawing.Size(800, 70);
            this.panelFormulario.TabIndex = 2;
            // 
            // labelGestaoArtigos
            // 
            this.labelGestaoArtigos.AutoSize = true;
            this.labelGestaoArtigos.Font = new System.Drawing.Font("Segoe UI", 13.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGestaoArtigos.ForeColor = System.Drawing.Color.White;
            this.labelGestaoArtigos.Location = new System.Drawing.Point(303, 20);
            this.labelGestaoArtigos.Name = "labelGestaoArtigos";
            this.labelGestaoArtigos.Size = new System.Drawing.Size(195, 30);
            this.labelGestaoArtigos.TabIndex = 1;
            this.labelGestaoArtigos.Text = "Gestão de Artigos";
            this.labelGestaoArtigos.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelFiltroTipo
            // 
            this.labelFiltroTipo.AutoSize = true;
            this.labelFiltroTipo.Location = new System.Drawing.Point(189, 93);
            this.labelFiltroTipo.Name = "labelFiltroTipo";
            this.labelFiltroTipo.Size = new System.Drawing.Size(97, 16);
            this.labelFiltroTipo.TabIndex = 3;
            this.labelFiltroTipo.Text = "Filtrar por Tipo:";
            // 
            // cmbFiltroTipo
            // 
            this.cmbFiltroTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroTipo.FormattingEnabled = true;
            this.cmbFiltroTipo.Location = new System.Drawing.Point(326, 90);
            this.cmbFiltroTipo.Name = "cmbFiltroTipo";
            this.cmbFiltroTipo.Size = new System.Drawing.Size(131, 24);
            this.cmbFiltroTipo.TabIndex = 4;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.Location = new System.Drawing.Point(518, 85);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Size = new System.Drawing.Size(93, 33);
            this.btnFiltrar.TabIndex = 5;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.UseVisualStyleBackColor = true;
            // 
            // dgvArtigos
            // 
            this.dgvArtigos.AllowUserToAddRows = false;
            this.dgvArtigos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArtigos.Location = new System.Drawing.Point(196, 143);
            this.dgvArtigos.MultiSelect = false;
            this.dgvArtigos.Name = "dgvArtigos";
            this.dgvArtigos.ReadOnly = true;
            this.dgvArtigos.RowHeadersWidth = 51;
            this.dgvArtigos.RowTemplate.Height = 24;
            this.dgvArtigos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvArtigos.Size = new System.Drawing.Size(408, 174);
            this.dgvArtigos.TabIndex = 6;
            // 
            // btnNovo
            // 
            this.btnNovo.Location = new System.Drawing.Point(631, 143);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(83, 35);
            this.btnNovo.TabIndex = 7;
            this.btnNovo.Text = "Novo";
            this.btnNovo.UseVisualStyleBackColor = true;
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(631, 214);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(83, 35);
            this.btnEditar.TabIndex = 10;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(631, 282);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(83, 35);
            this.btnEliminar.TabIndex = 11;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Nome";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "Tipo de Artigo";
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(104, 3);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(194, 22);
            this.txtNome.TabIndex = 14;
            // 
            // cmbTipoArtigo
            // 
            this.cmbTipoArtigo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoArtigo.FormattingEnabled = true;
            this.cmbTipoArtigo.Location = new System.Drawing.Point(104, 44);
            this.cmbTipoArtigo.Name = "cmbTipoArtigo";
            this.cmbTipoArtigo.Size = new System.Drawing.Size(129, 24);
            this.cmbTipoArtigo.TabIndex = 15;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(319, 44);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(87, 35);
            this.btnCancelar.TabIndex = 16;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(319, 3);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(87, 35);
            this.btnGuardar.TabIndex = 17;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.lblStatus.Location = new System.Drawing.Point(299, 416);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 16);
            this.lblStatus.TabIndex = 19;
            // 
            // panelFormularioGestaoArtigos
            // 
            this.panelFormularioGestaoArtigos.Controls.Add(this.btnGuardar);
            this.panelFormularioGestaoArtigos.Controls.Add(this.btnCancelar);
            this.panelFormularioGestaoArtigos.Controls.Add(this.cmbTipoArtigo);
            this.panelFormularioGestaoArtigos.Controls.Add(this.txtNome);
            this.panelFormularioGestaoArtigos.Controls.Add(this.label2);
            this.panelFormularioGestaoArtigos.Controls.Add(this.label1);
            this.panelFormularioGestaoArtigos.Location = new System.Drawing.Point(192, 331);
            this.panelFormularioGestaoArtigos.Name = "panelFormularioGestaoArtigos";
            this.panelFormularioGestaoArtigos.Size = new System.Drawing.Size(433, 101);
            this.panelFormularioGestaoArtigos.TabIndex = 20;
            // 
            // btnVoltar
            // 
            this.btnVoltar.Location = new System.Drawing.Point(708, 409);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(80, 31);
            this.btnVoltar.TabIndex = 21;
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = true;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // FormGestaoArtigos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnVoltar);
            this.Controls.Add(this.panelFormularioGestaoArtigos);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnNovo);
            this.Controls.Add(this.dgvArtigos);
            this.Controls.Add(this.btnFiltrar);
            this.Controls.Add(this.cmbFiltroTipo);
            this.Controls.Add(this.labelFiltroTipo);
            this.Controls.Add(this.panelFormulario);
            this.Name = "FormGestaoArtigos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormGestaoArtigos";
            this.panelFormulario.ResumeLayout(false);
            this.panelFormulario.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArtigos)).EndInit();
            this.panelFormularioGestaoArtigos.ResumeLayout(false);
            this.panelFormularioGestaoArtigos.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFormulario;
        private System.Windows.Forms.Label labelGestaoArtigos;
        private System.Windows.Forms.Label labelFiltroTipo;
        private System.Windows.Forms.ComboBox cmbFiltroTipo;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.DataGridView dgvArtigos;
        private System.Windows.Forms.Button btnNovo;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.ComboBox cmbTipoArtigo;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelFormularioGestaoArtigos;
        private System.Windows.Forms.Button btnVoltar;
    }
}