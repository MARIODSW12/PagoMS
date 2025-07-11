using MediatR;
using Pagos.Application.DTOs;

namespace Pagos.Infrastructure.Queries
{
    public class GetPagosUsuarioQuery : IRequest<List<PagoDto>>
    {
        public string IdUsuario { get; set; }
        public GetPagosUsuarioQuery(string idUsuario)
        {
            IdUsuario = idUsuario;
        }
    }
}
