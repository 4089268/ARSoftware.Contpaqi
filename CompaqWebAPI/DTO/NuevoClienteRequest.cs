using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTO
{
    public class NuevoClienteRequest
    {
        [Required]
        public string? Codigo { get; set; }
        
        [Required]
        public string? RazonSocial { get; set; }

        [Required]
        [StringLength(13)]
        public string? RFC { get; set; }

        [Required]
        [Range(1,3)]
        public int Tipo { get; set; }
    }
}
