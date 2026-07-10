using MasterDetail.Frameworks.ResponseFrameworks.Contracts;
using MasterDetails.ApplicationServices.Dtos.CustomerDto;
using MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;
using MasterDetails.ApplicationServices.Dtos.ProductDto;
using MasterDetails.ApplicationServices.Helper.Mapper;
using MasterDetails.ApplicationServices.Services.Contracts.ContractCustomer;
using MasterDetails.Frameworks.ResponseFrameworks;
using MasterDetails.Models.DomainModels;
using MasterDetails.Models.DomainModels.ProductAggregates;
using MasterDetails.Models.Services.Contracts;

namespace MasterDetails.ApplicationServices.Services
{
    public class CustomerService : ICustomerApplicationService
    {
        #region[- Private Field -]
        private readonly ICustomerRepository _customerRepository;
        private readonly IResponseFactory _responseFactory;
        #endregion

        #region[- ctor() -]
        public CustomerService(ICustomerRepository customerRepository, IResponseFactory responseFactory)
        {
            _customerRepository = customerRepository;
            _responseFactory = responseFactory;
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<Response<List<GetAllCustomerDto>>> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepository.SelectCustomerAllAsync();


                if (customers == null)
                {
                    return _responseFactory.Fail<List<GetAllCustomerDto>>(
                        ResponseMessages.DatabaseError,
                        ResponseStatus.DatabaseError,
                        ErrorCodes.DatabaseError);
                }
                if (!customers.Any())
                {
                    return _responseFactory.Success(
                        new List<GetAllCustomerDto>(),
                        ResponseMessages.ListRetrievedSuccessfully);
                }

                var dto = customers.Select(customer => new GetAllCustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone

                }).ToList();


                   return _responseFactory.Success(
                       dto,
                       ResponseMessages.ListRetrievedSuccessfully);
               
            }
            catch (Exception)
            {
                return _responseFactory.Exception<List<GetAllCustomerDto>>(
                    ResponseMessages.UnexpectedError,
                    ErrorCodes.UnexpectedError);
            }
        }

        #endregion

        #region [- GetByIdAsync() -]
       /* public async Task<Response<GetCustomerDto>> GetByIdAsync(GetCustomerDto getCustomerDto)
        {
            try
            {
                if (getCustomerDto.Id == Guid.Empty)
                {
                    return _responseFactory.BadRequest<GetCustomerDto>(
                        ResponseMessages.InvalidCustomerId,
                        ErrorCodes.InvalidInput);
                }

                var customer = await _customerRepository.SelectCustomerById(getCustomerDto.Id);

                if (customer == null)
                {
                    return _responseFactory.NotFound<GetCustomerDto>(
                        ResponseMessages.CustomerNotFound,
                        ErrorCodes.CustomerNotFound);
                }

                //  var dto = CustomerMapper.ToDto(customer);
                var dtocustomer = new Customer()
                {
                    Id = getCustomerDto.Id,
                    FirstName = getCustomerDto.FirstName,
                    LastName = getCustomerDto.LastName,
                   Phone= getCustomerDto.Phone     

                };

                return _responseFactory.Success(
                    dtocustomer,
                    ResponseMessages.RetrievedSuccessfully);
            }
            catch
            {
                return _responseFactory.Exception<GetCustomerDto>(
                    ResponseMessages.UnexpectedError,
                    ErrorCodes.UnexpectedError);
            }
        }*/
        #endregion
    }
}
