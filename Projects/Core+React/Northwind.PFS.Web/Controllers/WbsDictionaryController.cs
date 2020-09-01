using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.PFS.Web.Models.ViewModels;
using Northwind.PFS.Web.Models;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using Northwind.CostPoint.Interfaces;
using Northwind.CostPoint.Entities;
using Northwind.Web.Infrastructure.Models;
using Northwind.Core.Entities;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class WbsDictionaryController : Controller
    {
        IWbsServiceCP _wbsService;
        IMapper _mapper;
        IUserService _userService;
        IWbsDictionaryService _wbsDictionaryService;
        public WbsDictionaryController(IWbsServiceCP wbsService, IWbsDictionaryService wbsDictionaryService,  IMapper mapper, IUserService userService)
        {
            _wbsService = wbsService;
            _mapper = mapper;
            _userService = userService;
            _wbsDictionaryService = wbsDictionaryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromBody] WbsDictionaryViewModel model)
        {
            try
            {
                var wbsDictionary = new WbsDictionary
                {
                    ProjectNumber = model.ProjectNumber,
                    CreatedBy = GetUserGuid(),
                    WbsNumber = model.WbsNumber,
                    WbsDictionaryTitle = model.WbsDictionaryTitle,
                    CreatedOn = DateTime.Now
                };
                _wbsDictionaryService.Add(wbsDictionary);
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult Get(string projectNumber, string wbsNumber, string searchValue, int skip, int take, string sortField, string dir)
        {
            try
            {
                if (string.IsNullOrEmpty(sortField))
                {
                    sortField = "CreatedOn";
                }
                var result = _wbsDictionaryService.GetWbsDictionary(projectNumber, wbsNumber, searchValue, skip, take, sortField, dir);
                var count = _wbsDictionaryService.GetWbsDictionaryCount(projectNumber, wbsNumber, searchValue);
                var list = new List<WbsDictionaryViewModel>();
                foreach (var dict in result)
                {
                    var user = _userService.GetUserByUserGuid(dict.CreatedBy);
                    var whoUser = user == null ? "" : FormatHelper.FormatFullName(user.Firstname, string.Empty, user.Lastname);

                    list.Add(new WbsDictionaryViewModel
                    {
                        ProjectNumber = dict.ProjectNumber,
                        CreatedBy = dict.CreatedBy,
                        CreatedOn = dict.CreatedOn,
                        WbsDictionaryGuid = dict.WbsDictionaryGuid,
                        WbsDictionaryTitle = dict.WbsDictionaryTitle,
                        WbsNumber = dict.WbsDictionaryTitle,
                        CreatedByName = whoUser
                    });
                }

                if (!string.IsNullOrEmpty(sortField))
                {
                    if (sortField.ToLower().Equals("createdon"))
                    {
                        if (dir.ToLower().Equals("asc"))
                            list = list.OrderBy(x => x.CreatedByName).ToList();
                        else
                            list = list.OrderByDescending(x => x.CreatedByName).ToList();
                    }
                }

                return Json(new { result = list, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }


        [HttpPost]
        public IActionResult Edit([FromBody] WbsDictionaryViewModel model)
        {
            try
            {
                var wbs = _wbsDictionaryService.GetWbsDictionaryByGuid(model.WbsDictionaryGuid);
                if (wbs == null)
                {
                    throw new Exception("Unable to find WBS");
                }

                wbs.WbsDictionaryTitle = model.WbsDictionaryTitle;
                wbs.CreatedOn = DateTime.Now;
                wbs.CreatedBy = GetUserGuid();
                _wbsDictionaryService.Update(wbs);
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
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    _wbsDictionaryService.Delete(id);
                }
                return Json(new { status = true });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        private Guid GetUserGuid()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Guid.Parse(User.Identity.Name);
            }
            return Guid.Empty;
        }
    }
}