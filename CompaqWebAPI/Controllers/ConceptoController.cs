using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core;
using CompaqWebAPI.DTO;
using CompaqWebAPI.Adapters;


namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/conceptos")]
    [ApiController]
    public class ConceptoController(ILogger<ConceptoController> logger) : ControllerBase
    {
        private readonly ILogger<ConceptoController> logger = logger;

        [HttpGet]
        public IActionResult Conceptos([FromRoute] int empresaId)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }catch(KeyNotFoundException)
            {
                return BadRequest(new { Title = "Empresa no disponible." });
            }

            IEnumerable<ConceptoSdk> conceptos = Array.Empty<ConceptoSdk>();
            string? errorMessage = null;
            try
            {
                conceptos = ConceptoSdk.BuscarConceptos();

            }catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener los conceptos");
                errorMessage = ex.Message;
            }
            finally
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }
            
            if(errorMessage != null)
            {
                return Conflict(new
                {
                    Title = "Error al obtener los conceptos",
                    Message = errorMessage
                });
            }

            return Ok(new
            {
                Conceptos = conceptos
            });
        }

        [HttpGet("{codigoConcepto}")]
        public IActionResult ConsultaConceptoPorCodigo([FromRoute] int empresaId, [FromRoute] string codigoConcepto)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest(new {Title ="Empresa no disponible"} );
            }

            ConceptoSdk? concepto = null;
            string? errorMessage = null;
            try
            {
                concepto = ConceptoSdk.BuscarConceptoPorCodigo(codigoConcepto);
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al buscar el concepto por codigo: {meesage}", err.Message);
                errorMessage = err.Message;
            }
            finally
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }

            if(!string.IsNullOrEmpty(errorMessage))
            {
                return Conflict(new
                {
                    Title = "Error al consutlar el concepto por codigo",
                    Message = errorMessage
                });
            }

            return Ok(new
            {
                concepto
            });
        }

        [HttpPost]
        public IActionResult AlmacenarConcepto([FromRoute] int empresaId, [FromBody] NuevoConceptoRequest request)
        {
            // * no existe funciona para generar nuevos concpetos en SDK
            return StatusCode(StatusCodes.Status501NotImplemented, "Este endpoint no esta disponible");

            // * validate the request
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                this.AbrirEmpresa(empresaId);
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al abrir la empresa");
                return BadRequest(new
                {
                    Title = "Empresa no disponible"
                });
            }
            var conceptoSdk = request.ToEntity();


            string? errorMessage = null;
            try
            {
                // TODO: store the new concept
                //ConceptoSdk.
            }
            catch (Exception err)
            {
                this.logger.LogError(err, "Error al almacenar el concepto: {message}", err.Message);
                errorMessage = err.Message;
            }
            finally
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }
            
            // * check if the operation was success
            if(!string.IsNullOrEmpty(errorMessage))
            {
                return Conflict(new
                {
                    Title = "Error al almacenar el concepto",
                    Message = errorMessage
                });
            }

            return StatusCode(201, new
            {
                Title = "Concepto generado"
            });
        }

        [HttpPatch("{codigoConcepto}")]
        public IActionResult ActualizarConcpeto([FromRoute] int empresaId, [FromRoute] string codigoConcepto, ActualizarConceptoRequest request)
        {
            try
            {
                this.AbrirEmpresa(empresaId);
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al abrir la empresa: {message}", err.Message);
                return BadRequest(new
                {
                    Title = "Empresa no disponible"
                });
            }

            // * search for the target concept
            ConceptoSdk? conceptoSdk = null;
            string? errorMessage = null;
            try
            {
                conceptoSdk = ConceptoSdk.BuscarConceptoPorCodigo(codigoConcepto);
            }
            catch (Exception err)
            {
                this.logger.LogError(err, "Error al obtener el concepto por codigo: {message}", err.Message);
                errorMessage = err.Message;
            }

            if(!string.IsNullOrEmpty(errorMessage))
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
                return Conflict(new
                {
                    Titlte = "Error al tratar de obtener el concepto",
                    Message = errorMessage
                });
            }

            if(conceptoSdk == null)
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
                return NotFound(new
                {
                    Titlte = "El concepto no se encuentra registrado en la empresa"
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
            }
            catch(Exception err)
            {
                this.logger.LogError(err, "Error al actualizar el concepto:{message}", err.Message);
                errorMessage = err.Message;
            }
            finally
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }

            if(errorMessage != null)
            {
                return Conflict(new
                {
                    Title = "Error al actualizar el concepto",
                    Message = errorMessage
                });
            }

            return Ok(new
            {
                Title = "Concepto actualizado"
            });
        }

        [HttpGet("catalogo/plantillas-digitales")]
        public IActionResult ObtenerCatalogoPlantillasDigitales([FromRoute] int empresaId)
        {
            // * no existe funcion para obtener el listado de las plantillas desde el .dll
            // TODO: Generar un catalogo estatico de plantillas?
            return StatusCode(StatusCodes.Status501NotImplemented, "Este endpoint no esta disponible");
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
        #endregion

    }
}
