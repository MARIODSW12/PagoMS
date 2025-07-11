
using Pagos.Domain.Exceptions;

namespace Pagos.Domain.ValueObjects
{
    public class VOIdAuction
    {
        public string IdAuction { get; private set; }
        public VOIdAuction(string idAuction)
        {
            if (string.IsNullOrWhiteSpace(idAuction))
                throw new IdAuctionInvalido();

            if (!Guid.TryParse(idAuction, out _))
                throw new IdAuctionInvalido();

            IdAuction = idAuction;
        }
    }
}
