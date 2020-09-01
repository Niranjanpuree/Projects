using System;
using System.Collections.Generic;
using System.Linq;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearchIndex.Model;
using AutoCare.Product.Web.Models.ViewModels;
using AutoCare.Product.Search.Model;
using AutoCare.Product.Web.Models.Shared;

namespace AutoCare.Product.Web.ViewModelMappers
{
    public class VehicleToWheelBaseSearchViewModelMapper : IVehicleToWheelBaseSearchViewModelMapper
    {
        public VehicleToWheelBaseSearchViewModel Map(VehicleToWheelBaseSearchResult source)
        {
            Guid guid;
            var viewModel = new VehicleToWheelBaseSearchViewModel()
            {
                Facets = new VehicleToWheelBaseSearchFacets()
                {
                    WheelBase = source.Facets.Any(f => f.Name == "wheelBaseName") ?
                        source.Facets.First(f => f.Name == "wheelBaseName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    WheelBaseMetric = source.Facets.Any(f => f.Name == "wheelBaseMetric") ?
                        source.Facets.First(f => f.Name == "wheelBaseMetric")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
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
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
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
                Result = new VehicleToWheelBaseSearchResultViewModel()
                {
                    WheelBases = source.Documents.Distinct(new DistinctWheelBaseIdComparer()).Select(item => new WheelBaseViewModel
                    {
                        Id = Convert.ToInt32(item.WheelBaseId),
                        Base = item.WheelBaseName,
                        WheelBaseMetric = item.WheelBaseMetric,
                        ChangeRequestId = item.WheelBaseChangeRequestId,
                    }).ToList(),
                    VehicleToWheelBases = source.Documents.Where(item => !Guid.TryParse(item.VehicleToWheelBaseId, out guid)).Select(item => new VehicleToWheelBaseViewModel
                    {
                        Id = Convert.ToInt32(item.VehicleToWheelBaseId),
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
                            ChangeRequestId = item.WheelBaseChangeRequestId,
                        },
                        WheelBaseId = item.WheelBaseId ?? 0,
                        VehicleId = item.VehicleId ?? 0,
                        WheelBaseName = item.WheelBaseName,
                        ChangeRequestId = item.VehicleToWheelBaseChangeRequestId
                    }).ToList()
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null,
            };

            return viewModel;
        }
    }

    public class DistinctWheelBaseIdComparer : IEqualityComparer<VehicleToWheelBaseDocument>
    {
        public bool Equals(VehicleToWheelBaseDocument x, VehicleToWheelBaseDocument y)
        {
            return x.WheelBaseId == y.WheelBaseId;
        }

        public int GetHashCode(VehicleToWheelBaseDocument obj)
        {
            return 0;
        }
    }
}