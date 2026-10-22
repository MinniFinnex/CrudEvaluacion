using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace CRUDEvaluacion2.Models
{
    public class TiendaContext
    {
        // Configuración de conexión para MySQL / MariaDB (XAMPP, WAMP, Docker o Local)
        private static readonly string Server = "localhost";
        private static readonly int Port = 3306;
        private static readonly string Database = "TiendaDB";
        private static readonly string Uid = "root";
        private static readonly string Password = "";

        public static string ConnectionString =>
            $"Server={Server};Port={Port};Database={Database};Uid={Uid};Pwd={Password};Charset=utf8mb4;SslMode=none;";

        private static string ServerConnectionString =>
            $"Server={Server};Port={Port};Uid={Uid};Pwd={Password};Charset=utf8mb4;SslMode=none;";

        public static bool InicializarBaseDeDatos(out string error)
        {
            error = string.Empty;
            try
            {
                // 1. Asegurar que la base de datos TiendaDB existe en MySQL
                using (var conn = new MySqlConnection(ServerConnectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{Database}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
                        cmd.ExecuteNonQuery();
                    }
                }

                // 2. Asegurar creación de tablas
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            CREATE TABLE IF NOT EXISTS Categorias (
                                Id INT AUTO_INCREMENT PRIMARY KEY,
                                Nombre VARCHAR(50) NOT NULL
                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

                            CREATE TABLE IF NOT EXISTS Productos (
                                Id INT AUTO_INCREMENT PRIMARY KEY,
                                Nombre VARCHAR(100) NOT NULL,
                                Precio DECIMAL(18, 2) NOT NULL,
                                CategoriaId INT NOT NULL,
                                CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId) 
                                    REFERENCES Categorias(Id) ON DELETE CASCADE
                            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
                        ";
                        cmd.ExecuteNonQuery();
                    }

                    // 3. Sembrar categorías iniciales si no existen registros
                    using (var cmdCheck = new MySqlCommand("SELECT COUNT(*) FROM Categorias;", conn))
                    {
                        long count = Convert.ToInt64(cmdCheck.ExecuteScalar());
                        if (count == 0)
                        {
                            using (var cmdSeed = new MySqlCommand(@"
                                INSERT INTO Categorias (Nombre) VALUES 
                                ('Electrónica'),
                                ('Ropa'),
                                ('Hogar'),
                                ('Alimentos');", conn))
                            {
                                cmdSeed.ExecuteNonQuery();
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public List<Categoria> ObtenerCategorias()
        {
            var lista = new List<Categoria>();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT Id, Nombre FROM Categorias ORDER BY Id;", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Categoria
                        {
                            Id = reader.GetInt32("Id"),
                            Nombre = reader.GetString("Nombre")
                        });
                    }
                }
            }
            return lista;
        }

        public DataTable ObtenerProductosDataTable()
        {
            var dt = new DataTable();
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        p.Id, 
                        p.Nombre AS Producto, 
                        p.Precio, 
                        c.Nombre AS Categoria, 
                        p.CategoriaId
                    FROM Productos p
                    INNER JOIN Categorias c ON p.CategoriaId = c.Id
                    ORDER BY p.Id DESC;";

                using (var cmd = new MySqlCommand(query, conn))
                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            return dt;
        }

        public bool InsertarProducto(Producto producto, out string error)
        {
            error = string.Empty;
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "INSERT INTO Productos (Nombre, Precio, CategoriaId) VALUES (@nombre, @precio, @catId);", conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                        cmd.Parameters.AddWithValue("@precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@catId", producto.CategoriaId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool ActualizarProducto(Producto producto, out string error)
        {
            error = string.Empty;
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(
                        "UPDATE Productos SET Nombre = @nombre, Precio = @precio, CategoriaId = @catId WHERE Id = @id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", producto.Id);
                        cmd.Parameters.AddWithValue("@nombre", producto.Nombre);
                        cmd.Parameters.AddWithValue("@precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@catId", producto.CategoriaId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public bool EliminarProducto(int id, out string error)
        {
            error = string.Empty;
            try
            {
                using (var conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("DELETE FROM Productos WHERE Id = @id;", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public DataTable ObtenerDatosParaReporte(int? categoriaId = null)
        {
            var dt = new DataTable("dtProductosReporte");
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Categoria", typeof(string));

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                string query = @"
                    SELECT 
                        p.Id, 
                        p.Nombre, 
                        p.Precio, 
                        c.Nombre AS Categoria
                    FROM Productos p
                    INNER JOIN Categorias c ON p.CategoriaId = c.Id";

                if (categoriaId.HasValue && categoriaId.Value > 0)
                {
                    query += " WHERE p.CategoriaId = @catId";
                }

                query += " ORDER BY p.Id ASC;";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    if (categoriaId.HasValue && categoriaId.Value > 0)
                    {
                        cmd.Parameters.AddWithValue("@catId", categoriaId.Value);
                    }

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dt.Rows.Add(
                                reader.GetInt32("Id"),
                                reader.GetString("Nombre"),
                                reader.GetDecimal("Precio"),
                                reader.GetString("Categoria")
                            );
                        }
                    }
                }
            }
            return dt;
        }

        public string ObtenerNombreCategoria(int categoriaId)
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT Nombre FROM Categorias WHERE Id = @id;", conn))
                {
                    cmd.Parameters.AddWithValue("@id", categoriaId);
                    object? res = cmd.ExecuteScalar();
                    return res != null ? res.ToString() ?? "Seleccionada" : "Seleccionada";
                }
            }
        }
    }
}
