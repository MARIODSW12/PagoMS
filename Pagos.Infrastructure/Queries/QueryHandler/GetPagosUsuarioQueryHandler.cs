using MediatR;

using Pagos.Application.DTOs;

using Pagos.Infrastructure.Interfaces;

namespace Pagos.Infrastructure.Queries.QueryHandler
{
    public class GetPagosUsuarioQueryHandler : IRequestHandler<GetPagosUsuarioQuery, List<PagoDto>>
    {
        private readonly IPagoReadRepository PagoRepository;

        public GetPagosUsuarioQueryHandler(IPagoReadRepository pagoRepository)
        {
            PagoRepository = pagoRepository;
        }

        public async Task<List<PagoDto>> Handle(GetPagosUsuarioQuery pagoIdUsuario, CancellationToken cancellationToken)
        {
            try
            {
                var pagos = await PagoRepository.GetPagosPorIdUsuario(pagoIdUsuario.IdUsuario);

                var returnPagos = new List<PagoDto>();
                foreach (var pago in pagos)
                {
                    var returnPago = new PagoDto
                    {
                        IdPago = pago["_id"].AsString,
                        IdMPago = pago["idMPago"].AsString,
                        IdUsuario = pago["idUsuario"].AsString,
                        IdAuction = pago["idAuction"].AsString,
                        Monto = pago["monto"].AsDecimal,
                        FechaPago = pago["fechaPago"].ToUniversalTime(),
                        IdExternalPago = pago.Contains("idExternalPago") ? pago["idExternalPago"].AsString : null
                    };
                    returnPagos.Add(returnPago);
                }

                return returnPagos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
