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
var vehicleToBedConfig_service_1 = require('./vehicleToBedConfig.service');
var bedConfig_service_1 = require('../bedConfig/bedConfig.service');
var vehicle_service_1 = require('../vehicle/vehicle.service');
var VehicleToBedConfigsComponent = (function () {
    function VehicleToBedConfigsComponent() {
    }
    VehicleToBedConfigsComponent = __decorate([
        core_1.Component({
            selector: 'vehicleToBedConfigs-component',
            template: "<router-outlet></router-outlet>",
            providers: [vehicleToBedConfig_service_1.VehicleToBedConfigService, vehicle_service_1.VehicleService, bedConfig_service_1.BedConfigService]
        }), 
        __metadata('design:paramtypes', [])
    ], VehicleToBedConfigsComponent);
    return VehicleToBedConfigsComponent;
}());
exports.VehicleToBedConfigsComponent = VehicleToBedConfigsComponent;
