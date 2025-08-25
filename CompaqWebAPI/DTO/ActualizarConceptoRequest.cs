using System.ComponentModel.DataAnnotations;

namespace CompaqWebAPI.DTO
{
    public class ActualizarConceptoRequest
    {
        public string? Nombre { get; set; }

        public string? Serie { get; set; }

        public string? RutaEntrega { get; set; }

        public string? PrefijoConcepto { get; set; }

        public string? PlantillaFormatoDigital { get; set; }
    }
}
