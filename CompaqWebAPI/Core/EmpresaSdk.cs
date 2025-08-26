using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using System.Text;

namespace CompaqWebAPI.Core
{
    public sealed class EmpresaSdk
    {
        /// <summary>
        ///     Campo CIDEMPRESA - Identificador de la empresa.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo CNOMBREEMPRESA - Nombre de la empresa.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        ///     Campo CRUTADATOS - Ruta de la empresa.
        /// </summary>
        public string Ruta { get; set; }
    }
}
