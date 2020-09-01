using System;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;
using System.Linq;

namespace AutoCare.Product.Web.Models.Shared
{
    [Serializable]
    public class VehicleToBrakeConfigSearchFilters
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] FrontBrakeTypes { get; set; }
        public string[] RearBrakeTypes { get; set; }
        public string[] BrakeAbs { get; set; }
        public string[] BrakeSystem { get; set; }
        public int[] VehicleIds { get; set; }

        public int BrakeConfigId { get; set; }

        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx = null;

            filterEx = AddStartYearFilter(filterEx);

            filterEx = AddEndYearFilter(filterEx);

            filterEx = AddMakesFilter(filterEx);

            filterEx = AddModelsFilter(filterEx);

            filterEx = AddSubModelsFilter(filterEx);

            filterEx = AddVehicleTypeFilter(filterEx);

            filterEx = AddVehicleTypeGroupFilter(filterEx);

            filterEx = AddRegionFilter(filterEx);

            filterEx = AddFrontBrakeTypeFilter(filterEx);

            filterEx = AddRearBrakeTypeFilter(filterEx);

            filterEx = AddBrakeAbsFilter(filterEx);

            filterEx = AddBrakeSystemFilter(filterEx);

            filterEx = AddVehicleIdsFilter(filterEx);

            filterEx = AddBrakeConfigIdFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddStartYearFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (StartYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);     //TODO: pushkar: check if AndAlso() will work
            }
            return filterEx;
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddEndYearFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (EndYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddMakesFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (Makes == null || !Makes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> makeFilterEx = null;
            foreach (var make in Makes)
            {
                makeFilterEx = makeFilterEx.OrElse(x => x.MakeName == make);
            }

            return filterEx.AndAlso(makeFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddModelsFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (Models == null || !Models.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> modelFilterEx = null;
            foreach (var model in Models)
            {
                modelFilterEx = modelFilterEx.OrElse(x => x.ModelName == model);
            }

            return filterEx.AndAlso(modelFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddSubModelsFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (SubModels == null || !SubModels.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> subModelsFilterEx = null;
            foreach (var subModel in SubModels)
            {
                subModelsFilterEx = subModelsFilterEx.OrElse(x => x.SubModelName == subModel);
            }

            return filterEx.AndAlso(subModelsFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddVehicleTypeFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (VehicleTypes == null || !VehicleTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> vehicleTypeFilterEx = null;
            foreach (var vehicleType in VehicleTypes)
            {
                vehicleTypeFilterEx = vehicleTypeFilterEx.OrElse(x => x.VehicleTypeName == vehicleType);
            }

            return filterEx.AndAlso(vehicleTypeFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddVehicleTypeGroupFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (VehicleTypeGroups == null || !VehicleTypeGroups.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> vehicleTypeGroupFilterEx = null;
            foreach (var vehicleTypeGroup in VehicleTypeGroups)
            {
                vehicleTypeGroupFilterEx = vehicleTypeGroupFilterEx.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup);
            }

            return filterEx.AndAlso(vehicleTypeGroupFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddRegionFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (Regions == null || !Regions.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> regionFilterEx = null;
            foreach (var region in Regions)
            {
                regionFilterEx = regionFilterEx.OrElse(x => x.RegionName == region);
            }

            return filterEx.AndAlso(regionFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddFrontBrakeTypeFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (FrontBrakeTypes == null || !FrontBrakeTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> frontBrakeTypeFilterEx = null;
            foreach (var frontBrakeType in FrontBrakeTypes)
            {
                frontBrakeTypeFilterEx = frontBrakeTypeFilterEx.OrElse(x => x.FrontBrakeTypeName == frontBrakeType);
            }

            return filterEx.AndAlso(frontBrakeTypeFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddRearBrakeTypeFilter(
            Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (RearBrakeTypes == null || !RearBrakeTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> rearBrakeTypeFilterEx = null;
            foreach (var rearBrakeType in RearBrakeTypes)
            {
                rearBrakeTypeFilterEx = rearBrakeTypeFilterEx.OrElse(x => x.RearBrakeTypeName == rearBrakeType);
            }

            return filterEx.AndAlso(rearBrakeTypeFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddBrakeAbsFilter(
            Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (BrakeAbs == null || !BrakeAbs.Any())
            {
                return filterEx;
            }
            Expression<Func<VehicleToBrakeConfigDocument, bool>> brakeAbsFilterEx = null;
            foreach (var brakeAbs in BrakeAbs)
            {
                brakeAbsFilterEx = brakeAbsFilterEx.OrElse(x => x.BrakeABSName == brakeAbs);
            }

            return filterEx.AndAlso(brakeAbsFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddBrakeSystemFilter(
            Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (BrakeSystem == null || !BrakeSystem.Any())
            {
                return filterEx;
            }
            Expression<Func<VehicleToBrakeConfigDocument, bool>> brakeSystemFilterEx = null;
            foreach (var brakeSystem in BrakeSystem)
            {
                brakeSystemFilterEx = brakeSystemFilterEx.OrElse(x => x.BrakeSystemName == brakeSystem);
            }

            return filterEx.AndAlso(brakeSystemFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddVehicleIdsFilter(
            Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (VehicleIds == null || !VehicleIds.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBrakeConfigDocument, bool>> vehicleIdFilterEx = null;
            foreach (var vehicleId in VehicleIds)
            {
                vehicleIdFilterEx = vehicleIdFilterEx.OrElse(x => x.VehicleId == vehicleId);
            }

            return filterEx.AndAlso(vehicleIdFilterEx);
        }

        private Expression<Func<VehicleToBrakeConfigDocument, bool>> AddBrakeConfigIdFilter(Expression<Func<VehicleToBrakeConfigDocument, bool>> filterEx)
        {
            if (BrakeConfigId != 0)
            {
                filterEx = filterEx.AndAlso(x => x.BrakeConfigId == BrakeConfigId);
            }
            return filterEx;
        }
    }
}