import 'core-js';
import * as React from "react";
import { Remote } from "../../Common/Remote/Remote";
import SpinnerPage from "../FileUpload/SpinnerPage";
import { Guid } from "guid-typescript";

declare var window: any;
declare var $: any;

interface IfarClauseProps {
    dataUrl: string;
    farClauseGuid: any;
    crudType: any;
}
interface IfarClauseState {
    farClauseList: any[];
    farContractTypeList: any[];
    dataUrl: string;
    selected: boolean;
    selectedVal: any[];
    showLoading: boolean;
}
export class FarClause extends React.Component<IfarClauseProps, IfarClauseState>{
    searchTextBox: any;

    constructor(props: IfarClauseProps) {
        super(props);
        this.state = {
            farClauseList: [],
            farContractTypeList: [],
            dataUrl: "",
            selected: false,
            selectedVal: [],
            showLoading: false,
        };
        this.onChangeOption = this.onChangeOption.bind(this);
        this.onChangeFarContractType = this.onChangeFarContractType.bind(this);

    }

    loadData() {
        let sender = this;
        this.setState({
            showLoading: true
        })

        if (this.props.crudType === 'add') {
            Remote.get("/Admin/FarContractType/GetAll",
                (response: any) => {
                    sender.setState({ farContractTypeList: response.result, showLoading: false })
                },
                (err: any) => { window.Dialog.alert(err) });
        }
        else {
            Remote.get("/Admin/FarContractTypeClause/GetFarContractTypeByFarClauseId?farClauseId=" + this.props.farClauseGuid,
                (response: any) => {
                    sender.setState({ farContractTypeList: response.result, showLoading: false })
                },
                (err: any) => { window.Dialog.alert(err) });
        }
    }

    componentDidMount() {
        this.loadData();
    }

    async onChangeOption(e: any, id: any) {

        this.setState({
            showLoading: true
        })

        var data = {
            farClauseGuid: id,
            IsApplicable: false,
            IsRequired: false,
            IsOptional: false
        };

        if (e.target.value === 'IsApplicable') {
            data.IsApplicable = true;
        } else if (e.target.value === 'IsRequired') {
            data.IsRequired = true;
        } else if (e.target.value === 'IsOptional') {
            data.IsOptional = true;
        }

        let result = await Remote.postDataAsync("/Admin/farClause/Edit", data);
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

    onChangeFarContractType(e: any, index: any) {
        let arrVal = this.state.farContractTypeList;
        arrVal[index].selectedValue = e.target.value;

        this.setState({
            farContractTypeList: arrVal
        })
    }


    render() {
        return (
            <div className="">
                {this.state.showLoading && <SpinnerPage />}
                {this.state.farContractTypeList.length > 0 && <div>
                    {/* <h4 className="border-bottom pb-1 mb-3">Far Contract Types</h4> */}
                    <div className="farClause k-grid-container">
                        <div className="row form-group mt-2 align-items-center border-bottom border-top pb-1 pt-2">
                            <div className="col-sm-3 col-md-4"><h6 className="text-muted mb-0">Far Contract Types</h6></div>
                            <div className="col-sm-9 col-md-8">
                                <div className="row text-center">
                                    <div className="col-3 control-label text-muted mb-0">Required</div>
                                    <div className="col-3 control-label text-muted mb-0">Applicable</div>
                                    <div className="col-3 control-label text-muted mb-0">Optional</div>
                                    <div className="col-3 control-label text-muted mb-0">Not Applicable</div>
                                </div>
                            </div>
                        </div>
                        {this.state.farContractTypeList.map((farContractType: any, index: any) => {
                            let newId1 = "far1" + Guid.create();
                            let newId2 = "far2" + Guid.create();
                            let newId3 = "far3" + Guid.create();
                            let newId4 = "far4" + Guid.create();
                            return (
                                <div className="row form-group" key={index}>
                                    <div className="col-sm-3 col-md-4">
                                        <b><label className="">{farContractType.title} ({farContractType.code})</label></b>
                                    </div>
                                    <div className="col-sm-9 col-md-8">
                                        <div className="row text-center">
                                            <div className="col-3">
                                                <input type='radio' name={farContractType.displayCode} value="Required" className="k-radio" id={newId1} disabled={this.props.crudType === 'detail'}
                                                    checked={farContractType.selectedValue === 'Required'} onChange={(e) => {
                                                        this.onChangeFarContractType(e, index);
                                                    }} />
                                                <label htmlFor={newId1} className="k-radio-label"></label>
                                            </div>
                                            <div className="col-3">
                                                <input type='radio' name={farContractType.displayCode} value="Applicable" className="k-radio" id={newId2} disabled={this.props.crudType === 'detail'}
                                                    checked={farContractType.selectedValue === 'Applicable'} onChange={(e) => {
                                                        this.onChangeFarContractType(e, index);
                                                    }} />
                                                <label htmlFor={newId2} className="k-radio-label"></label>
                                            </div>
                                            <div className="col-3">
                                                <input type='radio' name={farContractType.displayCode} value="Optional" className="k-radio" id={newId3} disabled={this.props.crudType === 'detail'}
                                                    checked={farContractType.selectedValue === 'Optional'} onChange={(e) => {
                                                        this.onChangeFarContractType(e, index);
                                                    }} />
                                                <label htmlFor={newId3} className="k-radio-label"></label>
                                            </div>
                                            <div className="col-3">
                                                <input type='radio' name={farContractType.displayCode} value="NotApplicable" className="k-radio" id={newId4} disabled={this.props.crudType === 'detail'}
                                                    checked={farContractType.selectedValue === '' || farContractType.selectedValue === 'NotApplicable' || farContractType.selectedValue === undefined} onChange={(e) => {
                                                        this.onChangeFarContractType(e, index);
                                                    }} />
                                                <label htmlFor={newId4} className="k-radio-label"></label>
                                            </div>
                                        </div>
                                    </div>
                                    {/*this.state.showErrors && <div className="text-danger">{this.state.errorTextFarClause}</div> */}
                                </div>
                            );
                        })}

                        {/*this.props.crudType === 'detail' && this.state.farContractTypeList.map((farContractType: any, index: any) => {
                            let newId1 = "far1" + Guid.create();
                            let newId2 = "far2" + Guid.create();
                            let newId3 = "far3" + Guid.create();
                            return (
                                <div className="row form-group" key={index}>
                                    <div className="col-sm-3 col-md-2">
                                        <b><label className="">{farContractType.code}</label></b>
                                    </div>
                                    <div className="col">
                                        <input type='radio' name={farContractType.displayCode} value={farContractType.selectedValue} className="k-radio" id={newId1}
                                            checked={farContractType.selectedValue === 'Required'} onChange={(e) => {
                                                this.onChangeFarContractType(e, index);
                                            }} />
                                        <label htmlFor={newId1} className="k-radio-label"></label>

                                        <input type='radio' name={farContractType.displayCode} value={farContractType.selectedValue} className="k-radio" id={newId2}
                                            checked={farContractType.selectedValue === 'Applicable'} onChange={(e) => {
                                                this.onChangeFarContractType(e, index);
                                            }} />
                                        <label htmlFor={newId2} className="k-radio-label"></label>

                                        <input type='radio' name={farContractType.displayCode} value="IsOptional" className="k-radio" id={newId3}
                                            checked={farContractType.selectedValue === 'Optional'} onChange={(e) => {
                                                this.onChangeFarContractType(e, index);
                                            }} />
                                        <label htmlFor={newId3} className="k-radio-label"></label>
                                    </div>
                                </div>
                            );
                        }) */}
                    </div>
                </div>
                }
            </div>
        );
    }
}