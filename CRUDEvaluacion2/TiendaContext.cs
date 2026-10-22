using Microsoft.EntityFrameworkCore;

namespace CRUDEvaluacion2.Models
{
    public class TiendaContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TiendaDB;Trusted_Connection=True;");
        }
    }
}
