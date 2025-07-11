using MongoDB.Bson;
using MongoDB.Driver;

using Pagos.Domain.Aggregate;
using Pagos.Domain.Exceptions;

using Pagos.Infrastructure.Configurations;
using Pagos.Infrastructure.Interfaces;

namespace Pagos.Infrastructure.Persistences.Repositories.MongoRead
{
    public class PagoReadRepository: IPagoReadRepository
    {
        private readonly IMongoCollection<BsonDocument> PagoColexion;

        public PagoReadRepository(MongoReadPagoDbConfig mongoConfig)
        {
            PagoColexion = mongoConfig.db.GetCollection<BsonDocument>("pagos_read");
        }

        async public Task AgregarPago(BsonDocument pago)
        {
            try
            {
                await PagoColexion.InsertOneAsync(pago);
            }
            catch (MongoConnectionException ex)
            {
                throw new ErrorConexionBd();
            }
            catch (Exception ex) {
                throw;
            }
        }

        async public Task<BsonDocument> GetPagoPorId(string idPago)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", idPago);
                var pago = await PagoColexion.Find(filter).FirstOrDefaultAsync();

                return pago;
            }
            catch (MongoConnectionException ex)
            {
                throw new ErrorConexionBd();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async public Task<BsonDocument> GetPagoPorIdSubasta(string idSubasta)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("idAuction", idSubasta);
                var pago = await PagoColexion.Find(filter).FirstOrDefaultAsync();
                return pago;
            }
            catch (MongoConnectionException ex)
            {
                throw new ErrorConexionBd();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async public Task<List<BsonDocument>> GetPagosPorIdUsuario(string idUsuario)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("idUsuario", idUsuario);
                var pagos = await PagoColexion.Find(filter).ToListAsync();
                return pagos;
            }
            catch (MongoConnectionException ex)
            {
                throw new ErrorConexionBd();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
