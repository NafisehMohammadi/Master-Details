using MasterDetails.Models.DomainModels.ProductAggregates;
using MasterDetails.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.Models.Services.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region [- Private Fields() -]
        private readonly ProjectDbContext _projectDbContext;
        #endregion

        #region [- Ctor() -]
        public ProductRepository(ProjectDbContext context)
        {
            _projectDbContext = context;
        }
         #endregion

        #region [- SelectProductAllAsync() -]
        public async Task<IEnumerable<Product>> SelectProductAllAsync()
        {
           // return  await _projectDbContext.Product.ToListAsync();
            return await _projectDbContext.Products .AsNoTracking()
                                .OrderBy(x => x.Title).ToListAsync();
        }
        #endregion

        #region [- selectByIdAsync() -]
        public async Task<Product?> selectByIdAsync(Guid id)
        {
            return await _projectDbContext.Products

           .AsNoTracking()

           .FirstOrDefaultAsync(x => x.Id == id);
        }
        #endregion

    }
}
