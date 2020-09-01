using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Interfaces;
using Northwind.Web.Models.ViewModels;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class StatesController : Controller
    {
        private readonly IStateService _stateService;
        public StatesController(IStateService stateService)
        { _stateService = stateService;
        }
        [HttpGet]
        public JsonResult GetStatesByCountryId(Guid countryId)
        {
            try
            {
                var states = _stateService.GetStateByCountryGuid(countryId);
                var stateList = states.Select(x => new
                { Keys = x.StateId, Values = x.GRT == true ? x.StateName + " (GRT)" : x.StateName });
                var model = (from c in stateList select new KeyValuePairModel<Guid, string> { Keys = c.Keys, Values = c.Values }).ToList();
                return Json(model);
            }
            catch (Exception)
            {
                return Json("");
            }
        }
        [HttpGet]
        public JsonResult GetStatesByAbbreviation(string Abbreviation)
        {
            try
            {
                var states = _stateService.GetStateByAbbreviation(Abbreviation);
                var model = (from c in states select new KeyValuePairModel<Guid, string> { Keys = c.StateId, Values = c.StateName }).FirstOrDefault();
                return Json(model);
            }
            catch (Exception)
            {
                return Json("");
            }
        }
    }
}