using Asasy.Domain.Common.PathUrl;
using Asasy.Service.Api.Contract.Auth;
using Asasy.Service.Api.Contract.Logic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "OrderProvider")]
    [ApiController]
    public class OrderProviderController : ControllerBase
    {
        private readonly IOrderProvider _OrderProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountService _accountService;
        public OrderProviderController( ICurrentUserService currentUserService, IAccountService accountService, IOrderProvider OrderProvider)
        {
            _accountService = accountService;
            _OrderProvider = OrderProvider;
            _currentUserService = currentUserService;
        }
        /// <summary>
        /// List current Order For Provider
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
        [HttpGet(ApiRoutes.OrderApi.ListcurrentOrderForProvider)]
        public async Task<IActionResult> ListcurrentOrderForProvider(string lang = "ar")
        {
            try
            {
                return Ok(await _OrderProvider.ListcurrentOrderForProvider(_currentUserService.UserId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
        /// <summary>
        /// List New Order For Provider
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// "order": [
        ///   {
        ///     "orderId": 7,
        ///     "orderType": "tvkin",
        ///     "name": "Api@aait.sa",
        ///     "img": "https://localhost:44306/images/Category/d5e17d36d2db4c53af3662bcd23aef4d.jpg",
        ///     "location": "test",
        ///     "stutes": "تم الموافقه من قبل العميل ع العرض",
        ///     "date": "08/11/2021 09:21"
        ///   }
        /// ],
        ///   "ads": [
        ///  "id": 1,
        ///  "img": "images/Slider/014f9599d6104ea58221bf79554a157c.jpeg"
        ///}
        /// ]
        /// </remarks>
        /// <param lang="lang">ar or en</param>
        /// <response code="200">Sucess</response>
        /// <response code="400">BadRequest Or Exption</response>
        /// <response code="500">internal server error</response>

        [HttpGet(ApiRoutes.OrderApi.ListAdsNewOrderForProvider)]
        public async Task<IActionResult> ListAdsNewOrderForProvider(string lang = "ar")
        {
            try
            {
                var adsAndOrder = new
                {
                    order = await _OrderProvider.ListNewOrderForProvider(_currentUserService.UserId, lang),
                    // ads = await _appProviderService.ListAds()
                };
                return Ok(adsAndOrder);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
        /// <summary>
        /// List Ended Order For Provider
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
        [HttpGet(ApiRoutes.OrderApi.ListEndedOrderForProvider)]
        public async Task<IActionResult> ListEndedOrderForProvider(string lang = "ar")
        {
            try
            {
                return Ok(await _OrderProvider.ListEndedOrderForProvider(_currentUserService.UserId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }
        /// <summary>
        /// Get Order Info For privider
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
        [HttpGet(ApiRoutes.OrderApi.GetOrderInfoForProvider)]
        public async Task<IActionResult> GetOrderInfoForProvider(int orderId, string lang = "ar")
        {

            try
            {
                return Ok(await _OrderProvider.GetOrderInfo(orderId, lang));
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { msg = ex.Message });
            }
        }


    }
}
