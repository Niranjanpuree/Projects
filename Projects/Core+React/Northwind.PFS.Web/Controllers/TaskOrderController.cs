using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Northwind.PFS.Web.Controllers
{
    public class TaskOrderController : Controller
    {
        IConfiguration _config;
        public TaskOrderController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize]
        public IActionResult Index()
        {
            return Redirect("~/PFS/project");
        }

        [HttpGet("/Login")]
        public IActionResult Login()
        {
            return Redirect(_config.GetValue<string>("SiteUrl") + "/Login?redir=" + _config.GetValue<string>("PFSSiteUrl"));
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            return Redirect(_config.GetValue<string>("SiteUrl") + "/Logout");
        }
    }
}