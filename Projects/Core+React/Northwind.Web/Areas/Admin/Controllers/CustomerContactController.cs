using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Models;
using Northwind.Core.Utilities;
using Microsoft.Extensions.Configuration;
using Northwind.Web.Models.ViewModels.Contact;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Authorization;
using static Northwind.Core.Entities.EnumGlobal;
using Microsoft.AspNetCore.Authorization;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CustomerContactController : Controller
    {
        private readonly ICustomerContactService _customerContactService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerContactTypeService _customerContactTypeService;
        private readonly IConfiguration _configuration;

        public CustomerContactController(
            ICustomerContactService customerContactService,
            ICustomerService customerService,
            ICustomerContactTypeService customerContactTypeService,
            IConfiguration configuration)
        {
            _customerContactService = customerContactService;
            _customerService = customerService;
            _customerContactTypeService = customerContactTypeService;
            _configuration = configuration;
        }
        // GET: CustomerContact

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Index(string searchValue, Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
            {
                return StatusCode(401);
            }
            ContactViewModel contactViewModel = new ContactViewModel();
            contactViewModel.SearchValue = searchValue;
            contactViewModel.CustomerGuid = customerGuid;
            var customerdetails = _customerService.GetCustomerById(customerGuid);
            contactViewModel.CustomerName = customerdetails.CustomerName;
            return PartialView(contactViewModel);
        }

        public IActionResult Get(string searchValue, Guid customerGuid, int pageSize, int skip, string sortField, string sortDirection)
        {
            List<ContactViewModel> contactViewModel = new List<ContactViewModel>();
            try
            {
                var modelData = _customerContactService.GetAll(searchValue, customerGuid, pageSize, skip, sortField, sortDirection);
                foreach (var items in modelData)
                {
                    var viewModel = ObjectMapper<CustomerContact, ContactViewModel>.Map(items);
                    contactViewModel.Add(viewModel);
                }
                var total = _customerContactService.TotalRecord(customerGuid);
                return Json(new { total = total, data = contactViewModel });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e); 
            }
        }

        #region Crud
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            ContactViewModel contactViewModel = new ContactViewModel();
            var modeldata = _customerContactService.GetDetailsById(id);
            contactViewModel = ObjectMapper<CustomerContact, ContactViewModel>.Map(modeldata);
            return PartialView(contactViewModel);
        }

        [HttpGet]
        public ActionResult Add(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
            {
                return StatusCode(401);
            }
            var contacttype = _customerContactTypeService.GetCustomerContactList();
            ContactViewModel contactViewModel = new ContactViewModel();
            contactViewModel.ContactTypeSelectListItems = contacttype.ToDictionary(x => x.ContactTypeGuid, x => x.ContactTypeName);
            contactViewModel.GenderSelectListItems = KeyValueHelper.getGender();
            contactViewModel.CustomerGuid = customerGuid;
            return PartialView(contactViewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ContactViewModel contactViewModel = new ContactViewModel();
            var modeldata = _customerContactService.GetDetailsById(id);
            contactViewModel = ObjectMapper<CustomerContact, ContactViewModel>.Map(modeldata);
            var CustomerDetails = _customerService.GetCustomerById(contactViewModel.CustomerGuid);
            contactViewModel.CustomerName = CustomerDetails.CustomerName;
            contactViewModel.ContactTypeSelectListItems = _customerContactTypeService.GetCustomerContactList().ToDictionary(x => x.ContactTypeGuid, x => x.ContactTypeName);
            contactViewModel.GenderSelectListItems = KeyValueHelper.getGender();
            return PartialView(contactViewModel);
        }

        [HttpPost]
        public IActionResult Add([FromBody]ContactViewModel contactViewModel)
        {
            try
            {
                Guid id = Guid.NewGuid();
                var customerContact = ObjectMapper<ContactViewModel, CustomerContact>.Map(contactViewModel);
                customerContact.ContactTypeGuid = contactViewModel.ContactType;
                customerContact.ContactGuid = id;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                customerContact.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                customerContact.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                customerContact.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                customerContact.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                customerContact.IsActive = true;
                customerContact.IsDeleted = false;
                var CustomerContact = _customerContactService.Add(customerContact);
                var jsonObjects = new
                {
                    contactguid = customerContact.ContactGuid,
                    searchvalue = customerContact.SearchValue,
                    customerguid = customerContact.CustomerGuid,
                    customerName = customerContact.CustomerName,
                    customerLastName = customerContact.LastName,
                    customerPhoneNumber = customerContact.PhoneNumber,
                    fullName = Infrastructure.Helpers.FormatHelper.FormatFullName(customerContact.FirstName, customerContact.MiddleName, customerContact.LastName)
                };
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", CustomerContact = jsonObjects });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody]ContactViewModel contactViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                contactViewModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contactViewModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                var customerContactdetails = _customerContactService.GetDetailsById(contactViewModel.ContactGuid);

                var contact = ObjectMapper<ContactViewModel, CustomerContact>.Map(contactViewModel);
                contact.ContactTypeGuid = contactViewModel.ContactType;
                var customerContact = _customerContactService.Edit(contact);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!", CustomerContact = new { SearchValue = " ", customerguid = contactViewModel.CustomerGuid, customerName = contactViewModel.CustomerName } });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public ActionResult Delete([FromBody]Guid[] ids)
        {
            try
            {
                _customerContactService.Delete(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this,ex);
            }
        }

        [HttpPost]
        public ActionResult Disable([FromBody]Guid[] ids)
        {
            try
            {
                _customerContactService.Disable(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this,ex);
            }
        }

        [HttpPost]
        public ActionResult Enable([FromBody]Guid[] ids)
        {
            try
            {
                _customerContactService.Enable(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        #endregion
    }
}
