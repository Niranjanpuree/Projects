using System;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;
using System.Linq;

namespace AutoCare.Product.Web.Models.Shared
{
    [Serializable]
    public class VehicleToBedConfigSearchFilters
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] BedLengths { get; set; }
        public string[] BedTypes { get; set; }
        public int[] VehicleIds { get; set; }

        public int BedConfigId { get; set; }

        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleToBedConfigDocument, bool>> filterEx = null;

            filterEx = AddStartYearFilter(filterEx);

            filterEx = AddEndYearFilter(filterEx);

            filterEx = AddMakesFilter(filterEx);

            filterEx = AddModelsFilter(filterEx);

            filterEx = AddSubModelsFilter(filterEx);

            filterEx = AddVehicleTypeFilter(filterEx);

            filterEx = AddVehicleTypeGroupFilter(filterEx);

            filterEx = AddRegionFilter(filterEx);

            filterEx = AddBedLengthFilter(filterEx);

            filterEx = AddBedTypeFilter(filterEx);

            filterEx = AddVehicleIdsFilter(filterEx);

            filterEx = AddBedConfigIdFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddStartYearFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (StartYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);     //TODO: pushkar: check if AndAlso() will work
            }
            return filterEx;
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddEndYearFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (EndYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddMakesFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (Makes == null || !Makes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> makeFilterEx = null;
            foreach (var make in Makes)
            {
                makeFilterEx = makeFilterEx.OrElse(x => x.MakeName == make);
            }

            return filterEx.AndAlso(makeFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddModelsFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (Models == null || !Models.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> modelFilterEx = null;
            foreach (var model in Models)
            {
                modelFilterEx = modelFilterEx.OrElse(x => x.ModelName == model);
            }

            return filterEx.AndAlso(modelFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddSubModelsFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (SubModels == null || !SubModels.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> subModelsFilterEx = null;
            foreach (var subModel in SubModels)
            {
                subModelsFilterEx = subModelsFilterEx.OrElse(x => x.SubModelName == subModel);
            }

            return filterEx.AndAlso(subModelsFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddVehicleTypeFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (VehicleTypes == null || !VehicleTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> vehicleTypeFilterEx = null;
            foreach (var vehicleType in VehicleTypes)
            {
                vehicleTypeFilterEx = vehicleTypeFilterEx.OrElse(x => x.VehicleTypeName == vehicleType);
            }

            return filterEx.AndAlso(vehicleTypeFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddVehicleTypeGroupFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (VehicleTypeGroups == null || !VehicleTypeGroups.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> vehicleTypeGroupFilterEx = null;
            foreach (var vehicleTypeGroup in VehicleTypeGroups)
            {
                vehicleTypeGroupFilterEx = vehicleTypeGroupFilterEx.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup);
            }

            return filterEx.AndAlso(vehicleTypeGroupFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddRegionFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (Regions == null || !Regions.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> regionFilterEx = null;
            foreach (var region in Regions)
            {
                regionFilterEx = regionFilterEx.OrElse(x => x.RegionName == region);
            }

            return filterEx.AndAlso(regionFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddBedLengthFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (BedLengths == null || !BedLengths.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> bedLengthFilterEx = null;
            foreach (var bedLength in BedLengths)
            {
                bedLengthFilterEx = bedLengthFilterEx.OrElse(x => x.BedLength == bedLength);
            }

            return filterEx.AndAlso(bedLengthFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddBedTypeFilter(
            Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (BedTypes == null || !BedTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> bedTypeFilterEx = null;
            foreach (var bedType in BedTypes)
            {
                bedTypeFilterEx = bedTypeFilterEx.OrElse(x => x.BedTypeName == bedType);
            }

            return filterEx.AndAlso(bedTypeFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddVehicleIdsFilter(
            Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (VehicleIds == null || !VehicleIds.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToBedConfigDocument, bool>> vehicleIdFilterEx = null;
            foreach (var vehicleId in VehicleIds)
            {
                vehicleIdFilterEx = vehicleIdFilterEx.OrElse(x => x.VehicleId == vehicleId);
            }

            return filterEx.AndAlso(vehicleIdFilterEx);
        }

        private Expression<Func<VehicleToBedConfigDocument, bool>> AddBedConfigIdFilter(Expression<Func<VehicleToBedConfigDocument, bool>> filterEx)
        {
            if (BedConfigId != 0)
            {
                filterEx = filterEx.AndAlso(x => x.BedConfigId == BedConfigId);
            }
            return filterEx;
        }
    }
}