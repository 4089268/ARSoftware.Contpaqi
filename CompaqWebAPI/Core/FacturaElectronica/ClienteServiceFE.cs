using System;
using System.Text;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.FacturaElectronica
{
    public class ClienteServiceFE : IClienteService
    {
        /// <summary>
        ///     Actualiza un cliente.
        /// </summary>
        /// <param name="cliente">Cliente del que se asignaran los datos a modificar.</param>
        public void ActualizarCliente(Cliente cliente)
        {
            // Buscar el cliente por código
            // Si el cliente existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscaCteProv(cliente.Codigo).TirarSiEsError();

            // Activar el modo de edición
            FacturaElectronicaSdk.fEditaCteProv().TirarSiEsError();

            // Actualizar los campos del registro donde el SDK esta posicionado
            FacturaElectronicaSdk.fSetDatoCteProv("CRAZONSOCIAL", cliente.RazonSocial).TirarSiEsError();
            FacturaElectronicaSdk.fSetDatoCteProv("CRFC", cliente.Rfc).TirarSiEsError();

            // Guardar los cambios realizados al registro
            FacturaElectronicaSdk.fGuardaCteProv().TirarSiEsError();
        }

        /// <summary>
        ///     Busca un cliente por código.
        /// </summary>
        /// <param name="clienteCodigo">El código del cliente a buscar.</param>
        /// <returns>El cliente a buscar.</returns>
        public Cliente BuscarClientePorCodigo(string clienteCodigo)
        {
            // Buscar el cliente por código
            // Si el cliente existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscaCteProv(clienteCodigo).TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            return LeerDatosCliente();
        }

        /// <summary>
        ///     Busca un cliente por id.
        /// </summary>
        /// <param name="clienteId">El id del cliente a buscar.</param>
        /// <returns>El cliente a buscar.</returns>
        public Cliente BuscarClientePorId(int clienteId)
        {
            // Buscar el cliente por id
            // Si el cliente existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscaIdCteProv(clienteId).TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            return LeerDatosCliente();
        }

        /// <summary>
        ///     Busca todos los clientes.
        /// </summary>
        /// <returns>La lista de clientes.</returns>
        public List<Cliente> BuscarClientes()
        {
            var clientesList = new List<Cliente>();

            // Posicionar el SDK en el primer registro
            int resultado = FacturaElectronicaSdk.fPosPrimerCteProv();
            if (resultado == SdkConstantes.CodigoExito)
                // Leer los datos del registro donde el SDK esta posicionado
                clientesList.Add(LeerDatosCliente());
            else
                return clientesList;

            // Crear un loop y posicionar el SDK en el siguiente registro
            while (FacturaElectronicaSdk.fPosSiguienteCteProv() == SdkConstantes.CodigoExito)
            {
                // Leer los datos del registro donde el SDK esta posicionado
                clientesList.Add(LeerDatosCliente());

                // Checar si el SDK esta posicionado en el ultimo registro
                // Si el SDK esta posicionado en el ultimo registro salir del loop
                if (FacturaElectronicaSdk.fPosEOFCteProv() == 1) break;
            }

            return clientesList;
        }

        /// <summary>
        ///     Crea un cliente nuevo.
        /// </summary>
        /// <param name="cliente">Cliente del cual se asignaran los datos.</param>
        /// <returns>El id del cliente creado.</returns>
        public int CrearCliente(Cliente cliente)
        {
            // Instanciar un cliente con la estructura tCteProv del SDK
            var clienteNuevo = new tCteProv
            {
                cCodigoCliente = cliente.Codigo,
                cRazonSocial = cliente.RazonSocial,
                cRFC = cliente.Rfc,
                cTipoCliente = cliente.Tipo,
                cFechaAlta = DateTime.Today.ToString(FormatosFechaSdk.Fecha),
                cEstatus = 1
            };

            // Declarar una variable donde se asignara el id del cliente nuevo
            var clienteNuevoId = 0;

            // Crear cliente nuevo
            FacturaElectronicaSdk.fAltaCteProv(ref clienteNuevoId, ref clienteNuevo).TirarSiEsError();

            return clienteNuevoId;
        }

        /// <summary>
        ///     Elimina un cliente.
        /// </summary>
        /// <param name="cliente">El cliente a eliminar.</param>
        public void EliminarCliente(Cliente cliente)
        {
            // Buscar el cliente por código
            // Si el cliente existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscaCteProv(cliente.Codigo).TirarSiEsError();

            // Borrar el registro de la base de datos 
            FacturaElectronicaSdk.fBorraCteProv().TirarSiEsError();
        }

        /// <summary>
        ///     Lee los datos del cliente donde el SDK esta posicionado.
        /// </summary>
        /// <returns>Regresa un cliente con los sus datos asignados.</returns>
        private Cliente LeerDatosCliente()
        {
            // Declarar variables a leer de la base de datos
            var idBd = new StringBuilder(3000);
            var codigoBd = new StringBuilder(3000);
            var razonSocialBd = new StringBuilder(3000);
            var rfcBd = new StringBuilder(3000);
            var tipoBd = new StringBuilder(3000);

            // Leer los datos del registro donde el SDK esta posicionado
            FacturaElectronicaSdk.fLeeDatoCteProv("CIDCLIENTEPROVEEDOR", idBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoCteProv("CCODIGOCLIENTE", codigoBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoCteProv("CRAZONSOCIAL", razonSocialBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoCteProv("CRFC", rfcBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoCteProv("CTIPOCLIENTE", tipoBd, 3000).TirarSiEsError();

            // Instanciar un cliente y asignar los datos de la base de datos
            return new Cliente
            {
                Id = int.Parse(idBd.ToString()),
                Codigo = codigoBd.ToString(),
                RazonSocial = razonSocialBd.ToString(),
                Rfc = rfcBd.ToString(),
                Tipo = int.Parse(tipoBd.ToString())
            };
        }

    }
}
