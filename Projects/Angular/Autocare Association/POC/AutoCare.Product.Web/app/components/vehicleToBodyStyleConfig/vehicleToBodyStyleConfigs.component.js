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
var vehicleToBodyStyleConfig_service_1 = require('./vehicleToBodyStyleConfig.service');
var bodyStyleConfig_service_1 = require('../bodyStyleConfig/bodyStyleConfig.service');
var vehicle_service_1 = require('../vehicle/vehicle.service');
var VehicleToBodyStyleConfigsComponent = (function () {
    function VehicleToBodyStyleConfigsComponent() {
    }
    VehicleToBodyStyleConfigsComponent = __decorate([
        core_1.Component({
            selector: 'vehicleToBodyStyleConfigs-component',
            template: "<router-outlet></router-outlet>",
            providers: [vehicleToBodyStyleConfig_service_1.VehicleToBodyStyleConfigService, vehicle_service_1.VehicleService, bodyStyleConfig_service_1.BodyStyleConfigService]
        }), 
        __metadata('design:paramtypes', [])
    ], VehicleToBodyStyleConfigsComponent);
    return VehicleToBodyStyleConfigsComponent;
}());
exports.VehicleToBodyStyleConfigsComponent = VehicleToBodyStyleConfigsComponent;
