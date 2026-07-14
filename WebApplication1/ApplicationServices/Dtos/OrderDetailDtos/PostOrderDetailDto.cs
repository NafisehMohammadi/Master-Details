namespace MasterDetails.ApplicationServices.Dtos.OrderDetailDtos
{
    public class PostOrderDetailDto
    {
        
        public Guid GuidKey { get; set; }
        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
