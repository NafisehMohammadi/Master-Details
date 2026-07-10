using MasterDetails.ApplicationServices.Dtos.OrderDetailDtos;

namespace MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos
{
    public class PostOrderHeaderDto
    {
        public Guid CustomerId { get; set; }


        public DateTime OrderDate { get; set; }


        public string? Description { get; set; }


        public List<PostOrderDetailDto> Details { get; set; }
            = new();
    }
}