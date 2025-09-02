using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Core.Comercial;

namespace CompaqWebAPI.Helpers
{
    public class InitSDKActionFilter : IActionFilter
    {
        private readonly ILogger<InitSDKActionFilter> logger;
        private readonly IConexionSDK conexionSDK;
        private readonly IEmpresaService empresaService;


        public InitSDKActionFilter(ILogger<InitSDKActionFilter> l, IConexionSDK conexionSDK, IEmpresaService empresaService)
        {
            this.logger = l;
            this.conexionSDK = conexionSDK;
            this.empresaService = empresaService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Extract the route-bound variable
            if (context.ActionArguments.TryGetValue("empresaId", out var value))
            {
                int empresaId = Convert.ToInt32(value);
                this.logger.LogInformation("Empresa seleccionada [{empresaId}]", value);

                // validate the selected id
                if (empresaId <= 1)
                {
                    context.Result = new ObjectResult( new { Title = "La empresa seleccionada es invalida."})
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    return;
                }


                conexionSDK.IniciarSdk("SUPERVISOR", "");
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
                    conexionSDK.AbrirEmpresa(empresaSeleccionada!.Ruta);
                }
                catch(Exception ex)
                {
                    conexionSDK.TerminarSdk();
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
                conexionSDK.CerrarEmpresa();
                conexionSDK.TerminarSdk();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al finalizar la conexion con el SDK.");
            }
        }
    }
}
