using Pagos.Domain.ValueObjects;
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.Aggregate
{
    public class Pago
    {
        public VOIdPago IdPago { get; private set;}
        public VOIdMPago IdMPago { get; private set;}
        public VOIdExternalPago? IdExternalPago { get; private set; }
        public VOIdUsuario IdUsuario { get; private set;}
        public VOIdAuction IdAuction { get; private set;}
        public VOFechaPago FechaPago { get; private set;}
        public VOMonto Monto { get; private set;}
        
        public Pago (VOIdPago idPago, VOIdMPago idMPago, VOIdUsuario idUsuario, VOFechaPago fechaPago, VOMonto monto, VOIdAuction idAuction, VOIdExternalPago? idExternalPago)
        {
            IdPago = idPago ?? throw new ArgumentNullException(nameof(idPago));
            IdMPago = idMPago ?? throw new ArgumentNullException(nameof(idMPago));
            IdUsuario = idUsuario ?? throw new ArgumentNullException(nameof(idUsuario));
            FechaPago = fechaPago ?? throw new ArgumentNullException(nameof(fechaPago));
            Monto = monto ?? throw new ArgumentNullException(nameof(monto));
            IdAuction = idAuction ?? throw new ArgumentNullException(nameof(idAuction));
            IdExternalPago = idExternalPago;
        }

    }
}
