using System;
using System.Data;
using System.Windows.Forms;
using CRUDEvaluacion2.Models;

namespace CRUDEvaluacion2
{
    public partial class Form1 : Form
    {
        private TiendaContext _context = new TiendaContext();
        private int _productoIdSeleccionado = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!TiendaContext.InicializarBaseDeDatos(out string error))
            {
                MessageBox.Show(
                    "No se pudo conectar al servidor MySQL / MariaDB.\n\nDetalle: " + error +
                    "\n\nAsegúrese de que el servidor MySQL (XAMPP / WAMP / Docker) esté ejecutándose en el puerto 3306.",
                    "Aviso de Conexión a Base de Datos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            CargarCategorias();
            CargarProductos();
        }

        // --- MÉTODOS DE SOPORTE ---
        private void CargarCategorias()
        {
            try
            {
                var categorias = _context.ObtenerCategorias();
                cmbCategorias.DataSource = categorias;
                cmbCategorias.DisplayMember = "Nombre";
                cmbCategorias.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar categorías: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductos()
        {
            try
            {
                var dt = _context.ObtenerProductosDataTable();
                dgvProductos.DataSource = dt;

                if (dgvProductos.Columns["CategoriaId"] != null)
                {
                    dgvProductos.Columns["CategoriaId"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            if (cmbCategorias.Items.Count > 0)
            {
                cmbCategorias.SelectedIndex = 0;
            }
            _productoIdSeleccionado = 0;
        }

        // --- ACCIONES CRUD ---
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || cmbCategorias.SelectedValue == null)
            {
                MessageBox.Show("Por favor, rellene los campos correctamente (Nombre, Precio numérico y Categoría).", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Precio = precio,
                CategoriaId = Convert.ToInt32(cmbCategorias.SelectedValue)
            };

            if (_context.InsertarProducto(nuevoProducto, out string error))
            {
                CargarProductos();
                LimpiarFormulario();
                MessageBox.Show("Producto guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al guardar producto: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_productoIdSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla para editar.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio) || cmbCategorias.SelectedValue == null)
            {
                MessageBox.Show("Por favor, rellene los campos correctamente.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var productoActualizado = new Producto
            {
                Id = _productoIdSeleccionado,
                Nombre = txtNombre.Text.Trim(),
                Precio = precio,
                CategoriaId = Convert.ToInt32(cmbCategorias.SelectedValue)
            };

            if (_context.ActualizarProducto(productoActualizado, out string error))
            {
                CargarProductos();
                LimpiarFormulario();
                MessageBox.Show("Producto actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al actualizar producto: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProductos.Rows[e.RowIndex].Cells["Id"].Value != null)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                _productoIdSeleccionado = Convert.ToInt32(row.Cells["Id"].Value);
                txtNombre.Text = row.Cells["Producto"].Value?.ToString() ?? "";
                txtPrecio.Text = row.Cells["Precio"].Value?.ToString() ?? "";
                if (row.Cells["CategoriaId"].Value != null && int.TryParse(row.Cells["CategoriaId"].Value.ToString(), out int catId))
                {
                    cmbCategorias.SelectedValue = catId;
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_productoIdSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un producto de la lista para eliminar.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                if (_context.EliminarProducto(_productoIdSeleccionado, out string error))
                {
                    CargarProductos();
                    LimpiarFormulario();
                    MessageBox.Show("Producto eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al eliminar producto: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // --- INFORMES RDLC Y FILTROS DINÁMICOS ---
        private void btnReporte_Click(object sender, EventArgs e)
        {
            GenerarReporte(filtroCategoria: false);
        }

        private void btnReporteFiltrado_Click(object sender, EventArgs e)
        {
            GenerarReporte(filtroCategoria: true);
        }

        private void GenerarReporte(bool filtroCategoria)
        {
            try
            {
                int? catId = null;
                string descripcionFiltro = "Reporte General (Todos los productos registrados)";

                if (filtroCategoria && cmbCategorias.SelectedValue != null && Convert.ToInt32(cmbCategorias.SelectedValue) > 0)
                {
                    catId = Convert.ToInt32(cmbCategorias.SelectedValue);
                    string catNombre = _context.ObtenerNombreCategoria(catId.Value);
                    descripcionFiltro = $"Filtrado por Categoría: {catNombre}";
                }

                DataTable dt = _context.ObtenerDatosParaReporte(catId);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No hay productos que coincidan con la selección para generar el reporte.", "Sin registros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var formReporte = new FormReporte(dt, descripcionFiltro))
                {
                    formReporte.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
