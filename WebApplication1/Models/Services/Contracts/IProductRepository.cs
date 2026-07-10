using MasterDetails.Models.DomainModels.ProductAggregates;

namespace MasterDetails.Models.Services.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> SelectProductAllAsync();

        Task<Product?> selectByIdAsync(Guid id);
    }
}
