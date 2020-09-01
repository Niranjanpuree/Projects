using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.BusinessServices;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.BusinessServices.EventHandler;
using AutoCare.Product.Application.BusinessServices.Padb;
using AutoCare.Product.Application.BusinessServices.Vcdb;
using AutoCare.Product.Application.Models.EntityModels;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Logging;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using AutoCare.Product.VcdbSearch.RepositoryService;
using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;
using AutoCare.Product.Web.Personify.ImsService;
using AutoCare.Product.Web.Personify.SsoService;
using AutoCare.Product.Web.ViewModelMappers;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using NLog;
using Unity.Mvc5;

namespace AutoCare.Product.Web
{
    public static class UnityConfig
    {
        public static IUnityContainer Container { get; private set; }

        public static void RegisterComponents()
        {
            Container = new UnityContainer();

            var defaultLogger = LogManager.GetLogger("Default");
            Container.RegisterInstance<ILogger>(defaultLogger);

            var defaultAutoMapper = Mapper.Instance;
            Container.RegisterInstance<IMapper>(defaultAutoMapper);

            var personifyConfiguration = new PersonifyConfiguration();
            Container.RegisterInstance<PersonifyConfiguration>(personifyConfiguration);

            var eventLocalBus = new EventLocalBus();
            Container.RegisterInstance<IEventBus>(eventLocalBus);

            Container.RegisterType<ITextSerializer, JsonTextSerializer>(new TransientLifetimeManager());

            Container.RegisterType<IApplicationLogger, ApplicationLoggerNLog>(new TransientLifetimeManager());

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            #region Application Service and Business Service
            Container.RegisterType<IEngineDesignationApplicationService, EngineDesignationApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IEngineDesignationBusinessService, EngineDesignationBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IEngineVersionApplicationService, EngineVersionApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IEngineVersionBusinessService, EngineVersionBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IEngineVinApplicationService, EngineVinApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IEngineVinBusinessService, EngineVinBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IFuelTypeApplicationService, FuelTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelTypeBusinessService, FuelTypeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBedLengthApplicationService, BedLengthApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBedLengthBusinessService, BedLengthBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IWheelBaseApplicationService, WheelBaseApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IWheelBaseBusinessService, WheelBaseBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBedTypeApplicationService, BedTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBedTypeBusinessService, BedTypeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IMakeApplicationService, MakeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IMakeBusinessService, MakeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IYearApplicationService, YearApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IYearBusinessService, YearBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IModelApplicationService, ModelApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IModelBusinessService, ModelBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<ILikeStagingApplicationService, LikeStagingApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<ILikeStagingBusinessService, LikeStagingBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleTypeApplicationService, VehicleTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleTypeBusinessService, VehicleTypeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleTypeGroupApplicationService, VehicleTypeGroupApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleTypeGroupBusinessService, VehicleTypeGroupBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<ISubModelApplicationService, SubModelApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<ISubModelBusinessService, SubModelBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<ISourceApplicationService, SourceApplicationService>(new TransientLifetimeManager());

            Container.RegisterType<IRegionApplicationService, RegionApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IRegionBusinessService, RegionBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBaseVehicleApplicationService, BaseVehicleApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBaseVehicleBusinessService, BaseVehicleBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IPublicationStageApplicationService, PublicationStageApplicationService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleApplicationService, VehicleApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleBusinessService, VehicleBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IMakeApplicationService, MakeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IMakeBusinessService, MakeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IModelApplicationService, ModelApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IModelBusinessService, ModelBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<ISubModelApplicationService, SubModelApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<ISubModelBusinessService, SubModelBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBrakeTypeApplicationService, BrakeTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeTypeBusinessService, BrakeTypeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBrakeSystemApplicationService, BrakeSystemApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeSystemBusinessService, BrakeSystemBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBrakeABSApplicationService, BrakeABSApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeABSBusinessService, BrakeABSBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBrakeConfigApplicationService, BrakeConfigApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeConfigBusinessService, BrakeConfigBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleToBrakeConfigApplicationService, VehicleToBrakeConfigApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBrakeConfigBusinessService, VehicleToBrakeConfigBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleToWheelBaseApplicationService, VehicleToWheelBaseApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseBusinessService, VehicleToWheelBaseBusinessService>(new TransientLifetimeManager());


            Container.RegisterType<IBedConfigApplicationService, BedConfigApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBedConfigBusinessService, BedConfigBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBodyStyleConfigApplicationService, BodyStyleConfigApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBodyStyleConfigBusinessService, BodyStyleConfigBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBodyTypeApplicationService, BodyTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBodyTypeBusinessService, BodyTypeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IFuelDeliverySubTypeApplicationService, FuelDeliverySubTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelDeliverySubTypeBusinessService, FuelDeliverySubTypeBusinessService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelDeliveryTypeApplicationService, FuelDeliveryTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelDeliveryTypeBusinessService, FuelDeliveryTypeBusinessService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelSystemControlTypeApplicationService, FuelSystemControlTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelSystemControlTypeBusinessService, FuelSystemControlTypeBusinessService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelSystemDesignApplicationService, FuelSystemDesignApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IFuelSystemDesignBusinessService, FuelSystemDesignBusinessService>(new TransientLifetimeManager());


            #endregion

            Container.RegisterType<IVehicleToBedConfigApplicationService, VehicleToBedConfigApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBedConfigBusinessService, VehicleToBedConfigBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IBodyNumDoorsApplicationService, BodyNumDoorsApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IBodyNumDoorsBusinessService,BodyNumDoorsBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleToBodyStyleConfigApplicationService, VehicleToBodyStyleConfigApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBodyStyleConfigBusinessService, VehicleToBodyStyleConfigBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleToWheelBaseApplicationService, VehicleToWheelBaseApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseBusinessService, VehicleToWheelBaseBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IMfrBodyCodeApplicationService, MfrBodyCodeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IMfrBodyCodeBusinessService, MfrBodyCodeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleToMfrBodyCodeApplicationService, VehicleToMfrBodyCodeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToMfrBodyCodeBusinessService, VehicleToMfrBodyCodeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IDriveTypeApplicationService, DriveTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IDriveTypeBusinessService, DriveTypeBusinessService>(new TransientLifetimeManager());

            Container.RegisterType<IVehicleToDriveTypeApplicationService, VehicleToDriveTypeApplicationService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToDriveTypeBusinessService, VehicleToDriveTypeBusinessService>(new TransientLifetimeManager());


            Container.RegisterType<IVcdbApproveChangeRequestProcessor, VcdbApproveChangeRequestProcessor>(new TransientLifetimeManager());
            Container.RegisterType<IVcdbRejectChangeRequestProcessor, VcdbRejectChangeRequestProcessor>(new TransientLifetimeManager());
            Container.RegisterType<IVcdbPreliminaryApproveChangeRequestProcessor, VcdbPreliminaryApproveChangeRequestProcessor>(new TransientLifetimeManager());
            Container.RegisterType<IVcdbDeleteChangeRequestProcessor, VcdbDeleteChangeRequestProcessor>(new TransientLifetimeManager());

            #region Data Indexer
            Container.RegisterType<IBaseVehicleDataIndexer, BaseVehicleDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleDataIndexer, VehicleDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IWheelBaseDataIndexer, WheelBaseDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBrakeConfigDataIndexer, VehicleToBrakeConfigDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseDataIndexer, VehicleToWheelBaseDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBodyStyleConfigDataIndexer, VehicleToBodyStyleConfigDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeConfigDataIndexer, BrakeConfigDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IMakeDataIndexer, MakeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IModelDataIndexer, ModelDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<ISubModelDataIndexer, SubModelDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleTypeDataIndexer, VehicleTypeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleTypeGroupDataIndexer, VehicleTypeGroupDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeSystemDataIndexer, BrakeSystemDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeTypeDataIndexer, BrakeTypeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBodyNumDoorsDataIndexer, BodyNumDoorsDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBodyTypeDataIndexer, BodyTypeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IRegionDataIndexer, RegionDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IYearDataIndexer, YearDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBrakeABSDataIndexer, BrakeABSDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBedLengthDataIndexer, BedLengthDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBedTypeDataIndexer, BedTypeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBedConfigDataIndexer, BedConfigDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBedConfigDataIndexer, VehicleToBedConfigDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBodyStyleConfigDataIndexer, BodyStyleConfigDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IBodyTypeDataIndexer, BodyTypeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IMfrBodyCodeDataIndexer, MfrBodyCodeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToMfrBodyCodeDataIndexer, VehicleToMfrBodyCodeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IDriveTypeDataIndexer, DriveTypeDataIndexer>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToDriveTypeDataIndexer, VehicleToDriveTypeDataIndexer>(
                new TransientLifetimeManager());
            #endregion

            #region Event Handler
            Container.RegisterType<IEventHandler<ApprovedEvent<BaseVehicle>>, BaseVehicleChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BaseVehicle>>, BaseVehicleChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BaseVehicle>>, BaseVehicleChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BedLength>>, BedLengthChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BedLength>>, BedLengthChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BedLength>>, BedLengthChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BodyNumDoors>>, BodyNumDoorsChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BodyNumDoors>>, BodyNumDoorsChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BodyNumDoors>>, BodyNumDoorsChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BodyType>>, BodyTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BodyType>>, BodyTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BodyType>>, BodyTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BedConfig>>, BedConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BedConfig>>, BedConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BedConfig>>, BedConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BrakeABS>>, BrakeABSChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BrakeABS>>, BrakeABSChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BrakeABS>>, BrakeABSChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BrakeConfig>>, BrakeConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BrakeConfig>>, BrakeConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BrakeConfig>>, BrakeConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BrakeSystem>>, BrakeSystemChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BrakeSystem>>, BrakeSystemChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BrakeSystem>>, BrakeSystemChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BrakeType>>, BrakeTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BrakeType>>, BrakeTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BrakeType>>, BrakeTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BedType>>, BedTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BedType>>, BedTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BedType>>, BedTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<Vehicle>>, VehicleChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<Vehicle>>, VehicleChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<Vehicle>>, VehicleChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleToBrakeConfig>>, VehicleToBrakeConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleToBrakeConfig>>, VehicleToBrakeConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleToBrakeConfig>>, VehicleToBrakeConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleToBodyStyleConfig>>, VehicleToBodyStyleConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleToBodyStyleConfig>>, VehicleToBodyStyleConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleToBodyStyleConfig>>, VehicleToBodyStyleConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleToWheelBase>>, VehicleToWheelBaseChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleToWheelBase>>, VehicleToWheelBaseChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleToWheelBase>>, VehicleToWheelBaseChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleType>>, VehicleTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleType>>, VehicleTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleType>>, VehicleTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleTypeGroup>>, VehicleTypeGroupChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleTypeGroup>>, VehicleTypeGroupChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleTypeGroup>>, VehicleTypeGroupChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<Make>>, MakeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<Make>>, MakeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<Make>>, MakeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<Model>>, ModelChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<Model>>, ModelChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<Model>>, ModelChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<SubModel>>, SubModelChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<SubModel>>, SubModelChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<SubModel>>, SubModelChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<Region>>, RegionChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<Region>>, RegionChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<Region>>, RegionChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<Year>>, YearChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<Year>>, YearChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<Year>>, YearChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<BodyStyleConfig>>, BodyStyleConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<BodyStyleConfig>>, BodyStyleConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<BodyStyleConfig>>, BodyStyleConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleToBedConfig>>, VehicleToBedConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleToBedConfig>>, VehicleToBedConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleToBedConfig>>, VehicleToBedConfigChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<WheelBase>>, WheelBaseChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<WheelBase>>, WheelBaseChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<WheelBase>>, WheelBaseChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<MfrBodyCode>>, MfrBodyCodeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<MfrBodyCode>>, MfrBodyCodeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<MfrBodyCode>>, MfrBodyCodeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<DriveType>>, DriveTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<DriveType>>, DriveTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<DriveType>>, DriveTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleToDriveType>>, VehicleToDriveTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleToDriveType>>, VehicleToDriveTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleToDriveType>>, VehicleToDriveTypeChangeRequestReviewEventHandler>(new TransientLifetimeManager());

            Container.RegisterType<IEventHandler<ApprovedEvent<VehicleToMfrBodyCode>>, VehicleToMfrBodyCodeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<RejectedEvent<VehicleToMfrBodyCode>>, VehicleToMfrBodyCodeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            Container.RegisterType<IEventHandler<DeletedEvent<VehicleToMfrBodyCode>>, VehicleToMfrBodyCodeChangeRequestReviewEventHandler>(new TransientLifetimeManager());
            #endregion

            #region Azure Search
            Container.RegisterType<IVehicleSearchService, VehicleSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleSearchRepositoryService, VehicleSearchAzureRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicles"));
            Container.RegisterType<IVehicleIndexingService, VehicleIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleIndexingRepositoryService, VehicleIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicles"));

            Container.RegisterType<IChangeRequestSearchService, ChangeRequestSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IChangeRequestSearchRepositoryService, ChangeRequestSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "changerequests"));
            Container.RegisterType<IChangeRequestSearchViewModelMapper, ChangeRequestSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IChangeRequestIndexingService, ChangeRequestIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IChangeRequestIndexingRepositoryService, ChangeRequestIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "changerequests"));

            Container.RegisterType<IVehicleToBrakeConfigSearchService, VehicleToBrakeConfigSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBrakeConfigSearchRepositoryService, VehicleToBrakeConfigSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobrakeconfigs"));
            Container.RegisterType<IVehicleToBrakeConfigIndexingService, VehicleToBrakeConfigIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBrakeConfigIndexingRepositoryService, VehicleToBrakeConfigIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobrakeconfigs"));

            Container.RegisterType<IVehicleToBedConfigSearchService, VehicleToBedConfigSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBedConfigSearchRepositoryService, VehicleToBedConfigSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobedconfigs"));
            Container.RegisterType<IVehicleToBedConfigIndexingService, VehicleToBedConfigIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBedConfigIndexingRepositoryService, VehicleToBedConfigIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobedconfigs"));

            Container.RegisterType<IVehicleToBodyStyleConfigSearchService, VehicleToBodyStyleConfigSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBodyStyleConfigSearchRepositoryService, VehicleToBodyStyleConfigSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobodystyleconfigs"));
            Container.RegisterType<IVehicleToBodyStyleConfigIndexingService, VehicleToBodyStyleConfigIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBodyStyleConfigIndexingRepositoryService, VehicleToBodyStyleConfigIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletobodystyleconfigs"));

            Container.RegisterType<IVehicleToMfrBodyCodeSearchService, VehicleToMfrBodyCodeSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToMfrBodyCodeSearchRepositoryService, VehicleToMfrBodyCodeSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletomfrbodycodes"));
            Container.RegisterType<IVehicleToMfrBodyCodeIndexingService, VehicleToMfrBodyCodeIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToMfrBodyCodeIndexingRepositoryService, VehicleToMfrBodyCodeIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletomfrbodycodes"));

            Container.RegisterType<IVehicleToDriveTypeSearchService, VehicleToDriveTypeSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToDriveTypeSearchRepositoryService, VehicleToDriveTypeSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletodrivetypes"));
            Container.RegisterType<IVehicleToDriveTypeIndexingService, VehicleToDriveTypeIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToDriveTypeIndexingRepositoryService, VehicleToDriveTypeIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletodrivetypes"));


            Container.RegisterType<IVehicleToWheelBaseSearchService, VehicleToWheelBaseSearchService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseSearchRepositoryService, VehicleToWheelBaseSearchRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletowheelbases"));
            Container.RegisterType<IVehicleToWheelBaseIndexingService, VehicleToWheelBaseIndexingService>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseIndexingRepositoryService, VehicleToWheelBaseIndexingRepositoryService>(new TransientLifetimeManager(),
                new InjectionConstructor("optimussearch", "24C77889585CFB6E756E7783DE693438", "vehicletowheelbases"));

            
            Container.RegisterType<IVehicleSearchViewModelMapper, VehicleSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBrakeConfigSearchViewModelMapper, VehicleToBrakeConfigSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBrakeConfigViewModelMapper, VehicleToBrakeConfigViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBedConfigSearchViewModelMapper, VehicleToBedConfigSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBedConfigViewModelMapper, VehicleToBedConfigViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBodyStyleConfigSearchViewModelMapper, VehicleToBodyStyleConfigSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToBodyStyleConfigViewModelMapper, VehicleToBodyStyleConfigViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToMfrBodyCodeSearchViewModelMapper, VehicleToMfrBodyCodeSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToMfrBodyCodeViewModelMapper, VehicleToMfrBodyCodeViewModelMapper>(new TransientLifetimeManager());
            //Container.RegisterType<IVehicleToDriveTypeSearchViewModelMapper, VehicleToDriveTypeSearchViewModelMapper>(new TransientLifetimeManager());
            //Container.RegisterType<IVehicleToDriveTypeViewModelMapper, VehicleToDriveTypeViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseSearchViewModelMapper, VehicleToWheelBaseSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToWheelBaseViewModelMapper, VehicleToWheelBaseViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToDriveTypeSearchViewModelMapper, VehicleToDriveTypeSearchViewModelMapper>(new TransientLifetimeManager());
            Container.RegisterType<IVehicleToDriveTypeViewModelMapper, VehicleToDriveTypeViewModelMapper>(new TransientLifetimeManager());
            #endregion

            Container.RegisterType<DbContext, VehicleConfigurationContext>("vcdbContext", new TransientLifetimeManager());
            //Container.RegisterType<DbContext, PadbContext>("pcdbContext", new TransientLifetimeManager());

            Container.RegisterType(typeof(IVcdbSqlServerEfRepositoryService<>), typeof(VcdbSqlServerEfRepositoryService<>),
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<DbContext>("vcdbContext"), typeof(ITextSerializer)));

            Container.RegisterType(typeof(IVehicleToBrakeConfigRepositoryService), typeof(VehicleToBrakeConfigRepositoryService),
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<DbContext>("vcdbContext"), typeof(ITextSerializer)));

            Container.RegisterType(typeof(IModelSqlServerEfRepositoryService), typeof(ModelSqlServerEfRepositoryService),
                new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<DbContext>("vcdbContext"), typeof(ITextSerializer)));


            Container.RegisterType(typeof(IVcdbBusinessService<>), typeof(VcdbBusinessService<>),
                new TransientLifetimeManager());

            Container.RegisterType<IJwtTokenHelper, JwtTokenHelper>(new TransientLifetimeManager());

            Container.RegisterType(typeof(IUserStore<AutoCareUser>), typeof(AutoCareUserStore),
                new TransientLifetimeManager());

            Container.RegisterType(typeof(IAuthenticationManager),
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            Container.RegisterType<IPersonifyHelper, PersonifyHelper>(new TransientLifetimeManager(),
                new InjectionConstructor(typeof(PersonifyConfiguration),
                    new InjectionParameter<serviceSoapClient>(null), new InjectionParameter<MServiceSoapClient>(null)));

            Container.RegisterType<IVcdbUnitOfWork, VcdbUnitOfWork>(new TransientLifetimeManager(),
                new InjectionConstructor(new ResolvedParameter<DbContext>("vcdbContext"), typeof(ITextSerializer), Container));

            Container.RegisterType(typeof(IAzureFileStorageRepositoryService), typeof(AzureFileStorageRepositoryService),
                new TransientLifetimeManager());

            Container.RegisterType<IAzureFileStorageBusinessService, AzureFileStorageBusinessService>(new TransientLifetimeManager());
            Container.RegisterType<IAzureFileStorageApplicationService, AzureFileStorageApplicationService>(new TransientLifetimeManager());

            VcdbChangeRequestDependencyRegistration();

            //Padb Configuration for future
            Container.RegisterType(typeof(IPadbSqlServerEfRepositoryService<>), typeof(PadbSqlServerEfRepositoryService<>),
               new TransientLifetimeManager(),
               new InjectionConstructor(new ResolvedParameter<DbContext>("padbContext"), typeof(ITextSerializer)));

            PadbChangeRequestDependencyRegistration();
            DependencyResolver.SetResolver(new UnityDependencyResolver(Container));
        }

        private static void VcdbChangeRequestDependencyRegistration()
        {
            Container.RegisterType(typeof(IVcdbChangeRequestBusinessService), typeof(VcdbChangeRequestBusinessService),
                new TransientLifetimeManager());

            Container.RegisterType(typeof(IVcdbChangeRequestItemBusinessService), typeof(VcdbChangeRequestItemBusinessService),
                new TransientLifetimeManager());

            Container.RegisterType(typeof(IVcdbChangeRequestService), typeof(VcdbChangeRequestService),
                new TransientLifetimeManager());

            Container.RegisterType(typeof(IVcdbChangeRequestCommentsBusinessService), typeof(VcdbChangeRequestCommentsBusinessService),
                new TransientLifetimeManager());

            Container.RegisterType(typeof(IVcdbChangeRequestAttachmentBusinessService), typeof(VcdbChangeRequestAttachmentBusinessService),
                new TransientLifetimeManager());
        }

        private static void PadbChangeRequestDependencyRegistration()
        {
            Container.RegisterType(typeof(IPadbChangeRequestBusinessService<,,,>), typeof(PadbChangeRequestBusinessService<,,,>),
                new TransientLifetimeManager());

            Container.RegisterType(typeof(IPadbChangeRequestService<,,,>), typeof(PadbChangeRequestService<,,,>),
                new TransientLifetimeManager());
        }
    }
}