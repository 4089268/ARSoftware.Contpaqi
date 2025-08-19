using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using WebAPI.Core;
using WebAPI.DTO;
using System.Reflection;

namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/facturas")]
    [ApiController]
    public class FacturaController(ILogger<FacturaController> logger) : ControllerBase
    {
        private readonly ILogger<FacturaController> logger = logger;


        [HttpPost]
        public ActionResult<DocumentoSdk> GenerarFactura([FromRoute] int empresaId, [FromBody] NuevaFacturaRequest nuevaFacturaRequest)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest(new { Title = "Empresa no disponible" });
            }

            // * validate the request
            if(!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            // TODO: Validate if the client and concept exist

            string? errorMessaage = null;

            // * generar factura
            int documentoId = 0;
            try
            {
                ConceptoSdk concepto = ConceptoSdk.BuscarConceptoPorCodigo(nuevaFacturaRequest.CodigoConcepto!);
                this.logger.LogInformation("Concepto seleccionado: {cliente}", concepto.Nombre);

                ClienteSdk cliente = ClienteSdk.BuscarClientePorCodigo(nuevaFacturaRequest.CodigoCliente!);
                this.logger.LogInformation("Cliente seleccionado: {cliente}", cliente.RazonSocial);

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
                errorMessaage = err.Message;
            }

            if (!string.IsNullOrEmpty(errorMessaage))
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();

                return Conflict(new
                {
                    Title = "Error al generar la factura",
                    Message = errorMessaage
                });
            }


            // * obtener datos de la factura
            DocumentoSdk? documentoResp = null;
            try
            {
                documentoResp = DocumentoSdk.BuscarDocumentoPorId(documentoId);
            }
            catch(Exception err)
            {
                errorMessaage = err.Message;
            }
            finally
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }
            
            if(!string.IsNullOrEmpty(errorMessaage))
            {
                return Conflict(new
                {
                    Title = "Error al obtener los datos del documento",
                    Message = errorMessaage
                });
            }

            return Ok(documentoResp!);
        }


        [HttpGet("{facturaId}/pdf")]
        public IActionResult ObtenerDocumento([FromRoute] int empresaId, [FromRoute] int facturaId)
        {
            return Conflict(new { Title = "No implementado" });

            try
            {
                AbrirEmpresa(empresaId);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest(new { Title = "Empresa no disponible" });
            }

            
            string? errorMessaage = null;
            int documentoId = 0;

            // * obtener datos de la factura
            DocumentoSdk? documentoResp = null;
            try
            {
                documentoResp = DocumentoSdk.BuscarDocumentoPorId(documentoId);
            }
            catch (Exception err)
            {
                errorMessaage = err.Message;
            }
            finally
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }

            if (!string.IsNullOrEmpty(errorMessaage))
            {
                return Conflict(new
                {
                    Title = "Error al obtener los datos del documento",
                    Message = errorMessaage
                });
            }

            return Ok(documentoResp!);
        }

        #region Private methods
        /// <summary>
        /// Trata de abrir la empresa seleccionada
        /// </summary>
        /// <param name="empresaId"></param>
        /// <exception cref="KeyNotFoundException">Empresa no encontrada</exception>
        private void AbrirEmpresa(int empresaId)
        {
            ConexionSDK.IniciarSdk("SUPERVISOR", "");
            var empresaSeleccionada = EmpresaSdk.BuscarEmpresas().FirstOrDefault(item => item.Id == empresaId);
            if (empresaSeleccionada == null)
            {
                throw new KeyNotFoundException();
            }
            ConexionSDK.AbrirEmpresa(empresaSeleccionada!.Ruta);
        }

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
