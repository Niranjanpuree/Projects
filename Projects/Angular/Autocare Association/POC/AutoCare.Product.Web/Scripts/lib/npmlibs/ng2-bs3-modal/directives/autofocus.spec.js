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
var testing_1 = require('@angular/core/testing');
var ng2_bs3_modal_1 = require('../ng2-bs3-modal');
var common_1 = require('../test/common');
describe('AutofocusDirective', function () {
    var fixture;
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [ng2_bs3_modal_1.Ng2Bs3ModalModule],
            declarations: [TestComponent, MissingModalComponent]
        });
    });
    afterEach(testing_1.fakeAsync(function () {
        testing_1.TestBed.resetTestingModule();
        testing_1.tick(300); // backdrop transition
        testing_1.tick(150); // modal transition
    }));
    it('should not throw an error if a modal isn\'t present', function () {
        var fixture = common_1.createRoot(MissingModalComponent);
    });
    it('should autofocus on element when modal is opened', testing_1.fakeAsync(function () {
        var fixture = common_1.createRoot(TestComponent);
        fixture.componentInstance.open();
        testing_1.tick();
        expect(document.getElementById('text')).toBe(document.activeElement);
    }));
    it('should autofocus on element when modal is opened with animations', testing_1.fakeAsync(function () {
        var fixture = common_1.createRoot(TestComponent);
        fixture.componentInstance.animation = true;
        fixture.detectChanges();
        fixture.componentInstance.open();
        testing_1.tick(150); // backdrop transition
        testing_1.tick(300); // modal transition
        expect(document.getElementById('text')).toBe(document.activeElement);
    }));
});
var TestComponent = (function () {
    function TestComponent() {
        this.animation = false;
    }
    TestComponent.prototype.open = function () {
        return this.modal.open();
    };
    __decorate([
        core_1.ViewChild(ng2_bs3_modal_1.ModalComponent), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], TestComponent.prototype, "modal", void 0);
    TestComponent = __decorate([
        core_1.Component({
            selector: 'test-component',
            template: "\n        <modal #modal [animation]=\"animation\">\n            <modal-header [show-close]=\"true\">\n                <h4 class=\"modal-title\">I'm a modal!</h4>\n            </modal-header>\n            <modal-body>\n                <input type=\"text\" id=\"text\" autofocus />\n            </modal-body>\n            <modal-footer [show-default-buttons]=\"true\"></modal-footer>\n        </modal>\n    "
        }), 
        __metadata('design:paramtypes', [])
    ], TestComponent);
    return TestComponent;
}());
var MissingModalComponent = (function () {
    function MissingModalComponent() {
    }
    MissingModalComponent = __decorate([
        core_1.Component({
            selector: 'missing-modal-component',
            template: "\n        <input type=\"text\" id=\"text\" autofocus />\n    "
        }), 
        __metadata('design:paramtypes', [])
    ], MissingModalComponent);
    return MissingModalComponent;
}());
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLnNwZWMuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi9zcmMvZGlyZWN0aXZlcy9hdXRvZm9jdXMuc3BlYy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7O0FBQUEscUJBQXFDLGVBQWUsQ0FBQyxDQUFBO0FBQ3JELHdCQUE0RSx1QkFBdUIsQ0FBQyxDQUFBO0FBRXBHLDhCQUFrRCxrQkFBa0IsQ0FBQyxDQUFBO0FBQ3JFLHVCQUFvQyxnQkFBZ0IsQ0FBQyxDQUFBO0FBRXJELFFBQVEsQ0FBQyxvQkFBb0IsRUFBRTtJQUUzQixJQUFJLE9BQThCLENBQUM7SUFFbkMsVUFBVSxDQUFDO1FBQ1AsaUJBQU8sQ0FBQyxzQkFBc0IsQ0FBQztZQUMzQixPQUFPLEVBQUUsQ0FBQyxpQ0FBaUIsQ0FBQztZQUM1QixZQUFZLEVBQUUsQ0FBQyxhQUFhLEVBQUUscUJBQXFCLENBQUM7U0FDdkQsQ0FBQyxDQUFDO0lBQ1AsQ0FBQyxDQUFDLENBQUM7SUFFSCxTQUFTLENBQUMsbUJBQVMsQ0FBQztRQUNoQixpQkFBTyxDQUFDLGtCQUFrQixFQUFFLENBQUM7UUFDN0IsY0FBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsc0JBQXNCO1FBQ2pDLGNBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLG1CQUFtQjtJQUNsQyxDQUFDLENBQUMsQ0FBQyxDQUFDO0lBRUosRUFBRSxDQUFDLHFEQUFxRCxFQUFFO1FBQ3RELElBQU0sT0FBTyxHQUFHLG1CQUFVLENBQUMscUJBQXFCLENBQUMsQ0FBQztJQUN0RCxDQUFDLENBQUMsQ0FBQztJQUVILEVBQUUsQ0FBQyxrREFBa0QsRUFBRSxtQkFBUyxDQUFDO1FBQzdELElBQU0sT0FBTyxHQUFHLG1CQUFVLENBQUMsYUFBYSxDQUFDLENBQUM7UUFDMUMsT0FBTyxDQUFDLGlCQUFpQixDQUFDLElBQUksRUFBRSxDQUFDO1FBQ2pDLGNBQUksRUFBRSxDQUFDO1FBQ1AsTUFBTSxDQUFDLFFBQVEsQ0FBQyxjQUFjLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLGFBQWEsQ0FBQyxDQUFDO0lBQ3pFLENBQUMsQ0FBQyxDQUFDLENBQUM7SUFFSixFQUFFLENBQUMsa0VBQWtFLEVBQUUsbUJBQVMsQ0FBQztRQUM3RSxJQUFNLE9BQU8sR0FBRyxtQkFBVSxDQUFDLGFBQWEsQ0FBQyxDQUFDO1FBQzFDLE9BQU8sQ0FBQyxpQkFBaUIsQ0FBQyxTQUFTLEdBQUcsSUFBSSxDQUFDO1FBQzNDLE9BQU8sQ0FBQyxhQUFhLEVBQUUsQ0FBQztRQUN4QixPQUFPLENBQUMsaUJBQWlCLENBQUMsSUFBSSxFQUFFLENBQUM7UUFDakMsY0FBSSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUMsc0JBQXNCO1FBQ2pDLGNBQUksQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLG1CQUFtQjtRQUM5QixNQUFNLENBQUMsUUFBUSxDQUFDLGNBQWMsQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsYUFBYSxDQUFDLENBQUM7SUFDekUsQ0FBQyxDQUFDLENBQUMsQ0FBQztBQUNSLENBQUMsQ0FBQyxDQUFDO0FBZ0JIO0lBQUE7UUFHSSxjQUFTLEdBQVksS0FBSyxDQUFDO0lBSy9CLENBQUM7SUFIRyw0QkFBSSxHQUFKO1FBQ0ksTUFBTSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxFQUFFLENBQUM7SUFDN0IsQ0FBQztJQU5EO1FBQUMsZ0JBQVMsQ0FBQyw4QkFBYyxDQUFDOztnREFBQTtJQWY5QjtRQUFDLGdCQUFTLENBQUM7WUFDUCxRQUFRLEVBQUUsZ0JBQWdCO1lBQzFCLFFBQVEsRUFBRSx1WkFVVDtTQUNKLENBQUM7O3FCQUFBO0lBU0Ysb0JBQUM7QUFBRCxDQUFDLEFBUkQsSUFRQztBQVFEO0lBQUE7SUFDQSxDQUFDO0lBUEQ7UUFBQyxnQkFBUyxDQUFDO1lBQ1AsUUFBUSxFQUFFLHlCQUF5QjtZQUNuQyxRQUFRLEVBQUUsK0RBRVQ7U0FDSixDQUFDOzs2QkFBQTtJQUVGLDRCQUFDO0FBQUQsQ0FBQyxBQURELElBQ0MifQ==