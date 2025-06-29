using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Asasy.Domain.DTO.AuthApiDTO
{
    public class UpdateDataUserDto
    {
        public string userName { get; set; }
        public string email { get; set; }
        public IFormFile imgProfile { get; set; }
    }
}
