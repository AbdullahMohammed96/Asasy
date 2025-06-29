using Asasy.Domain.DTO.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Contract.Logic
{
    public interface IOrderClient
    {
        Task<List<OrderListDto>> ListNewOrderForUser(string userId, string lang);
        Task<List<OrderListDto>> ListcurrentOrderForUser(string userId, string lang);
        Task<List<OrderListDto>> ListEndedOrderForUser(string userId, string lang);
        Task<OrderInfoDto> GetOrderInfo(int orderId, string lang);
    }
}
