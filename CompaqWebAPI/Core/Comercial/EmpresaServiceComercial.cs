using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using CompaqWebAPI.Core.Interfaces;
using System.Text;

namespace CompaqWebAPI.Core.Comercial
{
    public class EmpresaServiceComercial : IEmpresaService
    {
        public List<EmpresaSdk> BuscarEmpresas()
        {
            var empresasList = new List<EmpresaSdk>();

            // Declarar variables a leer de la base de datos
            var idBd = 0;
            var nombreBd = new StringBuilder(SdkConstantes.kLongNombre);
            var rutaBd = new StringBuilder(SdkConstantes.kLongRuta);

            // Posicionar el SDK en el primer registro
            int sdkResult = ComercialSdk.fPosPrimerEmpresa(ref idBd, nombreBd, rutaBd);
            if (sdkResult != SdkConstantes.CodigoExito)
            {
                return empresasList;
            }

            // Instanciar una empresa y asignar los datos de la base de datos
            empresasList.Add(new EmpresaSdk
            {
                Id = idBd,
                Nombre = nombreBd.ToString(),
                Ruta = rutaBd.ToString()
            }
            );

            // Crear un loop y posicionar el SDK en el siguiente registro
            while (ComercialSdk.fPosSiguienteEmpresa(ref idBd, nombreBd, rutaBd) == SdkConstantes.CodigoExito)
            {
                // Instanciar una empresa y asignar los datos de la base de datos       
                empresasList.Add(new EmpresaSdk
                {
                    Id = idBd,
                    Nombre = nombreBd.ToString(),
                    Ruta = rutaBd.ToString()
                });
            }
            return empresasList;
        }
    }
}
