using MasterDetail.Frameworks.ResponseFrameworks.Contracts;
using MasterDetails.ApplicationServices.Dtos.CustomerDto;
using MasterDetails.ApplicationServices.Dtos.ProductDto;
using MasterDetails.ApplicationServices.Services.Contracts.ContractProduct;
using MasterDetails.Frameworks.ResponseFrameworks;
using MasterDetails.Models.Services.Contracts;


namespace MasterDetails.ApplicationServices.Services
{
    public class ProductService : IProductApplicationService
    { 
        #region[- Private Field -]
        private readonly IProductRepository _productRepository;
        private readonly IResponseFactory _responseFactory;
        #endregion

        #region[- ctor() -]
        public ProductService(IProductRepository productRepository, IResponseFactory responseFactory)
        {
            _productRepository = productRepository;
            _responseFactory = responseFactory;
        }
        #endregion

        #region [- GetAllAsync() -]
        public async Task<Response<List<GetProductDto>>> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.SelectProductAllAsync();


                if (products == null)
                {
                    return _responseFactory.Fail<List<GetProductDto>>(
                        ResponseMessages.DatabaseError,
                        ResponseStatus.DatabaseError,
                        ErrorCodes.DatabaseError);
                }
                if (!products.Any())
                {
                    return _responseFactory.Success(
                        new List<GetProductDto>(),
                        ResponseMessages.ListRetrievedSuccessfully);
                }

                var dto = products.Select(product => new GetProductDto
                {
                    Id = product.Id,
                   Title=product.Title,
                   DescriptionRecord=product.DescriptionRecord,
                   UnitPrice=product.UnitPrice

                }).ToList();


                return _responseFactory.Success(
                    dto,
                    ResponseMessages.ListRetrievedSuccessfully);
              
            }
            catch (Exception)
            {
                return _responseFactory.Exception<List<GetProductDto>>(
                    ResponseMessages.UnexpectedError,
                    ErrorCodes.UnexpectedError);
            }
        }
        #endregion
    }
}
