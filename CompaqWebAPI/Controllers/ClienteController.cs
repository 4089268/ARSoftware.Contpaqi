using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI.Adapters;
using WebAPI.Core;
using WebAPI.DTO;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Helpers;

namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/clientes")]
    [ApiController]
    [ServiceFilter(typeof(InitSDKActionFilter))]
    public class ClienteController(ILogger<ClienteController> logger, IEmpresaService es) : ControllerBase
    {
        private readonly ILogger<ClienteController> logger = logger;
        private readonly IEmpresaService empresaService = es;

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
            ClienteResponse? cliente = null;
            string errorMessage = string.Empty;
            try
            {
                cliente = ClienteSdk.BuscarClientePorCodigo(codigoCliente).ToResponse();
                cliente.TipoDesc = this.tiposCliente.FirstOrDefault(elem => elem["Id"].Equals(cliente.Tipo))?["Nombre"].ToString() ?? String.Empty;
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "");
                Console.WriteLine(ex.Message);
                errorMessage = ex.Message;
            }
            
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
            // * Validate the request
            if (!ModelState.IsValid)
            {
                var _errors = ModelState
                    .Where( ms => ms.Value?.Errors.Any() == true)
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
                cliente.Id = ClienteSdk.CrearCliente(cliente);
                logger.LogInformation("Nuevo cliente generado {razonSocial}", cliente.RazonSocial);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al generar el cliente nuevo");
                errorMessage = ex.Message;
            }

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

        [HttpPatch("{codigoCliente}")]
        public IActionResult ActualizarCliente( [FromRoute] int empresaId, [FromRoute] string codigoCliente, [FromBody] ActualizarClienteRequest request)
        {
            // * obtener cliente
            ClienteSdk? cliente = null;
            string errorMessage = string.Empty;
            try
            {
                cliente = ClienteSdk.BuscarClientePorCodigo(codigoCliente);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener los datos del cliente: {message}", ex.Message);
                Console.WriteLine(ex.Message);
                errorMessage = ex.Message;
            }

            if(cliente == null)
            {
                return NotFound(new
                {
                    Title = "El cliente no se encuentra registrado en el sistema."
                });
            }


            // * actualizar datos cliente
            // TODO: Aply some validations like RFC match the same structure, etc.
            try
            {
                if(!string.IsNullOrEmpty(request.RFC))
                {
                    cliente.Rfc = request.RFC.Trim();
                }
                if (!string.IsNullOrEmpty(request.RazonSocial))
                {
                    cliente.RazonSocial = request.RazonSocial.Trim();
                }
                if (request.Tipo != 0)
                {
                    cliente.Tipo = request.Tipo;
                }
                ClienteSdk.ActualizarCliente(cliente);
                this.logger.LogInformation("Cliente {codigo}|{nombre} actualizado", cliente.Codigo, cliente.RazonSocial);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al actualizar los datos del cliente: {message}", ex.Message);
                errorMessage = ex.Message;
            }

            if(!string.IsNullOrEmpty(errorMessage))
            {
                return Conflict(new
                {
                    Title = "Error al actualizar el cliente",
                    Message = errorMessage
                });
            }

            return Ok(new
            {
                Title = "Cliente actualizado"
            });
        }

        [HttpDelete("{codigoCliente}")]
        public IActionResult EliminarCliente([FromRoute] int empresaId, [FromRoute] string codigoCliente)
        {
            // * obtener cliente
            ClienteSdk? cliente = null;
            string errorMessage = string.Empty;
            try
            {
                cliente = ClienteSdk.BuscarClientePorCodigo(codigoCliente);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener el cliente: {message}", ex.Message);
                errorMessage = ex.Message;
            }

            if (cliente == null)
            {
                return NotFound(new
                {
                    Title = "El cliente no se encuentra registrado en el sistema."
                });
            }

            // * eliminando cliente
            try
            {
                ClienteSdk.EliminarCliente(cliente);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al tratar de eliminar el cliente:{message}", ex.Message);
            }

            return Ok(new
            {
                Title = "Eliminar cliente"
            });
        }

    }
}
