using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<string?> RealizarPago(decimal monto, string customerId, string paymentMethodId);
    }
}
