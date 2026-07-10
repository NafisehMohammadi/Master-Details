using MasterDetails.Models.DomainModels.OrderDetailAggregates;

namespace MasterDetails.Models.DomainModels.ProductAggregates
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string DescriptionRecord { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

      /*  // Navigation Property
        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();*/
    }
}