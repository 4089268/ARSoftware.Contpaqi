using System.Text;
using CompaqWebAPI.Models;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;

namespace CompaqWebAPI.Core.Interfaces
{
    public interface IDocumentoService
    {
        public void ActualizarDocumento(Documento documento);

        /// <summary>
        ///     Busca un documento por id.
        /// </summary>
        /// <param name="documentoId">El id del documento a buscar.</param>
        /// <returns>El documento a buscar.</returns>
        public Documento BuscarDocumentoPorId(int documentoId);

        /// <summary>
        ///     Busca un documento por llave.
        /// </summary>
        /// <param name="codigoConcepto">El código de concepto del documento a buscar.</param>
        /// <param name="serie">La serie del documento a buscar.</param>
        /// <param name="folio">El folio del documento a buscar.</param>
        /// <returns>El documento a buscar.</returns>
        public Documento BuscarDocumentoPorLlave(string codigoConcepto, string serie, string folio);

        /// <summary>
        ///     Filtra y busca los documentos de un cliente.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio.</param>
        /// <param name="fechaFin">Fecha fin.</param>
        /// <param name="codigoConcepto">Código del concepto de documento.</param>
        /// <param name="codigoClienteProveedor">Código del cliente.</param>
        /// <returns>Lista de documentos con sus datos asignados.</returns>
        public List<Documento> BuscarDocumentosPorFiltro(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto,
            string codigoClienteProveedor);

        /// <summary>
        ///     Busca el siguiente serie y folio del concepto.
        /// </summary>
        /// <param name="codigoConcepto">El código del concepto de documento.</param>
        /// <returns>La llave con el siguiente serie y folio.</returns>
        public tLlaveDoc BuscarSiguienteSerieYFolio(string codigoConcepto);

        /// <summary>
        ///     Cancelar un documento.
        /// </summary>
        /// <param name="idDocumento">El id del documento a cancelar.</param>
        /// <param name="contrasenaCertificado">La contraseña del certificado.</param>
        /// <param name="motivoCancelacion">El código de motivo de cancelación.</param>
        /// <param name="uuidRemplazo">El UUID de reemplazo si se requiere.</param>
        public void CancelarDocumento(int idDocumento, string contrasenaCertificado, string motivoCancelacion, string uuidRemplazo);

        /// <summary>
        ///     Crea un documento nuevo.
        /// </summary>
        /// <param name="documento">El documento a crear.</param>
        /// <returns>El id del documento creado.</returns>
        public int CrearDocumento(Documento documento);

        /// <summary>
        ///     Crea un documento de cargo o abono.
        /// </summary>
        /// <param name="documento">El documento a crear.</param>
        /// <returns>El id del documento creado.</returns>
        public int CrearDocumentoCargoAbono(Documento documento);

        /// <summary>
        ///     Elimina un documento.
        /// </summary>
        /// <param name="documento">El documento a eliminar.</param>
        public void EliminarDocumento(Documento documento);

        /// <summary>
        ///     Genera el documento digital de un CFDI.
        /// </summary>
        /// <param name="codigoConceptoDocumento">El código de concepto del documento a generar.</param>
        /// <param name="serieDocumento">La serie del documento generar.</param>
        /// <param name="folioDocumento">El folio del documento a generar.</param>
        /// <param name="tipoArchivo">El tipo de archivo. 0 = XML, 1 = PDF</param>
        /// <param name="rutaPlantilla">Ruta de la plantilla cuando se genera el PDF.</param>
        public void GenerarDocumentoDigital(string codigoConceptoDocumento, string serieDocumento, double folioDocumento,
            int tipoArchivo, string rutaPlantilla);

        /// <summary>
        ///     Salda un documento.
        /// </summary>
        /// <param name="documentoAPagar">Documento al que se le va a aplicar el pago.</param>
        /// <param name="documentoPago">El documento de pago.</param>
        /// <param name="importe">El importe que se va a aplicar.</param>
        /// <param name="fecha">La fecha en que se va a aplicar el pago.</param>
        public void SaldarDocumento(tLlaveDoc documentoAPagar, tLlaveDoc documentoPago, double importe, DateTime fecha);

        /// <summary>
        ///     Timbra un documento.
        /// </summary>
        /// <param name="codigoConceptoDocumento">El código de concepto del documento a timbrar.</param>
        /// <param name="serieDocumento">La serie del documento a timbrar.</param>
        /// <param name="folioDocumento">El folio del documento a timbrar.</param>
        /// <param name="contrasenaCertificado">La contraseña del certificado.</param>
        /// <param name="rutaArchivoAdicional">Un archivo adicional como un complemento.</param>
        public void TimbrarDocumento(string codigoConceptoDocumento, string serieDocumento, double folioDocumento,
            string contrasenaCertificado, string rutaArchivoAdicional);
    }
}
