using CompaqWebAPI.Core.Comercial;
using CompaqWebAPI.Core.FacturaElectronica;
using CompaqWebAPI.Core.Interfaces;
using System;

namespace CompaqWebAPI.Core
{
    public static class FacturaElectronicaServiceCollection
    {
        public static void AddFacturaElectronicaSDKServicesServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IConexionSDK, ConexionSdkFE>();
            services.AddScoped<IEmpresaService, EmpresaServiceFE>();
            services.AddScoped<IClienteService, ClienteServiceFE>();
            services.AddScoped<IProductoService, ProductoServiceFE>();
            services.AddScoped<IMovimientoService, MovimientoServiceFE>();
            services.AddScoped<IConceptoService, ConceptoServiceFE>();
            services.AddScoped<IDocumentoService, DocumentoServiceFE>();
        }
    }
}
