using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Adapters;
using WebAPI.Core;
using WebAPI.DTO;

namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/productos")]
    [ApiController]
    public class ProductoController(ILogger<ProductoController> logger) : ControllerBase
    {
        private readonly ILogger<ProductoController> logger = logger;

        // TODO: Move this to a Enum
        private readonly ICollection<IDictionary<string, object>> tiposProductos = new Dictionary<string, object>[]
        {
            new () { { "Id", 1 }, { "Nombre", "Producto" } },
            new () { { "Id", 2 },{ "Nombre", "Paquete" } },
            new () {{ "Id", 3 }, { "Nombre", "Servicio" } }
        };


        [HttpGet]
        public ActionResult<IEnumerable<ProductoResponse>> ObtenerProductos([FromRoute] int empresaId)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }
            catch(KeyNotFoundException)
            {
                return BadRequest(new { Title = "Empresa no disponible" });
            }


            List<ProductoResponse> productos = new();
            string? errorMessage = null;
            try
            {
                var _productosSdk = ProductoSdk.BuscarProductos();
                foreach(var prod in _productosSdk)
                {
                    var productoResp = prod.ToResponse();
                    try
                    {
                        productoResp.TipoDesc = this.tiposProductos.FirstOrDefault(el => el["Id"].Equals(prod.Tipo))?["Nombre"].ToString() ?? string.Empty;
                    }
                    catch (Exception) { }
                    productos.Add(productoResp);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener el listado de productos: {message}", ex.Message);
                errorMessage = ex.Message;
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
                    Title = "Error al obtener los productos",
                    Message = errorMessage
                });
            }
            
            return Ok(productos);
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
