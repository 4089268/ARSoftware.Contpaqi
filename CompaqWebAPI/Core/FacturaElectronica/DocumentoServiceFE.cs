using System;
using System.Text;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using CompaqWebAPI.Models;
using CompaqWebAPI.Core.Interfaces;


namespace CompaqWebAPI.Core.FacturaElectronica
{
    public class DocumentoServiceFE(IClienteService cService, IConceptoService ccService) : IDocumentoService
    {

        private readonly IClienteService clienteService = cService;
        private readonly IConceptoService conceptoService = ccService;

        /// <summary>
        ///     Actualiza los datos de un documento.
        /// </summary>
        /// <param name="documento">Documento con los datos a actualizar.</param>
        public void ActualizarDocumento(Documento documento)
        {
            // Buscar el documento
            // Si el documento existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscarIdDocumento(documento.Id).TirarSiEsError();

            // Activar el modo de edición
            FacturaElectronicaSdk.fEditarDocumento().TirarSiEsError();

            // Actualizar los campos del registro donde el SDK esta posicionado
            FacturaElectronicaSdk.fSetDatoDocumento("COBSERVACIONES", documento.Observaciones).TirarSiEsError();

            // Guardar los cambios realizados al registro
            FacturaElectronicaSdk.fGuardaDocumento().TirarSiEsError();
        }

        ///// <summary>
        /////     Obtiene los datos del CFDI.
        ///// </summary>
        ///// <param name="documentoId">El id del documento a obtener los datos.</param>
        ///// <returns>Los datos del CFDI.</returns>
        //public static DatosCfdi BuscarDatosCfdi(int documentoId)
        //{
        //    // Buscar el documento
        //    // Si el documento existe el SDK se posiciona en el registro
        //    FacturaElectronicaSdk.fBuscarIdDocumento(documentoId).TirarSiEsError();

        //    // Declarar variables a leer de la base de datos
        //    var serieCertificadoEmisor = new StringBuilder(3000);
        //    var folioFiscalUUid = new StringBuilder(3000);
        //    var serieCertificadoSat = new StringBuilder(3000);
        //    var fecha = new StringBuilder(3000);
        //    var selloDigital = new StringBuilder(3000);
        //    var selloSat = new StringBuilder(3000);
        //    var cadenaOriginalComplementoSat = new StringBuilder(3000);
        //    var regimen = new StringBuilder(3000);
        //    var lugarExpedicion = new StringBuilder(3000);
        //    var moneda = new StringBuilder(3000);
        //    var folioFiscalOriginal = new StringBuilder(3000);
        //    var serieFolioFiscal = new StringBuilder(3000);
        //    var fechaFolioFiscal = new StringBuilder(3000);
        //    var montoFolioFiscal = new StringBuilder(3000);

        //    // Buscar los datos CFDI
        //    FacturaElectronicaSdk.fGetDatosCFDI(serieCertificadoEmisor, folioFiscalUUid, serieCertificadoSat, fecha, selloDigital, selloSat,
        //            cadenaOriginalComplementoSat, regimen, lugarExpedicion, moneda, folioFiscalOriginal, serieFolioFiscal, fechaFolioFiscal,
        //            montoFolioFiscal)
        //        .TirarSiEsError();

        //    // Instanciar un objecto DatosCfdi y asignar los valores de la base de datos
        //    return new DatosCfdi
        //    {
        //        SerieCertificadoEmisor = serieCertificadoEmisor.ToString(),
        //        FolioFiscalUUid = folioFiscalUUid.ToString(),
        //        SerieCertificadoSat = serieCertificadoSat.ToString(),
        //        Fecha = fecha.ToString(),
        //        SelloDigital = selloDigital.ToString(),
        //        SelloSat = selloSat.ToString(),
        //        CadenaOriginalComplementoSat = cadenaOriginalComplementoSat.ToString(),
        //        Regimen = regimen.ToString(),
        //        LugarExpedicion = lugarExpedicion.ToString(),
        //        Moneda = moneda.ToString(),
        //        FolioFiscalOriginal = folioFiscalOriginal.ToString(),
        //        SerieFolioFiscal = serieFolioFiscal.ToString(),
        //        FechaFolioFiscal = fechaFolioFiscal.ToString(),
        //        MontoFolioFiscal = montoFolioFiscal.ToString()
        //    };
        //}

