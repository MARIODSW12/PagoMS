using MongoDB.Bson;
using Pagos.Domain.Aggregate;

namespace Pagos.Infrastructure.Interfaces
{
    public interface IPagoReadRepository
    {
        Task<BsonDocument> GetPagoPorIdSubasta(string idSubasta);
        Task<BsonDocument> GetPagoPorId(string idPago);
        Task AgregarPago(BsonDocument pago);
        Task<List<BsonDocument>> GetPagosPorIdUsuario (string idUsuario);
    }
}
