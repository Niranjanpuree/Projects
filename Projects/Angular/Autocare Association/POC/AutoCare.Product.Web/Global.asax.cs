using System;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.NLog;
using AutoCare.Product.Vcdb.Model;
using Microsoft.Practices.Unity;
using NLog.Config;

namespace AutoCare.Product.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("utc_date",
                typeof(UtcDateLayoutRenderer));

            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //WebApiConfig.Register();
            AutoMapperConfig.CreateMap();
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "customer_id";
            RegisterEventHandlers();
        }

        private void RegisterEventHandlers()
        {
            var container = ((IUnityContainer)UnityConfig.Container);
            var eventLocalBus = container.Resolve<IEventBus>();

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BaseVehicle>, IEventHandler<ApprovedEvent<BaseVehicle>>>(
                    container.Resolve<Func<IEventHandler<ApprovedEvent<BaseVehicle>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<BaseVehicle>, IEventHandler<RejectedEvent<BaseVehicle>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<BaseVehicle>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<BaseVehicle>, IEventHandler<DeletedEvent<BaseVehicle>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<BaseVehicle>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BrakeABS>, IEventHandler<ApprovedEvent<BrakeABS>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BrakeABS>>>>());

            eventLocalBus
               .RegisterAsync
               <RejectedEvent<BrakeABS>, IEventHandler<RejectedEvent<BrakeABS>>>(
                   container.Resolve<Func<IEventHandler<RejectedEvent<BrakeABS>>>>());

            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BrakeABS>, IEventHandler<DeletedEvent<BrakeABS>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BrakeABS>>>>());
            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BodyNumDoors>, IEventHandler<ApprovedEvent<BodyNumDoors>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BodyNumDoors>>>>());

            eventLocalBus
               .RegisterAsync
               <RejectedEvent<BodyNumDoors>, IEventHandler<RejectedEvent<BodyNumDoors>>>(
                   container.Resolve<Func<IEventHandler<RejectedEvent<BodyNumDoors>>>>());

            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BodyNumDoors>, IEventHandler<DeletedEvent<BodyNumDoors>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BodyNumDoors>>>>());
            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BodyType>, IEventHandler<ApprovedEvent<BodyType>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BodyType>>>>());

            eventLocalBus
               .RegisterAsync
               <RejectedEvent<BodyType>, IEventHandler<RejectedEvent<BodyType>>>(
                   container.Resolve<Func<IEventHandler<RejectedEvent<BodyType>>>>());

            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BodyType>, IEventHandler<DeletedEvent<BodyType>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BodyType>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BrakeConfig>, IEventHandler<ApprovedEvent<BrakeConfig>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BrakeConfig>>>>());

            eventLocalBus
               .RegisterAsync
               <RejectedEvent<BrakeConfig>, IEventHandler<RejectedEvent<BrakeConfig>>>(
                   container.Resolve<Func<IEventHandler<RejectedEvent<BrakeConfig>>>>());

            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BrakeConfig>, IEventHandler<DeletedEvent<BrakeConfig>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BrakeConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BrakeSystem>, IEventHandler<ApprovedEvent<BrakeSystem>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BrakeSystem>>>>());

            eventLocalBus
               .RegisterAsync
               <RejectedEvent<BrakeSystem>, IEventHandler<RejectedEvent<BrakeSystem>>>(
                   container.Resolve<Func<IEventHandler<RejectedEvent<BrakeSystem>>>>());

            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BrakeSystem>, IEventHandler<DeletedEvent<BrakeSystem>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BrakeSystem>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<BrakeType>, IEventHandler<ApprovedEvent<BrakeType>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BrakeType>>>>());

            eventLocalBus
               .RegisterAsync
               <RejectedEvent<BrakeType>, IEventHandler<RejectedEvent<BrakeType>>>(
                   container.Resolve<Func<IEventHandler<RejectedEvent<BrakeType>>>>());

            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BrakeType>, IEventHandler<DeletedEvent<BrakeType>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BrakeType>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<Vehicle>, IEventHandler<ApprovedEvent<Vehicle>>>(
                    container.Resolve<Func<IEventHandler<ApprovedEvent<Vehicle>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<Vehicle>, IEventHandler<RejectedEvent<Vehicle>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<Vehicle>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<Vehicle>, IEventHandler<DeletedEvent<Vehicle>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<Vehicle>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<VehicleToBrakeConfig>, IEventHandler<ApprovedEvent<VehicleToBrakeConfig>>>(
                    container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleToBrakeConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleToBrakeConfig>, IEventHandler<RejectedEvent<VehicleToBrakeConfig>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleToBrakeConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<VehicleToBrakeConfig>, IEventHandler<DeletedEvent<VehicleToBrakeConfig>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<VehicleToBrakeConfig>>>>());

            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<VehicleToBodyStyleConfig>, IEventHandler<ApprovedEvent<VehicleToBodyStyleConfig>>>(
                  container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleToBodyStyleConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleToBodyStyleConfig>, IEventHandler<RejectedEvent<VehicleToBodyStyleConfig>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleToBodyStyleConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<VehicleToBodyStyleConfig>, IEventHandler<DeletedEvent<VehicleToBodyStyleConfig>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<VehicleToBodyStyleConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<VehicleType>, IEventHandler<ApprovedEvent<VehicleType>>>(
                    container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleType>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleType>, IEventHandler<RejectedEvent<VehicleType>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleType>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<VehicleType>, IEventHandler<DeletedEvent<VehicleType>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<VehicleType>>>>());

            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<VehicleTypeGroup>, IEventHandler<ApprovedEvent<VehicleTypeGroup>>>(
                    container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleTypeGroup>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleTypeGroup>, IEventHandler<RejectedEvent<VehicleTypeGroup>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleTypeGroup>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<VehicleTypeGroup>, IEventHandler<DeletedEvent<VehicleTypeGroup>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<VehicleTypeGroup>>>>());

            eventLocalBus
               .RegisterAsync
               <ApprovedEvent<Region>, IEventHandler<ApprovedEvent<Region>>>(
                  container.Resolve<Func<IEventHandler<ApprovedEvent<Region>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<Region>, IEventHandler<RejectedEvent<Region>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<Region>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<Region>, IEventHandler<DeletedEvent<Region>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<Region>>>>());

            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<BedLength>, IEventHandler<ApprovedEvent<BedLength>>>(
                 container.Resolve<Func<IEventHandler<ApprovedEvent<BedLength>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<BedLength>, IEventHandler<RejectedEvent<BedLength>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<BedLength>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<BedLength>, IEventHandler<DeletedEvent<BedLength>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<BedLength>>>>());

            eventLocalBus
             .RegisterAsync
             <ApprovedEvent<BedType>, IEventHandler<ApprovedEvent<BedType>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BedType>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<BedType>, IEventHandler<RejectedEvent<BedType>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<BedType>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<BedType>, IEventHandler<DeletedEvent<BedType>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<BedType>>>>());

            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<Year>, IEventHandler<ApprovedEvent<Year>>>(
                 container.Resolve<Func<IEventHandler<ApprovedEvent<Year>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<Year>, IEventHandler<RejectedEvent<Year>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<Year>>>>());
            eventLocalBus
                .RegisterAsync
                <DeletedEvent<Year>, IEventHandler<DeletedEvent<Year>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<Year>>>>());

            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<Make>, IEventHandler<ApprovedEvent<Make>>>(
                 container.Resolve<Func<IEventHandler<ApprovedEvent<Make>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<Make>, IEventHandler<RejectedEvent<Make>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<Make>>>>());
            eventLocalBus
                .RegisterAsync
                <DeletedEvent<Make>, IEventHandler<DeletedEvent<Make>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<Make>>>>());

            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<Model>, IEventHandler<ApprovedEvent<Model>>>(
                 container.Resolve<Func<IEventHandler<ApprovedEvent<Model>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<Model>, IEventHandler<RejectedEvent<Model>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<Model>>>>());
            eventLocalBus
                .RegisterAsync
                <DeletedEvent<Model>, IEventHandler<DeletedEvent<Model>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<Model>>>>());

            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<SubModel>, IEventHandler<ApprovedEvent<SubModel>>>(
                 container.Resolve<Func<IEventHandler<ApprovedEvent<SubModel>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<SubModel>, IEventHandler<RejectedEvent<SubModel>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<SubModel>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<SubModel>, IEventHandler<DeletedEvent<SubModel>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<SubModel>>>>());
            eventLocalBus
              .RegisterAsync
              <ApprovedEvent<BedConfig>, IEventHandler<ApprovedEvent<BedConfig>>>(
                 container.Resolve<Func<IEventHandler<ApprovedEvent<BedConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<BedConfig>, IEventHandler<RejectedEvent<BedConfig>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<BedConfig>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BedConfig>, IEventHandler<DeletedEvent<BedConfig>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BedConfig>>>>());

            eventLocalBus
             .RegisterAsync
             <ApprovedEvent<BodyStyleConfig>, IEventHandler<ApprovedEvent<BodyStyleConfig>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<BodyStyleConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<BodyStyleConfig>, IEventHandler<RejectedEvent<BodyStyleConfig>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<BodyStyleConfig>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<BodyStyleConfig>, IEventHandler<DeletedEvent<BodyStyleConfig>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<BodyStyleConfig>>>>());

            eventLocalBus
             .RegisterAsync
             <ApprovedEvent<VehicleToBedConfig>, IEventHandler<ApprovedEvent<VehicleToBedConfig>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleToBedConfig>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleToBedConfig>, IEventHandler<RejectedEvent<VehicleToBedConfig>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleToBedConfig>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<VehicleToBedConfig>, IEventHandler<DeletedEvent<VehicleToBedConfig>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<VehicleToBedConfig>>>>());

            eventLocalBus
             .RegisterAsync
             <ApprovedEvent<MfrBodyCode>, IEventHandler<ApprovedEvent<MfrBodyCode>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<MfrBodyCode>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<MfrBodyCode>, IEventHandler<RejectedEvent<MfrBodyCode>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<MfrBodyCode>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<MfrBodyCode>, IEventHandler<DeletedEvent<MfrBodyCode>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<MfrBodyCode>>>>());

            eventLocalBus
            .RegisterAsync
            <ApprovedEvent<WheelBase>, IEventHandler<ApprovedEvent<WheelBase>>>(
               container.Resolve<Func<IEventHandler<ApprovedEvent<WheelBase>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<WheelBase>, IEventHandler<RejectedEvent<WheelBase>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<WheelBase>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<WheelBase>, IEventHandler<DeletedEvent<WheelBase>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<WheelBase>>>>());


            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleToWheelBase>, IEventHandler<RejectedEvent<VehicleToWheelBase>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleToWheelBase>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<VehicleToWheelBase>, IEventHandler<DeletedEvent<VehicleToWheelBase>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<VehicleToWheelBase>>>>());

            eventLocalBus
             .RegisterAsync
             <ApprovedEvent<VehicleToWheelBase>, IEventHandler<ApprovedEvent<VehicleToWheelBase>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleToWheelBase>>>>());

            eventLocalBus
           .RegisterAsync
           <ApprovedEvent<DriveType>, IEventHandler<ApprovedEvent<DriveType>>>(
              container.Resolve<Func<IEventHandler<ApprovedEvent<DriveType>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<DriveType>, IEventHandler<RejectedEvent<DriveType>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<DriveType>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<DriveType>, IEventHandler<DeletedEvent<DriveType>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<DriveType>>>>());


            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleToDriveType>, IEventHandler<RejectedEvent<VehicleToDriveType>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleToDriveType>>>>());
            eventLocalBus
               .RegisterAsync
               <DeletedEvent<VehicleToDriveType>, IEventHandler<DeletedEvent<VehicleToDriveType>>>(
                   container.Resolve<Func<IEventHandler<DeletedEvent<VehicleToDriveType>>>>());

            eventLocalBus
             .RegisterAsync
             <ApprovedEvent<VehicleToDriveType>, IEventHandler<ApprovedEvent<VehicleToDriveType>>>(
                container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleToDriveType>>>>());
            eventLocalBus
                .RegisterAsync
                <ApprovedEvent<VehicleToMfrBodyCode>, IEventHandler<ApprovedEvent<VehicleToMfrBodyCode>>>(
                    container.Resolve<Func<IEventHandler<ApprovedEvent<VehicleToMfrBodyCode>>>>());

            eventLocalBus
                .RegisterAsync
                <RejectedEvent<VehicleToMfrBodyCode>, IEventHandler<RejectedEvent<VehicleToMfrBodyCode>>>(
                    container.Resolve<Func<IEventHandler<RejectedEvent<VehicleToMfrBodyCode>>>>());

            eventLocalBus
                .RegisterAsync
                <DeletedEvent<VehicleToMfrBodyCode>, IEventHandler<DeletedEvent<VehicleToMfrBodyCode>>>(
                    container.Resolve<Func<IEventHandler<DeletedEvent<VehicleToMfrBodyCode>>>>());



        }
    }
}
