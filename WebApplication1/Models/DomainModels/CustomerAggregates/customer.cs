using MasterDetails.Models.DomainModels.OrderHeaderAggregates;

namespace MasterDetails.Models.DomainModels
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<OrderHeader> Orders { get; set; }
            = new List<OrderHeader>();
    }
}