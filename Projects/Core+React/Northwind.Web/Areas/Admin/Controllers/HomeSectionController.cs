using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Entities.HomePage;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Core.Specifications;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Article;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeSectionController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        string documentRoot;

        public HomeSectionController(IArticleService articleService, IMapper mapper, IFileService fileService, IConfiguration configuration, IUserService userService)
        {
            _articleService = articleService;
            _mapper = mapper;
            _fileService = fileService;
            _configuration = configuration;
            _userService = userService;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");

        }

        public ActionResult Index()
        {
            ViewBag.ArticleTypes = GetComboData();
            return View();
        }

        public ActionResult AddArticle()
        {
            ViewBag.ArticleTypes = GetComboData();
            return View(new ArticleViewModel());
        }

        public async System.Threading.Tasks.Task<IActionResult> GetArticle(string searchValue, int pageSize, int skip, int take, string sortField, string dir, int showDraft = 0, string additionalFilter ="")
        {
            try
            {
                var searchspec = new SearchSpec();
                searchspec.Dir = dir;
                searchspec.SearchText = searchValue;
                searchspec.PageSize = pageSize;
                searchspec.Skip = skip;
                searchspec.Take = take;
                searchspec.SortField = sortField;
                int articalTypeNumber = 0;
                bool showDraftedArticle = false;
                if(!string.IsNullOrEmpty(additionalFilter))
                {
                    var articalType = _articleService.GetArticleTypeByName(additionalFilter);
                    if (articalType.Result != null)
                    {
                        articalTypeNumber = articalType.Result.ArticleTypeId;
                    }
                }
                if (showDraft > 0)
                    showDraftedArticle = true;

                var articleData = await _articleService.GetArticles(articalTypeNumber, searchspec, showDraftedArticle);

                var result = articleData.Select(x => new
                {
                    Id = x.ArticleId,
                    Title = x.Title,
                    ShowInArchive = (x.ShowInArchive == true ? "Yes" : "No"),
                    IslocalMedia = (x.IsLocalMedia == true ? "Yes" : "No"),
                    Excerpt = x.Excerpt,
                    ArticleTypeName = x.ArticleTypeName,
                    IsFeatured = (x.IsFeatured == true ? "Yes" : "No"),
                    Body = x.Body,
                    MediaCaption = x.MediaCaption,
                    Status = x.Status.ToString(),
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy"),
                    CreatedOn = x.CreatedOn?.ToString("MM/dd/yyyy"),
                    CreatedByName = x.CreatedByName,
                    UpdatedByName =x.UpdatedByName

                }).ToList();

                var totalCount = await _articleService.TotalArticleRecord(searchValue);
                return Ok(new { result, count = totalCount });

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public ActionResult AddUpdate(ArticleViewModel model, IFormFile file, string Submit)
        {
            if (ModelState.IsValid)
            {
                Article article = _mapper.Map<Article>(model);
                if (Request.Form.Files.Count > 0)
                {
                    var fullPhysicalFilePath = string.Concat(documentRoot, "\\Article\\Images");
                    var uniqueFileName =
                         _fileService.SaveFile(fullPhysicalFilePath, file);

                    var relativeFilePath = Path.Combine("Article\\Images", uniqueFileName);
                    article.PrimaryMediaPath = relativeFilePath;
                }
                article.CreatedOn = model.Date;
                article.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                article.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                article.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                if (Submit == "Publish")
                {
                    article.Status = ArticleStatus.Published;
                }
                else
                {
                    article.Status = ArticleStatus.Draft;
                }
                article.IsDeleted = false;
                if (model.Action == "Edit")
                {
                    if (Submit == "Update and Publish")
                    {
                        article.Status = ArticleStatus.Published;
                    }
                    _articleService.Update(article);
                }
                else
                {
                    _articleService.Add(article);
                }
                return RedirectToAction("Index");
            }
            ViewBag.ArticleTypes = GetComboData();
            return View( "AddArticle", new ArticleViewModel());
            
        }

        private List<SelectListItem> GetComboData()
        {
            var comboListData = new List<SelectListItem>();
            var comboData = _articleService.GetArticleTypes().Result;
            if (comboData != null && comboData.Count() > 0)
            {
                comboListData = comboData.Select(data => new SelectListItem { Text = data.Name, Value = data.ArticleTypeId.ToString() }).ToList();
            }

            return comboListData;

        }

        public async System.Threading.Tasks.Task<IActionResult> EditArticle(int Id)
        {
          var data = await _articleService.GetArticle(Id);
            ViewBag.ArticleTypes = GetComboData();
            ArticleViewModel articleViewModel = _mapper.Map<ArticleViewModel>(data);
            articleViewModel.Date = data.CreatedOn;
            articleViewModel.Action = "Edit";
            articleViewModel.Body = HttpUtility.HtmlDecode(articleViewModel.Body);
            return  PartialView(articleViewModel);
        }

        public async System.Threading.Tasks.Task<IActionResult> DeleteArticle(int Id)
        {
            Article article = new Article();
            article.ArticleId = Id;
            await _articleService.Delete(article);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult DeleteMultiple([FromBody] int[] ids)
        {
            _articleService.DeleteMultiple(ids);
            return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
        }

        public async System.Threading.Tasks.Task<IActionResult> GetArticlesForHomePage(ArticleTypes articleTypes)
        {
            try
            {
                var articleData = await _articleService.GetArticlesForHomePage(articleTypes);
                return Ok(articleData);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }



    }
}
