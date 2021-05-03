using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<CouponController> _logger;

        public CouponController(ICouponRepository couponRepository, ILogger<CouponController> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;
        }
        
        [HttpGet("{id:length(24)}", Name = "GetCoupon")]
        public ActionResult<Coupon> FindById(string id)
        {
            var coupon = _couponRepository.FindById(id);

            if (coupon == null)
            {
                return NotFound();
            }

            return coupon;
        }
        
        [HttpPost]
        public ActionResult<Coupon> Create([FromBody] Coupon coupon)
        {
            try
            {
                var result = _couponRepository.Create(coupon);

                return CreatedAtRoute("GetCoupon", new {id = result.Id}, result);
            }
            catch (InvalidInputException ex)
            {
                _logger.LogError($"Failed to create new coupon:\n{ex}");

                return BadRequest();
            }
        }
        
        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteCoupon(string id)
        {
            _couponRepository.Remove(id);

            return NoContent();
        }
    }
    
}