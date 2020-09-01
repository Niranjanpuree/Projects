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
    public class VehicleToMfrBodyCodeSearchViewModelMapper : IVehicleToMfrBodyCodeSearchViewModelMapper
    {
        public VehicleToMfrBodyCodeSearchViewModel Map(VehicleToMfrBodyCodeSearchResult source)
        {
            Guid guid;
            var viewModel = new VehicleToMfrBodyCodeSearchViewModel()
            {
                Facets = new VehicleToMfrBodyCodeSearchFacets()
                {
                    MfrBodyCodes = source.Facets.Any(f => f.Name == "mfrBodyCodeName")
                        ? source.Facets.First(f => f.Name == "mfrBodyCodeName")
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
                Result = new VehicleToMfrBodyCodeSearchResultViewModel()
                {
                    MfrBodyCodes =
                        source.Documents.Distinct(new DistinctMfrBodyCodeIdComparer())
                            .Select(item => new MfrBodyCodeViewModel
                            {
                                Id = Convert.ToInt32(item.MfrBodyCodeId),
                                Name = item.MfrBodyCodeName,
                                ChangeRequestId = item.MfrBodyCodeChangeRequestId,
                            }).ToList(),
                    VehicleToMfrBodyCodes =
                        source.Documents.Where(item => !Guid.TryParse(item.VehicleToMfrBodyCodeId, out guid))
                            .Select(item => new VehicleToMfrBodyCodeViewModel
                            {
                                Id = Convert.ToInt32(item.VehicleToMfrBodyCodeId),
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
                                    ChangeRequestId = item.MfrBodyCodeChangeRequestId,
                                },
                                MfrBodyCodeId = item.MfrBodyCodeId ?? 0,
                                VehicleId = item.VehicleId ?? 0,
                                Name = item.MfrBodyCodeName,

                                MfrBodyCode = new MfrBodyCodeViewModel
                                {
                                    Id = item.MfrBodyCodeId ?? 0
                                    //just id is sufficient, it will used by check box selection events
                                },
                                ChangeRequestId = item.VehicleToMfrBodyCodeChangeRequestId
                            }).ToList()
                },
                TotalCount = source.TotalCount != null && source.TotalCount > 1000 ? source.TotalCount : null,
            };

            return viewModel;
        }

        public class DistinctMfrBodyCodeIdComparer : IEqualityComparer<VehicleToMfrBodyCodeDocument>
        {
            public bool Equals(VehicleToMfrBodyCodeDocument x, VehicleToMfrBodyCodeDocument y)
            {
                return x.MfrBodyCodeId == y.MfrBodyCodeId;
            }

            public int GetHashCode(VehicleToMfrBodyCodeDocument obj)
            {
                return 0;
            }
        }
    }
}