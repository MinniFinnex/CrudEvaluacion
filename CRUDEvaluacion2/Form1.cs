using System;
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
                    new Categoria { Nombre = "Ropa" }
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
                    Categoria = p.Categorias.Nombre,
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
            cmbCategorias.SelectedIndex = -1;
            cmbCategorias.Text = string.Empty; // Fuerza a limpiar el texto visual del ComboBox
            _productoIdSeleccionado = 0;
        }

        // --- ACCIONES CRUD ---
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Por favor, rellene los campos correctamente.");
                return;
            }

            // CREATE (Guardar un nuevo producto)
            var nuevoProducto = new Producto
            {
                Nombre = txtNombre.Text,
                Precio = precio,
                CategoriaId = (int)cmbCategorias.SelectedValue
            };
            _context.Productos.Add(nuevoProducto);

            _context.SaveChanges();
            CargarProductos();
            LimpiarFormulario();
            MessageBox.Show("Producto guardado con éxito.");
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (_productoIdSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla para editar.");
                return;
            }

            if (string.IsNullOrEmpty(txtNombre.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Por favor, rellene los campos correctamente.");
                return;
            }

            // UPDATE (Actualizar producto existente)
            var productoExistente = _context.Productos.Find(_productoIdSeleccionado);
            if (productoExistente != null)
            {
                productoExistente.Nombre = txtNombre.Text;
                productoExistente.Precio = precio;
                productoExistente.CategoriaId = (int)cmbCategorias.SelectedValue;

                _context.SaveChanges();
                CargarProductos();
                LimpiarFormulario();
                MessageBox.Show("Producto actualizado con éxito.");
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                _productoIdSeleccionado = Convert.ToInt32(row.Cells["Id"].Value);
                txtNombre.Text = row.Cells["Producto"].Value.ToString();
                txtPrecio.Text = row.Cells["Precio"].Value.ToString();
                cmbCategorias.SelectedValue = Convert.ToInt32(row.Cells["CategoriaId"].Value);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_productoIdSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un producto de la lista para eliminar.");
                return;
            }

            var confirm = MessageBox.Show("¿Está seguro de eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var producto = _context.Productos.Find(_productoIdSeleccionado);
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges();
                    CargarProductos();
                    LimpiarFormulario();
                    MessageBox.Show("Producto eliminado.");
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }
    }
}