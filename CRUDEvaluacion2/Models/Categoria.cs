using System.Collections.Generic;
namespace CRUDEvaluacion2.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        // Relación: Una categoría tiene muchos productos 
        public virtual ICollection<Producto> Productos { get; set; } = new List<Productos>();
    }
} 