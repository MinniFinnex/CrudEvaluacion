namespace CRUDEvaluacion2.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public override string ToString() => Nombre;
    }
}
