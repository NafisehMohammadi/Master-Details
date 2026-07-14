using MasterDetail.ApplicationServices.Dtos.OrderDetailDtos;
using MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;
using MasterDetails.Models.DomainModels.OrderDetailAggregates;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;

namespace MasterDetails.ApplicationServices.Helper.Mapper
{
    public static class OrderMapper
    {

        #region Create Mapping DTO -> Entity

        public static OrderHeader ToOrderHeader(
            PostOrderHeaderDto dto)
        {
            return new OrderHeader
            {
                GuidKey = dto.GuidKey,

                CustomerId = dto.CustomerId,

                OrderDate = dto.OrderDate,

                Description = dto.Description,

                Details = dto.Details
                    .Select(detail => new OrderDetail
                    {
                        GuidKey = detail.GuidKey,

                        ProductId = detail.ProductId,

                        Quantity = detail.Quantity,

                        UnitPrice = detail.UnitPrice,

                        CreatedDate = DateTime.UtcNow,

                        IsDeleted = false

                    })
                    .ToList()
            };
        }

        #endregion



        #region Get By Id Mapping Entity -> DTO

        public static GetOrderHeaderDto ToGetOrderHeaderDto(
            OrderHeader order)
        {
            return new GetOrderHeaderDto
            {
                Id = order.Id,

                GuidKey = order.GuidKey,

                OrderNumber = order.OrderNumber,

                CustomerId = order.CustomerId,

                CustomerName =
                    $"{order.Customer.FirstName} {order.Customer.LastName}",

                OrderDate = order.OrderDate,

                TotalAmount = order.TotalAmount,

                Description = order.Description,


                Details = order.Details
                    .Select(detail => new GetOrderDetailDto
                    {
                        Id = detail.Id,

                        ProductId = detail.ProductId,

                        ProductTitle =
                            detail.Product.Title,

                        Quantity = detail.Quantity,

                        UnitPrice = detail.UnitPrice,

                        LineTotal = detail.LineTotal

                    })
                    .ToList()
            };
        }

        #endregion



        #region GetAll Mapping


        public static GetAllOrderHeaderDto ToGetAllOrderHeaderDto(
            OrderHeader order)
        {
            return new GetAllOrderHeaderDto
            {
                Id = order.Id,

                OrderNumber = order.OrderNumber,

                CustomerId = order.CustomerId,

                CustomerName =
                    $"{order.Customer.FirstName} {order.Customer.LastName}",

                OrderDate = order.OrderDate,

                TotalAmount = order.TotalAmount,

                Description = order.Description
            };
        }


        public static List<GetAllOrderHeaderDto>
            ToGetAllOrderHeaderDtoList(
            List<OrderHeader> orders)
        {
            return orders
                .Select(ToGetAllOrderHeaderDto)
                .ToList();
        }


        #endregion



        #region Update Mapping DTO -> Existing Entity

        public static void UpdateOrderHeader(
     OrderHeader order,
     PutOrderHeaderDto dto)
        {
            order.CustomerId = dto.CustomerId;

            order.OrderDate = dto.OrderDate;

            order.Description = dto.Description;

            order.GuidKey = dto.GuidKey;


            order.Details =
                dto.Details.Select(detail =>
                new OrderDetail
                {
                    Id = detail.Id ?? Guid.Empty,

                    GuidKey = detail.GuidKey,

                    ProductId = detail.ProductId,

                    Quantity = detail.Quantity,

                    UnitPrice = detail.UnitPrice,

                    LineTotal =
                        detail.Quantity *
                        detail.UnitPrice,

                    IsDeleted = false,

                    CreatedDate = DateTime.UtcNow
                })
                .ToList();
        }

        #endregion


    }
}