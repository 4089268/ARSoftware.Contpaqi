using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Models;
using System.Text;

namespace CompaqWebAPI.Core.FacturaElectronica
{
    public class EmpresaServiceFE : IEmpresaService
    {
        public List<Empresa> BuscarEmpresas()
        {
            var empresasList = new List<Empresa>();

            // Declarar variables a leer de la base de datos
            var idBd = 0;
            var nombreBd = new StringBuilder(SdkConstantes.kLongNombre);
            var rutaBd = new StringBuilder(SdkConstantes.kLongRuta);

            // Posicionar el SDK en el primer registro
            int sdkResult = FacturaElectronicaSdk.fPosPrimerEmpresa(ref idBd, nombreBd, rutaBd);
            if (sdkResult != SdkConstantes.CodigoExito)
            {
                return empresasList;
            }

            // Instanciar una empresa y asignar los datos de la base de datos
            empresasList.Add(new Empresa
            {
                Id = idBd,
                Nombre = nombreBd.ToString(),
                Ruta = rutaBd.ToString()
            }
            );

            // Crear un loop y posicionar el SDK en el siguiente registro
            while (FacturaElectronicaSdk.fPosSiguienteEmpresa(ref idBd, nombreBd, rutaBd) == SdkConstantes.CodigoExito)
            {
                // Instanciar una empresa y asignar los datos de la base de datos       
                empresasList.Add(new Empresa
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
