using MasterDetail.ApplicationServices.Dtos.OrderDetailDtos;
using MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;

namespace MasterDetails.ApplicationServices.Helper.Mapper
{
    public static class OrderMapper
    {
        public static GetOrderHeaderDto ToGetOrderHeaderDto(  OrderHeader order)
        {
            return new GetOrderHeaderDto
            {
                Id = order.Id,

                OrderNumber = order.OrderNumber,

                CustomerId = order.CustomerId,

                CustomerName =  $"{order.Customer.FirstName} {order.Customer.LastName}",

                OrderDate = order.OrderDate,

                TotalAmount = order.TotalAmount,

                Description = order.Description,

                Details = order.Details.Select(detail =>
                    new GetOrderDetailDto
                    {
                        Id = detail.Id,

                        ProductId = detail.ProductId,

                        ProductTitle=detail.Product.Title,

                        Quantity = detail.Quantity,

                        UnitPrice = detail.UnitPrice,

                        LineTotal = detail.LineTotal
                    }).ToList()
            };
        }
        public static List<GetOrderHeaderDto> ToGetOrderHeaderDtoList( List<OrderHeader> orders)
        {
            return orders
                .Select(ToGetOrderHeaderDto)
                .ToList();
        }
        public static GetAllOrderHeaderDto ToGetAllOrderHeaderDto( OrderHeader order)
        {
            return new GetAllOrderHeaderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = $"{order.Customer.FirstName} {order.Customer.LastName}",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Description = order.Description
            };
        }
        public static List<GetAllOrderHeaderDto> ToGetAllOrderHeaderDtoList(List<OrderHeader> orders)
        {
            return orders
                .Select(ToGetAllOrderHeaderDto)
                .ToList();
        }
    }
}