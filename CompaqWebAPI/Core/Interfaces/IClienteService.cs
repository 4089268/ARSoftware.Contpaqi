using System.Text;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.Interfaces
{
    public interface IClienteService
    {
        /// <summary>
        ///     Actualiza un cliente.
        /// </summary>
        /// <param name="cliente">Cliente del que se asignaran los datos a modificar.</param>
        public void ActualizarCliente(Cliente cliente);

        /// <summary>
        ///     Busca un cliente por código.
        /// </summary>
        /// <param name="clienteCodigo">El código del cliente a buscar.</param>
        /// <returns>El cliente a buscar.</returns>
        public Cliente BuscarClientePorCodigo(string clienteCodigo);

        /// <summary>
        ///     Busca un cliente por id.
        /// </summary>
        /// <param name="clienteId">El id del cliente a buscar.</param>
        /// <returns>El cliente a buscar.</returns>
        public Cliente BuscarClientePorId(int clienteId);

        /// <summary>
        ///     Busca todos los clientes.
        /// </summary>
        /// <returns>La lista de clientes.</returns>
        public List<Cliente> BuscarClientes();

        /// <summary>
        ///     Crea un cliente nuevo.
        /// </summary>
        /// <param name="cliente">Cliente del cual se asignaran los datos.</param>
        /// <returns>El id del cliente creado.</returns>
        public int CrearCliente(Cliente cliente);

        /// <summary>
        ///     Elimina un cliente.
        /// </summary>
        /// <param name="cliente">El cliente a eliminar.</param>
        public void EliminarCliente(Cliente cliente);
    }
}
