using System;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;
using System.Linq;

namespace AutoCare.Product.Web.Models.Shared
{
    [Serializable]
    public class VehicleToWheelBaseSearchFilters
    {
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] WheelBase { get; set; }
        public string[] WheelBaseMetric { get; set; }
        public int[] VehicleIds { get; set; }
        public int WheelBaseId { get; set; }


        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx = null;

            filterEx = AddStartYearFilter(filterEx);

            filterEx = AddEndYearFilter(filterEx);

            filterEx = AddMakesFilter(filterEx);

            filterEx = AddModelsFilter(filterEx);

            filterEx = AddSubModelsFilter(filterEx);

            filterEx = AddVehicleTypeFilter(filterEx);

            filterEx = AddVehicleTypeGroupFilter(filterEx);

            filterEx = AddRegionFilter(filterEx);

            filterEx = AddWheelBaseFilter(filterEx);

            filterEx = AddWheelBaseIdFilter(filterEx);

            filterEx = AddWheelBaseMetricFilter(filterEx);

            filterEx = AddVehicleIdsFilter(filterEx);

            return filterEx?.ToAzureSearchFilter();
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddStartYearFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (StartYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);     //TODO: pushkar: check if AndAlso() will work
            }
            return filterEx;
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddEndYearFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (EndYear != 0)
            {
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);
            }
            return filterEx;
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddMakesFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (Makes == null || !Makes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> makeFilterEx = null;
            foreach (var make in Makes)
            {
                makeFilterEx = makeFilterEx.OrElse(x => x.MakeName == make);
            }

            return filterEx.AndAlso(makeFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddModelsFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (Models == null || !Models.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> modelFilterEx = null;
            foreach (var model in Models)
            {
                modelFilterEx = modelFilterEx.OrElse(x => x.ModelName == model);
            }

            return filterEx.AndAlso(modelFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddSubModelsFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (SubModels == null || !SubModels.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> subModelsFilterEx = null;
            foreach (var subModel in SubModels)
            {
                subModelsFilterEx = subModelsFilterEx.OrElse(x => x.SubModelName == subModel);
            }

            return filterEx.AndAlso(subModelsFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddVehicleTypeFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (VehicleTypes == null || !VehicleTypes.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> vehicleTypeFilterEx = null;
            foreach (var vehicleType in VehicleTypes)
            {
                vehicleTypeFilterEx = vehicleTypeFilterEx.OrElse(x => x.VehicleTypeName == vehicleType);
            }

            return filterEx.AndAlso(vehicleTypeFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddVehicleTypeGroupFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (VehicleTypeGroups == null || !VehicleTypeGroups.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> vehicleTypeGroupFilterEx = null;
            foreach (var vehicleTypeGroup in VehicleTypeGroups)
            {
                vehicleTypeGroupFilterEx = vehicleTypeGroupFilterEx.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup);
            }

            return filterEx.AndAlso(vehicleTypeGroupFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddRegionFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (Regions == null || !Regions.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> regionFilterEx = null;
            foreach (var region in Regions)
            {
                regionFilterEx = regionFilterEx.OrElse(x => x.RegionName == region);
            }

            return filterEx.AndAlso(regionFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddWheelBaseFilter(
            Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (WheelBaseMetric == null || !WheelBaseMetric.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> wheelBaseMetricFilterEx = null;
            foreach (var wheelBaseMetric in WheelBaseMetric)
            {
                wheelBaseMetricFilterEx = wheelBaseMetricFilterEx.OrElse(x => x.WheelBaseMetric == wheelBaseMetric);
            }

            return filterEx.AndAlso(wheelBaseMetricFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddWheelBaseMetricFilter(
           Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (WheelBase == null || !WheelBase.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> wheelBaseFilterEx = null;
            foreach (var wheelBase in WheelBase)
            {
                wheelBaseFilterEx = wheelBaseFilterEx.OrElse(x => x.WheelBaseName == wheelBase);
            }

            return filterEx.AndAlso(wheelBaseFilterEx);
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddWheelBaseIdFilter(Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (WheelBaseId != 0)
            {
                filterEx = filterEx.AndAlso(x => x.WheelBaseId == WheelBaseId);
            }
            return filterEx;
        }

        private Expression<Func<VehicleToWheelBaseDocument, bool>> AddVehicleIdsFilter(
            Expression<Func<VehicleToWheelBaseDocument, bool>> filterEx)
        {
            if (VehicleIds == null || !VehicleIds.Any())
            {
                return filterEx;
            }

            Expression<Func<VehicleToWheelBaseDocument, bool>> vehicleIdFilterEx = null;
            foreach (var vehicleId in VehicleIds)
            {
                vehicleIdFilterEx = vehicleIdFilterEx.OrElse(x => x.VehicleId == vehicleId);
            }

            return filterEx.AndAlso(vehicleIdFilterEx);
        }

    }
}