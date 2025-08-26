using System;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.Interfaces
{
    public interface IProductoService
    {
        /// <summary>
        ///     Actualiza un producto.
        /// </summary>
        /// <param name="producto">Producto del que se asignaran los datos a modificar.</param>
        public void ActualizarProducto(Producto producto);

        /// <summary>
        ///     Busca un producto por código.
        /// </summary>
        /// <param name="productoCodigo">El código del producto a buscar.</param>
        /// <returns>El producto a buscar.</returns>
        public Producto BuscarProductoPorCodigo(string productoCodigo);

        /// <summary>
        ///     Busca un producto por id.
        /// </summary>
        /// <param name="productoId">El id del producto a buscar.</param>
        /// <returns>El producto a buscar.</returns>
        public Producto BuscarProductoPorId(int productoId);

        /// <summary>
        ///     Busca todos los productos.
        /// </summary>
        /// <returns>La lista de productos.</returns>
        public List<Producto> BuscarProductos();

        /// <summary>
        ///     Crea un producto nuevo.
        /// </summary>
        /// <param name="producto">Producto del cual se asignaran los datos.</param>
        /// <returns>El id del producto creado.</returns>
        public int CrearProducto(Producto producto);

        /// <summary>
        ///     Elimina un producto.
        /// </summary>
        /// <param name="producto">El producto a eliminar.</param>
        public void EliminarProducto(Producto producto);

    }
}
