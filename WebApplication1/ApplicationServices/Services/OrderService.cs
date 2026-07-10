using MasterDetail.ApplicationServices.Dtos.OrderDetailDtos;
using MasterDetail.Frameworks.ResponseFrameworks.Contracts;
using MasterDetail.Repositories.Contract;
using MasterDetail.Repositories.Mappers;
using MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;
using MasterDetails.ApplicationServices.Helper.Mapper;
using MasterDetails.ApplicationServices.Services.Contracts.ContractOrder;
using MasterDetails.Frameworks.ResponseFrameworks;
using MasterDetails.Models.DomainModels.OrderDetailAggregates;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using MasterDetails.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace MasterDetails.ApplicationServices.Services
{
    public class OrderService : IOrderService
    {
        #region [- Private Fields() -]
        private readonly IOrderRepository _orderRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region [- Ctor() -]
        public OrderService(IOrderRepository orderRepository, IResponseFactory responseFactory,ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _responseFactory = responseFactory;
            _customerRepository = customerRepository;
        }
        #endregion

        #region [- PostAsync() -]
        public async Task<Response<Guid>> PostOrderAsync(PostOrderHeaderDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return _responseFactory.BadRequest<Guid>(
                        ResponseMessages.InvalidInput,
                        ErrorCodes.InvalidInput);
                }

                if (dto.Details == null || !dto.Details.Any())
                {
                    return _responseFactory.ValidationFail<Guid>(
                        new List<ValidationError>
                        {
                            new ValidationError
                            {
                                PropertyName = "Details",
                                ErrorMessage = ResponseMessages.OrderMustHaveAtLeastOneItem
                            }
                        });
                }
                var customer = await _customerRepository.SelectCustomerById(dto.CustomerId);

                if (customer == null)
                {
                    return _responseFactory.NotFound<Guid>(
                        ResponseMessages.CustomerNotFound,
                        ErrorCodes.CustomerNotFound);
                }
                var orderId = Guid.NewGuid();
                var orderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmss}";
                var order = new OrderHeader
                {
                    Id =orderId,
                    OrderNumber= orderNumber,
                    CustomerId = dto.CustomerId,
                    OrderDate = dto.OrderDate,
                    Description = dto.Description ,
                    CreatedDate = DateTime.UtcNow,
                    IsDeleted = false,
                    Details = dto.Details.Select(x => new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        UnitPrice = x.UnitPrice,
                        LineTotal = x.Quantity * x.UnitPrice,
                        CreatedDate = DateTime.UtcNow,
                        IsDeleted = false
                    }).ToList()
                };
                order.TotalAmount = order.Details.Sum(x => x.LineTotal);

                await _orderRepository.InsertAsync(order);

                return _responseFactory.Success(
                    order.Id, ResponseMessages.CreatedSuccessfully);
            }
             catch
             {
                 return _responseFactory.Exception<Guid>(
                     ResponseMessages.UnexpectedError,
                     ErrorCodes.UnexpectedError);
             }
         
        }
        #endregion


        #region [- PutOrderAsync() -]
        public async Task<Response<bool>> PutOrderAsync(PutOrderHeaderDto dto)
        {
            try
            {
                if (dto == null || dto.Id == Guid.Empty)
                {
                    return _responseFactory.BadRequest<bool>(
                        ResponseMessages.InvalidOrderId,
                        ErrorCodes.InvalidInput);
                }



                if (dto.Details == null || !dto.Details.Any())
                {
                    return _responseFactory.ValidationFail<bool>(
                        new List<ValidationError>
                        {
                    new ValidationError
                    {
                        PropertyName = "Details",
                        ErrorMessage =
                        ResponseMessages.OrderMustHaveAtLeastOneItem
                    }
                        });
                }



                var order =
                    await _orderRepository.GetByIdAsync(dto.Id);



                if (order == null)
                {
                    return _responseFactory.NotFound<bool>(
                        ResponseMessages.OrderNotFound,
                        ErrorCodes.OrderNotFound);
                }



                var customer =
                    await _customerRepository.SelectCustomerById(dto.CustomerId);

                if (customer == null)
                {
                    return _responseFactory.NotFound<bool>(
                        ResponseMessages.CustomerNotFound,
                        ErrorCodes.CustomerNotFound);
                }


                order.CustomerId = dto.CustomerId;

                order.OrderDate = dto.OrderDate;

                order.Description = dto.Description;


                order.Details =
                    dto.Details.Select(d =>
                    new OrderDetail
                    {
                        Id = Guid.NewGuid(),

                        ProductId = d.ProductId,

                        Quantity = d.Quantity,

                        UnitPrice = d.UnitPrice,

                        LineTotal =
                            d.Quantity * d.UnitPrice,

                        CreatedDate =
                            DateTime.UtcNow,

                        IsDeleted = false

                    }).ToList();



                order.TotalAmount =
                    order.Details.Sum(x => x.LineTotal);



                await _orderRepository.UpdateAsync(order);



                return _responseFactory.Success(
                    true,
                    ResponseMessages.UpdatedSuccessfully);

            }
            catch
            {
                return _responseFactory.Exception<bool>(
                    ResponseMessages.UnexpectedError,
                    ErrorCodes.UnexpectedError);
            }
        }
        #endregion

        #region [- GetByIdAsync() -]

        public async Task<Response<GetOrderHeaderDto>> GetByIdAsync(GetOrderHeaderDto headerdto)
        {
            try
            {
                if (headerdto == null)
                {
                    return _responseFactory.BadRequest<GetOrderHeaderDto>(
                       ResponseMessages.InvalidOrderId,
                       ErrorCodes.InvalidOrderId);
                }
                if (headerdto.Id == Guid.Empty)
                {
                    return _responseFactory.BadRequest<GetOrderHeaderDto>(
                    ResponseMessages.InvalidOrderId,
                    ErrorCodes.InvalidOrderId);
                }
                var order = await _orderRepository.GetByIdAsync(headerdto.Id);
                if (order == null)
                {
                    return _responseFactory.NotFound<GetOrderHeaderDto>
                        (ResponseMessages.OrderNotFound,
                        ErrorCodes.OrderNotFound);
                }
                var dto = OrderMapper.ToGetOrderHeaderDto(order);
                return _responseFactory.Success(dto,
                    ResponseMessages.ListRetrievedSuccessfully);
            }
            catch
            {
                return _responseFactory.Exception<GetOrderHeaderDto>(
                    ResponseMessages.UnexpectedError,
                    ErrorCodes.UnexpectedError);
            }
        }

        /* public async Task<Response<GetOrderHeaderDto>> GetByIdAsync(Guid orderid)
      {
         var order=await _orderRepository.GetByIdAsync(orderid);
          if (order == null)
          {
              return _responseFactory.NotFound<GetOrderHeaderDto>(ResponseMessages.OrderNotFound,
                                                                  ErrorCodes.OrderNotFound);
          }
          var dto = OrderMapper.ToGetOrderHeaderDto(order);

          return _responseFactory.Success(dto, ResponseMessages.OrderRetrievedSuccessfully);

      }*/
        #endregion

        #region [- GetAllAsync() -]
        public async Task<Response<List<GetAllOrderHeaderDto>>> GetAllAsync()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();

                if (orders == null || !orders.Any())
                {
                    return _responseFactory.Success(
                        new List<GetAllOrderHeaderDto>(),
                        ResponseMessages.ListRetrievedSuccessfully);
                }

                var dto = OrderMapper.ToGetAllOrderHeaderDtoList(orders);

                return _responseFactory.Success(
                    dto,
                    ResponseMessages.ListRetrievedSuccessfully);
            }
             catch
             {
                 return _responseFactory.Exception<List<GetAllOrderHeaderDto>>(
                     ResponseMessages.UnexpectedError,
                     ErrorCodes.UnexpectedError);
             }
        }

        #endregion


        #region [- DeleteAsync() -]
        public async Task<Response<bool>> DeleteAsync(DeleteOrderHeaderDto delHeaderdto)
        {
            try
            {
                if (delHeaderdto == null)
                {
                    return _responseFactory.BadRequest<bool>(
                        ResponseMessages.InvalidInput,
                        ErrorCodes.InvalidInput);

                }
                if (delHeaderdto.Id == Guid.Empty)
                {
                    return _responseFactory.BadRequest<bool>(
                    ResponseMessages.InvalidOrderId,
                    ErrorCodes.InvalidOrderId);
                }
         
                var order = await _orderRepository.GetByIdAsync(delHeaderdto.Id);

                if (order == null)
                {
                    return _responseFactory.NotFound<bool>(
                        ResponseMessages.OrderNotFound,
                        ErrorCodes.OrderNotFound);
                }

                await _orderRepository.DeleteAsync(order);

                return _responseFactory.Success(true,
                    ResponseMessages.DeletedSuccessfully);
            }
            catch
            {
                return _responseFactory.Exception<bool>(
                    ResponseMessages.UnexpectedError,
                    ErrorCodes.UnexpectedError);
            }
        }

       
        /*  public async Task<Response<bool>> DeleteAsync(Guid id)
         {
             var order = await _orderRepository.GetByIdAsync(id);

             if (order == null)
             {
                 return _responseFactory.NotFound<bool>(
                     ResponseMessages.OrderNotFound,
                     ErrorCodes.OrderNotFound);
             }

             await _orderRepository.DeleteAsync(id);

             return _responseFactory.Success(
                 true,
                 ResponseMessages.OrderDeletedSuccessfully);
         }
       */


        #endregion
    }
}

