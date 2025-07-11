using MediatR;
using Pagos.Application.DTOs;

namespace Pagos.Application.Commands
{
    public class AgregarPagoCommand : IRequest<String>
    {
        public AgregarPagoDto Pago { get; set; }
        public string CustomerId { get; set; }
        public string PaymentMethodId { get; set; }
        public AgregarPagoCommand(AgregarPagoDto pago, string customerId, string paymentMethodId)
        {
            Pago = pago;
            CustomerId = customerId;
            PaymentMethodId = paymentMethodId;
        }
    }
}
