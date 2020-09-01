using System.Linq;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.InputModels;
using AutoCare.Product.Web.Models.ViewModels;
using AutoMapper;

namespace AutoCare.Product.Web
{
    public class AutoMapperConfig
    {
        public static void CreateMap()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Year, YearViewModel>();
                config.CreateMap<Year, YearInputModel>();
                config.CreateMap<Make, MakeViewModel>();

                config.CreateMap<VehicleType, VehicleTypeViewModel>()
                .ForMember(dest => dest.VehicleTypeGroupName, opt => opt.MapFrom(src => src.VehicleTypeGroup.Name));

                config.CreateMap<Model, ModelViewModel>()
                .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.Name));
                config.CreateMap<Model, ModelDetailViewModel>()
                    .ForMember(dest => dest.VehicleTypeName, opt => opt.MapFrom(src => src.VehicleType.Name));

                config.CreateMap<SubModel, SubModelViewModel>();
                config.CreateMap<BodyNumDoors, BodyNumDoorsViewModel>();
                config.CreateMap<BodyNumDoors, BodyNumDoorsDetailViewModel>();
                config.CreateMap<EngineDesignation, EngineDesignationViewModel>()
                .ForMember(dest => dest.EngineDesignationId, opt => opt.MapFrom(src => src.Id));
                config.CreateMap<EngineDesignation, EngineDesignationDetailViewModel>();

                config.CreateMap<FuelType, FuelTypeViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FuelTypeName));


                config.CreateMap<EngineVin, EngineVinViewModel>()
                .ForMember(dest => dest.EngineVinId, opt => opt.MapFrom(src => src.Id));
                config.CreateMap<EngineVin, EngineVinDetailViewModel>();

                config.CreateMap<EngineVersion, EngineVersionViewModel>()
                .ForMember(dest => dest.EngineVersionId, opt => opt.MapFrom(src => src.Id));
                config.CreateMap<EngineVersion, EngineVersionDetailViewModel>();
                config.CreateMap<BodyType, BodyTypeViewModel>();


                config.CreateMap<Source, SourceViewModel>();
                config.CreateMap<PublicationStage, PublicationStageViewModel>();

                config.CreateMap<Region, RegionViewModel>();
                config.CreateMap<Region, ParentRegionViewModel>();
                config.CreateMap<ReviewViewModel, ChangeRequestStagingWheelBaseViewModel>();
                config.CreateMap<ReviewViewModel, ChangeRequestStagingEngineDesignationViewModel>();
                config.CreateMap<ReviewViewModel, ChangeRequestStagingFuelTypeViewModel>();
                config.CreateMap<ReviewViewModel, ChangeRequestStagingEngineVinViewModel>();
                config.CreateMap<ReviewViewModel, ChangeRequestStagingEngineVersionViewModel>();

                config.CreateMap<Vehicle, VehicleViewModel>()
                .ForMember(dest => dest.SubModelName, opt => opt.MapFrom(src => src.SubModel.Name))
                .ForMember(dest => dest.SourceName, opt => opt.MapFrom(src => src.Source.Name))
                .ForMember(dest => dest.PublicationStageName, opt => opt.MapFrom(src => src.PublicationStage.Name))
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region.Name))
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.BaseVehicle.MakeId))
                .ForMember(dest => dest.MakeName, opt => opt.MapFrom(src => src.BaseVehicle.Make.Name))
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.BaseVehicle.ModelId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.BaseVehicle.Model.Name))
                .ForMember(dest => dest.YearId, opt => opt.MapFrom(src => src.BaseVehicle.YearId));

                config.CreateMap<BaseVehicle, BaseVehicleViewModel>();

                config.CreateMap<BrakeType, BrakeTypeViewModel>();
                config.CreateMap<WheelBase, WheelBaseViewModel>();
                config.CreateMap<BedLength, BedLengthViewModel>();
                config.CreateMap<BedLength, BedLengthDetailViewModel>();
                config.CreateMap<BedType, BedTypeViewModel>();
                config.CreateMap<BedType, BedTypeDetailViewModel>();

                config.CreateMap<BedConfig, BedConfigViewModel>()
                .ForMember(dest => dest.Length, opt => opt.MapFrom(src => src.BedLength.Length))
                .ForMember(dest => dest.BedLengthMetric, opt => opt.MapFrom(src => src.BedLength.BedLengthMetric));

                config.CreateMap<BodyStyleConfig, BodyStyleConfigViewModel>()
                .ForMember(dest => dest.NumDoors, opt => opt.MapFrom(src => src.BodyNumDoors.NumDoors));

                config.CreateMap<BrakeType, BrakeTypeDetailViewModel>();
                config.CreateMap<WheelBase, WheelBaseDetailViewModel>();

                config.CreateMap<BrakeSystem, BrakeSystemViewModel>();
                config.CreateMap<BrakeSystem, BrakeSystemDetailViewModel>();

                config.CreateMap<BrakeABS, BrakeABSViewModel>();
                config.CreateMap<BrakeABS, BrakeABSDetailViewModel>();

                config.CreateMap<BodyType, BodyTypeViewModel>();
                config.CreateMap<BodyType, BodyTypeDetailViewModel>();
                config.CreateMap<BodyStyleConfig, BodyStyleConfigViewModel>()
                    .ForMember(dest => dest.NumDoors, opt => opt.MapFrom(src => src.BodyNumDoors.NumDoors));
                config.CreateMap<MfrBodyCode, MfrBodyCodeViewModel>();
                config.CreateMap<DriveType, DriveTypeViewModel>();
                config.CreateMap<BrakeConfig, BrakeConfigViewModel>()
                    .ForMember(dest => dest.FrontBrakeTypeName, opt => opt.MapFrom(src => src.FrontBrakeType.Name))
                    .ForMember(dest => dest.RearBrakeTypeName, opt => opt.MapFrom(src => src.RearBrakeType.Name))
                    .ForMember(dest => dest.BrakeSystemName, opt => opt.MapFrom(src => src.BrakeSystem.Name))
                    .ForMember(dest => dest.BrakeABSName, opt => opt.MapFrom(src => src.BrakeABS.Name));

                config.CreateMap<VehicleTypeGroup, VehicleTypeGroupViewModel>();

                config.CreateMap<BaseVehicle, ModelViewModel>();
                config.CreateMap<LikesModel, LikesViewModel>();

                config.CreateMap<BaseVehicle, ModelViewModel>()
               .ForMember(dest => dest.BaseVehicleId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ModelId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model.Name));

                config.CreateMap<VehicleToBrakeConfig, VehicleToBrakeConfigViewModel>();
                config.CreateMap<VehicleToBedConfig, VehicleToBedConfigViewModel>();

                config.CreateMap<VehicleToMfrBodyCode, VehicleToMfrBodyCodeViewModel>();
                config.CreateMap<VehicleToBodyStyleConfig, VehicleToBodyStyleConfigViewModel>();
                config.CreateMap<VehicleToWheelBase, VehicleToWheelBaseViewModel>();
                config.CreateMap<VehicleToDriveType, VehicleToDriveTypeViewModel>();

                config.CreateMap<FuelDeliverySubType, FuelDeliverySubTypeViewModel>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FuelDeliverySubTypeName));

                #region Change Request
                config.CreateMap<ChangeRequestStaging, ChangeRequestStore>()
                .ForMember(dest => dest.RequestedDateTime, opt => opt.MapFrom(src => src.CreatedDateTime));

                config.CreateMap<ChangeRequestItemStaging, ChangeRequestItem>()
                .ForMember(dest => dest.ChangeRequestItemId, opt => opt.MapFrom(src => src.Id));

                config.CreateMap<CommentsStaging, Comments>();
                config.CreateMap<AttachmentsStaging, Attachments>();

                config.CreateMap<CommentsStagingModel, CommentsStagingViewModel>();
                config.CreateMap<AttachmentsModel, AttachmentsStagingViewModel>();

                config.CreateMap<ChangeRequestStagingReviewModel, ChangeRequestStagingReviewViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BodyNumDoors>, ChangeRequestStagingBodyNumDoorsViewModel>();
                config.CreateMap<WheelBaseChangeRequestStagingModel, ChangeRequestStagingWheelBaseViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BodyType>, ChangeRequestStagingBodyTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<Make>, ChangeRequestStagingMakeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<EngineDesignation>, ChangeRequestStagingEngineDesignationViewModel>();
                config.CreateMap<ChangeRequestStagingModel<FuelType>, ChangeRequestStagingFuelTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<EngineVin>, ChangeRequestStagingEngineVinViewModel>();
                config.CreateMap<ChangeRequestStagingModel<EngineVersion>, ChangeRequestStagingEngineVersionViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BedLength>, ChangeRequestStagingBedLengthViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BedType>, ChangeRequestStagingBedTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<Year>, ChangeRequestStagingYearViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BrakeType>, ChangeRequestStagingBrakeTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BrakeABS>, ChangeRequestStagingBrakeABSViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BrakeSystem>, ChangeRequestStagingBrakeSystemViewModel>();
                config.CreateMap<ChangeRequestStagingModel<SubModel>, ChangeRequestStagingSubModelViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleType>, ChangeRequestStagingVehicleTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleTypeGroup>, ChangeRequestStagingVehicleTypeGroupViewModel>();
                config.CreateMap<ChangeRequestStagingModel<Model>, ChangeRequestStagingModelViewModel>();
                config.CreateMap<ChangeRequestStagingModel<Region>, ChangeRequestStagingRegionViewModel>();
                config.CreateMap<ChangeRequestStagingModel<Vehicle>, ChangeRequestStagingVehicleViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleToBrakeConfig>, ChangeRequestStagingVehicleToBrakeConfigViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BedConfig>, ChangeRequestStagingBedConfigViewModel>();
                config.CreateMap<BaseVehicleChangeRequestStagingModel, ChangeRequestStagingBaseVehicleViewModel>();
                config.CreateMap<BrakeConfigChangeRequestStagingModel, ChangeRequestStagingBrakeConfigViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleToBedConfig>, ChangeRequestStagingVehicleToBedConfigViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleToMfrBodyCode>, ChangeRequestStagingVehicleToMfrBodyCodeViewModel>();
                config.CreateMap<BedConfigChangeRequestStagingModel, ChangeRequestStagingBedConfigViewModel>();
                config.CreateMap<ChangeRequestStagingModel<BodyStyleConfig>, ChangeRequestStagingBodyStyleConfigViewModel>();
                config.CreateMap<BodyStyleConfigChangeRequestStagingModel, ChangeRequestStagingBodyStyleConfigViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleToBodyStyleConfig>, ChangeRequestStagingVehicleToBodyStyleConfigViewModel>();
                config.CreateMap<BodyStyleConfigChangeRequestStagingModel, ChangeRequestStagingVehicleToBodyStyleConfigViewModel>();
                config.CreateMap<MfrBodyCodeChangeRequestStagingModel, ChangeRequestStagingMfrBodyCodeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<MfrBodyCode>, ChangeRequestStagingMfrBodyCodeViewModel>();
                config.CreateMap<DriveTypeChangeRequestStagingModel, ChangeRequestStagingDriveTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<DriveType>, ChangeRequestStagingDriveTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleToWheelBase>, ChangeRequestStagingVehicleToWheelBaseViewModel>();
                config.CreateMap<ChangeRequestStagingModel<VehicleToDriveType>, ChangeRequestStagingVehicleToDriveTypeViewModel>();
                config.CreateMap<ChangeRequestStagingModel<FuelDeliverySubType>, ChangeRequestStagingFuelDeliverySubTypeViewModel>();

                #endregion
            });
        }
    }

    public static class IgnoreVirtualExtensions
    {
        public static IMappingExpression<TSource, TDestination>
               IgnoreAllVirtual<TSource, TDestination>(
                   this IMappingExpression<TSource, TDestination> expression)
        {
            var desType = typeof(TDestination);
            foreach (var property in desType.GetProperties().Where(p =>
                                     p.GetGetMethod().IsVirtual))
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }

            return expression;
        }
    }
}