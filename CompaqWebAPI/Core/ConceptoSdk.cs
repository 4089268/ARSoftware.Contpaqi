using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.Excepciones;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using System;
using System.Text;

namespace WebAPI.Core
{
    public class ConceptoSdk
    {
        /// <summary>
        ///     Campo CCODIGOCONCEPTO - Código del concepto.
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        ///     Campo CIDCONCEPTODOCUMENTO - Identificador del concepto de documento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Campo CNOMBRECONCEPTO - Nombre del concepto.
        /// </summary>
        public string Nombre { get; set; } = default!;

        /// <summary>
        ///     Serie para clasificar las facturas
        /// </summary>
        public string? Serie { get; set; }

        /// <summary>
        ///     Ruta donde se almacenaran las facturas (pdf, xml) generadas para este concepto.
        /// </summary>
        public string? RutaEntrega { get; set; }

        /// <summary>
        ///     Prefijo para el nombre del archivo de entrega.
        /// </summary>
        public string? PrefijoConcepto { get; set; }

        /// <summary>
        ///    Nombre de plantilla utilizada para generar los documentos digitales.
        /// </summary>
        public string? PlantillaFormatoDigital { get; set; }


        public static ConceptoSdk BuscarConceptoPorCodigo(string conceptoCodigo)
        {
            ComercialSdk.fBuscaConceptoDocto(conceptoCodigo).TirarSiEsError();

            return LeerDatosConcepto();
        }

        public static ConceptoSdk BuscarConceptoPorId(int conceptoId)
        {
            ComercialSdk.fBuscaIdConceptoDocto(conceptoId).TirarSiEsError();

            return LeerDatosConcepto();
        }

        private static ConceptoSdk LeerDatosConcepto()
        {
            var stringBuffer = new StringBuilder(3000);

            ComercialSdk.fLeeDatoConceptoDocto("CIDCONCEPTODOCUMENTO", stringBuffer, 3000);
            var idConcepto = int.Parse(stringBuffer.ToString());
            stringBuffer.Clear();

            ComercialSdk.fLeeDatoConceptoDocto("CCODIGOCONCEPTO", stringBuffer, 3000);
            var codigoConcepto = stringBuffer.ToString();
            stringBuffer.Clear();

            ComercialSdk.fLeeDatoConceptoDocto("CNOMBRECONCEPTO", stringBuffer, 3000);
            var nombreConcepto = stringBuffer.ToString();
            stringBuffer.Clear();

            ComercialSdk.fLeeDatoConceptoDocto("CSERIEPOROMISION", stringBuffer, 3000);
            var serieConcepto = stringBuffer.ToString();
            stringBuffer.Clear();

            ComercialSdk.fLeeDatoConceptoDocto("CRUTAENTREGA", stringBuffer, 3000);
            var rutaEntrega = stringBuffer.ToString();
            stringBuffer.Clear();

            ComercialSdk.fLeeDatoConceptoDocto("CPREFIJOCONCEPTO", stringBuffer, 3000);
            var prefijoConcepto = stringBuffer.ToString();
            stringBuffer.Clear();

            ComercialSdk.fLeeDatoConceptoDocto("CPLAMIGCFD", stringBuffer, 3000);
            var plantillaDigit = stringBuffer.ToString();
            stringBuffer.Clear();

            return new ConceptoSdk {
                Id = idConcepto,
                Codigo = codigoConcepto,
                Nombre = nombreConcepto,
                Serie = serieConcepto,
                RutaEntrega = rutaEntrega,
                PrefijoConcepto = prefijoConcepto,
                PlantillaFormatoDigital = plantillaDigit
            };
        }

        public static List<ConceptoSdk> BuscarConceptos()
        {
            var conceptosList = new List<ConceptoSdk>();

            // Posicionar el SDK en el primer registro
            int resultado = ComercialSdk.fPosPrimerConceptoDocto();
            if (resultado == SdkConstantes.CodigoExito)
                // Leer los datos del registro donde el SDK esta posicionado
                conceptosList.Add(LeerDatosConcepto());
            else
                return conceptosList;

            // Crear un loop y posicionar el SDK en el siguiente registro
            while (ComercialSdk.fPosSiguienteConceptoDocto() == SdkConstantes.CodigoExito)
            {
                // Leer los datos del registro donde el SDK esta posicionado
                conceptosList.Add(LeerDatosConcepto());

                // Checar si el SDK esta posicionado en el ultimo registro
                // Si el SDK esta posicionado en el ultimo registro salir del loop
                if (ComercialSdk.fPosEOFConceptoDocto() == 1) break;
            }

            return conceptosList;
        }

        public static void ActualizarConcepto(ConceptoSdk concepto)
        {
            // Buscar el cliente por código
            // Si el cliente existe el SDK se posiciona en el registro
            ComercialSdk.fBuscaConceptoDocto(concepto.Codigo).TirarSiEsError();

            // Activar el modo de edición
            ComercialSdk.fEditaConceptoDocto().TirarSiEsError();

            // Actualizar los campos del registro donde el SDK esta posicionado
            ComercialSdk.fSetDatoConceptoDocto("CNOMBRECONCEPTO", concepto.Nombre).TirarSiEsError();
            if (!string.IsNullOrEmpty(concepto.Serie))
            {
                ComercialSdk.fSetDatoConceptoDocto("CSERIEPOROMISION", concepto.Serie).TirarSiEsError();
            }

            if(!string.IsNullOrEmpty(concepto.RutaEntrega))
            {
                ComercialSdk.fSetDatoConceptoDocto("CRUTAENTREGA", concepto.RutaEntrega).TirarSiEsError();
            }

            if(!string.IsNullOrEmpty(concepto.PrefijoConcepto))
            {
                ComercialSdk.fSetDatoConceptoDocto("CPREFIJOCONCEPTO", concepto.PrefijoConcepto).TirarSiEsError();
            }

            if(!string.IsNullOrEmpty(concepto.PlantillaFormatoDigital))
            {
                ComercialSdk.fSetDatoConceptoDocto("CPLAMIGCFD", concepto.PlantillaFormatoDigital).TirarSiEsError();
            }

            // Guardar los cambios realizados al registro
            ComercialSdk.fGuardaConceptoDocto().TirarSiEsError();
        }
    }
}
