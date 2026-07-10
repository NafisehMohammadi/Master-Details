using MasterDetails.Models.DomainModels.ProductAggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterDetails.Models.Configurations
{
    public class ProductConfiguration
        : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.DescriptionRecord)
                   .HasMaxLength(500);

            builder.Property(x => x.UnitPrice)
                   .HasPrecision(18, 2);

        /*    builder.HasMany(x => x.OrderDetails)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);*/
        }
    }
}