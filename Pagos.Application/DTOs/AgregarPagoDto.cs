
namespace Pagos.Application.DTOs
{
    public class AgregarPagoDto
    {
        public string IdMPago { get; set; }
        public string IdUsuario { get; set; }
        public string IdAuction { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
    }
}
