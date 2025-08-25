using CompaqWebAPI.DTO;
using WebAPI.Core;

namespace CompaqWebAPI.Adapters
{
    public static class ConceptoAdapter
    {
        public static ConceptoSdk ToEntity(this NuevoConceptoRequest request)
        {
            var conceptoSdk = new ConceptoSdk
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
