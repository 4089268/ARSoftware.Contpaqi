using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Core;

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
