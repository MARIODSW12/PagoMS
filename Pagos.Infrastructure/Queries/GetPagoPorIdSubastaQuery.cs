using MediatR;
using Pagos.Application.DTOs;

namespace Pagos.Infrastructure.Queries
{
    public class GetPagoPorIdSubastaQuery : IRequest<PagoDto?>
    {
        public string IdSubasta { get; set; }
        public GetPagoPorIdSubastaQuery(string idSubasta)
        {
            IdSubasta = idSubasta;
        }
    }
}
