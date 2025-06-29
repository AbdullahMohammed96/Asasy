namespace Asasy.Infrastructure.Extension
{
    public class AuthorizeRolesAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Asasy.Domain.Enums.Roles[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
