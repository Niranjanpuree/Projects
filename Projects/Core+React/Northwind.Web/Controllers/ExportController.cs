using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        IResourceAttributeService _resourceAttribute;

        public ExportController(IResourceAttributeService resourceAttribute)
        {
            _resourceAttribute = resourceAttribute;
        }

        [HttpGet("/Export/{resource}")]
        public IActionResult Index(string resource)
        {
            var lstAttr = new List<GridviewField>();
            try
            {
                var lstAttributes = (List<ResourceAttribute>)_resourceAttribute.GetByResourceToExport(resource);

                lstAttr = lstAttributes.Select(c => new GridviewField
                {
                    FieldLabel = c.Title,
                    FieldName = c.Name == c.Name.ToUpper() ? c.Name.ToLower() : c.Name.Substring(0, 1).ToLower() + c.Name.Substring(1),
                }).ToList();
                return PartialView("ReportFields", lstAttr);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView("ReportFields", lstAttr);
            }
        }

        [HttpOptions("/GridFields/{resource}")]
        public IActionResult GridFieldOptions(string resource)
        {
            return Ok();
        }

        [HttpGet("/GridFields/{resource}")]
        public IActionResult GridFields(string resource)
        {
            var lstAttr = new List<GridviewField>();
            try
            {
                var lstAttributes = (List<ResourceAttribute>)_resourceAttribute.GetByResourceToGrid(resource);
                if (resource.ToUpper() != "USER")
                {
                    lstAttributes = lstAttributes.FindAll(c => c.VisibleToGrid == true || c.Exportable == true).ToList();
                }

                lstAttr = lstAttributes.Select(c => new GridviewField
                {
                    FieldLabel = c.Title,
                    FieldName = c.Name == c.Name.ToUpper() ? c.Name.ToLower() : c.Name.Substring(0, 1).ToLower() + c.Name.Substring(1),
                    Clickable = true,
                    IsDefaultSortField = c.DefaultSortField,
                    IsFilterable = true,
                    ColumnMinimumWidth = c.ColumnMinimumWidth,
                    ColumnWidth = c.ColumnWidth,
                    IsSortable = true,
                    OrderIndex = c.GridFieldOrder,
                    visibleToGrid = c.VisibleToGrid,
                    HelpText = c.HelpText,
                    GridColumnCss = c.GridColumnCss,
                    Format = c.GridColumnFormat //c.Name.ToLower().Equals("awardamount") ? "{0:#,###.00}" : "{0:%s}"
                }).ToList();
                return Ok(lstAttr);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
    }
}