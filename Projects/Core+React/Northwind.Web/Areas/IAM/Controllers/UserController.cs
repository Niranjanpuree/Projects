using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Specifications;
using Northwind.Web.Areas.IAM.Models;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Settings.Controllers
{
    [Area("IAM")]

    public class UserController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUserService _userService;
        private readonly IResourceService _resourceService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IGroupUserService _groupUserService;
        private readonly IManagerUserService _managerUserService;
        private readonly IGroupService _groupService;

        public UserController(IHostingEnvironment hostingEnvironment,
            IUserService userService, IResourceService resourceService,
            IGroupUserService groupUserService, IManagerUserService managerUserService,
            IGroupService groupService,
            IResourceAttributeValueService resourceAttributeValueService)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _resourceService = resourceService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _groupUserService = groupUserService;
            _managerUserService = managerUserService;
            _groupService = groupService;
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Add()
        {
            var userStatuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "UserStatus");
            if (userStatuses != null && userStatuses.Count() > 0)
            {
                ViewBag.UserStatuses = userStatuses.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            var jobTitles = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobTitle");
            if (jobTitles != null && jobTitles.Count() > 0)
            {
                ViewBag.JobTitles = jobTitles.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            var jobStatuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobStatus");
            if (jobStatuses != null && jobStatuses.Count() > 0)
            {
                ViewBag.JobStatuses = jobStatuses.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            return View();
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            var userStatuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "UserStatus");
            if (userStatuses != null && userStatuses.Count() > 0)
            {
                ViewBag.UserStatuses = userStatuses.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            var jobTitles = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobTitle");
            if (jobTitles != null && jobTitles.Count() > 0)
            {
                ViewBag.JobTitles = jobTitles.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            var jobStatuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobStatus");
            if (jobStatuses != null && jobStatuses.Count() > 0)
            {
                ViewBag.JobStatuses = jobStatuses.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            try
            {
                var user = _userService.GetUserByUserGuid(id);
                if (user != null)
                {
                    var managers = _managerUserService.GetManagerUser(user.UserGuid);
                    if (managers.Count() > 0)
                    {
                        Guid managerId = managers.FirstOrDefault().ManagerGUID;
                        var manager = _userService.GetUserByUserGuid(managerId);
                        if (manager != null)
                        {
                            user.Manager = manager.Firstname + " " + manager.Lastname;
                        }

                    }

                    var groups = _groupUserService.GetGroupUserByUserGUID(user.UserGuid);
                    if (groups.Count() > 0)
                    {
                        var groupId = groups.FirstOrDefault().GroupGUID;
                        var group = _groupService.GetGroupByGUID(groupId);
                        if (group != null)
                        {
                            user.Group = group.CN;
                        }

                    }
                }
                var userViewModel = Mapper<User, UserEditViewModel>.Map(user);
                return View(userViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Details(Guid id)
        {
            var userStatuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "UserStatus");
            if (userStatuses != null && userStatuses.Count() > 0)
            {
                ViewBag.UserStatuses = userStatuses.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            var jobTitles = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobTitle");
            if (jobTitles != null && jobTitles.Count() > 0)
            {
                ViewBag.JobTitles = jobTitles.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }
            var jobStatuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobStatus");
            if (jobStatuses != null && jobStatuses.Count() > 0)
            {
                ViewBag.JobStatuses = jobStatuses.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }

            var userStatus = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "UserStatus");

            try
            {
                var user = _userService.GetUserByUserGuid(id);
                if (user != null)
                {
                    var managers = _managerUserService.GetManagerUser(user.UserGuid);
                    if (managers.Count() > 0)
                    {
                        Guid managerId = managers.FirstOrDefault().ManagerGUID;
                        var manager = _userService.GetUserByUserGuid(managerId);
                        if (manager != null)
                        {
                            user.Manager = manager.Firstname + " " + manager.Lastname;
                        }

                    }

                    var groups = _groupUserService.GetGroupUserByUserGUID(user.UserGuid);
                    if (groups.Count() > 0)
                    {
                        var groupId = groups.FirstOrDefault().GroupGUID;
                        var group = _groupService.GetGroupByGUID(groupId);
                        if (group != null)
                        {
                            user.Group = group.CN;
                        }

                    }
                }
                var userViewModel = Mapper<User, UserEditViewModel>.Map(user);
                if (userStatus != null)
                {
                    var status = userStatus.Where(c => c.Value == userViewModel.UserStatus);
                    if (status != null)
                    {
                        userViewModel.UserStatus = status.SingleOrDefault().Name;
                    }
                }
                return View(userViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        public IActionResult Detail(Guid id)
        {
            
            try
            {
                var user = _userService.GetUserByUserGuid(id);
                var userViewModel = Mapper<User, UserEditViewModel>.Map(user);
                return Ok(new { result = userViewModel });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Add([FromBody] UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.MobilePhone) && !model.MobilePhone.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid mobile number.");
                    }

                    if (!string.IsNullOrEmpty(model.WorkPhone) && !model.WorkPhone.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid work phone number.");
                    }

                    if (!string.IsNullOrEmpty(model.HomePhone) && !model.HomePhone.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid home phone number.");
                    }

                    if (!string.IsNullOrEmpty(model.PersonalEmail) && !model.PersonalEmail.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid personal email address.");
                    }

                    var existingUsers = _userService.GetUsersByUsername(new string[] { model.Username }.ToList());
                    if (existingUsers.Count() > 0)
                    {
                        throw new Exception("Username is already in use. Please choose unique.");
                    }

                    var user = Mapper<UserViewModel, User>.Map(model);
                    user.UserGuid = Guid.NewGuid();
                    var userId = _userService.Insert(user);
                    if (model.Group != "" && Guid.Parse(model.Group) != Guid.Empty)
                    {
                        _groupUserService.Insert(new GroupUser { GroupGUID = Guid.Parse(model.Group), UserGUID = userId });
                    }
                    if (model.Manager != "" && Guid.Parse(model.Manager) != Guid.Empty)
                    {
                        _managerUserService.Insert(new ManagerUser { ManagerGUID = Guid.Parse(model.Manager), UserGUID = userId });
                    }

                    user = _userService.GetUserByUserGuid(userId);
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return BadRequestFormatter.BadRequest(this, ex);
                }
            }
            else
            {
                return BadRequestFormatter.BadRequest(this, "Bad Request");
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Edit([FromBody] UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(model.MobilePhone) && !model.MobilePhone.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid mobile number.");
                    }

                    if (!string.IsNullOrEmpty(model.WorkPhone) && !model.WorkPhone.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid work phone number.");
                    }

                    if (!string.IsNullOrEmpty(model.HomePhone) && !model.HomePhone.isValidPhoneNumber())
                    {
                        throw new Exception("Please enter valid home phone number.");
                    }

                    if (!string.IsNullOrEmpty(model.PersonalEmail) && !model.PersonalEmail.isValidEmail())
                    {
                        throw new Exception("Please enter valid personal email address.");
                    }
                    var existingUsers = _userService.GetUsersByUsername(new string[] { model.Username }.ToList());
                    if (existingUsers.Count() > 0)
                    {
                        var otherUsers = existingUsers.Where(c => c.UserGuid != model.UserGuid).ToList();
                        if (otherUsers.Count() > 0)
                        {
                            throw new Exception("Username is already in use. Please choose unique.");
                        }
                    }

                    var user = Mapper<UserEditViewModel, User>.Map(model);
                    _userService.Update(user);


                    if (model.Group != "" && Guid.Parse(model.Group) != Guid.Empty)
                    {
                        var existings = _groupUserService.GetGroupUserByUserGUIDAndGroupGUID(model.UserGuid, Guid.Parse(model.Group));
                        if (existings.Count() == 0)
                        {
                            _groupUserService.Insert(new GroupUser { GroupGUID = Guid.Parse(model.Group), UserGUID = user.UserGuid });
                        }
                    }
                    if (model.Manager != "" && Guid.Parse(model.Manager) != Guid.Empty)
                    {
                        var existings = _managerUserService.GetManagerUser(model.UserGuid);
                        if (existings.Count() > 0)
                        {
                            var existing = existings.Where(c => c.ManagerGUID == Guid.Parse(model.Manager));
                            if (existing.Count() == 0)
                            {
                                _managerUserService.Insert(new ManagerUser { ManagerGUID = Guid.Parse(model.Manager), UserGUID = user.UserGuid });
                            }
                        }
                        else
                        {
                            _managerUserService.Insert(new ManagerUser { ManagerGUID = Guid.Parse(model.Manager), UserGUID = user.UserGuid });
                        }
                    }

                    user = _userService.GetUserByUserGuid(user.UserGuid);
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return BadRequestFormatter.BadRequest(this, ex);
                }
            }
            else
            {
                return BadRequestFormatter.BadRequest(this, "Invalid Model State");
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var user = _userService.GetUserByUserGuid(id);
                var viewUser = Mapper<User, UserViewModel>.Map(user);
                return PartialView(viewUser);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Delete([FromBody] UserViewModel model)
        {
            try
            {
                _managerUserService.DeleteByUserId(model.UserGuid);
                _managerUserService.DeleteByManagerId(model.UserGuid);
                _groupUserService.DeleteByUserId(model.UserGuid);
                _userService.DeleteByUserId(model.UserGuid);
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult UserGroups(Guid id)
        {
            try
            {
                var groups = _groupService.GetGroups();
                var userGroups = _groupUserService.GetGroupUserByUserGUID(id).Select(c => c.GroupGUID).ToList();
                var groupsViewModels = groups.Select(c => Mapper<Group, GroupViewModel>.Map(c)).ToList().Where(c => !userGroups.Contains(c.GroupGuid)).ToList();
                var userGroupsView = userGroups.Select(c => Mapper<Group, GroupViewModel>.Map(groups.Where(c1 => c1.GroupGuid == c).SingleOrDefault())).ToList();
                var user = _userService.GetUserByUserGuid(id);
                dynamic model = new
                {
                    User = user,
                    Groups = groupsViewModels,
                    UserGroups = userGroupsView
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        #region "Display Fields"
        public ActionResult ReportFields()
        {
            List<string> resourceTypes = new List<string>();
            resourceTypes.Add("User");
            var resources = _resourceService.GetResourceAttributes(resourceTypes);
            var fields = new List<GridviewField>();
            foreach (var field in resources)
            {
                fields.Add(new GridviewField { FieldLabel = field.Title, FieldName = field.Name.ToCamelCase(), IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = true });
            }

            return PartialView(fields);
        }

        public IActionResult GridviewFields()
        {

            var fields = new List<GridviewField>();
            fields.Add(new GridviewField { FieldLabel = "Name", FieldName = "name", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = true, Clickable = true, visibleToGrid = true });
            fields.Add(new GridviewField { FieldLabel = "Username", FieldName = "username", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = true });
            fields.Add(new GridviewField { FieldLabel = "Job Title", FieldName = "jobTitle", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = true });
            fields.Add(new GridviewField { FieldLabel = "Department", FieldName = "department", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = true });
            fields.Add(new GridviewField { FieldLabel = "Company", FieldName = "company", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = true });
            fields.Add(new GridviewField { FieldLabel = "Work Email", FieldName = "workEmail", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = true });
            fields.Add(new GridviewField { FieldLabel = "Mobile", FieldName = "mobilePhone", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = false });
            fields.Add(new GridviewField { FieldLabel = "Phone", FieldName = "workPhone", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = false });
            fields.Add(new GridviewField { FieldLabel = "Group", FieldName = "group", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = false });
            fields.Add(new GridviewField { FieldLabel = "Manager", FieldName = "manager", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = false });
            fields.Add(new GridviewField { FieldLabel = "Job Status", FieldName = "jobStatus", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, visibleToGrid = false });
            fields.Add(new GridviewField { FieldLabel = "Status", FieldName = "userStatus", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = false, Clickable = true });
            return Ok(fields);
        }
        #endregion

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignedGroups(Guid id, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                var userGroups = _groupUserService.GetGroupUserByUserGUID(id);
                if (userGroups.Count() > 0)
                {
                    var lst = userGroups.Select(c => c.GroupGUID).ToList();
                    var groups = _groupService.GetGroupByGUID(lst);
                    var viewGroups = groups.Select(c => Mapper<Group, GroupViewModel>.Map(c)).ToList();
                    return Ok(new { count = lst.Count(), result = viewGroups });
                }
                else
                {
                    return Ok(new { count = 0 });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignToGroup(Guid id, [FromBody] List<GroupViewModel> model)
        {
            try
            {

               
               foreach (var guid in model)
                {
                    _groupUserService.Insert(new GroupUser
                    {
                        GroupGUID =guid.GroupGuid,
                        UserGUID = id
                    });
                    
                }
                return Ok(new { status = true });
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);

            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteGroupBatch([FromBody] List<GroupViewModel> list)
        {
            try
            {
                List<Guid> guids = new List<Guid>();
                foreach (var item in list)
                {
                    guids.Add(item.GroupGuid);
                }

                var models = new List<GroupViewModel>();
                foreach (var group in list)
                {
                    models.Add(new GroupViewModel
                    {
                        CN = group.CN,
                        Description = group.Description,
                        UserGuid = group.UserGuid
                    });
                }

                return View(models);
            }
            catch (Exception ex)
            {
                var model = new ErrorViewModel();
                model.Message = ex.Message;
                return View("Error", model);
            }

        }
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteGroupBatchPost([FromBody] List<GroupViewModel> list)
        {
            try
            {
                List<Guid> lst = new List<Guid>();
                var userGuid = "";
                foreach (var item in list)
                {
                    lst.Add(item.GroupGuid);
                    if (item.UserGuid != null)
                    { userGuid = item.UserGuid; }
                }
                _groupService.DeleteGroup(lst, userGuid);
                return Ok(new { state = true });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Group()
        {
            return View();
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Policy()
        {
            return View();
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Get(string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                if (sortField == "name")
                    sortField = "Firstname";
                else if (sortField == "group")
                    sortField = "[group]";
                var users = _userService.GetUsers(searchValue, take, skip, sortField, dir);
                var total = _userService.GetUserCount(searchValue);
                var nUsers = new List<UserViewModel>();
                foreach (var user in users)
                {
                    UserViewModel u = Mapper<User, UserViewModel>.Map(user);
                    u.Name = u.Firstname + " " + u.Lastname;
                    nUsers.Add(u);
                }
                return Ok(new
                {
                    result = nUsers,
                    count = total
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }

        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GetUserForSwitchUser(string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                if (sortField == "name")
                    sortField = "DisplayName";
                var users = _userService.GetUsersPerson(searchValue, take, skip, sortField, dir);
                var total = _userService.GetUsersCountPerson(searchValue);
                var nUsers = new List<UserViewModel>();
                foreach (var user in users)
                {
                    UserViewModel u = Mapper<User, UserViewModel>.Map(user);
                    u.Displayname = user.DisplayName;
                    nUsers.Add(u);
                }
                return Ok(new
                {
                    result = nUsers,
                    count = total
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }

        }

        //Since Gridview needs definition of options like jobStatus, need a separate endpoint
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GetUsers(string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                if (sortField == "name")
                    sortField = "Firstname";
                else if (sortField == "group")
                    sortField = "[group]";
                var users = _userService.GetUsers(searchValue, take, skip, sortField, dir);
                var total = _userService.GetUserCount(searchValue);
                var nUsers = new List<UserViewModel>();
                var statuses = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobStatus");
                foreach (var user in users)
                {
                    var jobStatus = user.JobStatus;
                    UserViewModel u = Mapper<User, UserViewModel>.Map(user);
                    u.Name = u.Firstname + " " + u.Lastname;
                    var uStatus = statuses.Where(c => c.Value == jobStatus).SingleOrDefault();
                    if (uStatus != null)
                    {
                        u.JobStatus = uStatus.Name;
                    }
                    nUsers.Add(u);
                }
                return Ok(new
                {
                    result = nUsers,
                    count = total
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteBatch([FromBody] List<UserViewModel> list)
        {
            try
            {
                List<Guid> guids = new List<Guid>();
                foreach (var item in list)
                {
                    guids.Add(item.UserGuid);
                }
                var users = _userService.GetUserByUserGuid(guids);
                var models = new List<UserViewModel>();
                foreach (var user in users)
                {
                    var userView = Mapper<User, UserViewModel>.Map(user);
                    userView.Name = user.Firstname + " " + user.Lastname;
                    models.Add(userView);
                }

                return View(models);
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }

        }

        // POST: Group/Delete/5
        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteBatchPost([FromBody] List<UserViewModel> list)
        {
            try
            {
                List<Guid> lst = new List<Guid>();
                foreach (var item in list)
                {
                    lst.Add(item.UserGuid);
                }
                _userService.Delete(lst);
                return Ok(new { state = true });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult UnassignGroup(Guid id, Guid id1)
        {
            try
            {
                var groupUsers = _groupUserService.GetGroupUserByUserGUIDAndGroupGUID(id, id1);
                if (groupUsers.Count() > 0)
                {
                    var groupUser = groupUsers.SingleOrDefault();
                    var group = _groupService.GetGroupByGUID(groupUser.GroupGUID);
                    if (group != null)
                    {
                        var groupViewModel = Mapper<Group, GroupViewModel>.Map(group);
                        ViewBag.GroupUser = groupUser;
                        return PartialView(groupViewModel);
                    }
                }
                ModelState.AddModelError("", "Unable to find group.");
                return PartialView();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult UnassignGroup([FromBody] GroupUser model)
        {
            try
            {
                _groupUserService.DeleteByGroupUserId(model.GroupUserGUID);
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult UnassignedGroups(Guid userGuid, string searchValue, int take)
        {
            try
            {
                var userGroups = _groupUserService.GetGroupUnassignedToUser(userGuid, searchValue, take);
                if (userGroups.Count() > 0)
                {
                    var viewUsers = userGroups.Select(c => Mapper<Group, GroupViewModel>.Map(c)).ToList();
                    return Ok(viewUsers);
                }
                else
                {
                    return Ok(new { status = true });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Search()
        {
            var userSearchSpec = new UserSearchSpec();
            var criteria = new Criteria();
            var attributeToReturn = new List<string>();
            attributeToReturn.Add("Username");
            attributeToReturn.Add("Firstname");
            attributeToReturn.Add("UserGuid");
            criteria.Attribute = new ResourceAttribute()
            {
                AttributeType = ResourceAttributeType.String,
                ResourceType = "User",
                Name = "Username",
            };

            criteria.Operator = OperatorName.StringEquals;
            criteria.Value = new List<object>
            {
                "santosh.aryal",
                "shishir.baral"
            };
            userSearchSpec.AddCriteria(criteria);
            userSearchSpec.AttributesToReturn = attributeToReturn;
            return Json(_userService.Find(userSearchSpec));

        }

        [HttpPost]
        [Authorize]  // this method uses for key personnel in contract page 
        public IActionResult GetUsersData([FromBody] string searchText)
        {
            try
            {
                var listData = _userService.GetUsersData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = Core.Utilities.FormatHelper.FormatFullNamewithDesignation(ob.Firstname, String.Empty, ob.Lastname, ob.JobTitle);
                    model.label = result.Trim();
                    model.value = ob.UserGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ActionName("GetUsersData")]
        //[Secure(ResourceType.Admin, ResourceActionPermission.List)]
        [Authorize]  // this method uses for key personnel in contract page 
        public IActionResult GetUsers([FromBody] string searchText)
        {
            try
            {
                var userList = _userService.GetUsersData(searchText).Where(y => !string.IsNullOrEmpty(y.WorkEmail) && !string.IsNullOrEmpty(y.Firstname));
                userList.ToList().ForEach(x => x.DisplayName = x.Firstname + " " + x.Lastname);

                return Ok(new { data = userList });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GetUsersDataForAutoComplete(Guid? distributionListId, string searchValue, int take)
        {
            try
            {
                var list = _userService.GetUsersData(searchValue);
                return Json(list);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult UserPermissions(Guid userGuid)
        {
            try
            {
                var addItem = false;
                
                var loggedUser = Guid.Empty;
                if (userGuid == Guid.Empty)
                {
                    loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                }
                else
                {
                    loggedUser = userGuid;
                }

                var userGroups = _groupUserService.GetGroupUserByUserGUID(loggedUser);
                var userGroupGuids = userGroups.Select(c => c.GroupGUID).ToList();
                var list = new List<GroupPermissionApplicationModelView>();
                var resources = _groupService.GetGroupResources(loggedUser);
                var applications = resources.Select(c => c.Application).Distinct().OrderBy(c => c).ToList();
                foreach (var app in applications)
                {
                    addItem = false;
                    var item = new GroupPermissionApplicationModelView
                    {
                        Application = app,
                        Resources = resources.Where(c => c.Application == app).AsEnumerable()
                    };
                    foreach (var r in item.Resources)
                    {
                        r.Actions = _groupService.GetGroupResourceActions(userGroupGuids, r.ResourceGuid);
                        if (r.Actions.Count() > 0)
                        {
                            addItem = true;
                        }
                        
                    }
                    if (addItem)
                    {
                        list.Add(item);
                    }
                }
                  
               
                return Json(list);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Remove([FromBody] UserGroup model)
        {
            _groupService.RemoveUserGroup(model.UserGuid, model.GroupGuid);
            return Ok(new { status = true });
        }


        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignedGroupsCount(Guid id)
        {
            try
            {
                var userGroups = _groupUserService.GetGroupUserCountByUserGUID(id);
               
                 return Ok(new { count = userGroups.Count() });
               
            }
            catch 
            {
                return Ok(new { count = 0 });
            }
        }
    }
}

