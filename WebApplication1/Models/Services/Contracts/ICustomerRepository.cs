using MasterDetails.Models.DomainModels;


namespace MasterDetails.Models.Services.Contracts
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> SelectCustomerAllAsync();
        Task<Customer> SelectCustomerById(Guid id);

    }




}
