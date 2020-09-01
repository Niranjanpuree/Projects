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
    [RoutePrefix("vehicleToMfrBodyCodeSearch")]
    public class VehicleToMfrBodyCodeSearchController : ApiController
    {
        // GET: VehicleToMfrBodyCodeSearch

        private readonly IVehicleToMfrBodyCodeSearchService _vehicleToMfrBodyCodeSearchService;
        private readonly IVehicleToMfrBodyCodeSearchViewModelMapper _vehicleToMfrBodyCodeSearchViewModelMapper;
        private readonly IVehicleToMfrBodyCodeViewModelMapper _vehicleToMfrBodyCodeViewModelMapper;
        public VehicleToMfrBodyCodeSearchController(IVehicleToMfrBodyCodeSearchService vehicleToMfrBodyCodeSearchService,
            IVehicleToMfrBodyCodeSearchViewModelMapper vehicleToMfrBodyCodeSearchViewModelMapper, IVehicleToMfrBodyCodeViewModelMapper vehicleToMfrBodyCodeViewModelMapper)
        {
            _vehicleToMfrBodyCodeSearchService = vehicleToMfrBodyCodeSearchService;
            _vehicleToMfrBodyCodeSearchViewModelMapper = vehicleToMfrBodyCodeSearchViewModelMapper;
            _vehicleToMfrBodyCodeViewModelMapper = vehicleToMfrBodyCodeViewModelMapper;
        }

        [HttpPost]
        [Route("")]
        public async Task<VehicleToMfrBodyCodeSearchViewModel> Search(
            VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                StartYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.EndYear),
                Makes = vehicleToMfrBodyCodeSearchInputModel.Makes,
                Models = vehicleToMfrBodyCodeSearchInputModel.Models,
                SubModels = vehicleToMfrBodyCodeSearchInputModel.SubModels,
                VehicleTypes = vehicleToMfrBodyCodeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes
            };
            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
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
            var viewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return viewModel;
        }
        [HttpPost]
        [Route("vehicle")]
        public async Task<List<VehicleToMfrBodyCodeViewModel>> Search(List<string> vehicleIdArray/*string vehicleIds*/)
        {
            List<VehicleToMfrBodyCodeViewModel> vehicleToMfrBodyCodes = null;
            //if (!string.IsNullOrWhiteSpace(vehicleIds))
            if (vehicleIdArray != null && vehicleIdArray.Count > 0)
            {
                var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
                {
                    VehicleIds = vehicleIdArray.Select(item => Convert.ToInt32(item)).ToArray()
                };

                var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                    new SearchOptions
                    {
                        RecordCount = 1000
                    });

                vehicleToMfrBodyCodes = _vehicleToMfrBodyCodeViewModelMapper.Map(result);

            }
            return vehicleToMfrBodyCodes;
        }

        [HttpGet]
        [Route("mfrBodyCode/{mfrBodyCodeId:int}")]
        public async Task<VehicleToMfrBodyCodeSearchViewModel> SearchByMfrBodyCodeId(int mfrBodyCodeId)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = mfrBodyCodeId,
            };
            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
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
                        "mfrBodyCodeId"
                    },
                RecordCount = 1000,
                ReturnTotalCount = true,
            });
            var mfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return mfrBodyCodeSearchViewModel;
        }
        [HttpPost]
        [Route("associations")]
        public async Task<List<VehicleToMfrBodyCodeViewModel>> GetAssociations(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            List<VehicleToMfrBodyCodeViewModel> vehicleToMfrBodyCodes = new List<VehicleToMfrBodyCodeViewModel>();

            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                StartYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.EndYear),
                Makes = vehicleToMfrBodyCodeSearchInputModel.Makes,
                Models = vehicleToMfrBodyCodeSearchInputModel.Models,
                SubModels = vehicleToMfrBodyCodeSearchInputModel.SubModels,
                VehicleTypes = vehicleToMfrBodyCodeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
            };

            bool isEndReached = false;
            int pageNumber = 1;
            do
            {
                var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions() { RecordCount = 1000, PageNumber = pageNumber });

                if (result != null && result.Documents != null && result.Documents.Any())
                {
                    vehicleToMfrBodyCodes.AddRange(_vehicleToMfrBodyCodeViewModelMapper.Map(result));

                    pageNumber++;
                }
                else
                {
                    isEndReached = true;
                }
            } while (!isEndReached);

            return vehicleToMfrBodyCodes;
        }
          [HttpPost]
        [Route("facets")]
        public async Task<VehicleToMfrBodyCodeSearchViewModel> RefreshFacets(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var vehicleToMfrBodyCodeSearchViewModel = new VehicleToMfrBodyCodeSearchViewModel
            {
                Facets = new VehicleToMfrBodyCodeSearchFacets
                {
                    Regions = await RefreshRegionFacet(vehicleToMfrBodyCodeSearchInputModel),
                    VehicleTypeGroups = await RefreshVehicleTypeGroupFacet(vehicleToMfrBodyCodeSearchInputModel),
                    VehicleTypes = await RefreshVehicleTypeFacet(vehicleToMfrBodyCodeSearchInputModel),
                    Years = await RefreshYearFacet(vehicleToMfrBodyCodeSearchInputModel),
                    Makes = await RefreshMakesFacet(vehicleToMfrBodyCodeSearchInputModel),
                    Models = await RefreshModelsFacet(vehicleToMfrBodyCodeSearchInputModel),
                    SubModels = await RefreshSubModelsFacet(vehicleToMfrBodyCodeSearchInputModel),
                    MfrBodyCodes = await RefreshMfrBodyCodeFacet(vehicleToMfrBodyCodeSearchInputModel),
                }
            };

            return vehicleToMfrBodyCodeSearchViewModel;
        }
        private async Task<string[]> RefreshRegionFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes,
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "regionName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.Regions;
        }

        private async Task<string[]> RefreshVehicleTypeGroupFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeGroupName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.VehicleTypeGroups;
        }

        private async Task<string[]> RefreshVehicleTypeFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "vehicleTypeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.VehicleTypes;
        }

        private async Task<string[]> RefreshYearFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                VehicleTypes = vehicleToMfrBodyCodeSearchInputModel.VehicleTypes,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes,
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "yearId,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.Years;
        }

        private async Task<string[]> RefreshMakesFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                StartYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.EndYear),
                VehicleTypes = vehicleToMfrBodyCodeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes,
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "makeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.Makes;
        }

        private async Task<string[]> RefreshModelsFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                StartYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.EndYear),
                Makes = vehicleToMfrBodyCodeSearchInputModel.Makes,
                VehicleTypes = vehicleToMfrBodyCodeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes,
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "modelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.Models;
        }

        private async Task<string[]> RefreshSubModelsFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
                MfrBodyCodeId = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodeId,
                StartYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.StartYear),
                EndYear = Convert.ToInt32(vehicleToMfrBodyCodeSearchInputModel.EndYear),
                Makes = vehicleToMfrBodyCodeSearchInputModel.Makes,
                Models = vehicleToMfrBodyCodeSearchInputModel.Models,
                VehicleTypes = vehicleToMfrBodyCodeSearchInputModel.VehicleTypes,
                VehicleTypeGroups = vehicleToMfrBodyCodeSearchInputModel.VehicleTypeGroups,
                Regions = vehicleToMfrBodyCodeSearchInputModel.Regions,
                MfrBodyCodes = vehicleToMfrBodyCodeSearchInputModel.MfrBodyCodes
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "subModelName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.SubModels;
        }
        private async Task<string[]> RefreshMfrBodyCodeFacet(VehicleToMfrBodyCodeSearchInputModel vehicleToMfrBodyCodeSearchInputModel)
        {
            var applyFilters = new VehicleToMfrBodyCodeSearchFilters()
            {
            };

            var result = await _vehicleToMfrBodyCodeSearchService.SearchAsync("", applyFilters.ToAzureSearchFilter(),
                new SearchOptions
                {
                    FacetsToInclude = new List<string>
                    {
                        "mfrBodyCodeName,count:1000",
                    },
                    RecordCount = 0
                });

            var vehicleToMfrBodyCodeSearchViewModel = _vehicleToMfrBodyCodeSearchViewModelMapper.Map(result);
            return vehicleToMfrBodyCodeSearchViewModel.Facets.MfrBodyCodes;
        }


    }
}