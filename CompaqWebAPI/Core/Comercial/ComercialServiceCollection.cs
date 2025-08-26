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
        }
    }
}
