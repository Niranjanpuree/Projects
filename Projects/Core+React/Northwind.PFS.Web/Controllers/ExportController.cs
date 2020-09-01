using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.PFS.Web.Areas.CostPoint.Controllers
{
    public class ExportController : Controller
    {
        IResourceAttributeService _resourceAttribute;

        public ExportController(IResourceAttributeService resourceAttribute)
        {
            _resourceAttribute = resourceAttribute;
        }

        [HttpGet("Export/{resource}")]
        public IActionResult Export(string resource)
        {
            var lstAttr = new List<GridviewField>();
            try
            {
                var lstAttributes = (List<ResourceAttribute>)_resourceAttribute.GetByResourceToExport(WebUtility.UrlDecode(resource));

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

        [HttpGet("GridFields/{resource}")]
        public IActionResult GridFields(string resource)
        {
            var lstAttr = new List<GridviewField>();
            try
            {
                var lstAttributes = (List<ResourceAttribute>)_resourceAttribute.GetByResourceToGrid(WebUtility.UrlDecode(resource));
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
                    IsSortable = true,
                    OrderIndex = c.GridFieldOrder,
                    visibleToGrid = c.VisibleToGrid,
                    GridColumnCss = c.GridColumnCss,
                    ColumnWidth = c.ColumnWidth,
                    ColumnMinimumWidth = c.ColumnMinimumWidth,
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


