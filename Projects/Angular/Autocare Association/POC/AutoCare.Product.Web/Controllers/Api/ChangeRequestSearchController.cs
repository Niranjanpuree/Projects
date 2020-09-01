using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.Shared;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Web.ViewModelMappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;

//using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("changeRequestSearch")]
    public class ChangeRequestSearchController : ApiControllerBase
    {
        private readonly IChangeRequestSearchService _changeRequestSearchService;
        private readonly IChangeRequestSearchViewModelMapper _changeRequestSearchViewModelMapper;
        private readonly IVcdbChangeRequestService _changeRequestApplicationService;

        public ChangeRequestSearchController(IChangeRequestSearchService changeRequestSearchService,
            IChangeRequestSearchViewModelMapper changeRequestSearchViewModelMapper,
            IVcdbChangeRequestService changeRequestApplicationService
            )
        {
            _changeRequestSearchService = changeRequestSearchService;
            _changeRequestSearchViewModelMapper = changeRequestSearchViewModelMapper;
            _changeRequestApplicationService = changeRequestApplicationService;
        }

        [HttpPost]
        [Route("")]
        public async Task<ChangeRequestSearchViewModel> Search(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var applyFilter = new ChangeRequestSearchFilters()
            {
                Statuses = changeRequestSearchInputModel.Statuses,
                ChangeTypes = changeRequestSearchInputModel.ChangeTypes,
                ChangeEntities = changeRequestSearchInputModel.ChangeEntities,
                RequestsBy = changeRequestSearchInputModel.RequestsBy,
                Assignees = changeRequestSearchInputModel.Assignees,
                SubmittedDateFrom = !String.IsNullOrWhiteSpace(changeRequestSearchInputModel.SubmittedDateFrom)? Convert.ToDateTime(changeRequestSearchInputModel.SubmittedDateFrom): (DateTime?) null,
                SubmittedDateTo = !String.IsNullOrWhiteSpace(changeRequestSearchInputModel.SubmittedDateTo)? Convert.ToDateTime(changeRequestSearchInputModel.SubmittedDateTo): (DateTime?) null
            };

            var result = await _changeRequestSearchService.SearchAsync("", applyFilter.ToAzureSearchFilter(), new SearchOptions()
            {
                FacetsToInclude = new List<string>()
                {
                    "changeType",
                    "entity",
                    "statusText",
                    "requestedBy",
                    "assignee"
                },
                RecordCount = 1000,
                OrderBy = new List<string>()
                {
                    "updatedDate desc",
                    "submittedDate desc",
                }
            });
            //return _changeRequestSearchViewModelMapper.Map(result);
            ChangeRequestSearchViewModel mappedResult = _changeRequestSearchViewModelMapper.Map(result);
            mappedResult.CanBulkSubmit = CurrentUser.Roles.Contains(CustomRoles.Admin) ||
                                         CurrentUser.Roles.Contains(CustomRoles.Researcher);
            mappedResult.IsAdmin = CurrentUser.Roles.Contains(CustomRoles.Admin);
            return mappedResult;
        }

        [HttpGet]
        [Route("changeRequest/{changeRequestId}")]
        public async Task<ChangeRequestSearchViewModel> SearchbyChangeRequestId(string changeRequestId)
        {
            var applyFilters = new ChangeRequestSearchFilters()
            {
                ChangeRequestId = changeRequestId,
            };
            var result = await _changeRequestSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "changeType",
                        "entity",
                        "statusText",
                        "requestedBy",
                        "assignee"
                    },
                    RecordCount = 1
                });

            return _changeRequestSearchViewModelMapper.Map(result);
        }

        //TODO: We need to remove this function as ChangeTypes will be extracted from Azure
        [HttpGet]
        [Route("getChangeTypes")]
        public async Task<List<ChangeTypeFacet>> GetChangeTypes()
        {
            var changeList = new List<ChangeTypeFacet>
            {
                new ChangeTypeFacet() {Name = ChangeType.Add.ToString()},
                new ChangeTypeFacet() {Name = ChangeType.Delete.ToString()},
                new ChangeTypeFacet() {Name = ChangeType.Modify.ToString()},
                new ChangeTypeFacet() {Name = ChangeType.Replace.ToString()}
            };

            //List<ChangeTypeFacet> changeTypes = await _changeRequestApplicationService.GetAllChangeTypes();
            return changeList;
        }

        //TODO: We need to remove this function as ChangeEntities~ will be extracted from Azure
        [HttpGet]
        [Route("getChangeEntities")]
        public async Task<List<ChangeEntityFacet>> GetChangeEntites()
        {
            List<ChangeEntityFacet> changeTypes = await _changeRequestApplicationService.GetAllChangeTypes();

            return changeTypes;
        }

        [HttpPost]
        [Route("getAssociatedCount")]
        public async Task<AssociationCount> GetAssociatedCount(List<ChangeRequestAssociationInputModel> selectedChangeRequestStagings)
        {
            List<ChangeRequestStaging> associationLookupForSelectedCR = new List<ChangeRequestStaging>();
            selectedChangeRequestStagings.ForEach(x =>
            {
                associationLookupForSelectedCR.Add(new ChangeRequestStaging()
                {
                    Id = x.Id,
                    Entity = x.Entity
                });
            });
            AssociationCount count = await _changeRequestApplicationService.GetAssociatedCount(associationLookupForSelectedCR);
            return count;
        }

        [HttpPost]
        [Route("assignReviewer")]
        public async Task<IHttpActionResult> AssignReviewer(AssignReviewerInputModel assignReviewer)
        {
            AssignReviewerBusinessModel assignReviewerBusinessModel = new AssignReviewerBusinessModel()
            {
                ChangeRequestIds = assignReviewer.ChangeRequestIds,
                AssignedToRoleId = assignReviewer.AssignedToRoleId,
                AssignedToUserId = assignReviewer.AssignedToUserId,
                AssignedByUserId = CurrentUser.UserName
            };

            var result = await _changeRequestApplicationService.AssignReviewer(assignReviewerBusinessModel);
            return Ok(result);
        }

        [HttpGet]
        [Route("getStatus")]
        public List<ChangeStatusFacet> GetStatus()
        {
            List<ChangeStatusFacet> statusList = (
                Enum.GetValues(
                    typeof(ChangeRequestStatus))
                        .Cast<ChangeRequestStatus>()
                        .Select(v => v.ToString())
                    .ToList())
                .Select((x, i) =>
                new ChangeStatusFacet()
                {
                    Id = i,
                    Status = x
                }).ToList();

            return statusList;
        }

        //TODO: Call shared business service to delegate to respective business service based upon the entity type
        //[HttpPost]
        //[Route("selectedApprove")]
        //public void SelectedApprove(List<ChangeRequestBulkReviewInputModel> selectedCR)
        //{

        //}

        //[HttpPost]
        //[Route("selectedReject")]
        //public void SelectedReject(List<ChangeRequestBulkReviewInputModel> selectedCR)
        //{

        //}

        //[HttpPost]
        //[Route("selectedPreliminary")]
        //public void SelectedPreliminary(List<ChangeRequestBulkReviewInputModel> selectedCR)
        //{

        //}
        [HttpGet]
        [Route("changeRequestId/{changeRequestId}/status/{reviewStatus}")]
        public async Task<List<CommentsStagingModel>> GetRequestorComments(string changeRequestId, string reviewStatus)
        {
            List<CommentsStagingModel> requestorComments = await _changeRequestApplicationService.GetRequestorComments(changeRequestId, reviewStatus);
            return requestorComments;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<ChangeRequestSearchViewModel> RefreshFacets(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var changeRequestSearchViewModel = new ChangeRequestSearchViewModel()
            {
                Facets = new ChangeRequestSearchFacets()
                {
                    Statuses = await RefreshStatusFacet(changeRequestSearchInputModel),
                    ChangeTypes = await RefreshTypeFacet(changeRequestSearchInputModel),
                    ChangeEntities = await RefreshEntityFacet(changeRequestSearchInputModel),
                    RequestsBy = await RefreshRequestByFacet(changeRequestSearchInputModel),
                    Assignees = await RefreshAssigneeFacet(changeRequestSearchInputModel)
                }
            };

            return changeRequestSearchViewModel;
        }

        private async Task<string[]> RefreshStatusFacet(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var applyFilters = new ChangeRequestSearchFilters()
            {
                //NOTE: This is commented based upon the remarks made in email on Sept 15, 2016
                //Assignees = changeRequestSearchInputModel.Assignees,
                //ChangeEntities = changeRequestSearchInputModel.ChangeEntities,
                //RequestsBy = changeRequestSearchInputModel.RequestsBy,
                //ChangeTypes = changeRequestSearchInputModel.ChangeTypes
            };

            var result = await _changeRequestSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "statusText,count:1000",
                    },
                    RecordCount = 0
                });

            var changeRequestSearchViewModel = _changeRequestSearchViewModelMapper.Map(result);
            return changeRequestSearchViewModel.Facets.Statuses;
        }

        private async Task<string[]> RefreshTypeFacet(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var applyFilters = new ChangeRequestSearchFilters()
            {
                Assignees = changeRequestSearchInputModel.Assignees,
                ChangeEntities = changeRequestSearchInputModel.ChangeEntities,
                RequestsBy = changeRequestSearchInputModel.RequestsBy,
                Statuses = changeRequestSearchInputModel.Statuses
            };

            var result = await _changeRequestSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "changeType,count:1000",
                    },
                    RecordCount = 0
                });

            var changeRequestSearchViewModel = _changeRequestSearchViewModelMapper.Map(result);
            return changeRequestSearchViewModel.Facets.ChangeTypes;
        }

        private async Task<string[]> RefreshEntityFacet(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var applyFilters = new ChangeRequestSearchFilters()
            {
                Assignees = changeRequestSearchInputModel.Assignees,
                Statuses = changeRequestSearchInputModel.Statuses,
                RequestsBy = changeRequestSearchInputModel.RequestsBy,
                ChangeTypes = changeRequestSearchInputModel.ChangeTypes
            };

            var result = await _changeRequestSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "entity,count:1000",
                    },
                    RecordCount = 0
                });

            var changeRequestSearchViewModel = _changeRequestSearchViewModelMapper.Map(result);
            return changeRequestSearchViewModel.Facets.ChangeEntities;
        }

        private async Task<string[]> RefreshRequestByFacet(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var applyFilters = new ChangeRequestSearchFilters()
            {
                Assignees = changeRequestSearchInputModel.Assignees,
                ChangeEntities = changeRequestSearchInputModel.ChangeEntities,
                Statuses = changeRequestSearchInputModel.Statuses,
                ChangeTypes = changeRequestSearchInputModel.ChangeTypes
            };

            var result = await _changeRequestSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "requestedBy,count:1000",
                    },
                    RecordCount = 0
                });

            var changeRequestSearchViewModel = _changeRequestSearchViewModelMapper.Map(result);
            return changeRequestSearchViewModel.Facets.RequestsBy;
        }

        private async Task<string[]> RefreshAssigneeFacet(ChangeRequestSearchInputModel changeRequestSearchInputModel)
        {
            var applyFilters = new ChangeRequestSearchFilters()
            {
                RequestsBy = changeRequestSearchInputModel.RequestsBy,
                ChangeEntities = changeRequestSearchInputModel.ChangeEntities,
                Statuses = changeRequestSearchInputModel.Statuses,
                ChangeTypes = changeRequestSearchInputModel.ChangeTypes
            };

            var result = await _changeRequestSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "assignee,count:1000",
                    },
                    RecordCount = 0
                });

            var changeRequestSearchViewModel = _changeRequestSearchViewModelMapper.Map(result);
            return changeRequestSearchViewModel.Facets.Assignees;
        }
    }
}