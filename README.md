# CrudEvaluacion
Crud evaluacion del parcial en la rama **MASTER**

-- Kevin Alexander Rivera Escobar
-- Juan Josue Irias Roque
-- Elias Caleb Rivera 
-- Josue David Flores Martinez


# Sistema de Gestión de Tienda (CRUD Multitabla)
¡Bienvenido! Este es un proyecto de escritorio desarrollado en **C# con Windows
Forms** utilizando **Visual Studio 2022**. Implementa un sistema CRUD (Crear,
Leer, Actualizar, Eliminar) completo y multitabla conectado a una base de datos
local de SQL Server (`.mdf`).
## Características del Proyecto
- **CRUD Completo:** Gestión integral de registros (Inserción, lectura,
actualización y borrado lógico/físico).
- **Arquitectura Multitabla:** Manejo de relaciones Uno a Muchos (1:N) entre
Categorías y Productos.
- **ORM Moderno:** Utiliza **Entity Framework Core** para el mapeo de datos,
eliminando la necesidad de escribir consultas SQL manuales.

- **Interfaz Intuitiva:** Grid dinámico con actualización en tiempo real y
controles autolimpiables.
---
## Arquitectura de la Base de Datos
El sistema utiliza una base de datos local basada en servicios (`TiendaDB.mdf`)
con la siguiente estructura de tablas y dependencias:
 CATEGORIAS {
 int Id PK
 string Nombre
 }
 PRODUCTOS {
 int Id PK
 string Nombre
 decimal Precio
 int CategoriaId FK
 }
### Script de Creación (DDL)
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
 CONSTRAINT [FK_Productos_Categorias] FOREIGN KEY ([CategoriaId]) REFERENCES
[dbo].[Categorias] ([Id]) ON DELETE CASCADE
);

## Tecnologías y Herramientas Herramientas Utilizadas
- **Lenguaje:** C# (.NET [Indica tu versión, ej: 8.0])
- **Entorno:** Visual Studio 2022
- **Tecnología de UI:** Windows Forms (WinForms)
- **Base de Datos:** SQL Server Express LocalDB (`.mdf`)
- **ORM:** Entity Framework Core (Microsoft.EntityFrameworkCore.SqlServer)
---
## Requisitos e Instalación
Para clonar y ejecutar esta aplicación localmente, necesita tener instalado
**Visual Studio 2022** con la carga de trabajo **Desarrollo de escritorio de
.NET** y el componente **LocalDB de SQL Server Express**.
1. **Clone el repositorio:**
git clone https://github.com/MinniFinnex/CrudEvaluacion.git
2. Abra el archivo de solución `.sln` en Visual Studio 2022.
3. Asegúrese de restaurar los paquetes NuGet si el IDE no lo hace
automáticamente.
4. Presione `F5` o haga clic en **Iniciar** para ejecutar el proyecto.
> **Nota sobre la Base de Datos:** El proyecto está configurado para buscar el
archivo `TiendaDB.mdf` automáticamente en el directorio de salida. Asegúrese de
que el archivo `.mdf` tenga la propiedad *Copiar en el directorio de salida*
configurada como *Copiar si es posterior*.

-- Kevin Alexander Rivera Escobar
-- Juan Josue Irias Roque
-- Elias Caleb Rivera 
-- Josue David Flores Martinez
