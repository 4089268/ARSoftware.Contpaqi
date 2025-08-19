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
        public string Nombre { get; set; }

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
            var idBd = new StringBuilder(3000);
            var codigoBd = new StringBuilder(3000);
            var nombreBd = new StringBuilder(3000);

            ComercialSdk.fLeeDatoConceptoDocto("CIDCONCEPTODOCUMENTO", idBd, 3000);
            ComercialSdk.fLeeDatoConceptoDocto("CCODIGOCONCEPTO", codigoBd, 3000);
            ComercialSdk.fLeeDatoConceptoDocto("CNOMBRECONCEPTO", nombreBd, 3000);

            return new ConceptoSdk { Id = int.Parse(idBd.ToString()), Codigo = codigoBd.ToString(), Nombre = nombreBd.ToString() };
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
    }
}