        /// <summary>
        ///     Busca un documento por id.
        /// </summary>
        /// <param name="documentoId">El id del documento a buscar.</param>
        /// <returns>El documento a buscar.</returns>
        public Documento BuscarDocumentoPorId(int documentoId)
        {
            // Buscar el documento por id
            // Si el documento existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscarIdDocumento(documentoId).TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            return LeerDatosDocumento();
        }

        /// <summary>
        ///     Busca un documento por llave.
        /// </summary>
        /// <param name="codigoConcepto">El código de concepto del documento a buscar.</param>
        /// <param name="serie">La serie del documento a buscar.</param>
        /// <param name="folio">El folio del documento a buscar.</param>
        /// <returns>El documento a buscar.</returns>
        public Documento BuscarDocumentoPorLlave(string codigoConcepto, string serie, string folio)
        {
            // Buscar el documento por llave
            // Si el documento existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscarDocumento(codigoConcepto, serie, folio).TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            return LeerDatosDocumento();
        }

        /// <summary>
        ///     Filtra y busca los documentos de un cliente.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio.</param>
        /// <param name="fechaFin">Fecha fin.</param>
        /// <param name="codigoConcepto">Código del concepto de documento.</param>
        /// <param name="codigoClienteProveedor">Código del cliente.</param>
        /// <returns>Lista de documentos con sus datos asignados.</returns>
        public List<Documento> BuscarDocumentosPorFiltro(DateTime fechaInicio, DateTime fechaFin, string codigoConcepto,
            string codigoClienteProveedor)
        {
            var documentosList = new List<Documento>();

            // Cancelar filtro
            FacturaElectronicaSdk.fCancelaFiltroDocumento().TirarSiEsError();

            // Filtrar documentos
            FacturaElectronicaSdk.fSetFiltroDocumento(fechaInicio.ToString(FormatosFechaSdk.Fecha), fechaFin.ToString(FormatosFechaSdk.Fecha),
                    codigoConcepto, codigoClienteProveedor)
                .TirarSiEsError();

            // Posicionar el SDK en el primer registro
            FacturaElectronicaSdk.fPosPrimerDocumento().TirarSiEsError();

            // Leer los datos del registro donde el SDK esta posicionado
            documentosList.Add(LeerDatosDocumento());

            // Crear un loop y posicionar el SDK en el siguiente registro
            while (FacturaElectronicaSdk.fPosSiguienteDocumento() == SdkConstantes.CodigoExito)
            {
                // Leer los datos del registro donde el SDK esta posicionado
                documentosList.Add(LeerDatosDocumento());

                // Checar si el SDK esta posicionado en el ultimo registro
                // Si el SDK esta posicionado en el ultimo registro salir del loop
                if (FacturaElectronicaSdk.fPosEOF() == 1) break;
            }

            // Cancelar filtro
            FacturaElectronicaSdk.fCancelaFiltroDocumento().TirarSiEsError();

            return documentosList;
        }

        /// <summary>
        ///     Busca el siguiente serie y folio del concepto.
        /// </summary>
        /// <param name="codigoConcepto">El código del concepto de documento.</param>
        /// <returns>La llave con el siguiente serie y folio.</returns>
        public tLlaveDoc BuscarSiguienteSerieYFolio(string codigoConcepto)
        {
            // Declarar variables a asignar por el SDK
            double folioBd = 0;
            var serieBd = new StringBuilder();

            // Buscar el siguiente folio
            FacturaElectronicaSdk.fSiguienteFolio(codigoConcepto, serieBd, ref folioBd).TirarSiEsError();

            // Instanciar una llave y asignar los datos asignados por el SDK
            return new tLlaveDoc { aCodConcepto = codigoConcepto, aSerie = serieBd.ToString(), aFolio = folioBd };
        }

        /// <summary>
        ///     Cancelar un documento.
        /// </summary>
        /// <param name="idDocumento">El id del documento a cancelar.</param>
        /// <param name="contrasenaCertificado">La contraseña del certificado.</param>
        /// <param name="motivoCancelacion">El código de motivo de cancelación.</param>
        /// <param name="uuidRemplazo">El UUID de reemplazo si se requiere.</param>
        public void CancelarDocumento(int idDocumento, string contrasenaCertificado, string motivoCancelacion, string uuidRemplazo)
        {
            // Buscar el documento
            FacturaElectronicaSdk.fBuscarIdDocumento(idDocumento).TirarSiEsError();

            // Ingresar la contraseña del certificado
            FacturaElectronicaSdk.fCancelaDoctoInfo(contrasenaCertificado).TirarSiEsError();

            // Cancelar el documento
            FacturaElectronicaSdk.fCancelaDocumentoConMotivo(motivoCancelacion, uuidRemplazo).TirarSiEsError();
        }

