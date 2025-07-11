using MassTransit;
using MongoDB.Bson;
using Pagos.Domain.Events;
using Pagos.Infrastructure.Interfaces;

namespace Pagos.Infrastructure.Consumers
{
    public class AgregarPagoConsumer(
        IServiceProvider serviceProvider,
        IPagoReadRepository pagoReadRepository) : IConsumer<AgregarPagoEvent>
    {
        private readonly IServiceProvider ServiceProvider = serviceProvider;
        private readonly IPagoReadRepository PagoReadRepository = pagoReadRepository;

        public async Task Consume(ConsumeContext<AgregarPagoEvent> context)
        {
            try
            {
                var pago = new BsonDocument
                {
                    { "_id", context.Message.IdPago },
                    { "idUsuario", context.Message.IdUsuario },
                    { "idMPago", context.Message.IdMPago },
                    { "idAuction", context.Message.IdAuction },
                    { "monto", context.Message.Monto },
                    { "fechaPago", context.Message.FechaPago },
                    { "idExternalPago", context.Message.IdExternalPago}
                };
                await PagoReadRepository.AgregarPago(pago);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
