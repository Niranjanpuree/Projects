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
var router_1 = require("@angular/router");
var vehicleType_service_1 = require('../vehicleType/vehicleType.service');
var model_service_1 = require('./model.service');
var ng2_bs3_modal_1 = require('ng2-bs3-modal/ng2-bs3-modal');
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var constants_warehouse_1 = require('../constants-warehouse');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var navigation_service_1 = require("../shared/navigation.service");
var ModelListComponent = (function () {
    function ModelListComponent(modelService, vehicleTypeService, toastr, router, navigationService) {
        var _this = this;
        this.modelService = modelService;
        this.vehicleTypeService = vehicleTypeService;
        this.toastr = toastr;
        this.router = router;
        this.navigationService = navigationService;
        this.filteredModels = [];
        this.model = {};
        this.modifiedModel = {};
        this.deleteModel = {};
        this.modelNameFilter = '';
        this.showLoadingGif = false;
        this.getSuggestions = function (keyword) {
            return _this.modelService.getFilteredModels(keyword);
        };
        this.applyFilter = function (keyword) {
            _this.showLoadingGif = true;
            keyword = keyword || '';
            if (keyword == '') {
                keyword = _this.modelNameFilter;
            }
            else {
                _this.modelNameFilter = keyword;
            }
            if (String(_this.modelNameFilter) === "") {
                _this.modelService.getModels().subscribe(function (m) {
                    _this.models = m;
                    _this.showLoadingGif = false;
                    _this.filteredModels = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
            else {
                _this.modelService.getFilteredModels(_this.modelNameFilter).subscribe(function (m) {
                    _this.models = m;
                    _this.showLoadingGif = false;
                    _this.filteredModels = [];
                }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
            }
        };
    }
    ModelListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        this.modelService.getModels().subscribe(function (m) {
            _this.models = m;
            // load vehicletype
            _this.vehicleTypeService.getAllVehicleTypes().subscribe(function (m) {
                _this.vehicleTypes = m;
                _this.showLoadingGif = false;
            }, function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                _this.showLoadingGif = false;
            });
        }, function (error) {
            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            _this.showLoadingGif = false;
        });
    };
    ModelListComponent.prototype.onCancel = function (action) {
        this.newPopupAcFileUploader.reset(true);
        this.newPopup.close();
        this.modifyPopupAcFileUploader.reset(true);
        this.modifyPopup.close();
        this.deletePopupAcFileUploader.reset(true);
        this.deleteConfirmPopup.close();
    };
    ModelListComponent.prototype.onSelect = function (model) {
        this.modelNameFilter = model.name;
        this.filteredModels = [];
    };
    ModelListComponent.prototype.validationCheck = function (item) {
        if (item.changeType !== "Delete") {
            if (!item.name) {
                this.toastr.warning("Model Name is required.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
            if (item.vehicleTypeId === -1) {
                this.toastr.warning("Please select vehicle type.", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        if (item.changeType == "Modify") {
            if (item.name === this.model.name && item.vehicleTypeId === this.model.vehicleTypeId) {
                this.toastr.warning("Nothing has changed", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return false;
            }
        }
        return true;
    };
    ModelListComponent.prototype.onNew = function () {
        this.model = {};
        this.model.vehicleTypeId = -1;
        this.newPopup.open("md");
    };
    ModelListComponent.prototype.onNewSubmit = function () {
        var _this = this;
        this.model.changeType = "Add";
        if (!this.validationCheck(this.model)) {
            return;
        }
        this.showLoadingGif = true;
        this.newPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.model.attachments = uploadedFiles;
            }
            if (_this.model.attachments) {
                _this.model.attachments = _this.model.attachments.concat(_this.newPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.modelService.addModel(_this.model).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.model.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.add + " the \"" + _this.model.name + "\" Model change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.model.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.add, _this.model.name);
                _this.toastr.warning(errorresponse, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.newPopupAcFileUploader.reset(true);
                _this.newPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.newPopupAcFileUploader.reset(true);
            _this.newPopup.close();
        });
    };
    ModelListComponent.prototype.onModify = function (model) {
        var _this = this;
        this.showLoadingGif = true;
        this.model = model;
        if (!model.baseVehicleCount && !model.vehicleCount) {
            this.modelService.getModelDetail(model.id).subscribe(function (m) {
                model.baseVehicleCount = m.baseVehicleCount;
                model.vehicleCount = m.vehicleCount;
                _this.modifiedModel = JSON.parse(JSON.stringify(model));
                _this.showLoadingGif = false;
                _this.modifyPopup.open("md");
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            this.modifiedModel = JSON.parse(JSON.stringify(model));
            this.showLoadingGif = false;
            this.modifyPopup.open("md");
        }
    };
    ModelListComponent.prototype.onModifySubmit = function () {
        var _this = this;
        this.modifiedModel.changeType = "Modify";
        if (!this.validationCheck(this.modifiedModel)) {
            return;
        }
        this.showLoadingGif = true;
        this.modifyPopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.modifiedModel.attachments = uploadedFiles;
            }
            if (_this.modifiedModel.attachments) {
                _this.modifiedModel.attachments = _this.modifiedModel.attachments.concat(_this.modifyPopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.modelService.updateModel(_this.modifiedModel.id, _this.modifiedModel).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.model.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify + " the \"" + _this.model.name + "\" Model change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.models.filter(function (x) { return x.id == _this.modifiedModel.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.model.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
                _this.showLoadingGif = false;
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.modify, _this.model.name);
                _this.toastr.warning(errorresponse, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.modifyPopupAcFileUploader.reset(true);
                _this.modifyPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.modifyPopupAcFileUploader.reset(true);
            _this.modifyPopup.close();
        });
    };
    ModelListComponent.prototype.onDelete = function (model) {
        var _this = this;
        this.model = model;
        this.deleteModel.comment = "";
        this.showLoadingGif = true;
        if (!model.baseVehicleCount && !model.vehicleCount) {
            this.modelService.getModelDetail(model.id).subscribe(function (m) {
                model.baseVehicleCount = m.baseVehicleCount;
                model.vehicleCount = m.vehicleCount;
                if (model.baseVehicleCount > 0) {
                    _this.showLoadingGif = false;
                    _this.deleteErrorPopup.open("sm");
                }
                else {
                    _this.showLoadingGif = false;
                    _this.deleteConfirmPopup.open("md");
                }
            }, function (error) { return _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle); });
        }
        else {
            if (model.baseVehicleCount > 0) {
                this.showLoadingGif = false;
                this.deleteErrorPopup.open("sm");
            }
            else {
                this.showLoadingGif = false;
                this.deleteConfirmPopup.open("md");
            }
        }
    };
    ModelListComponent.prototype.onDeleteConfirm = function () {
        var _this = this;
        this.deleteModel.changeType = "Delete";
        if (!this.validationCheck(this.deleteModel)) {
            return;
        }
        this.showLoadingGif = true;
        this.deletePopupAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.deleteModel.attachments = uploadedFiles;
            }
            if (_this.deleteModel.attachments) {
                _this.deleteModel.attachments = _this.deleteModel.attachments.concat(_this.deletePopupAcFileUploader.getFilesMarkedToDelete());
            }
            _this.modelService.deleteModel(_this.model.id, _this.deleteModel).subscribe(function (response) {
                if (response) {
                    var successMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.success("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.model.name);
                    successMessage.title = "Your request to " + constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove + " the \"" + _this.model.name + "\" Model change requestid  \"" + response + "\" will be reviewed.";
                    _this.toastr.success(successMessage.body, successMessage.title);
                    _this.models.filter(function (x) { return x.id == _this.model.id; })[0].changeRequestId = response;
                }
                else {
                    var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.model.name);
                    _this.toastr.warning(errorMessage.body, errorMessage.title);
                }
            }, (function (errorresponse) {
                var errorMessage = constants_warehouse_1.ConstantsWarehouse.notificationMessage.error("Model", constants_warehouse_1.ConstantsWarehouse.changeRequestType.remove, _this.model.name);
                _this.toastr.warning(errorresponse, errorMessage.title);
                _this.showLoadingGif = false;
            }), function () {
                _this.showLoadingGif = false;
                _this.deletePopupAcFileUploader.reset(true);
                _this.deleteConfirmPopup.close();
            });
        }, function (error) {
            _this.showLoadingGif = false;
            _this.deletePopupAcFileUploader.reset(true);
            _this.deleteConfirmPopup.close();
        });
    };
    ModelListComponent.prototype.view = function (ModelVm) {
        this.navigationService.backRoute = "/" + document.URL.replace(document.baseURI.toString(), "");
        var changeRequestLink = "/change/review/model/" + ModelVm.changeRequestId;
        this.router.navigateByUrl(changeRequestLink);
    };
    ModelListComponent.prototype.cleanupComponent = function () {
        return this.newPopupAcFileUploader.cleanupAllTempContainers() && this.modifyPopupAcFileUploader.cleanupAllTempContainers() && this.deletePopupAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild('newPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ModelListComponent.prototype, "newPopup", void 0);
    __decorate([
        core_1.ViewChild('modifyPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ModelListComponent.prototype, "modifyPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteErrorPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ModelListComponent.prototype, "deleteErrorPopup", void 0);
    __decorate([
        core_1.ViewChild('deleteConfirmPopup'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], ModelListComponent.prototype, "deleteConfirmPopup", void 0);
    __decorate([
        core_1.ViewChild("newPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], ModelListComponent.prototype, "newPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("modifyPopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], ModelListComponent.prototype, "modifyPopupAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("deletePopupAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], ModelListComponent.prototype, "deletePopupAcFileUploader", void 0);
    ModelListComponent = __decorate([
        core_1.Component({
            selector: 'model-list-component',
            templateUrl: 'app/templates/model/model-list.component.html',
        }), 
        __metadata('design:paramtypes', [model_service_1.ModelService, vehicleType_service_1.VehicleTypeService, ng2_toastr_1.ToastsManager, router_1.Router, navigation_service_1.NavigationService])
    ], ModelListComponent);
    return ModelListComponent;
}());
exports.ModelListComponent = ModelListComponent;
