using Microsoft.AspNetCore.Http;

namespace Asasy.Domain.DTO.SocialMediaDTO
{
    public class SocialMediaAddDto
    {
        public string name { get; set; }
        public IFormFile img { get; set; }
        public string url { get; set; }
    }
}
