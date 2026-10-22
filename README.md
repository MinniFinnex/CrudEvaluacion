# CrudEvaluacion - Sistema de Gestión de Tienda (CRUD Multitabla)

> **Nota importante:** El código fuente funcional y actualizado se encuentra en la rama [`master`](https://github.com/MinniFinnex/CrudEvaluacion/tree/master).

---

## 👥 Integrantes del Proyecto
* **Kevin Alexander Rivera Escobar**
* **Juan Josue Irias Roque**
* **Elias Caleb Rivera**
* **Josue David Flores Martinez**

---

##  Descripción del Proyecto

Sistema de escritorio desarrollado en **C# / Windows Forms** bajo la plataforma **.NET 8.0** en **Visual Studio 2022**. Implementa una solución integral para operaciones CRUD (Crear, Leer, Actualizar y Eliminar) con arquitectura relacional multitabla (1:N) entre **Categorías** y **Productos**, conectada a un servidor relacional **MySQL / MariaDB** mediante el conector **`MySql.Data`**, además de incorporar un módulo de generación y exportación de reportes dinámicos en formato PDF con **Microsoft Reporting Services (RDLC)**.

---

##  Características Principales

* **CRUD Completo:** Inserción, lectura en DataGridView interactivo, edición y eliminación de registros.
* **Arquitectura Multitabla (1:N):** Relación de clave foránea entre Categorías y Productos con cascada referencial.
* **Inicialización y Auto-Migración de BD:** La aplicación detecta el servidor MySQL local y crea automáticamente la base de datos `TiendaDB`, las tablas relacionales y los datos base de categorías al iniciar.
* **Generación de Reportes RDLC (PDF):**
  * Reporte general de todos los productos registrados.
  * Reporte dinámico con filtros interactivos por categoría seleccionada.
* **Interfaz de Usuario Intuitiva:** Validación de formularios, selección directa desde la grilla y limpieza ágil de controles.

---

## 🛠️ Especificaciones Técnicas y Compatibilidad

| Parámetro | Configuración |
| :--- | :--- |
| **Target Framework** | `.NET 8.0` (`net8.0-windows`) |
| **Lenguaje** | C# 12 |
| **Tipo de Aplicación** | Windows Forms (WinForms) |
| **Driver de Base de Datos** | `MySql.Data` (v8.3.0 o superior) |
| **Gestor de BD** | MySQL 8.0+ / MariaDB 10.4+ (XAMPP / WAMP / Docker / Local) |
| **Visor de Reportes** | `ReportViewerCore.WinForms` (v15.1.33) |

---

##  Arquitectura y Estructura de la Base de Datos

El sistema trabaja sobre la base de datos relacional `TiendaDB`:

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

### Script DDL SQL (MySQL / MariaDB)

```sql
CREATE DATABASE IF NOT EXISTS `TiendaDB` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `TiendaDB`;

-- Tabla: Categorias
CREATE TABLE IF NOT EXISTS Categorias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Tabla: Productos
CREATE TABLE IF NOT EXISTS Productos (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    CategoriaId INT NOT NULL,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) 
        REFERENCES Categorias(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Inserción inicial de categorías
INSERT INTO Categorias (Nombre) VALUES 
('Electrónica'),
('Ropa'),
('Hogar'),
('Alimentos');
```

---

##  Requisitos e Instalación

Para clonar, compilar y ejecutar este proyecto localmente:

1. **Requisitos Previos:**
   * **SDK de .NET 8.0** y **Visual Studio 2022** (con la carga de trabajo *Desarrollo de escritorio de .NET*).
   * **Servidor MySQL/MariaDB** en ejecución en el puerto 3306 (por ejemplo, iniciando el módulo MySQL en **XAMPP** o **WAMP**).

2. **Clonar el repositorio (rama `master`):**
   ```bash
   git clone -b master https://github.com/MinniFinnex/CrudEvaluacion.git
   cd CrudEvaluacion
   ```

3. **Abrir y ejecutar:**
   * Abra la solución `CRUDEvaluacion2.slnx` o el archivo de proyecto `CRUDEvaluacion2/CRUDEvaluacion2.csproj` en Visual Studio 2022.
   * Restaure los paquetes NuGet si el entorno lo solicita.
   * Presione `F5` o haga clic en **Iniciar**.
   * También puede ejecutarse desde la terminal con:
     ```bash
     dotnet run --project CRUDEvaluacion2/CRUDEvaluacion2.csproj
     ```
