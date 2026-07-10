namespace MasterDetail.ApplicationServices.Dtos.OrderDetailDtos;

public class GetOrderDetailDto
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string ProductTitle { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal LineTotal { get; set; }
}