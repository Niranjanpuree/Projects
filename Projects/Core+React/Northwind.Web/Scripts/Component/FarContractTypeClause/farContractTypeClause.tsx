import 'core-js';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { Dialog as KendoDialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Pagination } from "../../Common/Pagination/Pagination";
import { AlphabetFilter } from "../../Common/AlphabetFilter/AlphabetFilter";
import { FarClause } from './farClauseList';
import SpinnerPage from "../FileUpload/SpinnerPage";
import { guid } from '@progress/kendo-react-common';
import { Guid } from "guid-typescript";
import { FarContractTypeClauseDialog } from './farContractTypeClauseDialog';



declare var window: any;
declare var $: any;

interface IFarContractTypeClauseProps {
    dataUrl: string;
}
interface IFarContractTypeClauseState {
    farClauseList: any[];
    farContractTypeList: any[];
    recordCount: number;
    searchValue: string;
    skip: number;
    pageSize: number;
    filterBy: string;
    activeItem: number;
    dataUrl: string;
    selected: boolean;
    showLoading: boolean;
    openAddDialog: boolean;
}
export class FarContractTypeClause extends React.Component<IFarContractTypeClauseProps, IFarContractTypeClauseState>{
    searchTextBox: any;

    constructor(props: IFarContractTypeClauseProps) {
        super(props);
        this.state = {
            farClauseList: [],
            farContractTypeList: [],
            recordCount: 0,
            searchValue: '',
            skip: 0,
            pageSize: 12,
            filterBy: 'All',
            activeItem: 1,
            dataUrl: "",
            selected: false,
            showLoading: false,
            openAddDialog: false
        };
        this.onClickFilter = this.onClickFilter.bind(this);
        this.onSearch = this.onSearch.bind(this);
        this.onPageChange = this.onPageChange.bind(this);
        this.onPageSizeChange = this.onPageSizeChange.bind(this);
        this.onKeypress = this.onKeypress.bind(this);
        this.onClear = this.onClear.bind(this);
        this.resetPage = this.resetPage.bind(this);
        this.onChangeOption = this.onChangeOption.bind(this);
        this.onOpenAddDialog = this.onOpenAddDialog.bind(this);
        this.onCloseDialog = this.onCloseDialog.bind(this);
    }

    resetPage() {
        this.setState({ activeItem: 1, skip: 0 });
    }

    onClickFilter(val: any) {
        this.resetPage();
        this.setState({ searchValue: this.state.searchValue, skip: 0 });
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
        this.setState({
            showLoading: true
        })

        let sender = this;
        var url = this.props.dataUrl + "?searchValue=" + this.state.searchValue + "&take=" + this.state.pageSize + "&skip=" + this.state.skip + "&dir=" + "asc" + "&filterBy=" + this.state.filterBy;
        this.setState({ dataUrl: url });
        Remote.get(url, (data: any) => {
            sender.setState({ farClauseList: data.result, recordCount: data.count, showLoading: false });
        }, (error: string) => { alert(error) });
    }

    renderBlock() {
        let items: any[] = [];
        for (let i in this.state.farClauseList) {
            items.push(<FarClause key={i} farClauseList={this.state.farClauseList[i]} />)
        }
        return items;
    }

    componentDidMount() {
        this.loadData();
        this.resetPage();
    }

    async onChangeOption(e: any, id: any) {

        this.setState({
            showLoading: true
        })

        var data = {
            FarContractTypeClauseGuid: id,
            IsApplicable: false,
            IsRequired: false,
            IsOptional: false
        };

        if (e.target.value === 'IsApplicable') {
            data.IsApplicable = true;
        } else if (e.target.value === 'IsRequired') {7
            data.IsRequired = true;
        } else if (e.target.value === 'IsOptional') {
            data.IsOptional = true;
        }

        let result = await Remote.postDataAsync("/Admin/FarContractTypeClause/Edit", data);
        if (result.ok) {
            this.setState({
                showLoading: false
            })
        }
        else {
            let error = JSON.parse(await result.text());
            window.Dialog.alert(error[""].errors[0].errorMessage, "Error");
        }
    }

    onOpenAddDialog() {
        this.setState({
            openAddDialog: true
        })
    }

