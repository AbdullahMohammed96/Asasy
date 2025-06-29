using Asasy.Domain.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Logic
{
    public interface IOrderProvider
    {
        Task<List<OrderListDto>> ListNewOrderForProvider(string providerId, string lang);
        Task<List<OrderListDto>> ListcurrentOrderForProvider(string providerId, string lang);
        Task<List<OrderListDto>> ListEndedOrderForProvider(string providerId, string lang);
        Task<OrderInfoDto> GetOrderInfo(int orderId, string lang);
    }
}
