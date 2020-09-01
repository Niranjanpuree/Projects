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
var vehicleToBrakeConfig_service_1 = require('./vehicleToBrakeConfig.service');
var brakeConfig_service_1 = require('../brakeConfig/brakeConfig.service');
var brakeType_service_1 = require('../brakeType/brakeType.service');
var brakeSystem_service_1 = require('../brakeSystem/brakeSystem.service');
var brakeABS_service_1 = require('../brakeABS/brakeABS.service');
var vehicle_service_1 = require('../vehicle/vehicle.service');
var VehicleToBrakeConfigsComponent = (function () {
    function VehicleToBrakeConfigsComponent() {
    }
    VehicleToBrakeConfigsComponent = __decorate([
        core_1.Component({
            selector: 'vehicleToBrakeConfigs-component',
            template: "<router-outlet></router-outlet>",
            providers: [vehicleToBrakeConfig_service_1.VehicleToBrakeConfigService, vehicle_service_1.VehicleService, brakeConfig_service_1.BrakeConfigService, brakeType_service_1.BrakeTypeService, brakeSystem_service_1.BrakeSystemService, brakeABS_service_1.BrakeABSService]
        }), 
        __metadata('design:paramtypes', [])
    ], VehicleToBrakeConfigsComponent);
    return VehicleToBrakeConfigsComponent;
}());
exports.VehicleToBrakeConfigsComponent = VehicleToBrakeConfigsComponent;
