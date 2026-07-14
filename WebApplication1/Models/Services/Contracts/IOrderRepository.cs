using MasterDetails.Models.DomainModels.OrderHeaderAggregates;


namespace MasterDetail.Repositories.Contract;


public interface IOrderRepository
{

    Task CreateAsync(OrderHeader order);
   Task UpdateAsync(OrderHeader order);
    Task<OrderHeader?> GetByIdAsync(Guid id);
    Task<List<OrderHeader>> GetAllAsync();
    // Task DeleteAsync(Guid id);
    Task DeleteAsync(OrderHeader order);


}