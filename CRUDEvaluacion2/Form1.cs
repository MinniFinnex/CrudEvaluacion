using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
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
            CargarCategorias();
            CargarProductos();
        }

        // --- MÉTODOS DE SOPORTE ---
        private void CargarCategorias()
        {
            if (!_context.Categorias.Any())
            {
                _context.Categorias.AddRange(
                    new Categoria { Nombre = "Electrónica" },
                    new Categoria { Nombre = "Ropa" },
                    new Categoria { Nombre = "Hogar" },
                    new Categoria { Nombre = "Alimentos" }
                );
                _context.SaveChanges();
            }

            cmbCategorias.DataSource = _context.Categorias.ToList();
            cmbCategorias.DisplayMember = "Nombre";
            cmbCategorias.ValueMember = "Id";
        }

        private void CargarProductos()
        {
            var productos = _context.Productos
                .Include(p => p.Categorias)
                .Select(p => new
                {
                    p.Id,
                    Producto = p.Nombre,
                    p.Precio,
                    Categoria = p.Categorias != null ? p.Categorias.Nombre : "Sin Categoría",
                    p.CategoriaId
                }).ToList();

            dgvProductos.DataSource = productos;

            if (dgvProductos.Columns["CategoriaId"] != null)
                dgvProductos.Columns["CategoriaId"].Visible = false;
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtPrecio.Clear();
            if (cmbCategorias.Items.Count > 0)
                cmbCategorias.SelectedIndex = 0;
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

            // CREATE (Guardar un nuevo producto)
            var nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text.Trim(),
                Precio = precio,
                CategoriaId = (int)cmbCategorias.SelectedValue
            };
            _context.Productos.Add(nuevoProducto);

            _context.SaveChanges();
            CargarProductos();
            LimpiarFormulario();
            MessageBox.Show("Producto guardado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            // UPDATE (Actualizar producto existente)
            var productoExistente = _context.Productos.Find(_productoIdSeleccionado);
            if (productoExistente != null)
            {
                productoExistente.Nombre = txtNombre.Text.Trim();
                productoExistente.Precio = precio;
                productoExistente.CategoriaId = (int)cmbCategorias.SelectedValue;

                _context.SaveChanges();
                CargarProductos();
                LimpiarFormulario();
                MessageBox.Show("Producto actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (row.Cells["CategoriaId"].Value != null)
                {
                    cmbCategorias.SelectedValue = Convert.ToInt32(row.Cells["CategoriaId"].Value);
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
                var producto = _context.Productos.Find(_productoIdSeleccionado);
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges();
                    CargarProductos();
                    LimpiarFormulario();
                    MessageBox.Show("Producto eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        // --- PASOS 5 Y 6: INFORMES RDLC Y FILTROS DINÁMICOS ---
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
            var dt = new DataTable("dtProductosReporte");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Categoria", typeof(string));

            var query = _context.Productos.Include(p => p.Categorias).AsQueryable();
            string descripcionFiltro = "Reporte General (Todos los productos registrados)";

            if (filtroCategoria && cmbCategorias.SelectedValue != null && (int)cmbCategorias.SelectedValue > 0)
            {
                int catId = (int)cmbCategorias.SelectedValue;
                query = query.Where(p => p.CategoriaId == catId);
                var cat = _context.Categorias.Find(catId);
                descripcionFiltro = $"Filtrado por Categoría: {(cat != null ? cat.Nombre : "Seleccionada")}";
            }

            var lista = query.ToList();

            if (!lista.Any())
            {
                MessageBox.Show("No hay productos que coincidan con la selección para generar el reporte.", "Sin registros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var prod in lista)
            {
                dt.Rows.Add(prod.Id, prod.Nombre, prod.Precio, prod.Categorias != null ? prod.Categorias.Nombre : "Sin Categoría");
            }

            using (var formReporte = new FormReporte(dt, descripcionFiltro))
            {
                formReporte.ShowDialog();
            }
        }
    }
}
