import {Component, ElementRef, Input, Output, EventEmitter, OnInit,
    OnChanges, SimpleChange, DoCheck, IterableDiffers, ViewChild } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import 'rxjs/Rx';
import { AcGridColumn, AcGridPaginatorComponent, pageNumberChangedEventArg, IPagingDetail} from './ac-grid';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

export interface ISortByDetail {
    field: string,
    orderByDesc: boolean,
}

export interface pageChangedEventArg {
    selectedPage: number,
    pageSize: number,
    sortBy: ISortByDetail[];
}

export interface IDataSourceDetail {
    data: any[],
    totalRecordsCount: number;
}

@Component({
    selector: 'ac-grid',
    templateUrl: 'app/lib/aclibs/ac-grid/ac-grid.component.html',
    styleUrls: ['app/lib/aclibs/ac-grid/ac-grid.component.css'],
    exportAs: 'acGrid'
})
export class AcGridComponent implements OnInit, OnChanges, DoCheck {

    @Input('dataSource') dataSource: any[] = [];
    @Input('remoteSource') remoteSource: string;
    @Input('paging') paging: string;
    @Input('columns') columns: string;
    @Input("gridColumns") gridColumns: AcGridColumn[];
    @Input("selectAllChecked") selectAllChecked: boolean;
    @Input("initialSortBy") initialSortBy: string;
    @Output('pageChangedEvent') pageChangedEvent = new EventEmitter<pageChangedEventArg>();
    @Output('selectAllChangedEvent') selectAllChangedEvent = new EventEmitter<boolean>();
    // event emitter for sortColumnEvent.
    @Output('sortColumnEvent') sortColumnEvent = new EventEmitter<string>();
    @Input("thresholdRecordCount") thresholdRecordCount: number = 100;

    public items: any[];
    private rowTemplate: any;
    private sortedColumnName: string = "";
    private sortedByDesc: boolean = false;
    private columnCount: number;
    private pageSize: number = 0;
    private activePage: number = 1;
    private sortByCriteria: ISortByDetail[];
    differ: any;
    private pagingDetail: IPagingDetail;
    @ViewChild(AcGridPaginatorComponent) paginator: AcGridPaginatorComponent;
    private pageSplit: any[];
    private totalRecordsCount: number = 0;
    private applySorting = true;
    private selectAllStatus: boolean;

    constructor(differs: IterableDiffers) {
        this.differ = differs.find([]).create(null);
    }

    ngOnInit(): void {
        this.gridColumns = <AcGridColumn[]>JSON.parse(this.columns);

        if (this.paging) {
            this.pagingDetail = <IPagingDetail>JSON.parse(this.paging);
            this.pageSize = this.pagingDetail.pageSize;
        }
        this.columnCount = this.gridColumns.length;
        if (this.initialSortBy && this.initialSortBy.length) {
            this.sortByCriteria = <ISortByDetail[]>JSON.parse(this.initialSortBy);
        }
        let pagi = this.paginator;
    }

    ngOnChanges(changes: { [propertyName: string]: SimpleChange }) {
        for (let propName in changes) {
            if (propName === 'dataSource') {

                //if (this.dataSource) {
                //    this.totalRecordsCount = this.dataSource.length;
                //    this.updatePageSplit(this.activePage, this.dataSource);

                //    if (this.paginator) {
                //        this.paginator.refreshPaginator(this.totalRecordsCount, this.activePage);
                //    } else {
                //        this.loadPage(this.activePage);
                //    }
                //} else {
                //    this.loadPage(this.activePage);
                //}
            }
            else if (propName === 'remoteSource') {
                if (this.remoteSource) {
                    let remoteSourceDetail = <IDataSourceDetail>JSON.parse(this.remoteSource);
                    this.dataSource = remoteSourceDetail.data;
                    this.totalRecordsCount = remoteSourceDetail.totalRecordsCount;

                    if (this.paginator) {
                        this.paginator.refreshPaginator(this.totalRecordsCount, this.activePage);
                    } else {
                        this.loadPage(this.activePage);
                    }
                } else {
                    this.loadPage(this.activePage);
                }
            }
        }
    }

    ngDoCheck(): any {
        var changes = this.differ.diff(this.dataSource);
        if (changes) {
            let added = false;
            let removed = false;

            changes.forEachAddedItem((elt) => {
                added = true;
            });
            changes.forEachRemovedItem((elt) => {
                removed = true;
            });

            if (!added && !removed) {
                return;
            }

            this.totalRecordsCount = this.dataSource.length;
            this.updatePageSplit(this.activePage, this.dataSource);

            if (this.paginator) {
                this.paginator.refreshPaginator(this.totalRecordsCount, this.activePage);
            } else {
                this.loadPage(this.activePage);
            }
        }
    }

    getColumnCount(): string {
        return this.gridColumns.length.toString();
    }

    sortColumn(sortedColumn: AcGridColumn) {
        if (this.sortedColumnName !== sortedColumn.header) {
            this.sortedByDesc = false;
        } else {
            this.sortedByDesc = !this.sortedByDesc;
        }

        this.sortByCriteria = <ISortByDetail[]>[{ "field": sortedColumn.field, "orderByDesc": this.sortedByDesc }];
        this.sortedColumnName = sortedColumn.header;
        this.sortedByDesc = this.sortedByDesc;

        if (this.pagingDetail && this.pagingDetail.isServerSide) {
            this.pageChangedEvent.emit({
                selectedPage: this.activePage,
                pageSize: this.pageSize,
                sortBy: this.sortByCriteria
            });
        } else {
            this.updatePageSplit(this.activePage, this.dataSource);
            this.loadPage(this.activePage);
        }

        // emmit sortColumnEvent with header
        this.sortColumnEvent.emit(sortedColumn.header);
    }

