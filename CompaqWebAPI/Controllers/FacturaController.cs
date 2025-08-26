using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Helpers;
using CompaqWebAPI.Models;
using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/facturas")]
    [ApiController]
    [ServiceFilter(typeof(InitSDKActionFilter))]
    public class FacturaController(ILogger<FacturaController> logger, IConceptoService cService, IMovimientoService mService, IProductoService pService, IDocumentoService dService, IClienteService ccService) : ControllerBase
    {
        private readonly ILogger<FacturaController> logger = logger;
        private readonly IConceptoService conceptoService = cService;
        private readonly IMovimientoService movimientoService = mService;
        private readonly IProductoService productoService = pService;
        private readonly IDocumentoService documentoService = dService;
        private readonly IClienteService clienteService = ccService;


        [HttpPost]
        public ActionResult<Documento> GenerarFactura([FromRoute] int empresaId, [FromBody] NuevaFacturaRequest nuevaFacturaRequest)
        {
            // * validate the request
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            // TODO: Validate if the client and concept exist

            // * generar factura
            int documentoId = 0;
            try
            {
                Concepto concepto = this.conceptoService.BuscarConceptoPorCodigo(nuevaFacturaRequest.CodigoConcepto!);
                this.logger.LogDebug("Concepto seleccionado: {cliente}", concepto.Nombre);

                Cliente cliente = this.clienteService.BuscarClientePorCodigo(nuevaFacturaRequest.CodigoCliente!);
                this.logger.LogDebug("Cliente seleccionado: {cliente}", cliente.RazonSocial);

                tLlaveDoc siguienteFolio = this.documentoService.BuscarSiguienteSerieYFolio(concepto.Codigo);

                var nuevoDocumento = new Documento
                {
                    ConceptoId = concepto.Id,
                    Fecha = DateTime.Today,
                    Serie = siguienteFolio.aSerie,
                    Folio = siguienteFolio.aFolio,
                    ClienteId = cliente.Id,
                    Referencia = nuevaFacturaRequest.Referencia ?? string.Empty,
                    Observaciones = nuevaFacturaRequest.Observaciones ?? string.Empty,
                };
                documentoId = this.documentoService.CrearDocumento(nuevoDocumento);

                // * agregar movimientos (conceptos) a la factura
                foreach(var movimiento in nuevaFacturaRequest.Movimientos!)
                {
                    var movimientoId = CrearMovimientoFactura(
                        documentoId,
                        movimiento,
                        nuevaFacturaRequest.Referencia,
                        nuevaFacturaRequest.Observaciones
                    );
                }
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al generar la factura: {message}", err.Message);
                return Conflict(new
                {
                    Title = "Error al generar la factura",
                    err.Message
                });
            }


            // * obtener datos de la factura
            Documento? documentoResp = null;
            try
            {
                documentoResp = this.documentoService.BuscarDocumentoPorId(documentoId);
                return Ok( new
                {
                    Titlte = "Factura generada con exito.",
                    Factura = documentoResp
                });
            }
            catch(Exception err)
            {
                return Conflict(new
                {
                    Title = "Error al obtener los datos del documento",
                    err.Message
                });
            }
        }


        [HttpGet("{facturaId}/pdf")]
        public IActionResult ObtenerDocumento([FromRoute] int empresaId, [FromRoute] int facturaId)
        {
            return StatusCode(StatusCodes.Status501NotImplemented, "Este endpoint no esta disponible");
        }

        #region Private methods
        
        private int CrearMovimientoFactura(int documentoId, MovimientoRequest request, string? referencia, string? observaciones)
        {
            this.logger.LogDebug("Generando movimiento de documento {documentoId}", documentoId);
            Producto producto = this.productoService.BuscarProductoPorCodigo(request.CodigoProducto!);
            this.logger.LogInformation("  Producto seleccionado: {producto}", producto.Nombre);
            var movimiento = new Movimiento
            {
                DocumentoId = documentoId,
                ProductoId = producto.Id,
                Unidades = request.Unidades!.Value,
                Precio = Convert.ToDouble(request.PrecioUnidad!.Value),
                Referencia = referencia ?? string.Empty,
                Observaciones = observaciones ?? string.Empty
            };
            var idMovimiento = this.movimientoService.CrearMovimiento(movimiento);
            return idMovimiento;
        }
        #endregion

    }
}
