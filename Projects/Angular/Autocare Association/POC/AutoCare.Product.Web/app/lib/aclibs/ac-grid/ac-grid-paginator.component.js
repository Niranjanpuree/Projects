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
var AcGridPaginatorComponent = (function () {
    function AcGridPaginatorComponent() {
        var _this = this;
        this.totalRecordsCount = 0;
        this.totalPageCount = 0;
        this.activePage = 1;
        this.actualNumberOfPagesForPageList = 0;
        this.pageNumberChangedEvent = new core_1.EventEmitter();
        this.pagesList = [];
        this.showGoToFirstPage = false;
        this.showGoToLastPage = false;
        this.refreshPaginator = function (newTotalRecordsCount, newActivePage) {
            _this.totalRecordsCount = newTotalRecordsCount;
            _this.activePage = newActivePage;
            if (newTotalRecordsCount > 0) {
                _this.totalPageCount = Math.floor(newTotalRecordsCount / _this.pagingDetail.pageSize) +
                    ((newTotalRecordsCount % _this.pagingDetail.pageSize) > 0 ? 1 : 0);
                _this.actualNumberOfPagesForPageList = _this.totalPageCount > _this.pagingDetail.numberOfPagesOnPageList ?
                    _this.pagingDetail.numberOfPagesOnPageList :
                    _this.totalPageCount;
                _this.identifyPageNumbers(_this.activePage);
            }
            _this.loadPage(_this.activePage);
        };
    }
    AcGridPaginatorComponent.prototype.ngOnInit = function () {
        if (this.pagingDetail) {
            //    this.pagingDetail = <IPagingDetail>JSON.parse(this.paging);
            //    //To set default numberOfPagesOnPageList
            this.pagingDetail.numberOfPagesOnPageList = !this.pagingDetail.numberOfPagesOnPageList ||
                this.pagingDetail.numberOfPagesOnPageList == 0 ?
                5 : this.pagingDetail.numberOfPagesOnPageList;
        }
    };
    // note: setter for pagingDetail
    AcGridPaginatorComponent.prototype.setPagingDetail = function (pagingDetail) {
        this.pagingDetail = pagingDetail;
    };
    AcGridPaginatorComponent.prototype.loadPage = function (selectedPage) {
        if (this.activePage == 0) {
            return;
        }
        if (this.activePage != selectedPage) {
            this.identifyPageNumbers(selectedPage);
        }
        this.activePage = selectedPage;
        this.pageNumberChangedEvent.emit({
            selectedPage: selectedPage,
            pageSize: this.pagingDetail.pageSize
        });
    };
    //To find the page numbers to be displayed
    AcGridPaginatorComponent.prototype.identifyPageNumbers = function (selectedPage) {
        this.pagesList = [];
        var leftToCenter = [];
        var rightToCenter = [];
        for (var i = 1, j = selectedPage - 1; i <= this.actualNumberOfPagesForPageList && j >= 1; i++, j--) {
            leftToCenter.push(j);
        }
        for (var i = 1, j = selectedPage + 1; i <= this.actualNumberOfPagesForPageList && j <= this.totalPageCount; i++, j++) {
            rightToCenter.push(j);
        }
        leftToCenter = leftToCenter.sort(function (a, b) { return a - b; });
        rightToCenter = rightToCenter.sort(function (a, b) { return a - b; }).reverse();
        this.pagesList.push(selectedPage);
        for (var i = 1; i <= this.actualNumberOfPagesForPageList; i++) {
            if (this.pagesList.length >= this.actualNumberOfPagesForPageList) {
                break;
            }
            var page = 0;
            if (i % 2) {
                page = leftToCenter.pop();
                if (!page) {
                    page = rightToCenter.pop();
                }
            }
            else {
                page = rightToCenter.pop();
                if (!page) {
                    page = leftToCenter.pop();
                }
            }
            if (page) {
                this.pagesList.push(page);
            }
        }
        this.pagesList.sort(function (a, b) { return a - b; });
        this.showGoToFirstPage = this.actualNumberOfPagesForPageList == this.pagingDetail.numberOfPagesOnPageList &&
            this.pagesList.indexOf(1) === -1;
        this.showGoToLastPage = this.actualNumberOfPagesForPageList == this.pagingDetail.numberOfPagesOnPageList &&
            this.pagesList.indexOf(this.totalPageCount) === -1;
    };
    __decorate([
        core_1.Input('totalRecordsCount'), 
        __metadata('design:type', Number)
    ], AcGridPaginatorComponent.prototype, "totalRecordsCount", void 0);
    __decorate([
        core_1.Input('pagingDetail'), 
        __metadata('design:type', Object)
    ], AcGridPaginatorComponent.prototype, "pagingDetail", void 0);
    __decorate([
        core_1.Output('pageNumberChangedEvent'), 
        __metadata('design:type', Object)
    ], AcGridPaginatorComponent.prototype, "pageNumberChangedEvent", void 0);
    AcGridPaginatorComponent = __decorate([
        core_1.Component({
            selector: 'ac-grid-paginator',
            templateUrl: 'app/lib/aclibs/ac-grid/ac-grid-paginator.component.html',
            styleUrls: ['app/lib/aclibs/ac-grid/ac-grid.component.css'],
        }), 
        __metadata('design:paramtypes', [])
    ], AcGridPaginatorComponent);
    return AcGridPaginatorComponent;
}());
exports.AcGridPaginatorComponent = AcGridPaginatorComponent;
