using MasterDetails.ApplicationServices.Services.Contracts.ContractCustomer;
using MasterDetails.ApplicationServices.Services.Contracts.ContractProduct;
using Microsoft.AspNetCore.Mvc;

namespace MasterDetails.Controllers
{
    public class ProductController : Controller
    {
        #region [-Private Field-]
        private readonly IProductApplicationService _productApplicationService; 

        #endregion
        #region[- ctor  -]
        public ProductController (IProductApplicationService productApplicationService)
        {
            _productApplicationService = productApplicationService;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        #region[- GetAll() -] 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productApplicationService.GetAllAsync();
            return Ok(result);

        }

        #endregion
    }
}
