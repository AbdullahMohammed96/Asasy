using AAITHelper.Enums;
using AAITHelper;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.CouponDto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Api.Contract.Copon;
using Asasy.Service.Api.Contract.Logic;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "OrderClient")]
    [ApiController]
    public class OrderClientController : ControllerBase
    {
        private readonly IOrderClient _OrderClient;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICouponService _couponService;

        public OrderClientController(ICurrentUserService currentUserService, IOrderClient OrderClient, ICouponService couponService)
        {
            _currentUserService = currentUserService;
            _couponService = couponService;
        }
        /// <summary>
        /// List current Order For user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// [
        ///   {
        ///     "orderId": 7,
        ///     "orderType": "tvkin",
        ///     "name": "Api@aait.sa",
        ///     "img": "https://localhost:44306/images/Category/d5e17d36d2db4c53af3662bcd23aef4d.jpg",
        ///     "location": "test",
        ///     "stutes": "تم الموافقه من قبل العميل ع العرض",
        ///     "date": "08/11/2021 09:21"
        ///   }
        /// ]
        /// </remarks>
        /// <param lang="lang">ar or en</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.OrderApi.ListcurrentOrderForUser)]
        public async Task<IActionResult> ListcurrentOrderForUser(string lang = "ar")
        {
            try
            {
                return Ok(await _OrderClient.ListcurrentOrderForUser(_currentUserService.UserId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
        /// <summary>
        /// List new Order For user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// [
        ///   {
        ///     "orderId": 7,
        ///     "orderType": "tvkin",
        ///     "name": "Api@aait.sa",
        ///     "img": "https://localhost:44306/images/Category/d5e17d36d2db4c53af3662bcd23aef4d.jpg",
        ///     "location": "test",
        ///     "stutes": "تم الموافقه من قبل العميل ع العرض",
        ///     "date": "08/11/2021 09:21"
        ///   }
        /// ]
        /// </remarks>
        /// <param lang="lang">ar or en</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.OrderApi.ListNewOrderForUser)]
        public async Task<IActionResult> ListNewOrderForUser(string lang = "ar")
        {
            try
            {
                return Ok(await _OrderClient.ListNewOrderForUser(_currentUserService.UserId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
        /// <summary>
        /// List Ended Order For user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// [
        ///   {
        ///     "orderId": 7,
        ///     "orderType": "tvkin",
        ///     "name": "Api@aait.sa",
        ///     "img": "https://localhost:44306/images/Category/d5e17d36d2db4c53af3662bcd23aef4d.jpg",
        ///     "location": "test",
        ///     "stutes": "تم الموافقه من قبل العميل ع العرض",
        ///     "date": "08/11/2021 09:21"
        ///   }
        /// ]
        /// </remarks>
        /// <param lang="lang">ar or en</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.OrderApi.ListEndedOrderForUser)]
        public async Task<IActionResult> ListEndedOrderForUser(string lang = "ar")
        {
            try
            {
                return Ok(await _OrderClient.ListEndedOrderForUser(_currentUserService.UserId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
        /// <summary>
        /// Get Order Info For User
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// {
        ///   "orderId": 7,
        ///   "userName": "amr",
        ///   "providerName": "Api@aait.sa",
        ///   "date": "08/11/2021 09:21",
        ///   "cityName": "المنصوره",
        ///   "imgs": [
        ///     "\thttps://localhost:44306/images/Category/7660df3475a1487c8658c8bc5e057bba.jpg"
        ///   ],
        ///   "info": "غلات",
        ///   "stutes": 2,
        ///   "price": 100
        /// } 
        /// </remarks>
        /// <param lang="lang">ar or en</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [HttpGet(ApiRoutes.OrderApi.GetOrderInfoForUser)]
        public async Task<IActionResult> GetOrderInfoForUser(int orderId, string lang = "ar")
        {
            try
            {
                return Ok(await _OrderClient.GetOrderInfo(orderId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }


        /// <summary>
        /// Use Coupon 
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///   {
        ///    lastTotal:100
        ///  }
        /// </remarks>
        /// <param couponCode="UseCouponDto.couponCode">ar or en</param>
        /// <param couponId="UseCouponDto.couponId"></param>
        /// <param userId="UseCouponDto.userId"></param>
        /// <param total="UseCouponDto.total"></param>
        /// <param lang="UseCouponDto.lang"></param>
        /// <returns>return last Total </returns>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>
        [AllowAnonymous]
        [HttpPost(ApiRoutes.OrderApi.UseCopon)]
        public async Task<IActionResult> UseCoupon([FromForm] UseCouponDto useCouponDto)
        {
            try
            {


                // check coponUse
                int CoponId = await _couponService.GetCoponId(useCouponDto.couponCode);

                if (CoponId != 0)
                {
                    bool used = await _couponService.CheckCouponUsage(CoponId, _currentUserService.UserId);
                    if (used)
                    {
                        return BadRequest(new { msg = HelperMsg.creatMessage(useCouponDto.lang, "تم استخدام الكوبون من قبل", "Copon is used befor") });
                    }

                }


                var isExist = await _couponService.CheckCouponExist(useCouponDto.couponCode);
                if (!isExist)
                {
                    return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.SomethingWrong.ToNumber(), useCouponDto.lang) });
                }

                bool isExpired = await _couponService.CheckExpiredCoupon(useCouponDto.couponCode);
                if (isExpired)
                {
                    return BadRequest(new { msg = HelperMsg.creatMessage(useCouponDto.lang, "عذرا لقد انتهت مده صلاحيه الكوبون", "Sorry, the validity of the coupon has expired") });
                }

                bool isMaxUsage = await _couponService.CheckCouponMaxUse(useCouponDto.couponCode);
                if (isMaxUsage)
                {
                    return BadRequest(new { msg = HelperMsg.creatMessage(useCouponDto.lang, "عذرا تم تجاوز الحد الاقصى لاستخدام الكوبون", "Sorry, the maximum use of the coupon has been exceeded") });
                }

                bool isUsedBefore = await _couponService.CheckCouponUsage(useCouponDto.couponId, useCouponDto.userId);
                if (isUsedBefore)
                {
                    return BadRequest(new { msg = HelperMsg.creatMessage(useCouponDto.lang, "تم استخدام الكوبون من قبل", "The copon has already been used") });
                }

                var lastTotal = await _couponService.GetLastTotalForUsingCoupon(useCouponDto.couponCode, useCouponDto.total);

                return Ok(new { lastTotal = lastTotal });

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }

    }
}
