using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Core;
using CompaqWebAPI.Core.Interfaces;

namespace CompaqWebAPI.Helpers
{
    public class InitSDKActionFilter : IActionFilter
    {
        private readonly IEmpresaService empresaService;
        private readonly ILogger<InitSDKActionFilter> logger;

        public InitSDKActionFilter(IEmpresaService empresaService, ILogger<InitSDKActionFilter> l)
        {
            this.empresaService = empresaService;
            this.logger = l;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Extract the route-bound variable
            if (context.ActionArguments.TryGetValue("empresaId", out var value))
            {
                int empresaId = Convert.ToInt32(value);

                // TODO: Wrap the ConexionSDK on a service
                ConexionSDK.IniciarSdk("SUPERVISOR", "");
                var empresaSeleccionada = this.empresaService.BuscarEmpresas().FirstOrDefault(item => item.Id == empresaId);
                if(empresaSeleccionada == null)
                {
                    context.Result = new ObjectResult(new { Title = "La empresa solicitada no existe." })
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }

                try
                {
                    ConexionSDK.AbrirEmpresa(empresaSeleccionada!.Ruta);
                }
                catch(Exception ex)
                {
                    this.logger.LogError(ex, "Error al inicializar la empresa: {message}", ex.Message);
                    context.Result = new ObjectResult(new { Title = "Error al inicializar la empresa", ex.Message })
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                }

                this.logger.LogDebug("[Filter] Before action, EmpresaId = {empresaId}", empresaId);
            }
            else
            {
                context.Result = new ObjectResult(new { Title = "No se especifico una empresa valida." })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al finalizar la conexion con el SDK.");
            }
        }
    }
}
