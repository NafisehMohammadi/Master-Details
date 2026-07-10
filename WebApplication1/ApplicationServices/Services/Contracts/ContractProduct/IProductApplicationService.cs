
using MasterDetails.ApplicationServices.Dtos.ProductDto;
using MasterDetails.Frameworks.ResponseFrameworks;

namespace MasterDetails.ApplicationServices.Services.Contracts.ContractProduct
{
    public interface IProductApplicationService
    {
        Task<Response<List<GetProductDto>>> GetAllAsync();

    }
}
