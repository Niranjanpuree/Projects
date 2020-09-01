import * as React from "react";
import * as ReactDOM from "react-dom";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Button } from "@progress/kendo-react-buttons";
import { Remote } from "../../Common/Remote/Remote";
import { DropDown } from "../../Common/DropDown/DropDown";
declare var window: any;
declare var document: any;

interface IfarContractTypeClauseDialogProps {
    closeDialogEvent: Function;
}

interface IfarContractTypeClauseDialogStates {
    farClauseList: any[];
    farContractTypeList: any[];
    farClauseGuid: any;
    farContractTypeGuid: any;
    isApplicable: boolean;
    isRequired: boolean;
    isOptional: boolean;
    notValid: boolean;
    showErrors: boolean;
    errorTextFarClause: string;
    errorTextFarContractType: string;
    errorTextOptions: string;
}

let farClauseDefaultItem = {
    number: 'Please Select Far Clause'
}

let farContractTypeDefaultItem = {
    code: 'Please Select Far Contract Type'
}

export class FarContractTypeClauseDialog extends React.Component<IfarContractTypeClauseDialogProps, IfarContractTypeClauseDialogStates> {
    dropDownFarClause: DropDown;
    dropDownFarContractType: DropDown;
    constructor(props: any) {
        super(props);
        this.onSubmitFiles = this.onSubmitFiles.bind(this);
        this.onChangeOption = this.onChangeOption.bind(this);
        this.onChangeFarClause = this.onChangeFarClause.bind(this);
        this.onChangeFarContractType = this.onChangeFarContractType.bind(this);

        this.onCancel = this.onCancel.bind(this);
        this.state = {
            farClauseList: [],
            farContractTypeList: [],
            farClauseGuid: null,
            farContractTypeGuid: null,
            isApplicable: false,
            isRequired: false,
            isOptional: false,
            notValid: false,
            showErrors: false,
            errorTextFarClause: '',
            errorTextFarContractType: '',
            errorTextOptions: ''
        }
    }

