
using Pagos.Domain.ValueObjects;

namespace Pagos.Application.DTOs
{
    public class PagoDto
    {
        public string IdPago { get; set; }
        public string IdMPago { get; set; }
        public string? IdExternalPago { get; set; }
        public string IdUsuario { get; set; }
        public string IdAuction { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
    }
}
