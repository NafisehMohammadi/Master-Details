using MasterDetails.ApplicationServices.Services.Contracts.ContractCustomer;
using MasterDetails.Frameworks.ResponseFrameworks;
using Microsoft.AspNetCore.Mvc;

namespace MasterDetails.Controllers
{
    public class CustomerController : Controller
    {
        #region [-Private Field-]
        private readonly ICustomerApplicationService _customerApplicationService;
        #endregion
        #region[- ctor -]
        public CustomerController(ICustomerApplicationService customerApplicationService )
        {
            _customerApplicationService = customerApplicationService;
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
            var result = await _customerApplicationService.GetAllAsync();
            return Ok(result);
          
        }
        #endregion
    }
}
