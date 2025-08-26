using System;
using System.Text;
using Microsoft.Win32;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.Excepciones;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using CompaqWebAPI.Core.Interfaces;

namespace CompaqWebAPI.Core.FacturaElectronica
{
    public class ConexionSdkFE : IConexionSDK
    {
        /// <summary>
        ///     Abre la empresa de trabajo.
        /// </summary>
        /// <param name="rutaEmpresa">Ruta del directorio de la empresa.</param>
        public void AbrirEmpresa(string rutaEmpresa)
        {
            FacturaElectronicaSdk.fAbreEmpresa(rutaEmpresa).TirarSiEsError();
        }

        /// <summary>
        ///     Busca el directorio donde se encuentra el SDK en el registro de Windows.
        /// </summary>
        /// <param name="nombreLlaveRegistro">La llave del registro de Windows.</param>
        /// <returns>La ruta del directorio donde se encuentra el SDK.</returns>
        private string BuscarDirectorioDelSdk(string nombreLlaveRegistro)
        {
            // Buscar registro de windows
            RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

            // Buscar la llave de CONTPAQi
            RegistryKey? keySitema = registryKey.OpenSubKey(nombreLlaveRegistro, false);

            if (keySitema == null)
            {
                throw new Exception(string.Format("No se encontró la llave del registro {0}, nombreLlaveRegistro"));
            }
            // No se encontró la llave

            // Leer el valor del campo DIRECTORIOBASE donde se encuentra la ruta del SDK
            var directorioBaseKey = keySitema.GetValue(LlavesRegistroWindowsSdk.NombreCampoRutaSdk);

            if (directorioBaseKey == null)
            {
                throw new Exception(
                    string.Format("No se encontró el valor del campo {0} del registro {1}", LlavesRegistroWindowsSdk.NombreCampoRutaSdk, nombreLlaveRegistro));
            }

            return directorioBaseKey.ToString() ?? string.Empty;
        }

        /// <summary>
        ///     Cierra la empresa de trabajo.
        /// </summary>
        public void CerrarEmpresa()
        {
            FacturaElectronicaSdk.fCierraEmpresa();
        }

        /// <summary>
        ///     Establece el directorio de trabajo en el directorio donde se encuentra el SDK.
        /// </summary>
        private void EstablecerElDirectorioDeTrabajo()
        {
            // Buscar el directorio donde se encuentra el SDK
            string rutaSdk = BuscarDirectorioDelSdk(LlavesRegistroWindowsSdk.FacturaElectronica);
            Console.WriteLine(">> Ruta SDK Factura: " + rutaSdk);

            // Establecer el directorio de trabajo en el directorio donde se encuentra el SDK
            Directory.SetCurrentDirectory(rutaSdk);
        }

        /// <summary>
        ///     Inicia la conexión con el sistema y muestra una ventana de autenticación donde el usuario podrá ingresar su nombre
        ///     de usuario y contraseña.
        /// </summary>
        public void IniciarSdk()
        {
            // Establecer el directorio de trabajo en el directorio donde se encuentra el SDK
            EstablecerElDirectorioDeTrabajo();

            // Iniciar conexión
            FacturaElectronicaSdk.fSetNombrePAQ(NombresPaqSdk.FacturaElectronica).TirarSiEsError();
        }

        /// <summary>
        ///     Inicia la conexión con el sistema e ingresa el usuario y contraseña programáticamente para que no se muestre la
        ///     ventana de autenticación de Comercial.
        /// </summary>
        /// <param name="nombreUsuario">Nombre de usuario del sistema de Comercial.</param>
        /// <param name="contrasena">Contraseña del sistema de Comercial.</param>
        public void IniciarSdk(string nombreUsuario, string contrasena)
        {
            // Establecer el directorio de trabajo en el directorio donde se encuentra el SDK
            EstablecerElDirectorioDeTrabajo();

            // Ingresar programáticamente el usuario y contraseña del sistema de Comercial
            // FacturaElectronicaSdk.fInicioSesionSDK(nombreUsuario, contrasena);

            // Iniciar conexión
            FacturaElectronicaSdk.fSetNombrePAQ(NombresPaqSdk.FacturaElectronica);
        }

        /// <summary>
        ///     Inicia la conexión con el sistema e ingresa el usuario y contraseña programáticamente para que no se muestre la
        ///     ventana de autenticación de Comercial y Contabilidad.
        /// </summary>
        /// <param name="nombreUsuarioComercial">Nombre de usuario del sistema de Comercial.</param>
        /// <param name="contrasenaComercial">Contraseña del sistema de Comercial.</param>
        /// <param name="nombreUsuarioContabilidad">Nombre de usuario del sistema de Contabilidad.</param>
        /// <param name="contrasenaContabilidad">Contraseña del sistema de Contabilidad.</param>
        public void IniciarSdk(string nombreUsuarioComercial, string contrasenaComercial, string nombreUsuarioContabilidad,
            string contrasenaContabilidad)
        {
            // Iniciar conexión con el sistema
            IniciarSdk(nombreUsuarioComercial, contrasenaComercial);

            // Ingresar programáticamente el usuario y contraseña del sistema de Contabilidad
            FacturaElectronicaSdk.fInicioSesionSDKCONTPAQi(nombreUsuarioContabilidad, contrasenaContabilidad);
        }

        /// <summary>
        ///     Termina la conexión con el sistema y libera recursos.
        /// </summary>
        public void TerminarSdk()
        {
            FacturaElectronicaSdk.fTerminaSDK();
        }
    }
}
