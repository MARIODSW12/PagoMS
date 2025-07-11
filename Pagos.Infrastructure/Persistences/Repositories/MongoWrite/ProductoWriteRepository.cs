using MongoDB.Bson;
using MongoDB.Driver;

using Pagos.Domain.Aggregate;
using Pagos.Domain.Interfaces;
using Pagos.Domain.Exceptions;
using Pagos.Domain.ValueObjects;
using Pagos.Infrastructure.Configurations;

namespace Pagos.Infrastructure.Persistences.Repositories.MongoWrite
{
    public class PagoWriteRepository: IPagoRepository
    {
        private readonly IMongoCollection<BsonDocument> PagoColexion;

        public PagoWriteRepository(MongoWritePagoDbConfig mongoConfig)
        {
            PagoColexion = mongoConfig.db.GetCollection<BsonDocument>("pagos_write");
        }

        async public Task AgregarPago(Pago pago)
        {
            try
            {
                var pagoInsert = new BsonDocument
                {
                    { "_id", pago.IdPago.IdPago },
                    { "idUsuario", pago.IdUsuario.IdUsuario },
                    { "idMPago", pago.IdMPago.IdMPago },
                    { "idAuction", pago.IdAuction.IdAuction },
                    { "monto", pago.Monto.Monto },
                    { "fechaPago", pago.FechaPago.FechaPago },
                    { "idExternalPago", pago.IdExternalPago == null ? BsonNull.Value : pago.IdExternalPago.IdExternalPago}
                };
                await PagoColexion.InsertOneAsync(pagoInsert);
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

        async public Task EliminarPago(string idPago)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", idPago);
                await PagoColexion.DeleteOneAsync(filter);
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

        async public Task<Pago?> ObtenerPagoPorId(string idPago)
        {
            try {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", idPago);
                var pagoDoc = await PagoColexion.Find(filter).FirstOrDefaultAsync();

                if (pagoDoc == null)
                {
                    return null;
                }

                return new Pago(
                    new VOIdPago(pagoDoc["_id"].AsString),
                    new VOIdMPago(pagoDoc["idMPago"].AsString),
                    new VOIdUsuario(pagoDoc["idUsuario"].AsString),
                    new VOFechaPago(pagoDoc["fechaPago"].ToUniversalTime()),
                    new VOMonto(pagoDoc["monto"].AsDecimal),
                    new VOIdAuction(pagoDoc["idAuction"].AsString),
                    pagoDoc.Contains("idExternalPago") && !pagoDoc["idExternalPago"].IsBsonNull
                        ? new VOIdExternalPago(pagoDoc["idExternalPago"].AsString)
                        : null
                );
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

        async public Task ActualizarIdPagoExterno(string idPago, string idExternalPago)
        {
            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("_id", idPago);
                var update = Builders<BsonDocument>.Update.Set("idExternalPago", idExternalPago);
                await PagoColexion.UpdateOneAsync(filter, update);
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
