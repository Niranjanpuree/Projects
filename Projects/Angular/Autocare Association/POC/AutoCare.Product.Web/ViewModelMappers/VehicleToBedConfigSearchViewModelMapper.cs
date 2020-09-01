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
    public class VehicleToBedConfigSearchViewModelMapper : IVehicleToBedConfigSearchViewModelMapper
    {
        public VehicleToBedConfigSearchViewModel Map(VehicleToBedConfigSearchResult source)
        {
            Guid guid;
            var viewModel = new VehicleToBedConfigSearchViewModel()
            {
                Facets = new VehicleToBedConfigSearchFacets()
                {
                    BedLengths = source.Facets.Any(f => f.Name == "bedLength") ?
                        source.Facets.First(f => f.Name == "bedLength")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    BedTypes = source.Facets.Any(f => f.Name == "bedTypeName") ?
                        source.Facets.First(f => f.Name == "bedTypeName")
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
                Result = new VehicleToBedConfigSearchResultViewModel()
                {
                    BedConfigs = source.Documents.Distinct(new DistinctBedConfigIdComparer()).Select(item => new BedConfigViewModel
                    {
                        Id = Convert.ToInt32(item.BedConfigId),
                        BedLengthId = item.BedLengthId ?? 0,
                        Length = item.BedLength,
                        BedLengthMetric=item.BedLengthMetric,
                        BedTypeId = item.BedTypeId ?? 0,
                        BedTypeName = item.BedTypeName,
                        ChangeRequestId = item.BedConfigChangeRequestId,
                    }).ToList(),
                    VehicleToBedConfigs = source.Documents.Where(item => !Guid.TryParse(item.VehicleToBedConfigId, out guid)).Select(item => new VehicleToBedConfigViewModel
                    {
                        Id = Convert.ToInt32(item.VehicleToBedConfigId),
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
                            ChangeRequestId = item.BedConfigChangeRequestId,
                        },
                        BedConfigId = item.BedConfigId ?? 0,
                        VehicleId = item.VehicleId ?? 0,
                        BedLengthId = item.BedLengthId ?? 0,
                        BedLengthMetric=item.BedLengthMetric,
                        Length = item.BedLength,
                        BedTypeId = item.BedTypeId ?? 0,
                        BedTypeName = item.BedTypeName,
                        BedConfig = new BedConfigViewModel
                        {
                            Id = item.BedConfigId ?? 0 //just id is sufficient, it will used by check box selection events
                        },
                        ChangeRequestId = item.VehicleToBedConfigChangeRequestId
                    }).ToList()
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null
            };

            return viewModel;
        }
    }

    public class DistinctBedConfigIdComparer : IEqualityComparer<VehicleToBedConfigDocument>
    {
        public bool Equals(VehicleToBedConfigDocument x, VehicleToBedConfigDocument y)
        {
            return x.BedConfigId == y.BedConfigId;
        }

        public int GetHashCode(VehicleToBedConfigDocument obj)
        {
            return 0;
        }
    }
}