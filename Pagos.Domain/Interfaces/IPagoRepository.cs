using Pagos.Domain.Aggregate;

namespace Pagos.Domain.Interfaces
{
    public interface IPagoRepository
    {
        Task AgregarPago (Pago pago);
        Task EliminarPago(string idPago);
        Task<Pago?> ObtenerPagoPorId(string idPago);
        Task ActualizarIdPagoExterno(string idPago, string idExternalPago);
    }
}
