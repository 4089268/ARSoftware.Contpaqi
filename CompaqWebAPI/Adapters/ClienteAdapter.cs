using CompaqWebAPI.Models;
using WebAPI.DTO;

namespace WebAPI.Adapters
{
    public static class ClienteAdapter
    {
        public static Cliente ToEntity(this NuevoClienteRequest request)
        {
            var cliente = new Cliente
            {
                Codigo = request.Codigo!,
                RazonSocial = request.RazonSocial!,
                Tipo = request.Tipo!,
                Rfc = request.RFC!,
            };
            return cliente;
        }

        public static ClienteResponse ToResponse(this Cliente cliente)
        {
            var clienteResp = new ClienteResponse
            {
                Id = cliente.Id,
                Codigo = cliente.Codigo,
                RazonSocial = cliente.RazonSocial,
                Rfc = cliente.Rfc,
                Tipo = cliente.Tipo,
                TipoDesc = string.Empty
            };
            return clienteResp;
        }
    }
}
