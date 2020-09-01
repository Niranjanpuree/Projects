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
    [RoutePrefix("vehicleToBrakeConfigSearch")]
    public class VehicleToBrakeConfigSearchController : ApiController
    {
        private readonly IVehicleToBrakeConfigSearchService _vehicleToBrakeConfigSearchService;
        private readonly IVehicleToBrakeConfigSearchViewModelMapper _vehicleToBrakeConfigSearchViewModelMapper;
        private readonly IVehicleToBrakeConfigViewModelMapper _vehicleToBrakeConfigViewModelMapper;

        public VehicleToBrakeConfigSearchController(IVehicleToBrakeConfigSearchService vehicleToBrakeConfigSearchService,
            IVehicleToBrakeConfigSearchViewModelMapper vehicleToBrakeConfigSearchViewModelMapper, IVehicleToBrakeConfigViewModelMapper vehicleToBrakeConfigViewModelMapper)
        {
            _vehicleToBrakeConfigSearchService = vehicleToBrakeConfigSearchService;
            _vehicleToBrakeConfigSearchViewModelMapper = vehicleToBrakeConfigSearchViewModelMapper;
            _vehicleToBrakeConfigViewModelMapper = vehicleToBrakeConfigViewModelMapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleToBrakeConfigSearchViewModel> Search(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                StartYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.EndYear),
                Makes = vehicleToBrakeConfigSearchInputModel.Makes,
                Models = vehicleToBrakeConfigSearchInputModel.Models,
                SubModels = vehicleToBrakeConfigSearchInputModel.SubModels,
                VehicleTypes = vehicleToBrakeConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "frontBrakeTypeName,count:1000",
                        "rearBrakeTypeName,count:1000",
                        "brakeABSName,count:1000",
                        "brakeSystemName,count:1000",
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

            var viewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return viewModel;
        }

        [HttpPost]
        [Route("vehicle")]
        public async Task<List<VehicleToBrakeConfigViewModel>> Search(List<string> vehicleIdArray/*string vehicleIds*/)
        {
            List<VehicleToBrakeConfigViewModel> vehicleToBrakeConfigs = null;
            //if (!string.IsNullOrWhiteSpace(vehicleIds))
            if (vehicleIdArray != null && vehicleIdArray.Count > 0)
            {
                var applyFilters = new VehicleToBrakeConfigSearchFilters()
                {
                    VehicleIds = vehicleIdArray.Select(item => Convert.ToInt32(item)).ToArray()
                };

                var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        RecordCount = 1000
                    });

                vehicleToBrakeConfigs = _vehicleToBrakeConfigViewModelMapper.Map(result);

            }
            return vehicleToBrakeConfigs;
        }

        [HttpGet]
        [Route("brakeConfig/{brakeConfigId:int}")]
        public async Task<VehicleToBrakeConfigSearchViewModel> SearchByBrakeConfigId(int brakeConfigId)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = brakeConfigId,
            };
            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
            new SearchOptions
            {
                FacetsToInclude = new List<string>
                    {
                        "frontBrakeTypeName",
                        "rearBrakeTypeName",
                        "brakeABSName",
                        "brakeSystemName",
                        "regionName",
                        "vehicleTypeName",
                        "vehicleTypeGroupName",
                        "makeName",
                        "modelName",
                        "subModelName",
                        "brakeConfigId"
                    },
                RecordCount = 1000,
                ReturnTotalCount = true,
            });
            var brakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return brakeConfigSearchViewModel;
        }

        [HttpPost]
        [Route("associations")]
        public async Task<List<VehicleToBrakeConfigViewModel>> GetAssociations(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            List<VehicleToBrakeConfigViewModel> vehicleToBrakeConfigs = new List<VehicleToBrakeConfigViewModel>();

            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                StartYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.EndYear),
                Makes = vehicleToBrakeConfigSearchInputModel.Makes,
                Models = vehicleToBrakeConfigSearchInputModel.Models,
                SubModels = vehicleToBrakeConfigSearchInputModel.SubModels,
                VehicleTypes = vehicleToBrakeConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
            };

            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                if (result != null && result.Documents != null && result.Documents.Any())
                {
                    vehicleToBrakeConfigs.AddRange(_vehicleToBrakeConfigViewModelMapper.Map(result));

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            return vehicleToBrakeConfigs;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<VehicleToBrakeConfigSearchViewModel> RefreshFacets(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var vehicleToBrakeConfigSearchViewModel = new VehicleToBrakeConfigSearchViewModel
            {
                Facets = new VehicleToBrakeConfigSearchFacets
                {
                    Regions = await RefreshRegionFacet(vehicleToBrakeConfigSearchInputModel),
                    VehicleTypeGroups = await RefreshVehicleTypeGroupFacet(vehicleToBrakeConfigSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleToBrakeConfigSearchInputModel),
                    Years = await RefreshYearFacet(vehicleToBrakeConfigSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleToBrakeConfigSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleToBrakeConfigSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleToBrakeConfigSearchInputModel),

                    FrontBrakeTypes = await RefreshFrontBrakeTypeFacet(vehicleToBrakeConfigSearchInputModel),
                    RearBrakeTypes = await RefreshRearBrakeTypeFacet(vehicleToBrakeConfigSearchInputModel),
                    BrakeAbs = await RefreshBrakeAbsFacet(vehicleToBrakeConfigSearchInputModel),
                    BrakeSystems = await RefreshBrakeSystemFacet(vehicleToBrakeConfigSearchInputModel),
                }
            };

            return vehicleToBrakeConfigSearchViewModel;
        }

        private async Task<string[]> RefreshRegionFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeGroupName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleToBrakeConfigSearchInputModel.VehicleTypes,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "yearId,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                StartYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.EndYear),
                VehicleTypes = vehicleToBrakeConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "makeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                StartYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.EndYear),
                Makes = vehicleToBrakeConfigSearchInputModel.Makes,
                VehicleTypes = vehicleToBrakeConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "modelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
                BrakeConfigId = vehicleToBrakeConfigSearchInputModel.BrakeConfigId,
                StartYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBrakeConfigSearchInputModel.EndYear),
                Makes = vehicleToBrakeConfigSearchInputModel.Makes,
                Models = vehicleToBrakeConfigSearchInputModel.Models,
                VehicleTypes = vehicleToBrakeConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBrakeConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBrakeConfigSearchInputModel.Regions,
                FrontBrakeTypes = vehicleToBrakeConfigSearchInputModel.FrontBrakeTypes,
                RearBrakeTypes = vehicleToBrakeConfigSearchInputModel.RearBrakeTypes,
                BrakeAbs = vehicleToBrakeConfigSearchInputModel.BrakeAbs,
                BrakeSystem = vehicleToBrakeConfigSearchInputModel.BrakeSystems,
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "subModelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.SubModels;
        }

        private async Task<string[]> RefreshFrontBrakeTypeFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "frontBrakeTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.FrontBrakeTypes;
        }

        private async Task<string[]> RefreshRearBrakeTypeFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "rearBrakeTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.RearBrakeTypes;
        }

        private async Task<string[]> RefreshBrakeAbsFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "brakeABSName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.BrakeAbs;
        }

        private async Task<string[]> RefreshBrakeSystemFacet(VehicleToBrakeConfigSearchInputModel vehicleToBrakeConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBrakeConfigSearchFilters()
            {
            };

            var result = await _vehicleToBrakeConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "brakeSystemName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBrakeConfigSearchViewModel = _vehicleToBrakeConfigSearchViewModelMapper.Map(result);
            return vehicleToBrakeConfigSearchViewModel.Facets.BrakeSystems;
        }
    }
}
