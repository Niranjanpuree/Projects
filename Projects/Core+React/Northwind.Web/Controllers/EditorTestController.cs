﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Web.Controllers
{
    public class EditorTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}