        /// <summary>
        ///     Crea un documento nuevo.
        /// </summary>
        /// <param name="documento">El documento a crear.</param>
        /// <returns>El id del documento creado.</returns>
        public int CrearDocumento(Documento documento)
        {
            Concepto concepto = this.conceptoService.BuscarConceptoPorId(documento.ConceptoId);
            Cliente cliente = this.clienteService.BuscarClientePorId(documento.ClienteId);

            // Instanciar un documento con la estructura tDocumento del SDK
            var nuevoDocumento = new tDocumento
            {
                aCodConcepto = concepto.Codigo,
                aSerie = documento.Serie,
                aFolio = documento.Folio,
                aFecha = documento.Fecha.ToString(FormatosFechaSdk.Fecha),
                aCodigoCteProv = cliente.Codigo,
                aNumMoneda = 1,
                aTipoCambio = 1,
                aReferencia = documento.Referencia
            };

            // Declarar una variable donde se asignara el id del documento nuevo
            var nuevoDocumentoId = 0;

            // Crear documento nuevo
            FacturaElectronicaSdk.fAltaDocumento(ref nuevoDocumentoId, ref nuevoDocumento).TirarSiEsError(); ;

            documento.Id = nuevoDocumentoId;

            // Editar los datos extra que no son parte de la estructura tDocumento
            ActualizarDocumento(documento);

            return nuevoDocumentoId;
        }

        /// <summary>
        ///     Crea un documento de cargo o abono.
        /// </summary>
        /// <param name="documento">El documento a crear.</param>
        /// <returns>El id del documento creado.</returns>
        public int CrearDocumentoCargoAbono(Documento documento)
        {
            Concepto concepto = this.conceptoService.BuscarConceptoPorId(documento.ConceptoId);
            Cliente cliente = this.clienteService.BuscarClientePorId(documento.ClienteId);

            // Instanciar un documento con la estructura tDocumento del SDK
            var nuevoDocumento = new tDocumento
            {
                aCodConcepto = concepto.Codigo,
                aSerie = documento.Serie,
                aFolio = documento.Folio,
                aFecha = documento.Fecha.ToString(FormatosFechaSdk.Fecha),
                aCodigoCteProv = cliente.Codigo,
                aNumMoneda = 1,
                aTipoCambio = 1,
                aReferencia = documento.Referencia,
                aImporte = documento.Total
            };

            // Crear documento de cargo o abono
            FacturaElectronicaSdk.fAltaDocumentoCargoAbono(ref nuevoDocumento).TirarSiEsError();

            // Buscar el id del documento.
            var idBd = new StringBuilder(3000);
            FacturaElectronicaSdk.fLeeDatoDocumento("CIDDOCUMENTO", idBd, 3000).TirarSiEsError();
            int nuevoDocumentoId = int.Parse(idBd.ToString());

            documento.Id = nuevoDocumentoId;

            // Actualizar los datos extra que no son parte de la estructura tDocumento
            ActualizarDocumento(documento);

            return nuevoDocumentoId;
        }

        /// <summary>
        ///     Elimina un documento.
        /// </summary>
        /// <param name="documento">El documento a eliminar.</param>
        public void EliminarDocumento(Documento documento)
        {
            // Buscar el documento
            // Si el documento existe el SDK se posiciona en el registro
            FacturaElectronicaSdk.fBuscarIdDocumento(documento.Id).TirarSiEsError();

            // Eliminar el documento
            FacturaElectronicaSdk.fBorraDocumento().TirarSiEsError();
        }

