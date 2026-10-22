# Guía de Implementación: CRUD Evaluaciones en Windows Forms (.NET)

Este repositorio (**CrudEvaluacion**) contiene la guía, arquitectura e instrucciones paso a paso para completar un sistema de escritorio con operaciones CRUD (Crear, Leer, Actualizar, Eliminar), relaciones multitabla (1:N), generación de informes en PDF con Microsoft Reporting Services (RDLC) y gráficos dinámicos utilizando **C#**, **Windows Forms** y **Entity Framework Core**.

---

## 📋 Tabla de Contenidos
1. [Requisitos del Sistema](#requisitos-del-sistema)
2. [Estructura y Arquitectura de la Base de Datos](#estructura-y-arquitectura-de-la-base-de-datos)
3. [Paso 1: Configuración del Proyecto y BD Local (`.mdf`)](#paso-1-configuración-del-proyecto-y-bd-local-mdf)
4. [Paso 2: Instalación de Dependencias (NuGet)](#paso-2-instalación-de-dependencias-nuget)
5. [Paso 3: Creación de Modelos y Contexto de Datos](#paso-3-creación-de-modelos-y-contexto-de-datos)
6. [Paso 4: Diseño de Interfaz y Código del Formulario Principal](#paso-4-diseño-de-interfaz-y-código-del-formulario-principal)
7. [Paso 5: Informes y Reportes en PDF (RDLC)](#paso-5-informes-y-reportes-en-pdf-rdlc)
8. [Paso 6: Filtros Dinámicos y Gráficos Estadísticos](#paso-6-filtros-dinámicos-y-gráficos-estadísticos)
9. [Flujo de Trabajo con Git y GitHub](#flujo-de-trabajo-con-git-y-github)

---

## 🛠 Requisitos del Sistema
* **IDE:** Visual Studio 2022.
* **Plataforma:** .NET 8.0 o .NET 6.0 (Windows Forms App).
* **Carga de trabajo:** Desarrollo de escritorio de .NET.
* **Componente extra:** LocalDB de SQL Server Express.
* **Extensión VS:** *Microsoft RDLC Report Designer 2022*.

---

## 🗄 Estructura y Arquitectura de la Base de Datos

El sistema trabaja con un archivo local `TiendaDB.mdf` con dos tablas en relación uno a muchos ($1:N$):

```
+------------------+         +-----------------------+
|    CATEGORIAS    |         |       PRODUCTOS       |
+------------------+         +-----------------------+
| PK  Id (INT)     |<-------1| PK  Id (INT)          |
|     Nombre (STR) |        *|     Nombre (STR)      |
+------------------+         |     Precio (DECIMAL)  |
                             | FK  CategoriaId (INT) |
                             +-----------------------+
```

### Script DDL SQL

```sql
-- Tabla: Categorias
CREATE TABLE [dbo].[Categorias] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Nombre] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- Tabla: Productos
CREATE TABLE [dbo].[Productos] (
    [Id] INT IDENTITY (1, 1) NOT NULL,
    [Nombre] NVARCHAR (100) NOT NULL,
    [Precio] DECIMAL (18, 2) NOT NULL,
    [CategoriaId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([CategoriaId]) 
        REFERENCES [dbo].[Categorias] ([Id]) ON DELETE CASCADE
);
```

---

## 🚀 Paso 1: Configuración del Proyecto y BD Local (`.mdf`)

1. En Visual Studio 2022, cree un proyecto de **Aplicación de Windows Forms (.NET)** nombrado `CrudEvaluacion` o el nombre de su solución.
2. Clic derecho en el proyecto $\rightarrow$ **Agregar** $\rightarrow$ **Nuevo elemento...**
3. Seleccione **Base de datos basada en servicio** y nombre el archivo `TiendaDB.mdf`.
4. Vaya al **Explorador de servidores**, despliegue la base de datos `TiendaDB.mdf`, clic derecho en **Tablas** $\rightarrow$ **Agregar nueva tabla** y ejecute los scripts DDL de `Categorias` y `Productos`.
5. **Configuración crítica:** Seleccione `TiendaDB.mdf` en el Explorador de soluciones y asegúrese de que la propiedad **Copiar en el directorio de salida** esté establecida en **Copiar si es posterior**.

---

## 📦 Paso 2: Instalación de Dependencias (NuGet)

Abra la **Consola del Administrador de Paquetes** (*Herramientas $\rightarrow$ Administrador de paquetes NuGet $\rightarrow$ Consola del Administrador de paquetes*) y ejecute:

```powershell
# Entity Framework Core para SQL Server
Install-Package Microsoft.EntityFrameworkCore.SqlServer

# Visor de Reportes RDLC para Windows Forms
Install-Package Microsoft.ReportingServices.ReportViewerControl.Winforms
```

---

## 💻 Paso 3: Creación de Modelos y Contexto de Datos

Cree una carpeta llamada `Models` en la raíz del proyecto e incluya los siguientes 3 archivos:

### 1. `Categoria.cs`
```csharp
using System.Collections.Generic;

namespace CrudEvaluacion.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        // Propiedad de navegación
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
```

### 2. `Producto.cs`
```csharp
namespace CrudEvaluacion.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        // Propiedad de navegación
        public virtual Categoria Categoria { get; set; } = null!;
    }
}
```

### 3. `TiendaContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace CrudEvaluacion.Models
{
    public class TiendaContext : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dbPath = Path.Combine(baseDir, "TiendaDB.mdf");

            optionsBuilder.UseSqlServer($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbPath};Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
```

---

## 🖼 Paso 4: Diseño de Interfaz y Código del Formulario Principal

### Elementos Visuales en `Form1`
* `dgvProductos`: DataGridView.
* `txtNombre`: TextBox.
* `txtPrecio`: TextBox.
* `cmbCategorias`: ComboBox.
* `btnGuardar`, `btnEditar`, `btnEliminar`, `btnLimpiar`: Buttons.

### Código de `Form1.cs`
```csharp
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using CrudEvaluacion.Models;

namespace CrudEvaluacion
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
                .Include(p => p.Categoria)
                .Select(p => new
                {
                    p.Id,
                    Producto = p.Nombre,
                    p.Precio,
                    Categoria = p.Categoria.Nombre,
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
            if (cmbCategorias.Items.Count > 0) cmbCategorias.SelectedIndex = 0;
            _productoIdSeleccionado = 0;
            btnGuardar.Text = "Guardar";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || !decimal.TryParse(txtPrecio.Text, out decimal precio))
            {
                MessageBox.Show("Por favor, rellene los campos correctamente.");
                return;
            }

            if (_productoIdSeleccionado == 0)
            {
                var nuevoProducto = new Producto
                {
                    Nombre = txtNombre.Text,
                    Precio = precio,
                    CategoriaId = (int)cmbCategorias.SelectedValue
                };
                _context.Productos.Add(nuevoProducto);
            }
            else
            {
                var productoExistente = _context.Productos.Find(_productoIdSeleccionado);
                if (productoExistente != null)
                {
                    productoExistente.Nombre = txtNombre.Text;
                    productoExistente.Precio = precio;
                    productoExistente.CategoriaId = (int)cmbCategorias.SelectedValue;
                }
            }

            _context.SaveChanges();
            CargarProductos();
            LimpiarFormulario();
            MessageBox.Show("Operación realizada con éxito.");
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
                btnGuardar.Text = "Actualizar";
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_productoIdSeleccionado == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
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
```

---

## 📊 Paso 5: Informes y Reportes en PDF (RDLC)

1. **Crear el Dataset del Reporte (`TiendaDataSet.xsd`):**
   * Clic derecho en el proyecto $\rightarrow$ Agregar $\rightarrow$ Nuevo elemento $\rightarrow$ **Conjunto de datos**.
   * Agregue una `DataTable` llamada `dtProductosReporte` con las columnas: `Id` (Int32), `Nombre` (String), `Precio` (Decimal), `Categoria` (String).
2. **Crear el Reporte (`InformeProductos.rdlc`):**
   * Clic derecho $\rightarrow$ Agregar $\rightarrow$ **Informe**.
   * Vincule el Dataset en **Datos del informe** $\rightarrow$ **Agregar conjunto de datos**.
   * Diseñe una tabla y asegúrese de que la propiedad del archivo `.rdlc` tenga **Copiar si es posterior**.
3. **Formulario Visor (`FormReporte.cs`):**
   * Agregue un Formulario con un control `ReportViewer` (`reportViewer1`) con `Dock = Fill`.

```csharp
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

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
            reportViewer1.LocalReport.ReportPath = rutaReporte;

            if (!string.IsNullOrEmpty(_descripcionFiltro))
            {
                ReportParameter[] parametros = new ReportParameter[1];
                parametros[0] = new ReportParameter("FiltroUsuario", _descripcionFiltro);
                reportViewer1.LocalReport.SetParameters(parametros);
            }

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
```

---

## 📈 Paso 6: Filtros Dinámicos y Gráficos Estadísticos

### Gráficos Estadísticos
En el archivo `InformeProductos.rdlc`:
1. Clic derecho en el lienzo $\rightarrow$ **Insertar** $\rightarrow$ **Gráfico** (Columnas o Pastel).
2. En el panel **Datos del gráfico**:
   * **Valores:** Seleccione `Nombre` (se convertirá en `Count(Nombre)`).
   * **Grupos de categorías:** Seleccione `Categoria`.
3. Clic derecho en la serie $\rightarrow$ **Mostrar etiquetas de datos**.

---

## 🐙 Flujo de Trabajo con Git y GitHub

### Primera vez subiendo el repositorio
1. En la barra inferior de Visual Studio, haga clic en **Agregar al control de código fuente** $\rightarrow$ **Git**.
2. Seleccione **GitHub**, inicie sesión si es necesario y nombre el repositorio como **CrudEvaluacion**.
3. Asegúrese de que cree automáticamente el `.gitignore` para .NET (evita subir `.ldf` temporales de SQL).
4. Haga clic en **Crear e insertar (Create and Push)**.

### Subir cambios continuos (Commit & Push)
1. Abra la pestaña **Cambios de Git**.
2. Ingrese un mensaje explicativo de los cambios (ej: *"Se agrega reporte RDLC y filtros"*).
3. Haga clic en **Confirmar todo (Commit All)**.
4. Presione el botón **Insertar (Push)** para sincronizar con GitHub.