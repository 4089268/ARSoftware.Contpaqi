using System;
using CompaqWebAPI.Models;

namespace CompaqWebAPI.Core.Interfaces
{
    public interface IConceptoService
    {
        public Concepto BuscarConceptoPorCodigo(string conceptoCodigo);

        public Concepto BuscarConceptoPorId(int conceptoId);

        public List<Concepto> BuscarConceptos();

        public void ActualizarConcepto(Concepto concepto);
    }
}
