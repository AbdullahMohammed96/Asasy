using Asasy.Service.Api.Contract.Auth;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Asasy.Service.Api.Implementation.Auth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string UserId => _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    }
}
