using MasterDetails.Models.DomainModels.OrderHeaderAggregates;


namespace MasterDetail.Repositories.Contract;


public interface IOrderRepository
{

    Task<OrderHeader?> GetByIdAsync(Guid id);


    Task<List<OrderHeader>> GetAllAsync();


    Task InsertAsync(OrderHeader order);


    Task UpdateAsync(OrderHeader order);


    // Task DeleteAsync(Guid id);
    Task DeleteAsync(OrderHeader order);


}