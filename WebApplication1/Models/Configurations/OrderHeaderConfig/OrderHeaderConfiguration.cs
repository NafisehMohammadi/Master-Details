using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterDetails.Models.Configurations
{
    public class OrderHeaderConfiguration
        : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.ToTable("OrderHeader");

            builder.HasKey(x => x.Id);


            builder.Property(x => x.OrderNumber)
                   .HasMaxLength(40)
                   .IsRequired();


            builder.Property(x => x.Description)
                   .HasMaxLength(1000);


            builder.Property(x => x.TotalAmount)
                   .HasPrecision(18, 2);


            builder.Property(x => x.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");


            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);


            // Relation: Customer 1 ---- N OrderHeader

            builder.HasOne(x => x.Customer)

                   .WithMany(x => x.Orders)

                   .HasForeignKey(x => x.CustomerId)

                   .OnDelete(DeleteBehavior.Restrict);



            // Relation: OrderHeader 1 ---- N OrderDetail

            builder.HasMany(x => x.Details)

                   .WithOne(x => x.OrderHeader)

                   .HasForeignKey(x => x.OrderHeaderId)

                   .OnDelete(DeleteBehavior.Cascade);



            builder.HasIndex(x => x.OrderNumber)

                   .IsUnique();
        }
    }
}