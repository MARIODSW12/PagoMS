
namespace MPago.Application.DTOs
{
    public class MPagoDto
    {
        public string idMPago { get; set; }
        public string idPostor { get; set; }
        public string idMPagoStripe { get; set; }
        public string idClienteStripe { get; set; }
        public string marca { get; set; }
        public int mesExpiracion { get; set; }
        public int anioExpiracion { get; set; }
        public string ultimos4 { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool predeterminado { get; set; }
    }
}
