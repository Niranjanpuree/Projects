using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Northwind.Web.Models.ViewModels;
using Northwind.Core.Entities;
using Northwind.web.Helpers;
using Northwind.Web.Models;
using Northwind.Web.Helpers;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OfficeContactController : Controller
    {
        private readonly IOfficeContactService _officeContactService;
        private readonly IOfficeService _officeService;
        private readonly IConfiguration _configuration;

        public OfficeContactController(IOfficeContactService officeContactService,
                                       IOfficeService officeService,
                                       IConfiguration configuration)
        {
            _officeContactService = officeContactService;
            _officeService = officeService;
            _configuration = configuration;
        }
        // GET: OfficeContact
        public ActionResult Index(string searchValue, Guid officeGuid)
        {
            if (officeGuid == Guid.Empty)
            {
                return StatusCode(401);
            }
            OfficeContactViewModel officeContactViewModel = new OfficeContactViewModel();
            officeContactViewModel.SearchValue = searchValue;
            officeContactViewModel.OfficeGuid = officeGuid;

            var officedetails = _officeService.GetById(officeGuid);
            officeContactViewModel.OfficeName = officedetails.OfficeName;
            return PartialView(officeContactViewModel);
        }

        public IActionResult Get(string searchValue, Guid officeGuid, int pageSize, int skip, string sortField, string sortDirection)
        {
            try
            {
                var officeContactModel = _officeContactService.GetAll(searchValue, officeGuid, pageSize, skip, sortField, sortDirection);
                List<OfficeContactViewModel> officeContactListViewModels = new List<OfficeContactViewModel>();
                foreach (var ob in officeContactModel)
                {
                    var mapVal = Models.ObjectMapper<OfficeContact, OfficeContactViewModel>.Map(ob);
                    officeContactListViewModels.Add(mapVal);
                }

                var total = _officeContactService.TotalRecord(officeGuid);
                return Json(new { total = total, data = officeContactListViewModels });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        #region Crud
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            var officeContactModel = _officeContactService.GetById(id);
            var officeContactViewModel = Models.ObjectMapper<OfficeContact, OfficeContactViewModel>.Map(officeContactModel);

            return PartialView(officeContactViewModel);
        }

        [HttpGet]
        public ActionResult Add(Guid officeGuid, string contactType = null)
        {
            if (officeGuid == Guid.Empty)
            {
                return StatusCode(401);
            }
            var contactTypes = _officeContactService.GetContactType();

            OfficeContactViewModel officeContactViewModel = new OfficeContactViewModel();
            officeContactViewModel.ContactTypeSelectListItems = contactTypes
                .Where(x => (x.Value == contactType || contactType == null)).ToDictionary(x => x.Key, x => x.Value);
            officeContactViewModel.OfficeGuid = officeGuid;
            return PartialView(officeContactViewModel);
        }
        [HttpPost]
        public IActionResult Add([FromBody]OfficeContactViewModel officeContactViewModel)
        {
            try
            {
                var officeContactModel = Models.ObjectMapper<OfficeContactViewModel, OfficeContact>.Map(officeContactViewModel);

                Guid id = Guid.NewGuid();
                officeContactModel.ContactGuid = id;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                officeContactModel.CreatedOn = DateTime.Now;
                officeContactModel.CreatedBy = id;
                officeContactModel.UpdatedOn = DateTime.Now;
                officeContactModel.UpdatedBy = id;
                officeContactModel.IsActive = true;
                officeContactModel.IsDeleted = false;
                var officeContact = _officeContactService.Add(officeContactModel);
                var jsonObjects = new
                {
                    contactguid = officeContactModel.ContactGuid,
                    searchvalue = officeContactModel.SearchValue,
                    officeguid = officeContactModel.OfficeGuid,
                    officeName = officeContactModel.OfficeName,
                    officeLastName = officeContactModel.LastName,
                    officePhoneNumber = officeContactModel.PhoneNumber,
                    fullName = FormatHelper.FormatFullName(officeContactModel.FirstName, officeContactModel.MiddleName, officeContactModel.LastName)
                };
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", officeContact = jsonObjects });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var officeContactModel = _officeContactService.GetById(id);
            var officeContactViewModel = Models.ObjectMapper<OfficeContact, OfficeContactViewModel>.Map(officeContactModel);

            var officeDetails = _officeService.GetById(officeContactViewModel.OfficeGuid);

            officeContactViewModel.OfficeName = officeDetails.OfficeName;
            officeContactViewModel.ContactTypeSelectListItems = _officeContactService.GetContactType();
            return PartialView(officeContactViewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromBody]OfficeContactViewModel officeContactViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            officeContactViewModel.UpdatedOn = DateTime.Now;
            officeContactViewModel.UpdatedBy = UserHelper.CurrentUserGuid();

            var officeContactModel = Models.ObjectMapper<OfficeContactViewModel, OfficeContact>.Map(officeContactViewModel);

            _officeContactService.Edit(officeContactModel);
            return Ok(new
            {
                status = ResponseStatus.success.ToString(),
                message = "Successfully Updated !!",
                OfficeContact = new
                {
                    SearchValue = " ",
                    Officeguid = officeContactViewModel.OfficeGuid,
                    OfficeName = officeContactViewModel.OfficeName
                }
            });
        }

        [HttpPost]
        public ActionResult Delete([FromBody]Guid[] ids)
        {
            try
            {
                _officeContactService.Delete(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        public ActionResult Disable([FromBody]Guid[] ids)
        {
            try
            {
                _officeContactService.Disable(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost]
        public ActionResult Enable([FromBody]Guid[] ids)
        {
            try
            {
                _officeContactService.EnableOfficeContact(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        #endregion
    }
}
