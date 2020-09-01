import 'core-js';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { EmployeeDirectoryBlock } from "./EmployeeDirectoryBlock";
import { Dialog as KendoDialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Pagination } from "../../Common/Pagination/Pagination";
import { AlphabetFilter } from "../../Common/AlphabetFilter/AlphabetFilter";
import { ExportFile } from "../../Common/ExportFile/ExportFile";


declare var window: any;
declare var $: any;

interface IEmployeeDirectoryProps {
    exportUrl: string;
    dataUrl: string;
}
interface IEmployeeDirectoryState {
    employeeList: any[];
    recordCount: number;
    searchValue: string;
    skip: number;
    pageSize: number;
    filterBy: string;
    activeItem: number;
    dataUrl: string;
}
export class EmployeeDirectory extends React.Component<IEmployeeDirectoryProps, IEmployeeDirectoryState>{
    searchTextBox: any;

    constructor(props: IEmployeeDirectoryProps) {
        super(props);
        this.state = {
            employeeList: [],
            recordCount: 0,
            searchValue: '',
            skip: 0,
            pageSize: 12,
            filterBy: 'All',
            activeItem: 1,
            dataUrl: ""
        };
        this.onClickFilter = this.onClickFilter.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onPageChange = this.onPageChange.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onKeypress = this.onKeypress.bind(this);
        this.onClear = this.onClear.bind(this);
        this.resetPage = this.resetPage.bind(this);
    }

    resetPage() {
        this.setState({ activeItem: 1, skip: 0 });
    }

    onClickFilter(val: any) {
        this.resetPage();
        this.setState({ searchValue: this.state.searchValue, skip:0 });
        this.setState({ filterBy: val }, this.loadData);
    }
    onSearch(e: any) {
        this.resetPage();
        this.setState({ filterBy: "All" });
        this.setState({ searchValue: this.searchTextBox.value }, this.loadData)
    }
    onPageChange(pageNumber: any, offset: number) {
        this.setState({ activeItem: pageNumber });
        this.setState({ skip: offset }, this.loadData);
    }
    onPageSizeChange(e: any) {
        this.setState({ pageSize: parseInt(e.target.value) }, this.loadData)
    }
    renderBlock() {
        let items: any[] = [];
        for (let i in this.state.employeeList) {
            items.push(<EmployeeDirectoryBlock key={i} properties={this.state.employeeList[i]} />)
        }
        return items;
    }
    onKeypress(e: any) {
        if (e.keyCode === 13) {
            this.resetPage();
            this.setState({ filterBy: "All" })
            this.setState({ searchValue: this.searchTextBox.value }, this.loadData)
        }
    }
    onClear() {
        this.setState({ filterBy: "All" })
        this.searchTextBox.value = "";
        this.resetPage();
        this.setState({ searchValue: "" }, this.loadData)
    }
    loadData() {
        let sender = this;
        var url = this.props.dataUrl + "?searchValue=" + this.state.searchValue + "&take=" + this.state.pageSize + "&skip=" + this.state.skip + "&dir=" + "asc" + "&filterBy=" + this.state.filterBy;
        this.setState({ dataUrl: url });
        Remote.get(url, (data: any) => {
            sender.setState({ employeeList: data.data, recordCount: data.count });
        }, (error: string) => { alert(error) });
    }
    componentDidMount() {
        this.loadData();
        this.resetPage();
    }
    render() {
        return (
            <div className="">
                <div className="row">
                    <div className="col">
                        <div className="search-form-r">
                            <div className="input-group  mr-3">
                                <input defaultValue={this.state.searchValue} type='text' onChange={this.onKeypress} onKeyDown={this.onKeypress} placeholder="Search By Firstname, Lastname, Email" ref={(c) => { this.searchTextBox = c; }} className="form-control k-textbox" />
                                <div className="input-group-append" onClick={this.onSearch}>
                                    <div className="input-group-text">
                                        <a href="javascript:void(0)"  className="k-icon k-i-search">&nbsp;</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="clearfix d-block d-md-none"></div>
                        <div className="mt-3 mt-md-0">
                            <AlphabetFilter filterBy={this.state.filterBy} onClickFilter={this.onClickFilter} />
                        </div>
                    </div>
                    <div className="col-md-auto col-sm-12 mt-3 mt-md-0">
                        <ExportFile exportURL={this.state.dataUrl} columnKey="User" />
                    </div>
                </div>
                <div className="row">
                    {this.state.searchValue !== "" && <div className="col search-pills-container">
                        <div className="badge badge-pill badge-secondary">
                            {this.state.searchValue} <a href="javascript:void(0)" onClick={this.onClear} id="clearSearch" className="pill-close"><i className="material-icons" >close</i></a>
                        </div>
                    </div>}
                </div>
                <div className="row employee-directory k-grid-container">
                    {this.state.employeeList.length ? this.renderBlock() : <div className="not-found-message text-center w-100"><i className="k-icon k-i-user"></i><h4 className="mt-0 text-muted"><b>Oops!</b> Can't find employee on your filter criteria.</h4></div>}
                </div>
                <Pagination totalCount={this.state.recordCount} pageSize={this.state.pageSize} activePage={this.state.activeItem} onPageChange={this.onPageChange} onPageSizeChange={this.onPageSizeChange} />
            </div>
        );
    }
}