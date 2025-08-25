using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class ActualizarClienteRequest
    {
        public string? RazonSocial { get; set; }

        [StringLength(13)]
        public string? RFC { get; set; }
        
        public int Tipo { get; set; }
    }
}
