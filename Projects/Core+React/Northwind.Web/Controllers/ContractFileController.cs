using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;
using Northwind.Web.Helpers;
using Northwind.Core.Entities;
using Newtonsoft.Json;
using static Northwind.Web.Models.ViewModels.EnumGlobal;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Controllers
{
    public class ContractFileController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IContractsService _contractRefactorService;
        private readonly IMapper _mapper;
        string documentRoot;

        public ContractFileController(
            IFileService fileService,
            IConfiguration configuration,
            IContractsService contractRefactorService,
            IMapper mapper)
        {
            _fileService = fileService;
            _configuration = configuration;
            _contractRefactorService = contractRefactorService;
            _mapper = mapper;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");
        }

        [HttpGet]
        public ActionResult Add(Guid contractGuid, string formType)
        {
            try
            {
                var fileModel = new ContractFileViewModel();
                Guid contractFileGuid = Guid.NewGuid();
                fileModel.ContractResourceFileGuid = contractFileGuid;
                fileModel.ResourceGuid = contractGuid;
                fileModel.keys = formType;

                return PartialView(fileModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        [HttpGet]
        public ActionResult Detail(Guid contractGuid, string formType)
        {
            var fileEntity = _contractRefactorService.GetFilesByContractGuid(contractGuid, formType);
            var model = _mapper.Map<ContractFileViewModel>(fileEntity);
            try
            {
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult Edit(Guid contractGuid,string formType)
        {
            var fileEntity = _contractRefactorService.GetFilesByContractGuid(contractGuid,formType);
            var model = _mapper.Map<ContractFileViewModel>(fileEntity);
            try
            {
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Get the list of Csv For the Grid list
        /// </summary>
        //[HttpGet]
        //public IActionResult Get(Guid contractGuid,string formType)
        //{
        //    try
        //    {
        //        var fileEntity = _contractRefactorService.GetFilesByContractGuid(contractGuid,formType);
        //        var model = _mapper.Map<ContractFileViewModel>(fileEntity);
        //        var data = CsvValidationHelper.ChecksValidHeaderAndReadTheFile(model.UploadFileName, UploadMethodName.WorkBreakDownStructure);
        //        var listdata = data.Select(x => new
        //        {
        //            ContractGuid = contractGuid,
        //            WBSCode = x.WBSCode,
        //            Description = x.Description,
        //            Value = x.Value,
        //            ContractType = x.ContractType,
        //            InvoiceAtThisLevel = x.InvoiceAtThisLevel
        //        }).ToList();
        //        return Json(new { data = listdata });
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(ex.ToString(), ex.Message);
        //        return BadRequest(ModelState);
        //    }
        //}
        [HttpPost]
        public IActionResult Add([FromForm]ContractFileViewModel fileModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                fileModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                fileModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                fileModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                fileModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                fileModel.IsActive = true;
                fileModel.IsDeleted = false;
                fileModel.IsCsv = true;
                // gets the contractnumber to save the file in the folder.
                var ContractNumber = _contractRefactorService.GetContractNumberById(fileModel.ResourceGuid);
                if (fileModel.FileToUpload != null || fileModel.FileToUpload.Length != 0)
                {
                    //checks whether the file extension is the correct one and the validates the fields if the file is Csv.
                    var isfileValid = _fileService.UploadFileTypeCheck(fileModel.FileToUpload);
                   // var filename = _fileService.FilePostWithCount($@"{documentRoot}/{ContractNumber}/WorkBreakdownStructure/", fileModel.FileToUpload);
                    if (!isfileValid)
                    {
                        fileModel.IsCsv = false;
                    }
                    else
                    {
                        //Helpers.CsvValidationHelper.ChecksValidHeaderAndReadTheFile(filename, UploadMethodName.WorkBreakDownStructure);
                    }
                    //fileModel.UploadFileName = filename;
                }
                //soft delete the previous uploaded files
                _contractRefactorService.DeleteContractFileByContractGuid(fileModel.ResourceGuid, fileModel.keys);

                var fileEntity = _mapper.Map<ContractResourceFile>(fileModel);
                _contractRefactorService.InsertContractFile(fileEntity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult Edit([FromForm]ContractFileViewModel fileModel)
        {
            try
            {
                fileModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                fileModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                fileModel.IsActive = true;
                fileModel.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var fileEntity = _mapper.Map<ContractResourceFile>(fileModel);
                _contractRefactorService.UpdateContractFile(fileEntity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Checks the Inline Edited Grid Data and converts to CSV and saves in the folder. 
        /// </summary>
        //[HttpPost]
        //public IActionResult Get([FromBody] string searchText)
        //{
        //    try
        //    {
        //        var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
        //        var ContractGuid = list[0].ContractGuid;
        //        var model = _contractRefactorService.GetFilesByContractFileGuid(ContractGuid);
        //        string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
        //        var newfilename = FormatHelper.GetNewFileName(fileName);
        //        var listdata = list.Select(x => new
        //        {
        //            WBSCode = x.WBSCode,
        //            Description = x.Description,
        //            Value = x.Value,
        //            ContractType = x.ContractType,
        //            InvoiceAtThisLevel = x.InvoiceAtThisLevel
        //        }).ToList();
        //        var dynamicList = listdata.Cast<dynamic>().ToList();

        //        // saves the files to the folder specified 
        //        Helpers.CsvValidationHelper.SaveTheUpdatedCsv(dynamicList, newfilename);

        //        // after file save the saved file is valid if not valid the file is deleted 
        //        Helpers.CsvValidationHelper.ChecksValidHeaderAndReadTheFile(newfilename, UploadMethodName.WorkBreakDownStructure);

        //        //updates the new filename/path to the database
        //        _contractRefactorService.UpdateFileName(model.ContractResourceFileGuid, newfilename);

        //        return Ok(new { status = ResponseStatus.success.ToString(), path = newfilename });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}