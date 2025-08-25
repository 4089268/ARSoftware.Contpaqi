using CompaqWebAPI.DTO;
using System;
using WebAPI.Core;
using WebAPI.DTO;

namespace WebAPI.Adapters
{
    public static class ProductoAdapter
    {
        public static ProductoResponse ToResponse(this ProductoSdk producto)
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

        public static ProductoSdk ToEntity(this NuevoProductoRequest request)
        {
            var producto = new ProductoSdk
            {
                Codigo = request.Codigo ?? string.Empty,
                Nombre = request.Nombre ?? string.Empty,
                Tipo = request.Tipo!.Value
            };
            return producto;
        }
    }
}
