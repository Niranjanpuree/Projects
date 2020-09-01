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
var modal_1 = require('./modal');
var ModalFooterComponent = (function () {
    function ModalFooterComponent(modal) {
        this.modal = modal;
        this.showDefaultButtons = false;
        this.dismissButtonLabel = 'Dismiss';
        this.closeButtonLabel = 'Close';
    }
    __decorate([
        core_1.Input('show-default-buttons'), 
        __metadata('design:type', Boolean)
    ], ModalFooterComponent.prototype, "showDefaultButtons", void 0);
    __decorate([
        core_1.Input('dismiss-button-label'), 
        __metadata('design:type', String)
    ], ModalFooterComponent.prototype, "dismissButtonLabel", void 0);
    __decorate([
        core_1.Input('close-button-label'), 
        __metadata('design:type', String)
    ], ModalFooterComponent.prototype, "closeButtonLabel", void 0);
    ModalFooterComponent = __decorate([
        core_1.Component({
            selector: 'modal-footer',
            template: "\n        <div class=\"modal-footer\">\n            <ng-content></ng-content>\n            <button *ngIf=\"showDefaultButtons\" type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" (click)=\"modal.dismiss()\">{{dismissButtonLabel}}</button>\n            <button *ngIf=\"showDefaultButtons\" type=\"button\" class=\"btn btn-primary\" (click)=\"modal.close()\">{{closeButtonLabel}}</button>\n        </div>\n    "
        }), 
        __metadata('design:paramtypes', [modal_1.ModalComponent])
    ], ModalFooterComponent);
    return ModalFooterComponent;
}());
exports.ModalFooterComponent = ModalFooterComponent;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwtZm9vdGVyLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vc3JjL2NvbXBvbmVudHMvbW9kYWwtZm9vdGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7QUFBQSxxQkFBeUMsZUFBZSxDQUFDLENBQUE7QUFDekQsc0JBQStCLFNBQVMsQ0FBQyxDQUFBO0FBWXpDO0lBSUksOEJBQW9CLEtBQXFCO1FBQXJCLFVBQUssR0FBTCxLQUFLLENBQWdCO1FBSFYsdUJBQWtCLEdBQVksS0FBSyxDQUFDO1FBQ3BDLHVCQUFrQixHQUFXLFNBQVMsQ0FBQztRQUN6QyxxQkFBZ0IsR0FBVyxPQUFPLENBQUM7SUFDbkIsQ0FBQztJQUg5QztRQUFDLFlBQUssQ0FBQyxzQkFBc0IsQ0FBQzs7b0VBQUE7SUFDOUI7UUFBQyxZQUFLLENBQUMsc0JBQXNCLENBQUM7O29FQUFBO0lBQzlCO1FBQUMsWUFBSyxDQUFDLG9CQUFvQixDQUFDOztrRUFBQTtJQWJoQztRQUFDLGdCQUFTLENBQUM7WUFDUCxRQUFRLEVBQUUsY0FBYztZQUN4QixRQUFRLEVBQUUsd2FBTVQ7U0FDSixDQUFDOzs0QkFBQTtJQU1GLDJCQUFDO0FBQUQsQ0FBQyxBQUxELElBS0M7QUFMWSw0QkFBb0IsdUJBS2hDLENBQUEifQ==