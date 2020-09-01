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
    public class VehicleToBrakeConfigSearchViewModelMapper : IVehicleToBrakeConfigSearchViewModelMapper
    {
        public VehicleToBrakeConfigSearchViewModel Map(VehicleToBrakeConfigSearchResult source)
        {
            Guid guid;
            var viewModel = new VehicleToBrakeConfigSearchViewModel()
            {
                Facets = new VehicleToBrakeConfigSearchFacets()
                {
                    FrontBrakeTypes = source.Facets.Any(f => f.Name == "frontBrakeTypeName") ?
                        source.Facets.First(f => f.Name == "frontBrakeTypeName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    RearBrakeTypes = source.Facets.Any(f => f.Name == "rearBrakeTypeName") ?
                        source.Facets.First(f => f.Name == "rearBrakeTypeName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    BrakeAbs = source.Facets.Any(f => f.Name == "brakeABSName") ?
                        source.Facets.First(f => f.Name == "brakeABSName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray() : default(string[]),
                    BrakeSystems = source.Facets.Any(f => f.Name == "brakeSystemName") ?
                        source.Facets.First(f => f.Name == "brakeSystemName")
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
                Result = new VehicleToBrakeConfigSearchResultViewModel()
                {
                    BrakeConfigs = source.Documents.Distinct(new DistinctBrakeConfigIdComparer()).Select(item => new BrakeConfigViewModel
                    {
                        Id = Convert.ToInt32(item.BrakeConfigId),
                        FrontBrakeTypeId = item.FrontBrakeTypeId ?? 0,
                        BrakeSystemId = item.BrakeSystemId ?? 0,
                        BrakeABSId = item.BrakeABSId ?? 0,
                        RearBrakeTypeId = item.RearBrakeTypeId ?? 0,
                        FrontBrakeTypeName = item.FrontBrakeTypeName,
                        RearBrakeTypeName = item.RearBrakeTypeName,
                        BrakeSystemName = item.BrakeSystemName,
                        BrakeABSName = item.BrakeABSName,
                        ChangeRequestId = item.BrakeConfigChangeRequestId,
                    }).ToList(),
                    VehicleToBrakeConfigs = source.Documents.Where(item => !Guid.TryParse(item.VehicleToBrakeConfigId, out guid)).Select(item => new VehicleToBrakeConfigViewModel
                    {
                        Id = Convert.ToInt32(item.VehicleToBrakeConfigId),
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
                            ChangeRequestId = item.BrakeConfigChangeRequestId,
                        },
                        BrakeConfigId = item.BrakeConfigId ?? 0,
                        VehicleId = item.VehicleId ?? 0,
                        FrontBrakeTypeId = item.FrontBrakeTypeId ?? 0,
                        FrontBrakeTypeName = item.FrontBrakeTypeName,
                        RearBrakeTypeId = item.RearBrakeTypeId ?? 0,
                        RearBrakeTypeName = item.RearBrakeTypeName,
                        BrakeSystemId = item.BrakeSystemId ?? 0,
                        BrakeSystemName = item.BrakeSystemName,
                        BrakeABSId = item.BrakeABSId ?? 0,
                        BrakeABSName = item.BrakeABSName,
                        BrakeConfig = new BrakeConfigViewModel
                        {
                            Id = item.BrakeConfigId ?? 0 //just id is sufficient, it will used by check box selection events
                        },
                        ChangeRequestId = item.VehicleToBrakeConfigChangeRequestId
                    }).ToList()
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null,
            };

            return viewModel;
        }
    }

    public class DistinctBrakeConfigIdComparer : IEqualityComparer<VehicleToBrakeConfigDocument>
    {
        public bool Equals(VehicleToBrakeConfigDocument x, VehicleToBrakeConfigDocument y)
        {
            return x.BrakeConfigId == y.BrakeConfigId;
        }

        public int GetHashCode(VehicleToBrakeConfigDocument obj)
        {
            return 0;
        }
    }
}