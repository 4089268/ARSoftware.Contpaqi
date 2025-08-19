using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class NuevaFacturaRequest
    {
[Required]
        public string? CodigoConcepto { get; set; }

        [Required]
        public string? CodigoCliente { get; set; }

        [Required]
        public IEnumerable<MovimientoRequest>? Movimientos { get; set; }

        [Required]
        public string? Referencia { get; set; }

        [Required]
        public string? Observaciones { get; set; }
    }
}
