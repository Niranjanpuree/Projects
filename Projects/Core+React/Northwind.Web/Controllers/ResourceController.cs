using System;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;

namespace Northwind.Web.Controllers
{
    public class ResourceController : Controller
    {
        private readonly IResourceService _resourceService;

        public ResourceController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get()
        {
            return Json(_resourceService.GetAll());

        }

        public IActionResult GetActions(string resourceId)
        {

            if (Guid.TryParse(resourceId, out Guid guid))
            {
                return Json(_resourceService.GetResourceAction(Guid.Parse(resourceId)));
            }
            else
            {
                throw new Exception("Invalid Input for GetActions");
            }

        }
    }
}