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
    public class VehicleToBodyStyleConfigSearchViewModelMapper: IVehicleToBodyStyleConfigSearchViewModelMapper
    {
        public VehicleToBodyStyleConfigSearchViewModel Map(VehicleToBodyStyleConfigSearchResult source)
        {
            Guid guid;
            var viewModel = new VehicleToBodyStyleConfigSearchViewModel()
            {
                Facets = new VehicleToBodyStyleConfigSearchFacets()
                {
                    // vehicle
                    Makes = source.Facets.Any(f => f.Name == "makeName")
                        ? source.Facets.First(f => f.Name == "makeName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Models = source.Facets.Any(f => f.Name == "modelName")
                        ? source.Facets.First(f => f.Name == "modelName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    SubModels = source.Facets.Any(f => f.Name == "subModelName")
                        ? source.Facets.First(f => f.Name == "subModelName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Years = source.Facets.Any(f => f.Name == "yearId")
                        ? source.Facets.First(f => f.Name == "yearId")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Regions = source.Facets.Any(f => f.Name == "regionName")
                        ? source.Facets.First(f => f.Name == "regionName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    // body style
                    BodyNumDoors = source.Facets.Any(f => f.Name == "bodyNumDoors")
                        ? source.Facets.First(f => f.Name == "bodyNumDoors")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    BodyTypes = source.Facets.Any(f => f.Name == "bodyTypeName")
                        ? source.Facets.First(f => f.Name == "bodyTypeName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    // others
                    VehicleTypes = source.Facets.Any(f => f.Name == "vehicleTypeName")
                        ? source.Facets.First(f => f.Name == "vehicleTypeName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    VehicleTypeGroups = source.Facets.Any(f => f.Name == "vehicleTypeGroupName")
                        ? source.Facets.First(f => f.Name == "vehicleTypeGroupName")
                            .Value.Select(item => ((SimpleValue) item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                },
                Result = new VehicleToBodyStyleConfigSearchResultViewModel()
                {
                    BodyStyleConfigs = source.Documents.Distinct(new DistinctBodyStyleConfigIdComparer()).Select(item => new BodyStyleConfigViewModel
                    {
                        Id = Convert.ToInt32(item.BodyStyleConfigId),
                        ChangeRequestId = item.BodyStyleConfigChangeRequestId,
                        BodyNumDoorsId = item.BodyNumDoorsId ?? 0,
                        NumDoors = item.BodyNumDoors,
                        BodyTypeId = item.BodyTypeId ?? 0,
                        BodyTypeName = item.BodyTypeName
                    }).ToList(),
                    VehicleToBodyStyleConfigs = source.Documents.Where(item => !Guid.TryParse(item.VehicleToBodyStyleConfigId, out guid))
                        .Select(item => new VehicleToBodyStyleConfigViewModel
                        {
                            Id = Convert.ToInt32(item.VehicleToBodyStyleConfigId),
                            Vehicle = new VehicleViewModel
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
                                ChangeRequestId = item.BodyStyleConfigChangeRequestId,
                            },
                            BodyStyleConfigId = item.BodyStyleConfigId ?? 0,
                            VehicleId = item.VehicleId ?? 0,
                            BodyNumberDoorsId = item.BodyNumDoorsId ?? 0,
                            BodyNumDoors = item.BodyNumDoors,
                            BodyTypeId = item.BodyTypeId ?? 0,
                            BodyTypeName = item.BodyTypeName,
                            BodyStyleConfig = new BodyStyleConfigViewModel
                            {
                                Id = item.BodyStyleConfigId ?? 0 //just id is sufficient, it will used by check box selection events
                            },
                            ChangeRequestId = item.VehicleToBodyStyleConfigChangeRequestId
                        }).ToList()
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null
            };
            return viewModel;
        }

        private class DistinctBodyStyleConfigIdComparer : IEqualityComparer<VehicleToBodyStyleConfigDocument>
        {
            public bool Equals(VehicleToBodyStyleConfigDocument x, VehicleToBodyStyleConfigDocument y)
            {
                return x.BodyStyleConfigId == y.BodyStyleConfigId;
            }

            public int GetHashCode(VehicleToBodyStyleConfigDocument obj)
            {
                return 0;
            }
        }
    }
}