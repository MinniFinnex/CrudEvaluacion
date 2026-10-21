namespace CRUDEvaluacion2.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public int CategoriaId { get; set; }

        // Propiedades de navegación
        public virtual Categoria Categorias { get; set; } = null!;
    }
}
