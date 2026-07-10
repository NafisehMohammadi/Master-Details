namespace MasterDetail.ApplicationServices.Dtos.OrderDetailDtos;

public class DeleteOrderDetailDto
{
    public Guid Id { get; set; }

    public Guid OrderHeaderId { get; set; }
}