    componentDidMount() {
        let sender = this;
        Remote.get("/Admin/FarClause/GetAll",
            (response: any) => {
                sender.setState({ farClauseList: response.result })
            },
            (err: any) => { window.Dialog.alert(err) });

        Remote.get("/Admin/FarContractType/GetAll",
            (response: any) => {
                sender.setState({ farContractTypeList: response.result, notValid: true })
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    onSubmitFiles(e: any) {

        let submitData: boolean = true;

        //validate elements..

        this.setState({
            showErrors: false,
            errorTextFarClause: '',
            errorTextFarContractType: '',
            errorTextOptions: ''
        })

        if (this.state.farClauseGuid === null || this.state.farClauseGuid === farClauseDefaultItem.number) {
            this.setState({
                showErrors: true,
                errorTextFarClause: "Far Clause Field is required."
            })
            submitData = false;
        }

        if (this.state.farContractTypeGuid === null || this.state.farContractTypeGuid === farContractTypeDefaultItem.code) {
            this.setState({
                showErrors: true,
                errorTextFarContractType: "Far Contract Type Field is required."
            })
            submitData = false;
        }

        //if (!this.state.notValid) {
        //    if (!this.state.isApplicable && !this.state.isRequired && !this.state.isOptional) {
        //        this.setState({
        //            showErrors: true,
        //            errorTextOptions: "Please select any of the options."
        //        })
        //        submitData = false;
        //    }
        //}

        if (submitData) {
            var data = {
                FarClauseGuid: this.state.farClauseGuid,
                FarContractTypeGuid: this.state.farContractTypeGuid,
                IsApplicable: this.state.isApplicable,
                IsRequired: this.state.isRequired,
                IsOptional: this.state.isOptional
            };
            Remote.postData('/Admin/FarContractTypeClause/Add', data, (e: any) => { this.props.closeDialogEvent(); }, (err: any) => { window.Dialog.alert(err) });
        }
    }

    onChangeFarClause(e: any) {
        this.setState({
            farClauseGuid: e.target.value
        })
    }
    onChangeFarContractType(e: any) {
        this.setState({
            farContractTypeGuid: e.target.value
        })
    }

    onCancel(e: any) {
        this.props.closeDialogEvent();
    }

    onChangeOption(e: any) {
        this.setState({
            isApplicable: e.target.value === 'IsApplicable' ? true : false,
            isRequired: e.target.value === 'IsRequired' ? true : false,
            isOptional: e.target.value === 'IsOptional' ? true : false,
            notValid: e.target.value === 'NotValid' ? true : false,
        })
    }
    render() {

        return (<div>
            <Dialog title="Add Far Contract Type Clause" width="70%" height="auto" className="dialog-custom">
                <div>
                    {/*<div className="form-group">
                        <label className="">Far Clause</label>
                        <DropDown ref={(c) => this.dropDownFarClause = c} dataUrl="/Admin/FarClause/GetAll" dataVal="farClauseGuid" textVal="number" defaultItem={farClauseDefaultItem} />
                        {this.state.showErrors && <div className="text-danger">{this.state.errorTextFarClause}</div>}
                    </div>

                    <div className="form-group">
                        <label className="">Far Contract Type</label>
                        <DropDown ref={(c) => this.dropDownFarContractType = c} dataUrl="/Admin/FarContractType/GetAll" dataVal="farContractTypeGuid" textVal="code" defaultItem={farContractTypeDefaultItem} />
                        {this.state.showErrors && <div className="text-danger">{this.state.errorTextFarContractType}</div>}
                    </div>
                    */}
                    <div className="form-group">
                        <label className="">Far Clause</label>
                        <select className="form-control" onChange={(e) => this.onChangeFarClause(e)}>
                            <option > {farClauseDefaultItem.number} </option>
                            {this.state.farClauseList.map((farClause: any, index: any) => {
                                return (
                                    <option key={index} value={farClause.farClauseGuid}> {farClause.number} </option>
                                );
                            })}
                        </select>
                        {this.state.showErrors && <div className="text-danger">{this.state.errorTextFarClause}</div>}
                    </div>
                    <div className="form-group">
                        <label className="">Far Contract Type</label>
                        <select className="form-control" onChange={(e) => this.onChangeFarContractType(e)}>
                            <option > {farContractTypeDefaultItem.code} </option>
                            {this.state.farContractTypeList.map((farContractType: any, index: any) => {
                                return (
                                    <option key={index} value={farContractType.farContractTypeGuid}> {farContractType.code} </option>
                                );
                            })}
                        </select>
                        {this.state.showErrors && <div className="text-danger">{this.state.errorTextFarContractType}</div>}
                    </div>

                    <input type='radio' name="options" checked={this.state.isRequired} value="IsRequired" className="k-radio" id="isRequired"
                        onChange={(e) => {
                            this.onChangeOption(e);
                        }} />
                    <label htmlFor="isRequired" className="k-radio-label">Is Required</label>

                    <input type='radio' name="options" checked={this.state.isApplicable} value="IsApplicable" className="k-radio" id="isApplicable"
                        onChange={(e) => {
                            this.onChangeOption(e);
                        }} />
                    <label htmlFor="isApplicable" className="k-radio-label">Is Applicable</label>

                    <input type='radio' name="options" checked={this.state.isOptional} value="IsOptional" className="k-radio" id="isOptional"
                        onChange={(e) => {
                            this.onChangeOption(e);
                        }} />
                    <label htmlFor="isOptional" className="k-radio-label">Is Optional</label>

                    <input type='radio' name="options" checked={this.state.notValid} value="NotValid" className="k-radio" id="notValid"
                        onChange={(e) => {
                            this.onChangeOption(e);
                        }} />
                    <label htmlFor="notValid" className="k-radio-label">Not valid</label>
                </div>

                <DialogActionsBar>
                    <Button primary={true} onClick={this.onSubmitFiles} type="button">Add</Button>
                    <Button onClick={this.onCancel} type="button">Cancel</Button>
                </DialogActionsBar>
            </Dialog >}
        </div>);
    }
}