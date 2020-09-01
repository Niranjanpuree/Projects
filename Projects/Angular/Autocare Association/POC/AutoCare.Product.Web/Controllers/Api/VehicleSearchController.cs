using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.Shared;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Web.ViewModelMappers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicleSearch")]
    public class VehicleSearchController : ApiController
    {
        private readonly IVehicleSearchService _vehicleSearchService;
        private readonly IVehicleSearchViewModelMapper _vehicleSearchViewModelMapper;

        public VehicleSearchController(IVehicleSearchService vehicleSearchService,
            IVehicleSearchViewModelMapper vehicleSearchViewModelMapper)
        {
            _vehicleSearchService = vehicleSearchService;
            _vehicleSearchViewModelMapper = vehicleSearchViewModelMapper;
        }

        [HttpGet]
        [Route("baseVehicle/{baseVehicleId:int}")]
        public async Task<VehicleSearchViewModel> SearchByBaseVehicleId(int baseVehicleId)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                BaseVehicleId = baseVehicleId,
            };
            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName",
                        "vehicleTypeName",
                        "vehicleTypeGroupName",
                        "makeName",
                        "modelName,count:20",
                        "subModelName,count:20"
                    },
                    RecordCount = 1000
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel;
        }

        [HttpGet]
        [Route("vehicle/{vehicleId:int}")]
        public async Task<VehicleSearchViewModel> SearchByVehicleId(string vehicleId)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                VehicleId = vehicleId,
            };
            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName",
                        "vehicleTypeName",
                        "vehicleTypeGroupName",
                        "makeName",
                        "modelName,count:20",
                        "subModelName,count:20"
                    },
                    RecordCount = 1000
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleSearchViewModel> Search(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                StartYear = Convert.ToInt32(vehicleSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleSearchInputModel.EndYear),
                Makes = vehicleSearchInputModel.Makes,
                Models = vehicleSearchInputModel.Models,
                SubModels = vehicleSearchInputModel.SubModels,
                VehicleTypes = vehicleSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleSearchInputModel.VehicleTypeGroups,
                Regions = vehicleSearchInputModel.Regions
            };
            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                        "vehicleTypeName,count:1000",
                        "vehicleTypeGroupName,count:1000",
                        "makeName,count:1000",
                        "modelName,count:1000",
                        "subModelName,count:1000"
                    },
                    RecordCount = 1000,
                    ReturnTotalCount = true,
                    //NOTE: default sorting by base id desc is done in client side
                    //OrderBy = new List<string>()
                    //{
                    //    "baseVehicleId desc",
                    //}
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<VehicleSearchViewModel> RefreshFacets(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var vehicleSearchViewModel = new VehicleSearchViewModel
            {
                Facets = new VehicleSearchFacets
                {
                    Regions = await RefreshRegionFacet(vehicleSearchInputModel),
                    VehicleTypeGroups = await RefreshVehicleTypeGroupFacet(vehicleSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleSearchInputModel),
                    Years = await RefreshYearFacet(vehicleSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleSearchInputModel)
                }
            };

            return vehicleSearchViewModel;
        }

        private async Task<string[]> RefreshRegionFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                Regions = vehicleSearchInputModel.Regions
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeGroupName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                Regions = vehicleSearchInputModel.Regions,
                VehicleTypeGroups = vehicleSearchInputModel.VehicleTypeGroups
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                Regions = vehicleSearchInputModel.Regions,
                VehicleTypeGroups = vehicleSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleSearchInputModel.VehicleTypes
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "yearId,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                StartYear = Convert.ToInt32(vehicleSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleSearchInputModel.EndYear),
                VehicleTypes = vehicleSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleSearchInputModel.VehicleTypeGroups,
                Regions = vehicleSearchInputModel.Regions
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "makeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                StartYear = Convert.ToInt32(vehicleSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleSearchInputModel.EndYear),
                Makes = vehicleSearchInputModel.Makes,
                VehicleTypes = vehicleSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleSearchInputModel.VehicleTypeGroups,
                Regions = vehicleSearchInputModel.Regions
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "modelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(VehicleSearchInputModel vehicleSearchInputModel)
        {
            var applyFilters = new VehicleSearchFilters()
            {
                StartYear = Convert.ToInt32(vehicleSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleSearchInputModel.EndYear),
                Makes = vehicleSearchInputModel.Makes,
                Models = vehicleSearchInputModel.Models,
                VehicleTypes = vehicleSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleSearchInputModel.VehicleTypeGroups,
                Regions = vehicleSearchInputModel.Regions
            };

            var result = await _vehicleSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "subModelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleSearchViewModel = _vehicleSearchViewModelMapper.Map(result);
            return vehicleSearchViewModel.Facets.SubModels;
        }
    }
}
