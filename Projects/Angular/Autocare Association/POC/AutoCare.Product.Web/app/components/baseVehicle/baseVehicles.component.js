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
var baseVehicle_service_1 = require('./baseVehicle.service');
var model_service_1 = require('../model/model.service');
var make_service_1 = require('../make/make.service');
var year_service_1 = require('../year/year.service');
var BaseVehiclesComponent = (function () {
    function BaseVehiclesComponent() {
    }
    BaseVehiclesComponent = __decorate([
        core_1.Component({
            selector: 'baseVehicles-component',
            template: "<router-outlet></router-outlet>",
            providers: [baseVehicle_service_1.BaseVehicleService, model_service_1.ModelService, make_service_1.MakeService, year_service_1.YearService]
        }), 
        __metadata('design:paramtypes', [])
    ], BaseVehiclesComponent);
    return BaseVehiclesComponent;
}());
exports.BaseVehiclesComponent = BaseVehiclesComponent;
