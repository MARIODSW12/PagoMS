using MediatR;

namespace Pagos.Domain.Events
{
    public class AgregarPagoEvent : INotification
    {
        public string IdPago { get; set; }
        public string IdMPago { get; set; }
        public string IdExternalPago { get; set; }
        public string IdUsuario { get; set; }
        public string IdAuction { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }

        public AgregarPagoEvent(string idPago, string idMPago, string idUsuario, decimal monto, DateTime fechaPago, string idAuction, string idExternalPago)
        {
            IdPago = idPago;
            IdMPago = idMPago;
            IdUsuario = idUsuario;
            Monto = monto;
            FechaPago = fechaPago;
            IdAuction = idAuction;
            IdExternalPago = idExternalPago;
        }
    }
}
