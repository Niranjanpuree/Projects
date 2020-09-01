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
    [RoutePrefix("vehicleToBedConfigSearch")]
    public class VehicleToBedConfigSearchController : ApiController
    {
        private readonly IVehicleToBedConfigSearchService _vehicleToBedConfigSearchService;
        private readonly IVehicleToBedConfigSearchViewModelMapper _vehicleToBedConfigSearchViewModelMapper;
        private readonly IVehicleToBedConfigViewModelMapper _vehicleToBedConfigViewModelMapper;

        public VehicleToBedConfigSearchController(IVehicleToBedConfigSearchService vehicleToBedConfigSearchService,
            IVehicleToBedConfigSearchViewModelMapper vehicleToBedConfigSearchViewModelMapper, IVehicleToBedConfigViewModelMapper vehicleToBedConfigViewModelMapper)
        {
            _vehicleToBedConfigSearchService = vehicleToBedConfigSearchService;
            _vehicleToBedConfigSearchViewModelMapper = vehicleToBedConfigSearchViewModelMapper;
            _vehicleToBedConfigViewModelMapper = vehicleToBedConfigViewModelMapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleToBedConfigSearchViewModel> Search(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                StartYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.EndYear),
                Makes = vehicleToBedConfigSearchInputModel.Makes,
                Models = vehicleToBedConfigSearchInputModel.Models,
                SubModels = vehicleToBedConfigSearchInputModel.SubModels,
                VehicleTypes = vehicleToBedConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "bedLength,count:1000",
                        "bedTypeName,count:1000",
                        "regionName,count:1000",
                        "vehicleTypeName,count:1000",
                        "vehicleTypeGroupName,count:1000",
                        "makeName,count:1000",
                        "modelName,count:1000",
                        "subModelName,count:1000",
                    },
                    RecordCount = 1000,
                    ReturnTotalCount = true
                });

            var viewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return viewModel;
        }

        [HttpPost]
        [Route("vehicle")]
        public async Task<List<VehicleToBedConfigViewModel>> Search(List<string> vehicleIdArray)
        {
            List<VehicleToBedConfigViewModel> vehicleToBedConfigs = null;
            if (vehicleIdArray != null && vehicleIdArray.Count > 0)
            {
                var applyFilters = new VehicleToBedConfigSearchFilters()
                {
                    VehicleIds = vehicleIdArray.Select(item => Convert.ToInt32(item)).ToArray()
                };

                var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        RecordCount = 1000
                    });

                vehicleToBedConfigs = _vehicleToBedConfigViewModelMapper.Map(result);

            }
            return vehicleToBedConfigs;
        }

        [HttpGet]
        [Route("bedConfig/{bedConfigId:int}")]
        public async Task<VehicleToBedConfigSearchViewModel> SearchByBedConfigId(int bedConfigId)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = bedConfigId,
            };
            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
            new SearchOptions
            {
                FacetsToInclude = new List<string>
                    {
                        "bedLength",
                        "bedTypeName",
                        "regionName",
                        "vehicleTypeName",
                        "vehicleTypeGroupName",
                        "makeName",
                        "modelName",
                        "subModelName",
                        "bedConfigId"
                    },
                RecordCount = 1000
            });
            var bedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return bedConfigSearchViewModel;
        }

        [HttpPost]
        [Route("associations")]
        public async Task<List<VehicleToBedConfigViewModel>> GetAssociations(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            List<VehicleToBedConfigViewModel> vehicleToBedConfigs = new List<VehicleToBedConfigViewModel>();

            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                StartYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.EndYear),
                Makes = vehicleToBedConfigSearchInputModel.Makes,
                Models = vehicleToBedConfigSearchInputModel.Models,
                SubModels = vehicleToBedConfigSearchInputModel.SubModels,
                VehicleTypes = vehicleToBedConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
            };

            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                if (result != null && result.Documents != null && result.Documents.Any())
                {
                    vehicleToBedConfigs.AddRange(_vehicleToBedConfigViewModelMapper.Map(result));

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            return vehicleToBedConfigs;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<VehicleToBedConfigSearchViewModel> RefreshFacets(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var vehicleToBedConfigSearchViewModel = new VehicleToBedConfigSearchViewModel
            {
                Facets = new VehicleToBedConfigSearchFacets
                {
                    Regions = await RefreshRegionFacet(vehicleToBedConfigSearchInputModel),
                    VehicleTypeGroups = await RefreshVehicleTypeGroupFacet(vehicleToBedConfigSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleToBedConfigSearchInputModel),
                    Years = await RefreshYearFacet(vehicleToBedConfigSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleToBedConfigSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleToBedConfigSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleToBedConfigSearchInputModel),
                    BedLengths = await RefreshBedLengthFacet(vehicleToBedConfigSearchInputModel),
                    BedTypes = await RefreshBedTypeFacet(vehicleToBedConfigSearchInputModel)
                }
            };

            return vehicleToBedConfigSearchViewModel;
        }

        private async Task<string[]> RefreshRegionFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeGroupName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleToBedConfigSearchInputModel.VehicleTypes,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "yearId,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                StartYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.EndYear),
                VehicleTypes = vehicleToBedConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "makeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                StartYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.EndYear),
                Makes = vehicleToBedConfigSearchInputModel.Makes,
                VehicleTypes = vehicleToBedConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "modelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
                BedConfigId = vehicleToBedConfigSearchInputModel.BedConfigId,
                StartYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToBedConfigSearchInputModel.EndYear),
                Makes = vehicleToBedConfigSearchInputModel.Makes,
                Models = vehicleToBedConfigSearchInputModel.Models,
                VehicleTypes = vehicleToBedConfigSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToBedConfigSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToBedConfigSearchInputModel.Regions,
                BedLengths = vehicleToBedConfigSearchInputModel.BedLengths,
                BedTypes = vehicleToBedConfigSearchInputModel.BedTypes
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "subModelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.SubModels;
        }

        private async Task<string[]> RefreshBedLengthFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "bedLength,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.BedLengths;
        }

        private async Task<string[]> RefreshBedTypeFacet(VehicleToBedConfigSearchInputModel vehicleToBedConfigSearchInputModel)
        {
            var applyFilters = new VehicleToBedConfigSearchFilters()
            {
            };

            var result = await _vehicleToBedConfigSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "bedTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToBedConfigSearchViewModel = _vehicleToBedConfigSearchViewModelMapper.Map(result);
            return vehicleToBedConfigSearchViewModel.Facets.BedTypes;
        }
        
    }
}
