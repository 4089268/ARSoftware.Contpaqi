using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Helpers;
using CompaqWebAPI.Core.Comercial;


namespace WebAPI.Controllers
{

    [Route("api/empresas")]
    [ApiController]
    public class EmpresaController(ILogger<EmpresaController> logger, IConexionSDK conexionSDK, IEmpresaService es) : ControllerBase
    {
        private readonly ILogger<EmpresaController> logger = logger;
        private readonly IConexionSDK conexionSDK = conexionSDK;
        private readonly IEmpresaService empresaService = es;

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<string> companies = Array.Empty<string>();
            try
            {
                logger.LogInformation("Iniciando SDK");
                this.conexionSDK.IniciarSdk("SUPERVISOR", "");
                companies = empresaService.BuscarEmpresas().Select(e => string.Format("{0}|{1}", e.Id, e.Nombre)).ToList();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error al obtener las empresas: {message}", ex.Message);
            }
            finally
            {
                this.conexionSDK.TerminarSdk();
            }

            logger.LogInformation("Total empresas {total}", companies.Count());

            return Ok(new
            {
                companies
            });
        }
    }
}
