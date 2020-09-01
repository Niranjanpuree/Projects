using System;
using System.Linq;
using System.Linq.Expressions;
using AutoCare.Product.Infrastructure;
using AutoCare.Product.VcdbSearchIndex.Model;

namespace AutoCare.Product.Web.Models.Shared
{
    [Serializable]
    public class VehicleToBodyStyleConfigSearchFilters
    {
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string[] SubModels { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        // body
        public string[] BodyNumDoors { get; set; }
        public string[] BodyTypes { get; set; }

        public int[] VehicleIds { get; set; }
        public int BodyStyleConfigId { get; set; }

        // make query string for azure search
        public string ToAzureSearchFilter()
        {
            Expression<Func<VehicleToBodyStyleConfigDocument, bool>> filterEx = this.AddFilter();
            return filterEx?.ToAzureSearchFilter();
        }

        private Expression<Func<VehicleToBodyStyleConfigDocument, bool>> AddFilter()
        {
            Expression<Func<VehicleToBodyStyleConfigDocument, bool>> filterEx = null;

            if (Makes != null && Makes.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> makeFilterEx =
                    Makes.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, make) => current.OrElse(x => x.MakeName == make));
                filterEx = filterEx.AndAlso(makeFilterEx);
            }

            if (Models != null && Models.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> modelFilterEx =
                    Models.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, model) => current.OrElse(x => x.ModelName == model));
                filterEx = filterEx.AndAlso(modelFilterEx);
            }

            if (SubModels != null && SubModels.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> subModelsFilterEx =
                    SubModels.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, subModel) => current.OrElse(x => x.SubModelName == subModel));
                filterEx = filterEx.AndAlso(subModelsFilterEx);
            }

            if (StartYear != 0)
                filterEx = filterEx.AndAlso(x => x.YearId >= StartYear);

            if (EndYear != 0)
                filterEx = filterEx.AndAlso(x => x.YearId <= EndYear);

            if (Regions != null && Regions.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> regionsFilterEx =
                    Regions.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, region) => current.OrElse(x => x.RegionName == region));
                filterEx = filterEx.AndAlso(regionsFilterEx);
            }

            if (VehicleTypes != null && VehicleTypes.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> vehicleTypeFilterEx =
                    VehicleTypes.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, vehicleType) => current.OrElse(x => x.VehicleTypeName == vehicleType));
                filterEx = filterEx.AndAlso(vehicleTypeFilterEx);
            }

            if (VehicleTypeGroups != null && VehicleTypeGroups.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> vehicleTypeGroupFilterEx =
                    VehicleTypeGroups.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, vehicleTypeGroup) => current.OrElse(x => x.VehicleTypeGroupName == vehicleTypeGroup));
                filterEx = filterEx.AndAlso(vehicleTypeGroupFilterEx);
            }

            // body
            if (BodyNumDoors != null && BodyNumDoors.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> bodyNumDoorsFilterEx =
                    BodyNumDoors.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, bodyNumDoor) => current.OrElse(x => x.BodyNumDoors == bodyNumDoor));
                filterEx = filterEx.AndAlso(bodyNumDoorsFilterEx);
            }

            if (BodyTypes != null && BodyTypes.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> bodyTypesFilterEx =
                    BodyTypes.Aggregate<string, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                    (current, bodyType) => current.OrElse(x => x.BodyTypeName == bodyType));
                filterEx = filterEx.AndAlso(bodyTypesFilterEx);
            }

            // others
            if (VehicleIds != null && VehicleIds.Any())
            {
                Expression<Func<VehicleToBodyStyleConfigDocument, bool>> vehicleIdFilterEx =
                    VehicleIds.Aggregate<int, Expression<Func<VehicleToBodyStyleConfigDocument, bool>>>(null,
                        (current, vehicleId) => current.OrElse(x => x.VehicleId == vehicleId));
                filterEx = filterEx.AndAlso(vehicleIdFilterEx);
            }

            if (BodyStyleConfigId != 0)
                filterEx = filterEx.AndAlso(x => x.BodyStyleConfigId == BodyStyleConfigId);

            return filterEx;
        }
    }
}