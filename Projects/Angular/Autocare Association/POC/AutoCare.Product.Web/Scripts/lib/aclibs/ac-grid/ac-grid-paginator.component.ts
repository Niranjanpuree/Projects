import { Component, Input, Output, OnInit, OnChanges, SimpleChange, EventEmitter } from '@angular/core'

export interface IPagingDetail {
    pageSize: number,
    numberOfPagesOnPageList: number,
    isServerSide: boolean
}

export interface pageNumberChangedEventArg {
    selectedPage: number,
    pageSize: number,
}

@Component({
    selector: 'ac-grid-paginator',
    templateUrl: 'app/lib/aclibs/ac-grid/ac-grid-paginator.component.html',
    styleUrls: ['app/lib/aclibs/ac-grid/ac-grid.component.css'],
})
export class AcGridPaginatorComponent implements OnInit {
    @Input('totalRecordsCount') totalRecordsCount: number = 0;
    @Input('pagingDetail') pagingDetail: IPagingDetail;

    private totalPageCount: number = 0;
    private activePage: number = 1;
    private actualNumberOfPagesForPageList = 0;
    private pagesToDisplay: number[];
    @Output('pageNumberChangedEvent') pageNumberChangedEvent = new EventEmitter<pageNumberChangedEventArg>();
    private pagesList = [];
    private showGoToFirstPage: boolean = false;
    private showGoToLastPage: boolean = false;
    

    ngOnInit(): void {
        if (this.pagingDetail) {
        //    this.pagingDetail = <IPagingDetail>JSON.parse(this.paging);

        //    //To set default numberOfPagesOnPageList
            this.pagingDetail.numberOfPagesOnPageList = !this.pagingDetail.numberOfPagesOnPageList ||
                this.pagingDetail.numberOfPagesOnPageList == 0 ?
                5 : this.pagingDetail.numberOfPagesOnPageList;

        //    this.loadPage(this.activePage);

        //    if (this.dataSource && this.dataSource.length) {
        //        this.totalPageCount = Math.floor(this.dataSource.length / this.pagingDetail.pageSize) +
        //            ((this.dataSource.length % this.pagingDetail.pageSize) > 0 ? 1 : 0);

        //        this.actualNumberOfPagesForPageList = this.totalPageCount > this.pagingDetail.numberOfPagesOnPageList ?
        //            this.pagingDetail.numberOfPagesOnPageList :
        //            this.totalPageCount;

        //        this.identifyPageNumbers(this.activePage);
        //    }
        }
    }

    // note: setter for pagingDetail
    public setPagingDetail(pagingDetail: IPagingDetail) {
        this.pagingDetail = pagingDetail;
    }

    refreshPaginator = (newTotalRecordsCount: number, newActivePage: number) => {
        this.totalRecordsCount = newTotalRecordsCount;
        this.activePage = newActivePage;
        if (newTotalRecordsCount > 0) {
            this.totalPageCount = Math.floor(newTotalRecordsCount / this.pagingDetail.pageSize) +
                ((newTotalRecordsCount % this.pagingDetail.pageSize) > 0 ? 1 : 0);

            this.actualNumberOfPagesForPageList = this.totalPageCount > this.pagingDetail.numberOfPagesOnPageList ?
                this.pagingDetail.numberOfPagesOnPageList :
                this.totalPageCount;

            this.identifyPageNumbers(this.activePage);
        }

        this.loadPage(this.activePage);
    }

    loadPage(selectedPage: number) {
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
    }

    //To find the page numbers to be displayed
    identifyPageNumbers(selectedPage: number) {
        this.pagesList = [];
        let leftToCenter = [];
        let rightToCenter = [];

        for (var i = 1, j = selectedPage - 1; i <= this.actualNumberOfPagesForPageList && j >= 1; i++ , j--) {
            leftToCenter.push(j)
        }

        for (var i = 1, j = selectedPage + 1; i <= this.actualNumberOfPagesForPageList && j <= this.totalPageCount; i++ , j++) {
            rightToCenter.push(j)
        }

        leftToCenter = leftToCenter.sort((a,b) => a - b);
        rightToCenter = rightToCenter.sort((a,b) => a - b).reverse();

        this.pagesList.push(selectedPage);
        for (var i = 1; i <= this.actualNumberOfPagesForPageList; i++) {
            if (this.pagesList.length >= this.actualNumberOfPagesForPageList) {
                break;
            }

            let page: number = 0;

            if (i % 2) {
                page = leftToCenter.pop();
                if (!page) {
                    page = rightToCenter.pop();
                }
            } else {
                page = rightToCenter.pop();
                if (!page) {
                    page = leftToCenter.pop();
                }
            }
            if (page) {
                this.pagesList.push(page);
            }
        }

        this.pagesList.sort((a,b) => a - b);

        this.showGoToFirstPage = this.actualNumberOfPagesForPageList == this.pagingDetail.numberOfPagesOnPageList &&
            this.pagesList.indexOf(1) === -1;

        this.showGoToLastPage = this.actualNumberOfPagesForPageList == this.pagingDetail.numberOfPagesOnPageList &&
            this.pagesList.indexOf(this.totalPageCount) === -1;
    }
}