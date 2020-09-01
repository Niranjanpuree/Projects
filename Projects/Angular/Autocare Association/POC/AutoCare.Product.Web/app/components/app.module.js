"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var platform_browser_1 = require('@angular/platform-browser');
var forms_1 = require('@angular/forms');
var common_1 = require("@angular/common");
var app_component_1 = require('./app.component');
var mainHeader_component_1 = require('./shared/mainHeader.component');
var subHeader_component_1 = require('./shared/subHeader.component');
var footer_component_1 = require('./shared/footer.component');
var dashboard_component_1 = require('./dashboard/dashboard.component');
var search_component_1 = require('./search/search.component');
var myChangeRequest_component_1 = require('./myChangeRequests/myChangeRequest.component');
var recentChanges_component_1 = require('./recentChanges/recentChanges.component');
var downloadRequest_component_1 = require('./downloadRequests/downloadRequest.component');
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var ac_autocomplete_1 = require("./../lib/aclibs/ac-autocomplete/ac-autocomplete");
var ac_autocomplete_component_1 = require("./../lib/aclibs/ac-autocomplete/ac-autocomplete.component");
var ac_fileuploader_1 = require('./../lib/aclibs/ac-fileuploader/ac-fileuploader');
var ac_grid_1 = require("./../lib/aclibs/ac-grid/ac-grid");
var loadingGif_component_1 = require('./shared/loadingGif.component');
var ng2_toastr_1 = require("./../lib/aclibs/ng2-toastr/ng2-toastr");
var ng2_toastr_2 = require("../lib/aclibs/ng2-toastr/ng2-toastr");
var authorize_service_1 = require('./authorize.service');
var authentication_service_1 = require('./authentication.service');
var cleanupGuard_service_1 = require("./cleanupGuard.service");
var shared_service_1 = require("./shared/shared.service");
var navigation_service_1 = require("./shared/navigation.service");
var httpHelper_1 = require('./httpHelper');
var app_routes_1 = require('./app.routes');
var http_1 = require('@angular/http');
var compiler_1 = require("@angular/compiler");
var referenceData_component_1 = require('./referenceData/referenceData.component');
var referenceData_component_2 = require('./PCADB/referenceData/referenceData.component');
var referenceData_component_3 = require('./QDB/referenceData/referenceData.component');
var makes_1 = require('./make/makes');
var models_1 = require('./model/models');
var years_1 = require('./year/years');
var subModels_1 = require('./subModel/subModels');
var regions_1 = require('./region/regions');
var vehicleTypeGroups_1 = require('./vehicleTypeGroup/vehicleTypeGroups');
var vehicleTypes_1 = require('./vehicleType/vehicleTypes');
var baseVehicles_1 = require('./baseVehicle/baseVehicles');
var vehicles_1 = require('./vehicle/vehicles');
var brakeTypes_1 = require('./brakeType/brakeTypes');
var brakeABSes_1 = require('./brakeABS/brakeABSes');
var brakeSystems_1 = require('./brakeSystem/brakeSystems');
var bedTypes_1 = require('./bedType/bedTypes');
var engineDesignations_1 = require('./engineDesignation/engineDesignations');
var engineVins_1 = require('./engineVin/engineVins');
var engineVersions_1 = require('./engineVersion/engineVersions');
var fuelTypes_1 = require('./fuelType/fuelTypes');
var bedLengths_1 = require('./bedLength/bedLengths');
var bodyTypes_1 = require('./bodyType/bodyTypes');
var bodyNumDoors_1 = require('./bodyNumDoors/bodyNumDoors');
var driveTypes_1 = require('./driveType/driveTypes');
var mfrBodyCodes_1 = require('./mfrBodyCode/mfrBodyCodes');
var wheelBases_1 = require('./wheelBase/wheelBases');
var brakeConfigs_1 = require('./brakeConfig/brakeConfigs');
var bedConfigs_1 = require('./bedConfig/bedConfigs');
var bodyStyleConfig_1 = require('./bodyStyleConfig/bodyStyleConfig');
var vehicleToBrakeConfigs_1 = require("./vehicleToBrakeConfig/vehicleToBrakeConfigs");
var vehicleToBedConfigs_1 = require("./vehicleToBedConfig/vehicleToBedConfigs");
var vehicleToBodyStyleConfigs_1 = require("./vehicleToBodyStyleConfig/vehicleToBodyStyleConfigs");
var vehicleToDriveTypes_1 = require("./vehicleToDriveType/vehicleToDriveTypes");
var vehicleToMfrBodyCodes_1 = require("./vehicleToMfrBodyCode/vehicleToMfrBodyCodes");
var vehicleToWheelBases_1 = require("./vehicleToWheelBase/vehicleToWheelBases");
var reviewerComments_component_1 = require("./changeRequestReview/reviewerComments.component");
var userLikes_component_1 = require("./changeRequestReview/userLikes.component");
var system_menubar_component_1 = require("./system/system-menubar.component");
var system_menubar_component_2 = require("./system/system-menubar.component"); //pushkar keep just 1 of 2
var vehicleToBrakeConfig_searchPanel_component_1 = require("./vehicleToBrakeConfig/vehicleToBrakeConfig-searchPanel.component");
var vehicleToBedConfig_searchPanel_component_1 = require("./vehicleToBedConfig/vehicleToBedConfig-searchPanel.component");
var vehicleToBodyStyleConfig_searchPanel_component_1 = require("./vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-searchPanel.component");
var vehicleToDriveType_searchPanel_component_1 = require("./vehicleToDriveType/vehicleToDriveType-searchPanel.component");
var vehicleToMfrBodyCode_searchPanel_component_1 = require("./vehicleToMfrBodyCode/vehicleToMfrBodyCode-searchPanel.component");
var vehicleToWheelBase_searchPanel_component_1 = require("./vehicleToWheelBase/vehicleToWheelBase-searchPanel.component");
var fuelDeliverySubTypes_1 = require('./fuelDeliverySubType/fuelDeliverySubTypes');
var changes_1 = require('./change/changes');
var systems_1 = require('./system/systems');
var ng2_datetime_1 = require("./../lib/aclibs/ng2-datetime/ng2-datetime");
var core_2 = require('@angular/core');
var customExceptionHandler_1 = require('./customExceptionHandler');
var options = {
    autoDismiss: true,
    toastLife: 30 * 1000,
    positionClass: 'toast-top-full-width'
};
var AppModule = (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [platform_browser_1.BrowserModule, forms_1.FormsModule, common_1.CommonModule, http_1.HttpModule, app_routes_1.APP_ROUTE_PROVIDERS, ng2_toastr_1.ToastModule.forRoot(options), ng2_datetime_1.NKDatetimeModule],
            declarations: [app_component_1.AppComponent, mainHeader_component_1.MainHeaderComponent, subHeader_component_1.SubHeaderComponent, footer_component_1.FooterComponent,
                dashboard_component_1.DashboardComponent, search_component_1.SearchComponent, myChangeRequest_component_1.MyChangeRequestComponent, recentChanges_component_1.RecentChangesComponent, downloadRequest_component_1.DownloadRequestComponent,
                ac_autocomplete_1.AutoCompleteDirective, ac_autocomplete_component_1.AutoCompleteComponent, ac_fileuploader_1.AcFileUploader,
                ac_grid_1.AcGridComponent, ac_grid_1.AcGridPaginatorComponent,
                loadingGif_component_1.LoadingGifComponent,
                ng2_bs3_modal_1.ModalComponent,
                reviewerComments_component_1.ReviewerCommentsComponent,
                userLikes_component_1.UserLikesComponent,
                system_menubar_component_1.SystemMenuBar, system_menubar_component_2.SystemMenuBar,
                changes_1.ChangesComponent, changes_1.ChangeSearchComponent,
                referenceData_component_1.ReferenceDataComponent, referenceData_component_2.PCADBReferenceDataComponent, referenceData_component_3.QDBReferenceDataComponent,
                makes_1.MakesComponent, makes_1.MakeListComponent, makes_1.MakeReviewComponent,
                models_1.ModelsComponent, models_1.ModelListComponent,
                models_1.ModelReviewComponent,
                years_1.YearsComponent, years_1.YearListComponent, years_1.YearReviewComponent,
                subModels_1.SubModelsComponent, subModels_1.SubModelListComponent,
                subModels_1.SubModelReviewComponent,
                regions_1.RegionsComponent, regions_1.RegionListComponent, regions_1.RegionReviewComponent,
                vehicleTypeGroups_1.VehicleTypeGroupsComponent, vehicleTypeGroups_1.VehicleTypeGroupListComponent, vehicleTypeGroups_1.VehicleTypeGroupReviewComponent,
                vehicleTypes_1.VehicleTypesComponent, vehicleTypes_1.VehicleTypeListComponent, vehicleTypes_1.VehicleTypeReviewComponent,
                baseVehicles_1.BaseVehiclesComponent, baseVehicles_1.BaseVehicleAddComponent, baseVehicles_1.BaseVehicleDeleteComponent, baseVehicles_1.BaseVehicleModifyComponent, baseVehicles_1.BaseVehicleReplaceComponent, baseVehicles_1.BaseVehicleReplaceConfirmComponent, baseVehicles_1.BaseVehicleReviewComponent,
                vehicles_1.VehiclesComponent, vehicles_1.VehicleAddComponent, vehicles_1.VehicleDeleteComponent, vehicles_1.VehicleModifyComponent, vehicles_1.VehicleReviewComponent, vehicles_1.VehicleSearchComponent,
                brakeTypes_1.BrakeTypesComponent, brakeTypes_1.BrakeTypeListComponent, brakeTypes_1.BrakeTypeReviewComponent,
                brakeABSes_1.BrakeABSesComponent, brakeABSes_1.BrakeABSListComponent, brakeABSes_1.BrakeABSReviewComponent,
                brakeSystems_1.BrakeSystemsComponent, brakeSystems_1.BrakeSystemListComponent, brakeSystems_1.BrakeSystemReviewComponent,
                bedTypes_1.BedTypesComponent, bedTypes_1.BedTypeListComponent, bedTypes_1.BedTypeReviewComponent,
                bedLengths_1.BedLengthComponent, bedLengths_1.BedLengthListComponent, bedLengths_1.BedLengthReviewComponent,
                bodyTypes_1.BodyTypesComponent, bodyTypes_1.BodyTypeListComponent, bodyTypes_1.BodyTypeReviewComponent,
                bodyNumDoors_1.BodyNumDoorsComponent, bodyNumDoors_1.BodyNumDoorsListComponent, bodyNumDoors_1.BodyNumDoorsReviewComponent,
                brakeConfigs_1.BrakeConfigsComponent, brakeConfigs_1.BrakeConfigAddComponent, brakeConfigs_1.BrakeConfigDeleteComponent, brakeConfigs_1.BrakeConfigModifyComponent, brakeConfigs_1.BrakeConfigReplaceComponent, brakeConfigs_1.BrakeConfigReplaceConfirmComponent, brakeConfigs_1.BrakeConfigReviewComponent,
                bedConfigs_1.BedConfigsComponent, bedConfigs_1.BedConfigAddComponent, bedConfigs_1.BedConfigDeleteComponent, bedConfigs_1.BedConfigModifyComponent, bedConfigs_1.BedConfigReplaceComponent, bedConfigs_1.BedConfigReplaceConfirmComponent, bedConfigs_1.BedConfigReviewComponent,
                bodyStyleConfig_1.BodyStyleConfigsComponent, bodyStyleConfig_1.BodyStyleConfigAddComponent, bodyStyleConfig_1.BodyStyleConfigDeleteComponent, bodyStyleConfig_1.BodyStyleConfigModifyComponent, bodyStyleConfig_1.BodyStyleConfigReplaceComponent, bodyStyleConfig_1.BodyStyleConfigReplaceConfirmComponent, bodyStyleConfig_1.BodyStyleConfigReviewComponent,
                driveTypes_1.DriveTypesComponent, driveTypes_1.DriveTypeReplaceComponent, driveTypes_1.DriveTypeReplaceConfirmComponent, driveTypes_1.DriveTypeReviewComponent,
                mfrBodyCodes_1.MfrBodyCodesComponent, mfrBodyCodes_1.MfrBodyCodeReplaceComponent, mfrBodyCodes_1.MfrBodyCodeReplaceConfirmComponent,
                mfrBodyCodes_1.MfrBodyCodeReviewComponent,
                wheelBases_1.WheelBasesComponent, wheelBases_1.WheelBaseReplaceComponent, wheelBases_1.WheelBaseReplaceConfirmComponent, wheelBases_1.WheelBaseReviewComponent,
                engineDesignations_1.EngineDesignationsComponent, engineDesignations_1.EngineDesignationListComponent, engineDesignations_1.EngineDesignationReviewComponent,
                engineVins_1.EngineVinsComponent, engineVins_1.EngineVinListComponent, engineVins_1.EngineVinReviewComponent,
                engineVersions_1.EngineVersionsComponent, engineVersions_1.EngineVersionListComponent, engineVersions_1.EngineVersionReviewComponent,
                vehicleToBrakeConfig_searchPanel_component_1.VehicleToBrakeConfigSearchPanel, vehicleToBedConfig_searchPanel_component_1.VehicleToBedConfigSearchPanel, vehicleToBodyStyleConfig_searchPanel_component_1.VehicleToBodyStyleConfigSearchPanel, vehicleToDriveType_searchPanel_component_1.VehicleToDriveTypeSearchPanel, vehicleToMfrBodyCode_searchPanel_component_1.VehicleToMfrBodyCodeSearchPanel, vehicleToWheelBase_searchPanel_component_1.VehicleToWheelBaseSearchPanel,
                vehicleToBrakeConfigs_1.VehicleToBrakeConfigsComponent, vehicleToBrakeConfigs_1.VehicleToBrakeConfigAddComponent, vehicleToBrakeConfigs_1.VehicleToBrakeConfigReviewComponent, vehicleToBrakeConfigs_1.VehicleToBrakeConfigSearchComponent,
                vehicleToBedConfigs_1.VehicleToBedConfigsComponent, vehicleToBedConfigs_1.VehicleToBedConfigAddComponent, vehicleToBedConfigs_1.VehicleToBedConfigReviewComponent, vehicleToBedConfigs_1.VehicleToBedConfigSearchComponent,
                vehicleToBodyStyleConfigs_1.VehicleToBodyStyleConfigsComponent, vehicleToBodyStyleConfigs_1.VehicleToBodyStyleConfigAddComponent, vehicleToBodyStyleConfigs_1.VehicleToBodyStyleConfigReviewComponent, vehicleToBodyStyleConfigs_1.VehicleToBodyStyleConfigSearchComponent,
                vehicleToDriveTypes_1.VehicleToDriveTypesComponent, vehicleToDriveTypes_1.VehicleToDriveTypeSearchComponent, vehicleToDriveTypes_1.VehicleToDriveTypeAddComponent, vehicleToDriveTypes_1.VehicleToDriveTypeReviewComponent,
                vehicleToMfrBodyCodes_1.VehicleToMfrBodyCodesComponent, vehicleToMfrBodyCodes_1.VehicleToMfrBodyCodeSearchComponent, vehicleToMfrBodyCodes_1.VehicleToMfrBodyCodeAddComponent, vehicleToMfrBodyCodes_1.VehicleToMfrBodyCodeReviewComponent,
                vehicleToWheelBases_1.VehicleToWheelBasesComponent, vehicleToWheelBases_1.VehicleToWheelBaseAddComponent, vehicleToWheelBases_1.VehicleToWheelBaseSearchComponent, vehicleToWheelBases_1.VehicleToWheelBaseReviewComponent,
                changes_1.ChangesComponent, changes_1.ChangeSearchComponent,
                systems_1.SystemsComponent, systems_1.SystemSearchComponent,
                fuelDeliverySubTypes_1.FuelDeliverySubTypesComponent, fuelDeliverySubTypes_1.FuelDeliverySubTypeListComponent, fuelDeliverySubTypes_1.FuelDeliverySubTypeReviewComponent,
                fuelTypes_1.FuelTypesComponent, fuelTypes_1.FuelTypeListComponent, fuelTypes_1.FuelTypeReviewComponent
            ],
            entryComponents: [ac_autocomplete_component_1.AutoCompleteComponent],
            bootstrap: [app_component_1.AppComponent],
            providers: [httpHelper_1.HttpHelper, compiler_1.COMPILER_PROVIDERS, shared_service_1.SharedService, navigation_service_1.NavigationService,
                authorize_service_1.AuthorizeService, authentication_service_1.AuthenticationService, ng2_toastr_2.ToastsManager, cleanupGuard_service_1.cleanupGuardService,
                { provide: core_2.ErrorHandler, useClass: customExceptionHandler_1.CustomExceptionHandler }]
        }), 
        __metadata('design:paramtypes', [])
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
