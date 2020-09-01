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
System.register("components/modal-instance", ['rxjs/Observable', 'rxjs/add/operator/map', 'rxjs/add/observable/fromEvent'], function(exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var Observable_1;
    var ModalInstance, ModalResult;
    function booleanOrValue(value) {
        if (value === 'true')
            return true;
        else if (value === 'false')
            return false;
        return value;
    }
    function toPromise(observable) {
        return new Promise(function (resolve, reject) {
            observable.subscribe(function (next) {
                resolve(next);
            });
        });
    }
    return {
        setters:[
            function (Observable_1_1) {
                Observable_1 = Observable_1_1;
            },
            function (_1) {},
            function (_2) {}],
        execute: function() {
            ModalInstance = (function () {
                function ModalInstance(element) {
                    this.element = element;
                    this.suffix = '.ng2-bs3-modal';
                    this.shownEventName = 'shown.bs.modal' + this.suffix;
                    this.hiddenEventName = 'hidden.bs.modal' + this.suffix;
                    this.visible = false;
                    this.init();
                }
                ModalInstance.prototype.open = function () {
                    return this.show();
                };
                ModalInstance.prototype.close = function () {
                    this.result = ModalResult.Close;
                    return this.hide();
                };
                ModalInstance.prototype.dismiss = function () {
                    this.result = ModalResult.Dismiss;
                    return this.hide();
                };
                ModalInstance.prototype.destroy = function () {
                    var _this = this;
                    return this.hide().then(function () {
                        if (_this.$modal) {
                            _this.$modal.data('bs.modal', null);
                            _this.$modal.remove();
                        }
                    });
                };
                ModalInstance.prototype.show = function () {
                    var promise = toPromise(this.shown);
                    this.resetData();
                    this.$modal.modal();
                    return promise;
                };
                ModalInstance.prototype.hide = function () {
                    if (this.$modal && this.visible) {
                        var promise = toPromise(this.hidden);
                        this.$modal.modal('hide');
                        return promise;
                    }
                    return Promise.resolve(this.result);
                };
                ModalInstance.prototype.init = function () {
                    var _this = this;
                    this.$modal = jQuery(this.element.nativeElement);
                    this.$modal.appendTo('body');
                    this.shown = Observable_1.Observable.fromEvent(this.$modal, this.shownEventName)
                        .map(function () {
                        _this.visible = true;
                    });
                    this.hidden = Observable_1.Observable.fromEvent(this.$modal, this.hiddenEventName)
                        .map(function () {
                        var result = (!_this.result || _this.result === ModalResult.None)
                            ? ModalResult.Dismiss : _this.result;
                        _this.result = ModalResult.None;
                        _this.visible = false;
                        return result;
                    });
                };
                ModalInstance.prototype.resetData = function () {
                    this.$modal.removeData();
                    this.$modal.data('backdrop', booleanOrValue(this.$modal.attr('data-backdrop')));
                    this.$modal.data('keyboard', booleanOrValue(this.$modal.attr('data-keyboard')));
                };
                return ModalInstance;
            }());
            exports_1("ModalInstance", ModalInstance);
            (function (ModalResult) {
                ModalResult[ModalResult["None"] = 0] = "None";
                ModalResult[ModalResult["Close"] = 1] = "Close";
                ModalResult[ModalResult["Dismiss"] = 2] = "Dismiss";
            })(ModalResult || (ModalResult = {}));
            exports_1("ModalResult", ModalResult);
        }
    }
});
System.register("components/modal", ['@angular/core', "components/modal-instance"], function(exports_2, context_2) {
    "use strict";
    var __moduleName = context_2 && context_2.id;
    var core_1, modal_instance_1;
    var ModalComponent, ModalSize;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (modal_instance_1_1) {
                modal_instance_1 = modal_instance_1_1;
            }],
        execute: function() {
            ModalComponent = (function () {
                function ModalComponent(element) {
                    var _this = this;
                    this.element = element;
                    this.overrideSize = null;
                    this.visible = false;
                    this.animation = true;
                    this.backdrop = true;
                    this.keyboard = true;
                    this.cssClass = '';
                    this.onClose = new core_1.EventEmitter(false);
                    this.onDismiss = new core_1.EventEmitter(false);
                    this.onOpen = new core_1.EventEmitter(false);
                    this.instance = new modal_instance_1.ModalInstance(this.element);
                    this.instance.hidden.subscribe(function (result) {
                        _this.visible = _this.instance.visible;
                        if (result === modal_instance_1.ModalResult.Dismiss) {
                            _this.onDismiss.emit(undefined);
                        }
                    });
                    this.instance.shown.subscribe(function () {
                        _this.onOpen.emit(undefined);
                    });
                }
                Object.defineProperty(ModalComponent.prototype, "fadeClass", {
                    get: function () {
                        return this.animation;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(ModalComponent.prototype, "dataKeyboardAttr", {
                    get: function () {
                        return this.keyboard;
                    },
                    enumerable: true,
                    configurable: true
                });
                Object.defineProperty(ModalComponent.prototype, "dataBackdropAttr", {
                    get: function () {
                        return this.backdrop;
                    },
                    enumerable: true,
                    configurable: true
                });
                ModalComponent.prototype.ngOnDestroy = function () {
                    return this.instance && this.instance.destroy();
                };
                ModalComponent.prototype.routerCanDeactivate = function () {
                    return this.ngOnDestroy();
                };
                ModalComponent.prototype.open = function (size) {
                    var _this = this;
                    if (ModalSize.validSize(size))
                        this.overrideSize = size;
                    return this.instance.open().then(function () {
                        _this.visible = _this.instance.visible;
                    });
                };
                ModalComponent.prototype.close = function (value) {
                    var _this = this;
                    return this.instance.close().then(function () {
                        _this.onClose.emit(value);
                    });
                };
                ModalComponent.prototype.dismiss = function () {
                    return this.instance.dismiss();
                };
                ModalComponent.prototype.getCssClasses = function () {
                    var classes = [];
                    if (this.isSmall()) {
                        classes.push('modal-sm');
                    }
                    if (this.isLarge()) {
                        classes.push('modal-lg');
                    }
                    if (this.cssClass !== '') {
                        classes.push(this.cssClass);
                    }
                    return classes.join(' ');
                };
                ModalComponent.prototype.isSmall = function () {
                    return this.overrideSize !== ModalSize.Large
                        && this.size === ModalSize.Small
                        || this.overrideSize === ModalSize.Small;
                };
                ModalComponent.prototype.isLarge = function () {
                    return this.overrideSize !== ModalSize.Small
                        && this.size === ModalSize.Large
                        || this.overrideSize === ModalSize.Large;
                };
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Boolean)
                ], ModalComponent.prototype, "animation", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Object)
                ], ModalComponent.prototype, "backdrop", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', Boolean)
                ], ModalComponent.prototype, "keyboard", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', String)
                ], ModalComponent.prototype, "size", void 0);
                __decorate([
                    core_1.Input(), 
                    __metadata('design:type', String)
                ], ModalComponent.prototype, "cssClass", void 0);
                __decorate([
                    core_1.Output(), 
                    __metadata('design:type', core_1.EventEmitter)
                ], ModalComponent.prototype, "onClose", void 0);
                __decorate([
                    core_1.Output(), 
                    __metadata('design:type', core_1.EventEmitter)
                ], ModalComponent.prototype, "onDismiss", void 0);
                __decorate([
                    core_1.Output(), 
                    __metadata('design:type', core_1.EventEmitter)
                ], ModalComponent.prototype, "onOpen", void 0);
                __decorate([
                    core_1.HostBinding('class.fade'), 
                    __metadata('design:type', Boolean)
                ], ModalComponent.prototype, "fadeClass", null);
                __decorate([
                    core_1.HostBinding('attr.data-keyboard'), 
                    __metadata('design:type', Boolean)
                ], ModalComponent.prototype, "dataKeyboardAttr", null);
                __decorate([
                    core_1.HostBinding('attr.data-backdrop'), 
                    __metadata('design:type', Object)
                ], ModalComponent.prototype, "dataBackdropAttr", null);
                ModalComponent = __decorate([
                    core_1.Component({
                        selector: 'modal',
                        host: {
                            'class': 'modal',
                            'role': 'dialog',
                            'tabindex': '-1'
                        },
                        template: "\n        <div class=\"modal-dialog\" [ngClass]=\"getCssClasses()\">\n            <div class=\"modal-content\">\n                <ng-content></ng-content>\n            </div>\n        </div>\n    "
                    }), 
                    __metadata('design:paramtypes', [core_1.ElementRef])
                ], ModalComponent);
                return ModalComponent;
            }());
            exports_2("ModalComponent", ModalComponent);
            ModalSize = (function () {
                function ModalSize() {
                }
                ModalSize.validSize = function (size) {
                    return size && (size === ModalSize.Small || size === ModalSize.Large);
                };
                ModalSize.Small = 'sm';
                ModalSize.Large = 'lg';
                return ModalSize;
            }());
            exports_2("ModalSize", ModalSize);
        }
    }
});
System.register("components/modal-body", ['@angular/core'], function(exports_3, context_3) {
    "use strict";
    var __moduleName = context_3 && context_3.id;
    var core_2;
    var ModalBodyComponent;
    return {
        setters:[
            function (core_2_1) {
                core_2 = core_2_1;
            }],
        execute: function() {
            ModalBodyComponent = (function () {
                function ModalBodyComponent() {
                }
                ModalBodyComponent = __decorate([
                    core_2.Component({
                        selector: 'modal-body',
                        template: "\n        <div class=\"modal-body\">\n            <ng-content></ng-content>\n        </div>\n    "
                    }), 
                    __metadata('design:paramtypes', [])
                ], ModalBodyComponent);
                return ModalBodyComponent;
            }());
            exports_3("ModalBodyComponent", ModalBodyComponent);
        }
    }
});
System.register("components/modal-footer", ['@angular/core', "components/modal"], function(exports_4, context_4) {
    "use strict";
    var __moduleName = context_4 && context_4.id;
    var core_3, modal_1;
    var ModalFooterComponent;
    return {
        setters:[
            function (core_3_1) {
                core_3 = core_3_1;
            },
            function (modal_1_1) {
                modal_1 = modal_1_1;
            }],
        execute: function() {
            ModalFooterComponent = (function () {
                function ModalFooterComponent(modal) {
                    this.modal = modal;
                    this.showDefaultButtons = false;
                    this.dismissButtonLabel = 'Dismiss';
                    this.closeButtonLabel = 'Close';
                }
                __decorate([
                    core_3.Input('show-default-buttons'), 
                    __metadata('design:type', Boolean)
                ], ModalFooterComponent.prototype, "showDefaultButtons", void 0);
                __decorate([
                    core_3.Input('dismiss-button-label'), 
                    __metadata('design:type', String)
                ], ModalFooterComponent.prototype, "dismissButtonLabel", void 0);
                __decorate([
                    core_3.Input('close-button-label'), 
                    __metadata('design:type', String)
                ], ModalFooterComponent.prototype, "closeButtonLabel", void 0);
                ModalFooterComponent = __decorate([
                    core_3.Component({
                        selector: 'modal-footer',
                        template: "\n        <div class=\"modal-footer\">\n            <ng-content></ng-content>\n            <button *ngIf=\"showDefaultButtons\" type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\" (click)=\"modal.dismiss()\">{{dismissButtonLabel}}</button>\n            <button *ngIf=\"showDefaultButtons\" type=\"button\" class=\"btn btn-primary\" (click)=\"modal.close()\">{{closeButtonLabel}}</button>\n        </div>\n    "
                    }), 
                    __metadata('design:paramtypes', [modal_1.ModalComponent])
                ], ModalFooterComponent);
                return ModalFooterComponent;
            }());
            exports_4("ModalFooterComponent", ModalFooterComponent);
        }
    }
});
System.register("components/modal-header", ['@angular/core', "components/modal"], function(exports_5, context_5) {
    "use strict";
    var __moduleName = context_5 && context_5.id;
    var core_4, modal_2;
    var ModalHeaderComponent;
    return {
        setters:[
            function (core_4_1) {
                core_4 = core_4_1;
            },
            function (modal_2_1) {
                modal_2 = modal_2_1;
            }],
        execute: function() {
            ModalHeaderComponent = (function () {
                function ModalHeaderComponent(modal) {
                    this.modal = modal;
                    this.showClose = false;
                }
                __decorate([
                    core_4.Input('show-close'), 
                    __metadata('design:type', Boolean)
                ], ModalHeaderComponent.prototype, "showClose", void 0);
                ModalHeaderComponent = __decorate([
                    core_4.Component({
                        selector: 'modal-header',
                        template: "\n        <div class=\"modal-header\">\n            <button *ngIf=\"showClose\" type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n                <span aria-hidden=\"true\">&times;</span>\n            </button>\n            <ng-content></ng-content>\n        </div>\n    "
                    }), 
                    __metadata('design:paramtypes', [modal_2.ModalComponent])
                ], ModalHeaderComponent);
                return ModalHeaderComponent;
            }());
            exports_5("ModalHeaderComponent", ModalHeaderComponent);
        }
    }
});
System.register("directives/autofocus", ['@angular/core', "components/modal"], function(exports_6, context_6) {
    "use strict";
    var __moduleName = context_6 && context_6.id;
    var core_5, modal_3;
    var AutofocusDirective;
    return {
        setters:[
            function (core_5_1) {
                core_5 = core_5_1;
            },
            function (modal_3_1) {
                modal_3 = modal_3_1;
            }],
        execute: function() {
            AutofocusDirective = (function () {
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
                    core_5.Directive({
                        selector: '[autofocus]'
                    }),
                    __param(1, core_5.Optional()), 
                    __metadata('design:paramtypes', [core_5.ElementRef, modal_3.ModalComponent])
                ], AutofocusDirective);
                return AutofocusDirective;
            }());
            exports_6("AutofocusDirective", AutofocusDirective);
        }
    }
});
System.register("ng2-bs3-modal", ['@angular/core', '@angular/common', "components/modal", "components/modal-header", "components/modal-body", "components/modal-footer", "directives/autofocus", "components/modal-instance"], function(exports_7, context_7) {
    "use strict";
    var __moduleName = context_7 && context_7.id;
    var core_6, common_1, modal_4, modal_header_1, modal_body_1, modal_footer_1, autofocus_1;
    var Ng2Bs3ModalModule;
    var exportedNames_1 = {
        'Ng2Bs3ModalModule': true
    };
    function exportStar_1(m) {
        var exports = {};
        for(var n in m) {
            if (n !== "default"&& !exportedNames_1.hasOwnProperty(n)) exports[n] = m[n];
        }
        exports_7(exports);
    }
    return {
        setters:[
            function (core_6_1) {
                core_6 = core_6_1;
            },
            function (common_1_1) {
                common_1 = common_1_1;
            },
            function (modal_4_1) {
                modal_4 = modal_4_1;
                exportStar_1(modal_4_1);
            },
            function (modal_header_1_1) {
                modal_header_1 = modal_header_1_1;
                exportStar_1(modal_header_1_1);
            },
            function (modal_body_1_1) {
                modal_body_1 = modal_body_1_1;
                exportStar_1(modal_body_1_1);
            },
            function (modal_footer_1_1) {
                modal_footer_1 = modal_footer_1_1;
                exportStar_1(modal_footer_1_1);
            },
            function (autofocus_1_1) {
                autofocus_1 = autofocus_1_1;
            },
            function (modal_instance_2_1) {
                exportStar_1(modal_instance_2_1);
            }],
        execute: function() {
            Ng2Bs3ModalModule = (function () {
                function Ng2Bs3ModalModule() {
                }
                Ng2Bs3ModalModule = __decorate([
                    core_6.NgModule({
                        imports: [
                            common_1.CommonModule
                        ],
                        declarations: [
                            modal_4.ModalComponent,
                            modal_header_1.ModalHeaderComponent,
                            modal_body_1.ModalBodyComponent,
                            modal_footer_1.ModalFooterComponent,
                            autofocus_1.AutofocusDirective
                        ],
                        exports: [
                            modal_4.ModalComponent,
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
            exports_7("Ng2Bs3ModalModule", Ng2Bs3ModalModule);
        }
    }
});
System.register("test/common", ['@angular/core/testing'], function(exports_8, context_8) {
    "use strict";
    var __moduleName = context_8 && context_8.id;
    var testing_1;
    function createRoot(type, router) {
        var f = testing_1.TestBed.createComponent(type);
        f.detectChanges();
        if (router) {
            router.initialNavigation();
            advance(f);
        }
        return f;
    }
    exports_8("createRoot", createRoot);
    function advance(fixture, millis) {
        testing_1.tick(millis);
        fixture.detectChanges();
    }
    exports_8("advance", advance);
    function ticks() {
        var millises = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            millises[_i - 0] = arguments[_i];
        }
        millises.forEach(function (m) { return testing_1.tick(m); });
    }
    exports_8("ticks", ticks);
    return {
        setters:[
            function (testing_1_1) {
                testing_1 = testing_1_1;
            }],
        execute: function() {
        }
    }
});
System.register("directives/autofocus.spec", ['@angular/core', '@angular/core/testing', "ng2-bs3-modal", "test/common"], function(exports_9, context_9) {
    "use strict";
    var __moduleName = context_9 && context_9.id;
    var core_7, testing_2, ng2_bs3_modal_1, common_2;
    var TestComponent, MissingModalComponent;
    return {
        setters:[
            function (core_7_1) {
                core_7 = core_7_1;
            },
            function (testing_2_1) {
                testing_2 = testing_2_1;
            },
            function (ng2_bs3_modal_1_1) {
                ng2_bs3_modal_1 = ng2_bs3_modal_1_1;
            },
            function (common_2_1) {
                common_2 = common_2_1;
            }],
        execute: function() {
            describe('AutofocusDirective', function () {
                var fixture;
                beforeEach(function () {
                    testing_2.TestBed.configureTestingModule({
                        imports: [ng2_bs3_modal_1.Ng2Bs3ModalModule],
                        declarations: [TestComponent, MissingModalComponent]
                    });
                });
                afterEach(testing_2.fakeAsync(function () {
                    testing_2.TestBed.resetTestingModule();
                    testing_2.tick(300); // backdrop transition
                    testing_2.tick(150); // modal transition
                }));
                it('should not throw an error if a modal isn\'t present', function () {
                    var fixture = common_2.createRoot(MissingModalComponent);
                });
                it('should autofocus on element when modal is opened', testing_2.fakeAsync(function () {
                    var fixture = common_2.createRoot(TestComponent);
                    fixture.componentInstance.open();
                    testing_2.tick();
                    expect(document.getElementById('text')).toBe(document.activeElement);
                }));
                it('should autofocus on element when modal is opened with animations', testing_2.fakeAsync(function () {
                    var fixture = common_2.createRoot(TestComponent);
                    fixture.componentInstance.animation = true;
                    fixture.detectChanges();
                    fixture.componentInstance.open();
                    testing_2.tick(150); // backdrop transition
                    testing_2.tick(300); // modal transition
                    expect(document.getElementById('text')).toBe(document.activeElement);
                }));
            });
            TestComponent = (function () {
                function TestComponent() {
                    this.animation = false;
                }
                TestComponent.prototype.open = function () {
                    return this.modal.open();
                };
                __decorate([
                    core_7.ViewChild(ng2_bs3_modal_1.ModalComponent), 
                    __metadata('design:type', ng2_bs3_modal_1.ModalComponent)
                ], TestComponent.prototype, "modal", void 0);
                TestComponent = __decorate([
                    core_7.Component({
                        selector: 'test-component',
                        template: "\n        <modal #modal [animation]=\"animation\">\n            <modal-header [show-close]=\"true\">\n                <h4 class=\"modal-title\">I'm a modal!</h4>\n            </modal-header>\n            <modal-body>\n                <input type=\"text\" id=\"text\" autofocus />\n            </modal-body>\n            <modal-footer [show-default-buttons]=\"true\"></modal-footer>\n        </modal>\n    "
                    }), 
                    __metadata('design:paramtypes', [])
                ], TestComponent);
                return TestComponent;
            }());
            MissingModalComponent = (function () {
                function MissingModalComponent() {
                }
                MissingModalComponent = __decorate([
                    core_7.Component({
                        selector: 'missing-modal-component',
                        template: "\n        <input type=\"text\" id=\"text\" autofocus />\n    "
                    }), 
                    __metadata('design:paramtypes', [])
                ], MissingModalComponent);
                return MissingModalComponent;
            }());
        }
    }
});
System.register("ng2-bs3-modal.spec", ['@angular/core', '@angular/common', '@angular/core/testing', '@angular/router', '@angular/router/testing', "ng2-bs3-modal", "test/common"], function(exports_10, context_10) {
    "use strict";
    var __moduleName = context_10 && context_10.id;
    var core_8, common_3, testing_3, router_1, testing_4, ng2_bs3_modal_2, common_4;
    var GlueService, TestComponent, TestComponent2, RootComponent, TestModule;
    return {
        setters:[
            function (core_8_1) {
                core_8 = core_8_1;
            },
            function (common_3_1) {
                common_3 = common_3_1;
            },
            function (testing_3_1) {
                testing_3 = testing_3_1;
            },
            function (router_1_1) {
                router_1 = router_1_1;
            },
            function (testing_4_1) {
                testing_4 = testing_4_1;
            },
            function (ng2_bs3_modal_2_1) {
                ng2_bs3_modal_2 = ng2_bs3_modal_2_1;
            },
            function (common_4_1) {
                common_4 = common_4_1;
            }],
        execute: function() {
            describe('ModalComponent', function () {
                beforeEach(function () {
                    jasmine.addMatchers(window['jasmine-jquery-matchers']);
                });
                beforeEach(function () {
                    testing_3.TestBed.configureTestingModule({
                        imports: [
                            TestModule,
                            testing_4.RouterTestingModule.withRoutes([
                                { path: '', component: TestComponent },
                                { path: 'test2', component: TestComponent2 }
                            ])
                        ]
                    });
                });
                afterEach(testing_3.fakeAsync(function () {
                    testing_3.TestBed.resetTestingModule();
                    common_4.ticks(300, 150); // backdrop, modal transitions
                }));
                it('should instantiate component', function () {
                    var fixture = testing_3.TestBed.createComponent(TestComponent);
                    expect(fixture.componentInstance instanceof TestComponent).toBe(true, 'should create AppComponent');
                });
                it('should render', function () {
                    var fixture = common_4.createRoot(TestComponent);
                    expect(document.querySelectorAll('.modal').length).toBe(1);
                });
                it('should cleanup when destroyed', testing_3.fakeAsync(function () {
                    var modal = common_4.createRoot(TestComponent).componentInstance.modal;
                    modal.ngOnDestroy();
                    testing_3.tick();
                    expect(document.querySelectorAll('.modal').length).toBe(0);
                }));
                it('should emit onClose when modal is closed and animation is enabled', testing_3.fakeAsync(function () {
                    var fixture = common_4.createRoot(TestComponent);
                    var modal = fixture.componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    fixture.componentInstance.animate = true;
                    fixture.detectChanges();
                    modal.onClose.subscribe(spy);
                    modal.open();
                    modal.close();
                    common_4.ticks(150, 300, 300, 150); // backdrop, modal transitions
                    expect(spy).toHaveBeenCalled();
                }));
                it('should emit onClose when modal is closed and animation is disabled', testing_3.fakeAsync(function () {
                    var modal = common_4.createRoot(TestComponent).componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    modal.onClose.subscribe(spy);
                    modal.close();
                    testing_3.tick();
                    expect(spy).toHaveBeenCalled();
                }));
                it('should emit value passed to close when onClose emits', testing_3.fakeAsync(function () {
                    var modal = common_4.createRoot(TestComponent).componentInstance.modal;
                    var spy = jasmine.createSpy('').and.callFake(function (x) { return x; });
                    var value = 'hello';
                    modal.onClose.subscribe(spy);
                    modal.close(value);
                    testing_3.tick();
                    expect(spy.calls.first().returnValue).toBe(value);
                }));
                it('should emit onDismiss when modal is dimissed and animation is disabled', testing_3.fakeAsync(function () {
                    var modal = common_4.createRoot(TestComponent).componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    modal.onDismiss.subscribe(spy);
                    modal.open();
                    modal.dismiss();
                    testing_3.tick();
                    expect(spy).toHaveBeenCalled();
                }));
                it('should emit onDismiss when modal is dismissed and animation is enabled', testing_3.fakeAsync(function () {
                    var fixture = common_4.createRoot(TestComponent);
                    var modal = fixture.componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    fixture.componentInstance.animate = true;
                    fixture.detectChanges();
                    modal.onClose.subscribe(spy);
                    modal.open();
                    modal.close();
                    common_4.ticks(150, 300, 300, 150); // backdrop, modal transitions
                    expect(spy).toHaveBeenCalled();
                }));
                it('should emit onDismiss only once', testing_3.fakeAsync(function () {
                    var fixture = common_4.createRoot(TestComponent);
                    var modal = fixture.componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    fixture.componentInstance.animate = true;
                    fixture.detectChanges();
                    modal.onClose.subscribe(spy);
                    modal.open();
                    modal.close();
                    common_4.ticks(150, 300, 300, 150); // backdrop, modal transitions
                    expect(spy).toHaveBeenCalledTimes(1);
                }));
                it('should emit onDismiss when modal is closed, opened, then dimissed from backdrop', testing_3.fakeAsync(function () {
                    var fixture = common_4.createRoot(TestComponent);
                    var modal = fixture.componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    fixture.componentInstance.animate = true;
                    fixture.detectChanges();
                    modal.onDismiss.subscribe(spy);
                    modal.open();
                    modal.close();
                    modal.open();
                    document.querySelector('.modal').click();
                    common_4.ticks(150, 300, 300, 150, 150, 300, 300, 150); // backdrop, modal transitions
                    expect(spy).toHaveBeenCalled();
                }));
                it('should emit onDismiss when modal is dismissed a second time from backdrop', testing_3.fakeAsync(function () {
                    var fixture = common_4.createRoot(TestComponent);
                    var modal = fixture.componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    fixture.componentInstance.animate = true;
                    fixture.detectChanges();
                    modal.onDismiss.subscribe(spy);
                    modal.open();
                    modal.dismiss();
                    modal.open();
                    document.querySelector('.modal').click();
                    common_4.ticks(150, 300, 300, 150, 150, 300, 300, 150); // backdrop, modal transitions
                    expect(spy).toHaveBeenCalledTimes(2);
                }));
                it('should emit onOpen when modal is opened and animations have been enabled', testing_3.fakeAsync(function () {
                    var fixture = common_4.createRoot(TestComponent);
                    var modal = fixture.componentInstance.modal;
                    var spy = jasmine.createSpy('');
                    fixture.componentInstance.animate = true;
                    fixture.detectChanges();
                    modal.onOpen.subscribe(spy);
                    modal.open();
                    common_4.ticks(150, 300); // backdrop, modal transitions
                    expect(spy).toHaveBeenCalled();
                }));
                describe('Routing', function () {
                    it('should not throw an error when navigating on modal close', testing_3.fakeAsync(testing_3.inject([router_1.Router], function (router) {
                        // let zone = window['Zone']['ProxyZoneSpec'].assertPresent().getDelegate();
                        var fixture = common_4.createRoot(RootComponent, router);
                        var modal = fixture.componentInstance.glue.testComponent.modal;
                        modal.onClose.subscribe(function () {
                            router.navigateByUrl('/test2');
                            common_4.advance(fixture);
                            var content = fixture.debugElement.nativeElement.querySelector('test-component2');
                            expect(content).toHaveText('hello');
                        });
                        modal.open();
                        common_4.advance(fixture, 150); // backdrop transition
                        common_4.advance(fixture, 300); // modal transition
                        modal.close();
                        common_4.advance(fixture, 300); // modal transition
                        common_4.advance(fixture, 150); // backdrop transition
                    })));
                });
            });
            GlueService = (function () {
                function GlueService() {
                }
                return GlueService;
            }());
            TestComponent = (function () {
                function TestComponent(glue) {
                    this.animate = false;
                    glue.testComponent = this;
                }
                __decorate([
                    core_8.ViewChild(ng2_bs3_modal_2.ModalComponent), 
                    __metadata('design:type', ng2_bs3_modal_2.ModalComponent)
                ], TestComponent.prototype, "modal", void 0);
                TestComponent = __decorate([
                    core_8.Component({
                        selector: 'test-component',
                        template: "\n        <button type=\"button\" class=\"btn btn-default\" (click)=\"modal.open()\" (onClose)=\"onClose()\">Open me!</button>\n\n        <modal #modal [animation]=\"animate\">\n            <modal-header [show-close]=\"true\">\n                <h4 class=\"modal-title\">I'm a modal!</h4>\n            </modal-header>\n            <modal-body>\n                Hello World!\n            </modal-body>\n            <modal-footer [show-default-buttons]=\"true\"></modal-footer>\n        </modal>\n    "
                    }),
                    __param(0, core_8.Inject(GlueService)), 
                    __metadata('design:paramtypes', [GlueService])
                ], TestComponent);
                return TestComponent;
            }());
            TestComponent2 = (function () {
                function TestComponent2() {
                    this.message = 'hello';
                }
                TestComponent2 = __decorate([
                    core_8.Component({
                        selector: 'test-component2',
                        template: "{{message}}",
                    }), 
                    __metadata('design:paramtypes', [])
                ], TestComponent2);
                return TestComponent2;
            }());
            RootComponent = (function () {
                function RootComponent(glue) {
                    this.glue = glue;
                }
                RootComponent = __decorate([
                    core_8.Component({
                        selector: 'app-component',
                        template: "\n        <router-outlet></router-outlet>\n    "
                    }),
                    __param(0, core_8.Inject(GlueService)), 
                    __metadata('design:paramtypes', [GlueService])
                ], RootComponent);
                return RootComponent;
            }());
            TestModule = (function () {
                function TestModule() {
                }
                TestModule = __decorate([
                    core_8.NgModule({
                        imports: [testing_4.RouterTestingModule, ng2_bs3_modal_2.Ng2Bs3ModalModule, common_3.CommonModule],
                        providers: [GlueService],
                        declarations: [TestComponent, TestComponent2, RootComponent],
                        exports: [TestComponent, TestComponent2, RootComponent]
                    }), 
                    __metadata('design:paramtypes', [])
                ], TestModule);
                return TestModule;
            }());
        }
    }
});
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmcyLWJzMy1tb2RhbC5qcyIsInNvdXJjZVJvb3QiOiIiLCJzb3VyY2VzIjpbIi4uL3NyYy9jb21wb25lbnRzL21vZGFsLWluc3RhbmNlLnRzIiwiLi4vc3JjL2NvbXBvbmVudHMvbW9kYWwudHMiLCIuLi9zcmMvY29tcG9uZW50cy9tb2RhbC1ib2R5LnRzIiwiLi4vc3JjL2NvbXBvbmVudHMvbW9kYWwtZm9vdGVyLnRzIiwiLi4vc3JjL2NvbXBvbmVudHMvbW9kYWwtaGVhZGVyLnRzIiwiLi4vc3JjL2RpcmVjdGl2ZXMvYXV0b2ZvY3VzLnRzIiwiLi4vc3JjL25nMi1iczMtbW9kYWwudHMiLCIuLi9zcmMvdGVzdC9jb21tb24udHMiLCIuLi9zcmMvZGlyZWN0aXZlcy9hdXRvZm9jdXMuc3BlYy50cyIsIi4uL3NyYy9uZzItYnMzLW1vZGFsLnNwZWMudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7Ozs7Ozs7Ozs7Ozs7SUEwRkEsd0JBQXdCLEtBQUs7UUFDekIsRUFBRSxDQUFDLENBQUMsS0FBSyxLQUFLLE1BQU0sQ0FBQztZQUNqQixNQUFNLENBQUMsSUFBSSxDQUFDO1FBQ2hCLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxLQUFLLEtBQUssT0FBTyxDQUFDO1lBQ3ZCLE1BQU0sQ0FBQyxLQUFLLENBQUM7UUFDakIsTUFBTSxDQUFDLEtBQUssQ0FBQztJQUNqQixDQUFDO0lBRUQsbUJBQXNCLFVBQXlCO1FBQzNDLE1BQU0sQ0FBQyxJQUFJLE9BQU8sQ0FBQyxVQUFDLE9BQU8sRUFBRSxNQUFNO1lBQy9CLFVBQVUsQ0FBQyxTQUFTLENBQUMsVUFBQSxJQUFJO2dCQUNyQixPQUFPLENBQUMsSUFBSSxDQUFDLENBQUM7WUFDbEIsQ0FBQyxDQUFDLENBQUM7UUFDUCxDQUFDLENBQUMsQ0FBQztJQUNQLENBQUM7Ozs7Ozs7OztZQWpHRDtnQkFZSSx1QkFBb0IsT0FBbUI7b0JBQW5CLFlBQU8sR0FBUCxPQUFPLENBQVk7b0JBVi9CLFdBQU0sR0FBVyxnQkFB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