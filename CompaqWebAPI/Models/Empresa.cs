using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using System.Text;

namespace CompaqWebAPI.Models
{
    public sealed class Empresa
    {
        /// <summary>
        ///     Campo CIDEMPRESA - Identificador de la empresa.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo CNOMBREEMPRESA - Nombre de la empresa.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CRUTADATOS - Ruta de la empresa.
        /// </summary>
        public string Ruta { get; set; } = string.Empty;
    }
}
