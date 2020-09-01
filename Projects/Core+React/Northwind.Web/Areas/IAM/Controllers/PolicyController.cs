using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Newtonsoft.Json;
using Northwind.Web.Areas.IAM.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Serialization;
using Northwind.Web.Infrastructure.Models;
using System.Collections.Generic;
using Northwind.Web.Infrastructure.Authorization;
using static Northwind.Core.Entities.EnumGlobal;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Areas.Settings.Controllers
{
    [Area("IAM")]
    public class PolicyController : Controller
    {
        private readonly IResourceService _resourceService;
        private readonly IPolicyService _policyService;

        public PolicyController(IResourceService resourceService, IPolicyService policyService)
        {
            _resourceService = resourceService;
            _policyService = policyService;
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Save([FromBody] SavePolicyViewModel policyModel)
        {
            try
            {
                //Camelcase serialization
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var policy = new Policy();
                policy.Name = policyModel.PolicyName;
                policy.Description = policyModel.PolicyDesc;
                //var policyRules = new List<Rule>();
                var policyRules = policyModel.PolicyRules.Select(x =>
                {
                    return new Rule()
                    {
                        Name = "Some Name",
                        Description = "Some Description",
                        RuleId = Guid.NewGuid(),
                        Action = x.Actions.Select(y => y.Name).ToList(),
                        Resource = x.Resources.Select(z => z.Name).ToList(),
                        Effect = x.SelectedEffect,

                    };
                });
                policy.Rules = policyRules.ToList();

                var policyEntity = new PolicyEntity();
                policyEntity.Description = policyModel.PolicyDesc;
                policyEntity.Title = policyModel.PolicyName;
                policyEntity.Name = policyModel.PolicyName;
                policyEntity.PolicyJson = JsonConvert.SerializeObject(policy, settings);
                policyEntity.Policy = policy;
                _policyService.Insert(policyEntity);
                return Ok(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Get(string searchValue, int take, int skip, string sortField, string dir, string additionalFilter)
        {
            //var pdp = new PolicyDecisionPoint(new FakePolicyRepository());
            //var policies = pdp.GetPolicies(Guid.Empty);
            var policies = _policyService.GetPolicyEntities();

            var p = policies.Select(x => new
            {
                PolicyGuid = x.PolicyGuid,
                name = x.Name,
                description = x.Description
            });
            return Json(new { result = p, count = policies.Count() });
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Index()
        {
            return View();
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Add()
        {
            var model = new AddPolicyViewModel();
            var resources = _resourceService.GetAll();
            var listItems = resources.Select(x => new SelectListItem(x.Title, x.ResourceGuid.ToString()));
            model.Resources.AddRange(listItems);
            return View(model);
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Delete(Guid id)
        {
            var policy = _policyService.GetPolicyEntity(id);
            if (policy != null)
            {
                try
                {
                    _policyService.Delete(policy);
                    return Json(new { status = true });
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ModelState.AddModelError("", ex.Message);
                    return BadRequestFormatter.BadRequest(this, ex);
                }
            }
            else
            {
                return Json(new { status = false, message = "Unable to find policy to delete." });
            }
        }

        [HttpPost]
        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult Delete([FromBody]IEnumerable<PolicyEntity> postValues)
        {
            try
            {
                var lst = new List<Policy>();
                var notfound = "";
                foreach (var item in postValues)
                {
                    var policy = _policyService.GetPolicyEntity(item.PolicyGuid);
                    if (policy != null)
                    {
                        try
                        {
                            _policyService.Delete(policy);
                            return Json(new { status = true });
                        }
                        catch (Exception ex)
                        {
                            notfound += $"{ ex.Message }.\r\n";
                        }
                    }
                    else
                    {
                        notfound += $"Unable to find Policy{ item.Name }.\r\n";
                    }
                }

                if (!string.IsNullOrEmpty(notfound))
                {
                    notfound = "Some records are deleted with following errors.\r\n" + notfound;
                    ModelState.Clear();
                    ModelState.AddModelError("", notfound);
                    return BadRequestFormatter.BadRequest(this, notfound);
                }
                else
                {
                    return Json(new { status = true });
                }

            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GetActions(string resourceId)
        {
            if (Guid.TryParse(resourceId, out Guid guid))
            {
                return Json(_resourceService.GetResourceAction(Guid.Parse(resourceId)));
            }
            else
            {
                throw new Exception("Invalid Input for GetActions");
            }
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.List)]
        public IActionResult GetConditionPanel()
        {
            return PartialView("_ConditionsPanel");
        }
    }
}
