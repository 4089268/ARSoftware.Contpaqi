using System;

namespace CompaqWebAPI.Models
{
    public class Documento
    {
        /// <summary>
        ///     Campo CIDCLIENTEPROVEEDOR - Identificador del cliente o proveedor del documento.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        ///     Campo CIDCONCEPTODOCUMENTO - Identificador del concepto del documento.
        /// </summary>
        public int ConceptoId { get; set; }

        /// <summary>
        ///     Campo CFECHA - Fecha del documento.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        ///     Campo CFOLIO - Folio del documento.
        /// </summary>
        public double Folio { get; set; }

        /// <summary>
        ///     Campo CIDDOCUMENTO - Identificador del documento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo COBSERVACIONES - Observaciones del documento.
        /// </summary>
        public string Observaciones { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CREFERENCIA - Referencia del documento.
        /// </summary>
        public string Referencia { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CSERIEDOCUMENTO - Serie del documento.
        /// </summary>
        public string Serie { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CTOTAL - Importe del total de los totales de los movimientos para el documento.
        /// </summary>
        public double Total { get; set; }


        public override string ToString()
        {
            return
                $"{Id} - {Fecha:MM/dd/yyyy} - ConceptoID:{ConceptoId} - {Serie} - {Folio} - ClienteID:{ClienteId} - {Total:C}";
        }
    }
}
