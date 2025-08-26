using System;
using System.Text;


namespace CompaqWebAPI.Models
{
    public class Movimiento
    {
        /// <summary>
        ///     Campo CIDDOCUMENTO - Identificador del documento dueño del movimiento.
        /// </summary>
        public int DocumentoId { get; set; }

        /// <summary>
        ///     Campo CIDMOVIMIENTO - Identificador del movimiento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo COBSERVAMOV - Observaciones del movimiento.
        /// </summary>
        public string Observaciones { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CPRECIO - Precio del producto.
        /// </summary>
        public double Precio { get; set; }

        /// <summary>
        ///     Campo CIDPRODUCTO - Identificador del producto del movimiento
        /// </summary>
        public int ProductoId { get; set; }

        /// <summary>
        ///     Campo CREFERENCIA - Referencia del movimiento.
        /// </summary>
        public string Referencia { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CTOTAL - Importe del total del movimiento.
        /// </summary>
        public double Total { get; set; }

        /// <summary>
        ///     Campo CUNIDADES - Cantidad de unidad base del movimiento.
        /// </summary>
        public double Unidades { get; set; }


        public override string ToString()
        {
            return $"{Id} - ProductoID:{ProductoId} - {Unidades} - {Precio} - {Total:C}";
        }
    }
}
