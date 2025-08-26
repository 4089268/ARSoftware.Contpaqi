using CompaqWebAPI.DTO;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Adapters
{
    public static class ConceptoAdapter
    {
        public static Concepto ToEntity(this NuevoConceptoRequest request)
        {
            var conceptoSdk = new Concepto
            {
                Codigo = request.Codigo!,
                Nombre = request.Nombre!,
                Serie = request.Serie,
                RutaEntrega = request.RutaEntrega,
                PrefijoConcepto = request.PrefijoConcepto,
                PlantillaFormatoDigital = request.PlantillaFormatoDigital
            };
            return conceptoSdk;
        }
    }
}
