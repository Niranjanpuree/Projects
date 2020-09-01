using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IManagerUserService _managerUserService;
        private readonly IConfiguration _configuration;

        public ProfileController(
            IUserService userService,
            IManagerUserService managerUserService,
            IConfiguration configuration)
        {
            _userService = userService;
            _managerUserService = managerUserService;
            _configuration = configuration;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {

                IEnumerable<ManagerUser> managerUsers = new List<ManagerUser>();
                List<string> managerNames = new List<string>();
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                var user = _userService.GetUserByUserGuid(loggedUser);

                var userViewModel = Models.ObjectMapper<User, UserViewModel>.Map(user);

                managerUsers = _managerUserService.GetManagerUser(loggedUser);

                foreach (var manager in managerUsers)
                {
                    var userManager = _userService.GetUserByUserGuid(manager.ManagerGUID);
                    var fullName = Infrastructure.Helpers.FormatHelper.FormatFullName(userManager.Firstname, string.Empty, userManager.Lastname);
                    managerNames.Add(fullName);
                }

                userViewModel.Manager = string.Join(" , ", managerNames.OrderBy(x => x));

                return View(userViewModel);

            }
            catch
            {
            }

            return View();
        }
    }
}
