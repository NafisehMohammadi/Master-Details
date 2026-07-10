using MasterDetails.Models.DomainModels;
using MasterDetails.Models.DomainModels.OrderDetailAggregates;

namespace MasterDetails.Models.DomainModels.OrderHeaderAggregates
{
    public class OrderHeader
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation Property
        public Customer Customer { get; set; } = null!;

        // Navigation Property
        public ICollection<OrderDetail> Details { get; set; }
            = new List<OrderDetail>();
    }
}