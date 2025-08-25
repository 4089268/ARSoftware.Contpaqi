using System.ComponentModel.DataAnnotations;

namespace CompaqWebAPI.DTO
{
    public class NuevoProductoRequest
    {
        [Required]
        public string? Codigo { get; set; }


        [Required]
        /// <summary>
        ///     CNOMBREPRODUCTO - Nombre del producto.
        /// </summary>
        public string? Nombre { get; set; }


        [Required]
        /// <summary>
        ///     Campo CTIPOPRODUCTO - Tipo de producto: 1 = Producto, 2 = Paquete, 3 = Servicio
        /// </summary>
        public int? Tipo { get; set; } = 0;
    }
}
