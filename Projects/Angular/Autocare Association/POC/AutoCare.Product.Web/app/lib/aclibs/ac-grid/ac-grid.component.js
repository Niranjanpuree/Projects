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
require('rxjs/Rx');
var ac_grid_1 = require('./ac-grid');
var AcGridComponent = (function () {
    function AcGridComponent(differs) {
        this.dataSource = [];
        this.pageChangedEvent = new core_1.EventEmitter();
        this.selectAllChangedEvent = new core_1.EventEmitter();
        // event emitter for sortColumnEvent.
        this.sortColumnEvent = new core_1.EventEmitter();
        this.thresholdRecordCount = 100;
        this.sortedColumnName = "";
        this.sortedByDesc = false;
        this.pageSize = 0;
        this.activePage = 1;
        this.totalRecordsCount = 0;
        this.applySorting = true;
        this.differ = differs.find([]).create(null);
    }
    AcGridComponent.prototype.ngOnInit = function () {
        this.gridColumns = JSON.parse(this.columns);
        if (this.paging) {
            this.pagingDetail = JSON.parse(this.paging);
            this.pageSize = this.pagingDetail.pageSize;
        }
        this.columnCount = this.gridColumns.length;
        if (this.initialSortBy && this.initialSortBy.length) {
            this.sortByCriteria = JSON.parse(this.initialSortBy);
        }
        var pagi = this.paginator;
    };
    AcGridComponent.prototype.ngOnChanges = function (changes) {
        for (var propName in changes) {
            if (propName === 'dataSource') {
            }
            else if (propName === 'remoteSource') {
                if (this.remoteSource) {
                    var remoteSourceDetail = JSON.parse(this.remoteSource);
                    this.dataSource = remoteSourceDetail.data;
                    this.totalRecordsCount = remoteSourceDetail.totalRecordsCount;
                    if (this.paginator) {
                        this.paginator.refreshPaginator(this.totalRecordsCount, this.activePage);
                    }
                    else {
                        this.loadPage(this.activePage);
                    }
                }
                else {
                    this.loadPage(this.activePage);
                }
            }
        }
    };
    AcGridComponent.prototype.ngDoCheck = function () {
        var changes = this.differ.diff(this.dataSource);
        if (changes) {
            var added_1 = false;
            var removed_1 = false;
            changes.forEachAddedItem(function (elt) {
                added_1 = true;
            });
            changes.forEachRemovedItem(function (elt) {
                removed_1 = true;
            });
            if (!added_1 && !removed_1) {
                return;
            }
            this.totalRecordsCount = this.dataSource.length;
            this.updatePageSplit(this.activePage, this.dataSource);
            if (this.paginator) {
                this.paginator.refreshPaginator(this.totalRecordsCount, this.activePage);
            }
            else {
                this.loadPage(this.activePage);
            }
        }
    };
    AcGridComponent.prototype.getColumnCount = function () {
        return this.gridColumns.length.toString();
    };
    AcGridComponent.prototype.sortColumn = function (sortedColumn) {
        if (this.sortedColumnName !== sortedColumn.header) {
            this.sortedByDesc = false;
        }
        else {
            this.sortedByDesc = !this.sortedByDesc;
        }
        this.sortByCriteria = [{ "field": sortedColumn.field, "orderByDesc": this.sortedByDesc }];
        this.sortedColumnName = sortedColumn.header;
        this.sortedByDesc = this.sortedByDesc;
        if (this.pagingDetail && this.pagingDetail.isServerSide) {
            this.pageChangedEvent.emit({
                selectedPage: this.activePage,
                pageSize: this.pageSize,
                sortBy: this.sortByCriteria
            });
        }
        else {
            this.updatePageSplit(this.activePage, this.dataSource);
            this.loadPage(this.activePage);
        }
        // emmit sortColumnEvent with header
        this.sortColumnEvent.emit(sortedColumn.header);
    };
    AcGridComponent.prototype.sortBy = function (fields, sourceArray) {
        var _this = this;
        if (!fields || !fields.length || !sourceArray || !sourceArray.length) {
            return [];
        }
        var i = 0;
        var object = fields[0].field.substring(0, fields[0].field.indexOf('.'));
        var field = fields[0].field.substring(fields[0].field.indexOf('.') + 1);
        if (typeof sourceArray[0][object] === 'object' || sourceArray[0][object] instanceof Object) {
            return sourceArray.sort(function (itemLeft, itemRight) {
                return _this.thenByForObject(itemLeft, itemRight, fields, i, field, object);
            });
        }
        return sourceArray.sort(function (itemLeft, itemRight) {
            return _this.thenBy(itemLeft, itemRight, fields, i);
        });
    };
    AcGridComponent.prototype.getItemsOnPage = function (page) {
        if (page <= this.pageSplit.length && page > 0)
            return this.pageSplit[page - 1];
        else
            return this.pageSplit[0];
    };
    AcGridComponent.prototype.thenByForObject = function (itemLeft, itemRight, fields, i, field, object) {
        var orderByDesc = fields[i].orderByDesc;
        var left = itemLeft[object][field] ? itemLeft[object][field].toString().trim() : "";
        var right = itemRight[object][field] ? itemRight[object][field].toString().trim() : "";
        if (left.localeCompare(right, "kn", { sensitive: 'base', numeric: true }) < 0) {
            return orderByDesc ? 1 : -1;
        }
        if (left.localeCompare(right, "kn", { sensitive: 'base', numeric: true }) > 0) {
            return orderByDesc ? -1 : 1;
        }
        i++;
        if (i >= fields.length) {
            return 0;
        }
        return this.thenByForObject(itemLeft, itemRight, fields, i, field, object);
    };
    AcGridComponent.prototype.thenBy = function (itemLeft, itemRight, fields, i) {
        var orderByDesc = fields[i].orderByDesc;
        //if (itemLeft[fields[i].field] < itemRight[fields[i].field]) {
        //    return orderByDesc ? 1 : -1;
        //}
        //if (itemLeft[fields[i].field] > itemRight[fields[i].field]) {
        //    return orderByDesc ? -1 : 1;
        //}
        var left = itemLeft[fields[i].field].toString().trim();
        var right = itemRight[fields[i].field].toString().trim();
        if (left.localeCompare(right, "kn", { sensitive: 'base', numeric: true }) < 0) {
            return orderByDesc ? 1 : -1;
        }
        if (left.localeCompare(right, "kn", { sensitive: 'base', numeric: true }) > 0) {
            return orderByDesc ? -1 : 1;
        }
        i++;
        if (i >= fields.length) {
            return 0;
        }
        return this.thenBy(itemLeft, itemRight, fields, i);
    };
    // note: wrapper for onPageChanged()
    AcGridComponent.prototype.refresh = function () {
        this.ngOnInit();
        this.activePage = 1;
        this.updatePageSplit(this.activePage, this.dataSource);
        this.loadPage(this.activePage);
    };
    AcGridComponent.prototype.onPrevious = function () {
        var pageEvent = {
            pageSize: this.pageSize,
            selectedPage: (this.activePage - 1)
        };
        if (pageEvent.selectedPage < 1) {
            this.activePage = 1;
            pageEvent.selectedPage = this.activePage;
        }
        this.onPageChanged(pageEvent);
    };
    AcGridComponent.prototype.onNext = function () {
        var pageEvent = { pageSize: Number(this.pageSize), selectedPage: Number(this.activePage + 1) };
        if (this.pageSplit && pageEvent.selectedPage > this.pageSplit.length) {
            this.activePage = this.pageSplit.length;
            pageEvent.selectedPage = this.activePage;
        }
        this.onPageChanged(pageEvent);
    };
    AcGridComponent.prototype.onFirst = function () {
        var pageEvent = {
            pageSize: this.pageSize,
            selectedPage: (this.activePage - (this.activePage - 1))
        };
        if (pageEvent.selectedPage < 1) {
            this.activePage = 1;
            pageEvent.selectedPage = this.activePage;
        }
        this.onPageChanged(pageEvent);
    };
    AcGridComponent.prototype.onLast = function () {
        var pageEvent = { pageSize: Number(this.pageSize), selectedPage: Number(this.pageSplit.length) };
        this.onPageChanged(pageEvent);
    };
    AcGridComponent.prototype.onBlur = function () {
        var pageEvent = {
            pageSize: this.pageSize,
            selectedPage: this.activePage
        };
        if (pageEvent.selectedPage < 1) {
            this.activePage = 1;
            pageEvent.selectedPage = this.activePage;
        }
        else if (this.pageSplit && pageEvent.selectedPage > this.pageSplit.length) {
            this.activePage = this.pageSplit.length;
            pageEvent.selectedPage = this.activePage;
        }
        this.onPageChanged(pageEvent);
    };
    AcGridComponent.prototype.onKeyPress = function ($keyEvent) {
        if ($keyEvent.which === 13) {
            var pageEvent = {
                pageSize: this.pageSize,
                selectedPage: this.activePage
            };
            if (pageEvent.selectedPage < 1) {
                this.activePage = 1;
                pageEvent.selectedPage = this.activePage;
            }
            else if (this.pageSplit && pageEvent.selectedPage > this.pageSplit.length) {
                this.activePage = this.pageSplit.length;
                pageEvent.selectedPage = this.activePage;
            }
            this.onPageChanged(pageEvent);
        }
    };
    AcGridComponent.prototype.onPageSizeChange = function ($event) {
        this.paging = '{"pageSize":' + $event.target.value + ', "numberOfPagesOnPageList":' + this.pagingDetail.numberOfPagesOnPageList + '}';
        this.ngOnInit();
        this.activePage = 1;
        this.updatePageSplit(this.activePage, this.dataSource);
        this.loadPage(this.activePage);
        this.pageChangedEvent.emit({
            selectedPage: this.activePage,
            pageSize: this.pageSize,
            sortBy: this.sortByCriteria
        });
    };
    AcGridComponent.prototype.onPageChanged = function (pageChangedEventArg) {
        if (!pageChangedEventArg) {
            return;
        }
        if (pageChangedEventArg.selectedPage !== this.activePage) {
            if (this.pagingDetail && this.pagingDetail.isServerSide) {
                this.pageSplit = [];
            }
            this.pageChangedEvent.emit({
                selectedPage: pageChangedEventArg.selectedPage,
                pageSize: pageChangedEventArg.pageSize,
                sortBy: this.sortByCriteria
            });
        }
        this.loadPage(pageChangedEventArg.selectedPage);
    };
    AcGridComponent.prototype.loadPage = function (pageNumber) {
        if (!this.pageSplit || !this.pageSplit.length) {
            this.items = [];
            return;
        }
        if (pageNumber && pageNumber != 0) {
            this.activePage = pageNumber;
        }
        if (this.pageSize == 0) {
            this.pageSize = this.totalRecordsCount;
        }
        if (this.pagingDetail && this.pagingDetail.isServerSide) {
            this.loadPageServerSide();
            return;
        }
        //this.sortBy(this.sortByCriteria, this.dataSource);
        //let startIndex = this.activePage == 0 || this.pageSize == 0 ? 0 : (this.activePage - 1) * this.pageSize;
        ////let endIndex = (startIndex + this.pageSize) <= this.dataSource.length ?
        ////    (startIndex + this.pageSize) : this.dataSource.length - 1;
        //// NOTE: Removed (- 1) from this.dataSource.length for calculation of endIndex. This was returning 1 less record to the view.
        //let endIndex = (startIndex + this.pageSize) <= this.dataSource.length ?
        //    (startIndex + this.pageSize) : this.dataSource.length;
        //this.items = this.dataSource.slice(startIndex, endIndex);
        if (this.pageSplit && this.pageSplit.length >= pageNumber) {
            this.items = this.pageSplit[pageNumber - 1];
            return;
        }
        this.items = [];
    };
    AcGridComponent.prototype.loadPageServerSide = function () {
        if (!this.pageSplit || !this.pageSplit.length) {
            this.items = [];
            return;
        }
        this.items = this.pageSplit[0];
    };
    AcGridComponent.prototype.updatePageSplit = function (pageNumber, data) {
        this.items = [];
        if (!data || !data.length) {
            this.pageSplit = [];
            return;
        }
        if (pageNumber && pageNumber != 0) {
            this.activePage = pageNumber;
        }
        if (this.pagingDetail && this.pagingDetail.isServerSide) {
            this.pageSplit = [];
            this.pageSplit[0] = data;
            return;
        }
        this.sortBy(this.sortByCriteria, data);
        this.splitDataToPages(this.pageSize, data);
    };
    AcGridComponent.prototype.splitDataToPages = function (pageSize, data) {
        // explicitly cast pageSize to number
        pageSize = Number(pageSize);
        this.pageSplit = [];
        if (!data || !data.length) {
            return this.pageSplit;
        }
        if (pageSize == 0) {
            this.pageSplit[0] = data;
            return this.pageSplit;
        }
        var recordsCount = data.length;
        var totalPageCount = Math.floor(recordsCount / pageSize) +
            ((recordsCount % pageSize) > 0 ? 1 : 0);
        var startIndex = 0;
        for (var i = 0; i < totalPageCount && startIndex < recordsCount; i++) {
            if (startIndex + pageSize > recordsCount) {
                pageSize = recordsCount - startIndex;
            }
            this.pageSplit[i] = data.slice(startIndex, startIndex + pageSize);
            startIndex += pageSize;
        }
    };
    AcGridComponent.prototype.selectAllChanged = function (selected) {
        if (this.totalRecordsCount <= this.thresholdRecordCount) {
            this.selectAllChangedEvent.emit(!selected);
        }
        else {
            // this code would never be executed under working condition.
            // the checkbox would be hidden/ disabled.
            this.selectAllChecked = false;
        }
    };
    __decorate([
        core_1.Input('dataSource'), 
        __metadata('design:type', Array)
    ], AcGridComponent.prototype, "dataSource", void 0);
    __decorate([
        core_1.Input('remoteSource'), 
        __metadata('design:type', String)
    ], AcGridComponent.prototype, "remoteSource", void 0);
    __decorate([
        core_1.Input('paging'), 
        __metadata('design:type', String)
    ], AcGridComponent.prototype, "paging", void 0);
    __decorate([
        core_1.Input('columns'), 
        __metadata('design:type', String)
    ], AcGridComponent.prototype, "columns", void 0);
    __decorate([
        core_1.Input("gridColumns"), 
        __metadata('design:type', Array)
    ], AcGridComponent.prototype, "gridColumns", void 0);
    __decorate([
        core_1.Input("selectAllChecked"), 
        __metadata('design:type', Boolean)
    ], AcGridComponent.prototype, "selectAllChecked", void 0);
    __decorate([
        core_1.Input("initialSortBy"), 
        __metadata('design:type', String)
    ], AcGridComponent.prototype, "initialSortBy", void 0);
    __decorate([
        core_1.Output('pageChangedEvent'), 
        __metadata('design:type', Object)
    ], AcGridComponent.prototype, "pageChangedEvent", void 0);
    __decorate([
        core_1.Output('selectAllChangedEvent'), 
        __metadata('design:type', Object)
    ], AcGridComponent.prototype, "selectAllChangedEvent", void 0);
    __decorate([
        core_1.Output('sortColumnEvent'), 
        __metadata('design:type', Object)
    ], AcGridComponent.prototype, "sortColumnEvent", void 0);
    __decorate([
        core_1.Input("thresholdRecordCount"), 
        __metadata('design:type', Number)
    ], AcGridComponent.prototype, "thresholdRecordCount", void 0);
    __decorate([
        core_1.ViewChild(ac_grid_1.AcGridPaginatorComponent), 
        __metadata('design:type', ac_grid_1.AcGridPaginatorComponent)
    ], AcGridComponent.prototype, "paginator", void 0);
    AcGridComponent = __decorate([
        core_1.Component({
            selector: 'ac-grid',
            templateUrl: 'app/lib/aclibs/ac-grid/ac-grid.component.html',
            styleUrls: ['app/lib/aclibs/ac-grid/ac-grid.component.css'],
            exportAs: 'acGrid'
        }), 
        __metadata('design:paramtypes', [core_1.IterableDiffers])
    ], AcGridComponent);
    return AcGridComponent;
}());
exports.AcGridComponent = AcGridComponent;
