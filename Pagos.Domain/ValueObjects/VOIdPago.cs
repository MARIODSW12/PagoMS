
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.ValueObjects
{
    public class VOIdPago
    {
        public string IdPago { get; private set; }
        public VOIdPago (string idPago)
        {
            if (string.IsNullOrWhiteSpace(idPago))
                throw new IdPagoInvalido();

            if (!Guid.TryParse(idPago, out _))
                throw new IdPagoInvalido();

            IdPago = idPago;
        }
    }
}
