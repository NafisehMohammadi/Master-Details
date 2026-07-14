namespace MasterDetails.ApplicationServices.Dtos.OrderDetailDtos
{
    public class PutOrderDetailDto
    {
        public Guid? Id { get; set; }

        public Guid GuidKey { get; set; }

        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
