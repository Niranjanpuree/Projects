using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Services;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Web.Controllers
{
    public class PaystubController : Controller
    {
        private readonly IPaystubService _service;
        public PaystubController(IPaystubService service)
        {
            _service = service;
        }


        public IActionResult Get()
        {

            return Json(_service.GetByEmployeeId());
        }
    }
}