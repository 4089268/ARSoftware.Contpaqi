using System;
using Microsoft.AspNetCore.Mvc;
using CompaqWebAPI.DTO;
using CompaqWebAPI.Helpers;
using CompaqWebAPI.Models;
using WebAPI.Adapters;
using WebAPI.DTO;
using CompaqWebAPI.Core.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/{empresaId}/productos")]
    [ApiController]
    [ServiceFilter(typeof(InitSDKActionFilter))]
    public class ProductoController(ILogger<ProductoController> logger, IProductoService pService) : ControllerBase
    {
        private readonly ILogger<ProductoController> logger = logger;
        private readonly IProductoService productoService = pService;

        // TODO: Move this to a Enum
        private readonly ICollection<IDictionary<string, object>> tiposProductos = new Dictionary<string, object>[]
        {
            new () { { "Id", 1 }, { "Nombre", "Producto" } },
            new () { { "Id", 2 }, { "Nombre", "Paquete" } },
            new () { { "Id", 3 }, { "Nombre", "Servicio" } }
        };


        [HttpGet]
        public ActionResult<IEnumerable<ProductoResponse>> ObtenerProductos([FromRoute] int empresaId)
        {
            List<ProductoResponse> productos = new();
            try
            {
                var _productosSdk = this.productoService.BuscarProductos();
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
                return Ok(productos);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al obtener el listado de productos: {message}", ex.Message);
                return Conflict(new
                {
                    Title = "Error al obtener los productos",
                    ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult CrearProducto([FromRoute] int empresaId, NuevoProductoRequest request)
        {
            Producto productoSdk = new();
            try
            {
                productoSdk = request.ToEntity();
                this.productoService.CrearProducto(productoSdk);
                this.logger.LogInformation("Nuevo producto creado");
                return StatusCode(201, new
                {
                    Title = "Producto generado",
                    Producto = productoSdk
                });
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al genearr el nuevo producto: {message}", ex.Message);
                return Conflict(new
                {
                    Title = "Error al generar el nuevo producto",
                    ex.Message
                });
            }
        }

        [HttpPatch("{codigoProducto}")]
        public IActionResult ActualizarProducto([FromRoute] int empresaId, [FromRoute] string codigoProducto, ActualizarProductoRequest request)
        {
            // * buscar producto
            Producto? producto = null;
            try
            {
                producto = this.productoService.BuscarProductoPorCodigo(codigoProducto);
                if(producto == null)
                {
                    return NotFound(new
                    {
                        Title = "El producto no se encuentra registrado en el sistema"
                    });
                }
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al buscar el producto: {message}", ex.Message);
                return Conflict(new
                {
                    Title = "Error al buscar el producto",
                    ex.Message
                });
            }

            // * actualizar producto
            try
            {
                if(!string.IsNullOrEmpty(request.Nombre))
                {
                    producto.Nombre = request.Nombre;
                }

                if (request.Tipo != null)
                {
                    producto.Tipo = request.Tipo.Value;
                }

                this.productoService.ActualizarProducto(producto);
                this.logger.LogInformation("Producto {codigo}|{nombre} actualizado", producto.Codigo, producto.Nombre);

                return Ok(new
                {
                    Title = "Producto actualizado"
                });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al actualizaz el producto: {message}", ex.Message);
                return Conflict(new
                {
                    Title = "Error al actualizar el producto",
                    ex.Message
                });
            }
        }

        [HttpGet("catalogo/tipo-productos")]
        public IActionResult ObtenerCatalogoTipoProducto([FromRoute] int empresaId)
        {
            return Ok(new
            {
                tiposProductos
            });
        }
    }
}
