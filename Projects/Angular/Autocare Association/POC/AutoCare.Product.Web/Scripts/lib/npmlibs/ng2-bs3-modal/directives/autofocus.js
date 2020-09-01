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
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
var core_1 = require('@angular/core');
var modal_1 = require('../components/modal');
var AutofocusDirective = (function () {
    function AutofocusDirective(el, modal) {
        var _this = this;
        this.el = el;
        this.modal = modal;
        if (modal) {
            this.modal.onOpen.subscribe(function () {
                _this.el.nativeElement.focus();
            });
        }
    }
    AutofocusDirective = __decorate([
        core_1.Directive({
            selector: '[autofocus]'
        }),
        __param(1, core_1.Optional()), 
        __metadata('design:paramtypes', [core_1.ElementRef, modal_1.ModalComponent])
    ], AutofocusDirective);
    return AutofocusDirective;
}());
exports.AutofocusDirective = AutofocusDirective;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsiLi4vc3JjL2RpcmVjdGl2ZXMvYXV0b2ZvY3VzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7Ozs7QUFBQSxxQkFBd0QsZUFBZSxDQUFDLENBQUE7QUFDeEUsc0JBQStCLHFCQUFxQixDQUFDLENBQUE7QUFLckQ7SUFDSSw0QkFBb0IsRUFBYyxFQUFzQixLQUFxQjtRQURqRixpQkFRQztRQVB1QixPQUFFLEdBQUYsRUFBRSxDQUFZO1FBQXNCLFVBQUssR0FBTCxLQUFLLENBQWdCO1FBQ3pFLEVBQUUsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUM7WUFDUixJQUFJLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxTQUFTLENBQUM7Z0JBQ3hCLEtBQUksQ0FBQyxFQUFFLENBQUMsYUFBYSxDQUFDLEtBQUssRUFBRSxDQUFDO1lBQ2xDLENBQUMsQ0FBQyxDQUFDO1FBQ1AsQ0FBQztJQUNMLENBQUM7SUFWTDtRQUFDLGdCQUFTLENBQUM7WUFDUCxRQUFRLEVBQUUsYUFBYTtTQUMxQixDQUFDO21CQUV1QyxlQUFRLEVBQUU7OzBCQUZqRDtJQVNGLHlCQUFDO0FBQUQsQ0FBQyxBQVJELElBUUM7QUFSWSwwQkFBa0IscUJBUTlCLENBQUEifQ==