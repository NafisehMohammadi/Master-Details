using MasterDetail.Repositories.Helpers;
using MasterDetails.Models.DomainModels.OrderDetailAggregates;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using System.Data.Common;

namespace MasterDetail.Repositories.Mappers
{
    public static class OrderDataReaderMapper
    {
        public static OrderHeader? MapOrder(
            DbDataReader reader)
        {
            OrderHeader? order = null;

            while (reader.Read())
            {
                if (order == null)
                {
                    order = new OrderHeader
                    {
                        Id = reader.GetGuid("Id"),

                        OrderNumber = reader.GetString("OrderNumber"),

                        CustomerId = reader.GetGuid("CustomerId"),

                     //  CustomerName = reader.GetString("CustomerName"),

                        OrderDate = reader.GetDateTime("OrderDate"),

                        TotalAmount = reader.GetDecimal("TotalAmount"),

                        Description = reader.GetNullableString("Description"),

                        Details = new List<OrderDetail>()
                    };
                }
                if (!reader.IsDBNull(reader.GetOrdinal("DetailId")))
                {
                    order.Details.Add(
                        new OrderDetail
                        {
                            Id = reader.GetGuid("DetailId"),

                            ProductId = reader.GetGuid("ProductId"),

                            Quantity = reader.GetDecimal("Quantity"),

                            UnitPrice = reader.GetDecimal("UnitPrice"),

                            LineTotal = reader.GetDecimal("LineTotal")
                        });
                }
               
            }

            return order;
        }
    }
}