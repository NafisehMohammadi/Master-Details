using MasterDetails.Models.DomainModels;
using MasterDetails.Models.DomainModels.OrderDetailAggregates;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using MasterDetails.Models.DomainModels.ProductAggregates;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(
            DbContextOptions<ProjectDbContext> options)
            : base(options)
        {

        }


        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }



        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ProjectDbContext).Assembly);


            base.OnModelCreating(modelBuilder);
        }
    }
}