namespace MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;

public class GetAllOrderHeaderDto
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }
    public string? Description { get; set; }
}
