using CompaqWebAPI.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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
            new () { { "Id", 3 }, { "Nombre", "Servicio" } }
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

        [HttpPost]
        public IActionResult CrearProducto([FromRoute] int empresaId, NuevoProductoRequest request)
        {
            try
            {
                this.AbrirEmpresa(empresaId);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al inicializar la empresa");
                return BadRequest(new { Title = "Empresa no disponible" });
            }

            string? errorMessage = null;
            ProductoSdk productoSdk = new();
            try
            {
                productoSdk = request.ToEntity();
                ProductoSdk.CrearProducto(productoSdk);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al genearr el nuevo producto: {message}", ex.Message);
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
                    Title = "Error al generar el nuevo producto",
                    Message = errorMessage
                });
            }

            return StatusCode(201, new
            {
                Title = "Producto generado",
                Producto = productoSdk
            });

        }

        [HttpPatch("{codigoProducto}")]
        public IActionResult ActualizarProducto([FromRoute] int empresaId, [FromRoute] string codigoProducto, ActualizarProductoRequest request)
        {
            try
            {
                AbrirEmpresa(empresaId);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al abrir la empresa");
                return BadRequest(new
                {
                    Title = "Empresa no disponible"
                });
            }


            // * buscar producto
            ProductoSdk? producto = null;
            string? errorMessage = null;
            try
            {
                producto = ProductoSdk.BuscarProductoPorCodigo(codigoProducto);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex, "Error al buscar el producto: {message}", ex.Message);
                errorMessage = ex.Message;
            }

            if(!string.IsNullOrEmpty(errorMessage))
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
                return Conflict(new
                {
                    Title = "Error al buscar el producto",
                    Message = errorMessage
                });
            }

            if(producto == null)
            {
                ConexionSDK.CerrarEmpresa();
                ConexionSDK.TerminarSdk();
                return NotFound(new
                {
                    Title = "El producto no se encuentra registrado en el sistema"
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

                ProductoSdk.ActualizarProducto(producto);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al actualizaz el producto: {message}", ex.Message);
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
                    Title = "Error al actualizar el producto"
                });
            }

            return Ok( new
            {
                Title = "Producto actualizado"
            });
        }

       [HttpGet("catalogo/tipo-productos")]
        public IActionResult ObtenerCatalogoTipoProducto([FromRoute] int empresaId)
        {
            return Ok(new
            {
                tiposProductos
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
