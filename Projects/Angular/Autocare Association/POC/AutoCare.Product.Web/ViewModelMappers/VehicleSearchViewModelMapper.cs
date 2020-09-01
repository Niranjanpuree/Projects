using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.Search.Model;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Web.Models.Shared;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleSearchViewModelMapper : IVehicleSearchViewModelMapper
    {
        public VehicleSearchViewModel Map(VehicleSearchResult source)
        {
            Guid guid;
            var vehicleSearchViewModel = new VehicleSearchViewModel()
            {
                Facets = new VehicleSearchFacets()
                {
                    Regions = source.Facets.Any(f => f.Name == "regionName") ?
                        source.Facets.First(f => f.Name == "regionName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    VehicleTypeGroups = source.Facets.Any(f => f.Name == "vehicleTypeGroupName") ?
                        source.Facets.First(f => f.Name == "vehicleTypeGroupName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    VehicleTypes = source.Facets.Any(f => f.Name == "vehicleTypeName") ?
                        source.Facets.First(f => f.Name == "vehicleTypeName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    Years = source.Facets.Any(f => f.Name == "yearId") ?
                        source.Facets.First(f => f.Name == "yearId")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    Makes = source.Facets.Any(f => f.Name == "makeName") ?
                        source.Facets.First(f => f.Name == "makeName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    Models = source.Facets.Any(f => f.Name == "modelName") ?
                        source.Facets.First(f => f.Name == "modelName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    SubModels = source.Facets.Any(f => f.Name == "subModelName") ?
                        source.Facets.First(f => f.Name == "subModelName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                },
                Result = new VehicleSearchResultViewModel
                {
                    //NOTE: VehicleId of Guid means new base vehicle without vehicle information
                    Vehicles = source.Documents.Where(item=> !Guid.TryParse(item.VehicleId, out guid)).Select(item => new VehicleViewModel
                    {
                        BaseVehicleId = item.BaseVehicleId ?? 0,
                        Id = Convert.ToInt32(item.VehicleId),
                        MakeId = item.MakeId ?? 0,
                        MakeName = item.MakeName,
                        ModelId = item.ModelId ?? 0,
                        ModelName = item.ModelName,
                        RegionId = item.RegionId ?? 0,
                        RegionName = item.RegionName,
                        SourceName = item.Source,
                        SubModelId = item.SubModelId ?? 0,
                        SubModelName = item.SubModelName,
                        YearId = item.YearId ?? 0,
                        ChangeRequestId = item.VehicleChangeRequestId
                    }).ToList(),
                    BaseVehicles = source.Documents.Distinct(new DistinctBaseVehicleIdComparer()).Select(item => new BaseVehicleViewModel
                    {
                        Id = item.BaseVehicleId ?? 0,
                        MakeId = item.MakeId ?? 0,
                        MakeName = item.MakeName,
                        ModelId = item.ModelId ?? 0,
                        ModelName = item.ModelName,
                        YearId = item.YearId ?? 0,
                        ChangeRequestId = item.BaseVehicleChangeRequestId
                    }).ToList(),
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null,
            };

            return vehicleSearchViewModel;
        }
    }

    public class DistinctBaseVehicleIdComparer : IEqualityComparer<VehicleDocument>
    {
        public bool Equals(VehicleDocument x, VehicleDocument y)
        {
            return x.BaseVehicleId == y.BaseVehicleId;
        }

        public int GetHashCode(VehicleDocument obj)
        {
            return 0;
        }
    }
}