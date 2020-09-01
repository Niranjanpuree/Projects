using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Infrastructure
{
    public class QueryOperatorController : Controller
    {
        private readonly IResourceService _resourceService;
        public QueryOperatorController(IResourceService resourceService)
        {
            _resourceService = resourceService;

        }
         // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get()
        {
            return Json(_resourceService.GetQueryOperators());
        }
    }
}
