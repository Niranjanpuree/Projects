import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { AppComponent }  from './app.component';
import { MainHeaderComponent } from './shared/mainHeader.component';
import { SubHeaderComponent } from './shared/subHeader.component';
import { FooterComponent } from './shared/footer.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SearchComponent } from './search/search.component';
import { MyChangeRequestComponent } from './myChangeRequests/myChangeRequest.component';
import { RecentChangesComponent } from './recentChanges/recentChanges.component';
import { DownloadRequestComponent } from './downloadRequests/downloadRequest.component';
import { ModalComponent }       from "ng2-bs3-modal/ng2-bs3-modal";
import { AutoCompleteDirective }                from "./../lib/aclibs/ac-autocomplete/ac-autocomplete";
import { AutoCompleteComponent } from "./../lib/aclibs/ac-autocomplete/ac-autocomplete.component";
import { AcFileUploader }                       from './../lib/aclibs/ac-fileuploader/ac-fileuploader';
import { AcGridComponent, AcGridPaginatorComponent }                      from "./../lib/aclibs/ac-grid/ac-grid";
import { LoadingGifComponent }                  from './shared/loadingGif.component';
import { ToastModule, ToastOptions } from "./../lib/aclibs/ng2-toastr/ng2-toastr";
import {ToastsManager} from "../lib/aclibs/ng2-toastr/ng2-toastr";
import { AuthorizeService } from './authorize.service';
import { AuthenticationService } from './authentication.service';
import { cleanupGuardService } from "./cleanupGuard.service";
import { SharedService } from "./shared/shared.service";
import { NavigationService } from "./shared/navigation.service";
import { HttpHelper } from './httpHelper';
import { APP_ROUTE_PROVIDERS } from './app.routes';
import { HttpModule }    from '@angular/http';
import { RuntimeCompiler } from '@angular/compiler';
import { COMPILER_PROVIDERS } from "@angular/compiler";

import { ReferenceDataComponent } from './referenceData/referenceData.component';
import { PCADBReferenceDataComponent } from './PCADB/referenceData/referenceData.component';
import { QDBReferenceDataComponent } from './QDB/referenceData/referenceData.component';

import { MakesComponent, MakeListComponent, MakeReviewComponent } from './make/makes';
import { ModelsComponent, ModelListComponent, ModelReviewComponent} from './model/models';
import { YearsComponent, YearListComponent, YearReviewComponent} from './year/years';
import { SubModelsComponent, SubModelListComponent, SubModelReviewComponent} from './subModel/subModels';
import { RegionsComponent, RegionListComponent, RegionReviewComponent} from './region/regions';
import { VehicleTypeGroupsComponent, VehicleTypeGroupListComponent, VehicleTypeGroupReviewComponent} from './vehicleTypeGroup/vehicleTypeGroups';
import { VehicleTypesComponent, VehicleTypeListComponent, VehicleTypeReviewComponent} from './vehicleType/vehicleTypes';
import { BaseVehiclesComponent, BaseVehicleAddComponent, BaseVehicleDeleteComponent, BaseVehicleModifyComponent, BaseVehicleReplaceComponent, BaseVehicleReplaceConfirmComponent, BaseVehicleReviewComponent } from './baseVehicle/baseVehicles';
import { VehiclesComponent, VehicleAddComponent, VehicleDeleteComponent, VehicleModifyComponent, VehicleReviewComponent, VehicleSearchComponent } from './vehicle/vehicles';

import { BrakeTypesComponent, BrakeTypeListComponent, BrakeTypeReviewComponent } from './brakeType/brakeTypes';
import { BrakeABSesComponent, BrakeABSListComponent, BrakeABSReviewComponent } from './brakeABS/brakeABSes';
import { BrakeSystemsComponent, BrakeSystemListComponent, BrakeSystemReviewComponent } from './brakeSystem/brakeSystems';