        /// <summary>
        ///     Genera el documento digital de un CFDI.
        /// </summary>
        /// <param name="codigoConceptoDocumento">El código de concepto del documento a generar.</param>
        /// <param name="serieDocumento">La serie del documento generar.</param>
        /// <param name="folioDocumento">El folio del documento a generar.</param>
        /// <param name="tipoArchivo">El tipo de archivo. 0 = XML, 1 = PDF</param>
        /// <param name="rutaPlantilla">Ruta de la plantilla cuando se genera el PDF.</param>
        public void GenerarDocumentoDigital(string codigoConceptoDocumento, string serieDocumento, double folioDocumento,
            int tipoArchivo, string rutaPlantilla)
        {
            // Generar el documento digital del CFDI
            FacturaElectronicaSdk.fEntregEnDiscoXML(codigoConceptoDocumento, serieDocumento, folioDocumento, tipoArchivo, rutaPlantilla)
                .TirarSiEsError();
        }

        /// <summary>
        ///     Lee los datos del documento donde el SDK esta posicionado.
        /// </summary>
        /// <returns>Un documento con los sus datos asignados.</returns>
        private Documento LeerDatosDocumento()
        {
            // Declarar variables a leer de la base de datos
            var idBd = new StringBuilder(3000);
            var conceptoIdBd = new StringBuilder(3000);
            var serieBd = new StringBuilder(3000);
            var folioBd = new StringBuilder(3000);
            var fechaBd = new StringBuilder(3000);
            var clienteIdBd = new StringBuilder(3000);
            var referenciaBd = new StringBuilder(3000);
            var observacionesBd = new StringBuilder(3000);
            var totalBd = new StringBuilder(3000);

            // Leer los datos del registro donde el SDK esta posicionado
            FacturaElectronicaSdk.fLeeDatoDocumento("CIDDOCUMENTO", idBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CIDCONCEPTODOCUMENTO", conceptoIdBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CSERIEDOCUMENTO", serieBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CFOLIO", folioBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CFECHA", fechaBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CIDCLIENTEPROVEEDOR", clienteIdBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CREFERENCIA", referenciaBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("COBSERVACIONES", observacionesBd, 3000).TirarSiEsError();
            FacturaElectronicaSdk.fLeeDatoDocumento("CTOTAL", totalBd, 3000).TirarSiEsError();

            // Instanciar un documento y asignar los datos de la base de datos
            return new Documento
            {
                Id = int.Parse(idBd.ToString()),
                ConceptoId = int.Parse(conceptoIdBd.ToString()),
                Serie = serieBd.ToString(),
                Folio = double.Parse(folioBd.ToString()),
                Fecha = DateTime.ParseExact(fechaBd.ToString(), FormatosFechaSdk.Comercial, null),
                ClienteId = int.Parse(clienteIdBd.ToString()),
                Referencia = referenciaBd.ToString(),
                Observaciones = observacionesBd.ToString(),
                Total = double.Parse(totalBd.ToString())
            };
        }

        /// <summary>
        ///     Salda un documento.
        /// </summary>
        /// <param name="documentoAPagar">Documento al que se le va a aplicar el pago.</param>
        /// <param name="documentoPago">El documento de pago.</param>
        /// <param name="importe">El importe que se va a aplicar.</param>
        /// <param name="fecha">La fecha en que se va a aplicar el pago.</param>
        public void SaldarDocumento(tLlaveDoc documentoAPagar, tLlaveDoc documentoPago, double importe, DateTime fecha)
        {
            FacturaElectronicaSdk.fSaldarDocumento(ref documentoAPagar, ref documentoPago, importe, 1, fecha.ToString(FormatosFechaSdk.Fecha))
                .TirarSiEsError();
        }

        /// <summary>
        ///     Timbra un documento.
        /// </summary>
        /// <param name="codigoConceptoDocumento">El código de concepto del documento a timbrar.</param>
        /// <param name="serieDocumento">La serie del documento a timbrar.</param>
        /// <param name="folioDocumento">El folio del documento a timbrar.</param>
        /// <param name="contrasenaCertificado">La contraseña del certificado.</param>
        /// <param name="rutaArchivoAdicional">Un archivo adicional como un complemento.</param>
        public void TimbrarDocumento(string codigoConceptoDocumento, string serieDocumento, double folioDocumento,
            string contrasenaCertificado, string rutaArchivoAdicional)
        {
            // Timbrar el documento
            FacturaElectronicaSdk.fEmitirDocumento(codigoConceptoDocumento, serieDocumento, folioDocumento, contrasenaCertificado, rutaArchivoAdicional)
                .TirarSiEsError();
        }
    }
}
