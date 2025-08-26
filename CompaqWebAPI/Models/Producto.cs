using System;
using System.Text;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using ARSoftware.Contpaqi.Comercial.Sdk.Excepciones;

namespace CompaqWebAPI.Models
{
    public class Producto
    {
        /// <summary>
        ///     Campo CCODIGOPRODUCTO - Código del producto.
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CIDPRODUCTO - Identificador del producto.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo CNOMBREPRODUCTO - Nombre del producto.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CTIPOPRODUCTO - Tipo de producto: 1 = Producto, 2 = Paquete, 3 = Servicio
        /// </summary>
        public int Tipo { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Codigo} - {Nombre} - {Tipo}";
        }
    }
}
