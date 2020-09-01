using System;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;
using System.Linq;

namespace AutoCare.Product.Web.Models.Shared
{
    public class VehicleToDriveTypeSearchFilters
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] DriveTypes { get; set; }
        public int[] VehicleIds { get; set; }
        public int DriveTypeId { get; set; }

        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx = null;
            filterEx = AddStartYearFilter(filterEx);

            filterEx = AddEndYearFilter(filterEx);

            filterEx = AddMakesFilter(filterEx);

            filterEx = AddModelsFilter(filterEx);

            filterEx = AddSubModelsFilter(filterEx);

            filterEx = AddVehicleTypeFilter(filterEx);

            filterEx = AddVehicleTypeGroupFilter(filterEx);

            filterEx = AddRegionFilter(filterEx);
            filterEx = AddDriveTypeFilter(filterEx);

            filterEx = AddVehicleIdsFilter(filterEx);
            filterEx = AddDriveTypeIdFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }
        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddStartYearFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (StartYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);     //TODO: pushkar: check if AndAlso() will work
            }
            return filterEx;
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddEndYearFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (EndYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddMakesFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (Makes == null || !Makes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> makeFilterEx = null;
            foreach (var make in Makes)
            {
                makeFilterEx = makeFilterEx.OrElse(x => x.MakeName == make);
            }

            return filterEx.AndAlso(makeFilterEx);
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddModelsFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (Models == null || !Models.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> modelFilterEx = null;
            foreach (var model in Models)
            {
                modelFilterEx = modelFilterEx.OrElse(x => x.ModelName == model);
            }

            return filterEx.AndAlso(modelFilterEx);
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddSubModelsFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (SubModels == null || !SubModels.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> subModelsFilterEx = null;
            foreach (var subModel in SubModels)
            {
                subModelsFilterEx = subModelsFilterEx.OrElse(x => x.SubModelName == subModel);
            }

            return filterEx.AndAlso(subModelsFilterEx);
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddVehicleTypeFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (VehicleTypes == null || !VehicleTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> vehicleTypeFilterEx = null;
            foreach (var vehicleType in VehicleTypes)
            {
                vehicleTypeFilterEx = vehicleTypeFilterEx.OrElse(x => x.VehicleTypeName == vehicleType);
            }

            return filterEx.AndAlso(vehicleTypeFilterEx);
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddVehicleTypeGroupFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (VehicleTypeGroups == null || !VehicleTypeGroups.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> vehicleTypeGroupFilterEx = null;
            foreach (var vehicleTypeGroup in VehicleTypeGroups)
            {
                vehicleTypeGroupFilterEx = vehicleTypeGroupFilterEx.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup);
            }

            return filterEx.AndAlso(vehicleTypeGroupFilterEx);
        }

        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddRegionFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (Regions == null || !Regions.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> regionFilterEx = null;
            foreach (var region in Regions)
            {
                regionFilterEx = regionFilterEx.OrElse(x => x.RegionName == region);
            }

            return filterEx.AndAlso(regionFilterEx);
        }
        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddDriveTypeFilter(
          Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (DriveTypes == null || !DriveTypes.Any())
            {
                return filterEx;
            }
            Expression<Func<VehicleToDriveTypeDocument, bool>> mfrBodyCodeFilterEx = null;
            foreach (var driveType in DriveTypes)
            {
                mfrBodyCodeFilterEx = mfrBodyCodeFilterEx.OrElse(x => x.DriveTypeName == driveType);
            }

            return filterEx.AndAlso(mfrBodyCodeFilterEx);
        }
        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddVehicleIdsFilter(
          Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (VehicleIds == null || !VehicleIds.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToDriveTypeDocument, bool>> vehicleIdFilterEx = null;
            foreach (var vehicleId in VehicleIds)
            {
                vehicleIdFilterEx = vehicleIdFilterEx.OrElse(x => x.VehicleId == vehicleId);
            }

            return filterEx.AndAlso(vehicleIdFilterEx);
        }
        private Expression<Func<VehicleToDriveTypeDocument, bool>> AddDriveTypeIdFilter(Expression<Func<VehicleToDriveTypeDocument, bool>> filterEx)
        {
            if (DriveTypeId != 0)
            {
                filterEx = filterEx.AndAlso(x => x.DriveTypeId == DriveTypeId);
            }
            return filterEx;
        }

    }
}