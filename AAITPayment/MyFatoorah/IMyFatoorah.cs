using AAITPayment.MyFatoorah.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAITPayment.MyFatoorah
{
    public interface IMyFatoorah
    {
        public MyFatoorahCallBackUrlConfig MyFatoorahCallBackUrlConfig { get; set; }

        Task<List<PaymentMethod>> InitiatePayment(string amount);
        Task<ExecutePaymentResponse> ExecutePayment(ExecutePaymentModel executePaymentModel);
        Task<string> GetPaymentStatus(string key, string keyType);
        Task<string> PerformRequest(string requestJSON, string endPoint);

    }
}
