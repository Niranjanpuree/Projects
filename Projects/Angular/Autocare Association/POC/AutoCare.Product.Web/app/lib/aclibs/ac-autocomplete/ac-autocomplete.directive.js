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
var ac_autocomplete_component_1 = require("./ac-autocomplete.component");
/**
 * display auto-complete section with input and dropdown list when it is clicked
 */
var AutoCompleteDirective = (function () {
    function AutoCompleteDirective(resolver, viewContainerRef) {
        var _this = this;
        this.resolver = resolver;
        this.viewContainerRef = viewContainerRef;
        this.minChars = 0;
        this.keywordChanged = new core_1.EventEmitter();
        this.popupBottom = 0;
        this.keyword = '';
        //show auto-complete list below the current element
        this.showAutoComplete = function () {
            var factory = _this.resolver.resolveComponentFactory(ac_autocomplete_component_1.AutoCompleteComponent);
            _this.componentRef = _this.viewContainerRef.createComponent(factory);
            _this.acEl = _this.componentRef.location.nativeElement;
            _this.autoCompleteComp = _this.componentRef.instance;
            _this.autoCompleteComp.listFormatter = _this.listFormatter;
            //component.prefillFunc = this.prefillFunc;
            _this.autoCompleteComp.pathToData = _this.pathToData;
            _this.autoCompleteComp.minChars = _this.minChars;
            _this.autoCompleteComp.valuePropertyName = _this.valuePropertyName || 'id';
            _this.autoCompleteComp.displayPropertyName = _this.displayPropertyName || 'value';
            _this.autoCompleteComp.source = _this.source;
            _this.autoCompleteComp.sourceAction = _this.sourceAction;
            _this.autoCompleteComp.placeholder = _this.placeholder;
            _this.autoCompleteComp.valueSelectedEvent.subscribe(function (val) {
                if (typeof val === 'undefined') {
                    _this.keywordChanged.emit(_this.el.value);
                }
                else if (typeof val === "string") {
                    _this.keywordChanged.emit(val);
                }
                else {
                    var displayVal = val[_this.autoCompleteComp.displayPropertyName];
                    _this.el.value = displayVal;
                    _this.keywordChanged.emit(displayVal);
                }
                if (_this.valueChanged) {
                    _this.valueChanged(val);
                }
            });
            setTimeout(function () {
                /* setting width/height auto complete */
                var thisElBCR = _this.el.getBoundingClientRect();
                var offsetHeight = _this.el.offsetHeight;
                _this.acEl.style.width = thisElBCR.width + 'px';
                //this.acEl.style.height = "38px";
                _this.acEl.style.position = 'absolute';
                _this.acEl.style.zIndex = '1';
                _this.acEl.style.top = _this.el.offsetHeight + _this.el.offsetTop + 2 + 'px';
                _this.acEl.style.left = _this.el.offsetLeft + 'px';
            });
            document.addEventListener('click', function (event) {
                if (event.target !== _this.el && event.target !== _this.acEl) {
                    _this.autoCompleteComp.hideDropdownList();
                }
            });
        };
        this.el = this.viewContainerRef.element.nativeElement;
    }
    AutoCompleteDirective.prototype.ngOnInit = function () {
        if (this.el.parentElement !== this.autoCompleteDiv) {
            this.autoCompleteDiv = document.createElement("div");
            this.autoCompleteDiv.className = 'ng2-auto-complete';
            this.autoCompleteDiv.style.display = 'inline-block';
            this.autoCompleteDiv.style.position = 'relative';
            this.el.parentElement.insertBefore(this.autoCompleteDiv, this.el.nextSibling);
            this.autoCompleteDiv.appendChild(this.el);
        }
        this.showAutoComplete();
    };
    AutoCompleteDirective.prototype.ngOnDestroy = function () {
        this.hideAndDestroyAutoComplete();
    };
    AutoCompleteDirective.prototype.inputEventHandler = function ($event) {
        var inputElement = $event.target;
        if (this.autoCompleteComp) {
            this.autoCompleteComp.reloadListInDelay(inputElement.value);
        }
    };
    AutoCompleteDirective.prototype.keydownEventHandler = function ($event) {
        if (this.autoCompleteComp) {
            this.autoCompleteComp.inputElKeyHandler($event);
        }
    };
    AutoCompleteDirective.prototype.hideAndDestroyAutoComplete = function () {
        if (this.el.parentElement === this.autoCompleteDiv) {
            var autoCompleteDivPatent = this.el.parentElement.parentElement;
            autoCompleteDivPatent.insertBefore(this.el, this.autoCompleteDiv);
            autoCompleteDivPatent.removeChild(this.autoCompleteDiv);
        }
        if (this.componentRef) {
            this.componentRef.destroy();
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], AutoCompleteDirective.prototype, "placeholder", void 0);
    __decorate([
        core_1.Input('list-formatter'), 
        __metadata('design:type', Function)
    ], AutoCompleteDirective.prototype, "listFormatter", void 0);
    __decorate([
        core_1.Input('value-changed'), 
        __metadata('design:type', Function)
    ], AutoCompleteDirective.prototype, "valueChanged", void 0);
    __decorate([
        core_1.Input('source'), 
        __metadata('design:type', Object)
    ], AutoCompleteDirective.prototype, "source", void 0);
    __decorate([
        core_1.Input('min-chars'), 
        __metadata('design:type', Number)
    ], AutoCompleteDirective.prototype, "minChars", void 0);
    __decorate([
        core_1.Input('path-to-data'), 
        __metadata('design:type', String)
    ], AutoCompleteDirective.prototype, "pathToData", void 0);
    __decorate([
        core_1.Input('value-property-name'), 
        __metadata('design:type', String)
    ], AutoCompleteDirective.prototype, "valuePropertyName", void 0);
    __decorate([
        core_1.Input('display-property-name'), 
        __metadata('design:type', String)
    ], AutoCompleteDirective.prototype, "displayPropertyName", void 0);
    __decorate([
        core_1.Input('dataSourceMethod'), 
        __metadata('design:type', Function)
    ], AutoCompleteDirective.prototype, "sourceAction", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], AutoCompleteDirective.prototype, "ngModel", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], AutoCompleteDirective.prototype, "keywordChanged", void 0);
    AutoCompleteDirective = __decorate([
        core_1.Directive({
            selector: '[ac-autocomplete]',
            host: {
                '(input)': 'inputEventHandler($event)',
                '(keydown)': 'keydownEventHandler($event)',
            }
        }), 
        __metadata('design:paramtypes', [core_1.ComponentFactoryResolver, core_1.ViewContainerRef])
    ], AutoCompleteDirective);
    return AutoCompleteDirective;
}());
exports.AutoCompleteDirective = AutoCompleteDirective;
