using MasterDetail.Repositories.Helpers;
using MasterDetails.Models.DomainModels.OrderDetailAggregates;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using System.Data.Common;

namespace MasterDetail.Repositories.Mappers
{
    public static class OrderListDataReaderMapper
    {
        public static List<OrderHeader> Map(DbDataReader reader)
        {
            var orders = new Dictionary<Guid, OrderHeader>();

            while (reader.Read())
            {
                var orderId = reader.GetGuid("Id");

                if (!orders.TryGetValue(orderId, out var order))
                {
                    order = new OrderHeader
                    {
                        Id = orderId,

                        OrderNumber = reader.GetString("OrderNumber"),

                        CustomerId = reader.GetGuid("CustomerId"),

                      //  CustomerName = reader.GetString("CustomerName"),

                        OrderDate = reader.GetDateTime("OrderDate"),

                        TotalAmount = reader.GetDecimal("TotalAmount"),

                        Description = reader.GetNullableString("Description"),

                        Details = new List<OrderDetail>()
                    };

                    orders.Add(orderId, order);
                }

                if (!reader.IsDBNull(reader.GetOrdinal("DetailId")))
                {
                    order.Details.Add(new OrderDetail
                    {
                        Id = reader.GetGuid("DetailId"),

                        ProductId = reader.GetGuid("ProductId"),

                        Quantity = reader.GetDecimal("Quantity"),

                        UnitPrice = reader.GetDecimal("UnitPrice"),

                        LineTotal = reader.GetDecimal("LineTotal")
                    });
                }
            }

            return orders.Values.ToList();
        }
    }
}