namespace CRUDEvaluacion2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvProductos = new DataGridView();
            txtNombre = new TextBox();
            txtPrecio = new TextBox();
            cmbCategorias = new ComboBox();
            btnGuardar = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnLimpiar = new Button();
            btnReporte = new Button();
            btnReporteFiltrado = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
            SuspendLayout();
            // 
            // dgvProductos
            // 
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.AllowUserToDeleteRows = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductos.Location = new Point(50, 30);
            dgvProductos.MultiSelect = false;
            dgvProductos.Name = "dgvProductos";
            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.Size = new Size(620, 190);
            dgvProductos.TabIndex = 0;
            dgvProductos.CellClick += dgvProductos_CellClick;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(181, 240);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(180, 27);
            txtNombre.TabIndex = 1;
            // 
            // txtPrecio
            // 
            txtPrecio.Location = new Point(181, 285);
            txtPrecio.Name = "txtPrecio";
            txtPrecio.Size = new Size(180, 27);
            txtPrecio.TabIndex = 2;
            // 
            // cmbCategorias
            // 
            cmbCategorias.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategorias.FormattingEnabled = true;
            cmbCategorias.Location = new Point(181, 330);
            cmbCategorias.Name = "cmbCategorias";
            cmbCategorias.Size = new Size(180, 28);
            cmbCategorias.TabIndex = 3;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(40, 167, 69);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(60, 390);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 38);
            btnGuardar.TabIndex = 4;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.FromArgb(0, 123, 255);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(190, 390);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(110, 38);
            btnEditar.TabIndex = 5;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = false;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(220, 53, 69);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(320, 390);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(110, 38);
            btnEliminar.TabIndex = 6;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnLimpiar
            // 
            btnLimpiar.BackColor = Color.FromArgb(108, 117, 125);
            btnLimpiar.ForeColor = Color.White;
            btnLimpiar.Location = new Point(450, 390);
            btnLimpiar.Name = "btnLimpiar";
            btnLimpiar.Size = new Size(110, 38);
            btnLimpiar.TabIndex = 7;
            btnLimpiar.Text = "Limpiar";
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.Click += btnLimpiar_Click;
            // 
            // btnReporte
            // 
            btnReporte.BackColor = Color.FromArgb(23, 162, 184);
            btnReporte.ForeColor = Color.White;
            btnReporte.Location = new Point(130, 445);
            btnReporte.Name = "btnReporte";
            btnReporte.Size = new Size(180, 42);
            btnReporte.TabIndex = 8;
            btnReporte.Text = "📊 Reporte General (PDF)";
            btnReporte.UseVisualStyleBackColor = false;
            btnReporte.Click += btnReporte_Click;
            // 
            // btnReporteFiltrado
            // 
            btnReporteFiltrado.BackColor = Color.FromArgb(111, 66, 193);
            btnReporteFiltrado.ForeColor = Color.White;
            btnReporteFiltrado.Location = new Point(330, 445);
            btnReporteFiltrado.Name = "btnReporteFiltrado";
            btnReporteFiltrado.Size = new Size(240, 42);
            btnReporteFiltrado.TabIndex = 9;
            btnReporteFiltrado.Text = "🔍 Reporte por Categoría (PDF)";
            btnReporteFiltrado.UseVisualStyleBackColor = false;
            btnReporteFiltrado.Click += btnReporteFiltrado_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 243);
            label1.Name = "label1";
            label1.Size = new Size(71, 20);
            label1.TabIndex = 10;
            label1.Text = "Nombre :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 288);
            label2.Name = "label2";
            label2.Size = new Size(57, 20);
            label2.TabIndex = 11;
            label2.Text = "Precio :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(67, 333);
            label3.Name = "label3";
            label3.Size = new Size(81, 20);
            label3.TabIndex = 12;
            label3.Text = "Categoria :";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 510);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnLimpiar);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnGuardar);
            Controls.Add(btnReporte);
            Controls.Add(btnReporteFiltrado);
            Controls.Add(cmbCategorias);
            Controls.Add(txtPrecio);
            Controls.Add(txtNombre);
            Controls.Add(dgvProductos);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestión de Productos - CRUD & Reportes";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvProductos;
        private TextBox txtNombre;
        private TextBox txtPrecio;
        private ComboBox cmbCategorias;
        private Button btnGuardar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private Button btnReporte;
        private Button btnReporteFiltrado;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
