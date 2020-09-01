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
var ModalHeaderComponent = (function () {
    function ModalHeaderComponent(modal) {
        this.modal = modal;
        this.showClose = false;
    }
    __decorate([
        core_1.Input('show-close'), 
        __metadata('design:type', Boolean)
    ], ModalHeaderComponent.prototype, "showClose", void 0);
    ModalHeaderComponent = __decorate([
        core_1.Component({
            selector: 'modal-header',
            template: "\n        <div class=\"modal-header\">\n            <button *ngIf=\"showClose\" type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n                <span aria-hidden=\"true\">&times;</span>\n            </button>\n            <ng-content></ng-content>\n        </div>\n    "
        }), 
        __metadata('design:paramtypes', [modal_1.ModalComponent])
    ], ModalHeaderComponent);
    return ModalHeaderComponent;
}());
exports.ModalHeaderComponent = ModalHeaderComponent;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwtaGVhZGVyLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vc3JjL2NvbXBvbmVudHMvbW9kYWwtaGVhZGVyLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7QUFBQSxxQkFBeUMsZUFBZSxDQUFDLENBQUE7QUFDekQsc0JBQStCLFNBQVMsQ0FBQyxDQUFBO0FBYXpDO0lBRUksOEJBQW9CLEtBQXFCO1FBQXJCLFVBQUssR0FBTCxLQUFLLENBQWdCO1FBRHBCLGNBQVMsR0FBWSxLQUFLLENBQUM7SUFDSCxDQUFDO0lBRDlDO1FBQUMsWUFBSyxDQUFDLFlBQVksQ0FBQzs7MkRBQUE7SUFaeEI7UUFBQyxnQkFBUyxDQUFDO1lBQ1AsUUFBUSxFQUFFLGNBQWM7WUFDeEIsUUFBUSxFQUFFLHlVQU9UO1NBQ0osQ0FBQzs7NEJBQUE7SUFJRiwyQkFBQztBQUFELENBQUMsQUFIRCxJQUdDO0FBSFksNEJBQW9CLHVCQUdoQyxDQUFBIn0=