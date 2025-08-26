using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using Microsoft.Win32;

namespace CompaqWebAPI.Core.Interfaces
{
    public interface IConexionSDK
    {
        /// <summary>
        ///     Abre la empresa de trabajo.
        /// </summary>
        /// <param name="rutaEmpresa">Ruta del directorio de la empresa.</param>
        public void AbrirEmpresa(string rutaEmpresa);


        /// <summary>
        ///     Cierra la empresa de trabajo.
        /// </summary>
        public void CerrarEmpresa();


        /// <summary>
        ///     Inicia la conexión con el sistema y muestra una ventana de autenticación donde el usuario podrá ingresar su nombre
        ///     de usuario y contraseña.
        /// </summary>
        public void IniciarSdk();


        /// <summary>
        ///     Inicia la conexión con el sistema e ingresa el usuario y contraseña programáticamente para que no se muestre la
        ///     ventana de autenticación de Comercial.
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario del sistema de Comercial.</param>
        /// <param name="contrasena">Contraseña del sistema de Comercial.</param>
        public void IniciarSdk(string nombreUsuario, string contrasena);


        /// <summary>
        ///     Inicia la conexión con el sistema e ingresa el usuario y contraseña programáticamente para que no se muestre la
        ///     ventana de autenticación de Comercial y Contabilidad.
        /// </summary>
        /// <param name="nombreUsuarioComercial">Nombre de usuario del sistema de Comercial.</param>
        /// <param name="contrasenaComercial">Contraseña del sistema de Comercial.</param>
        /// <param name="nombreUsuarioContabilidad">Nombre de usuario del sistema de Contabilidad.</param>
        /// <param name="contrasenaContabilidad">Contraseña del sistema de Contabilidad.</param>
        public void IniciarSdk(string nombreUsuarioComercial, string contrasenaComercial, string nombreUsuarioContabilidad,
            string contrasenaContabilidad);


        /// <summary>
        ///     Termina la conexión con el sistema y libera recursos.
        /// </summary>
        public void TerminarSdk();
        
    }
}
