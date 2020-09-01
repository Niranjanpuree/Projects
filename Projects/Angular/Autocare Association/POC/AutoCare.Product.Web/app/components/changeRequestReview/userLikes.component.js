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
var likeStaging_service_1 = require("./likeStaging.service");
var ng2_bs3_modal_1 = require("ng2-bs3-modal/ng2-bs3-modal");
var UserLikesComponent = (function () {
    function UserLikesComponent(likeStagingService) {
        this.likeStagingService = likeStagingService;
        this.likes = [];
        this.errorMessage = "";
    }
    UserLikesComponent.prototype.showAllLikedBy = function (id) {
        var _this = this;
        this.likeStagingService.getAllLikedBy(id).subscribe(function (result) {
            _this.likes = result;
            _this.likedByPopupModel.open("md");
        });
    };
    __decorate([
        core_1.ViewChild('likedByPopupModel'), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], UserLikesComponent.prototype, "likedByPopupModel", void 0);
    UserLikesComponent = __decorate([
        core_1.Component({
            selector: 'user-likes-comp',
            templateUrl: 'app/templates/changeRequestReview/userLikes.component.html',
            providers: [likeStaging_service_1.LikeStagingService],
        }), 
        __metadata('design:paramtypes', [likeStaging_service_1.LikeStagingService])
    ], UserLikesComponent);
    return UserLikesComponent;
}());
exports.UserLikesComponent = UserLikesComponent;
