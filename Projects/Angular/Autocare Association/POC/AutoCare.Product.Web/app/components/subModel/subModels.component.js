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
var subModel_service_1 = require('./subModel.service');
var baseVehicle_service_1 = require('../baseVehicle/baseVehicle.service');
var SubModelsComponent = (function () {
    function SubModelsComponent() {
    }
    SubModelsComponent = __decorate([
        core_1.Component({
            selector: 'sub-models-component',
            template: "<router-outlet></router-outlet>",
            providers: [subModel_service_1.SubModelService, baseVehicle_service_1.BaseVehicleService]
        }), 
        __metadata('design:paramtypes', [])
    ], SubModelsComponent);
    return SubModelsComponent;
}());
exports.SubModelsComponent = SubModelsComponent;
