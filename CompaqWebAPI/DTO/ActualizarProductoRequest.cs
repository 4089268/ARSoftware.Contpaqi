using System.ComponentModel.DataAnnotations;

namespace CompaqWebAPI.DTO
{
    public class ActualizarProductoRequest
    {
        public string? Nombre { get; set; }

        public int? Tipo { get; set; } = 0;
    }
}
