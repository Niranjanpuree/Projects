using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels.Customer;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICommonService _commonService;
        private readonly ICountryService _countryService;
        private readonly IStateRepository _stateRepository;
        private readonly ICustomerTypeService _customerTypeService;
        private readonly IUsCustomerOfficeListService _usCustomerOfficeListService;
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public CustomerController(ICustomerService customerService,
                                  ICommonService commonService,
                                  IUsCustomerOfficeListService usCustomerOfficeListService,
                                  ICountryService countryService,
                                  ICustomerTypeService customerTypeService,
                                  IStateRepository stateRepository,
                                  IConfiguration configuration)
        {
            _customerService = customerService;
            _commonService = commonService;
            _configuration = configuration;
            _usCustomerOfficeListService = usCustomerOfficeListService;
            _countryService = countryService;
            _stateRepository = stateRepository;
            _customerTypeService = customerTypeService;
            _logger = LogManager.GetCurrentClassLogger();
        }
        // GET: Customer

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public ActionResult Index(string searchValue)
        {
            CustomerListViewModel customerListViewModel = new CustomerListViewModel();
            customerListViewModel.SearchValue = searchValue;
            return View(customerListViewModel);
        }

        [HttpPost]
        public ActionResult AdvanceSearch(LstSearch lstSearch)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index");
                }
                var resource = "Customer";
                var customerSearchSpec = new CustomerSearchSpec();
                var attributeToReturn = new List<string>();
                var attributeList = _customerService.GetAttributeNameListByResource(resource).ToList();
                attributeToReturn = attributeList.Select(x => x.AttributeName).ToList();
                foreach (var searchOb in lstSearch.SearchVms)
                {
                    var criteria = new Criteria();

                    criteria.Attribute = new ResourceAttribute()
                    {
                        AttributeType = (ResourceAttributeType)Enum.Parse(typeof(ResourceAttributeType), attributeList.Where(x => x.AttributeName.Equals(searchOb.AttributeName)).Select(x => x.DataType).First()),
                        ResourceType = resource,
                        Name = searchOb.AttributeName,
                    };

                    criteria.Operator = (OperatorName)Enum.Parse(typeof(OperatorName), searchOb.Operator);

                    var values = searchOb.FirstValue.Split(',');

                    List<object> list = new List<object>();
                    foreach (var val in values)
                    {
                        list.Add(val);
                    }
                    criteria.Value = list;
                    criteria.ValueToCompare = searchOb.ValueToCompare;
                    customerSearchSpec.AddCriteria(criteria);
                }
                customerSearchSpec.AttributesToReturn = attributeToReturn;
                return Json(_customerService.Find(customerSearchSpec));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            List<CustomerListViewModel> customerListViewModels = new List<CustomerListViewModel>();
            try
            {
                var modelData = _customerService.GetAll(searchValue, pageSize, skip, sortField, sortDirection);
                foreach (var items in modelData)
                {
                    var mappedData = ObjectMapper<Customer, CustomerListViewModel>.Map(items);
                    customerListViewModels.Add(mappedData);
                }
                var total = _customerService.TotalRecord(searchValue);
                var data = customerListViewModels.Select(x => new
                {
                    Abbreviations = x.Abbreviations,
                    Address = x.Address,
                    AddressLine1 = x.AddressLine1,
                    Agency = x.Agency,
                    City = x.City,
                    PrimaryEmail = x.PrimaryEmail,
                    Country = x.Country,
                    CustomerCode = x.CustomerCode,
                    CustomerGuid = x.CustomerGuid,
                    CustomerName = x.CustomerName,
                    CustomerTypeName = x.CustomerTypeName,
                    Department = x.Department,
                    DisplayName = x.DisplayName,
                    ZipCode = x.ZipCode,
                    IsActive = x.IsActive,
                    IsActiveStatus = x.IsActiveStatus,
                    SearchValue = x.SearchValue,
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();
                return Json(new { total = total, data = data });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        #region GET
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            try
            {
                var customerModel = _customerService.GetCustomerDetailsById(id);

                CustomerViewModel customerViewModel = new CustomerViewModel();
                customerViewModel = ObjectMapper<Customer, CustomerViewModel>.Map(customerModel);

                return PartialView(customerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                CustomerViewModel customerViewModel = new CustomerViewModel();
                var countryGuid = _countryService.GetCountryGuidBy3DigitCode("USA");
                customerViewModel.CountryId = countryGuid;
                customerViewModel.AgencySelectListItems = new Dictionary<string, string>();
                customerViewModel.DepartmentSelectListItems = _usCustomerOfficeListService.GetUsCustomerOfficeDepartmentList().ToDictionary(x => x.DepartmentName, x => x.DepartmentName);
                customerViewModel.CustomerTypeSelectListItems = _customerTypeService.GetCustomerTypeList().ToDictionary(x => x.CustomerTypeGuid, x => x.CustomerTypeName);
                customerViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
                customerViewModel.StateSelectListItems = _stateRepository.GetStateByCountryGuid(customerViewModel.CountryId).ToDictionary(x => x.StateId, x => x.StateName);
                return PartialView(customerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            try
            {
                var customerModel = _customerService.GetCustomerById(id);
                IDictionary<string, string> agencylist = new Dictionary<string, string>();
                CustomerViewModel customerViewModel = new CustomerViewModel();
                customerViewModel = ObjectMapper<Customer, CustomerViewModel>.Map(customerModel);
                var listvalue = string.IsNullOrEmpty(customerViewModel.Agency) ? "" : customerViewModel.Agency;
                agencylist.Add(new KeyValuePair<string, string>(listvalue, listvalue));
                customerViewModel.AgencySelectListItems = agencylist;
                customerViewModel.DepartmentSelectListItems = _usCustomerOfficeListService.GetUsCustomerOfficeDepartmentList().ToDictionary(x => x.DepartmentName, x => x.DepartmentName);
                customerViewModel.CustomerTypeSelectListItems = _customerTypeService.GetCustomerTypeList().ToDictionary(x => x.CustomerTypeGuid, x => x.CustomerTypeName);
                customerViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
                customerViewModel.StateSelectListItems = _stateRepository.GetStateByCountryGuid(customerViewModel.CountryId).ToDictionary(x => x.StateId, x => x.StateName);
                return PartialView(customerViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        #endregion

        #region Post
        [HttpPost]
        public IActionResult Add([FromBody] CustomerViewModel customerViewModel)
        {
            try
            {
                // Mapping Done

                Customer customer = new Customer();
                customer = ObjectMapper<CustomerViewModel, Customer>.Map(customerViewModel);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Guid id = Guid.NewGuid();
                customer.CustomerGuid = id;
                customer.CreatedOn = DateTime.Now;
                customer.CreatedBy = id;
                customer.UpdatedOn = DateTime.Now;
                customer.UpdatedBy = id;
                customer.IsActive = true;
                customer.IsDeleted = false;
                var CustomerName = customer.CustomerName;
                if (string.IsNullOrEmpty(customer.CustomerCode))
                {
                    customer.CustomerCode = "N/A";
                }
                else
                {
                    if (_customerService.CheckDuplicates(customer) > 0)
                    {
                        ModelState.AddModelError("", "Duplicate value entered for either code or name !!");
                        return BadRequest(ModelState);
                    }
                }
                _customerService.Add(customer);

                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.Customer.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Customer";

                var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                var resource = string.Format("{0} </br> GUID: {1} </br> Customer Name: {2} </br> Customer Code: {3}", ResourceType.Customer.ToString(), customer.CustomerGuid, customer.CustomerName, customer.CustomerCode);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), customer, resource, customer.CustomerGuid, UserHelper.GetHostedIp(HttpContext), "Customer Added", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", customer = new { customerGuid = customer.CustomerGuid, customerName = customer.CustomerName } });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public ActionResult Edit([FromBody]CustomerViewModel customerViewModel)
        {
            try
            {
                // Mapping Done
                Customer customer = new Customer();
                var customerType = _customerTypeService.GetCustomerTypeByGuid(customerViewModel.CustomerTypeGuid);
                customer = ObjectMapper<CustomerViewModel, Customer>.Map(customerViewModel);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (customer.CustomerCode != "N/A" && customerType == EnumGlobal.CustomerType.Federal.ToString())
                {
                    if (_customerService.CheckDuplicates(customer) > 0)
                    {
                        ModelState.AddModelError("", "Duplicate value entered for either code or name !!");
                        return BadRequest(ModelState);
                    }
                }
                if (string.IsNullOrEmpty(customer.CustomerCode))
                {
                    customer.CustomerCode = "N/A";
                }
                //var customerdetails = _customerService.GetCustomerById(customer.CustomerGuid);
                customer.UpdatedOn = DateTime.Now;
                customer.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);

                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.Customer.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Customer";

                var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                var resource = string.Format("{0} </br> GUID: {1} </br> Customer Name: {2} </br> Customer Code: {3}", ResourceType.Customer.ToString(), customer.CustomerGuid, customer.CustomerName, customer.CustomerCode);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), customer, resource, customer.CustomerGuid, UserHelper.GetHostedIp(HttpContext), "Customer Edited", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);

                _customerService.Edit(customer);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public ActionResult Delete([FromBody]Guid[] ids)
        {
            try
            {

                foreach (var id in ids)
                {
                    //audit log..
                    var customer = _customerService.GetCustomerDetailsById(id);
                    if (customer != null)
                    {
                        _customerService.DeleteById(id);

                        var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), ResourceType.Customer.ToString().ToLower());

                        var resource = string.Format("{0} </br> GUID: {1} </br> Customer Name: {2} </br> Customer Code: {3}", ResourceType.Customer.ToString(), id, customer.CustomerName, customer.CustomerCode);

                        AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), customer, resource, id, UserHelper.GetHostedIp(HttpContext), "Customer  Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
                    }
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public ActionResult Disable([FromBody]Guid[] ids)
        {
            try
            {
                _customerService.DisableCustomer(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var customer = _customerService.GetCustomerDetailsById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), ResourceType.Customer.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Customer";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID: {1} </br> Customer Name: {2} </br> Customer Code: {3}", ResourceType.Customer.ToString(), id, customer.CustomerName, customer.CustomerCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), customer, resource, id, UserHelper.GetHostedIp(HttpContext), "Customer Disabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public ActionResult Enable([FromBody]Guid[] ids)
        {
            try
            {
                _customerService.EnableCustomer(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var customer = _customerService.GetCustomerDetailsById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), ResourceType.Customer.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Customer";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID: {1} </br> Customer Name: {2} </br> Customer Code: {3}", ResourceType.Customer.ToString(), id, customer.CustomerName, customer.CustomerCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), customer, resource, id, UserHelper.GetHostedIp(HttpContext), "Customer Enabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult GetOfficeData([FromBody] string searchText)
        {
            try
            {
                string trimmedSearchText = searchText.Trim();
                var listData = _customerService.GetOfficeData(trimmedSearchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = Infrastructure.Helpers.FormatHelper.FormatCustomerNameAndAddress(ob.CustomerName, ob.Department, ob.Address, ob.City, ob.StatesName,
                        ob.ZipCode, ob.CountryName);
                    model.label = result.Trim();
                    model.value = ob.CustomerGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        #endregion

    }
}
