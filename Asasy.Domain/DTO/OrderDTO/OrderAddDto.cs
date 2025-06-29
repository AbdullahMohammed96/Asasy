using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Asasy.Domain.DTO.OrderDTO
{
    public class OrderAddDto
    {
        public int servId { get; set; }
        public string userId { get; set; }
        public string providerId { get; set; }
        public DateTime date { get; set; }
        public int cityId { get; set; }
        public List<IFormFile> img { get; set; }
        public string info { get; set; }
    }
}