import { BedTypesComponent, BedTypeListComponent, BedTypeReviewComponent } from './bedType/bedTypes';
import { EngineDesignationsComponent, EngineDesignationListComponent, EngineDesignationReviewComponent } from './engineDesignation/engineDesignations';
import { EngineVinsComponent, EngineVinListComponent, EngineVinReviewComponent } from './engineVin/engineVins';
import { EngineVersionsComponent, EngineVersionListComponent, EngineVersionReviewComponent } from './engineVersion/engineVersions';
import { FuelTypesComponent, FuelTypeListComponent, FuelTypeReviewComponent } from './fuelType/fuelTypes';

import { BedLengthComponent, BedLengthListComponent, BedLengthReviewComponent } from './bedLength/bedLengths';

import { BodyTypesComponent, BodyTypeListComponent, BodyTypeReviewComponent } from './bodyType/bodyTypes';
import { BodyNumDoorsComponent, BodyNumDoorsListComponent, BodyNumDoorsReviewComponent } from './bodyNumDoors/bodyNumDoors';

import { DriveTypesComponent, DriveTypeReplaceComponent, DriveTypeReplaceConfirmComponent, DriveTypeReviewComponent } from './driveType/driveTypes';
import { MfrBodyCodesComponent, MfrBodyCodeReplaceComponent, MfrBodyCodeReplaceConfirmComponent, MfrBodyCodeReviewComponent } from './mfrBodyCode/mfrBodyCodes';
import { WheelBasesComponent, WheelBaseReplaceComponent, WheelBaseReplaceConfirmComponent, WheelBaseReviewComponent } from './wheelBase/wheelBases';

import { BrakeConfigsComponent, BrakeConfigAddComponent, BrakeConfigDeleteComponent, BrakeConfigModifyComponent, BrakeConfigReplaceComponent, BrakeConfigReplaceConfirmComponent, BrakeConfigReviewComponent } from './brakeConfig/brakeConfigs';
import { BedConfigsComponent, BedConfigAddComponent, BedConfigDeleteComponent, BedConfigModifyComponent, BedConfigReplaceComponent, BedConfigReplaceConfirmComponent, BedConfigReviewComponent } from './bedConfig/bedConfigs';
import { BodyStyleConfigsComponent, BodyStyleConfigAddComponent, BodyStyleConfigDeleteComponent, BodyStyleConfigModifyComponent, BodyStyleConfigReplaceComponent, BodyStyleConfigReplaceConfirmComponent, BodyStyleConfigReviewComponent } from './bodyStyleConfig/bodyStyleConfig';

import { VehicleToBrakeConfigsComponent, VehicleToBrakeConfigAddComponent, VehicleToBrakeConfigReviewComponent, VehicleToBrakeConfigSearchComponent }                  from "./vehicleToBrakeConfig/vehicleToBrakeConfigs";
import { VehicleToBedConfigsComponent, VehicleToBedConfigAddComponent, VehicleToBedConfigReviewComponent, VehicleToBedConfigSearchComponent}                  from "./vehicleToBedConfig/vehicleToBedConfigs";
import { VehicleToBodyStyleConfigsComponent, VehicleToBodyStyleConfigAddComponent, VehicleToBodyStyleConfigReviewComponent, VehicleToBodyStyleConfigSearchComponent }                  from "./vehicleToBodyStyleConfig/vehicleToBodyStyleConfigs";
import { VehicleToDriveTypesComponent, VehicleToDriveTypeAddComponent, VehicleToDriveTypeReviewComponent, VehicleToDriveTypeSearchComponent}                  from "./vehicleToDriveType/vehicleToDriveTypes";
import { VehicleToMfrBodyCodesComponent, VehicleToMfrBodyCodeAddComponent, VehicleToMfrBodyCodeReviewComponent , VehicleToMfrBodyCodeSearchComponent}                  from "./vehicleToMfrBodyCode/vehicleToMfrBodyCodes";
import { VehicleToWheelBasesComponent, VehicleToWheelBaseAddComponent, VehicleToWheelBaseReviewComponent, VehicleToWheelBaseSearchComponent}                  from "./vehicleToWheelBase/vehicleToWheelBases";

