namespace Asasy.Domain.Common.PathUrl
{
    public class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;


        public static class OrderApi
        {
            public const string ListcurrentOrderForProvider = Base + "/ListcurrentOrderForProvider";
            public const string ListAdsNewOrderForProvider = Base + "/ListAdsNewOrderForProvider";
            public const string ListEndedOrderForProvider = Base + "/ListEndedOrderForProvider";
            public const string GetOrderInfoForProvider = Base + "/GetOrderInfoForProvider";

            public const string ListcurrentOrderForUser = Base + "/ListcurrentOrderForUser";
            public const string ListNewOrderForUser = Base + "/ListNewOrderForUser";
            public const string ListEndedOrderForUser = Base + "/ListEndedOrderForUser";
            public const string GetOrderInfoForUser = Base + "/GetOrderInfoForUser";

            public const string UseCopon = Base + "/UseCopon";

        }

        public static class Identity
        {

            public const string GetDataOfProvider = Base + "/GetDataOfProvider";
            public const string UpdateAsyncDataDelegt = Base + "/UpdateAsyncDataDelegt";
            public const string RegisterClient = Base + "/RegisterClient";
            public const string RegisterDeleget = Base + "/RegisterDeleget";
            public const string RemoveNotiyByIdAsync = Base + "/RemoveNotiyByIdAsync";
            public const string RemoveAllNotify = Base + "/RemoveAllNotify";
            public const string UpdateForWebsiteAsyncDataUser = Base + "/UpdateForWebsiteAsyncDataUser";
            public const string RemoveAccount = Base + "/RemoveAccount";
            // client
            public const string ResendCode = Base + "/ResendCode";
            public const string ListOfNotifyUser = Base + "/ListOfNotifyUser";
            public const string CountNotifactions = Base + "/CountNotifactions";
            public const string RemoveNotify = Base + "/RemoveNotify";
            public const string ChangeReciveOrder = Base + "/ChangeReciveOrder";
            public const string UpdateDataUser = Base + "/UpdateDataUser";
            public const string ChangePasswordByCode = Base + "/ChangePasswordByCode";
            public const string Login = Base + "/login";
            public const string ForgetPassword = Base + "/ForgetPassword";
            public const string Register = Base + "/register";
            public const string ListCity = Base + "/ListCity";
            public const string ConfirmCodeRegister = Base + "/ConfirmCodeRegister";
            public const string reset_password = Base + "/reset_password";
            public const string ChangePassward = Base + "/ChangePassward";
            public const string GetDataOfUser = Base + "/GetDataOfUser";
            public const string ConvertloyaltytoWallet = Base + "/ConvertloyaltytoWallet";

            // addtional services from user 
            public const string ChangeLanguage = Base + "/ChangeLanguage";
            public const string ChangeNotify = Base + "/ChangeNotify";
            public const string Logout = Base + "/Logout";
            public const string VerifyName = Base + "/VerifyName";


