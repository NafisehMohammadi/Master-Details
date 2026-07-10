using MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;
using MasterDetails.Frameworks.ResponseFrameworks;

namespace MasterDetails.ApplicationServices.Services.Contracts.ContractOrder;

public interface IOrderService
{
    Task<Response<Guid>> PostOrderAsync(PostOrderHeaderDto dto);

    Task<Response<bool>> PutOrderAsync(PutOrderHeaderDto dto);

    Task<Response<GetOrderHeaderDto>> GetByIdAsync(GetOrderHeaderDto dto);
  //  Task<Response<GetOrderHeaderDto>> GetByIdAsync(Guid id);

    Task<Response<List<GetAllOrderHeaderDto>>> GetAllAsync();

    Task<Response<bool>> DeleteAsync(DeleteOrderHeaderDto dto);
   // Task<Response<bool>> DeleteAsync(Guid id);
}