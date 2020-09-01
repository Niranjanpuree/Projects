using System;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;
using System.Linq;

namespace AutoCare.Product.Web.Models.Shared
{
    public class VehicleToMfrBodyCodeSearchFilters
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] MfrBodyCodes { get; set; }
        public int[] VehicleIds { get; set; }
        public int MfrBodyCodeId { get; set; }

        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx = null;
            filterEx = AddStartYearFilter(filterEx);

            filterEx = AddEndYearFilter(filterEx);

            filterEx = AddMakesFilter(filterEx);

            filterEx = AddModelsFilter(filterEx);

            filterEx = AddSubModelsFilter(filterEx);

            filterEx = AddVehicleTypeFilter(filterEx);

            filterEx = AddVehicleTypeGroupFilter(filterEx);

            filterEx = AddRegionFilter(filterEx);
            filterEx = AddMfrBodyCodeFilter(filterEx);

            filterEx = AddVehicleIdsFilter(filterEx);
            filterEx = AddMfrBodyCodeIdFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }
        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddStartYearFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (StartYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);     //TODO: pushkar: check if AndAlso() will work
            }
            return filterEx;
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddEndYearFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (EndYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddMakesFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (Makes == null || !Makes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> makeFilterEx = null;
            foreach (var make in Makes)
            {
                makeFilterEx = makeFilterEx.OrElse(x => x.MakeName == make);
            }

            return filterEx.AndAlso(makeFilterEx);
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddModelsFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (Models == null || !Models.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> modelFilterEx = null;
            foreach (var model in Models)
            {
                modelFilterEx = modelFilterEx.OrElse(x => x.ModelName == model);
            }

            return filterEx.AndAlso(modelFilterEx);
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddSubModelsFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (SubModels == null || !SubModels.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> subModelsFilterEx = null;
            foreach (var subModel in SubModels)
            {
                subModelsFilterEx = subModelsFilterEx.OrElse(x => x.SubModelName == subModel);
            }

            return filterEx.AndAlso(subModelsFilterEx);
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddVehicleTypeFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (VehicleTypes == null || !VehicleTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> vehicleTypeFilterEx = null;
            foreach (var vehicleType in VehicleTypes)
            {
                vehicleTypeFilterEx = vehicleTypeFilterEx.OrElse(x => x.VehicleTypeName == vehicleType);
            }

            return filterEx.AndAlso(vehicleTypeFilterEx);
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddVehicleTypeGroupFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (VehicleTypeGroups == null || !VehicleTypeGroups.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> vehicleTypeGroupFilterEx = null;
            foreach (var vehicleTypeGroup in VehicleTypeGroups)
            {
                vehicleTypeGroupFilterEx = vehicleTypeGroupFilterEx.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup);
            }

            return filterEx.AndAlso(vehicleTypeGroupFilterEx);
        }

        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddRegionFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (Regions == null || !Regions.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> regionFilterEx = null;
            foreach (var region in Regions)
            {
                regionFilterEx = regionFilterEx.OrElse(x => x.RegionName == region);
            }

            return filterEx.AndAlso(regionFilterEx);
        }
        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddMfrBodyCodeFilter(
          Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (MfrBodyCodes == null || !MfrBodyCodes.Any())
            {
                return filterEx;
            }
            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> mfrBodyCodeFilterEx = null;
            foreach (var mfrBodyCode in MfrBodyCodes)
            {
                mfrBodyCodeFilterEx = mfrBodyCodeFilterEx.OrElse(x => x.MfrBodyCodeName == mfrBodyCode);
            }

            return filterEx.AndAlso(mfrBodyCodeFilterEx);
        }
        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddVehicleIdsFilter(
          Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (VehicleIds == null || !VehicleIds.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToMfrBodyCodeDocument, bool>> vehicleIdFilterEx = null;
            foreach (var vehicleId in VehicleIds)
            {
                vehicleIdFilterEx = vehicleIdFilterEx.OrElse(x => x.VehicleId == vehicleId);
            }

            return filterEx.AndAlso(vehicleIdFilterEx);
        }
        private Expression<Func<VehicleToMfrBodyCodeDocument, bool>> AddMfrBodyCodeIdFilter(Expression<Func<VehicleToMfrBodyCodeDocument, bool>> filterEx)
        {
            if (MfrBodyCodeId != 0)
            {
                filterEx = filterEx.AndAlso(x => x.MfrBodyCodeId == MfrBodyCodeId);
            }
            return filterEx;
        }

    }
}