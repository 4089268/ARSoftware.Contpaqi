using System;
using CompaqWebAPI.Core.Comercial;
using CompaqWebAPI.Core.Interfaces;

namespace CompaqWebAPI.Core
{
    public static class ComercialServiceCollection
    {
        public static void AddComercialSDKServicesServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IConexionSDK, ConexionSdkComercial>();
            services.AddScoped<IEmpresaService, EmpresaServiceComercial>();
            services.AddScoped<IClienteService, ClienteServiceComercial>();
            services.AddScoped<IProductoService, ProductoServiceComercial>();
            services.AddScoped<IMovimientoService, MovimientoServiceComercial>();
            services.AddScoped<IConceptoService, ConceptoServiceComercial>();
            services.AddScoped<IDocumentoService, DocumentoServiceComercial>();
        }
    }
}
