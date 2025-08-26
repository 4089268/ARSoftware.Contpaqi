using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using CompaqWebAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Core;
using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/facturas")]
    [ApiController]
    [ServiceFilter(typeof(InitSDKActionFilter))]
    public class FacturaController(ILogger<FacturaController> logger) : ControllerBase
    {
        private readonly ILogger<FacturaController> logger = logger;


        [HttpPost]
        public ActionResult<DocumentoSdk> GenerarFactura([FromRoute] int empresaId, [FromBody] NuevaFacturaRequest nuevaFacturaRequest)
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
                ConceptoSdk concepto = ConceptoSdk.BuscarConceptoPorCodigo(nuevaFacturaRequest.CodigoConcepto!);
                this.logger.LogDebug("Concepto seleccionado: {cliente}", concepto.Nombre);

                ClienteSdk cliente = ClienteSdk.BuscarClientePorCodigo(nuevaFacturaRequest.CodigoCliente!);
                this.logger.LogDebug("Cliente seleccionado: {cliente}", cliente.RazonSocial);

                tLlaveDoc siguienteFolio = DocumentoSdk.BuscarSiguienteSerieYFolio(concepto.Codigo);

                var nuevoDocumento = new DocumentoSdk
                {
                    ConceptoId = concepto.Id,
                    Fecha = DateTime.Today,
                    Serie = siguienteFolio.aSerie,
                    Folio = siguienteFolio.aFolio,
                    ClienteId = cliente.Id,
                    Referencia = nuevaFacturaRequest.Referencia ?? string.Empty,
                    Observaciones = nuevaFacturaRequest.Observaciones ?? string.Empty,
                };
                documentoId = DocumentoSdk.CrearDocumento(nuevoDocumento);

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
            DocumentoSdk? documentoResp = null;
            try
            {
                documentoResp = DocumentoSdk.BuscarDocumentoPorId(documentoId);
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
            ProductoSdk producto = ProductoSdk.BuscarProductoPorCodigo(request.CodigoProducto!);
            this.logger.LogInformation("  Producto seleccionado: {producto}", producto.Nombre);
            var movimiento = new MovimientoSdk
            {
                DocumentoId = documentoId,
                ProductoId = producto.Id,
                Unidades = request.Unidades!.Value,
                Precio = Convert.ToDouble(request.PrecioUnidad!.Value),
                Referencia = referencia ?? string.Empty,
                Observaciones = observaciones ?? string.Empty
            };
            var idMovimiento = MovimientoSdk.CrearMovimiento(movimiento);
            return idMovimiento;
        }
        #endregion

    }
}
