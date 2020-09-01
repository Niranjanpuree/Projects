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
var common_1 = require('@angular/common');
var testing_1 = require('@angular/core/testing');
var router_1 = require('@angular/router');
var testing_2 = require('@angular/router/testing');
var ng2_bs3_modal_1 = require('./ng2-bs3-modal');
var common_2 = require('./test/common');
describe('ModalComponent', function () {
    beforeEach(function () {
        jasmine.addMatchers(window['jasmine-jquery-matchers']);
    });
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            imports: [
                TestModule,
                testing_2.RouterTestingModule.withRoutes([
                    { path: '', component: TestComponent },
                    { path: 'test2', component: TestComponent2 }
                ])
            ]
        });
    });
    afterEach(testing_1.fakeAsync(function () {
        testing_1.TestBed.resetTestingModule();
        common_2.ticks(300, 150); // backdrop, modal transitions
    }));
    it('should instantiate component', function () {
        var fixture = testing_1.TestBed.createComponent(TestComponent);
        expect(fixture.componentInstance instanceof TestComponent).toBe(true, 'should create AppComponent');
    });
    it('should render', function () {
        var fixture = common_2.createRoot(TestComponent);
        expect(document.querySelectorAll('.modal').length).toBe(1);
    });
    it('should cleanup when destroyed', testing_1.fakeAsync(function () {
        var modal = common_2.createRoot(TestComponent).componentInstance.modal;
        modal.ngOnDestroy();
        testing_1.tick();
        expect(document.querySelectorAll('.modal').length).toBe(0);
    }));
    it('should emit onClose when modal is closed and animation is enabled', testing_1.fakeAsync(function () {
        var fixture = common_2.createRoot(TestComponent);
        var modal = fixture.componentInstance.modal;
        var spy = jasmine.createSpy('');
        fixture.componentInstance.animate = true;
        fixture.detectChanges();
        modal.onClose.subscribe(spy);
        modal.open();
        modal.close();
        common_2.ticks(150, 300, 300, 150); // backdrop, modal transitions
        expect(spy).toHaveBeenCalled();
    }));
    it('should emit onClose when modal is closed and animation is disabled', testing_1.fakeAsync(function () {
        var modal = common_2.createRoot(TestComponent).componentInstance.modal;
        var spy = jasmine.createSpy('');
        modal.onClose.subscribe(spy);
        modal.close();
        testing_1.tick();
        expect(spy).toHaveBeenCalled();
    }));
    it('should emit value passed to close when onClose emits', testing_1.fakeAsync(function () {
        var modal = common_2.createRoot(TestComponent).componentInstance.modal;
        var spy = jasmine.createSpy('').and.callFake(function (x) { return x; });
        var value = 'hello';
        modal.onClose.subscribe(spy);
        modal.close(value);
        testing_1.tick();
        expect(spy.calls.first().returnValue).toBe(value);
    }));
    it('should emit onDismiss when modal is dimissed and animation is disabled', testing_1.fakeAsync(function () {
        var modal = common_2.createRoot(TestComponent).componentInstance.modal;
        var spy = jasmine.createSpy('');
        modal.onDismiss.subscribe(spy);
        modal.open();
        modal.dismiss();
        testing_1.tick();
        expect(spy).toHaveBeenCalled();
    }));
    it('should emit onDismiss when modal is dismissed and animation is enabled', testing_1.fakeAsync(function () {
        var fixture = common_2.createRoot(TestComponent);
        var modal = fixture.componentInstance.modal;
        var spy = jasmine.createSpy('');
        fixture.componentInstance.animate = true;
        fixture.detectChanges();
        modal.onClose.subscribe(spy);
        modal.open();
        modal.close();
        common_2.ticks(150, 300, 300, 150); // backdrop, modal transitions
        expect(spy).toHaveBeenCalled();
    }));
    it('should emit onDismiss only once', testing_1.fakeAsync(function () {
        var fixture = common_2.createRoot(TestComponent);
        var modal = fixture.componentInstance.modal;
        var spy = jasmine.createSpy('');
        fixture.componentInstance.animate = true;
        fixture.detectChanges();
        modal.onClose.subscribe(spy);
        modal.open();
        modal.close();
        common_2.ticks(150, 300, 300, 150); // backdrop, modal transitions
        expect(spy).toHaveBeenCalledTimes(1);
    }));
    it('should emit onDismiss when modal is closed, opened, then dimissed from backdrop', testing_1.fakeAsync(function () {
        var fixture = common_2.createRoot(TestComponent);
        var modal = fixture.componentInstance.modal;
        var spy = jasmine.createSpy('');
        fixture.componentInstance.animate = true;
        fixture.detectChanges();
        modal.onDismiss.subscribe(spy);
        modal.open();
        modal.close();
        modal.open();
        document.querySelector('.modal').click();
        common_2.ticks(150, 300, 300, 150, 150, 300, 300, 150); // backdrop, modal transitions
        expect(spy).toHaveBeenCalled();
    }));
    it('should emit onDismiss when modal is dismissed a second time from backdrop', testing_1.fakeAsync(function () {
        var fixture = common_2.createRoot(TestComponent);
        var modal = fixture.componentInstance.modal;
        var spy = jasmine.createSpy('');
        fixture.componentInstance.animate = true;
        fixture.detectChanges();
        modal.onDismiss.subscribe(spy);
        modal.open();
        modal.dismiss();
        modal.open();
        document.querySelector('.modal').click();
        common_2.ticks(150, 300, 300, 150, 150, 300, 300, 150); // backdrop, modal transitions
        expect(spy).toHaveBeenCalledTimes(2);
    }));
    it('should emit onOpen when modal is opened and animations have been enabled', testing_1.fakeAsync(function () {
        var fixture = common_2.createRoot(TestComponent);
        var modal = fixture.componentInstance.modal;
        var spy = jasmine.createSpy('');
        fixture.componentInstance.animate = true;
        fixture.detectChanges();
        modal.onOpen.subscribe(spy);
        modal.open();
        common_2.ticks(150, 300); // backdrop, modal transitions
        expect(spy).toHaveBeenCalled();
    }));
    describe('Routing', function () {
        it('should not throw an error when navigating on modal close', testing_1.fakeAsync(testing_1.inject([router_1.Router], function (router) {
            // let zone = window['Zone']['ProxyZoneSpec'].assertPresent().getDelegate();
            var fixture = common_2.createRoot(RootComponent, router);
            var modal = fixture.componentInstance.glue.testComponent.modal;
            modal.onClose.subscribe(function () {
                router.navigateByUrl('/test2');
                common_2.advance(fixture);
                var content = fixture.debugElement.nativeElement.querySelector('test-component2');
                expect(content).toHaveText('hello');
            });
            modal.open();
            common_2.advance(fixture, 150); // backdrop transition
            common_2.advance(fixture, 300); // modal transition
            modal.close();
            common_2.advance(fixture, 300); // modal transition
            common_2.advance(fixture, 150); // backdrop transition
        })));
    });
});
var GlueService = (function () {
    function GlueService() {
    }
    return GlueService;
}());
var TestComponent = (function () {
    function TestComponent(glue) {
        this.animate = false;
        glue.testComponent = this;
    }
    __decorate([
        core_1.ViewChild(ng2_bs3_modal_1.ModalComponent), 
        __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
    ], TestComponent.prototype, "modal", void 0);
    TestComponent = __decorate([
        core_1.Component({
            selector: 'test-component',
            template: "\n        <button type=\"button\" class=\"btn btn-default\" (click)=\"modal.open()\" (onClose)=\"onClose()\">Open me!</button>\n\n        <modal #modal [animation]=\"animate\">\n            <modal-header [show-close]=\"true\">\n                <h4 class=\"modal-title\">I'm a modal!</h4>\n            </modal-header>\n            <modal-body>\n                Hello World!\n            </modal-body>\n            <modal-footer [show-default-buttons]=\"true\"></modal-footer>\n        </modal>\n    "
        }),
        __param(0, core_1.Inject(GlueService)), 
        __metadata('design:paramtypes', [GlueService])
    ], TestComponent);
    return TestComponent;
}());
var TestComponent2 = (function () {
    function TestComponent2() {
        this.message = 'hello';
    }
    TestComponent2 = __decorate([
        core_1.Component({
            selector: 'test-component2',
            template: "{{message}}",
        }), 
        __metadata('design:paramtypes', [])
    ], TestComponent2);
    return TestComponent2;
}());
var RootComponent = (function () {
    function RootComponent(glue) {
        this.glue = glue;
    }
    RootComponent = __decorate([
        core_1.Component({
            selector: 'app-component',
            template: "\n        <router-outlet></router-outlet>\n    "
        }),
        __param(0, core_1.Inject(GlueService)), 
        __metadata('design:paramtypes', [GlueService])
    ], RootComponent);
    return RootComponent;
}());
var TestModule = (function () {
    function TestModule() {
    }
    TestModule = __decorate([
        core_1.NgModule({
            imports: [testing_2.RouterTestingModule, ng2_bs3_modal_1.Ng2Bs3ModalModule, common_1.CommonModule],
            providers: [GlueService],
            declarations: [TestComponent, TestComponent2, RootComponent],
            exports: [TestComponent, TestComponent2, RootComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], TestModule);
    return TestModule;
}());
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmcyLWJzMy1tb2RhbC5zcGVjLmpzIiwic291cmNlUm9vdCI6IiIsInNvdXJjZXMiOlsic3JjL25nMi1iczMtbW9kYWwuc3BlYy50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7Ozs7O0FBQUEscUJBQW9FLGVBQWUsQ0FBQyxDQUFBO0FBQ3BGLHVCQUE2QixpQkFBaUIsQ0FBQyxDQUFBO0FBQy9DLHdCQUFtRSx1QkFBdUIsQ0FBQyxDQUFBO0FBQzNGLHVCQUF1QixpQkFBaUIsQ0FBQyxDQUFBO0FBQ3pDLHdCQUFvQyx5QkFBeUIsQ0FBQyxDQUFBO0FBRTlELDhCQUFrRCxpQkFBaUIsQ0FBQyxDQUFBO0FBQ3BFLHVCQUEyQyxlQUFlLENBQUMsQ0FBQTtBQUUzRCxRQUFRLENBQUMsZ0JBQWdCLEVBQUU7SUFFdkIsVUFBVSxDQUFDO1FBQ1AsT0FBTyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMseUJBQXlCLENBQUMsQ0FBQyxDQUFDO0lBQzNELENBQUMsQ0FBQyxDQUFDO0lBRUgsVUFBVSxDQUFDO1FBQ1AsaUJBQU8sQ0FBQyxzQkFBc0IsQ0FBQztZQUMzQixPQUFPLEVBQUU7Z0JBQ0wsVUFBVTtnQkFDViw2QkFBbUIsQ0FBQyxVQUFVLENBQUM7b0JBQzNCLEVBQUUsSUFBSSxFQUFFLEVBQUUsRUFBRSxTQUFTLEVBQUUsYUFBYSxFQUFFO29CQUN0QyxFQUFFLElBQUksRUFBRSxPQUFPLEVBQUUsU0FBUyxFQUFFLGNBQWMsRUFBRTtpQkFDL0MsQ0FBQzthQUNMO1NBQ0osQ0FBQyxDQUFDO0lBQ1AsQ0FBQyxDQUFDLENBQUM7SUFFSCxTQUFTLENBQUMsbUJBQVMsQ0FBQztRQUNoQixpQkFBTyxDQUFDLGtCQUFrQixFQUFFLENBQUM7UUFDN0IsY0FBSyxDQUFDLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQyxDQUFDLDhCQUE4QjtJQUNuRCxDQUFDLENBQUMsQ0FBQyxDQUFDO0lBRUosRUFBRSxDQUFDLDhCQUE4QixFQUFFO1FBQy9CLElBQUksT0FBTyxH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