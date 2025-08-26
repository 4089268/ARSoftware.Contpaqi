using System;
using System.Text;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.Comercial
{
    public class MovimientoServiceComercial(IProductoService pService) : IMovimientoService
    {
        private readonly IProductoService productoService = pService;


        /// <summary>
        ///     Actualiza los datos de un movimiento.
        /// </summary>
        /// <param name="movimiento">Movimiento con los datos a actualizar.</param>
        public void ActualizarMovimiento(Movimiento movimiento)
        {
            // Buscar el movimiento
            // Si el movimiento existe el SDK se posiciona en el registro
            ComercialSdk.fBuscarIdMovimiento(movimiento.Id).TirarSiEsError();

            // Activar el modo de edición
            ComercialSdk.fEditarMovimiento().TirarSiEsError();

            // Actualizar los campos del registro donde el SDK esta posicionado
            ComercialSdk.fSetDatoMovimiento("COBSERVAMOV", movimiento.Observaciones).TirarSiEsError();

            // Guardar los cambios realizados al registro
            ComercialSdk.fGuardaMovimiento().TirarSiEsError();
        }

        /// <summary>
        ///     Busca un movimiento por id.
        /// </summary>
        /// <param name="movimientoId">El id del movimiento a buscar.</param>
        /// <returns>Un movimiento con sus datos asignados.</returns>
        public Movimiento BuscarMovimientoPorId(int movimientoId)
        {
            // Buscar el movimiento por id
            // Si el movimientos existe el SDK se posiciona en el registro
            ComercialSdk.fBuscarIdMovimiento(movimientoId).TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            return LeerDatosMovimiento();
        }

        /// <summary>
        ///     Busca movimientos por filtro.
        /// </summary>
        /// <param name="documentoId">El id del documento utilizado para filtrar.</param>
        /// <returns>Lista de movimientos del documento.</returns>
        public List<Movimiento> BuscarMovimientosPorFiltro(int documentoId)
        {
            var movimientosList = new List<Movimiento>();

            // Cancelar filtro
            ComercialSdk.fCancelaFiltroMovimiento().TirarSiEsError();

            // Filtrar movimientos
            ComercialSdk.fSetFiltroMovimiento(documentoId).TirarSiEsError();

            // Posicionar el SDK en el primer registro
            ComercialSdk.fPosPrimerMovimiento().TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            movimientosList.Add(LeerDatosMovimiento());

            // Crear un loop y posicionar el SDK en el siguiente registro
            while (ComercialSdk.fPosSiguienteMovimiento() == SdkConstantes.CodigoExito)
            {
                // Leer los datos del registro donde el SDK esta posicionado
                movimientosList.Add(LeerDatosMovimiento());

                // Checar si el SDK esta posicionado en el ultimo registro
                // Si el SDK esta posicionado en el ultimo registro salir del loop
                if (ComercialSdk.fPosMovimientoEOF() == 1) break;
            }

            // Cancelar filtro
            ComercialSdk.fCancelaFiltroMovimiento().TirarSiEsError();

            return movimientosList;
        }

        /// <summary>
        ///     Crea un nuevo movimiento.
        /// </summary>
        /// <param name="movimiento">Movimiento a crear.</param>
        /// <returns></returns>
        public int CrearMovimiento(Movimiento movimiento)
        {
            Producto producto = productoService.BuscarProductoPorId(movimiento.ProductoId);

            // Instanciar un movimiento con la estructura tMovimiento del SDK
            var nuevoMovimiento = new tMovimiento
            {
                aCodProdSer = producto.Codigo,
                aUnidades = movimiento.Unidades,
                aPrecio = movimiento.Precio,
                aReferencia = movimiento.Referencia,
                aCodAlmacen = "1"
            };

            // Declarar una variable donde se asignara el id del movimiento nuevo
            var nuevoMovimientoId = 0;

            // Crear movimiento nuevo
            ComercialSdk.fAltaMovimiento(movimiento.DocumentoId, ref nuevoMovimientoId, ref nuevoMovimiento).TirarSiEsError();

            movimiento.Id = nuevoMovimientoId;

            // Editar los datos extras que no son parte de la estructura tMovimiento
            ActualizarMovimiento(movimiento);

            return nuevoMovimientoId;
        }

        /// <summary>
        ///     Eliminar un movimiento.
        /// </summary>
        /// <param name="movimiento">El movimiento a eliminar.</param>
        public void EliminarMovimiento(Movimiento movimiento)
        {
            // Buscar el movimiento
            // Si el movimiento existe el SDK se posiciona en el registro
            ComercialSdk.fBuscarIdMovimiento(movimiento.Id).TirarSiEsError();

            // Eliminar movimiento
            ComercialSdk.fBorraMovimiento(movimiento.DocumentoId, movimiento.Id).TirarSiEsError();
        }

        /// <summary>
        ///     Lee los datos del movimiento donde el SDK esta posicionado.
        /// </summary>
        /// <returns>Regresa un movimiento con los sus datos asignados.</returns>
        private Movimiento LeerDatosMovimiento()
        {
            // Declarar variables a leer de la base de datos
            var idBd = new StringBuilder(3000);
            var documentoIdBd = new StringBuilder(3000);
            var productoIdBd = new StringBuilder(3000);
            var unidadesBd = new StringBuilder(3000);
            var precioBd = new StringBuilder(3000);
            var referenciaBd = new StringBuilder(3000);
            var observacionesBd = new StringBuilder(3000);
            var totalBd = new StringBuilder(3000);

            // Leer los datos del registro donde el SDK esta posicionado
            ComercialSdk.fLeeDatoMovimiento("CIDMOVIMIENTO", idBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("CIDDOCUMENTO", documentoIdBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("CIDPRODUCTO", productoIdBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("CUNIDADES", unidadesBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("CPRECIO", precioBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("CREFERENCIA", referenciaBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("COBSERVAMOV", observacionesBd, 3000).TirarSiEsError();
            ComercialSdk.fLeeDatoMovimiento("CTOTAL", totalBd, 3000).TirarSiEsError();

            // Instanciar un movimiento y asignar los datos de la base de datos
            return new Movimiento
            {
                Id = int.Parse(idBd.ToString()),
                DocumentoId = int.Parse(documentoIdBd.ToString()),
                ProductoId = int.Parse(productoIdBd.ToString()),
                Unidades = double.Parse(unidadesBd.ToString()),
                Precio = double.Parse(precioBd.ToString()),
                Referencia = referenciaBd.ToString(),
                Observaciones = observacionesBd.ToString(),
                Total = double.Parse(totalBd.ToString())
            };
        }

    }
}
