using System;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using CompaqWebAPI.Models;


namespace CompaqWebAPI.Core.Interfaces
{
    public interface IMovimientoService
    {
        public void ActualizarMovimiento(Movimiento movimiento);

        /// <summary>
        ///     Busca un movimiento por id.
        /// </summary>
        /// <param name="movimientoId">El id del movimiento a buscar.</param>
        /// <returns>Un movimiento con sus datos asignados.</returns>
        public Movimiento BuscarMovimientoPorId(int movimientoId);

        /// <summary>
        ///     Busca movimientos por filtro.
        /// </summary>
        /// <param name="documentoId">El id del documento utilizado para filtrar.</param>
        /// <returns>Lista de movimientos del documento.</returns>
        public List<Movimiento> BuscarMovimientosPorFiltro(int documentoId);


        /// <summary>
        ///     Crea un nuevo movimiento.
        /// </summary>
        /// <param name="movimiento">Movimiento a crear.</param>
        /// <returns></returns>
        public int CrearMovimiento(Movimiento movimiento);

        /// <summary>
        ///     Eliminar un movimiento.
        /// </summary>
        /// <param name="movimiento">El movimiento a eliminar.</param>
        public void EliminarMovimiento(Movimiento movimiento);

    }
}
