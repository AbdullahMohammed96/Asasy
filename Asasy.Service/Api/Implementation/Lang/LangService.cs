using Asasy.Service.Api.Contract.Lang;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.Api.Implementation.Lang
{
    public class LangService : ILangService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LangService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Lang
        {
            get
            {
                StringValues headerValues;
                var nameFilter = string.Empty;

                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("lang", out headerValues))
                {
                    nameFilter = headerValues.FirstOrDefault();
                }

                return nameFilter == "" ? "ar" : nameFilter;
            }

        }
    }
}
