using System;
using System.Text;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;

namespace CompaqWebAPI.Models
{
    public class Cliente
    {
        /// <summary>
        ///     Campo CCODIGOCLIENTE - Código del cliente o proveedor.
        /// </summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CIDCLIENTEPROVEEDOR - Identificador del cliente o proveedor.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo CRAZONSOCIAL - Razón Social del cliente o proveedor.
        /// </summary>
        public string RazonSocial { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CRFC - Registro Federal de Contribuyentes del cliente.
        /// </summary>
        public string Rfc { get; set; } = string.Empty;

        /// <summary>
        ///     Campo CTIPOCLIENTE - Tipo de cliente o proveedor: 1 = Cliente 2 = Cliente/Proveedor 3 = Proveedor
        /// </summary>
        public int Tipo { get; set; }

        public override string ToString()
        {
            return $"{Id} - {Codigo} - {RazonSocial} - {Rfc} - {Tipo}";
        }
    }
}