            //new
            public const string SendCodeToUpdate = Base + "/SendCodeToUpdate";
            public const string CheckCodeIsCorrectOrNot = Base + "/CheckCodeIsCorrectOrNot";
            public const string ChangePhoneNumberByCode = Base + "/ChangePhoneNumberByCode";
            public const string ChangeAppearanceStatus = Base + "/ChangeAppearanceStatus";
            public const string TestSendSMS = Base + "/TestSendSMS";

        }
        public static class ClientLogic
        {
            public const string AddNewAds = Base + "/AddNewAds";
            public const string SubscribeInPackageByWallet = Base + "/SubscribeInPackageByWallet";
            public const string ChargingWallet = Base + "/ChargingWallet";
            public const string Slider = Base + "/Slider";
            public const string Home = Base + "/Home";
            public const string FiltrationAds = Base + "/FiltrationAds";
            public const string AdsByLocation = Base + "/AdsByLocation";
            public const string ListOfAdsByCategoryAndSubCategoryAndTitle = Base + "/ListOfAdsByCategoryAndSubCategoryAndTitle";
            public const string AdDetails = Base + "/AdDetails";
            public const string AddNewCommentForAd = Base + "/AddNewCommentForAd";
            public const string NewReplayForComment = Base + "/NewReplayForComment";
            public const string AllCommentsForAds = Base + "/AllCommentsForAds";
            public const string AddAdToFavorite = Base + "/AddAdToFavorite";
            public const string RemoveAdFromFavorite = Base + "/RemoveAdFromFavorite";
            public const string FavoriteList = Base + "/FavoriteList";
            public const string ReportAd = Base + "/ReportAd";
            public const string ReportProvider = Base + "/ReportProvider";
            public const string FollowAd = Base + "/FollowAd";
            public const string FollowProvider = Base + "/FollowProvider";
            public const string UnFollowAd = Base + "/UnFollowAd";
            public const string UnFollowProvider = Base + "/UnFollowProvider";
            public const string ListOfFollowsAd = Base + "/ListOfFollowsAd";
            public const string ListOfFollowsProvider = Base + "/ListOfFollowsProvider";
            public const string GetProviderInfo = Base + "/GetProviderInfo";
            public const string AddNewRateForProvider = Base + "/AddNewRateForProvider";
            public const string AllCommentsForProvider = Base + "/AllCommentsForProvider";
            public const string GetMyAds = Base + "/GetMyAds";
            public const string GetMyPackages = Base + "/GetMyPackages";
            public const string ResubscribeInPackage = Base + "/ResubscribeInPackage";
            public const string CheckIsPackageIsEndedOrNot = Base + "/CheckIsPackageIsEndedOrNot";
            public const string GetAdsDetailsToUpdate = Base + "/GetAdsDetailsToUpdate";
            public const string UpdateAds = Base + "/UpdateAds";
            public const string RemoveAd = Base + "/RemoveAd";

        }

        public static class Chat
        {
            public const string ListMessagesUser = Base + "/ListMessagesUser";
            public const string UploadNewFile = Base + "/UploadNewFile";
            public const string ListUsersMyChat = Base + "/ListUsersMyChat";
            public const string ListAllMessages = Base + "/ListAllMessages";
        }

        public static class Shared
        {
            public const string MaiCategories = Base + "/MaiCategories";
            public const string SubCategories = Base + "/SubCategories";
            public const string ListOfRegions = Base + "/ListOfRegions";
            public const string ListOfCitiesByRegion = Base + "/ListOfCitiesByRegion";
            public const string ListOfDistrictsByCity = Base + "/ListOfDistrictsByCity";
            public const string Packages = Base + "/Packages";
            public const string GetPackageDetails = Base + "/GetPackageDetails";
      
        }
        public static class More
        {
            public const string AboutUs = Base + "/AboutUs";
            public const string CommonQuestions = Base + "/CommonQuestions";
            public const string TermsAndConditions = Base + "/TermsAndConditions";
            public const string ContactWithus = Base + "/ContactWithUs";
            public const string GetWallet = Base + "/GetWallet";
            public const string CalculateCommissionApp = Base + "/CalculateCommissionApp";
            public const string PayAppCommissionByWallet = Base + "/PayAppCommissionByWallet";
            public const string GetPolicy = Base + "/GetPolicy";
            public const string GetDiscountSystem = Base + "/GetDiscountSystem";
            public const string GetTermsAndConditions = Base + "/GetTermsAndConditions";
            public const string GetContactWithUs = Base + "/GetContactWithUs";
            public const string GetProhibitedList = Base + "/GetProhibitedList";
            public const string Footer = Base + "/Footer";
            public const string AddNewComplaint = Base + "/AddNewComplaint";
            public const string ComplaintsList = Base + "/ComplaintsList";
            public const string ComplaintInfo = Base + "/ComplaintInfo";
        }


    }
}