    onCloseDialog() {
        this.setState({
            openAddDialog: false
        })
    }

    render() {
        return (
            <div className="">
                {this.state.showLoading && <SpinnerPage />}
                <div className="row">
                    <div className="col">
                        <div className="search-form-r">
                            <div className="input-group  mr-3">
                                <input defaultValue={this.state.searchValue} type='text' onChange={this.onKeypress} onKeyDown={this.onKeypress} placeholder="Search" ref={(c) => { this.searchTextBox = c; }} className="form-control k-textbox" />
                                <div className="input-group-append" onClick={this.onSearch}>
                                    <div className="input-group-text">
                                        <a href="javascript:void(0)" className="k-icon k-i-search">&nbsp;</a>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div className="col-auto">
                        <button className="btn btn-primary" onClick={() => { this.onOpenAddDialog() }}>Add New Far Contract Type Clause</button>
                    </div>
                </div>
                <div className="row">
                    {this.state.searchValue !== "" && <div className="col search-pills-container">
                        <div className="badge badge-pill badge-secondary">
                            {this.state.searchValue} <a href="javascript:void(0)" onClick={this.onClear} id="clearSearch" className="pill-close"><i className="material-icons" >close</i></a>
                        </div>
                    </div>}
                </div>
                <br />
                <div className="farClause k-grid-container">
                    <table className="table">
                        <thead>
                            <th>Far Clause Number</th>
                            <th>Title</th>
                            <th><table className="table text-center">
                                <thead>
                                    <th colSpan={4}>Type</th>
                                </thead>
                                <tr>
                                    <td>Name</td>
                                    <td>Is Required</td>
                                    <td>Is Applicable</td>
                                    <td>Is Optional</td>
                                </tr>
                            </table></th>
                        </thead>
                        <tbody>
                            {this.state.farClauseList.map((listValue, index) => {
                                return (
                                    <tr key={index}>
                                        <td>{listValue.number}</td>
                                        <td>{listValue.title}</td>
                                        <td>
                                            <table className="table text-center">
                                                {listValue.farContractTypeClauseViewModels.map((listFarWithClauseValue: any, index: any) => {
                                                    let newId1 = "far1" + Guid.create();
                                                    let newId2 = "far2" + Guid.create();
                                                    let newId3 = "far3" + Guid.create();
                                                    return (
                                                        <tr key={index}>
                                                            <td>{listFarWithClauseValue.farContractTypeCode}</td>
                                                            <td>
                                                                <input type='radio' name={index} value="IsRequired" className="k-radio" id={newId1}
                                                                    checked={listFarWithClauseValue.isRequired} onChange={(e) => {
                                                                        this.onChangeOption(e, listFarWithClauseValue.farContractTypeClauseGuid);
                                                                    }} />
                                                                <label htmlFor={newId1} className="k-radio-label"></label>
                                                            </td>
                                                            <td>
                                                                <input type='radio' name={index} value="IsApplicable" className="k-radio" id={newId2}
                                                                    checked={listFarWithClauseValue.isApplicable} onChange={(e) => {
                                                                        this.onChangeOption(e, listFarWithClauseValue.farContractTypeClauseGuid);
                                                                    }} />
                                                                <label htmlFor={newId2} className="k-radio-label"></label>
                                                            </td>
                                                            <td>
                                                                <input type='radio' name={index} value="IsOptional" className="k-radio" id={newId3}
                                                                    checked={listFarWithClauseValue.isOptional} onChange={(e) => {
                                                                        this.onChangeOption(e, listFarWithClauseValue.farContractTypeClauseGuid);
                                                                    }} />
                                                                <label htmlFor={newId3} className="k-radio-label"></label>
                                                            </td>
                                                        </tr>
                                                    );
                                                })}
                                            </table>
                                        </td>
                                    </tr>
                                );
                            })}
                        </tbody>
                    </table>
                </div>
                <Pagination totalCount={this.state.recordCount} pageSize={this.state.pageSize} activePage={this.state.activeItem} onPageChange={this.onPageChange} onPageSizeChange={this.onPageSizeChange} />
                {this.state.openAddDialog && <FarContractTypeClauseDialog closeDialogEvent={this.onCloseDialog} />}
            </div>
        );
    }
}