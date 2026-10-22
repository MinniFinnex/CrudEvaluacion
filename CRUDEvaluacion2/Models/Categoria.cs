using System.Collections.Generic;

namespace CRUDEvaluacion2.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Debe ser Producto en singular dentro de los corchetes angulares
        public ICollection<Producto> Productos { get; set; }
    }
}