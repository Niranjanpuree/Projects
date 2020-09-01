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

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicleToWheelBaseSearch")]
    public class VehicleToWheelBaseSearchController : ApiController
    {
        private readonly IVehicleToWheelBaseSearchService _vehicleToWheelBaseSearchService;
        private readonly IVehicleToWheelBaseSearchViewModelMapper _vehicleToWheelBaseSearchViewModelMapper;
        private readonly IVehicleToWheelBaseViewModelMapper _vehicleToWheelBaseViewModelMapper;

        public VehicleToWheelBaseSearchController(IVehicleToWheelBaseSearchService vehicleToWheelBaseSearchService,
            IVehicleToWheelBaseSearchViewModelMapper vehicleToWheelBaseSearchViewModelMapper, IVehicleToWheelBaseViewModelMapper vehicleToWheelBaseViewModelMapper)
        {
            _vehicleToWheelBaseSearchService = vehicleToWheelBaseSearchService;
            _vehicleToWheelBaseSearchViewModelMapper = vehicleToWheelBaseSearchViewModelMapper;
            _vehicleToWheelBaseViewModelMapper = vehicleToWheelBaseViewModelMapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleToWheelBaseSearchViewModel> Search(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                StartYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.EndYear),
                Makes = vehicleToWheelBaseSearchInputModel.Makes,
                Models = vehicleToWheelBaseSearchInputModel.Models,
                SubModels = vehicleToWheelBaseSearchInputModel.SubModels,
                VehicleTypes = vehicleToWheelBaseSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                        "vehicleTypeName,count:1000",
                        "vehicleTypeGroupName,count:1000",
                        "makeName,count:1000",
                        "modelName,count:1000",
                        "subModelName,count:1000",
                    },
                    RecordCount = 1000,
                    ReturnTotalCount = true,
                });

            var viewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return viewModel;
        }

        [HttpPost]
        [Route("vehicle")]
        public async Task<List<VehicleToWheelBaseViewModel>> Search(List<string> vehicleIdArray)
        {
            List<VehicleToWheelBaseViewModel> vehicleToWheelBases = null;
            if (vehicleIdArray != null && vehicleIdArray.Count > 0)
            {
                var applyFilters = new VehicleToWheelBaseSearchFilters()
                {
                    VehicleIds = vehicleIdArray.Select(item => Convert.ToInt32(item)).ToArray()
                };

                var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        RecordCount = 1000
                    });

                vehicleToWheelBases = _vehicleToWheelBaseViewModelMapper.Map(result);
            }

            return vehicleToWheelBases;
        }

        [HttpGet]
        [Route("WheelBase/{WheelBaseId:int}")]
        public async Task<VehicleToWheelBaseSearchViewModel> SearchByWheelBaseId(int wheelBaseId)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = wheelBaseId,
            };
            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
            new SearchOptions
            {
                FacetsToInclude = new List<string>
                    {
                        "regionName",
                        "vehicleTypeName",
                        "vehicleTypeGroupName",
                        "makeName",
                        "modelName",
                        "subModelName",
                    },
                RecordCount = 1000,
                ReturnTotalCount = true,
            });

            var wheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);

            return wheelBaseSearchViewModel;
        }

        [HttpPost]
        [Route("associations")]
        public async Task<List<VehicleToWheelBaseViewModel>> GetAssociations(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            List<VehicleToWheelBaseViewModel> vehicleToWheelBases = new List<VehicleToWheelBaseViewModel>();

            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                StartYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.EndYear),
                Makes = vehicleToWheelBaseSearchInputModel.Makes,
                Models = vehicleToWheelBaseSearchInputModel.Models,
                SubModels = vehicleToWheelBaseSearchInputModel.SubModels,
                VehicleTypes = vehicleToWheelBaseSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
            };

            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                if (result != null && result.Documents != null && result.Documents.Any())
                {
                    vehicleToWheelBases.AddRange(_vehicleToWheelBaseViewModelMapper.Map(result));

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            return vehicleToWheelBases;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<VehicleToWheelBaseSearchViewModel> RefreshFacets(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var vehicleToWheelBaseSearchViewModel = new VehicleToWheelBaseSearchViewModel
            {
                Facets = new VehicleToWheelBaseSearchFacets
                {
                    Regions = await RefreshRegionFacet(vehicleToWheelBaseSearchInputModel),
                    VehicleTypeGroups = await RefreshVehicleTypeGroupFacet(vehicleToWheelBaseSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleToWheelBaseSearchInputModel),
                    Years = await RefreshYearFacet(vehicleToWheelBaseSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleToWheelBaseSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleToWheelBaseSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleToWheelBaseSearchInputModel),
                }
            };

            return vehicleToWheelBaseSearchViewModel;
        }

        private async Task<string[]> RefreshRegionFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeGroupName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleToWheelBaseSearchInputModel.VehicleTypes,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "yearId,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                StartYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.EndYear),
                VehicleTypes = vehicleToWheelBaseSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "makeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                StartYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.EndYear),
                Makes = vehicleToWheelBaseSearchInputModel.Makes,
                VehicleTypes = vehicleToWheelBaseSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "modelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(VehicleToWheelBaseSearchInputModel vehicleToWheelBaseSearchInputModel)
        {
            var applyFilters = new VehicleToWheelBaseSearchFilters()
            {
                WheelBaseId = vehicleToWheelBaseSearchInputModel.WheelBaseId,
                StartYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToWheelBaseSearchInputModel.EndYear),
                Makes = vehicleToWheelBaseSearchInputModel.Makes,
                Models = vehicleToWheelBaseSearchInputModel.Models,
                VehicleTypes = vehicleToWheelBaseSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToWheelBaseSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToWheelBaseSearchInputModel.Regions,
            };

            var result = await _vehicleToWheelBaseSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "subModelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToWheelBaseSearchViewModel = _vehicleToWheelBaseSearchViewModelMapper.Map(result);
            return vehicleToWheelBaseSearchViewModel.Facets.SubModels;
        }
    }
}
