using AAITHelper;
using AAITHelper.Enums;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO;
using Asasy.Domain.DTO.More;
using Asasy.Domain.Enums;
using Asasy.Service.Api.Implementation.FilterValidation;
using Asasy.Service.Contract.More;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asasy.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(GroupName = "MoreAPI")]
    [PermessionFilterAttribute]
    [ApiController]
    public class MoreController : ControllerBase
    {
        private readonly IMoreService _moreService;

        public MoreController(IMoreService moreService)
        {
            _moreService = moreService;
        }



        /// <summary>
        /// Get Wallet for user
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.GetWallet)]
        public async Task<IActionResult> GetWallet()
        {
            try
            {
                var result = await _moreService.GetWallet();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }
        /// <summary>
        /// Calculate Commission App
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.CalculateCommissionApp)]
        [AllowAnonymous]
        public async Task<IActionResult> GetWallet(double value)
        {
            try
            {
                var result = await _moreService.CalculateCommissionApp(value);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Pay App Commission By Wallet
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.More.PayAppCommissionByWallet)]
        public async Task<IActionResult> PayAppCommissionByWallet(double value)
        {
            try
            {
                var check = await _moreService.PayAppCommissionByWallet(value);
                if (check.IsSuccess)
                {
                    return Ok(check);
                }
                else
                {
                    return BadRequest(check);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Get Policy
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.GetPolicy)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPolicy()
        {
            try
            {
                var result = await _moreService.Policy();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Get Discount System
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.GetDiscountSystem)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDiscountSystem()
        {
            try
            {
                var result = await _moreService.DiscountSystem();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Get Terms And Conditions
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.GetTermsAndConditions)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTermsAndConditions()
        {
            try
            {
                var result = await _moreService.TermsAndConditions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Get Contact With Us
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<ContactUsInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.GetContactWithUs)]
        [AllowAnonymous]
        public async Task<IActionResult> GetContactWithUs()
        {
            try
            {
                var result = await _moreService.ContactWithUs();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }


        /// <summary>
        /// Get Prohibited List
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<List<ProhibitedGoodsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.GetProhibitedList)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProhibitedList()
        {
            try
            {
                var result = await _moreService.ProhibitedGoods();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Get Prohibited List
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.Footer)]
        [AllowAnonymous]
        public async Task<IActionResult> Footer()
        {
            try
            {
                var result = await _moreService.Footer();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }
        /// <summary>
        /// Add New Complaint
        /// </summary>
        [ProducesResponseType(typeof(BaseResponseDto<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost(ApiRoutes.More.AddNewComplaint)]
        public async Task<IActionResult> AddNewComplaint([FromForm] AddNewComplaintsDto model)
        {
            try
            {
                var check = await _moreService.AddNewComplaint(model);
                if (check.IsSuccess)
                {

                    return Ok(check);
                }
                else
                {
                    return BadRequest(check);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Complaints List
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                      TypeComplaint
        ///                  {
        ///                      Waiting=1,
        ///                      Finished=2
        ///                  }
        ///       
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<List<ComplaintsListDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.ComplaintsList)]
        public async Task<IActionResult> ComplaintsList(int type = 1)
        {
            try
            {
                var result = await _moreService.ComplaintsList(type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }

        /// <summary>
        /// Get Complaint Info
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        /// 
        /// 
        ///                      TypeComplaint
        ///                  {
        ///                      Waiting=1,
        ///                      Finished=2
        ///                  }
        ///       
        /// </remarks>
        [ProducesResponseType(typeof(BaseResponseDto<ComplaintsDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet(ApiRoutes.More.ComplaintInfo)]
        public async Task<IActionResult> ComplaintInfo(int complaintId)
        {
            try
            {
                var result = await _moreService.ComplaintInfo(complaintId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { msg = HelperMsg.MsgValidation(EnumValidMsg.Auth.CheckData.ToNumber()) });
            }
        }
    }
}
