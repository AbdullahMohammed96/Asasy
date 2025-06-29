using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.ViewModel.CategoryModel
{
    public class MainCategoryViewModel
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescEn { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public string Img { get; set; } = string.Empty;
        public IFormFile CreateImg { get; set; }
        public bool IsActive { get; set; }
        public bool ShowSideManu { get; set; }
        public int? idCat { get; set; }
        public string CatName { get; set; } = string.Empty;
        public int OrderBy { get; set; }
        public string type { get; set; } = string.Empty; //Main -- //Sub
        public int CountCategory { get; set; }
    }
}