import { ReviewerCommentsComponent }                                  from "./changeRequestReview/reviewerComments.component";
import { UserLikesComponent }                                  from "./changeRequestReview/userLikes.component";
import { SystemMenuBar as SystemMenu }                      from "./system/system-menubar.component";
import { SystemMenuBar }                                    from "./system/system-menubar.component"; //pushkar keep just 1 of 2
import { VehicleToBrakeConfigSearchPanel }                  from "./vehicleToBrakeConfig/vehicleToBrakeConfig-searchPanel.component";
import { VehicleToBedConfigSearchPanel }                 from "./vehicleToBedConfig/vehicleToBedConfig-searchPanel.component";
import { VehicleToBodyStyleConfigSearchPanel }           from "./vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-searchPanel.component";
import { VehicleToDriveTypeSearchPanel }                  from "./vehicleToDriveType/vehicleToDriveType-searchPanel.component";
import { VehicleToMfrBodyCodeSearchPanel }                  from "./vehicleToMfrBodyCode/vehicleToMfrBodyCode-searchPanel.component";
import { VehicleToWheelBaseSearchPanel }                  from "./vehicleToWheelBase/vehicleToWheelBase-searchPanel.component";

import { FuelDeliverySubTypesComponent, FuelDeliverySubTypeListComponent, FuelDeliverySubTypeReviewComponent } from './fuelDeliverySubType/fuelDeliverySubTypes';

import { ChangesComponent, ChangeSearchComponent } from './change/changes';
import { SystemsComponent, SystemSearchComponent } from './system/systems';
import { NKDatetimeModule }                        from "./../lib/aclibs/ng2-datetime/ng2-datetime";
import { ErrorHandler } from '@angular/core';
import { CustomExceptionHandler }  from './customExceptionHandler';

let options = <ToastOptions>{
    autoDismiss: true,
    toastLife: 30 * 1000, //30 seconds
    positionClass: 'toast-top-full-width'
};

