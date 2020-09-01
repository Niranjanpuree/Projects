"use strict";
var make_review_component_1 = require("../make/make-review.component");
var model_review_component_1 = require("../model/model-review.component");
var brakeType_review_component_1 = require("../brakeType/brakeType-review.component");
var bedType_review_component_1 = require("../bedType/bedType-review.component");
var subModel_review_component_1 = require("../subModel/subModel-review.component");
var region_review_component_1 = require("../region/region-review.component");
var year_review_component_1 = require("../year/year-review.component");
var brakeABS_review_component_1 = require("../brakeABS/brakeABS-review.component");
var brakeSystem_review_component_1 = require("../brakeSystem/brakeSystem-review.component");
var baseVehicle_review_component_1 = require("../baseVehicle/baseVehicle-review.component");
var vehicleType_review_component_1 = require("../vehicleType/vehicleType-review.component");
var vehicleTypeGroup_review_component_1 = require("../vehicleTypeGroup/vehicleTypeGroup-review.component");
var brakeConfig_review_component_1 = require("../brakeConfig/brakeConfig-review.component");
var vehicle_review_component_1 = require("../vehicle/vehicle-review.component");
var vehicleToBrakeConfig_review_component_1 = require("../vehicleToBrakeConfig/vehicleToBrakeConfig-review.component");
var vehicleToWheelBase_review_component_1 = require("../vehicleToWheelBase/vehicleToWheelBase-review.component");
var authorize_service_1 = require('../authorize.service');
var bedConfig_review_component_1 = require("../bedConfig/bedConfig-review.component");
var vehicleToBedConfig_review_component_1 = require("../vehicleToBedConfig/vehicleToBedConfig-review.component");
var vehicleToBodyStyleConfig_review_component_1 = require("../vehicleToBodyStyleConfig/vehicleToBodyStyleConfig-review.component");
var bodyNumDoors_review_component_1 = require("../bodyNumDoors/bodyNumDoors-review.component");
var bodyType_review_component_1 = require("../bodyType/bodyType-review.component");
var wheelBase_review_component_1 = require("../wheelBase/wheelBase-review.component");
var bodyStyleConfig_review_component_1 = require("../bodyStyleConfig/bodyStyleConfig-review.component");
var mfrBodyCode_review_component_1 = require("../mfrBodyCode/mfrBodyCode-review.component");
var driveType_review_component_1 = require("../driveType/driveType-review.component");
var vehicleToMfrBodyCode_review_component_1 = require("../vehicleToMfrBodyCode/vehicleToMfrBodyCode-review.component");
var vehicleToDriveType_review_component_1 = require("../vehicleToDriveType/vehicleToDriveType-review.component");
var engineDesignation_review_component_1 = require("../engineDesignation/engineDesignation-review.component");
var engineVersion_review_component_1 = require("../engineVersion/engineVersion-review.component");
var engineVin_review_component_1 = require("../engineVin/engineVin-review.component");
var fuelType_review_component_1 = require("../fuelType/fuelType-review.component");
var fuelDeliverySubType_review_component_1 = require("../fuelDeliverySubType/fuelDeliverySubType-review.component");
exports.ReviewRoutes = [
    {
        path: 'review',
        children: [
            { path: 'make/:id', component: make_review_component_1.MakeReviewComponent },
            { path: 'submodel/:id', component: subModel_review_component_1.SubModelReviewComponent },
            { path: 'model/:id', component: model_review_component_1.ModelReviewComponent },
            { path: 'braketype/:id', component: brakeType_review_component_1.BrakeTypeReviewComponent },
            { path: 'brakeabs/:id', component: brakeABS_review_component_1.BrakeABSReviewComponent },
            { path: 'brakesystem/:id', component: brakeSystem_review_component_1.BrakeSystemReviewComponent },
            { path: 'region/:id', component: region_review_component_1.RegionReviewComponent },
            { path: 'year/:id', component: year_review_component_1.YearReviewComponent },
            { path: 'basevehicle/:id', component: baseVehicle_review_component_1.BaseVehicleReviewComponent },
            { path: 'vehicletype/:id', component: vehicleType_review_component_1.VehicleTypeReviewComponent },
            { path: 'vehicletypegroup/:id', component: vehicleTypeGroup_review_component_1.VehicleTypeGroupReviewComponent },
            { path: 'year/:id', component: year_review_component_1.YearReviewComponent },
            { path: 'brakeconfig/:id', component: brakeConfig_review_component_1.BrakeConfigReviewComponent },
            { path: 'vehicle/:id', component: vehicle_review_component_1.VehicleReviewComponent },
            { path: 'brakeConfig/:id', component: brakeConfig_review_component_1.BrakeConfigReviewComponent },
            //{ path: 'bedlength/:id', component: BedLengthReviewComponent },   //pushkar: need to figure out error in registering to NgModule
            { path: 'vehicletobrakeconfig/:id', component: vehicleToBrakeConfig_review_component_1.VehicleToBrakeConfigReviewComponent },
            { path: 'vehicletowheelbase/:id', component: vehicleToWheelBase_review_component_1.VehicleToWheelBaseReviewComponent },
            { path: 'bedtype/:id', component: bedType_review_component_1.BedTypeReviewComponent },
            { path: 'bedconfig/:id', component: bedConfig_review_component_1.BedConfigReviewComponent },
            { path: 'vehicletobedconfig/:id', component: vehicleToBedConfig_review_component_1.VehicleToBedConfigReviewComponent },
            { path: 'vehicletobodystyleconfig/:id', component: vehicleToBodyStyleConfig_review_component_1.VehicleToBodyStyleConfigReviewComponent },
            { path: 'bodytype/:id', component: bodyType_review_component_1.BodyTypeReviewComponent },
            { path: 'bodynumdoors/:id', component: bodyNumDoors_review_component_1.BodyNumDoorsReviewComponent },
            { path: 'bodystyleconfig/:id', component: bodyStyleConfig_review_component_1.BodyStyleConfigReviewComponent },
            { path: 'wheelbase/:id', component: wheelBase_review_component_1.WheelBaseReviewComponent },
            { path: 'mfrbodycode/:id', component: mfrBodyCode_review_component_1.MfrBodyCodeReviewComponent },
            { path: 'drivetype/:id', component: driveType_review_component_1.DriveTypeReviewComponent },
            { path: 'vehicletomfrbodycode/:id', component: vehicleToMfrBodyCode_review_component_1.VehicleToMfrBodyCodeReviewComponent },
            { path: 'vehicletodrivetype/:id', component: vehicleToDriveType_review_component_1.VehicleToDriveTypeReviewComponent },
            { path: 'fueldeliverysubtype/:id', component: fuelDeliverySubType_review_component_1.FuelDeliverySubTypeReviewComponent },
            { path: 'enginedesignation/:id', component: engineDesignation_review_component_1.EngineDesignationReviewComponent },
            { path: 'engineversion/:id', component: engineVersion_review_component_1.EngineVersionReviewComponent },
            { path: 'enginevin/:id', component: engineVin_review_component_1.EngineVinReviewComponent },
            { path: 'fueltype/:id', component: fuelType_review_component_1.FuelTypeReviewComponent },
        ],
        data: { activeTab: 'Changes' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
