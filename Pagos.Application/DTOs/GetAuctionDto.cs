
namespace Pagos.Application.DTOs
{
    public class GetAuctionDto
    {
        public string id { get; init; }
        public string userId { get; init; }
        public string productId { get; init; }
        public int productQuantity { get; init; }
        public string name { get; init; }
        public string description { get; init; }
        public string image { get; init; }
        public string status { get; init; }
        public decimal basePrice { get; init; }
        public int duration { get; init; }
        public decimal minimumIncrease { get; init; }
        public decimal reservePrice { get; init; }
        public DateTime startDate { get; init; }
        public string? productName { get; init; }
        public string? productImage { get; init; }
    }
}
