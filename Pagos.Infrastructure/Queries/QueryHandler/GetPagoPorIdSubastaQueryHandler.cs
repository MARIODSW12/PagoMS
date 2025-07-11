using MediatR;

using Pagos.Application.DTOs;

using Pagos.Infrastructure.Interfaces;

namespace Pagos.Infrastructure.Queries.QueryHandler
{
    public class GetPagoPorIdSubastaQueryHandler : IRequestHandler<GetPagoPorIdSubastaQuery, PagoDto?>
    {
        private readonly IPagoReadRepository PagoRepository;

        public GetPagoPorIdSubastaQueryHandler(IPagoReadRepository pagoRepository)
        {
            PagoRepository = pagoRepository;
        }

        public async Task<PagoDto?> Handle(GetPagoPorIdSubastaQuery pagoIdSubasta, CancellationToken cancellationToken)
        {
            try
            {
                var pago = await PagoRepository.GetPagoPorIdSubasta(pagoIdSubasta.IdSubasta);

                if (pago == null)
                {
                    return null;
                }

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

                return returnPago;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
