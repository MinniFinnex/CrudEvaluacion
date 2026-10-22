using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDEvaluacion2.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        // Propiedades de navegación
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categorias { get; set; } = null!;
    }
}
