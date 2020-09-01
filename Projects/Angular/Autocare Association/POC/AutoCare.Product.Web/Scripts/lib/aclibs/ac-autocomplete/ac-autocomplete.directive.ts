import {
    Directive,
    Input,
    Output,
    ComponentRef,
    ViewContainerRef,
    EventEmitter,
    OnInit,
    OnDestroy,
    ViewChild,
    Component,
    ComponentFactory,
    ComponentFactoryResolver
} from '@angular/core';
import {AutoCompleteComponent} from "./ac-autocomplete.component";
import { Observable } from "rxjs/Rx";

/**
 * display auto-complete section with input and dropdown list when it is clicked
 */
@Directive({
    selector: '[ac-autocomplete]',
    host: {
        '(input)': 'inputEventHandler($event)',
        '(keydown)': 'keydownEventHandler($event)',
    }
})
export class AutoCompleteDirective implements OnInit, OnDestroy {
    @Input() placeholder: string;
    @Input('list-formatter') listFormatter: (arg: any) => void;
    //@Input('prefill-func') prefillFunc: function;
    @Input('value-changed') valueChanged: (value: any) => void;
    @Input('source') source: any;
    @Input('min-chars') minChars: number = 0;
    @Input('path-to-data') pathToData: string;
    @Input('value-property-name') valuePropertyName: string;
    @Input('display-property-name') displayPropertyName: string;
    @Input('dataSourceMethod') sourceAction: (keyword: string) => Observable<any[]>;

    @Input() ngModel: String;
    @Output() keywordChanged = new EventEmitter();

    autoCompleteComp: AutoCompleteComponent

    public componentRef: ComponentRef<AutoCompleteComponent>;
    public el: HTMLInputElement;   // input or select element
    public acEl: HTMLElement; // auto complete element
    private autoCompleteDiv: HTMLElement;
    private popupBottom: number = 0;
    public keyword: string = '';
    constructor(
        private resolver: ComponentFactoryResolver,
        public viewContainerRef: ViewContainerRef
    ) {
        this.el = <HTMLInputElement>this.viewContainerRef.element.nativeElement;
    }

    ngOnInit() {
        if (this.el.parentElement !== this.autoCompleteDiv) {
            this.autoCompleteDiv = document.createElement("div");
            this.autoCompleteDiv.className = 'ng2-auto-complete';
            this.autoCompleteDiv.style.display = 'inline-block';
            this.autoCompleteDiv.style.position = 'relative';
            this.el.parentElement.insertBefore(this.autoCompleteDiv, this.el.nextSibling);
            this.autoCompleteDiv.appendChild(this.el);
        }

        this.showAutoComplete();
    }

    ngOnDestroy() {
        this.hideAndDestroyAutoComplete();
    }

    inputEventHandler($event) {
        var inputElement = <HTMLInputElement>$event.target;
        if (this.autoCompleteComp) {
            this.autoCompleteComp.reloadListInDelay(inputElement.value);
        }
    }

    keydownEventHandler($event) {
        if (this.autoCompleteComp) {
            this.autoCompleteComp.inputElKeyHandler($event);
        }
    }

    //show auto-complete list below the current element
    showAutoComplete = () => {
        let factory = this.resolver.resolveComponentFactory(AutoCompleteComponent);
        this.componentRef = this.viewContainerRef.createComponent(factory);

        this.acEl = this.componentRef.location.nativeElement;

        this.autoCompleteComp = this.componentRef.instance;
        this.autoCompleteComp.listFormatter = this.listFormatter;
        //component.prefillFunc = this.prefillFunc;
        this.autoCompleteComp.pathToData = this.pathToData;
        this.autoCompleteComp.minChars = this.minChars;
        this.autoCompleteComp.valuePropertyName = this.valuePropertyName || 'id';
        this.autoCompleteComp.displayPropertyName = this.displayPropertyName || 'value';
        this.autoCompleteComp.source = this.source;
        this.autoCompleteComp.sourceAction = this.sourceAction;
        this.autoCompleteComp.placeholder = this.placeholder;

        this.autoCompleteComp.valueSelectedEvent.subscribe((val: any) => {
            if (typeof val === 'undefined') {
                this.keywordChanged.emit(this.el.value);
            } else if (typeof val === "string") {
                this.keywordChanged.emit(val);
            } else {
                let displayVal = val[this.autoCompleteComp.displayPropertyName];
                this.el.value = displayVal;
                this.keywordChanged.emit(displayVal);
            }

            if (this.valueChanged) {
                this.valueChanged(val);
            }
        });

        setTimeout(() => { // it needs time to run ngOnInit within component
            /* setting width/height auto complete */
            let thisElBCR = this.el.getBoundingClientRect();
            let offsetHeight = this.el.offsetHeight
            this.acEl.style.width = thisElBCR.width + 'px';
            //this.acEl.style.height = "38px";
            this.acEl.style.position = 'absolute';
            this.acEl.style.zIndex = '1';
            this.acEl.style.top = this.el.offsetHeight + this.el.offsetTop + 2 + 'px';
            this.acEl.style.left = this.el.offsetLeft + 'px';
        });

        document.addEventListener('click', event => {
            if (event.target !== this.el && event.target !== this.acEl) {
                this.autoCompleteComp.hideDropdownList();
            }
        });
    }

    hideAndDestroyAutoComplete(): void {
        if (this.el.parentElement === this.autoCompleteDiv) {
            let autoCompleteDivPatent = this.el.parentElement.parentElement;
            autoCompleteDivPatent.insertBefore(this.el, this.autoCompleteDiv);
            autoCompleteDivPatent.removeChild(this.autoCompleteDiv);
        }

        if (this.componentRef) {
            this.componentRef.destroy();
        }
    }
}