    sortBy(fields: ISortByDetail[], sourceArray: any[]): any[] {
        if (!fields || !fields.length || !sourceArray || !sourceArray.length) {
            return [];
        }
        var i = 0;
        var object = fields[0].field.substring(0, fields[0].field.indexOf('.'));
        var field = fields[0].field.substring(fields[0].field.indexOf('.') + 1);
        if (typeof sourceArray[0][object] === 'object' || sourceArray[0][object] instanceof Object) {
            return sourceArray.sort((itemLeft, itemRight) => {
                return this.thenByForObject(itemLeft, itemRight, fields, i, field, object);
            });
        }
        return sourceArray.sort((itemLeft, itemRight) => {
            return this.thenBy(itemLeft, itemRight, fields, i);
        });
    }

    getItemsOnPage(page: number) {
        if (page <= this.pageSplit.length && page > 0)
            return this.pageSplit[page - 1];
        else
            return this.pageSplit[0];
    }

    private thenByForObject(itemLeft: any, itemRight: any, fields: ISortByDetail[], i: number, field: string, object: string) {
        var orderByDesc = fields[i].orderByDesc;

        let left = itemLeft[object][field] ? itemLeft[object][field].toString().trim() : "";
        let right = itemRight[object][field] ? itemRight[object][field].toString().trim() : "";
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
    }

    private thenBy(itemLeft: any, itemRight: any, fields: ISortByDetail[], i: number) {
        var orderByDesc = fields[i].orderByDesc;

        //if (itemLeft[fields[i].field] < itemRight[fields[i].field]) {
        //    return orderByDesc ? 1 : -1;
        //}
        //if (itemLeft[fields[i].field] > itemRight[fields[i].field]) {
        //    return orderByDesc ? -1 : 1;
        //}
        let left = itemLeft[fields[i].field].toString().trim();
        let right = itemRight[fields[i].field].toString().trim();
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
    }

    // note: wrapper for onPageChanged()
    public refresh() {
        this.ngOnInit();
        this.activePage = 1;
        this.updatePageSplit(this.activePage, this.dataSource);
        this.loadPage(this.activePage);
    }

    private onPrevious() {
        let pageEvent: pageNumberChangedEventArg = {
            pageSize: this.pageSize,
            selectedPage: (this.activePage - 1)
        };

        if (pageEvent.selectedPage < 1) {
            this.activePage = 1;
            pageEvent.selectedPage = this.activePage;
        }

        this.onPageChanged(pageEvent);
    }
    private onNext() {
        let pageEvent: pageNumberChangedEventArg = { pageSize: Number(this.pageSize), selectedPage: Number(this.activePage + 1) };
        if (this.pageSplit && pageEvent.selectedPage > this.pageSplit.length) {
            this.activePage = this.pageSplit.length;
            pageEvent.selectedPage = this.activePage;
        }

        this.onPageChanged(pageEvent);
    }

    private onFirst() {
        let pageEvent: pageNumberChangedEventArg = {
            pageSize: this.pageSize,
            selectedPage: (this.activePage - (this.activePage - 1))
        };
        if (pageEvent.selectedPage < 1) {
            this.activePage = 1;
            pageEvent.selectedPage = this.activePage;
        }
        this.onPageChanged(pageEvent);
    }
    private onLast() {
        let pageEvent: pageNumberChangedEventArg =
            { pageSize: Number(this.pageSize), selectedPage: Number(this.pageSplit.length) };
        this.onPageChanged(pageEvent);
    }

    private onBlur() {
        let pageEvent: pageNumberChangedEventArg = {
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
    private onKeyPress($keyEvent) {
        if ($keyEvent.which === 13) {
            let pageEvent: pageNumberChangedEventArg = {
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
    }

    private onPageSizeChange($event) {
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
    }



    private onPageChanged(pageChangedEventArg: pageNumberChangedEventArg) {
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
    }

    private loadPage(pageNumber?: number) {
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
    }

    private loadPageServerSide() {
        if (!this.pageSplit || !this.pageSplit.length) {
            this.items = [];
            return;
        }

        this.items = this.pageSplit[0];
    }

    private updatePageSplit(pageNumber?: number, data?: any[]) {
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
    }

    private splitDataToPages(pageSize: number, data: any[]): any[] {
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

        let recordsCount = data.length;

        let totalPageCount = Math.floor(recordsCount / pageSize) +
            ((recordsCount % pageSize) > 0 ? 1 : 0);

        let startIndex: number = 0;
        for (let i = 0; i < totalPageCount && startIndex < recordsCount; i++) {
            if (startIndex + pageSize > recordsCount) {
                pageSize = recordsCount - startIndex;
            }

            this.pageSplit[i] = data.slice(startIndex, startIndex + pageSize);
            startIndex += pageSize;
        }
    }

    private selectAllChanged(selected: boolean) {
        if (this.totalRecordsCount <= this.thresholdRecordCount) {
            this.selectAllChangedEvent.emit(!selected);
        } else {
            // this code would never be executed under working condition.
            // the checkbox would be hidden/ disabled.
            this.selectAllChecked = false;
        }
    }
}