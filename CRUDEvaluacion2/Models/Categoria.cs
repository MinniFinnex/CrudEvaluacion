using System.Collections.Generic;

namespace CRUDEvaluacion2.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        // Propiedad de navegación
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
