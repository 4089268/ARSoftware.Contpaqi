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
    }
}
