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
var Subject_1 = require("rxjs/Subject");
var ac_autocomplete_service_1 = require('./ac-autocomplete.service');
/**
 * show a selected date in monthly calendar
 * Each filteredList item has the following property in addition to data itself
 *   1. displayValue as string e.g. Allen Kim
 *   2. dataValue as any e.g. 1234
 */
var AutoCompleteComponent = (function () {
    /**
     * constructor
     */
    function AutoCompleteComponent(elementRef, autoComplete) {
        this.autoComplete = autoComplete;
        this.minChars = 0;
        this.valuePropertyName = 'id';
        this.displayPropertyName = 'value';
        this.dropdownVisible = false;
        this.isLoading = false;
        this.filteredList = [];
        this.itemIndex = -1;
        this.isCancelled = false;
        this.valueSelectedEvent = new Subject_1.Subject();
        this.delay = (function () {
            var timer = 0;
            return function (callback, ms) {
                clearTimeout(timer);
                timer = setTimeout(callback, ms);
            };
        })();
        this.el = elementRef.nativeElement;
        this.zone = new core_1.NgZone({ enableLongStackTrace: false });
    }
    /**
     * user enters into input el, shows list to select, then select one
     */
    AutoCompleteComponent.prototype.ngOnInit = function () {
        this.autoComplete.source = this.source;
        this.autoComplete.pathToData = this.pathToData;
    };
    AutoCompleteComponent.prototype.ngOnChanges = function (changes) {
        for (var propName in changes) {
            if (propName === 'filteredList') {
                alert('coming');
                this.isLoading = false;
            }
        }
    };
    AutoCompleteComponent.prototype.reloadListInDelay = function (searchText) {
        var _this = this;
        var delayMs = this.source.constructor.name == 'Array' ? 10 : 500;
        this.dropdownVisible = false;
        //executing after user stopped typing
        this.delay(function () { return _this.reloadList(searchText); }, delayMs);
    };
    //showDropdownList(): void {
    //    this.keyword = '';
    //    this.reloadList(this.keyword);
    //    //this.inputEl.focus();
    //}
    AutoCompleteComponent.prototype.hideDropdownList = function () {
        this.dropdownVisible = false;
    };
    AutoCompleteComponent.prototype.reloadList = function (searchText) {
        var _this = this;
        //let keyword = this.inputEl.value;
        if (this.isCancelled) {
            this.isLoading = false;
            this.isCancelled = false;
            return;
        }
        this.isLoading = true;
        var keyword = searchText;
        if (keyword.length >= this.minChars) {
            this.filteredList = [];
            var query = { keyword: keyword };
            this.dropdownVisible = true;
            this.sourceAction(keyword)
                .subscribe(function (resp) {
                _this.zone.run(function () {
                    _this.filteredList = resp;
                    if (!_this.filteredList || !_this.filteredList.length || _this.isCancelled) {
                        _this.dropdownVisible = false;
                        _this.isCancelled = false;
                    }
                    _this.isLoading = false;
                });
            }, function (error) { return null; }, function () { return _this.isLoading = false; } //complete
             //complete
            );
        }
    };
    AutoCompleteComponent.prototype.selectOne = function (data) {
        this.hideDropdownList();
        this.itemIndex = -1;
        this.valueSelectedEvent.next(data);
    };
    ;
    AutoCompleteComponent.prototype.inputElKeyHandler = function (evt) {
        var totalNumItem = !this.filteredList ? 0 : this.filteredList.length;
        switch (evt.keyCode) {
            case 27:
                this.hideDropdownList();
                break;
            case 38:
                this.itemIndex = (totalNumItem + this.itemIndex - 1) % totalNumItem;
                break;
            case 40:
                this.dropdownVisible = true;
                this.itemIndex = (totalNumItem + this.itemIndex + 1) % totalNumItem;
                break;
            case 13:
                this.isCancelled = true;
                if (!this.dropdownVisible) {
                    this.selectOne(undefined);
                    return;
                }
                this.selectOne(this.filteredList[this.itemIndex]);
                evt.preventDefault();
                break;
        }
    };
    ;
    AutoCompleteComponent.prototype.getFormattedList = function (data) {
        var formatter = this.listFormatter || this.defaultListFormatter;
        return formatter.apply(this, [data]);
    };
    AutoCompleteComponent.prototype.defaultListFormatter = function (data) {
        var html = "";
        html += data[this.displayPropertyName] ? "<span>" + data[this.displayPropertyName] + "</span>" : data;
        html += data[this.valuePropertyName] ? " (" + data[this.valuePropertyName] + ")" : "";
        return html;
    };
    __decorate([
        core_1.Input('list-formatter'), 
        __metadata('design:type', Function)
    ], AutoCompleteComponent.prototype, "listFormatter", void 0);
    __decorate([
        core_1.Input('source'), 
        __metadata('design:type', Object)
    ], AutoCompleteComponent.prototype, "source", void 0);
    __decorate([
        core_1.Input('path-to-data'), 
        __metadata('design:type', String)
    ], AutoCompleteComponent.prototype, "pathToData", void 0);
    __decorate([
        core_1.Input('min-chars'), 
        __metadata('design:type', Number)
    ], AutoCompleteComponent.prototype, "minChars", void 0);
    __decorate([
        core_1.Input('value-property-name'), 
        __metadata('design:type', String)
    ], AutoCompleteComponent.prototype, "valuePropertyName", void 0);
    __decorate([
        core_1.Input('display-property-name'), 
        __metadata('design:type', String)
    ], AutoCompleteComponent.prototype, "displayPropertyName", void 0);
    __decorate([
        core_1.Input('placeholder'), 
        __metadata('design:type', String)
    ], AutoCompleteComponent.prototype, "placeholder", void 0);
    __decorate([
        core_1.Input('dataSource'), 
        __metadata('design:type', Function)
    ], AutoCompleteComponent.prototype, "sourceAction", void 0);
    AutoCompleteComponent = __decorate([
        core_1.Component({
            providers: [ac_autocomplete_service_1.AutoCompleteService],
            selector: 'ac-autocomplete',
            templateUrl: 'app/lib/aclibs/ac-autocomplete/ac-autocomplete.component.html',
            styleUrls: ['app/lib/aclibs/ac-autocomplete/ac-autocomplete.component.css'],
            //encapsulation: ViewEncapsulation.Native
            encapsulation: core_1.ViewEncapsulation.None
        }), 
        __metadata('design:paramtypes', [core_1.ElementRef, ac_autocomplete_service_1.AutoCompleteService])
    ], AutoCompleteComponent);
    return AutoCompleteComponent;
}());
exports.AutoCompleteComponent = AutoCompleteComponent;
