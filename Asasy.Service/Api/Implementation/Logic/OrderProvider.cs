using AAITHelper.Enums;
using AutoMapper.QueryableExtensions;
using Asasy.Domain.Common.Helpers;
using Asasy.Domain.Common.PathUrl;
using Asasy.Domain.DTO.OrderDTO;
using Asasy.Domain.Enums;
using Asasy.Persistence;
using Asasy.Service.Api.Contract.Logic;
using Asasy.Service.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Implementation.Logic
{
    public class OrderProvider : IOrderProvider
    {
        private readonly ApplicationDbContext _db;

        public OrderProvider(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<OrderInfoDto> GetOrderInfo(int orderId, string lang)
        {
            OrderInfoDto GetOrderById = await _db.Orders.Where(x => x.Id == orderId).Select(o => new OrderInfoDto
            {
                date = o.DateTime.ToString("dd/MM/yyyy HH:mm"),
                imgs = o.OrderInfos.Select(i => DashBordUrl.baseUrlHost + i.Img).ToList(),
                info = o.Info,
                userName = o.User.user_Name,
                providerName = o.Provider.user_Name,
                orderId = o.Id,
                // price = o.PriceAfterDisc,
                stutes = o.Stutes

            }).AsNoTracking().SingleOrDefaultAsync();
            return GetOrderById;
        }

        public async Task<List<OrderListDto>> ListcurrentOrderForProvider(string providerId, string lang)
        {
            return await _db.Orders.Where(x => x.ProviderId == providerId && x.Stutes == OrderStutes.ClientPay.ToNumber()).AsQueryable().AsNoTracking().ProjectTo<OrderListDto>(MappingProfiles.OrdersProviderMapping(lang)).ToListAsync();
        }

        public async Task<List<OrderListDto>> ListEndedOrderForProvider(string providerId, string lang)
        {
            return await _db.Orders.Where(x => x.ProviderId == providerId && x.Stutes == OrderStutes.Finished.ToNumber()).AsQueryable().AsNoTracking().ProjectTo<OrderListDto>(MappingProfiles.OrdersProviderMapping(lang)).ToListAsync();
        }
        public async Task<List<OrderListDto>> ListNewOrderForProvider(string providerId, string lang)
        {
            return await _db.Orders.Where(x => x.ProviderId == providerId && x.Stutes <= OrderStutes.ProviderSendOffer.ToNumber()).AsQueryable().AsNoTracking().ProjectTo<OrderListDto>(MappingProfiles.OrdersProviderMapping(lang)).ToListAsync();
        }
    }
}
