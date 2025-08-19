using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WebAPI.Core;


namespace WebAPI.Controllers
{
    [Route("api/empresas")]
    [ApiController]
    public class EmpresaController(ILogger<EmpresaController> logger) : ControllerBase
    {
        private readonly ILogger<EmpresaController> logger = logger;

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<string> companies = Array.Empty<string>();
            try
            {
                logger.LogInformation("Iniciando SDK");
                ConexionSDK.IniciarSdk("SUPERVISOR", "");
                companies = EmpresaSdk.BuscarEmpresas().Select(e => string.Format("{0}|{1}", e.Id, e.Nombre)).ToList();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener las empresas: {message}", ex.Message);
            }
            finally
            {
                ConexionSDK.TerminarSdk();
            }

            logger.LogInformation("Total empresas {total}", companies.Count());

            return Ok(new
            {
                companies
            });
        }
    }
}
