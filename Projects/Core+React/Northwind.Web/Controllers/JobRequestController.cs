using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Web.Helpers;
using Northwind.Web.Models.ViewModels;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using NLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Models.ViewModels.FarClause;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Core.Services;
using static Northwind.Core.Entities.GenericNotification;
using EnumGlobal = Northwind.Core.Entities.EnumGlobal;
using System.Text;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Controllers
{
    public class JobRequestController : Controller
    {
        private readonly IJobRequestService _jobRequestService;
        private readonly ICommonService _commonService;
        private readonly IFarContractService _farContractService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IContractQuestionariesService _contractQuestionariesService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly IEmailSender _emailSender;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly IMapper _mapper;
        private readonly Logger _eventLogger;
        private readonly IConfiguration _configuration;
        private readonly IContractsService _contractRefactorService;
        private readonly IUserService _userService;
        private readonly IGenericNotificationService _genericNotificationService;
        private readonly IUrlHelper _urlHelper;
        private readonly Logger _logger;
        private readonly IQuestionaireUserAnswerService _questionaireUserAnswerService;

        public JobRequestController(
            IJobRequestService jobRequestService,
            ICommonService commonService,
            IMapper mapper,
            INotificationTemplatesService notificationTemplatesService,
            IEmailSender emailSender,
            INotificationBatchService notificationBatchService,
            IFarContractService farContractService,
            IFarContractTypeService farContractTypeService,
            IContractQuestionariesService contractQuestionariesService,
            IContractsService contractRefactorService,
            IUserService userService,
            IConfiguration configuration,
            IGenericNotificationService genericNotificationService,
            IUrlHelper urlHelper,
            IQuestionaireUserAnswerService questionaireUserAnswerService)
        {
            _jobRequestService = jobRequestService;
            _commonService = commonService;
            _farContractService = farContractService;
            _farContractTypeService = farContractTypeService;
            _contractQuestionariesService = contractQuestionariesService;
            _mapper = mapper;
            _configuration = configuration;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationBatchService = notificationBatchService;
            _contractRefactorService = contractRefactorService;
            _emailSender = emailSender;
            _userService = userService;
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
            _urlHelper = urlHelper;
            _genericNotificationService = genericNotificationService;
            _logger = LogManager.GetCurrentClassLogger();
            _questionaireUserAnswerService = questionaireUserAnswerService;
        }

        [Secure(ResourceType.JobRequest, ResourceActionPermission.List)]
        public IActionResult Index(string searchValue)
        {
            var model = new JobRequestViewModel();
            model.SearchValue = searchValue;
            return View(model);
        }

        [Secure(ResourceType.JobRequest, ResourceActionPermission.List)]
        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, string additionalFilterValue = "All")
        {
            try
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                var jobRequests = _jobRequestService.GetAll(searchValue, pageSize, skip, take, orderBy, dir, additionalFilterValue, userGuid).ToList();

                List<JobRequestViewModelForList> jobRequestViewModelForLists = new List<JobRequestViewModelForList>();
                foreach (var ob in jobRequests)
                {
                    var mapVal = Models.ObjectMapper<JobRequest, JobRequestViewModelForList>.Map(ob);
                    mapVal.ProjectNumber = ob.ProjectNumber;
                    if (ob.ProjectManager != null)
                        mapVal.ProjectManagerReview = ob.ProjectManager.DisplayName;
                    if (ob.ContractRepresentative != null)
                        mapVal.ContractReview = ob.ContractRepresentative.DisplayName;
                    if (ob.AccountRepresentative != null)
                        mapVal.AccountingReview = ob.AccountRepresentative.DisplayName;
                    if (ob.ProjectControls != null)
                        mapVal.ProjectControlReview = ob.ProjectControls.DisplayName;
                    jobRequestViewModelForLists.Add(mapVal);
                }
                int totalRecordCount = _jobRequestService.TotalRecord(searchValue, additionalFilterValue, userGuid);

                var data = jobRequestViewModelForLists.Select(x => new
                {
                    JobRequestGuid = x.JobRequestGuid,
                    ContractGuid = x.ContractGuid,
                    InitiatedBy = x.InitiatedBy,
                    JobRequestTitle = x.JobRequestTitle,
                    JobRequestStatus = Infrastructure.Helpers.FormatHelper.FormatJobRequestValue(x.TotalJobStatus, x.UpdatedOn.ToString("MM/dd/yyyy")),
                    ContractReview = Infrastructure.Helpers.FormatHelper.FormatJobRequestValue(ProgressStatus.Done.ToString(), "-" + x.ContractReview),  //contract review person initialize the job request
                    ProjectControlReview = Infrastructure.Helpers.FormatHelper.FormatJobRequestValue(x.ProjectControlReviewStatus, "-" + x.ProjectControlReview),
                    ProjectManagerReview = Infrastructure.Helpers.FormatHelper.FormatJobRequestValue(x.ProjectManagerReviewStatus, "-" + x.ProjectManagerReview),
                    AccountingReview = Infrastructure.Helpers.FormatHelper.FormatJobRequestValue(x.AccountingReviewStatus, "-" + x.AccountingReview),
                    IsActiveStatus = x.IsActive == true ? ActiveStatus.Active.ToString() : ActiveStatus.Inactive.ToString()
                }).ToList();
                return Json(new { total = totalRecordCount, data = data });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpGet]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Add)]
        public IActionResult Add(Guid id)
        {
            try
            {
                var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(id);
                var model = ContractsMapper.MapJobRequestToViewModel(jobRequestEntity);
                var companyListEntity = _jobRequestService.GetCompanyData();
                var companyList = _mapper.Map<ICollection<Models.ViewModels.KeyValuePairModel<Guid, string>>>(companyListEntity);
                if (model.Parent_ContractGuid == null)
                    model.BaseUrl = "Contract";
                else
                {
                    model.BaseUrl = "Project";
                    var parentContractDetails = _contractRefactorService.GetBasicContractById(model.Parent_ContractGuid ?? Guid.Empty);
                    var parentContractNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ContractNumber;
                    model.BasicContractInfo.ParentProjectNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ProjectNumber;
                }
                model.companyList = companyList;
                model.Status = (int)JobRequestStatus.ContractRepresentative;
                model.radioIsIntercompanyWorkOrder = KeyValueHelper.GetYesNo();
                model.ContractGuid = id;

                var questionaire = _mapper.Map<Models.ViewModels.Contract.ContractQuestionaireViewModel>(jobRequestEntity.Contracts.ContractQuestionaire);
                model.ContractQuestionaire = questionaire;
                model.farContractViewModel = GetFarContract(model.ContractGuid);
                model.IsNew = true;
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(e.ToString(), e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Add)]
        public ActionResult Add(JobRequestViewModel jobRequestViewModel)
        {
            try
            {
                List<string> selectionOfCompanies = new List<string>();

                JobRequest jobRequest = new JobRequest();
                jobRequest = ContractsMapper.MapViewModelToJobRequest(jobRequestViewModel);

                if (jobRequestViewModel.CompanySelected != null && jobRequestViewModel.CompanySelected.Count > 0)
                {
                    foreach (var name in jobRequestViewModel.CompanySelected)
                    {
                        selectionOfCompanies.Add(name);
                    }
                    jobRequest.Companies = string.Join(",", selectionOfCompanies);
                }
                Guid id = Guid.NewGuid();
                jobRequest.JobRequestGuid = id;
                jobRequest.CreatedOn = DateTime.Now;
                jobRequest.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                jobRequest.UpdatedOn = DateTime.Now;
                jobRequest.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                jobRequest.IsActive = true;
                jobRequest.IsDeleted = false;
                jobRequest.Status = (int)JobRequestStatus.ProjectControl;
                _jobRequestService.Add(jobRequest);

                SendNotification(jobRequest.JobRequestGuid, jobRequest.ContractGuid, jobRequest.Status);

                //audit log..
                var contractEntity = _contractRefactorService.GetContractEntityByContractId(jobRequest.ContractGuid);
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.JobRequest.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/jobRequest/Detail/" + jobRequest.ContractGuid);
                var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Job Request", contractEntity.ProjectNumber, contractEntity.ContractTitle);
                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), jobRequest.BasicContractInfo, resource, jobRequest.JobRequestGuid, UserHelper.GetHostedIp(HttpContext), "Job Request Added", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                //end of log..


                _contractRefactorService.UpdateContractUsers(jobRequest.Contracts.ContractUserRole);
                _contractRefactorService.UpdateProjectNumberByGuid(jobRequest.Contracts.ContractGuid, jobRequest.Contracts.ProjectNumber);
                //after updating contract send notification..
                var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(Core.Entities.EnumGlobal.ResourceType.JobRequest.ToString(), "Notify");
                //Core.Entities.EnumGlobal.CrudType.Create.ToString());
                var redirectUrl = string.Format($@"/JobRequest/Detail/{jobRequest.Contracts.ContractGuid}");

                var parameter = new
                {
                    redirectUrl = redirectUrl,
                    key = key,
                    cameFrom = "Contract Management",
                    resourceName = "Job Request",
                    resourceDisplayName = "Job Request",
                    resourceId = jobRequest.Contracts.ContractGuid
                };
                return RedirectToAction("Index", "Notification", parameter);
                //SendEmailToRespectivePersonnel(jobRequest.Status, jobRequest.Contracts.ContractGuid);
                //return RedirectToAction("Details", jobRequestViewModel.BaseUrl, new { id = jobRequest.Contracts.ContractGuid });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(jobRequestViewModel);
            }
        }

        [HttpGet]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Edit)]
        public IActionResult Edit(Guid id)
        {
            try
            {
                var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(id);
                var model = ContractsMapper.MapJobRequestToViewModel(jobRequestEntity);

                if (model.Parent_ContractGuid == null)
                    model.BaseUrl = "Contract";
                else
                {
                    model.BaseUrl = "Project";
                    var parentContractDetails = _contractRefactorService.GetBasicContractById(model.Parent_ContractGuid ?? Guid.Empty);
                    var parentContractNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ContractNumber;
                    model.BasicContractInfo.ParentProjectNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ProjectNumber;
                }
                var companyListEntity = _jobRequestService.GetCompanyData();
                var companyList = _mapper.Map<ICollection<Models.ViewModels.KeyValuePairModel<Guid, string>>>(companyListEntity);
                var companies = model.Companies;

                var questionaire = _mapper.Map<Models.ViewModels.Contract.ContractQuestionaireViewModel>(jobRequestEntity.Contracts.ContractQuestionaire);
                model.ContractQuestionaire = questionaire;
                List<string> companySelected = new List<string>();
                if (!string.IsNullOrEmpty(companies))
                {
                    var listCompany = companies.Split(",");
                    foreach (var name in listCompany)
                    {
                        companySelected.Add(name);
                    }
                }

                model.CompanySelected = companySelected;
                model.companyList = companyList;
                model.radioIsIntercompanyWorkOrder = KeyValueHelper.GetYesNo();
                model.ContractGuid = id;
                model.JobRequestGuid = id;
                model.farContractViewModel = GetFarContract(model.ContractGuid);
                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Details)]
        public IActionResult Detail(Guid id)
        {
            try
            {
                var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(id);
                var model = ContractsMapper.MapJobRequestToViewModel(jobRequestEntity);
                if (model.Parent_ContractGuid == null) model.BaseUrl = "Contract";
                else
                {
                    model.BaseUrl = "Project";
                    var parentContractDetails = _contractRefactorService.GetBasicContractById(model.Parent_ContractGuid ?? Guid.Empty);
                    var parentContractNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ContractNumber;
                    model.BasicContractInfo.ParentProjectNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ProjectNumber;
                }
                var companyEntityList = _jobRequestService.GetCompanyData();
                var companyList = _mapper.Map<ICollection<Models.ViewModels.KeyValuePairModel<Guid, string>>>(companyEntityList);
                var companies = model.Companies;

                var questionaire = _mapper.Map<Models.ViewModels.Contract.ContractQuestionaireViewModel>(jobRequestEntity.Contracts.ContractQuestionaire);
                model.ContractQuestionaire = questionaire;
                List<string> companySelected = new List<string>();
                if (!string.IsNullOrEmpty(companies))
                {
                    var listCompany = companies.Split(",");
                    foreach (var name in listCompany)
                    {
                        var companyName = _jobRequestService.GetCompanyName(name);
                        companySelected.Add(companyName);
                    }
                }
                model.CompanySelected = companySelected;
                model.companyList = companyList;
                model.ContractGuid = id;
                model.farContractViewModel = GetFarContract(model.ContractGuid);

                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Edit)]
        public ActionResult Edit(JobRequestViewModel jobRequestViewModel)
        {
            try
            {
                List<string> selectionOfCompanies = new List<string>();
                JobRequest jobRequest = new JobRequest();
                jobRequest = ContractsMapper.MapViewModelToJobRequest(jobRequestViewModel);
                jobRequest.UpdatedOn = DateTime.Now;
                jobRequest.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                jobRequest.IsActive = true;
                jobRequest.IsDeleted = false;
                if (jobRequestViewModel.CompanySelected != null && jobRequestViewModel.CompanySelected.Count > 0)
                {
                    foreach (var name in jobRequestViewModel.CompanySelected)
                    {
                        selectionOfCompanies.Add(name);
                    }
                    jobRequest.Companies = string.Join(",", selectionOfCompanies);
                }
                var loggedUser = UserHelper.GetLoggedUser(HttpContext);

                _jobRequestService.Edit(jobRequest);
                var getPreviousStatus = _jobRequestService.GetCurrentStatusByGuid(jobRequestViewModel.JobRequestGuid);
                if (getPreviousStatus > jobRequestViewModel.Status)
                {
                    SendNotification(jobRequestViewModel.JobRequestGuid, jobRequestViewModel.ContractGuid, jobRequestViewModel.Status);
                }

                //audit log..
                var contractEntity = _contractRefactorService.GetContractEntityByContractId(jobRequest.ContractGuid);
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.JobRequest.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/jobRequest/Detail/" + jobRequestViewModel.ContractGuid);
                var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Job Request", contractEntity.ProjectNumber, contractEntity.ContractTitle);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), jobRequest.BasicContractInfo, resource, jobRequest.JobRequestGuid, UserHelper.GetHostedIp(HttpContext), "Job Request Edited", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                //end of log..

                //remove president and regional manager from the list
                if (jobRequest.Contracts.ContractUserRole != null)
                {
                    var president = jobRequest.Contracts.ContractUserRole.Where(x => x.UserRole == Contracts._companyPresident).FirstOrDefault();
                    var regional = jobRequest.Contracts.ContractUserRole.Where(x => x.UserRole == Contracts._regionalManager).FirstOrDefault();
                    jobRequest.Contracts.ContractUserRole.ForEach(x => x.ContractGuid = jobRequest.Contracts.ContractGuid);
                    jobRequest.Contracts.ContractUserRole.Remove(president);
                    jobRequest.Contracts.ContractUserRole.Remove(regional);
                    _contractRefactorService.UpdateContractUsers(jobRequest.Contracts.ContractUserRole);
                }
                _contractRefactorService.UpdateProjectNumberByGuid(jobRequest.Contracts.ContractGuid, jobRequest.Contracts.ProjectNumber);

                var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(Core.Entities.EnumGlobal.ResourceType.JobRequest.ToString(), "Notify");
                //Core.Entities.EnumGlobal.CrudType.Edit.ToString());
                var redirectUrl = string.Format($@"/JobRequest/Detail/{jobRequest.Contracts.ContractGuid}");
                var parameter = new
                {
                    redirectUrl = redirectUrl,
                    key = key,
                    cameFrom = "Contract Management",
                    resourceName = "JobRequest",
                    resourceDisplayName = "JobRequest",
                    resourceId = jobRequest.Contracts.ContractGuid
                };
                return RedirectToAction("Index", "Notification", parameter);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(jobRequestViewModel);
            }
        }

        [HttpPost]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Delete)]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                _jobRequestService.Delete(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var jobRequestModel = _jobRequestService.GetJobRequestEntityByJobRequestId(id);
                    var contractEntity = _contractRefactorService.GetContractEntityByContractId(jobRequestModel.ContractGuid);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), ResourceType.JobRequest.ToString());

                    //var resource = string.Empty;
                    //if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                    //    resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.JobRequest.ToString(), jobRequestModel.JobRequestGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    //else
                    //    resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder  No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.JobRequest.ToString(), jobRequestModel.JobRequestGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);

                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Job Request", contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractEntity.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Job Request Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
                }
                //end of log..

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Edit)]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _jobRequestService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var jobRequestModel = _jobRequestService.GetJobRequestEntityByJobRequestId(id);
                    var contractEntity = _contractRefactorService.GetContractEntityByContractId(jobRequestModel.ContractGuid);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), ResourceType.JobRequest.ToString());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/jobRequest/Detail/" + jobRequestModel.ContractGuid);

                    //var resource = string.Empty;
                    //if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                    //    resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.JobRequest.ToString(), jobRequestModel.JobRequestGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    //else
                    //    resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder  No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.JobRequest.ToString(), jobRequestModel.JobRequestGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);

                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Job Request", contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractEntity.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Job Request Disabled", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                }
                //end of log..

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        [Secure(ResourceType.JobRequest, ResourceActionPermission.Edit)]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _jobRequestService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var jobRequestModel = _jobRequestService.GetJobRequestEntityByJobRequestId(id);
                    var contractEntity = _contractRefactorService.GetContractEntityByContractId(jobRequestModel.ContractGuid);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), ResourceType.JobRequest.ToString());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/jobRequest/Detail/" + jobRequestModel.ContractGuid);

                    //var resource = string.Empty;
                    //if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                    //    resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.JobRequest.ToString(), jobRequestModel.JobRequestGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    //else
                    //    resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder  No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.JobRequest.ToString(), jobRequestModel.JobRequestGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);

                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Job Request", contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractEntity.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Job Request Enabled", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                }
                //end of log..

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [Authorize]
        public PartialViewResult GetWbsView(Guid id)
        {
            var file = _contractRefactorService.GetFilesByContractGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
            var model = _mapper.Map<JobRequestViewModel>(file);
            return PartialView("_WorkBreakdownStructure", model);
        }

        /// <summary>
        /// For sending email to contract representative based on job request status
        /// </summary>
        /// <param name="status">Job Status</param>
        /// <param name="keyPerson">Contract representative</param>
        [Authorize]
        public void SendEmailToRespectivePersonnel(int status, Guid contractGuid)
        {
            var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(contractGuid);
            var model = ContractsMapper.MapJobRequestToViewModel(jobRequestEntity);
            var keyPersonnel = _contractRefactorService.GetKeyPersonnelByContractGuid(contractGuid);

            var param = new { id = contractGuid };
            var link = RouteUrlHelper.GetAbsoluteAction(_urlHelper, "JobRequest", "Detail", param);
            //var urlLink = new UrlHelper(ControllerContext.RequestContext);
            JobRequestEmailModel emailModel = new JobRequestEmailModel();
            emailModel.ContractNumber = model.BasicContractInfo.ContractNumber;
            emailModel.ProjectNumber = model.BasicContractInfo.ProjectNumber;
            emailModel.AwardingAgency = model.CustomerInformation.AwardingAgencyOfficeName;
            emailModel.FundingAgency = model.CustomerInformation.FundingAgencyOfficeName;
            emailModel.ClickableUrl = link;
            emailModel.Status = "In Progress";
            string emailTo = "gmagar@xylontech.com";
            string recipientName = string.Empty;
            string subject = string.Empty;

            Guid notifyTo = UserHelper.CurrentUserGuid(HttpContext);

            var notificationTemplate = _notificationTemplatesService.GetByKey("jobrequest-notify");
            var content = string.Empty;
            var template = string.Empty;
            if (notificationTemplate != null)
                template = notificationTemplate.Message;



            //for filtering the representative to send email
            switch (status)
            {
                case (int)JobRequestStatus.ContractRepresentative:
                    var controlRepresentative = model.KeyPersonnel.ProjectControls;
                    if (controlRepresentative != null)
                        notifyTo = controlRepresentative;
                    var projectUser = _userService.GetUserByUserGuid(notifyTo);
                    if (projectUser != null)
                    {
                        //emailTo = contractUser.WorkEmail;
                        recipientName = projectUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "A new Job Request Form has been submitted for contract: " + emailModel.ContractNumber;
                    }
                    var conManager = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectManager);
                    emailModel.NotifiedTo = conManager.Firstname + " " + conManager.Lastname;
                    var submittedBy = _userService.GetUserByUserGuid(model.KeyPersonnel.ContractRepresentative);
                    emailModel.SubmittedBy = submittedBy.Firstname + " " + submittedBy.Lastname;
                    break;
                case (int)JobRequestStatus.ProjectControl:
                    var projectRepresentative = model.KeyPersonnel.ProjectManager;
                    if (projectRepresentative != null)
                        notifyTo = projectRepresentative;
                    var managerUser = _userService.GetUserByUserGuid(notifyTo);
                    if (managerUser != null)
                    {
                        //emailTo = controlUser.WorkEmail;
                        recipientName = managerUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "A new Job Request Form has been submitted for contract: " + emailModel.ContractNumber;
                    }
                    var manager = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectManager);
                    emailModel.NotifiedTo = manager.Firstname + " " + manager.Lastname;

                    var submittedByProject = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectControls);
                    emailModel.SubmittedBy = submittedByProject.Firstname + " " + submittedByProject.Lastname;
                    break;
                case (int)JobRequestStatus.ProjectManager:
                    var managerRepresentative = model.KeyPersonnel.AccountingRepresentative;
                    if (managerRepresentative != null)
                        notifyTo = managerRepresentative;
                    var accountManager = _userService.GetUserByUserGuid(notifyTo);
                    if (accountManager != null)
                    {
                        //emailTo = projectManager.WorkEmail;
                        recipientName = accountManager.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "A new Job Request Form has been submitted for contract: " + emailModel.ContractNumber;
                    }
                    var submittedByManager = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectManager);
                    emailModel.SubmittedBy = submittedByManager.Firstname + " " + submittedByManager.Lastname;
                    break;
                case (int)JobRequestStatus.Accounting:
                    var accountUser = _userService.GetUserByUserGuid(notifyTo);
                    if (accountUser != null)
                    {
                        //emailTo = accountUser.WorkEmail;
                        recipientName = accountUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "Job Request has been set to done contract: " + emailModel.ContractNumber;
                    }
                    var submittedByAccount = _userService.GetUserByUserGuid(model.KeyPersonnel.AccountingRepresentative);
                    emailModel.SubmittedBy = submittedByAccount.Firstname + " " + submittedByAccount.Lastname;
                    emailModel.Status = "Done";
                    break;
                default:
                    break;
            }

            content = EmailHelper.GetContentForJobRequestNotify(template, keyPersonnel, emailModel);
            _emailSender.SendEmailAsync(emailTo, recipientName, subject, content);
        }

        private FarContractViewModel GetFarContract(Guid contractGuid)
        {
            var viewModel = new FarContractViewModel();
            var requiredList = new List<FarContractDetail>();
            var applicableList = new List<FarContractDetail>();
            var contractQuestionary = new ContractQuestionaireViewModel();

            Guid farContractType = _contractRefactorService.GetFarContractTypeGuidById(contractGuid);
            var farContractModel = _farContractTypeService.GetById(farContractType);
            var requiredListData = _farContractService.GetRequiredData(contractGuid, farContractType);
            var applicableListData = _farContractService.GetSelectedData(contractGuid, farContractType);
            requiredList = requiredListData;
            applicableList = applicableListData;


            var contractQuestionaireEntity = _contractQuestionariesService.GetContractQuestionariesById(contractGuid);
            var contractQuestionaireModel = _mapper.Map<ContractQuestionaireViewModel>(contractQuestionaireEntity);

            viewModel.ContractGuid = contractGuid;
            viewModel.ContractQuestionaires = contractQuestionary;
            viewModel.RequiredFarClauses = requiredList;
            viewModel.ApplicableFarClauses = applicableList;

            if (farContractModel != null)
            {
                viewModel.FarContractTypeCode = farContractModel.Code;
                viewModel.FarContractTypeName = farContractModel.Title;
            }
            if (contractQuestionaireModel != null)
            {
                contractQuestionary = contractQuestionaireModel;
                var users = _userService.GetUserByUserGuid(contractQuestionaireModel.UpdatedBy);
                viewModel.UpdatedBy = users != null ? users.DisplayName : "";
                viewModel.UpdatedOn = contractQuestionaireModel.UpdatedOn;
            }

            var hasAnyQA = _questionaireUserAnswerService.CheckQAByContractGuid(contractGuid);
            if (hasAnyQA)
                viewModel.Questionniare = _contractQuestionariesService.GetContractQuestionaries(ResourceType.FarContract.ToString(), "Edit", contractGuid).ToList();
            return viewModel;
        }

        private bool SendNotification(Guid resourceId, Guid contractGuid, int currentStage)
        {
            try
            {
                var notificationModel = new GenericNotificationViewModel();
                var notificationTemplatesDetails = new NotificationTemplatesDetail();
                var userList = new List<User>();
                var receiverInfo = new User();
                var receiverGuid = Guid.Empty;

                notificationModel.ResourceId = resourceId;
                notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/JobRequest/Detail/" + contractGuid);
                notificationModel.NotificationTemplateKey = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction
                (EnumGlobal.ResourceType.JobRequest.ToString(), EnumGlobal.CrudType.Notify.ToString());
                notificationModel.CurrentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                notificationModel.CurrentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                notificationModel.SendEmail = true;

                var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(contractGuid);
                var model = ContractsMapper.MapJobRequestToViewModel(jobRequestEntity);

                var keyPersonnels = _contractRefactorService.GetKeyPersonnelByContractGuid(contractGuid);
                if (keyPersonnels?.Any() == true)
                {
                    switch (currentStage)
                    {
                        case (int)JobRequestStatus.ProjectControl:
                            var projectControls = keyPersonnels.FirstOrDefault(x => x.UserRole == ContractUserRole._projectControls);
                            if (projectControls != null)
                            {
                                receiverGuid = projectControls.UserGuid;
                            }
                            break;
                        case (int)JobRequestStatus.ProjectManager:
                            var projectManager = keyPersonnels.FirstOrDefault(x => x.UserRole == ContractUserRole._projectManager);
                            if (projectManager != null)
                            {
                                receiverGuid = projectManager.UserGuid;
                            }
                            break;
                        case (int)JobRequestStatus.Accounting:
                            var accountRepresentative = keyPersonnels.FirstOrDefault(x => x.UserRole == ContractUserRole._accountRepresentative);
                            if (accountRepresentative != null)
                            {
                                receiverGuid = accountRepresentative.UserGuid;
                            }
                            break;
                    }

                    receiverInfo = _userService.GetUserByUserGuid(receiverGuid);
                    if (receiverInfo != null)
                    {
                        userList.Add(receiverInfo);
                        notificationModel.IndividualRecipients = userList;
                    }

                    var keyList = "<ul>";
                    foreach (var person in keyPersonnels)
                    {
                        keyList += "<li>" + person.User.DisplayName + " (" + person.UserRole + ")" + "</li>";
                    }
                    keyList += "</li>";
                    StringBuilder additionalUser = new StringBuilder(keyList);

                    notificationTemplatesDetails.ContractNumber = model.BasicContractInfo.ContractNumber;
                    notificationTemplatesDetails.AwardingAgency = model.CustomerInformation.AwardingAgencyOfficeName;
                    notificationTemplatesDetails.FundingAgency = model.CustomerInformation.FundingAgencyOfficeName;
                    notificationTemplatesDetails.ProjectNumber = model.BasicContractInfo.ProjectNumber;
                    notificationTemplatesDetails.ContractTitle = model.BasicContractInfo.ContractTitle;
                    notificationTemplatesDetails.Description = model.BasicContractInfo.Description;
                    notificationTemplatesDetails.AdditionalUser = additionalUser.ToString();
                    notificationTemplatesDetails.Status = "";
                    notificationModel.NotificationTemplatesDetail = notificationTemplatesDetails;
                    _genericNotificationService.AddNotificationMessage(notificationModel);
                    return true;
                }
                return false;
            }
            catch(Exception ex)
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                EventLogHelper.Error(_eventLogger, new EventLog
                {
                    EventGuid = Guid.NewGuid(),
                    Action = "Index",
                    Application = "ESS",
                    EventDate = DateTime.UtcNow,
                    Message = ex.Message,
                    Resource = ResourceType.Contract.ToString(),
                    StackTrace = ex.StackTrace,
                    UserGuid = userGuid
                });
                return false;
            }
        }

    }
}