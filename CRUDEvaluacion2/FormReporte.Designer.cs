namespace CRUDEvaluacion2
{
    partial class FormReporte
    {
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            SuspendLayout();
            // 
            // reportViewer1
            // 
            reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportViewer1.Location = new System.Drawing.Point(0, 0);
            reportViewer1.Name = "reportViewer1";
            reportViewer1.ServerReport.BearerToken = null;
            reportViewer1.Size = new System.Drawing.Size(884, 561);
            reportViewer1.TabIndex = 0;
            // 
            // FormReporte
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(884, 561);
            Controls.Add(reportViewer1);
            Name = "FormReporte";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Visor de Reporte - Inventario y Estadísticas";
            Load += FormReporte_Load;
            ResumeLayout(false);
        }
    }
}
