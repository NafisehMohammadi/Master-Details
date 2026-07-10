using MasterDetails.ApplicationServices.Dtos.CustomerDto;
using MasterDetails.Frameworks.ResponseFrameworks;

namespace MasterDetails.ApplicationServices.Services.Contracts.ContractCustomer
{
    public interface ICustomerApplicationService
    {
        Task<Response<List<GetAllCustomerDto>>> GetAllAsync();
       // Task<Response<GetCustomerDto>> GetByIdAsync(GetCustomerDto getCustomerDto);

    }
}
