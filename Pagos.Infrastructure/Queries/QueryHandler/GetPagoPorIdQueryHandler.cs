using MediatR;

using Pagos.Application.DTOs;

using Pagos.Infrastructure.Interfaces;

namespace Pagos.Infrastructure.Queries.QueryHandler
{
    public class GetPagoPorIdQueryHandler : IRequestHandler<GetPagoPorIdQuery, PagoDto?>
    {
        private readonly IPagoReadRepository PagoRepository;

        public GetPagoPorIdQueryHandler(IPagoReadRepository pagoRepository)
        {
            PagoRepository = pagoRepository;
        }

        public async Task<PagoDto?> Handle(GetPagoPorIdQuery idPago, CancellationToken cancellationToken)
        {
            try
            {
                var pago = await PagoRepository.GetPagoPorId(idPago.IdPago);

                if (pago == null)
                {
                    return null;
                }

                var pagoPorId = new PagoDto
                {
                    IdPago = pago["_id"].AsString,
                    IdMPago = pago["idMPago"].AsString,
                    IdUsuario = pago["idUsuario"].AsString,
                    IdAuction = pago["idAuction"].AsString,
                    Monto = pago["monto"].AsDecimal,
                    FechaPago = pago["fechaPago"].ToUniversalTime(),
                    IdExternalPago = pago.Contains("idExternalPago") ? pago["idExternalPago"].AsString : null
                };

                return pagoPorId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
