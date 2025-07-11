
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.ValueObjects
{
    public class VOIdExternalPago
    {
        public string IdExternalPago { get; private set; }
        public VOIdExternalPago(string idExternalPago)
        {
            IdExternalPago = idExternalPago;
        }
    }
}
