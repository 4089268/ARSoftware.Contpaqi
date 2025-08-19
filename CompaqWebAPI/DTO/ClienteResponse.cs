namespace WebAPI.DTO
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = default!;
        public string RazonSocial { get; set; } = default!;
        public string Rfc { get; set; } = default!;
        public int Tipo { get; set; }
        public string TipoDesc { get; set; } = default!;
    }
}
