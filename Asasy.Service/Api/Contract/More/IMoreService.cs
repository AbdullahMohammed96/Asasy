using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Asasy.Domain.DTO;
using Asasy.Domain.DTO.More;

namespace Asasy.Service.Contract.More
{
    public interface IMoreService
    {
        Task<AboutUsDto> AboutUs(string lang = "ar");
        Task<List<CommonQuestionsDto>> CommonQuestions(string lang = "ar");
        Task<TermsAndConditionsDto> TermsAndConditions(string lang = "ar");
        Task<bool> ContactWithUs(ContactWithUsDto contactWithUs);


        // new 
        Task<BaseResponseDto<string>> GetWallet();
        Task<BaseResponseDto<string>> CalculateCommissionApp(double value);
        Task<BaseResponseDto<string>> PayAppCommissionByWallet(double value);
        Task<BaseResponseDto<string>> Policy ();
        Task<BaseResponseDto<string>> DiscountSystem();
        Task<BaseResponseDto<string>> TermsAndConditions();
        Task<BaseResponseDto<ContactUsInfoDto>> ContactWithUs();
        Task<BaseResponseDto<List<ProhibitedGoodsListDto>>> ProhibitedGoods();
        Task<BaseResponseDto<string>> AddNewComplaint(AddNewComplaintsDto model);
        Task<BaseResponseDto<List<ComplaintsListDto>>> ComplaintsList(int type);
        Task<BaseResponseDto<ComplaintsDetailsDto>> ComplaintInfo(int complaintId);
        Task<BaseResponseDto<string>> Footer();

    }
}
