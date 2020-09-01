using System;
using System.Linq;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.Web.Models.Shared
{
    [Serializable]
    public class VehicleSearchFilters
    {
        public int BaseVehicleId { get; set; }
        public string VehicleId { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }

        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleDocument, bool>> filterEx = null;

            filterEx = AddBaseVehicleIdFilter(filterEx);

            filterEx = AddVehicleIdFilter(filterEx);

            filterEx = AddStartYearFilter(filterEx);

            filterEx = AddEndYearFilter(filterEx);

            filterEx = AddMakesFilter(filterEx);

            filterEx = AddModelsFilter(filterEx);

            filterEx = AddSubModelsFilter(filterEx);

            filterEx = AddVehicleTypeFilter(filterEx);

            filterEx = AddVehicleTypeGroupFilter(filterEx);

            filterEx = AddRegionFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }

        private Expression<Func<VehicleDocument, bool>> AddBaseVehicleIdFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (BaseVehicleId != 0)
            {
                filterEx = filterEx.AndAlso(x => x.BaseVehicleId == BaseVehicleId);
            }
            return filterEx;
        }

        private Expression<Func<VehicleDocument, bool>> AddVehicleIdFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (!string.IsNullOrWhiteSpace(VehicleId))
            {
                filterEx = filterEx.AndAlso(x => x.VehicleId == VehicleId);
            }
            return filterEx;
        }

        private Expression<Func<VehicleDocument, bool>> AddStartYearFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (StartYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleDocument, bool>> AddEndYearFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (EndYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleDocument, bool>> AddMakesFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (Makes == null || !Makes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleDocument, bool>> makeFilterEx = null;
            foreach (var make in Makes)
            {
                makeFilterEx = makeFilterEx.OrElse(x => x.MakeName == make);
            }

            return filterEx.AndAlso(makeFilterEx);
        }

        private Expression<Func<VehicleDocument, bool>> AddModelsFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (Models == null || !Models.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleDocument, bool>> modelFilterEx = null;
            foreach (var model in Models)
            {
                modelFilterEx = modelFilterEx.OrElse(x => x.ModelName == model);
            }

            return filterEx.AndAlso(modelFilterEx);
        }

        private Expression<Func<VehicleDocument, bool>> AddSubModelsFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (SubModels == null || !SubModels.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleDocument, bool>> subModelsFilterEx = null;
            foreach (var subModel in SubModels)
            {
                subModelsFilterEx = subModelsFilterEx.OrElse(x => x.SubModelName == subModel);
            }

            return filterEx.AndAlso(subModelsFilterEx);
        }

        private Expression<Func<VehicleDocument, bool>> AddVehicleTypeFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (VehicleTypes == null || !VehicleTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleDocument, bool>> vehicleTypeFilterEx = null;
            foreach (var vehicleType in VehicleTypes)
            {
                vehicleTypeFilterEx = vehicleTypeFilterEx.OrElse(x => x.VehicleTypeName == vehicleType);
            }

            return filterEx.AndAlso(vehicleTypeFilterEx);
        }

        private Expression<Func<VehicleDocument, bool>> AddVehicleTypeGroupFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (VehicleTypeGroups == null || !VehicleTypeGroups.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleDocument, bool>> vehicleTypeGroupFilterEx = null;
            foreach (var vehicleTypeGroup in VehicleTypeGroups)
            {
                vehicleTypeGroupFilterEx = vehicleTypeGroupFilterEx.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup);
            }

            return filterEx.AndAlso(vehicleTypeGroupFilterEx);
        }

        private Expression<Func<VehicleDocument, bool>> AddRegionFilter(Expression<Func<VehicleDocument, bool>> filterEx)
        {
            if (Regions == null || !Regions.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleDocument, bool>> regionFilterEx = null;
            foreach (var region in Regions)
            {
                regionFilterEx = regionFilterEx.OrElse(x => x.RegionName == region);
            }

            return filterEx.AndAlso(regionFilterEx);
        }
    }
}