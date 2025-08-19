using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class MovimientoRequest
    {
        [Required]
        public string? CodigoProducto { get; set; }

        [Required]
        public double? Unidades { get; set; }

        [Required]
        public decimal? PrecioUnidad { get; set; }
    }
}
