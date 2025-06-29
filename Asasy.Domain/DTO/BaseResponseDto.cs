using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.DTO
{
    public class BaseResponseDto<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        //public int Info { get; set; } = 0;
        public string Message { get; set; } = "";
        public string Error { get; set; } = "";
        public string Status { get; set; } = "";
        public List<ValidationError>? ValidationErrors { get; set; }
        public PaginationDto? PaginatioData { get; set; }
    }

    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class PaginationDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }


}
