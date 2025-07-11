
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.ValueObjects
{
    public class VOMonto
    {
        public decimal Monto { get; private set; }

        public VOMonto(decimal monto)
        {
            if (monto <= 0)
                throw new MontoInvalido();

            Monto = monto;
        }

    }
}
