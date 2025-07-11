
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.ValueObjects
{
    public class VOIdMPago
    {
        public string IdMPago { get; private set; }
        public VOIdMPago(string idMPago)
        {
            if (string.IsNullOrWhiteSpace(idMPago))
                throw new IdMPagoInvalido();

            if (!Guid.TryParse(idMPago, out _))
                throw new IdMPagoInvalido();

            IdMPago = idMPago;
        }
    }
}
