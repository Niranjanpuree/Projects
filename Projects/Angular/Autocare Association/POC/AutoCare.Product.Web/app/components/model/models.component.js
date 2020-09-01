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
var model_service_1 = require('./model.service');
var vehicleType_service_1 = require('../vehicleType/vehicleType.service');
var baseVehicle_service_1 = require('../baseVehicle/baseVehicle.service');
var httpHelper_1 = require('../httpHelper');
var ModelsComponent = (function () {
    function ModelsComponent() {
    }
    ModelsComponent = __decorate([
        core_1.Component({
            selector: 'models-component',
            template: "<router-outlet></router-outlet>",
            providers: [model_service_1.ModelService, vehicleType_service_1.VehicleTypeService, baseVehicle_service_1.BaseVehicleService, httpHelper_1.HttpHelper]
        }), 
        __metadata('design:paramtypes', [])
    ], ModelsComponent);
    return ModelsComponent;
}());
exports.ModelsComponent = ModelsComponent;
