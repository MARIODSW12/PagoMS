
namespace Pagos.Domain.ValueObjects
{
    public class VOFechaPago
    {
        public DateTime FechaPago { get; private set; }

        public VOFechaPago(DateTime fechaPago)
        {
            FechaPago = fechaPago;
        }

    }
}
