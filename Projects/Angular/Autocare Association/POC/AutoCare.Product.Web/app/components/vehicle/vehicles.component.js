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
var vehicle_search_service_1 = require('./vehicle-search.service');
var vehicle_service_1 = require('./vehicle.service');
var subModel_service_1 = require('../subModel/subModel.service');
var region_service_1 = require('../region/region.service');
var VehiclesComponent = (function () {
    function VehiclesComponent() {
    }
    VehiclesComponent = __decorate([
        core_1.Component({
            selector: 'vehicles-component',
            template: '<router-outlet></router-outlet>',
            providers: [vehicle_search_service_1.VehicleSearchService, vehicle_service_1.VehicleService, subModel_service_1.SubModelService, region_service_1.RegionService]
        }), 
        __metadata('design:paramtypes', [])
    ], VehiclesComponent);
    return VehiclesComponent;
}());
exports.VehiclesComponent = VehiclesComponent;
