using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Infrastructure.Helpers;
using static Northwind.Core.Entities.EnumGlobal;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly IGroupPermissionService _groupPermission;

        public SettingsController(IGroupPermissionService groupPermission)
        {
            _groupPermission = groupPermission;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            GetGroupPermissions();
            return View();
        }

        #region Group Permission Admin
        private bool IsAuthorizedForResource(EnumGlobal.ResourceType resourceType, EnumGlobal.ResourceActionPermission action)
        {
            try
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                var result = _groupPermission.IsUserPermitted(userGuid, resourceType, action);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void GetGroupPermissions()
        {
            ///group permission 
            ///Admin
            ViewBag.AdminList = IsAuthorizedForResource(ResourceType.Admin, ResourceActionPermission.List);
            ViewBag.ManageCompany = IsAuthorizedForResource(ResourceType.Admin, ResourceActionPermission.ManageCompany);
            ViewBag.ManageOffice = IsAuthorizedForResource(ResourceType.Admin, ResourceActionPermission.ManageOffice);
            ViewBag.ManageRegion = IsAuthorizedForResource(ResourceType.Admin, ResourceActionPermission.ManageRegion);

            ///Contract Clause
            //ViewBag.ContractClauseList = IsAuthorizedForResource(ResourceType.ContractClauses, ResourceActionPermission.List);
            ViewBag.ManageFarClause = IsAuthorizedForResource(ResourceType.Admin, ResourceActionPermission.ManageFar);
        }
        #endregion
    }
}
