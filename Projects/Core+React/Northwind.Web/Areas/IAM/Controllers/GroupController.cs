using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels;
using Northwind.Web.Models.ViewModels.Group;
using Northwind.Web.Models.ViewModels.Policy;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.IAM.Controllers
{
    [Area("IAM")]
    public class GroupController : Controller
    {
        IGroupService _groupService;
        IGroupUserService _groupUserService;
        IUserService _userService;
        IResourceAttributeValueService _resourceAttributeValueService;

        public GroupController(IGroupService groupService, IGroupUserService groupUserService, IUserService userService, IResourceAttributeValueService resourceAttributeValueService)
        {
            _groupService = groupService;
            _groupUserService = groupUserService;
            _userService = userService;
            _resourceAttributeValueService = resourceAttributeValueService;
        }
        // GET: Group
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Index()
        {
            return View();
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GroupListCombo(string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                if (take == 0)
                    take = 10;
                return Ok(_groupService.GetGroups(searchValue, take, skip, sortField, dir, new List<AdvancedSearchRequest>()));
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GroupList(string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                if (take == 0)
                    take = 10;
                var result = new
                {
                    result = _groupService.GetGroups(searchValue, take, skip, sortField, dir, new List<AdvancedSearchRequest>()),
                    count = _groupService.GetTotalRows(searchValue, take, skip, new List<AdvancedSearchRequest>())
                };
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GroupList(string searchValue, int take, int skip, string sortField, string dir, [FromBody] List<AdvancedSearchRequest> postValue)
        {
            try
            {
                if (take == 0)
                    take = 10;
                var result = new
                {
                    result = _groupService.GetGroups(searchValue, take, skip, sortField, dir, postValue),
                    count = _groupService.GetTotalRows(searchValue, take, skip, postValue)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public ActionResult ReportFields()
        {
            var fields = new List<GridviewField>();
            fields.Add(new GridviewField { FieldLabel = "Name", FieldName = "cn", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = true });            
            fields.Add(new GridviewField { FieldLabel = "Group Name", FieldName = "groupName", IsFilterable = true, IsSortable = true, OrderIndex = 3, IsDefaultSortField = false });
            fields.Add(new GridviewField { FieldLabel = "Description", FieldName = "description", IsFilterable = true, IsSortable = true, OrderIndex = 2, IsDefaultSortField = false });

            return PartialView(fields);
        }

        public IActionResult GridviewFields()
        {
            var fields = new List<GridviewField>();
            fields.Add(new GridviewField { FieldLabel = "Name", FieldName = "cn", IsFilterable = true, IsSortable = true, OrderIndex = 1, IsDefaultSortField = true });
            fields.Add(new GridviewField { FieldLabel = "Description", FieldName = "description", IsFilterable = true, IsSortable = true, OrderIndex = 2, IsDefaultSortField = false });

            return Ok(fields);
        }


        // GET: Group/Create
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Add()
        {
            return PartialView();
        }

        // POST: Group/Create
        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Add([FromBody] GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var group = new Group {
                        GroupGuid = Guid.NewGuid(),
                        CN = model.CN,
                        GroupName=model.CN,
                        Description= model.Description
                    };
                    var existingGroup = _groupService.GetGroupByName(model.CN);
                    if (existingGroup.Count() > 0)
                    {
                        throw new Exception("Duplicate group name is not allowed.");
                    }
                    _groupService.Insert(group);
                    return Ok(group);
                }
                catch(Exception ex)
                {
                    return BadRequestFormatter.BadRequest(this, ex);
                }
            }
            else
            {
                return BadRequestFormatter.BadRequest(this,"Bad Request");
            }
        }

        // GET: Group/Edit/5
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Edit([FromRoute]Guid id)
        {
            try
            {
                var grpGuid = id; // Guid.Parse(groupGuid);
                var group = _groupService.GetGroupByGUID(grpGuid);
                if(group == null)
                {
                    throw new ArgumentException("Unable to find group.");
                }
                var viewGroup = new GroupViewModel
                {
                    GroupGuid = group.GroupGuid,
                    CN = group.CN,
                    Description = group.Description
                };
                return PartialView(viewGroup);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Details([FromRoute]Guid id)
        {
            try
            {
                var grpGuid = id; // Guid.Parse(groupGuid);
                var group = _groupService.GetGroupByGUID(grpGuid);
                if (group == null)
                {
                    throw new ArgumentException("Unable to find group.");
                }
                var viewGroup = new GroupViewModel
                {
                    GroupGuid = group.GroupGuid,
                    CN = group.CN,
                    Description = group.Description
                };
                return PartialView(viewGroup);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Details(Dictionary<string, object> postValue)
        {
            try
            {
                return Redirect("/iam/group");
            }
            catch (Exception ex)
            {
                var grpGuid = Guid.Empty; // Guid.Parse(groupGuid);
                Guid.TryParse(postValue["groupGuid"].ToString(), out grpGuid);
                var group = _groupService.GetGroupByGUID(grpGuid);
                
                var viewGroup = new GroupViewModel
                {
                    GroupGuid = group.GroupGuid,
                    CN = group.CN,
                    Description = group.Description
                };
                return PartialView(viewGroup);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GetGroup([FromRoute]Guid id)
        {
            try
            {
                var grpGuid = id; // Guid.Parse(groupGuid);
                var group = _groupService.GetGroupByGUID(grpGuid);
                if (group == null)
                {
                    throw new ArgumentException("Unable to find group.");
                }
                var viewGroup = new GroupViewModel
                {
                    GroupGuid = group.GroupGuid,
                    CN = group.CN,
                    Description = group.Description
                };
                return Json(viewGroup);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        // POST: Group/Edit/5
        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Edit([FromBody] GroupViewModel model)
        {
            try
            {
                var group = new Group
                {
                    GroupGuid = model.GroupGuid,
                    CN = model.CN,
                    GroupName = model.CN,
                    Description = model.Description
                };
                var existingGroup = _groupService.GetGroupByName(model.CN);
                if (existingGroup.Count() > 0 && !existingGroup.ToList()[0].GroupGuid.Equals(model.GroupGuid))
                {
                    throw new Exception("Duplicate group name is not allowed.");
                }
                _groupService.Update(group);
                return Ok(group);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        // GET: Group/Delete/5
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Delete(Guid id)
        {
            try
            {
                var group = _groupService.GetGroupByGUID(id);
                if(group == null)
                {
                    throw new Exception("Unable to find group");
                }
                var model = new GroupViewModel
                {
                    CN = group.CN,
                    Description = group.Description,
                    GroupGuid = group.GroupGuid
                };
                return View(model);
            }
            catch (Exception ex)
            {
                var model = new ErrorViewModel();
                model.Message = ex.Message;
                return View("Error", model);
            }
            
        }

        [HttpPost("/IAM/Group/Delete")]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteGroup([FromBody] JObject group)
        {
            try
            {
                Guid groupId = Guid.Parse(group.Value<string>("GroupGuid"));
                var group1 = _groupService.GetGroupByGUID(groupId);
                if(group1 != null)
                {
                    _groupService.Delete(groupId);
                    return Json(new { status = true });
                }
                ModelState.Clear();
                ModelState.AddModelError("", "Unable to find groupd");
                return BadRequestFormatter.BadRequest(this, "Unable to find groupId");

            }
            catch (Exception ex)
            {
                ModelState.Clear();
                if (ex.Message.Contains("conflict"))
                {
                    ModelState.AddModelError("", "Group is in use so cannot be deleted.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to find groupId");
                }
                return BadRequestFormatter.BadRequest(this, "Unable to find groupId");
            }

        }

        // GET: Group/Delete/5
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteBatch([FromBody] List<GroupViewModel> list)
        {
            try
            {
                List<Guid> guids = new List<Guid>();
                foreach(var item in list)
                {
                    guids.Add(item.GroupGuid);
                }
                var groups = _groupService.GetGroupByGUID(guids);
                var models = new List<GroupViewModel>();
                foreach(var group in groups)
                {
                    models.Add(new GroupViewModel
                    {
                        CN = group.CN,
                        Description = group.Description,
                        GroupGuid = group.GroupGuid
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

        // GET: Group/Delete/5
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteUserBatch([FromBody] List<UserGroupViewModel> list)
        {
            try
            {
                List<Guid> guids = new List<Guid>();
                foreach (var item in list)
                {
                    guids.Add(item.UserGuid);
                }
             
                var models = new List<UserGroupViewModel>();
                foreach (var group in list)
                {
                    models.Add(new UserGroupViewModel
                    {
                        UserName = group.UserName,
                        FirstName = group.FirstName,
                        LastName = group.LastName,
                        GroupGuid=group.GroupGuid
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

        // POST: Group/Delete/5
        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteBatchPost([FromBody] List<GroupViewModel> list)
        {
            try
            {
                List<Guid> lst = new List<Guid>();
                foreach(var item in list)
                {
                    lst.Add(item.GroupGuid);
                }
                _groupService.Delete(lst);
                return Ok(new { state = true });
            }
            catch(Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult DeleteUserBatchPost([FromBody] List<UserGroupViewModel> list)
        {
            try
            {
                List<Guid> lst = new List<Guid>();
                var groupGuid ="";
                foreach (var item in list)
                {
                    lst.Add(item.UserGuid);
                    if (item.GroupGuid != null)
                    { groupGuid = item.GroupGuid; }
                }
                _groupService.Delete(lst,groupGuid);
                return Ok(new { state = true });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignedUsers(Guid id, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                var userGroups = _groupUserService.GetGroupUserByGroupGUID(id);
                if (userGroups.Count() > 0)
                {
                    var lst = userGroups.Select(c => c.UserGUID).ToList();
                    var users = _userService.GetUsers(lst, searchValue, take, skip, sortField, dir);
                    var userCount = _userService.GetUserCount(lst, searchValue, take, skip, sortField, dir);
                    var viewUsers = users.Select(c => Mapper<User, UserViewModel>.Map(c)).ToList();
                    return Ok(new { count = userCount, result = viewUsers });
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

        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult UnssignedUsers(Guid groupGuid, string searchValue, int take)
        {
            try
            {
                var userGroups = _groupUserService.GetGroupUnassignedUserToGroup(groupGuid, searchValue, take);
                if (userGroups.Count() > 0)
                {
                    var viewUsers = userGroups.Select(c => Mapper<User, UserViewModel>.Map(c)).ToList();
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

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignToGroup(Guid id, [FromBody] List<UserViewModel> users)
        {
            try
            {
                foreach(var model in users)
                {
                    var assigned = _groupUserService.GetGroupUserByUserGUIDAndGroupGUID(model.UserGuid, id);
                    var group = _groupService.GetGroupByGUID(id);
                    if (id == null)
                    {
                        ModelState.AddModelError("", "Unable to find group.");
                        return BadRequestFormatter.BadRequest(this, "Unable to find group.");
                    }
                    if (assigned.Count() == 0)
                    {
                        _groupUserService.Insert(new GroupUser
                        {
                            GroupGUID = id,
                            UserGUID = model.UserGuid
                        });
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError("", "User is already assigned to this group.");
                        return BadRequestFormatter.BadRequest(this, "User is already assigned to this group.");
                    }
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Group Guid</param>
        /// <param name="id1">User Guid</param>
        /// <returns></returns>
        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult UnassignUser(Guid id, Guid id1)
        {
            try
            {
                var groupUsers = _groupUserService.GetGroupUserByUserGUIDAndGroupGUID(id1, id);
                if (groupUsers.Count() > 0)
                {
                    var groupUser = groupUsers.ToList()[0];
                    var user = _userService.GetUserByUserGuid(groupUser.UserGUID);
                    if (user != null)
                    {
                        var userViewModel = Mapper<User, UserViewModel>.Map(user);
                        try
                        {
                            userViewModel.JobStatus = _resourceAttributeValueService.GetResourceValuesByResourceType("User", "JobStatus").Where(c => c.Value == userViewModel.JobStatus).SingleOrDefault().Name;
                        }
                        catch (Exception){}
                        ViewBag.GroupUser = groupUser;                        
                        return PartialView(userViewModel);
                    }
                }
                ModelState.AddModelError("", "Unable to find user.");
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
        public IActionResult UnassignUser([FromBody] GroupUser model)
        {
            try
            {
                _groupUserService.DeleteByGroupGuidUserGuid(model.GroupGUID, model.UserGUID);
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GroupGuid</param>
        /// <param name="searchValue"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="sortField"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        [HttpGet]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignedPolicy(Guid id, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                var list = _groupService.GetAssignedPolicyToGroup(id, searchValue, take, skip, sortField, dir);
                var count = _groupService.GetAssignedPolicyToGroupCount(id, searchValue);
                return Json(new { result = list, count = count });
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
        public IActionResult UnassignedPolicy(Guid groupGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                var list = _groupService.GetUnassignedPolicyToGroup(groupGuid, searchValue, take, skip, sortField, dir);
                return Json(list);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GroupGuid</param>
        /// <param name="postValue">Policy</param>
        /// <returns></returns>
        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult AssignPolicy(Guid id, [FromBody] PolicyViewModel postValue)
        {
            try
            {
                _groupService.AssignPolicyToGroup(id, postValue.PolicyGuid);
                return Json(new { status = true });
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
        public IActionResult UnassignPolicy([FromBody] GroupPolicyViewModel postValue)
        {
            try
            {
                _groupService.UnassignPolicyToGroup(postValue.GroupGuid, postValue.PolicyGuid);
                return Json(new { status = true });
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
        public IActionResult ResourceGroup(Guid groupGuid)
        {
            try
            {
                var addItem = false;
                var list = new List<GroupPermissionApplicationModelView>();
                var resources = _groupService.GetGroupResources();
                var applications = resources.Select(c => c.Application).Distinct().OrderBy(c => c).ToList();
                foreach(var app in applications)
                {
                    addItem = false;
                    var item = new GroupPermissionApplicationModelView
                    {
                        Application = app,
                        Resources = resources.Where(c => c.Application == app).AsEnumerable()
                    };
                    foreach (var r in item.Resources)
                    {
                        r.Actions = _groupService.GetGroupResourceActions(groupGuid, r.ResourceGuid);
                        if (r.Actions.Count() > 0)
                        {
                            addItem = true;
                        }
                    }
                    if(addItem)
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
        public IActionResult ResourceGroup(Guid groupGuid, [FromBody]  dynamic postValue)
        {
            var lists = new List<string>();
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(postValue["actions"]);
            try
            {
                 lists = JsonConvert.DeserializeObject<List<string>>(json);
            }
            catch
            {
                 lists = new List<string>();
                 var list = JsonConvert.DeserializeObject<string>(json);
                lists.Add(list);
            }
            try
            {   
                if(lists != null)
                {
                    _groupService.AssignGroupPermission(groupGuid, lists);
                }
                
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
    }
}