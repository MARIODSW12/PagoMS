
using Pagos.Domain.ValueObjects;

namespace Pagos.Application.DTOs
{
    public class ObtenerPagoSubastadorDto
    {
        public string IdPago { get; set; }
        public string MPago { get; set; }
        public string Usuario { get; set; }
        public string Comprador { get; set; }
        public string Subasta { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
    }
}
