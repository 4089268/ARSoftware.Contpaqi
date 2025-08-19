using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using WebAPI.Core;
using WebAPI.Adapters;
using WebAPI.DTO;


namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/clientes")]
    [ApiController]
    public class ClienteController(ILogger<ClienteController> logger) : ControllerBase
    {
        private readonly ILogger<ClienteController> logger = logger;

        // TODO: Move this to a Enum
        private readonly ICollection<IDictionary<string, object>> tiposCliente = new Dictionary<string, object>[]
        {
            new () { { "Id", 1 }, { "Nombre", "Cliente" } },
            new () { { "Id", 2 },{ "Nombre", "Cliente/Proveedor" } },
            new () {{ "Id", 3 }, { "Nombre", "Proveedor" } }
        };


        [HttpGet]
        public IActionResult Clientes([FromRoute] int empresaId)
        {
            try
            {
                this.AbrirEmpresa(empresaId);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest(new { Title = "Empresa no disponible" });
            }
            
            IEnumerable<ClienteSdk> clientes = Array.Empty<ClienteSdk>();
            string? errorMessage = null;
            try
            {
                clientes = ClienteSdk.BuscarClientes();
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener el listado de clientes");
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
                    Title = "Error al obtener los clientes",
                    Message = errorMessage
                });
            }

            return Ok(new
            {
                Clientes = clientes,
            });
        }

        [HttpGet("{codigoCliente}")]
        public IActionResult ConsultaClientePorCodigo([FromRoute] int empresaId, [FromRoute] string codigoCliente)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest( new { Title = "Empresa no disponible" });
            }

            ClienteResponse? cliente = null;
            string errorMessage = string.Empty;
            try
            {
                Console.WriteLine($"Obteniendo datos del cliente: {codigoCliente}");
                cliente = ClienteSdk.BuscarClientePorCodigo(codigoCliente).ToResponse();
                cliente.TipoDesc = this.tiposCliente.FirstOrDefault(elem => elem["Id"].Equals(cliente.Tipo))?["Nombre"].ToString() ?? String.Empty;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorMessage = ex.Message;
            }
            
            ConexionSDK.CerrarEmpresa();
            ConexionSDK.TerminarSdk();

            if(cliente != null)
            {
                return Ok( new
                    {
                        cliente
                    }
                );
            }
            else
            {
                return Conflict( new
                {
                    Title = "Error al obtener los datos del cliente",
                    Message = errorMessage
                });
            }
        }

        [HttpPost]
        public IActionResult CrearCliente([FromRoute] int empresaId, [FromBody] NuevoClienteRequest request)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }
            catch (KeyNotFoundException)
            {
                return BadRequest(new { Title = "Empresa no disponible" });
            }

            // * Validate the request
            if (!ModelState.IsValid)
            {
                var _errors = ModelState
                    .Where( ms => ms.Value.Errors.Any())
                    .ToDictionary(ms => ms.Key, ms => ms.Value?.Errors.Select( e => string.IsNullOrEmpty(e.ErrorMessage) ? "Invalid value." : e.ErrorMessage).ToArray());
                
                return UnprocessableEntity(new
                {
                    Title = "Uno o mas campose son invalidos",
                    Errors = _errors
                });
            }
            
            // * create the new client
            ClienteSdk cliente = request.ToEntity();
            string? errorMessage = null;
            try
            {
                logger.LogInformation("Generando nuevo cliente: {razonSocial}", cliente.RazonSocial);
                cliente.Id = ClienteSdk.CrearCliente(cliente);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al generar el cliente nuevo");
                errorMessage = ex.Message;
            }

            ConexionSDK.CerrarEmpresa();
            ConexionSDK.TerminarSdk();

            if (string.IsNullOrEmpty(errorMessage))
            {
                return StatusCode(201, new
                {
                    Title = "Cliente genearado",
                    cliente
                });
            }
            else
            {
                return Conflict(new
                {
                    Title = "Error al generar el nuevo cliente",
                    Message = errorMessage
                });
            }
        }

        [HttpGet("catalogo/tipos-cliente")]
        public IActionResult TiposCliente([FromRoute] int empresaId)
        {
            return Ok( new {
                TiposCliente = tiposCliente
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
