using MassTransit;
using MediatR;

namespace Pagos.Domain.Events.EventHandler
{
    public class AgregarPagoEventHandler : INotificationHandler<AgregarPagoEvent>
    {
        private readonly ISendEndpointProvider PublishEndpoint;

        public AgregarPagoEventHandler(ISendEndpointProvider publishEndpoint)
        {
            PublishEndpoint = publishEndpoint;
        }

        public async Task Handle(AgregarPagoEvent PagoAgregadoEvento, CancellationToken cancellationToken)
        {
            try
            {
                await PublishEndpoint.Send(PagoAgregadoEvento, cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
