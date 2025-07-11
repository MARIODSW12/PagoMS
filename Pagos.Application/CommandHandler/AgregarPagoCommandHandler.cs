using MediatR;
using Pagos.Application.Commands;
using Pagos.Application.Interfaces;
using Pagos.Domain.Aggregate;
using Pagos.Domain.Events;
using Pagos.Domain.Interfaces;
using Pagos.Domain.ValueObjects;
using System.Net;
using System.Xml.Linq;

namespace Pagos.Application.CommandHandler
{
    public class AgregarPagoCommandHandler : IRequestHandler<AgregarPagoCommand, string>
    {
        private readonly IPagoRepository PagoWriteRepository;
        private readonly IMediator Mediator;
        private readonly IPaymentService PaymentService;

        public AgregarPagoCommandHandler(IPagoRepository pagoWriteRepository, IMediator mediator, IPaymentService paymentService)
        {
            PagoWriteRepository = pagoWriteRepository ?? throw new ArgumentNullException(nameof(pagoWriteRepository));
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            PaymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        }

        public async Task<string> Handle(AgregarPagoCommand Pago, CancellationToken cancellationToken)
        {
            try
            {

                var pago = new Pago(
                    new VOIdPago(Guid.NewGuid().ToString()),
                    new VOIdMPago(Pago.Pago.IdMPago),
                    new VOIdUsuario(Pago.Pago.IdUsuario),
                    new VOFechaPago(Pago.Pago.FechaPago),
                    new VOMonto(Pago.Pago.Monto),
                    new VOIdAuction(Pago.Pago.IdAuction),
                    null
                );

                await PagoWriteRepository.AgregarPago(pago);

                var paymentResult = await PaymentService.RealizarPago(
                    Pago.Pago.Monto,
                    Pago.CustomerId,
                    Pago.PaymentMethodId
                );
                if (paymentResult == null)
                {
                    await PagoWriteRepository.EliminarPago(pago.IdPago.IdPago);
                    throw new Exception("Error al procesar el pago");
                }
                await PagoWriteRepository.ActualizarIdPagoExterno(pago.IdPago.IdPago, paymentResult);
                var PagoAgregadoEvento = new AgregarPagoEvent(
                    pago.IdPago.IdPago,
                    pago.IdMPago.IdMPago,
                    pago.IdUsuario.IdUsuario,
                    pago.Monto.Monto,
                    pago.FechaPago.FechaPago,
                    pago.IdAuction.IdAuction,
                    paymentResult
                );
                await Mediator.Publish(PagoAgregadoEvento);

                return pago.IdPago.IdPago;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
