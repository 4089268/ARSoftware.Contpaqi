using System;
using System.Text;
using ARSoftware.Contpaqi.Comercial.Sdk;
using ARSoftware.Contpaqi.Comercial.Sdk.Constantes;
using ARSoftware.Contpaqi.Comercial.Sdk.DatosAbstractos;
using ARSoftware.Contpaqi.Comercial.Sdk.Excepciones;
using ARSoftware.Contpaqi.Comercial.Sdk.Extensiones;
using CompaqWebAPI.Core.Interfaces;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.Comercial
{
    public class ConceptoServiceComercial : IConceptoService
    {

        public Concepto BuscarConceptoPorCodigo(string conceptoCodigo)
        {
            ComercialSdk.fBuscaConceptoDocto(conceptoCodigo).TirarSiEsError();

            return LeerDatosConcepto();
        }

        public Concepto BuscarConceptoPorId(int conceptoId)
        {
            ComercialSdk.fBuscaIdConceptoDocto(conceptoId).TirarSiEsError();

            return LeerDatosConcepto();
        }

        private Concepto LeerDatosConcepto()
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

            return new Concepto
            {
                Id = idConcepto,
                Codigo = codigoConcepto,
                Nombre = nombreConcepto,
                Serie = serieConcepto,
                RutaEntrega = rutaEntrega,
                PrefijoConcepto = prefijoConcepto,
                PlantillaFormatoDigital = plantillaDigit
            };
        }

        public List<Concepto> BuscarConceptos()
        {
            var conceptosList = new List<Concepto>();

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

        public void ActualizarConcepto(Concepto concepto)
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

            if (!string.IsNullOrEmpty(concepto.RutaEntrega))
            {
                ComercialSdk.fSetDatoConceptoDocto("CRUTAENTREGA", concepto.RutaEntrega).TirarSiEsError();
            }

            if (!string.IsNullOrEmpty(concepto.PrefijoConcepto))
            {
                ComercialSdk.fSetDatoConceptoDocto("CPREFIJOCONCEPTO", concepto.PrefijoConcepto).TirarSiEsError();
            }

            if (!string.IsNullOrEmpty(concepto.PlantillaFormatoDigital))
            {
                ComercialSdk.fSetDatoConceptoDocto("CPLAMIGCFD", concepto.PlantillaFormatoDigital).TirarSiEsError();
            }

            // Guardar los cambios realizados al registro
            ComercialSdk.fGuardaConceptoDocto().TirarSiEsError();
        }
    }
}
