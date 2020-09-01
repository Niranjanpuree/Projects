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
    public class VehicleToDriveTypeSearchViewModelMapper : IVehicleToDriveTypeSearchViewModelMapper
    {
        public VehicleToDriveTypeSearchViewModel Map(VehicleToDriveTypeSearchResult source)
        {
            Guid guid;
            var viewModel = new VehicleToDriveTypeSearchViewModel()
            {
                Facets = new VehicleToDriveTypeSearchFacets()
                {
                    DriveTypes = source.Facets.Any(f => f.Name == "driveTypeName")
                        ? source.Facets.First(f => f.Name == "driveTypeName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Regions = source.Facets.Any(f => f.Name == "regionName")
                        ? source.Facets.First(f => f.Name == "regionName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    VehicleTypeGroups = source.Facets.Any(f => f.Name == "vehicleTypeGroupName")
                        ? source.Facets.First(f => f.Name == "vehicleTypeGroupName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    VehicleTypes = source.Facets.Any(f => f.Name == "vehicleTypeName")
                        ? source.Facets.First(f => f.Name == "vehicleTypeName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Years = source.Facets.Any(f => f.Name == "yearId")
                        ? source.Facets.First(f => f.Name == "yearId")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Makes = source.Facets.Any(f => f.Name == "makeName")
                        ? source.Facets.First(f => f.Name == "makeName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    Models = source.Facets.Any(f => f.Name == "modelName")
                        ? source.Facets.First(f => f.Name == "modelName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                    SubModels = source.Facets.Any(f => f.Name == "subModelName")
                        ? source.Facets.First(f => f.Name == "subModelName")
                            .Value.Select(item => ((SimpleValue)item).Value.ToString()).OrderBy(item => item)
                            .ToArray()
                        : default(string[]),
                },
                Result = new VehicleToDriveTypeSearchResultViewModel()
                {
                    DriveTypes =
                        source.Documents.Distinct(new DistinctDriveTypeIdComparer())
                            .Select(item => new DriveTypeViewModel
                            {
                                Id = Convert.ToInt32(item.DriveTypeId),
                                Name = item.DriveTypeName,
                                ChangeRequestId = item.DriveTypeChangeRequestId,
                            }).ToList(),
                    VehicleToDriveTypes =
                        source.Documents.Where(item => !Guid.TryParse(item.VehicleToDriveTypeId, out guid))
                            .Select(item => new VehicleToDriveTypeViewModel
                            {
                                Id = Convert.ToInt32(item.VehicleToDriveTypeId),
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
                                    ChangeRequestId = item.DriveTypeChangeRequestId,
                                },
                                DriveTypeId = item.DriveTypeId ?? 0,
                                VehicleId = item.VehicleId ?? 0,
                                Name = item.DriveTypeName,

                                DriveType = new DriveTypeViewModel
                                {
                                    Id = item.DriveTypeId ?? 0
                                    //just id is sufficient, it will used by check box selection events
                                },
                                ChangeRequestId = item.VehicleToDriveTypeChangeRequestId
                            }).ToList()
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null,
            };

            return viewModel;
        }

        public class DistinctDriveTypeIdComparer : IEqualityComparer<VehicleToDriveTypeDocument>
        {
            public bool Equals(VehicleToDriveTypeDocument x, VehicleToDriveTypeDocument y)
            {
                return x.DriveTypeId == y.DriveTypeId;
            }

            public int GetHashCode(VehicleToDriveTypeDocument obj)
            {
                return 0;
            }
        }
    }
}