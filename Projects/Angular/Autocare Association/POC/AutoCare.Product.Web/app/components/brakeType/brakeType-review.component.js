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
var brakeType_service_1 = require("./brakeType.service");
var constants_warehouse_1 = require("../constants-warehouse");
var router_1 = require("@angular/router");
var changeRequestReview_model_1 = require("../changeRequestReview/changeRequestReview.model");
var likeStaging_service_1 = require("../changeRequestReview/likeStaging.service");
var ac_fileuploader_1 = require('../../lib/aclibs/ac-fileuploader/ac-fileuploader');
var shared_service_1 = require('../shared/shared.service');
var navigation_service_1 = require('../shared/navigation.service');
var user_service_1 = require("../change/user.service");
var change_service_1 = require("../change/change.service");
var userLikes_component_1 = require("../changeRequestReview/userLikes.component");
var BrakeTypeReviewComponent = (function () {
    function BrakeTypeReviewComponent(brakeTypeService, likeStagingService, toastr, route, router, sharedService, userService, changeService, navgationService) {
        this.brakeTypeService = brakeTypeService;
        this.likeStagingService = likeStagingService;
        this.toastr = toastr;
        this.route = route;
        this.router = router;
        this.sharedService = sharedService;
        this.userService = userService;
        this.changeService = changeService;
        this.navgationService = navgationService;
        this.usersForAssignment = [];
        this.assignedReviewer = { assignedToUserId: "-1", changeRequestIds: [] };
        this.isPreliminaryApproveChecked = false;
        this.isApproveChecked = false;
        this.isRejectChecked = false;
        this.changeRequestReview = { changeRequestId: 0, reviewedBy: "Me", reviewStatus: changeRequestReview_model_1.ChangeRequestStatus.Submitted };
        this.reviewComment = { comment: "" };
        this.likeStaging = { changeRequestId: 0, likedBy: "Me", likeStatus: "like" };
        this.likeStagingGet = { changeRequestId: 0, likedBy: "", likeStatus: "", likeCount: 0 };
        this.reviewCompleted = false;
        this.showLoadingGif = false;
        this.backNavigation = "/dashboard";
        this.backNavigationText = "Return to Dashboard";
        this.isLiked = true;
        //private _identityInfo: IIdentityInfo = { customerId: "", email: "", isAdmin: false, isRequestor: false, isResearcher: false };
        this.showPrelimRadio = true;
        if (this.navgationService.backRoute) {
            this.backNavigation = this.navgationService.backRoute;
            if (this.backNavigation.indexOf('change') > 0)
                this.backNavigationText = "Return to Change Requests";
            else
                this.backNavigationText = "Return to Reference Data";
        }
    }
    BrakeTypeReviewComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.showLoadingGif = true;
        // Load existing base vechile with reference from RouteParams
        var id = Number(this.route.snapshot.params["id"]);
        this.brakeTypeService.getChangeRequestStaging(id).subscribe(function (result) {
            _this.brakeTypeChangeRequest = result;
            // set review's changerequestid
            _this.changeRequestReview.changeRequestId = _this.brakeTypeChangeRequest.stagingItem.changeRequestId;
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
            //this._identityInfo = this.sharedService.getIdentityInfo(String(this.brakeTypeChangeRequest.stagingItem.submittedBy));
            //if (this.brakeTypeChangeRequest.stagingItem.status === "Approved" || this.brakeTypeChangeRequest.stagingItem.status === "Deleted" || this.brakeTypeChangeRequest.stagingItem.status === "Rejected") {
            //    this.reviewCompleted = true;
            //} else if (this.brakeTypeChangeRequest.stagingItem.status === "PreliminaryApproved") {
            //    this.showPrelimRadio = false;
            //    if (this._identityInfo.isResearcher)
            //        this.reviewCompleted = true;
            //}
            // check if Preliminary Approved.
            if (_this.brakeTypeChangeRequest.stagingItem.status == "PreliminaryApproved") {
                _this.showPrelimRadio = false;
            }
        }, function (error) {
            var toasterMessage = {
                title: "Change request review counldn't be loaded.",
                body: error.toString()
            };
            _this.toastr.success(toasterMessage.body, toasterMessage.title);
            _this.showLoadingGif = false;
        });
        this.usersForAssignment = this.userService.getUsersForAssignment();
    };
    BrakeTypeReviewComponent.prototype.onReviewerChange = function () {
        if (this.assignedReviewer.assignedToUserId !== "-1" && !this.isPreliminaryApproveChecked) {
            this.isApproveChecked = false;
            this.isRejectChecked = false;
        }
    };
    BrakeTypeReviewComponent.prototype.onChange = function (approved) {
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
    BrakeTypeReviewComponent.prototype.onSubmitReview = function (deletebuttonclicked) {
        var _this = this;
        if (!this.validationCheck()) {
            if (this.assignedReviewer.assignedToUserId !== "-1" && deletebuttonclicked != 'delete') {
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
                this.changeRequestReview.reviewStatus = changeRequestReview_model_1.ChangeRequestStatus[this.brakeTypeChangeRequest.stagingItem.status];
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
            _this.brakeTypeService.submitChangeRequestReview(_this.brakeTypeChangeRequest.stagingItem.changeRequestId, _this.changeRequestReview).subscribe(function (result) {
                if (result) {
                    var toasterMessage = null;
                    if (_this.changeRequestReview.reviewStatus != changeRequestReview_model_1.ChangeRequestStatus.Deleted) {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitSuccessTitle,
                            body: "Your review for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + " change request ID " + _this.brakeTypeChangeRequest.stagingItem.changeRequestId + " was submitted successfully."
                        };
                    }
                    else {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewDeleteSuccessTitle,
                            body: "Your delete request for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + "  change request ID " + _this.brakeTypeChangeRequest.stagingItem.changeRequestId + " was submitted successfully."
                        };
                    }
                    _this.toastr.success(toasterMessage.body, toasterMessage.title);
                    if (_this.assignedReviewer.assignedToUserId !== "-1" && deletebuttonclicked != 'delete')
                        _this.onAssignReviewer();
                    else
                        _this.router.navigateByUrl(_this.backNavigation);
                }
                else {
                    var toasterMessage = null;
                    if (_this.changeRequestReview.reviewStatus != changeRequestReview_model_1.ChangeRequestStatus.Deleted) {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitErrorTitle,
                            body: "Your review for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + "  change request ID " + _this.brakeTypeChangeRequest.stagingItem.changeRequestId + " failed."
                        };
                    }
                    else {
                        toasterMessage = {
                            title: constants_warehouse_1.ConstantsWarehouse.reviewDeleteErrorTitle,
                            body: "Your delete request for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + "  change request ID " + _this.brakeTypeChangeRequest.stagingItem.changeRequestId + " failed."
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
                        body: "Your review for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + " change request ID " + _this.brakeTypeChangeRequest.stagingItem.changeRequestId + " failed.\n" + error.toString() + "."
                    };
                }
                else {
                    toasterMessage = {
                        title: constants_warehouse_1.ConstantsWarehouse.reviewSubmitErrorTitle,
                        body: "Your delete request for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + "  change request ID " + _this.brakeTypeChangeRequest.stagingItem.changeRequestId + " failed .\n" + error.toString() + "."
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
            var toasterMessage = {
                title: "Attachment couldn't be uploaded.",
                body: error.toString()
            };
            _this.toastr.warning(toasterMessage.body, toasterMessage.title);
            _this.showLoadingGif = false;
        });
    };
    BrakeTypeReviewComponent.prototype.onAssignReviewer = function () {
        var _this = this;
        var user = this.usersForAssignment.filter(function (x) { return x.id === _this.assignedReviewer.assignedToUserId; })[0];
        this.assignedReviewer.assignedToRoleId = user.roleId;
        this.assignedReviewer.changeRequestIds[0] = this.brakeTypeChangeRequest.stagingItem.changeRequestId;
        this.changeService.assignReviewer(this.assignedReviewer).subscribe(function (result) {
            if (result) {
                var toasterMessage = {
                    title: "Your review was assigned to " + user.user,
                    body: "Your review for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + " assigned successfully."
                };
                _this.toastr.success(toasterMessage.body, toasterMessage.title);
                // redirect to search result
                _this.router.navigateByUrl(_this.backNavigation);
            }
            else {
                var toasterMessage = {
                    title: "Your review couldn't be assigned.",
                    body: "Your assignment for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + " failed."
                };
                _this.toastr.warning(toasterMessage.body, toasterMessage.title);
            }
        }, function (error) {
            var toasterMessage = {
                title: "Your review was couldn't be assigned.",
                body: "Your assignment for " + _this.brakeTypeChangeRequest.stagingItem.changeType + " " + _this.brakeTypeChangeRequest.stagingItem.entityName + " : " + _this.brakeTypeChangeRequest.entityStaging.name + " failed.\n" + error.toString() + "."
            };
            _this.toastr.warning(toasterMessage.body, toasterMessage.title);
        });
    };
    BrakeTypeReviewComponent.prototype.onCancel = function () {
        // redirect to search result
        this.router.navigateByUrl(this.backNavigation);
    };
    BrakeTypeReviewComponent.prototype.validationCheck = function () {
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
    BrakeTypeReviewComponent.prototype.onLikeClick = function (status) {
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
    BrakeTypeReviewComponent.prototype.getLikes = function (changeRequestId) {
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
    BrakeTypeReviewComponent.prototype.onClickLikes = function (id) {
        this.likeComponent.showAllLikedBy(id);
    };
    BrakeTypeReviewComponent.prototype.cleanupComponent = function () {
        return this.newAcFileUploader.cleanupAllTempContainers();
    };
    __decorate([
        core_1.ViewChild("newAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeTypeReviewComponent.prototype, "newAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("existingAcFileUploader"), 
        __metadata('design:type', ac_fileuploader_1.AcFileUploader)
    ], BrakeTypeReviewComponent.prototype, "existingAcFileUploader", void 0);
    __decorate([
        core_1.ViewChild("likeComponent"), 
        __metadata('design:type', userLikes_component_1.UserLikesComponent)
    ], BrakeTypeReviewComponent.prototype, "likeComponent", void 0);
    BrakeTypeReviewComponent = __decorate([
        core_1.Component({
            selector: 'brakeType-review-comp',
            templateUrl: 'app/templates/brakeType/brakeType-review.component.html',
            providers: [brakeType_service_1.BrakeTypeService, likeStaging_service_1.LikeStagingService, user_service_1.UserService, change_service_1.ChangeService],
        }), 
        __metadata('design:paramtypes', [brakeType_service_1.BrakeTypeService, likeStaging_service_1.LikeStagingService, ng2_toastr_1.ToastsManager, router_1.ActivatedRoute, router_1.Router, shared_service_1.SharedService, user_service_1.UserService, change_service_1.ChangeService, navigation_service_1.NavigationService])
    ], BrakeTypeReviewComponent);
    return BrakeTypeReviewComponent;
}());
exports.BrakeTypeReviewComponent = BrakeTypeReviewComponent;
