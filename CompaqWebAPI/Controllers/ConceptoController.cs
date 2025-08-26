using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core;
using CompaqWebAPI.Adapters;
using CompaqWebAPI.DTO;
using CompaqWebAPI.Helpers;


namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/conceptos")]
    [ApiController]
    [ServiceFilter(typeof(InitSDKActionFilter))]
    public class ConceptoController(ILogger<ConceptoController> logger) : ControllerBase
    {
        private readonly ILogger<ConceptoController> logger = logger;

        [HttpGet]
        public IActionResult Conceptos([FromRoute] int empresaId)
        {
            IEnumerable<ConceptoSdk> conceptos = Array.Empty<ConceptoSdk>();
            try
            {
                conceptos = ConceptoSdk.BuscarConceptos();
                return Ok(new
                {
                    Conceptos = conceptos
                });
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener los conceptos");
                return Conflict(new
                {
                    Title = "Error al obtener los conceptos",
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{codigoConcepto}")]
        public IActionResult ConsultaConceptoPorCodigo([FromRoute] int empresaId, [FromRoute] string codigoConcepto)
        {
            ConceptoSdk? concepto = null;
            try
            {
                concepto = ConceptoSdk.BuscarConceptoPorCodigo(codigoConcepto);
                return Ok(new
                {
                    concepto
                });
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al buscar el concepto por codigo: {meesage}", err.Message);
                return Conflict(new
                {
                    Title = "Error al consutlar el concepto por codigo",
                    err.Message
                });
            }
        }

        [HttpPost]
        public IActionResult AlmacenarConcepto([FromRoute] int empresaId, [FromBody] NuevoConceptoRequest request)
        {
            // * no existe funciona para generar nuevos concpetos en SDK
            return StatusCode(StatusCodes.Status501NotImplemented, "Este endpoint no esta disponible");   
        }

        [HttpPatch("{codigoConcepto}")]
        public IActionResult ActualizarConcpeto([FromRoute] int empresaId, [FromRoute] string codigoConcepto, ActualizarConceptoRequest request)
        {
            // * search for the target concept
            ConceptoSdk? conceptoSdk = null;
            try
            {
                conceptoSdk = ConceptoSdk.BuscarConceptoPorCodigo(codigoConcepto);
                if (conceptoSdk == null)
                {
                    return NotFound(new
                    {
                        Titlte = "El concepto no se encuentra registrado en la empresa"
                    });
                }
            }
            catch (Exception err)
            {
                this.logger.LogError(err, "Error al obtener el concepto por codigo: {message}", err.Message);
                return Conflict(new
                {
                    Titlte = "Error al tratar de obtener el concepto",
                    err.Message
                });
            }

            // * update the properties of the record
            try
            {
                conceptoSdk.Nombre = request.Nombre ?? conceptoSdk.Nombre;
                conceptoSdk.Serie = request.Serie ?? conceptoSdk.Serie;
                conceptoSdk.RutaEntrega = request.RutaEntrega ?? conceptoSdk.RutaEntrega;
                conceptoSdk.PrefijoConcepto = request.PrefijoConcepto ?? conceptoSdk.PrefijoConcepto;
                conceptoSdk.PlantillaFormatoDigital = request.PlantillaFormatoDigital ?? conceptoSdk.PlantillaFormatoDigital;
                ConceptoSdk.ActualizarConcepto(conceptoSdk);
                this.logger.LogInformation("Concepto {codigo}|{nombre} actualizado", conceptoSdk.Codigo, conceptoSdk.Nombre);

                return Ok(new
                {
                    Title = "Concepto actualizado"
                });
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al actualizar el concepto:{message}", err.Message);
                return Conflict(new
                {
                    Title = "Error al actualizar el concepto",
                    err.Message
                });
            }
        }

        [HttpGet("catalogo/plantillas-digitales")]
        public IActionResult ObtenerCatalogoPlantillasDigitales([FromRoute] int empresaId)
        {
            // * no existe funcion para obtener el listado de las plantillas desde el .dll
            // TODO: Generar un catalogo estatico de plantillas?
            return StatusCode(StatusCodes.Status501NotImplemented, "Este endpoint no esta disponible");
        }

    }
}
