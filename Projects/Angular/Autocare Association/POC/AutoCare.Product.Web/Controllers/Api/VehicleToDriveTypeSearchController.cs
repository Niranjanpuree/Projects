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
    [RoutePrefix("vehicleToDriveTypeSearch")]
    public class VehicleToDriveTypeSearchController : ApiController
    {
        // GET: VehicleToDriveTypeSearch

        private readonly IVehicleToDriveTypeSearchService _vehicleToDriveTypeSearchService;
        private readonly IVehicleToDriveTypeSearchViewModelMapper _vehicleToDriveTypeSearchViewModelMapper;
        private readonly IVehicleToDriveTypeViewModelMapper _vehicleToDriveTypeViewModelMapper;
        public VehicleToDriveTypeSearchController(IVehicleToDriveTypeSearchService vehicleToDriveTypeSearchService,
            IVehicleToDriveTypeSearchViewModelMapper vehicleToDriveTypeSearchViewModelMapper, IVehicleToDriveTypeViewModelMapper vehicleToDriveTypeViewModelMapper)
        {
            _vehicleToDriveTypeSearchService = vehicleToDriveTypeSearchService;
            _vehicleToDriveTypeSearchViewModelMapper = vehicleToDriveTypeSearchViewModelMapper;
            _vehicleToDriveTypeViewModelMapper = vehicleToDriveTypeViewModelMapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleToDriveTypeSearchViewModel> Search(
            VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                StartYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.EndYear),
                Makes = vehicleToDriveTypeSearchInputModel.Makes,
                Models = vehicleToDriveTypeSearchInputModel.Models,
                SubModels = vehicleToDriveTypeSearchInputModel.SubModels,
                VehicleTypes = vehicleToDriveTypeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes
            };
            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
               new SearchOptions
               {
                   FacetsToInclude = new List<string>
                   {
                        "driveTypeName,count:1000",
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
            var viewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return viewModel;
        }
        [HttpPost]
        [Route("vehicle")]
        public async Task<List<VehicleToDriveTypeViewModel>> Search(List<string> vehicleIdArray/*string vehicleIds*/)
        {
            List<VehicleToDriveTypeViewModel> vehicleToDriveTypes = null;
            //if (!string.IsNullOrWhiteSpace(vehicleIds))
            if (vehicleIdArray != null && vehicleIdArray.Count > 0)
            {
                var applyFilters = new VehicleToDriveTypeSearchFilters()
                {
                    VehicleIds = vehicleIdArray.Select(item => Convert.ToInt32(item)).ToArray()
                };

                var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        RecordCount = 1000
                    });

                vehicleToDriveTypes = _vehicleToDriveTypeViewModelMapper.Map(result);

            }
            return vehicleToDriveTypes;
        }

        [HttpGet]
        [Route("driveType/{driveTypeId:int}")]
        public async Task<VehicleToDriveTypeSearchViewModel> SearchByDriveTypeId(int driveTypeId)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = driveTypeId,
            };
            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
            new SearchOptions
            {
                FacetsToInclude = new List<string>
                    {
                        "driveTypeName",
                        "regionName",
                        "vehicleTypeName",
                        "vehicleTypeGroupName",
                        "makeName",
                        "modelName",
                        "subModelName",
                        "driveTypeId"
                    },
                RecordCount = 1000,
                ReturnTotalCount = true,
            });
            var driveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return driveTypeSearchViewModel;
        }
        [HttpPost]
        [Route("associations")]
        public async Task<List<VehicleToDriveTypeViewModel>> GetAssociations(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            List<VehicleToDriveTypeViewModel> vehicleToDriveTypes = new List<VehicleToDriveTypeViewModel>();

            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                StartYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.EndYear),
                Makes = vehicleToDriveTypeSearchInputModel.Makes,
                Models = vehicleToDriveTypeSearchInputModel.Models,
                SubModels = vehicleToDriveTypeSearchInputModel.SubModels,
                VehicleTypes = vehicleToDriveTypeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
            };

            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                if (result != null && result.Documents != null && result.Documents.Any())
                {
                    vehicleToDriveTypes.AddRange(_vehicleToDriveTypeViewModelMapper.Map(result));

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            return vehicleToDriveTypes;
        }

        [HttpPost]
        [Route("facets")]
        public async Task<VehicleToDriveTypeSearchViewModel> RefreshFacets(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var vehicleToDriveTypeSearchViewModel = new VehicleToDriveTypeSearchViewModel
            {
                Facets = new VehicleToDriveTypeSearchFacets
                {
                    Regions = await RefreshRegionFacet(vehicleToDriveTypeSearchInputModel),
                    VehicleTypeGroups = await RefreshVehicleTypeGroupFacet(vehicleToDriveTypeSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleToDriveTypeSearchInputModel),
                    Years = await RefreshYearFacet(vehicleToDriveTypeSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleToDriveTypeSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleToDriveTypeSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleToDriveTypeSearchInputModel),
                    DriveTypes = await RefreshDriveTypeFacet(vehicleToDriveTypeSearchInputModel),
                }
            };

            return vehicleToDriveTypeSearchViewModel;
        }
        private async Task<string[]> RefreshRegionFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes,
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeGroupName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleToDriveTypeSearchInputModel.VehicleTypes,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes,
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "yearId,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                StartYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.EndYear),
                VehicleTypes = vehicleToDriveTypeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes,
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "makeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                StartYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.EndYear),
                Makes = vehicleToDriveTypeSearchInputModel.Makes,
                VehicleTypes = vehicleToDriveTypeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes,
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "modelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
                DriveTypeId = vehicleToDriveTypeSearchInputModel.DriveTypeId,
                StartYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToDriveTypeSearchInputModel.EndYear),
                Makes = vehicleToDriveTypeSearchInputModel.Makes,
                Models = vehicleToDriveTypeSearchInputModel.Models,
                VehicleTypes = vehicleToDriveTypeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToDriveTypeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToDriveTypeSearchInputModel.Regions,
                DriveTypes = vehicleToDriveTypeSearchInputModel.DriveTypes
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "subModelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.SubModels;
        }
        private async Task<string[]> RefreshDriveTypeFacet(VehicleToDriveTypeSearchInputModel vehicleToDriveTypeSearchInputModel)
        {
            var applyFilters = new VehicleToDriveTypeSearchFilters()
            {
            };

            var result = await _vehicleToDriveTypeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "driveTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToDriveTypeSearchViewModel = _vehicleToDriveTypeSearchViewModelMapper.Map(result);
            return vehicleToDriveTypeSearchViewModel.Facets.DriveTypes;
        }


    }
}