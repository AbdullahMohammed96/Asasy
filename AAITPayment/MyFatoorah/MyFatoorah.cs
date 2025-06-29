using AAITPayment.HyperPay.Model;
using AAITPayment.MyFatoorah.Model;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah
{
    public class MyFatoorah : IMyFatoorah
    {
        public MyFatoorahCallBackUrlConfig MyFatoorahCallBackUrlConfig { get; set; }
        //// live
        //token value to be placed here from merchant account;
        //public string LiveToken { get; set; } = "_2uuhCDSzEy4pS-mov5PgqGOSITSxb8rjU7pF8cd0m53n23z7kUP1Nj7Tdqp4IFwuLBSvyJAw6ZllnrQn4NSASyZglyROpqgzXVCJ_51xVGGZBZ5WpqkcSavMVCGwTHgkXE41Ao7dTMVGpHfwnKJEIMXt7GE-gL0xlcogVbcnlpVY7h1j2bMvAtUYbXVTmzp3hVJHZmID3mj2PZipwc04o_6kTAsQnWsctJQscT670rGGOGfn7qNyllnHeU8B2ITdjjbsgjYDzE_pl7SfHuTjYHM6eY-0s_P-MA4G22WtiP0-I-fKtIeeI6lV52wA8C06fOoPGS0L1Mg7cFSP3gqRHNdNxIAkfrVDy4I--E-tbUK9g2O219UNgxLTktMkL_5xZ90zNtt9RBAt_89lbFj3XSB-ZIDO1QnIspAVVO_DrM5qt1aXYMLL0Rz42Dhg6EYU0uVcRtgVvMxN8XM59VOe_Hc8dkCHR8YpfMTOZxJ1734B-EAUhLh5qKlIpPnxgWgVLhGkL0YsKI4DF9dJaELLAAKGmuRsN117MwLLK8LbLnYmSeDIC5moItXtbLCyV5yw-a0kkurgpHCL5t37tTR8zL9g8MxdeUxQQFHtKa2UQUz94FGawySPzzV9NZ6cCGHDXoAN7S2x4F7C1DQQchJi5FnsNeuVTg5oEq6SA95AI1cjgHLw5-qcshuQ7Z5B4mEFXZ66uIJx6tvgGZY7yi_0jk4iglvkCktrJOncfcx6V3ViNhV6vwB0sx73q8Hv9ezT6_AVw";
        public string LiveToken { get; set; } = "sLBTEfd1XiFkwfr0qsxCu0qYvlNm96iTzq0rhvpU0me-4VReWcI9cvQyN8lbmUHz0jFQAB1st4NLAsfVJHoql4zfWpCy7aPQHveTVBrGh2dAqFpTYDfR8GLvahyWQvyGC7dfUmBQRaGFdQa9PO9M1FRF02kCIaFFObY9vdkrbVOmnBVpBhDsnWhp98UQ07wD7SNO6VszSpvF90PhThg2bq8UP2tpFPnCwQKnAxDpQJyhtpdUARkb7QpukBW5E7fMm2CklNfsWf8vAc92BFMF3tuedjl2gu-D2Ca1o0QWsJFnrj5I8WuvIJ6QTkpFH5m4sGY3RKXNyWc0vTrL7mtJa6NLLRoUSPfLP9sQkenKAyiEQ_GsEox32AMdVruKxBSM9jUmITU89TE6Cs0db_xfyogAMGWfwYByOf-K3QZT5BmyBLeBbXSjli2IVcmB0aMEowYpi2uQiGvXO2cYCRtwMJcTR4nCHzeNwbPpD2p48YDrl4uZzvJS1CUt9rKn0z0GgK_GeDQd2jl0Q7K62bmLGW9RIUlwurPhrmovPVdi61_A10BMx-7I__GMZSTSdNyvq-RueG_69BTMfwBRl6qF4oD4XQgGIf48M2zNsZPbOV4QBAi90nVPVhhCni9gmyzuLqTFqOHglKdX7hUqOS66X6GBJk40eAZEOzzz9pJiGJimVrVl";
        // test
        public string TestToken { get; set; } = "-Nb3Hjtgk-cHZFXFn3ukd9-CMGmYN2oFzjg4W0WNK4K7aYY20T0vyZWdGBHO-FrW-HSp4UCL0sqtDKK5NEX5dMq0KeIy2ySXBIagzj2wjVlRPrX1QFtBwAl3O-S2T4AX4ll-uyrsuvrHaYScN65txmh0FiW9bkYspO4H2Bpkkupv2mdG-BRvDeP8Lhdir0gPz65Jez8SAHKY8raSK2mTFK5CP7hUjdmpb_Iqz6yESpy4UV36Aj52LSleCPxlE3ySTAsp0kvAVGj-EqFzdp0BrAO-YK5AnEpr7_hbPwdTi1nisRwNBZYmjUXfCW2c_Yqzh1DHLEyxB4lD5d5LHziuNsw1du_WfhoXjJprf1-NriBh9nbtF8dvwXJR0GKk0_SSGdjwQ64Yz0mkt4kWkRekg2bbX7znq4QupQoMf7QMtGwkT5lBaE2SPhaUz78degNtSoLvxb_kUFxxOKGl1o1B37rsI7zIFpgrETEq-XQBTwCMyWfbUu36klAqQpIR3upKTxcaGsLGV7tvKSVifteEGy_qi1snpuOOe0IwQVntlj7U8JhlZQ36JjHMINmALPDA2YIqjm0cHt0YH9JQbwQvqjIPlVSAuP4C7iwI2lOtIBEVg7Rb4gsphBcX2afuxZWvIHiMztKk0jJDWZHXLM-Y-SrnyOv-AOkiQSCm2OeSNChoA5okIRrVA0UtfnV45d5PaCb3VA";
        public string liveBaseApiURL { get; set; } = "https://api-sa.myfatoorah.com/"; // ksa live
        public string testBaseApiURL { get; set; } = "https://apitest.myfatoorah.com/"; // test
        public bool IsLive { get; set; } = true;


        public MyFatoorah()
        {

        }

        public MyFatoorah(MyFatoorahCallBackUrlConfig myFatoorahCallBackUrlConfig, RouteValues routeValues)
        {
            MyFatoorahCallBackUrlConfig = myFatoorahCallBackUrlConfig;
            //myFatoorahCallBackUrlConfig.QueryParamsRouteValues = $"{nameof(routeValues.PackageId)}={routeValues.PackageId}&{nameof(routeValues.UserId)}={routeValues.UserId}&{nameof(routeValues.Type)}={routeValues.Type}&{nameof(routeValues.adsId)}={routeValues.adsId}&{nameof(routeValues.userSliderId)}={routeValues.userSliderId}";
            myFatoorahCallBackUrlConfig.QueryParamsRouteValues = $"{nameof(routeValues.UserId)}={routeValues.UserId}&{nameof(routeValues.Type)}={routeValues.Type}&{nameof(routeValues.adsId)}={routeValues.adsId}&{nameof(routeValues.packageId)}={routeValues.packageId}&{nameof(routeValues.userPackageId)}={routeValues.userPackageId}";
        }



        public async Task<List<PaymentMethod>> InitiatePayment(string amount)
        {
            //####### Initiate Payment ######
            InitiatePayment intiatePaymentRequest = new InitiatePayment()
            {
                InvoiceAmount = amount,
                CurrencyIso = IsLive ? Currency.SAR : Currency.KWD
            };

            string intitateRequestJSON = JsonConvert.SerializeObject(intiatePaymentRequest);
            string InitiatePaymentResponse = await PerformRequest(intitateRequestJSON, "InitiatePayment").ConfigureAwait(false);

            InitiateResponse initiateResponse = JsonConvert.DeserializeObject<InitiateResponse>(InitiatePaymentResponse);

            if (initiateResponse.IsSuccess)
            {
                return initiateResponse.Data.PaymentMethods;
            }

            return new List<PaymentMethod>();
        }


       
        public async Task<ExecutePaymentResponse> ExecutePayment(ExecutePaymentModel executePaymentModel)
        {
            var executeRequestJSON = JsonConvert.SerializeObject(executePaymentModel);
            var ExecutePaymentResponse = await PerformRequest(executeRequestJSON, "ExecutePayment").ConfigureAwait(false);

            ExecutePaymentResponse ExecuteResponse = JsonConvert.DeserializeObject<ExecutePaymentResponse>(ExecutePaymentResponse);

            if (ExecuteResponse.IsSuccess)
            {
                return ExecuteResponse;
            }

            return new ExecutePaymentResponse();
        }



        public async Task<string> GetPaymentStatus(string key, string keyType)
        {
            var GetPaymentStatusRequest = new
            {
                Key = key,
                KeyType = keyType
            };

            var GetPaymentStatusRequestJSON = JsonConvert.SerializeObject(GetPaymentStatusRequest);
            return await PerformRequest(GetPaymentStatusRequestJSON, "GetPaymentStatus").ConfigureAwait(false);
        }




        public async Task<string> PerformRequest(string requestJSON, string endPoint)
        {
            string apiUrl = IsLive ? liveBaseApiURL : testBaseApiURL;
            var token = IsLive ? LiveToken : TestToken;
            string url = apiUrl + $"/v2/{endPoint}";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpContent = new StringContent(requestJSON, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync(url, httpContent).ConfigureAwait(false);
            string response = string.Empty;
            if (!responseMessage.IsSuccessStatusCode)
            {
                response = JsonConvert.SerializeObject(new
                {
                    IsSuccess = false,
                    Message = responseMessage.StatusCode.ToString()
                });
            }
            else
            {
                response = await responseMessage.Content.ReadAsStringAsync();
            }

            return response;
        }
    }
}
