using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace CRUDEvaluacion2
{
    public partial class FormReporte : Form
    {
        private DataTable _datosReporte;
        private string _descripcionFiltro;

        public FormReporte(DataTable datos, string descripcionFiltro = "")
        {
            InitializeComponent();
            _datosReporte = datos;
            _descripcionFiltro = descripcionFiltro;
        }

        private void FormReporte_Load(object sender, EventArgs e)
        {
            try
            {
                reportViewer1.LocalReport.DataSources.Clear();
                string rutaReporte = Path.Combine(Application.StartupPath, "InformeProductos.rdlc");

                if (!File.Exists(rutaReporte))
                {
                    MessageBox.Show("No se encontró el archivo de reporte en: " + rutaReporte, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                reportViewer1.LocalReport.ReportPath = rutaReporte;

                string filtro = string.IsNullOrEmpty(_descripcionFiltro) ? "Todos los productos registrados" : _descripcionFiltro;
                ReportParameter[] parametros = new ReportParameter[1];
                parametros[0] = new ReportParameter("FiltroUsuario", filtro);
                reportViewer1.LocalReport.SetParameters(parametros);

                ReportDataSource rds = new ReportDataSource("DataSetProductos", _datosReporte);
                reportViewer1.LocalReport.DataSources.Add(rds);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la vista previa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
