using MasterDetails.ApplicationServices.Dtos.OrderDetailDtos;

    namespace MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos
    {
    public class PutOrderHeaderDto
    {
      
        public Guid Id { get; set; }

        public Guid GuidKey { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public string? Description { get; set; }

        public List<PutOrderDetailDto> Details { get; set; } = new();
    }
}

