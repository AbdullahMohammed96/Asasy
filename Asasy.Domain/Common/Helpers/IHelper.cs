using Asasy.Domain.Common.Helpers.DataTablePaginationServer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Domain.Common.Helpers
{
    public interface IHelper
    {
        public string Upload(IFormFile Photo, int FileName);
        public string GetRole(string role, string lang);
        public string CreatePDF(string controllerAction, int id);
    }
}
