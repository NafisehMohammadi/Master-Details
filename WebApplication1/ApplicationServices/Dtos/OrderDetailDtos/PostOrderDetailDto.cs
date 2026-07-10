namespace MasterDetails.ApplicationServices.Dtos.OrderDetailDtos
{
    public class PostOrderDetailDto
    {
        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
