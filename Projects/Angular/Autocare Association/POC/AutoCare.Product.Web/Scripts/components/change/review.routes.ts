import { Routes } from '@angular/router';
import { MakeReviewComponent } from "../make/make-review.component";
import { ModelReviewComponent } from "../model/model-review.component";
import { BrakeTypeReviewComponent } from "../brakeType/brakeType-review.component";
import { BedTypeReviewComponent } from "../bedType/bedType-review.component";
import { SubModelReviewComponent } from "../subModel/subModel-review.component";
import { RegionReviewComponent } from "../region/region-review.component";
import { YearReviewComponent } from "../year/year-review.component";
import { BrakeABSReviewComponent } from "../brakeABS/brakeABS-review.component";
import { BrakeSystemReviewComponent } from "../brakeSystem/brakeSystem-review.component";
import { BaseVehicleReviewComponent } from "../baseVehicle/baseVehicle-review.component";
import { VehicleTypeReviewComponent } from "../vehicleType/vehicleType-review.component";
import { VehicleTypeGroupReviewComponent } from "../vehicleTypeGroup/vehicleTypeGroup-review.component";
import { BrakeConfigReviewComponent } from "../brakeConfig/brakeConfig-review.component";
import { VehicleReviewComponent } from "../vehicle/vehicle-review.component";
import { BedLengthReviewComponent } from "../bedlength/bedLength-review.component";
import { VehicleToBrakeConfigReviewComponent } from "../vehicleToBrakeConfig/vehicleToBrakeConfig-review.component";
import { VehicleToWheelBaseReviewComponent } from "../vehicleToWheelBase/vehicleToWheelBase-review.component";
import { AuthorizeService } from '../authorize.service';
import { BedConfigReviewComponent } from "../bedConfig/bedConfig-review.component";
import { VehicleToBedConfigReviewComponent } from "../vehicleToBedConfig/vehicleToBedConfig-review.component";
import { VehicleToBodyStyleConfigReviewComponent } from "../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-review.component";
import { BodyNumDoorsReviewComponent } from "../bodyNumDoors/bodyNumDoors-review.component";
import { BodyTypeReviewComponent } from "../bodyType/bodyType-review.component";
import { WheelBaseReviewComponent } from "../wheelBase/wheelBase-review.component";
import { BodyStyleConfigReviewComponent } from "../bodyStyleConfig/bodyStyleConfig-review.component";
import {  MfrBodyCodeReviewComponent} from "../mfrBodyCode/mfrBodyCode-review.component";
import {  DriveTypeReviewComponent} from "../driveType/driveType-review.component";
import {  VehicleToMfrBodyCodeReviewComponent} from "../vehicleToMfrBodyCode/vehicleToMfrBodyCode-review.component";
import {  VehicleToDriveTypeReviewComponent} from "../vehicleToDriveType/vehicleToDriveType-review.component";
import {  EngineDesignationReviewComponent} from "../engineDesignation/engineDesignation-review.component";
import {  EngineVersionReviewComponent} from "../engineVersion/engineVersion-review.component";
import {  EngineVinReviewComponent} from "../engineVin/engineVin-review.component";
import {  FuelTypeReviewComponent} from "../fuelType/fuelType-review.component";

import {  FuelDeliverySubTypeReviewComponent} from "../fuelDeliverySubType/fuelDeliverySubType-review.component";


export const ReviewRoutes: Routes = [
    {
        path: 'review',
        children: [
            { path: 'make/:id', component: MakeReviewComponent },
            { path: 'submodel/:id', component: SubModelReviewComponent },
            { path: 'model/:id', component: ModelReviewComponent },
            { path: 'braketype/:id', component: BrakeTypeReviewComponent },
            { path: 'brakeabs/:id', component: BrakeABSReviewComponent },
            { path: 'brakesystem/:id', component: BrakeSystemReviewComponent },
            { path: 'region/:id', component: RegionReviewComponent },
            { path: 'year/:id', component: YearReviewComponent },
            { path: 'basevehicle/:id', component: BaseVehicleReviewComponent },
            { path: 'vehicletype/:id', component: VehicleTypeReviewComponent },
            { path: 'vehicletypegroup/:id', component: VehicleTypeGroupReviewComponent },
            { path: 'year/:id', component: YearReviewComponent },
            { path: 'brakeconfig/:id', component: BrakeConfigReviewComponent },
            { path: 'vehicle/:id', component: VehicleReviewComponent },
            { path: 'brakeConfig/:id', component: BrakeConfigReviewComponent },
            //{ path: 'bedlength/:id', component: BedLengthReviewComponent },   //pushkar: need to figure out error in registering to NgModule
            { path: 'vehicletobrakeconfig/:id', component: VehicleToBrakeConfigReviewComponent },
            { path: 'vehicletowheelbase/:id', component: VehicleToWheelBaseReviewComponent },
            { path: 'bedtype/:id', component: BedTypeReviewComponent },
            { path: 'bedconfig/:id', component: BedConfigReviewComponent },
            { path: 'vehicletobedconfig/:id', component: VehicleToBedConfigReviewComponent },
            { path: 'vehicletobodystyleconfig/:id', component: VehicleToBodyStyleConfigReviewComponent },
            { path: 'bodytype/:id', component: BodyTypeReviewComponent },
            { path: 'bodynumdoors/:id', component: BodyNumDoorsReviewComponent },
            { path: 'bodystyleconfig/:id', component: BodyStyleConfigReviewComponent },
            { path: 'wheelbase/:id', component: WheelBaseReviewComponent },
            { path: 'mfrbodycode/:id', component: MfrBodyCodeReviewComponent },
            { path: 'drivetype/:id', component: DriveTypeReviewComponent },
            { path: 'vehicletomfrbodycode/:id', component: VehicleToMfrBodyCodeReviewComponent },
            { path: 'vehicletodrivetype/:id', component: VehicleToDriveTypeReviewComponent },
            { path: 'fueldeliverysubtype/:id', component: FuelDeliverySubTypeReviewComponent },
            { path: 'enginedesignation/:id', component: EngineDesignationReviewComponent },
            { path: 'engineversion/:id', component: EngineVersionReviewComponent },
            { path: 'enginevin/:id', component: EngineVinReviewComponent },
            { path: 'fueltype/:id', component: FuelTypeReviewComponent },
        ],
        data: { activeTab: 'Changes' },
        canActivate: [AuthorizeService]
    }
];