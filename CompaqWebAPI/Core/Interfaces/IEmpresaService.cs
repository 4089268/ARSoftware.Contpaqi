using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.Interfaces
{
    public interface IEmpresaService
    {

        /// <summary>
        ///     Busca la lista de empresas del sistema.
        /// </summary>
        /// <returns>Lista de empresas del sistema.</returns>
        public List<Empresa> BuscarEmpresas();

    }
}
