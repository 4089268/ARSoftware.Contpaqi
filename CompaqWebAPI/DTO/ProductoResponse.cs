namespace WebAPI.DTO
{
    public class ProductoResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = default!;
        public string Nombre { get; set; } = default!;
        public int Tipo { get; set; }
        public string TipoDesc { get; set; } = default!;
    }
}
