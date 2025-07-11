using Pagos.Application.Interfaces;
using Stripe;

namespace Pagos.Infrastructure.Services
{
    public class StripeService : IPaymentService
    {

        public StripeService()
        {
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
        }

        async public Task<string?> RealizarPago(decimal monto, string customerId, string paymentMethodId)
        {
            Console.WriteLine($"Realizando pago de {monto} para el cliente {customerId} con el método de pago {paymentMethodId}");
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(monto * 100),
                Currency = "usd",
                Customer = customerId,
                PaymentMethod = paymentMethodId,
                Confirm = true,
                OffSession = true
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            if (intent.Status != "succeeded")
                return null;

            return intent.Id;
        }
    }
}
