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
var core_1 = require("@angular/core");
var ng2_toastr_1 = require("../../lib/aclibs/ng2-toastr/ng2-toastr");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var baseVehicle_service_1 = require("./baseVehicle.service");
var constants_warehouse_1 = require("../constants-warehouse");
var router_1 = require("@angular/router");
var changeRequestReview_model_1 = require("../changeRequestReview/changeRequestReview.model");
var vehicle_service_1 = require("../vehicle/vehicle.service");
var make_service_1 = require('../make/make.service');
var model_service_1 = require('../model/model.service');
var likeStaging_service_1 = require("../changeRequestReview/likeStaging.service");
var shared_service_1 = require('../shared/shared.service');
var navigation_service_1 = require('../shared/navigation.service');
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var user_service_1 = require("../change/user.service");
var change_service_1 = require("../change/change.service");
var userLikes_component_1 = require("../changeRequestReview/userLikes.component");
var BaseVehicleReviewComponent = (function () {
    function BaseVehicleReviewComponent(baseVehicleService, vehicleService, makeService, modelService, toastr, route, router, likeStagingService, sharedService, userService, changeService, navgationService) {
        this.baseVehicleService = baseVehicleService;
        this.vehicleService = vehicleService;
        this.makeService = makeService;
        this.modelService = modelService;
        this.toastr = toastr;
        this.route = route;
        this.router = router;
        this.likeStagingService = likeStagingService;
        this.sharedService = sharedService;
        this.userService = userService;
        this.changeService = changeService;
        this.navgationService = navgationService;
        this.isPreliminaryApproveChecked = false;
        this.isApproveChecked = false;
        this.isRejectChecked = false;
        this.changeRequestReview = { changeRequestId: 0, reviewedBy: "Me", reviewStatus: changeRequestReview_model_1.ChangeRequestStatus.Submitted };
        this.reviewComment = { comment: "" };
        this.likeStaging = { changeRequestId: 0, likedBy: "", likeStatus: "", likeCount: 0 };
        this.likeStagingGet = { changeRequestId: 0, likedBy: "", likeStatus: "", likeCount: 0 };
        this.showLoadingGif = false;
        this.backNavigation = "/dashboard";
        this.backNavigationText = "Return to Dashboard";
        this.isReviewCompleted = false;
        this.isLiked = false;
        //private _identityInfo: IIdentityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };
        this.usersForAssignment = [];
        this.assignedReviewer = { assignedToUserId: "-1", changeRequestIds: [] };
        this.showPrelimRadio = true;
        this.allLikedBy = [];
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('change') > 0)
                this.backNavigationText = "Return to Change Requests";
            else
                this.backNavigationText = "Return to Vehicle Search";
        }
    }
    BaseVehicleReviewComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.baseVehicleService.getChangeRequestStaging(id).subscribe(function (result) {
            _this.baseVehicleChangeRequest = result;
            // setup authorization components
            //this._identityInfo = this.sharedService.getIdentityInfo(String(this.baseVehicleChangeRequest.stagingItem.submittedBy));
            //if (this.baseVehicleChangeRequest.stagingItem.status === "Approved" || this.baseVehicleChangeRequest.stagingItem.status === "Deleted" ||
            //    this.baseVehicleChangeRequest.stagingItem.status === "Rejected") {
            //    this.isReviewCompleted = true;
            //} else if (this.baseVehicleChangeRequest.stagingItem.status === "PreliminaryApproved") {
            //    this.showPrelimRadio = false;
            //    if (this._identityInfo.isResearcher)
            //        this.isReviewCompleted = true;
            //}
            // check if Preliminary Approved.
            if (_this.baseVehicleChangeRequest.stagingItem.status == "PreliminaryApproved") {
                _this.showPrelimRadio = false;
            }
            if (_this.baseVehicleChangeRequest.stagingItem.changeType != "Replace") {
                // staging
                _this.makeService.getById(_this.baseVehicleChangeRequest.entityStaging.makeId).subscribe(function (m) {
                    _this.baseVehicleChangeRequest.entityStaging.makeName = m.name;
                    _this.modelService.getModelDetail(_this.baseVehicleChangeRequest.entityStaging.modelId).subscribe(function (m) {
                        _this.baseVehicleChangeRequest.entityStaging.modelName = m.name;
                        // set review's changerequestid
                        _this.changeRequestReview.changeRequestId = _this.baseVehicleChangeRequest.stagingItem.changeRequestId;
                        //this.getLikes(id);
                        _this.likeStagingService.getLikeDetails(id).subscribe(function (result) {
                            _this.likeStagingGet = result;
                            if (_this.likeStagingGet.likeStatus == "Like") {
                                _this.isLiked = true;
                            }
                            else if (!_this.likeStagingGet.likedBy) {
                                _this.isLiked = false;
                            }
                            _this.showLoadingGif = false;
                        }),
                            function (error) {
                                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                                _this.showLoadingGif = false;
                            };
                    }, function (error) {
                        _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                        _this.showLoadingGif = false;
                    });
                }, function (error) {
                    _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                    _this.showLoadingGif = false;
                });
                // existing
                if (_this.baseVehicleChangeRequest.entityCurrent != null) {
                    _this.makeService.getById(_this.baseVehicleChangeRequest.entityCurrent.makeId).subscribe(function (m) {
                        _this.baseVehicleChangeRequest.entityCurrent.makeName = m.name;
                        _this.modelService.getModelDetail(_this.baseVehicleChangeRequest.entityCurrent.modelId).subscribe(function (m) {
                            _this.baseVehicleChangeRequest.entityCurrent.modelName = m.name;
                        }, function (error) {
                            _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                            _this.showLoadingGif = false;
                        });
                    }, function (error) {
                        _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                        _this.showLoadingGif = false;
                    });
                }
            }
            else {
                //this.getLikes(id);
                _this.likeStagingService.getLikeDetails(id).subscribe(function (result) {
                    _this.likeStagingGet = result;
                    if (_this.likeStagingGet.likeStatus == "Like") {
                        _this.isLiked = true;
                    }
                    else if (!_this.likeStagingGet.likedBy) {
                        _this.isLiked = false;
                    }
                    _this.showLoadingGif = false;
                }),
                    function (error) {
                        _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
                        _this.showLoadingGif = false;
                    };
            }
        }, function (error) {
            _this.toastr.warning(error, "Change request review counldn't be loaded.");
            _this.showLoadingGif = false;
        });
        this.usersForAssignment = this.userService.getUsersForAssignment();
    };
    BaseVehicleReviewComponent.prototype.onReviewerChange = function () {
        if (this.assignedReviewer.assignedToUserId !== "-1" && !this.isPreliminaryApproveChecked) {
            this.isApproveChecked = false;
            this.isRejectChecked = false;
        }
    };
    BaseVehicleReviewComponent.prototype.onViewAffectedVehicles = function () {
        var _this = this;
        if (!this.baseVehicleChangeRequest || !this.baseVehicleChangeRequest.entityCurrent) {
            return;
        }
        this.viewAffectedVehiclesPopup.open("lg");
        if (!this.baseVehicleChangeRequest.entityCurrent.vehicles) {
            this.vehicleService.getVehiclesByBaseVehicleId(this.baseVehicleChangeRequest.entityCurrent.id).subscribe(function (m) {
                _this.baseVehicleChangeRequest.entityCurrent.vehicles = m;
                _this.baseVehicleChangeRequest.entityCurrent.vehicleCount = m.length;
            }, function (error) { return _this.toastr.warning(error); });
        }
    };
    BaseVehicleReviewComponent.prototype.onChange = function (approved) {
        if (this.assignedReviewer.assignedToUserId !== "-1" && approved !== "PreliminaryApprove") {
            this.assignedReviewer.assignedToUserId = "-1";
        }
        if (approved == "PreliminaryApprove") {
            if (!this.isPreliminaryApproveChecked) {
                this.isPreliminaryApproveChecked = true;
                this.isApproveChecked = false;
                this.isRejectChecked = false;
            }
        }
        else if (approved == "Approve") {
            if (!this.isApproveChecked) {
                this.isPreliminaryApproveChecked = false;
                this.isApproveChecked = true;
                this.isRejectChecked = false;
            }
        }
        else if (approved == "Reject") {
            if (!this.isRejectChecked) {
                this.isPreliminaryApproveChecked = false;
                this.isApproveChecked = false;
                this.isRejectChecked = true;
            }
        }
    };
    BaseVehicleReviewComponent.prototype.onSubmitReview = function (deletebuttonclicked) {
        var _this = this;
        if (!this.validationCheck()) {
            if (this.assignedReviewer.assignedToUserId !== "-1") {
                // note: the value on this.assignedReviewer.assignedToUserId is -1 by default, but type is string. Default is -1 not "".
                // if assingedReviewer-index is changed, then the action of Submit is assign-reviewer.
                this.onAssignReviewer();
                return;
            }
            else if (!deletebuttonclicked) {
                this.toastr.warning("No changes to be submitted!!", constants_warehouse_1.ConstantsWarehouse.validationTitle);
                return;
            }
        }
        // fill change-request-review
        if (deletebuttonclicked == 'delete') {
            this.changeRequestReview.reviewStatus = changeRequestReview_model_1.ChangeRequestStatus.Deleted;
        }
        else {
            if (this.isPreliminaryApproveChecked) {
                this.changeRequestReview.reviewStatus = changeRequestReview_model_1.ChangeRequestStatus.PreliminaryApproved;
            }
            else if (this.isApproveChecked) {
                this.changeRequestReview.reviewStatus = changeRequestReview_model_1.ChangeRequestStatus.Approved;
            }
            else if (this.isRejectChecked) {
                this.changeRequestReview.reviewStatus = changeRequestReview_model_1.ChangeRequestStatus.Rejected;
            }
            else {
                this.changeRequestReview.reviewStatus = changeRequestReview_model_1.ChangeRequestStatus[this.baseVehicleChangeRequest.stagingItem.status];
            }
        }
        this.changeRequestReview.reviewComment = this.reviewComment;
        this.showLoadingGif = true;
        this.newAcFileUploader.uploadAttachments().subscribe(function (uploadedFiles) {
            if (uploadedFiles && uploadedFiles.length > 0) {
                _this.changeRequestReview.attachments = uploadedFiles;
            }
            // new deleted attachments
            if (_this.newAcFileUploader.getFilesMarkedToDelete()) {
                if (_this.changeRequestReview.attachments) {
                    _this.changeRequestReview.attachments = _this.changeRequestReview.attachments.concat(_this.newAcFileUploader.getFilesMarkedToDelete());
                }
                else {
                    _this.changeRequestReview.attachments = _this.newAcFileUploader.getFilesMarkedToDelete();
                }
            }
            // existing attachments
            if (_this.existingAcFileUploader.getFilesMarkedToDelete()) {
                if (_this.changeRequestReview.attachments) {
                    _this.changeRequestReview.attachments = _this.changeRequestReview.attachments.concat(_this.existingAcFileUploader.getFilesMarkedToDelete());
                }
                else {
                    _this.changeRequestReview.attachments = _this.existingAcFileUploader.getFilesMarkedToDelete();
                }
            }
            var baseVehicleIdentity = _this.baseVehicleChangeRequest.entityStaging.makeName + ", " + _this.baseVehicleChangeRequest.entityStaging.modelName;
            _this.baseVehicleService.submitChangeRequestReview(_this.baseVehicleChangeRequest.stagingItem.changeRequestId, _this.changeRequestReview).subscribe(function (result) {
                if (result) {
                    var toasterMessage = null;
                    if (_this.changeRequestReview.reviewStatus != changeRequestReview_model_1.ChangeRequestStatus.Deleted) {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitSuccessTitle,
                            body: "Your review for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + " change request ID  '" + _this.baseVehicleChangeRequest.stagingItem.changeRequestId + "'  was submitted successfully."
                        };
                    }
                    else {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewDeleteSuccessTitle,
                            body: "Your delete request for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + "  change request ID '" + _this.baseVehicleChangeRequest.stagingItem.changeRequestId + "' was submitted successfully."
                        };
                    }
                    _this.toastr.success(toasterMessage.body, toasterMessage.title);
                    if (_this.assignedReviewer.assignedToUserId !== "-1")
                        _this.onAssignReviewer();
                    else
                        _this.router.navigateByUrl(_this.backNavigation);
                }
                else {
                    var toasterMessage = null;
                    if (_this.changeRequestReview.reviewStatus != changeRequestReview_model_1.ChangeRequestStatus.Deleted) {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitErrorTitle,
                            body: "Your review for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + "  change request ID  '" + _this.baseVehicleChangeRequest.stagingItem.changeRequestId + "' failed."
                        };
                    }
                    else {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewDeleteErrorTitle,
                            body: "Your delete request for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + "  change request ID  '" + _this.baseVehicleChangeRequest.stagingItem.changeRequestId + "' failed."
                        };
                    }
                    _this.toastr.warning(toasterMessage.body, toasterMessage.title);
                    _this.showLoadingGif = false;
                }
            }, function (error) {
                var toasterMessage = null;
                if (_this.changeRequestReview.reviewStatus != changeRequestReview_model_1.ChangeRequestStatus.Deleted) {
                    toasterMessage = {
                        title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitErrorTitle,
                        body: "Your review for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + " failed.\n" + error.toString() + "."
                    };
                }
                else {
                    toasterMessage = {
                        title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitErrorTitle,
                        body: "Your delete request for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + " failed.\n" + error.toString() + "."
                    };
                }
                _this.toastr.warning(toasterMessage.body, toasterMessage.title);
                _this.showLoadingGif = false;
            }, function () {
                _this.newAcFileUploader.reset();
                //this.showLoadingGif = false;
            });
        }, function (error) {
            _this.newAcFileUploader.reset();
            _this.toastr.warning(error, "Attachment couldn't be uploaded.");
            _this.showLoadingGif = false;
        });
    };
    BaseVehicleReviewComponent.prototype.onAssignReviewer = function () {
        var _this = this;
        var user = this.usersForAssignment.filter(function (x) { return x.id === _this.assignedReviewer.assignedToUserId; })[0];
        this.assignedReviewer.assignedToRoleId = user.roleId;
        this.assignedReviewer.changeRequestIds[0] = this.baseVehicleChangeRequest.stagingItem.changeRequestId;
        var baseVehicleIdentity = this.baseVehicleChangeRequest.entityStaging.makeName + ", " + this.baseVehicleChangeRequest.entityStaging.modelName;
        this.changeService.assignReviewer(this.assignedReviewer).subscribe(function (result) {
            if (result) {
                var toasterMessage = {
                    title: "Your review was assigned to " + user.user,
                    body: "Your review for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + " assigned successfully."
                };
                _this.toastr.success(toasterMessage.body, toasterMessage.title);
                // redirect to search result
                _this.router.navigateByUrl(_this.backNavigation);
            }
            else {
                var toasterMessage = {
                    title: "Your review couldn't be assigned.",
                    body: "Your assignment for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + " failed."
                };
                _this.toastr.warning(toasterMessage.body, toasterMessage.title);
            }
        }, function (error) {
            _this.toastr.warning("Your assignment for " + _this.baseVehicleChangeRequest.stagingItem.changeType + " \n                 " + _this.baseVehicleChangeRequest.stagingItem.entityName + " : " + baseVehicleIdentity + " failed.\n" + error + ".", "Your review couldn't be assigned.");
        });
    };
    BaseVehicleReviewComponent.prototype.onCancel = function () {
        // redirect to search result
        this.router.navigateByUrl(this.backNavigation);
    };
    BaseVehicleReviewComponent.prototype.validationCheck = function () {
        var status = false;
        //Status Changed
        if (this.isPreliminaryApproveChecked !== false || this.isApproveChecked !== false || this.isRejectChecked !== false) {
            status = true;
            return true;
        }
        //Comments Added
        if (this.reviewComment.comment.length > 0) {
            status = true;
            return true;
        }
        //Attachment Uploaded
        if (this.newAcFileUploader.checkAttachment()) {
            status = true;
            return status;
        }
        return status;
    };
    BaseVehicleReviewComponent.prototype.onLikeClick = function (status) {
        var _this = this;
        var id = Number(this.route.snapshot.params["id"]);
        this.likeStaging.changeRequestId = id;
        this.likeStaging.likeStatus = status;
        //this.likeStaging.likedBy = this._identityInfo.customerId;
        this.isLiked = true;
        this.likeStagingService.submitLike(this.likeStaging.changeRequestId, this.likeStaging).subscribe(function (result) {
            if (result) {
                _this.toastr.success("Your like was submitted successfully.", "Your like was submitted.");
            }
            else {
                _this.toastr.warning("Your like for " + _this.likeStaging.changeRequestId + " failed.", "Your like couldn't be submitted.");
            }
            _this.getLikes(id);
            _this.showLoadingGif = false;
        }, function (error) {
            var errorMessage = JSON.parse(String(error)).message;
            _this.toastr.warning("You like for " + _this.likeStaging.changeRequestId + " failed.\n" + errorMessage + ".error", "Your like couldn't be submitted.");
            _this.showLoadingGif = false;
        });
    };
    BaseVehicleReviewComponent.prototype.getLikes = function (changeRequestId) {
        var _this = this;
        this.likeStagingService.getLikeDetails(changeRequestId).subscribe(function (result) {
            _this.likeStagingGet = result;
            if (_this.likeStagingGet.likeStatus == "Like") {
                _this.isLiked = true;
            }
            else if (!_this.likeStagingGet.likedBy) {
                _this.isLiked = false;
            }
        }),
            function (error) {
                _this.toastr.warning(error, constants_warehouse_1.ConstantsWarehouse.errorTitle);
            };
    };
    BaseVehicleReviewComponent.prototype.onClickLikes = function (id) {
        this.likeComponent.showAllLikedBy(id);
    };
    BaseVehicleReviewComponent.prototype.cleanupComponent = function () {
        return this.newAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("viewAffectedVehiclesPopup"), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], BaseVehicleReviewComponent.prototype, "viewAffectedVehiclesPopup", void 0);
    __decorate([
        core_1.ViewChild("newAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleReviewComponent.prototype, "newAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("existingAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BaseVehicleReviewComponent.prototype, "existingAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("likeComponent"), 
        __metadata('design:type', userLikes_component_1.UserLikesComponent)
    ], BaseVehicleReviewComponent.prototype, "likeComponent", void 0);
    BaseVehicleReviewComponent = __decorate([
        core_1.Component({
            selector: 'baseVehicle-review-comp',
            templateUrl: 'app/templates/baseVehicle/baseVehicle-review.component.html',
            providers: [baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, model_service_1.ModelService, make_service_1.MakeService, likeStaging_service_1.LikeStagingService, user_service_1.UserService, change_service_1.ChangeService],
        }), 
        __metadata('design:paramtypes', [baseVehicle_service_1.BaseVehicleService, vehicle_service_1.VehicleService, make_service_1.MakeService, model_service_1.ModelService, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute, router_1.Router, likeStaging_service_1.LikeStagingService, shared_service_1.SharedService, user_service_1.UserService, change_service_1.ChangeService, navigation_service_1.NavigationService])
    ], BaseVehicleReviewComponent);
    return BaseVehicleReviewComponent;
}());
exports.BaseVehicleReviewComponent = BaseVehicleReviewComponent;
