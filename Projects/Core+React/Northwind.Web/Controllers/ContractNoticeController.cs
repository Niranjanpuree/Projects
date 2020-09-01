using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Contract;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    public class ContractNoticeController : Controller
    {

        private readonly IContractsService _contractService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IContractNoticeService _contractNoticeService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        string documentRoot;

        public ContractNoticeController(IContractsService contractService, IMapper mapper, IContractNoticeService contractNoticeService, IUserService userService, IResourceAttributeValueService resourceAttributeValueService, IConfiguration configuration)
        {
            _contractService = contractService;
            _mapper = mapper;
            _contractNoticeService = contractNoticeService;
            _userService = userService;
            _configuration = configuration;
            _resourceAttributeValueService = resourceAttributeValueService;
            documentRoot = _configuration.GetSection("Document").GetValue<string>("DocumentRoot");
        }

        #region  Crud
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.List)]
        public ActionResult GetContractNotice(Guid contractGuid, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
           try
            {
                var contractNotices = _contractNoticeService.GetContractNoticeByContractId(contractGuid,  searchValue,  pageSize,  skip,  take,  sortField,  dir);
                List<NoticeViewModel> list = new List<NoticeViewModel>();
                foreach (var data in contractNotices)
                {
                    NoticeViewModel notice = new NoticeViewModel();
                    notice.Attachment = HttpUtility.HtmlDecode(data.Attachment);
                    notice.IssuedDate = data.IssuedDate?.ToString("MM/dd/yyyy");
                    notice.LastUpdatedDate = data.LastUpdatedDate.ToString("MM/dd/yyyy");
                    notice.NoticeDescription = data.NoticeDescription;
                    notice.UpdatedBy = data.UpdatedBy;
                    notice.ResourceGuid = data.ResourceGuid;
                    notice.ContractNoticeGuid = data.ContractNoticeGuid;
                    notice.NoticeType = data.NoticeType;
                    notice.Resolution = data.Resolution;
                    list.Add(notice);
                }
                return Ok(new { result = list, count = _contractNoticeService.GetNoticeCount(contractGuid) });
            }

            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.ContractNotice, ResourceActionPermission.List)]
        public ActionResult GetAttachments(Guid resourceGuid)
        {
            try
            {
                var contractResourceFile = _contractNoticeService.GetAttachmentsByResourceGuid(resourceGuid);

                return PartialView("Attachments", contractResourceFile);
            }

            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.Add)]
        public ActionResult Add(Guid contractGuid)
        {

            var contractNoticeViewModel = new ContractNoticeViewModel();
            try
            {
                ViewBag.noticeTypes = GetComboData("NoticeContract", "NoticeType");
                ViewBag.resolutions = GetComboData("NoticeContract", "Resolution");
                var contractInfo = _contractService.GetInfoByContractGuid(contractGuid);
                contractNoticeViewModel.ContractGuid = contractGuid;
                contractNoticeViewModel.ContractTitle = contractInfo.ContractTitle;
                contractNoticeViewModel.ProjectNumber = contractInfo.ProjectNumber;
                contractNoticeViewModel.ContractNumber = contractInfo.ContractNumber;
                contractNoticeViewModel.ActionItem = "Add";
                ViewBag.Resourcekey = EnumGlobal.ResourceType.Contract.ToString();
                ViewBag.ContractResourceFileKey = ContractResourceFileKey.RFI.ToString();
                return PartialView(contractNoticeViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractNoticeViewModel);
            }
        }

        [HttpPost]
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.Add)]
        public IActionResult Add([FromBody]ContractNoticeViewModel contractNoticeModel)
        {
            List<string> filePath = new List<string>();
            Guid id = Guid.NewGuid();
            ContractNotice model = new ContractNotice();
            model.ContractNoticeGuid = id;
            model.IssuedDate = contractNoticeModel.IssuedDate;
            model.LastUpdatedDate = DateTime.Now;
            model.NoticeType = contractNoticeModel.NoticeType;
            model.Resolution = contractNoticeModel.Resolution;
            model.ResourceGuid = contractNoticeModel.ContractGuid;
            model.NoticeDescription = contractNoticeModel.NoticeDescription;
            model.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
            _contractNoticeService.Add(model);
            var data = _contractNoticeService.GetParentId(ContractResourceFileKey.RFI.ToString(), model.ResourceGuid);
            return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", istriggered = false, contractGuid = contractNoticeModel.ContractGuid, resourceId = contractNoticeModel.ContractGuid, ContentResourceGuid = id, uploadPath = data?.FilePath, parentid = data?.ContractResourceFileGuid, contractResourceFileKey= ContractResourceFileKey.RFI.ToString() });
        }

        [HttpGet]
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {

            var contractNoticeModel = new ContractNoticeViewModel();
            if (id != Guid.Empty)
            {
                ViewBag.noticeTypes = GetComboData("NoticeContract", "NoticeType");
                ViewBag.resolutions = GetComboData("NoticeContract", "Resolution");
                var contractNoticeEntity = _contractNoticeService.GetDetailById(id);
                contractNoticeModel = _mapper.Map<ContractNoticeViewModel>(contractNoticeEntity);
                contractNoticeModel.ContractGuid = contractNoticeEntity.ResourceGuid;
                var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.Displayname;
                ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
                ViewBag.ResourceId = contractNoticeEntity.ContractNoticeGuid;
                contractNoticeModel.ContractNoticeGuid = contractNoticeEntity.ContractNoticeGuid;
                ViewBag.Resourcekey = EnumGlobal.ResourceType.Contract.ToString();
                ViewBag.ContractResourceFileKey = ContractResourceFileKey.RFI.ToString();
            }
            try
            {
                return PartialView(contractNoticeModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractNoticeModel);
            }
        }

        [HttpPost]
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.Edit)]
        public IActionResult Edit([FromBody]ContractNoticeViewModel contractNoticeModel)
        {
            List<string> filePath = new List<string>();
            Guid id = Guid.NewGuid();
            ContractNotice model = new ContractNotice();
            model.ContractNoticeGuid = id;
            model.IssuedDate = contractNoticeModel.IssuedDate;
            model.LastUpdatedDate = DateTime.Now;
            model.NoticeType = contractNoticeModel.NoticeType;
            model.Resolution = contractNoticeModel.Resolution;
            model.ResourceGuid = contractNoticeModel.ContractGuid;
            model.NoticeDescription = contractNoticeModel.NoticeDescription;
            model.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
            var data = _contractNoticeService.GetParentId(ContractResourceFileKey.RFI.ToString(), model.ResourceGuid);
            _contractNoticeService.Add(model);
            return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", istriggered = false, contractGuid = contractNoticeModel.ContractGuid, resourceId = contractNoticeModel.ContractGuid, ContentResourceGuid = id, uploadPath = data?.FilePath, parentid = data?.ContractResourceFileGuid, contractResourceFileKey = ContractResourceFileKey.RFI.ToString() });
         }

        [HttpGet]
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.Details)]
        public IActionResult Details(Guid id)
        {
            var contractNoticeViewModel = new ContractNoticeViewModel();
            var contractInfo = _contractNoticeService.GetDetailById(id);

            contractNoticeViewModel.ContractGuid = contractInfo.ResourceGuid;
            contractNoticeViewModel.NoticeType = contractInfo.NoticeType;
            contractNoticeViewModel.Resolution = contractInfo.Resolution;
            contractNoticeViewModel.LastUpdatedDate = contractInfo.LastUpdatedDate;
            contractNoticeViewModel.IssuedDate = contractInfo.IssuedDate;

            return PartialView(contractNoticeViewModel);


        }

        [HttpGet]
        [Secure(ResourceType.ContractNotice, ResourceActionPermission.List)]
        public ActionResult GetDetailsByNoticeType(string Noticetype, Guid ResourceId, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var noticeDetails = _contractNoticeService.GetDetailsByNoticeType(Noticetype, ResourceId,  searchValue,  pageSize,  skip,  take,  sortField,  dir);

            var userName = "";
            List<ContractNoticeViewModel> lists = new List<ContractNoticeViewModel>();
            foreach (var data in noticeDetails)
            {
                ContractNoticeViewModel vm = new ContractNoticeViewModel();
                var userDetails = _userService.GetUserByUserGuid(data.UpdatedBy);
                if (userDetails != null)
                {
                    userName = userDetails.DisplayName;
                }
                vm.Attachment = HttpUtility.HtmlDecode(data.Attachment);
                vm.UpdatedBy = userName;
                vm.NoticeDescription = data.NoticeDescription;
                vm.Resolution = data.Resolution;
                vm.LastUpdatedDate = data.LastUpdatedDate;
                lists.Add(vm);
            }
            return Ok(new { result = lists, count = _contractNoticeService.GetNoticeViewDetailsCount(Noticetype, ResourceId) });


        }
        #endregion

        public IActionResult DownloadFile(string id)
        {
            
            var file = new FileInfo(documentRoot + id);
            var fileProvider = new FileExtensionContentTypeProvider();
            if (!fileProvider.TryGetContentType(file.Extension, out string contentType))
            {
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {file.Extension}.");
            }

            return File(new FileStream(documentRoot + id, FileMode.Open, FileAccess.Read), contentType, file.Name);
        }

        private List<SelectListItem> GetComboData(string resourceType, string name)
        {
            var comboListData = new List<SelectListItem>();
            var comboData = _resourceAttributeValueService.GetResourceValuesByResourceType(resourceType, name);
            if (comboData != null && comboData.Count() > 0)
            {
                comboListData = comboData.Select(data => new SelectListItem { Text = data.Name, Value = data.Value }).ToList();
            }

            return comboListData;

        }


    }
}
