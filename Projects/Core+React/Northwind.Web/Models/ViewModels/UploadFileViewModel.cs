using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class UploadFileViewModel
    {
        public Guid ParentGuid { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
