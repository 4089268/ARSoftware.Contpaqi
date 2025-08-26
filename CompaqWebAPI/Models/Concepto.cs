using System;
using System.Text;

namespace CompaqWebAPI.Models
{
    public class Concepto
    {
        /// <summary>
        ///     Campo CCODIGOCONCEPTO - Código del concepto.
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CIDCONCEPTODOCUMENTO - Identificador del concepto de documento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo CNOMBRECONCEPTO - Nombre del concepto.
        /// </summary>
        public string Nombre { get; set; } = default!;

        /// <summary>
        ///     Serie para clasificar las facturas
        /// </summary>
        public string? Serie { get; set; }

        /// <summary>
        ///     Ruta donde se almacenaran las facturas (pdf, xml) generadas para este concepto.
        /// </summary>
        public string? RutaEntrega { get; set; }

        /// <summary>
        ///     Prefijo para el nombre del archivo de entrega.
        /// </summary>
        public string? PrefijoConcepto { get; set; }

        /// <summary>
        ///    Nombre de plantilla utilizada para generar los documentos digitales.
        /// </summary>
        public string? PlantillaFormatoDigital { get; set; }

    }
}
