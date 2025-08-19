using WebAPI.Core;
using WebAPI.DTO;

namespace WebAPI.Adapters
{
    public static class ClienteAdapter
    {
        public static ClienteSdk ToEntity(this NuevoClienteRequest request)
        {
            var cliente = new ClienteSdk
            {
                Codigo = request.Codigo!,
                RazonSocial = request.RazonSocial!,
                Tipo = request.Tipo!,
                Rfc = request.RFC!,
            };
            return cliente;
        }

        public static ClienteResponse ToResponse(this ClienteSdk cliente)
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
