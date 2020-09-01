import {Component, ElementRef, Input, OnInit, OnChanges, SimpleChange,
    ViewEncapsulation, NgZone} from '@angular/core';
import {Subject} from "rxjs/Subject";
import {Observable} from "rxjs/Observable";
import {AutoCompleteService} from './ac-autocomplete.service';

/**
 * show a selected date in monthly calendar
 * Each filteredList item has the following property in addition to data itself
 *   1. displayValue as string e.g. Allen Kim
 *   2. dataValue as any e.g. 1234
 */
@Component({
    providers: [AutoCompleteService],
    selector: 'ac-autocomplete',
    templateUrl: 'app/lib/aclibs/ac-autocomplete/ac-autocomplete.component.html',
    styleUrls: ['app/lib/aclibs/ac-autocomplete/ac-autocomplete.component.css'],
    //encapsulation: ViewEncapsulation.Native
    encapsulation: ViewEncapsulation.None
    // encapsulation: ViewEncapsulation.Emulated is default
})
export class AutoCompleteComponent implements OnInit, OnChanges {

    /**
     * public variables
     */
    @Input('list-formatter') listFormatter: (arg: any) => void;
    @Input('source') source: any;
    @Input('path-to-data') pathToData: string;
    @Input('min-chars') minChars: number = 0;
    @Input('value-property-name') valuePropertyName: string = 'id';
    @Input('display-property-name') displayPropertyName: string = 'value';
    @Input('placeholder') placeholder: string;
    @Input('dataSource') sourceAction: (keyword: string) => Observable<any[]>;

    public el: HTMLElement;
    public inputEl: HTMLInputElement;

    public dropdownVisible: boolean = false;
    public isLoading: boolean = false;
    public filteredList: any[] = [];
    public itemIndex: number = -1;
    public keyword: string;
    private isCancelled = false;

    zone: NgZone;

    public valueSelectedEvent: Subject<any> = new Subject();
    /**
     * constructor
     */
    constructor(
        elementRef: ElementRef,
        public autoComplete: AutoCompleteService
    ) {
        this.el = elementRef.nativeElement;
        this.zone = new NgZone({ enableLongStackTrace: false });
    }

    /**
     * user enters into input el, shows list to select, then select one
     */
    ngOnInit(): void {
        this.autoComplete.source = this.source;
        this.autoComplete.pathToData = this.pathToData;
    }

    ngOnChanges(changes: { [propertyName: string]: SimpleChange }) {
        for (let propName in changes) {
            if (propName === 'filteredList') {
                alert('coming');
                this.isLoading = false;
            }
        }
    }

    reloadListInDelay(searchText: string): void {
        let delayMs = this.source.constructor.name == 'Array' ? 10 : 500;
        this.dropdownVisible = false;
        
        //executing after user stopped typing
        this.delay(() => this.reloadList(searchText), delayMs);
    }

    //showDropdownList(): void {
    //    this.keyword = '';

    //    this.reloadList(this.keyword);
    //    //this.inputEl.focus();
    //}

    hideDropdownList(): void {
        this.dropdownVisible = false;
    }

    reloadList(searchText: string): void {
        //let keyword = this.inputEl.value;
        if (this.isCancelled) {
            this.isLoading = false;
            this.isCancelled = false;
            return;
        }

        this.isLoading = true;
        let keyword = searchText;
        if (keyword.length >= this.minChars) {
            this.filteredList = [];
            let query = { keyword: keyword };
            this.dropdownVisible = true;
            this.sourceAction(keyword)
                .subscribe(
                resp => {
                    this.zone.run(() => {
                        this.filteredList = resp;
                        if (!this.filteredList || !this.filteredList.length || this.isCancelled) {
                            this.dropdownVisible = false;
                            this.isCancelled = false;
                        }

                        this.isLoading = false;
                    });
                },
                error => null,
                () => this.isLoading = false //complete
                );
        }
    }

    selectOne(data: any) {
        this.hideDropdownList();
        this.itemIndex = -1;
        this.valueSelectedEvent.next(data);
    };

    inputElKeyHandler(evt: any) {

        let totalNumItem = !this.filteredList ? 0 : this.filteredList.length;

        switch (evt.keyCode) {
            case 27: // ESC, hide auto complete
                this.hideDropdownList();
                break;

            case 38: // UP, select the previous li el
                this.itemIndex = (totalNumItem + this.itemIndex - 1) % totalNumItem;
                break;

            case 40: // DOWN, select the next li el or the first one
                this.dropdownVisible = true;
                this.itemIndex = (totalNumItem + this.itemIndex + 1) % totalNumItem;
                break;

            case 13: // ENTER, choose it!!
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

    getFormattedList(data: any): string {
        let formatter = this.listFormatter || this.defaultListFormatter;
        return formatter.apply(this, [data]);
    }

    private defaultListFormatter(data: any): string {
        let html: string = "";
        html += data[this.displayPropertyName] ? `<span>${data[this.displayPropertyName]}</span>` : data;
        html += data[this.valuePropertyName] ? ` (${data[this.valuePropertyName]})` : "";
        return html;
    }

    private delay = (function () {
        var timer = 0;
        return function (callback: any, ms) {
            clearTimeout(timer);
            timer = setTimeout(callback, ms);
        };
    })();

}