@NgModule({
    imports: [BrowserModule, FormsModule, CommonModule, HttpModule, APP_ROUTE_PROVIDERS, ToastModule.forRoot(options), NKDatetimeModule],
    declarations: [AppComponent, MainHeaderComponent, SubHeaderComponent, FooterComponent
        , DashboardComponent, SearchComponent, MyChangeRequestComponent, RecentChangesComponent, DownloadRequestComponent
        , AutoCompleteDirective, AutoCompleteComponent, AcFileUploader
        , AcGridComponent, AcGridPaginatorComponent
        , LoadingGifComponent
        , ModalComponent
        , ReviewerCommentsComponent
        , UserLikesComponent
        , SystemMenu, SystemMenuBar
        , ChangesComponent, ChangeSearchComponent
        , ReferenceDataComponent, PCADBReferenceDataComponent, QDBReferenceDataComponent
        , MakesComponent, MakeListComponent, MakeReviewComponent
        , ModelsComponent, ModelListComponent
        , ModelReviewComponent
        , YearsComponent, YearListComponent, YearReviewComponent
        , SubModelsComponent, SubModelListComponent
        , SubModelReviewComponent
        , RegionsComponent, RegionListComponent, RegionReviewComponent
        , VehicleTypeGroupsComponent, VehicleTypeGroupListComponent, VehicleTypeGroupReviewComponent
        , VehicleTypesComponent, VehicleTypeListComponent, VehicleTypeReviewComponent
        , BaseVehiclesComponent, BaseVehicleAddComponent, BaseVehicleDeleteComponent, BaseVehicleModifyComponent, BaseVehicleReplaceComponent, BaseVehicleReplaceConfirmComponent, BaseVehicleReviewComponent
        , VehiclesComponent, VehicleAddComponent, VehicleDeleteComponent, VehicleModifyComponent, VehicleReviewComponent, VehicleSearchComponent
        , BrakeTypesComponent, BrakeTypeListComponent, BrakeTypeReviewComponent
        , BrakeABSesComponent, BrakeABSListComponent, BrakeABSReviewComponent
        , BrakeSystemsComponent, BrakeSystemListComponent, BrakeSystemReviewComponent
        , BedTypesComponent, BedTypeListComponent, BedTypeReviewComponent
        , BedLengthComponent, BedLengthListComponent, BedLengthReviewComponent
        , BodyTypesComponent, BodyTypeListComponent, BodyTypeReviewComponent
        , BodyNumDoorsComponent, BodyNumDoorsListComponent, BodyNumDoorsReviewComponent
        , BrakeConfigsComponent, BrakeConfigAddComponent, BrakeConfigDeleteComponent, BrakeConfigModifyComponent, BrakeConfigReplaceComponent, BrakeConfigReplaceConfirmComponent, BrakeConfigReviewComponent
        , BedConfigsComponent, BedConfigAddComponent, BedConfigDeleteComponent, BedConfigModifyComponent, BedConfigReplaceComponent, BedConfigReplaceConfirmComponent, BedConfigReviewComponent
        , BodyStyleConfigsComponent, BodyStyleConfigAddComponent, BodyStyleConfigDeleteComponent, BodyStyleConfigModifyComponent, BodyStyleConfigReplaceComponent, BodyStyleConfigReplaceConfirmComponent, BodyStyleConfigReviewComponent

        , DriveTypesComponent, DriveTypeReplaceComponent, DriveTypeReplaceConfirmComponent, DriveTypeReviewComponent
        , MfrBodyCodesComponent, MfrBodyCodeReplaceComponent, MfrBodyCodeReplaceConfirmComponent
        , MfrBodyCodeReviewComponent
        , WheelBasesComponent, WheelBaseReplaceComponent, WheelBaseReplaceConfirmComponent, WheelBaseReviewComponent
        , EngineDesignationsComponent, EngineDesignationListComponent, EngineDesignationReviewComponent
        , EngineVinsComponent, EngineVinListComponent, EngineVinReviewComponent
        , EngineVersionsComponent, EngineVersionListComponent, EngineVersionReviewComponent
        , VehicleToBrakeConfigSearchPanel, VehicleToBedConfigSearchPanel, VehicleToBodyStyleConfigSearchPanel, VehicleToDriveTypeSearchPanel, VehicleToMfrBodyCodeSearchPanel, VehicleToWheelBaseSearchPanel

        , VehicleToBrakeConfigsComponent, VehicleToBrakeConfigAddComponent, VehicleToBrakeConfigReviewComponent, VehicleToBrakeConfigSearchComponent
        , VehicleToBedConfigsComponent, VehicleToBedConfigAddComponent, VehicleToBedConfigReviewComponent, VehicleToBedConfigSearchComponent
        , VehicleToBodyStyleConfigsComponent, VehicleToBodyStyleConfigAddComponent, VehicleToBodyStyleConfigReviewComponent, VehicleToBodyStyleConfigSearchComponent
        , VehicleToDriveTypesComponent, VehicleToDriveTypeSearchComponent, VehicleToDriveTypeAddComponent, VehicleToDriveTypeReviewComponent
        , VehicleToMfrBodyCodesComponent, VehicleToMfrBodyCodeSearchComponent, VehicleToMfrBodyCodeAddComponent, VehicleToMfrBodyCodeReviewComponent 
        , VehicleToWheelBasesComponent, VehicleToWheelBaseAddComponent, VehicleToWheelBaseSearchComponent, VehicleToWheelBaseReviewComponent
        , ChangesComponent, ChangeSearchComponent
        , SystemsComponent, SystemSearchComponent
        , FuelDeliverySubTypesComponent, FuelDeliverySubTypeListComponent, FuelDeliverySubTypeReviewComponent
        , FuelTypesComponent, FuelTypeListComponent, FuelTypeReviewComponent
    ],
    entryComponents: [AutoCompleteComponent],
    bootstrap: [AppComponent],
    providers: [HttpHelper, COMPILER_PROVIDERS, SharedService, NavigationService
        , AuthorizeService, AuthenticationService, ToastsManager, cleanupGuardService
        , { provide: ErrorHandler, useClass: CustomExceptionHandler }]
})

export class AppModule { }
