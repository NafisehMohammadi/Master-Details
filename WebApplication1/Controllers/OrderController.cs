using MasterDetails.ApplicationServices.Dtos.OrderHeaderDtos;
using MasterDetails.ApplicationServices.Services.Contracts.ContractOrder;
using MasterDetails.Frameworks.ResponseFrameworks;
using Microsoft.AspNetCore.Mvc;

namespace MasterDetails.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        #region[-Private Field-]
        private readonly IOrderService _orderService;
        #endregion

        #region[-ctor-]
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        #region [- Index() -]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region [- Post() -]   
        [HttpPost("PostOrderAsync")]
       
        public async Task<IActionResult> PostOrderAsync( [FromBody] PostOrderHeaderDto postOrderHeaderDto)
        {
            try
            {
                var result =
                    await _orderService.PostOrderAsync(postOrderHeaderDto);


                if (result.IsSuccess)
                {
                    return Ok(result);
                }


                if (result.Status == ResponseStatus.ValidationError)
                {
                    return BadRequest(result);
                }


                return StatusCode(500, result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        #endregion

        #region [- Put() -]
        [HttpPut("PutOrderAsync")]
        public async Task<IActionResult> PutOrderAsync([FromBody] PutOrderHeaderDto dto)
        {
            var result = await _orderService.PutOrderAsync(dto);
            return Ok(result);
        }
        #endregion

        #region[- GetAll() -] 
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            /*  var result = await _orderService.GetAllOrdersAsync();

              if (result.IsSuccess)
                  return Ok(result);

              if (result.Status == ResponseStatus.ValidationError)
                  return BadRequest(result);

              if (result.Status == ResponseStatus.NotFound)
                  return NotFound(result);

              return StatusCode(500, result);*/
            var result = await _orderService.GetAllAsync();

            if (result.IsSuccess)
                return Ok(result);

            switch (result.Status)
            {
                case ResponseStatus.ValidationError:
                    return BadRequest(result);

                case ResponseStatus.NotFound:
                    return NotFound(result);

                case ResponseStatus.DatabaseError:
                case ResponseStatus.Exception:
                    return StatusCode(500, result);

                default:
                    return StatusCode(500, result);
            }
        }
        #endregion

        #region[- GetById() -] 
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(GetOrderHeaderDto getOrderHeaderDto)
        {
            var result = await _orderService.GetByIdAsync(getOrderHeaderDto);

            if (result.IsSuccess)
                return Ok(result);

            if (result.Status == ResponseStatus.NotFound)
                return NotFound(result);

            return StatusCode(500, result);
        }
        #endregion

        #region [- DeletePAsync() -]
        [HttpDelete("DeleteAsync")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteOrderHeaderDto deleteOrderHeaderDto)
        {
          
            var result =await _orderService.DeleteAsync(deleteOrderHeaderDto);


            if (result.IsSuccess)
                return Ok(result);

            if (result.Status == ResponseStatus.NotFound)
                return NotFound(result);

            return StatusCode(500, result);
        }
        #endregion

    }
}
