"use strict";
var router_1 = require('@angular/router');
var vehicle_routes_1 = require('./vehicle/vehicle.routes');
var referenceData_routes_1 = require('./referenceData/referenceData.routes');
var referenceData_routes_2 = require('./PCADB/referenceData/referenceData.routes');
var referenceData_routes_3 = require('./QDB/referenceData/referenceData.routes');
var baseVehicle_routes_1 = require('./baseVehicle/baseVehicle.routes');
var brakeConfig_routes_1 = require('./brakeConfig/brakeConfig.routes');
var vehicleToBrakeConfig_routes_1 = require('./vehicleToBrakeConfig/vehicleToBrakeConfig.routes');
var vehicleToBedConfig_routes_1 = require('./vehicleToBedConfig/vehicleToBedConfig.routes');
var vehicleToBodyStyleConfig_routes_1 = require('./vehicleToBodyStyleConfig/vehicleToBodyStyleConfig.routes');
var vehicleToWheelBase_routes_1 = require('./vehicleToWheelBase/vehicleToWheelBase.routes');
var change_routes_1 = require('./change/change.routes');
var dashboard_routes_1 = require('./dashboard/dashboard.routes');
var system_routes_1 = require("./system/system.routes");
var bedConfig_routes_1 = require('./bedConfig/bedConfig.routes');
var bodyStyleConfig_routes_1 = require('./bodyStyleConfig/bodyStyleConfig.routes');
var driveType_routes_1 = require('./driveType/driveType.routes');
var mfrBodyCode_routes_1 = require('./mfrBodyCode/mfrBodyCode.routes');
var wheelBase_routes_1 = require('./wheelBase/wheelBase.routes');
var vehicleToMfrBodyCode_routes_1 = require('./vehicleToMfrBodyCode/vehicleToMfrBodyCode.routes');
var vehicleToDriveType_routes_1 = require('./vehicleToDriveType/vehicleToDriveType.routes');
exports.AppRoutes = dashboard_routes_1.DashboardRoutes.concat(referenceData_routes_1.ReferenceDataRoutes, baseVehicle_routes_1.BaseVehicleRoutes, brakeConfig_routes_1.BrakeConfigRoutes, vehicle_routes_1.VehicleRoutes, vehicleToBrakeConfig_routes_1.VehicleToBrakeConfigRoutes, vehicleToBodyStyleConfig_routes_1.VehicleToBodyStyleConfigRoutes, change_routes_1.ChangeRoutes, system_routes_1.SystemRoutes, referenceData_routes_2.PCADBReferenceDataRoutes, referenceData_routes_3.QDBReferenceDataRoutes, bedConfig_routes_1.BedConfigRoutes, vehicleToBedConfig_routes_1.VehicleToBedConfigRoutes, bodyStyleConfig_routes_1.BodyStyleConfigRoutes, vehicleToWheelBase_routes_1.VehicleToWheelBaseRoutes, driveType_routes_1.DriveTypeRoutes, vehicleToDriveType_routes_1.VehicleToDriveTypeRoutes, vehicleToMfrBodyCode_routes_1.VehicleToMfrBodyCodeRoutes, mfrBodyCode_routes_1.MfrBodyCodeRoutes, wheelBase_routes_1.WheelBaseRoutes, [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
]);
//TODO: pushkar: other remaining routes, exceptionhandler, toastmanager
exports.APP_ROUTE_PROVIDERS = router_1.RouterModule.forRoot(exports.AppRoutes);
