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
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
var core_1 = require('@angular/core');
var common_1 = require('@angular/common');
var modal_1 = require('./components/modal');
var modal_header_1 = require('./components/modal-header');
var modal_body_1 = require('./components/modal-body');
var modal_footer_1 = require('./components/modal-footer');
var autofocus_1 = require('./directives/autofocus');
__export(require('./components/modal'));
__export(require('./components/modal-header'));
__export(require('./components/modal-body'));
__export(require('./components/modal-footer'));
__export(require('./components/modal-instance'));
var Ng2Bs3ModalModule = (function () {
    function Ng2Bs3ModalModule() {
    }
    Ng2Bs3ModalModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule
            ],
            declarations: [
                modal_1.ModalComponent,
                modal_header_1.ModalHeaderComponent,
                modal_body_1.ModalBodyComponent,
                modal_footer_1.ModalFooterComponent,
                autofocus_1.AutofocusDirective
            ],
            exports: [
                modal_1.ModalComponent,
                modal_header_1.ModalHeaderComponent,
                modal_body_1.ModalBodyComponent,
                modal_footer_1.ModalFooterComponent,
                autofocus_1.AutofocusDirective
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], Ng2Bs3ModalModule);
    return Ng2Bs3ModalModule;
}());
exports.Ng2Bs3ModalModule = Ng2Bs3ModalModule;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmcyLWJzMy1tb2RhbC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbInNyYy9uZzItYnMzLW1vZGFsLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7Ozs7Ozs7QUFBQSxxQkFBeUIsZUFBZSxDQUFDLENBQUE7QUFDekMsdUJBQTZCLGlCQUFpQixDQUFDLENBQUE7QUFFL0Msc0JBQStCLG9CQUFvQixDQUFDLENBQUE7QUFDcEQsNkJBQXFDLDJCQUEyQixDQUFDLENBQUE7QUFDakUsMkJBQW1DLHlCQUF5QixDQUFDLENBQUE7QUFDN0QsNkJBQXFDLDJCQUEyQixDQUFDLENBQUE7QUFDakUsMEJBQW1DLHdCQUF3QixDQUFDLENBQUE7QUFFNUQsaUJBQWMsb0JBQW9CLENBQUMsRUFBQTtBQUNuQyxpQkFBYywyQkFBMkIsQ0FBQyxFQUFBO0FBQzFDLGlCQUFjLHlCQUF5QixDQUFDLEVBQUE7QUFDeEMsaUJBQWMsMkJBQTJCLENBQUMsRUFBQTtBQUMxQyxpQkFBYyw2QkFBNkIsQ0FBQyxFQUFBO0FBcUI1QztJQUFBO0lBQ0EsQ0FBQztJQXBCRDtRQUFDLGVBQVEsQ0FBQztZQUNOLE9BQU8sRUFBRTtnQkFDTCxxQkFBWTthQUNmO1lBQ0QsWUFBWSxFQUFFO2dCQUNWLHNCQUFjO2dCQUNkLG1DQUFvQjtnQkFDcEIsK0JBQWtCO2dCQUNsQixtQ0FBb0I7Z0JBQ3BCLDhCQUFrQjthQUNyQjtZQUNELE9BQU8sRUFBRTtnQkFDTCxzQkFBYztnQkFDZCxtQ0FBb0I7Z0JBQ3BCLCtCQUFrQjtnQkFDbEIsbUNBQW9CO2dCQUNwQiw4QkFBa0I7YUFDckI7U0FDSixDQUFDOzt5QkFBQTtJQUVGLHdCQUFDO0FBQUQsQ0FBQyxBQURELElBQ0M7QUFEWSx5QkFBaUIsb0JBQzdCLENBQUEifQ==