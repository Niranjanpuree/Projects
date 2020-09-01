using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.Shared;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Web.ViewModelMappers;

namespace AutoCare.Product.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("vehicleToBodyStyleConfigSearch")]
    public class VehicleToBodyStyleConfigSearchController : ApiControllerBase
    {
        private readonly IVehicleToBodyStyleConfigSearchService _vehicleToBodyStyleConfigSearchService;
        private readonly IVehicleToBodyStyleConfigSearchViewModelMapper _vehicleToBodyStyleConfigSearchViewModelMapper;
        private readonly IVehicleToBodyStyleConfigViewModelMapper _vehicleToBodyStyleConfigViewModelMapper;

        public VehicleToBodyStyleConfigSearchController(
            IVehicleToBodyStyleConfigSearchService vehicleToBodyStyleConfigSearchService,
            IVehicleToBodyStyleConfigSearchViewModelMapper vehicleToBodyStyleConfigSearchViewModelMapper,
            IVehicleToBodyStyleConfigViewModelMapper vehicleToBodyStyleConfigViewModelMapper)
        {
            _vehicleToBodyStyleConfigSearchService = vehicleToBodyStyleConfigSearchService;
            _vehicleToBodyStyleConfigSearchViewModelMapper = vehicleToBodyStyleConfigSearchViewModelMapper;
            _vehicleToBodyStyleConfigViewModelMapper = vehicleToBodyStyleConfigViewModelMapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleToBodyStyleConfigSearchViewModel> Search(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                // vehicle
                Makes = vehicleToBodyStyleConfigSearchInputModel.Makes,
                Models = vehicleToBodyStyleConfigSearchInputModel.Models,
                SubModels = vehicleToBodyStyleConfigSearchInputModel.SubModels,
                StartYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.EndYear),
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                // body
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
                // others
                VehicleTypes = vehicleToBodyStyleConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups
            };

            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            // vehicle
                            "makeName,count:1000",
                            "modelName,count:1000",
                            "subModelName,count:1000",
                            "regionName,count:1000",
                            // body
                            "bodyNumDoors,count:1000",
                            "bodyTypeName,count:1000",
                            // others
                            "vehicleTypeName,count:1000",
                            "vehicleTypeGroupName,count:1000",
                        },
                        RecordCount = 1000,
                        ReturnTotalCount = true
                    });

            var viewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return viewModel;
        }

        [HttpPost]
        [Route("vehicle")]
        public async Task<List<VehicleToBodyStyleConfigViewModel>> Search(List<string> vehicleIdArray)
        {
            List<VehicleToBodyStyleConfigViewModel> vehicleToBodyStyleConfigs = null;
            if (vehicleIdArray != null && vehicleIdArray.Count > 0)
            {
                var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
                {
                    VehicleIds = vehicleIdArray.Select(item => Convert.ToInt32(item)).ToArray()
                };

                var result =
                    await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                        new SearchOptions
                        {
                            RecordCount = 1000
                        });

                vehicleToBodyStyleConfigs = _vehicleToBodyStyleConfigViewModelMapper.Map(result);

            }
            return vehicleToBodyStyleConfigs;
        }

        [HttpGet]
        [Route("bodyStyleConfig/{bodyStyleConfigId:int}")]
        public async Task<VehicleToBodyStyleConfigSearchViewModel> SearchByBodyStyleConfigId(int bodyStyleConfigId)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = bodyStyleConfigId,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            // vehicle
                            "makeName,count:1000",
                            "modelName,count:1000",
                            "subModelName,count:1000",
                            "regionName,count:1000",
                            // body
                            "bodyStyleConfigId",
                            "bodyNumDoors,count:1000",
                            "bodyTypeName,count:1000",
                            // others
                            "vehicleTypeName,count:1000",
                            "vehicleTypeGroupName,count:1000"
                        },
                        RecordCount = 1000
                    });
            var bodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return bodyStyleConfigSearchViewModel;
        }

        [HttpPost]
        [Route("associations")]
        public async Task<List<VehicleToBodyStyleConfigViewModel>> GetAssociations(VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            List<VehicleToBodyStyleConfigViewModel> vehicleToBodyStyleConfigs = new List<VehicleToBodyStyleConfigViewModel>();

            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                StartYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.EndYear),
                Makes = vehicleToBodyStyleConfigSearchInputModel.Makes,
                Models = vehicleToBodyStyleConfigSearchInputModel.Models,
                SubModels = vehicleToBodyStyleConfigSearchInputModel.SubModels,
                VehicleTypes = vehicleToBodyStyleConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
            };

            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var result = await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                if (result != null && result.Documents != null && result.Documents.Any())
                {
                    vehicleToBodyStyleConfigs.AddRange(_vehicleToBodyStyleConfigViewModelMapper.Map(result));

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            return vehicleToBodyStyleConfigs;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<VehicleToBodyStyleConfigSearchViewModel> RefreshFacets(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var vehicleToBodyStyleConfigSearchViewModel = new VehicleToBodyStyleConfigSearchViewModel
            {
                Facets = new VehicleToBodyStyleConfigSearchFacets
                {
                    // facets precedence
                    Regions = await this.RefreshRegionFacet(vehicleToBodyStyleConfigSearchInputModel),
                    VehicleTypeGroups =
                        await this.RefreshVehicleTypeGroupFacet(vehicleToBodyStyleConfigSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleToBodyStyleConfigSearchInputModel),
                    Years = await RefreshYearFacet(vehicleToBodyStyleConfigSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleToBodyStyleConfigSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleToBodyStyleConfigSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleToBodyStyleConfigSearchInputModel),

                    BodyNumDoors = await RefreshBodyNumDoorsFacet(vehicleToBodyStyleConfigSearchInputModel),
                    BodyTypes = await RefreshBodyTypesFacet(vehicleToBodyStyleConfigSearchInputModel)
                }
            };
            return vehicleToBodyStyleConfigSearchViewModel;
        }

        private async Task<string[]> RefreshRegionFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "regionName,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "vehicleTypeGroupName,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups,
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "vehicleTypeName,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleToBodyStyleConfigSearchInputModel.VehicleTypes,
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "yearId,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                VehicleTypes = vehicleToBodyStyleConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups,
                StartYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.EndYear),
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "makeName,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                VehicleTypes = vehicleToBodyStyleConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups,
                StartYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.EndYear),
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
                Makes = vehicleToBodyStyleConfigSearchInputModel.Makes,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "modelName,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
                BodyStyleConfigId = vehicleToBodyStyleConfigSearchInputModel.BodyStyleConfigId,
                Regions = vehicleToBodyStyleConfigSearchInputModel.Regions,
                VehicleTypes = vehicleToBodyStyleConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBodyStyleConfigSearchInputModel.VehicleTypeGroups,
                StartYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBodyStyleConfigSearchInputModel.EndYear),
                BodyNumDoors = vehicleToBodyStyleConfigSearchInputModel.BodyNumDoors,
                BodyTypes = vehicleToBodyStyleConfigSearchInputModel.BodyTypes,
                Makes = vehicleToBodyStyleConfigSearchInputModel.Makes,
                Models = vehicleToBodyStyleConfigSearchInputModel.Models,
            };
            var result =
                await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        FacetsToInclude = new List<string>
                        {
                            "subModelName,count:1000",
                        },
                        RecordCount = 0
                    });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.SubModels;
        }

        private async Task<string[]> RefreshBodyNumDoorsFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
            };
            var result = await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "bodyNumDoors,count:1000",
                    },
                    RecordCount = 0
                });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.BodyNumDoors;
        }

        private async Task<string[]> RefreshBodyTypesFacet(
            VehicleToBodyStyleConfigSearchInputModel vehicleToBodyStyleConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBodyStyleConfigSearchFilters()
            {
            };
            var result = await _vehicleToBodyStyleConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "bodyTypeName,count:1000",
                    },
                    RecordCount = 0
                });
            var vehicleToBodyStyleConfigSearchViewModel = _vehicleToBodyStyleConfigSearchViewModelMapper.Map(result);
            return vehicleToBodyStyleConfigSearchViewModel.Facets.BodyTypes;
        }
    }
}