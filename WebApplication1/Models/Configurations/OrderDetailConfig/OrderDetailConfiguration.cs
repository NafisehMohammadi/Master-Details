using MasterDetails.Models.DomainModels.OrderDetailAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterDetails.Models.Configurations
{
    public class OrderDetailConfiguration
        : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(
            EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetail");


            builder.HasKey(x => x.Id);



            builder.Property(x => x.Quantity)
                   .HasPrecision(18, 2);



            builder.Property(x => x.UnitPrice)
                   .HasPrecision(18, 2);



            builder.Property(x => x.LineTotal)
                   .HasPrecision(18, 2);


            // Product - OrderDetail

            builder.HasOne(x => x.Product)

                   .WithMany()

                   .HasForeignKey(x => x.ProductId)

                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}