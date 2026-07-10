using MasterDetail.ApplicationServices.Dtos.OrderDetailDtos;
using MasterDetails.ApplicationServices.Dtos.OrderDetailDtos;

namespace MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;

public class GetOrderHeaderDto
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Description { get; set; }

    public List<GetOrderDetailDto> Details { get; set; } = new();
}