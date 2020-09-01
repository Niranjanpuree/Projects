import * as React from "react"
import * as ReactDOM from "react-dom"
import { Input, Switch } from "@progress/kendo-react-inputs";
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Remote } from "../../Common/Remote/Remote";
import { OfficeDirectoryBlock } from "./OfficeDirectoryBlock";
import { KendoGrid } from "../../Common/Grid/KendoGrid";
import { Pagination } from "../../Common/Pagination/Pagination";
import { AlphabetFilter } from "../../Common/AlphabetFilter/AlphabetFilter";
import { ExportFile } from "../../Common/ExportFile/ExportFile";
import { StrokeDasharrayProperty } from "csstype";

declare var window: any;
declare var numberWithCommas: any;
declare var $: any;
declare var Dialog: any;

interface IOfficeProps {
    dataUrl: string;
    exportUrl: string;
}

interface IOfficeState {
    officeList: any[];
    recordCount: number;
    searchValue: string;
    skip: number;
    pageSize: number;
    filterBy: string;
    activeItem: number;
    dataUrl: string;
}

export class OfficeDirectory extends React.Component<IOfficeProps, IOfficeState> {
    searchTextBox: any;
    pagination: Pagination;
    export: ExportFile;
    constructor(props: IOfficeProps) {
        super(props);
        this.state = {
            officeList: [],
            recordCount: 0,
            searchValue: '',
            skip: 0,
            pageSize: 12,
            filterBy: 'All',
            activeItem: 1,
            dataUrl: "",
        };
        this.onSearch = this.onSearch.bind(this);
        this.onPageChange = this.onPageChange.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onClickFilter = this.onClickFilter.bind(this);
        this.onClear = this.onClear.bind(this);
        this.onKeypress = this.onKeypress.bind(this);
        this.resetPage = this.resetPage.bind(this);
    }

    renderBlock() {
        let items: any[] = [];
        for (let i in this.state.officeList) {
            items.push(<OfficeDirectoryBlock key={i} properties={this.state.officeList[i]} />)
        }
        return items;
    }

    resetPage() {
        this.setState({ activeItem: 1, skip : 0 });
    }

    child() {
        <ExportFile exportURL={this.state.dataUrl} columnKey="Office" />
    }

    onPageChange(pageNumber: any, offset: number) {
        this.setState({ activeItem: pageNumber });
        this.setState({ skip: offset }, this.loadData);
    }

    onSearch(e: any) {
        this.resetPage();
        this.setState({ filterBy: "All"})
        this.setState({ searchValue: this.searchTextBox.value }, this.loadData)
    }

    onClear() {
        this.setState({ filterBy: "All" })
        this.searchTextBox.value = "";
        this.setState({ searchValue: "" }, this.loadData)
    }


    onKeypress(e:any) {
        if (e.keyCode === 13) {
            this.setState({ filterBy: "All" })
            this.setState({ searchValue: this.searchTextBox.value }, this.loadData);
            this.resetPage();
        }
    }

    loadData() {
        let $this = this;
        let url = this.props.dataUrl + "?searchValue=" + this.state.searchValue + "&filterBy=" + this.state.filterBy + "&pageSize=" + this.state.pageSize + "&skip=" + this.state.skip + "&sortField=&sortDirection=asc";
        this.setState({ dataUrl: url });
        Remote.get(url, (data: any) => {
            $this.setState({ officeList: data.data, recordCount: data.total });
        }, (error: string) => { alert(error) });
    }

    onPageSizeChange(e: any) {
        this.setState({ pageSize: parseInt(e.target.value) }, this.loadData)
    }

    onClickFilter(val: any, parent: any) {
        parent.setState({ activeItem: 1, skip: 0 })
        this.resetPage();
        parent.setState({ filterBy: val }, this.loadData);
       }

    componentDidMount() {
        this.loadData();
        this.resetPage();
    }

    render() {
        return (
            <div>
                <div className="row">
                    <div className="col">
                        <div className="search-form-r">
                            <div className="input-group mr-3">
                                <input defaultValue={this.state.searchValue} type='text' onChange={this.onKeypress} onKeyDown={this.onKeypress} placeholder="Search" ref={(c) => { this.searchTextBox = c; }} className="form-control k-textbox" />
                                <div className="input-group-append" onClick={this.onSearch}>
                                    <div className="input-group-text">
                                        <a href="javascript:void(0)" className="k-icon k-i-search">&nbsp;</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                       
                        <div className="clearfix d-block d-md-none"></div>
                        <div className="mt-3 mt-md-0">
                            <AlphabetFilter filterBy={this.state.filterBy} parent={this} onClickFilter={this.onClickFilter} />
                        </div>
                    </div>
                    <div className="col-md-auto col-sm-12 mt-3 mt-md-0">
                        <ExportFile exportURL={this.state.dataUrl} columnKey="Office" />
                    </div>
                    
                </div>
                <div className="row">
                    {this.state.searchValue !== "" && <div className="col search-pills-container">
                        <div className="badge badge-pill badge-secondary">
                            {this.state.searchValue} <a href="javascript:void(0)" onClick={this.onClear} id="clearSearch" className="pill-close"><i className="material-icons" >close</i></a>
                        </div>
                    </div>}
                </div>


                <div className="row office-directory k-grid-container">
                    {this.state.officeList.length ? this.renderBlock() : <p className="col-sm-12 text-center">No Records</p>}
                </div>
                <Pagination totalCount={this.state.recordCount} pageSize={this.state.pageSize} activePage={this.state.activeItem} onPageChange={this.onPageChange} onPageSizeChange={this.onPageSizeChange} />
            </div>
        );
    }

}