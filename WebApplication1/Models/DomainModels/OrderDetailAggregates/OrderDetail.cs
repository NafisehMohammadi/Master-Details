using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using MasterDetails.Models.DomainModels.ProductAggregates;

namespace MasterDetails.Models.DomainModels.OrderDetailAggregates
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid GuidKey { get; set; }
        public Guid OrderHeaderId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal LineTotal { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation Property
        public OrderHeader OrderHeader { get; set; } = null!;

        // Navigation Property
        public Product Product { get; set; } = null!;
    }
}