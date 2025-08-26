using CompaqWebAPI.DTO;
using CompaqWebAPI.Models;
using System;
using WebAPI.DTO;

namespace WebAPI.Adapters
{
    public static class ProductoAdapter
    {
        public static ProductoResponse ToResponse(this Producto producto)
        {
            var productoResponse = new ProductoResponse
            {
                Id = producto.Id,
                Codigo = producto.Codigo,
                Nombre = producto.Nombre,
                Tipo = producto.Tipo,
                TipoDesc = string.Empty
            };
            return productoResponse;
        }

        public static Producto ToEntity(this NuevoProductoRequest request)
        {
            var producto = new Producto
            {
                Codigo = request.Codigo ?? string.Empty,
                Nombre = request.Nombre ?? string.Empty,
                Tipo = request.Tipo!.Value
            };
            return producto;
        }
    }
}
