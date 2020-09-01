using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class DistributionListController : Controller
    {

        private readonly IDistributionListService _distributionListService;
        private readonly IUserService _userService;
        private readonly IDistributionUserService _distributionUserService;
        private readonly IConfiguration _configuration;

        public DistributionListController(IDistributionListService distributionListService,
                                          IUserService userService,
                                          IDistributionUserService distributionUserService,
                                          IConfiguration configuration)
        {
            _distributionListService = distributionListService;
            _userService = userService;
            _distributionUserService = distributionUserService;
            _configuration = configuration;
        }

        public IActionResult Index(string redirectUrl)
        {
            return View();
        }

        public IActionResult GetDistribution()
        {
            return PartialView("_DistributionPartial");
        }
        public IActionResult GetDistributionIndividualUser()
        {
            return PartialView("_DistributionIndividualUserPartial");
        }
        public IActionResult HasDistributionListForTheLoggedUser()
        {
            var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
            var hasDistribution = _distributionListService.HasDistributionList(loggedUser);

            return Json(new { hasDistribution = hasDistribution });
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            try
            {
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                var distributionList = _distributionListService.Get(loggedUser, searchValue, pageSize, skip, take, sortField, dir);
                List<DistributionListViewModel> result = new List<DistributionListViewModel>();
                var emptyListMessage = @"You have not created any distribution list yet";

                foreach (var distribution in distributionList)
                {
                    var mapVal = Models.ObjectMapper<DistributionList, DistributionListViewModel>.Map(distribution);
                    var user = _userService.GetUserByUserGuid(mapVal.CreatedBy);
                    mapVal.CreatedByName = Infrastructure.Helpers.FormatHelper.FormatFullName(user.Firstname, string.Empty, user.Lastname);
                    mapVal.MemberCount = _distributionUserService.GetDistributionUserCountByDistributionListId(mapVal.DistributionListGuid);
                    mapVal.UpdatedOnFormatted = mapVal.UpdatedOn.ToString("MM/dd/yyyy");
                    mapVal.IsOwner = distribution.CreatedBy == loggedUser ? true : false;
                    result.Add(mapVal);
                }

                if (distributionList.Count() == 0)
                    return Ok(new { result, count = _distributionListService.GetCount(loggedUser, searchValue), message = emptyListMessage });

                return Ok(new { result, count = _distributionListService.GetCount(loggedUser, searchValue) });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult GetUsers(Guid distributionListGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                var distributionUsers = _distributionUserService.GetUsersByDistributionListGuid(distributionListGuid, searchValue, take, skip, sortField, dir);
                var takeValueForRemainingUser = take - distributionUsers.Count();

                IEnumerable<User> userExceptDistributionUser = new List<User>();
                if (takeValueForRemainingUser > 0)
                {
                    userExceptDistributionUser =
                       _distributionUserService.GetUsersExceptDistributionUser(distributionListGuid, searchValue,
                           takeValueForRemainingUser,
                           skip, sortField, dir);
                }

                var total = _distributionUserService.GetUserCountExceptDistributionUser(distributionListGuid, searchValue);


                List<DistributionUserProfile> distributionUserProfiles = new List<DistributionUserProfile>();

                foreach (var user in distributionUsers)
                {
                    var mappedUser = Models.ObjectMapper<User, DistributionUserProfile>.Map(user);
                    mappedUser.Status = true;
                    distributionUserProfiles.Add(mappedUser);
                }

                foreach (var user in userExceptDistributionUser)
                {
                    var mappedUser = Models.ObjectMapper<User, DistributionUserProfile>.Map(user);
                    mappedUser.Status = false;
                    distributionUserProfiles.Add(mappedUser);
                }

                var result = distributionUserProfiles.OrderBy(x => x.Username).OrderByDescending(x => x.Status);
                return Ok(new
                {
                    result = result,
                    count = total
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult GetSelectedUsers(Guid distributionListGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                var distributionUsers = _distributionUserService.GetUsersByDistributionListGuid(distributionListGuid, searchValue, take, skip, sortField, dir);

                var total = _distributionUserService.GetSelectedUsersCount(distributionListGuid, searchValue);

                List<DistributionUserProfile> distributionUserProfiles = new List<DistributionUserProfile>();

                foreach (var user in distributionUsers)
                {
                    var mappedUser = Models.ObjectMapper<User, DistributionUserProfile>.Map(user);
                    mappedUser.Status = true;
                    mappedUser.DistributionListId = distributionListGuid;
                    distributionUserProfiles.Add(mappedUser);
                }

                //var result = distributionUserProfiles.OrderBy(x => x.Username).OrderByDescending(x => x.Status);
                return Ok(new
                {
                    result = distributionUserProfiles,
                    count = total
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult RemoveMemberFromDistributionList(Guid DistributioinListId, Guid UserId)
        {
            try
            {
                _distributionUserService.RemoveMemberFromDistributionList(DistributioinListId, UserId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        [HttpPost]
        public IActionResult Add([FromBody]DistributionListViewModel distributionViewModel)
        {
            try
            {
                var checkDuplicateDistributionTitle = _distributionListService.IsDuplicateTitle(distributionViewModel.Title);

                if (checkDuplicateDistributionTitle)
                {
                    var errorMessage = "Found duplicate distribution title..";
                    ModelState.AddModelError("", errorMessage);
                    return Ok(new { status = false, message = errorMessage });
                }

                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                var distribution = ObjectMapper<DistributionListViewModel, DistributionList>.Map(distributionViewModel);
                distribution.DistributionListGuid = Guid.NewGuid();
                distribution.IsActive = true;
                distribution.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                distribution.CreatedBy = loggedUser;
                distribution.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();

                //add selected users for distribution..
                _distributionListService.Add(distribution);

                AddDistributionUser(distributionViewModel, distribution, loggedUser);
                return Ok(new { status = true, message = "Successfully Updated !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody]DistributionListViewModel distributionViewModel)
        {
            try
            {
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                var distribution = ObjectMapper<DistributionListViewModel, DistributionList>.Map(distributionViewModel);
                distribution.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                distribution.UpdatedBy = loggedUser;

                _distributionListService.Edit(distribution);

                _distributionListService.DeleteDistributionUserByDistributionListId(distributionViewModel.DistributionListGuid);

                //add selected users for distribution..
                AddDistributionUser(distributionViewModel, distribution, loggedUser);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        public IActionResult Delete([FromBody]Guid[] ids)
        {
            try
            {
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                foreach (var id in ids)
                {
                    var distributionList = _distributionListService.GetDistributionListById(id);

                    if (distributionList.CreatedBy == loggedUser)
                    {
                        _distributionListService.DeleteDistributionUserByDistributionListId(id);
                        _distributionListService.Delete(id);
                    }
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        private void AddDistributionUser(DistributionListViewModel distributionViewModel, DistributionList distribution,
           Guid loggedUser)
        {

            List<DistributionUser> distributionUsers = new List<DistributionUser>();

            //if select all is selected in grid..
            if (distributionViewModel.UserSelection != null)
            {
                if (distributionViewModel.UserSelection.SelectedAll)
                {
                    var users = _userService.GetUsersData(string.Empty)
                        .Where(x => !string.IsNullOrEmpty(x.Firstname) && !string.IsNullOrEmpty(x.WorkEmail));

                    var usersExceptExcludelist = users.Where(x =>
                        !distributionViewModel.UserSelection.ExcludeList.Select(y => y.UserGuid).Contains(x.UserGuid));

                    if (usersExceptExcludelist.Count() > 0)
                    {
                        foreach (var user in usersExceptExcludelist)
                        {
                            var distributionUser = new DistributionUser()
                            {
                                DistributionUserGuid = Guid.NewGuid(),
                                DistributionListGuid = distribution.DistributionListGuid,
                                UserGuid = user.UserGuid,
                                CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                CreatedBy = loggedUser,
                                UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                UpdatedBy = loggedUser
                            };
                            distributionUsers.Add(distributionUser);
                        }
                    }

                    //see if user has been selected from multi select drop down control...
                    if (distributionViewModel.SelectedUsers.Count() > 0)
                    {
                        foreach (var user in distributionViewModel.SelectedUsers)
                        {
                            var distributionUser = new DistributionUser()
                            {
                                DistributionUserGuid = Guid.NewGuid(),
                                DistributionListGuid = distribution.DistributionListGuid,
                                UserGuid = user.UserGuid,
                                CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                CreatedBy = loggedUser,
                                UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                UpdatedBy = loggedUser
                            };
                            distributionUsers.Add(distributionUser);
                        }
                    }


                    // finally save the distinct user..
                    var distributionUniqueUser = distributionUsers.GroupBy(p => p.UserGuid)
                        .Select(g => g.FirstOrDefault())
                        .ToList(); ;
                    if (distributionUniqueUser.Count() > 0)
                    {
                        foreach (var user in distributionUniqueUser)
                        {
                            var distributionUser = new DistributionUser()
                            {
                                DistributionUserGuid = Guid.NewGuid(),
                                DistributionListGuid = distribution.DistributionListGuid,
                                UserGuid = user.UserGuid,
                                CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                CreatedBy = loggedUser,
                                UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                UpdatedBy = loggedUser
                            };
                            _distributionListService.AddDistributionUser(distributionUser);
                        }
                    }
                }
                // select all is not  selected in grid..
                else
                {
                    if (distributionViewModel.UserSelection.IncludeList.Count() > 0)
                    {
                        foreach (var user in distributionViewModel.UserSelection.IncludeList)
                        {
                            var distributionUser = new DistributionUser()
                            {
                                DistributionUserGuid = Guid.NewGuid(),
                                DistributionListGuid = distribution.DistributionListGuid,
                                UserGuid = user.UserGuid,
                                CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                CreatedBy = loggedUser,
                                UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                UpdatedBy = loggedUser
                            };
                            distributionUsers.Add(distributionUser);

                        }
                    }
                    //see if user has been selected from multi select drop down control...
                    if (distributionViewModel.SelectedUsers.Count() > 0)
                    {
                        foreach (var user in distributionViewModel.SelectedUsers)
                        {
                            var distributionUser = new DistributionUser()
                            {
                                DistributionUserGuid = Guid.NewGuid(),
                                DistributionListGuid = distribution.DistributionListGuid,
                                UserGuid = user.UserGuid,
                                CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                CreatedBy = loggedUser,
                                UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                UpdatedBy = loggedUser
                            };
                            distributionUsers.Add(distributionUser);
                        }
                    }
                    // finally save the distinct user..
                    var distributionUniqueUser = distributionUsers.GroupBy(p => p.UserGuid)
                        .Select(g => g.FirstOrDefault())
                        .ToList();
                    if (distributionUniqueUser.Count() > 0)
                    {
                        foreach (var user in distributionUniqueUser)
                        {
                            var distributionUser = new DistributionUser()
                            {
                                DistributionUserGuid = Guid.NewGuid(),
                                DistributionListGuid = distribution.DistributionListGuid,
                                UserGuid = user.UserGuid,
                                CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                CreatedBy = loggedUser,
                                UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                                UpdatedBy = loggedUser
                            };
                            _distributionListService.AddDistributionUser(distributionUser);
                        }
                    }
                }
            }
            else
            {
                if (distributionViewModel.SelectedUsers.Count() > 0)
                {
                    foreach (var user in distributionViewModel.SelectedUsers)
                    {
                        var distributionUser = new DistributionUser()
                        {
                            DistributionUserGuid = Guid.NewGuid(),
                            DistributionListGuid = distribution.DistributionListGuid,
                            UserGuid = user.UserGuid,
                            CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                            CreatedBy = loggedUser,
                            UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime(),
                            UpdatedBy = loggedUser
                        };
                        _distributionListService.AddDistributionUser(distributionUser);
                    }
                }
            }
        }
    }
}