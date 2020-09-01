"use strict";
var referenceData_component_1 = require('./referenceData.component');
var make_routes_1 = require('../make/make.routes');
var model_routes_1 = require('../model/model.routes');
var subModel_routes_1 = require('../subModel/subModel.routes');
var year_routes_1 = require('../year/year.routes');
var region_routes_1 = require('../region/region.routes');
var vehicleType_routes_1 = require('../vehicleType/vehicleType.routes');
var vehicleTypeGroup_routes_1 = require('../vehicleTypeGroup/vehicleTypeGroup.routes');
var brakeABS_routes_1 = require('../brakeABS/brakeABS.routes');
var bedLength_routes_1 = require('../bedLength/bedLength.routes');
var bedType_routes_1 = require('../bedType/bedType.routes');
var brakeSystem_routes_1 = require('../brakeSystem/brakeSystem.routes');
var brakeType_routes_1 = require('../brakeType/brakeType.routes');
var bodyType_routes_1 = require('../bodyType/bodyType.routes');
var bodyNumDoors_routes_1 = require('../bodyNumDoors/bodyNumDoors.routes');
var engineDesignation_routes_1 = require('../engineDesignation/engineDesignation.routes');
var engineVersion_routes_1 = require('../engineVersion/engineVersion.routes');
var fuelDeliverySubType_routes_1 = require('../fuelDeliverySubType/fuelDeliverySubType.routes');
var engineVin_routes_1 = require('../engineVin/engineVin.routes');
var fuelType_routes_1 = require('../fuelType/fuelType.routes');
var authorize_service_1 = require("../authorize.service");
exports.ReferenceDataRoutes = [
    {
        path: 'referencedata', component: referenceData_component_1.ReferenceDataComponent,
        children: make_routes_1.MakeRoutes.concat(model_routes_1.ModelRoutes, year_routes_1.YearRoutes, subModel_routes_1.SubModelRoutes, region_routes_1.RegionRoutes, vehicleType_routes_1.VehicleTypeRoutes, vehicleTypeGroup_routes_1.VehicleTypeGroupRoutes, brakeABS_routes_1.BrakeAbsRoutes, brakeSystem_routes_1.BrakeSystemRoutes, bedLength_routes_1.BedLengthRoutes, bedType_routes_1.BedTypeRoutes, brakeType_routes_1.BrakeTypeRoutes, bodyType_routes_1.BodyTypeRoutes, bodyNumDoors_routes_1.BodyNumDoorRoutes, engineDesignation_routes_1.EngineDesignationRoutes, engineVersion_routes_1.EngineVersionRoutes, fuelDeliverySubType_routes_1.FuelDeliverySubTypeRoutes, engineVin_routes_1.EngineVinRoutes, fuelType_routes_1.FuelTypeRoutes, [
            { path: '', redirectTo: 'make', pathMatch: 'full' },
        ]),
        data: { activeTab: 'ReferenceData' },
        canActivate: [authorize_service_1.AuthorizeService]
    }
];
