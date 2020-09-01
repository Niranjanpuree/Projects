using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class EmployeeDirectoryController : Controller
    {

        private readonly IUserService _userService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IManagerUserService _managerUserService;
        private readonly IGroupUserService _groupUserService;
        private readonly IGroupService _groupService;

        public EmployeeDirectoryController(IUserService userService,
            IManagerUserService managerUserService,
            IResourceAttributeValueService resourceAttributeValueService,
            IGroupService groupService,
            IGroupUserService groupUserService
            )
        {
            _userService = userService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _managerUserService = managerUserService;
            _groupUserService = groupUserService;
            _groupService = groupService;
        }
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        private dynamic GetExportableData(IList<UserViewModel> userList)
        {
            var data = userList.Select(m => new{
                Name = m.Name,
                Username = m.Username,
                DisplayName = m.Displayname,
                LastName = m.Lastname,
                FirstName = m.Firstname,
                GivenName = m.Givenname,
                UserStatus = m.UserStatus,
                WorkEmail = m.WorkEmail,
                PersonalEmail = m.PersonalEmail,
                WorkPhone = m.WorkPhone,
                HomePhone = m.HomePhone,
                JobTitle = m.JobTitle,
                Company = m.Company,
                Department = m.Department,
                Extension = m.Extension,
                JobStatus = m.JobStatus,
                Manager = m.Manager,
                Group = m.Group
            });
            return data;
        }

        public IActionResult Get(string searchValue, int take, int skip, string sortField, string dir, string filterBy,bool isExport)
        {
            try
            {
                var users = _userService.GetEmployeeDirectoryList(searchValue, take, skip, sortField, dir, filterBy);
                var total = _userService.GetEmployeeDirectoryCount(searchValue, filterBy);
                var nUsers = new List<UserViewModel>();
                foreach (var user in users)
                {
                    var jobStatus = user.JobStatus;
                    UserViewModel userViewModel = Mapper<User, UserViewModel>.Map(user);
                    var u = ValidateUserDetails(userViewModel);
                    nUsers.Add(u);
                }

                if (isExport)
                    return Ok(new { data = nUsers, count = total });
                return Ok(new
                {
                    data = nUsers,
                    count = total
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            try
            {
                var user = _userService.GetEmployeeDirectoryByUserGuid(id);
                var mappingUserViewModel = Mapper<User, UserViewModel>.Map(user);
                var userViewModel = ValidateUserDetails(mappingUserViewModel);
                return Ok(new
                {
                    result = userViewModel
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        public UserViewModel ValidateUserDetails(UserViewModel u)
        {
            var displayName = string.IsNullOrEmpty(u.Displayname) ? u.Lastname + " " + u.Firstname : u.Displayname;
            u.Displayname = displayName;
            if (string.IsNullOrEmpty(u.MobilePhone))
                u.MobilePhone = "N/A";
            if (string.IsNullOrEmpty(u.WorkPhone))
                u.WorkPhone = "N/A";
            if (string.IsNullOrEmpty(u.Department))
                u.Department = "N/A";
            if (string.IsNullOrEmpty(u.Company))
                u.Company = "N/A";
            if (string.IsNullOrEmpty(u.JobTitle))
                u.JobTitle = "N/A";
            if (string.IsNullOrEmpty(u.PersonalEmail))
                u.PersonalEmail = "N/A";
            if (string.IsNullOrEmpty(u.Givenname))
                u.Givenname = "N/A";
            if (string.IsNullOrEmpty(u.Manager))
                u.Manager = "N/A";
            if (string.IsNullOrEmpty(u.Group))
                u.Group = "N/A";
            if (string.IsNullOrWhiteSpace(u.HomePhone))
                u.HomePhone = "N/A";
            if (string.IsNullOrWhiteSpace(u.Extension))
                u.Extension = "N/A";
            return u;
        }
    }
}