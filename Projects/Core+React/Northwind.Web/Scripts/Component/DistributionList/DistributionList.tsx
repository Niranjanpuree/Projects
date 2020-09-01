import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../Common/Remote/Remote"
import { Input } from "@progress/kendo-react-inputs";
import { Button } from "@progress/kendo-react-buttons";
import { process } from "@progress/kendo-data-query";
import { Link, BrowserRouter, Redirect } from "react-router-dom";
import { KendoGrid as KendoGrid } from "../../Common/Grid/KendoGrid";
import { validateTitle } from "./Validate";
import { CheckBox } from "../../Common/CheckBox/CheckBox";
import { Dialog as KendoDialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
//import { AutoComplete } from "../../Common/AutoComplete/AutoComplete";
import { MultiSelectDropDown } from "../../Common/MultiSelectDropDown/MultiSelectDropDown";
import SpinnerPage from "../FileUpload/SpinnerPage";

declare var window: any;
declare var $: any;

interface IDistributionListProps {
    parentDomId: string,
    notificationTemplateKey: string,
    resourceId: any,
    showCreateDistributionLink: boolean,
    showAdditionalNote: boolean,
    showOnNotifySkipButton: boolean,
    showAddNewButton: boolean,
    showAdditionalIndividualRecipient: boolean,
    notifyCallBack?: Function,
    skipCallBack?: Function,
    showNotifyInstructionText?: boolean,
    showSuccessMessageAndNotifyInstructionText?: boolean,
    isRenderFromParentDialog?: boolean,
}

interface IDistributionListState {
    checkBoxProps: any,
    showDialog: boolean,
    showDialogForEdit: boolean,
    showDialogForViewMember: boolean,
    title?: string,
    distributionListGuid?: any,
    distributionTitle?: string,
    additionalNote?: string,
    titleErr: any,
    hasDistributionList: boolean,
    clientWidth: number,
    showLoading: boolean,
    //selectedItemAutoComplete: any
    //selectedItemAutoCompleteDistributionUser: any
}

export class DistributionList extends React.Component<IDistributionListProps, IDistributionListState> {
    grid: KendoGrid = null;
    //autoComplete: AutoComplete = null;
    //autoCompleteDistributionUser: AutoComplete = null;
    userSelectionGrid: KendoGrid = null;
    multiSelectDropDown: MultiSelectDropDown = null;
    multiSelectDropDownDistributionUser: MultiSelectDropDown = null;
    checkBox: CheckBox = null;
    distributionGridMenu: any[] = [];

    constructor(props: any) {
        super(props);
        this.addNewDistributionList = this.addNewDistributionList.bind(this);
        this.onSaveDistributionUser = this.onSaveDistributionUser.bind(this);
        this.onEditDistributionUser = this.onEditDistributionUser.bind(this);
        this.escFunction = this.escFunction.bind(this);

        this.onNotify = this.onNotify.bind(this);
        this.onCancel = this.onCancel.bind(this);
        //this.onAutoCompleteChange = this.onAutoCompleteChange.bind(this);
        //this.onAutoCompleteDistributionUserChange = this.onAutoCompleteDistributionUserChange.bind(this);

        let checkBoxProps = {
            initialChecked: false,
            labelName: "Is Public",
            disableControl: false,
        }

        this.distributionGridMenu = [
            {
                text: 'Delete', icon: 'delete', action: (data: any, grid: any) => {

                    grid.getSelectedItems((items: any[]) => {
                        if (items.length == 0) {
                            window.Dialog.alert("Please select at least a row to delete")
                            return;
                        } else {
                            window.Dialog.confirm({
                                text: "Are you sure you want to  delete distribution(s) ?",
                                title: "Confirm",
                                ok: function (e: any) {
                                    let idArr: any[] = [];
                                    let i: number = 0;
                                    for (i; i < items.length; i++) {
                                        idArr.push(items[i].distributionListGuid);
                                    }
                                    Remote.postData('/DistributionList/Delete', idArr, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                                },
                                cancel: function (e: any) {
                                }
                            });
                        }
                        grid.reloadData();
                    });

                }
            }
        ];

        this.state = {
            checkBoxProps: checkBoxProps,
            showDialog: false,
            showDialogForEdit: false,
            showDialogForViewMember: false,
            title: '',
            distributionListGuid: '',
            distributionTitle: '',
            titleErr: '',
            hasDistributionList: true,
            clientWidth: 0,
            showLoading: false,
            //selectedItemAutoComplete: null,
            //selectedItemAutoCompleteDistributionUser: null
        };
    }

    escFunction(event: any) {
        if (event.keyCode === 27) {
            this.onCancel();
            return;
        }
    }

    componentWillUnmount() {
        document.removeEventListener("keydown", this.escFunction, false);
    }

    componentDidMount() {
        document.addEventListener("keydown", this.escFunction, false);
        let sender = this;
        Remote.get("/DistributionList/HasDistributionListForTheLoggedUser",
            (response: any) => {
                sender.setState({
                    hasDistributionList: response.hasDistribution,
                    clientWidth: document.getElementById(sender.props.parentDomId).clientWidth - 250
                });
            },
            (err: any) => {
                window.Dialog.alert(err)
            });
    }

    /* button / rows  events */
    addNewDistributionList(e: any) {
        this.setState(prevState => ({
            showDialog: true,
            title: '',
            titleErr: '',
        }));
    }

    getRowMenus() {
        let rowMenus = [
            {
                text: "View Member(s)",
                icon: "folder-open",
                action: (data: any, grid: any) => {
                    this.setState({
                        showDialogForViewMember: true,
                        distributionListGuid: data.dataItem.distributionListGuid,
                        distributionTitle: data.dataItem.title,
                    });
                }
            },
            {
                text: "Edit",
                icon: "pencil",
                conditions: [{ field: 'isOwner', value: true }],
                action: (data: any, grid: any) => {
                    if (data.dataItem.isOwner) {
                        this.setState(prevState => ({
                            ...this.state,
                            checkBoxProps: {
                                ...prevState.checkBoxProps,
                                disableControl: false,
                            },
                        }));
                    } else {
                        this.setState(prevState => ({
                            ...this.state,
                            checkBoxProps: {
                                ...prevState.checkBoxProps,
                                disableControl: true,
                            },
                        }));
                    }
                    this.setState(prevState => ({
                        showDialogForEdit: true,
                        checkBoxProps: {
                            ...prevState.checkBoxProps,
                            initialChecked: data.dataItem.isPublic,
                        },
                        distributionListGuid: data.dataItem.distributionListGuid,
                        title: data.dataItem.title,
                        distributionTitle: data.dataItem.title,
                        titleErr: '',
                    }));
                }
            },
            {
                text: "Delete", icon: "delete",
                conditions: [{ field: 'isOwner', value: true }],
                action: (data: any, grid: any) => {

                    let ids: any[] = [];
                    let id = data.dataItem.distributionListGuid;
                    ids.push(id);
                    window.Dialog.confirm({
                        text: "Are you sure you want to  delete distribution ?",
                        title: "Confirm",
                        ok: function (e: any) {
                            Remote.postData('/DistributionList/Delete', ids, (e: any) => { grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                        },
                        cancel: function (e: any) {
                        }
                    });
                }
            }
        ];
        return rowMenus;
    }

    getDistributionListMemberRowMenus() {
        let sender = this;
        let rowMenus = [
            {
                text: "Remove", icon: "delete",
                action: (data: any, grid: any) => {
                    let ids: any[] = [];
                    let distributionListId = data.dataItem.distributionListId;
                    let userId = data.dataItem.userGuid;

                    window.Dialog.confirm({
                        text: "Are you sure you want to  delete member ?",
                        title: "Confirm",
                        ok: function (e: any) {
                            Remote.postData('/DistributionList/RemoveMemberFromDistributionList?DistributioinListId=' + distributionListId + '&UserId=' + userId, ids, (e: any) => { sender.grid.refresh(); grid.refresh() }, (err: any) => { window.Dialog.alert(err) });
                        },
                        cancel: function (e: any) {
                        }
                    });
                }
            }
        ];
        return rowMenus;
    }

    /* render dialogs */
    renderAddDialog() {
        const { title, titleErr, showDialog } = this.state;
        if (showDialog) {
            $('html').addClass('htmlClass');
            return (
                <div id="notifyDialogContainerAdd">
                    <KendoDialog title="Add Distribution List" width="85%" height="80%" className="dialog-custom">
                        <div className="">
                            <form
                                className="form-group"
                                onSubmit={this.onSubmit}
                                noValidate
                            >
                                <div className="row align-items-center">
                                    <div className="col-sm-6">
                                        <label className="control-label font-weight-bold">Distribution Title</label>
                                        {titleErr.status && <div className="text-danger">{titleErr.value}</div>}
                                        <input type="text"
                                            required
                                            placeholder="Distribution Title"
                                            className="form-control"
                                            name="title"
                                            value={title}
                                            onChange={this.handleTitleChange} />
                                    </div>
                                    <div className="col-sm-6">
                                        <label className="control-label font-weight-bold">Select Users</label>
                                        {/*<AutoComplete id="autoCUsers" ref={(c) => { this.autoCompleteDistributionUser = c; }} dataUrl={"/Iam/User/GetUsersDataForAutoComplete?distributionListId=" + ""} className="form-control" onChange={this.onAutoCompleteDistributionUserChange} displayField="displayName" placeHolder="Please type firstname or lastname" allowMultiple={true} showTooltip={true} tooltipAlignment="top" tooltipTemplate="`Name : ${data.displayName}\r\nJob Title : ${data.jobTitle}\r\nDepartment : ${data.department}\r\nEmail : ${data.workEmail}`" valueField="userGuid" />*/}
                                        <MultiSelectDropDown
                                            ref={(c) => { this.multiSelectDropDownDistributionUser = c }}
                                            dataUrl="/Iam/User/GetUsersData" />
                                    </div>
                                    <div className="col-auto mt-3 text-right">
                                        <CheckBox ref={(c) => { this.checkBox = c }} {...this.state.checkBoxProps} />
                                    </div>
                                </div>
                            </form >

                        </div>
                        <DialogActionsBar>
                            <Button onClick={this.onSaveDistributionUser} className="btn-primary" type="button">Save</Button>
                            <Button onClick={this.onCancel} type="button">Cancel</Button>
                        </DialogActionsBar>
                    </KendoDialog >
                </div>);
        }
    }



    renderEditDialog() {
        const { title, titleErr, showDialogForEdit, distributionListGuid, distributionTitle } = this.state;
        let dialogTitle = distributionTitle + " Edit";
        if (showDialogForEdit) {
            $('html').addClass('htmlClass');
            return (<KendoDialog title={dialogTitle} width="85%" className="dialog-custom" >
                <div id="distributionUserList" className="">
                    <form
                        className="form-group"
                        onSubmit={this.onSubmit}
                        noValidate
                    >
                        <div className="row align-items-center">
                            <div className="col-sm-6">
                                <label className="control-label font-weight-bold mb-0">Distribution Title</label>
                                {titleErr.status && <div className="text-danger">{titleErr.value}</div>}
                                <input type="text"
                                    required
                                    placeholder="Distribution Title"
                                    className="form-control"
                                    name="title"
                                    value={title}
                                    onChange={this.handleTitleChange} />
                            </div>
                            <div className="col-sm-6">
                                <label className="control-label font-weight-bold">Select Users</label>
                                {/*<AutoComplete id="autoCUsers" ref={(c) => { this.autoCompleteDistributionUser = c; }} dataUrl={"/Iam/User/GetUsersDataForAutoComplete?distributionListId=" + ""} className="form-control" onChange={this.onAutoCompleteDistributionUserChange} displayField="displayName" placeHolder="Please type firstname or lastname" allowMultiple={true} showTooltip={true} tooltipAlignment="top" tooltipTemplate="`Name : ${data.displayName}\r\nJob Title : ${data.jobTitle}\r\nDepartment : ${data.department}\r\nEmail : ${data.workEmail}`" valueField="userGuid" />*/}
                                <MultiSelectDropDown
                                    ref={(c) => { this.multiSelectDropDownDistributionUser = c }}
                                    dataUrl="/Iam/User/GetUsersData" />
                            </div>
                            <div className="col-auto mt-3 text-right">
                                <CheckBox ref={(c) => { this.checkBox = c }} {...this.state.checkBoxProps} />
                            </div>
                        </div>
                    </form >

                    <KendoGrid ref={(c) => { this.userSelectionGrid = c }}
                        selectionField="status"
                        gridHeight="auto"
                        itemNavigationUrl={"mailto:"}
                        navigationFields={['workEmail']}
                        showColumnMenu={false}
                        showSearchBox={false}
                        showGridAction={false}
                        rowMenus={this.getDistributionListMemberRowMenus()}
                        identityField="workEmail"
                        dataURL={"/DistributionList/GetSelectedUsers?distributionListGuid=" + distributionListGuid}
                        fieldUrl="/GridFields/DistributionUser"
                        gridWidth={this.state.clientWidth}
                        parent="distributionUserList" />

                </div>

                <DialogActionsBar>
                    <Button onClick={this.onEditDistributionUser} className="btn-primary" type="button">Save</Button>
                    <Button onClick={this.onCancel} type="button">Cancel</Button>
                </DialogActionsBar>
            </KendoDialog >);
        }
    }

    renderViewMemberDialog() {
        const { showDialogForViewMember, distributionListGuid, distributionTitle } = this.state;
        let title = distributionTitle;
        if (showDialogForViewMember) {
            $('html').addClass('htmlClass');
            return (<KendoDialog title={title} width="85%" className="dialog-custom">
                <div id="distributionUserList">
                    <KendoGrid
                        itemNavigationUrl={"mailto:"}
                        navigationFields={['workEmail']}
                        selectionField="status"
                        showColumnMenu={false}
                        showGridAction={false}
                        showSearchBox={true}
                        showSelection={false}
                        showAdvanceSearchBox={false}
                        identityField="workEmail"
                        dataURL={"/DistributionList/GetSelectedUsers?distributionListGuid=" + distributionListGuid}
                        fieldUrl="/GridFields/DistributionUser"
                        gridWidth={this.state.clientWidth}
                        parent="distributionUserList"
                    />
                </div>
                <DialogActionsBar>
                    <Button onClick={this.onCancel} type="button">Cancel</Button>
                </DialogActionsBar>
            </KendoDialog >);
        }
    }


    /* dialogs events */
    onSaveDistributionUser() {
        //check validation of inputs..
        let sender = this;
        if (sender.checkFormStatus(sender)) {
            //                        let userSelectionGrid = sender.userSelectionGrid.getGridDataToPost();
            let multiSelectDropDownDistributionUser = sender.multiSelectDropDownDistributionUser.state.value;
            let distributionViewModel = {
                //                            UserSelection: userSelectionGrid,
                Title: sender.state.title,
                IsPublic: this.checkBox.state.checked,
                SelectedUsers: multiSelectDropDownDistributionUser,
            }
            Remote.postData('/DistributionList/Add',
                distributionViewModel,
                (response: any) => {
                    //if response status is not success from the server then add server error in title error object..
                    if (response.status !== true) {
                        this.setState({
                            titleErr: {
                                status: true,
                                value: response.message
                            }
                        });
                    } else {
                        // close dialog if values has been successfully added..
                        sender.setState({
                            showDialog: false,
                            hasDistributionList: true,
                            title: '',
                        });
                        sender.grid.refresh();
                    }
                },
                (err: any) => {
                    window.Dialog.alert(err);
                });

        } else {
            sender.setState({
                showDialog: true,
                title: '',
            });
        }

        if (!this.props.isRenderFromParentDialog)
            $('html').removeClass('htmlClass');
    }

    onEditDistributionUser() {
        let sender = this;
        //check validation of inputs..
        if (this.checkFormStatus(sender)) {
            let userSelectionGrid = sender.userSelectionGrid.getGridDataToPost();
            let multiSelectDropDownDistributionUser = sender.multiSelectDropDownDistributionUser.state.value;

            let distributionViewModel = {
                DistributionListGuid: sender.state.distributionListGuid,
                UserSelection: userSelectionGrid,

                Title: sender.state.title,
                IsPublic: this.checkBox.state.checked,
                SelectedUsers: multiSelectDropDownDistributionUser,
            }
            Remote.postData('/DistributionList/Edit',
                distributionViewModel,
                (response: any) => { sender.grid.refresh(); },
                (err: any) => { window.Dialog.alert(err) });

            sender.setState({
                showDialogForEdit: false,
                title: ''
            });
        } else {
            sender.setState({
                showDialogForEdit: true,
                title: '',
            });
        }

        if (!this.props.isRenderFromParentDialog)
            $('html').removeClass('htmlClass');
    }

    onCancel() {
        this.setState({
            showDialog: false,
            showDialogForEdit: false,
            showDialogForViewMember: false,
        });

        if (!this.props.isRenderFromParentDialog)
            $('html').removeClass('htmlClass');
    }


    /* form events */
    onSubmit = (e: any) => {
        e.preventDefault();
    };

    handleTitleChange = (e: any) => {
        //remove required validation error from the view if data is filled ..
        if (e.target.name === 'title') {
            var titleErr = this.checkFormStatus(this);
            this.setState({
                titleErr: titleErr
            });
        }

        //set state of input fields..
        this.setState({
            title: e.target.value,
        });
    }

    handleAdditionalNotesChange = (e: any) => {
        //set state of input fields..
        this.setState({
            additionalNote: e.target.value,
        });
    }

    checkFormStatus(sender: any) {
        // form validation middleware
        const { title } = sender.state;
        const titleErr = validateTitle(title);

        if (!titleErr.status) {
            return true;
        } else {
            sender.setState({
                titleErr: titleErr
            });
            return false;
        }
    }


    /*lastly notify the selected  distribution list users  */
    async  onNotify(e: any) {

        this.setState({
            showLoading: true
        })
        let distributionSelection = this.grid.getGridDataToPost();
        let individualRecipients = this.multiSelectDropDown.state.value;
        let resourceId = this.props.resourceId;

        let additionalNotes = this.state.additionalNote;
        let notificationTemplateKey = this.props.notificationTemplateKey;
        if (distributionSelection.selectedAll === false && distributionSelection.includeList.length === 0 && individualRecipients.length === 0) {
            window.Dialog.alert("Please select any distribution list / add individual recipient for notification..");

            this.setState({
                showLoading: false
            })
            return;
        }
        let notificationViewModel = {
            DistributionSelection: distributionSelection,
            AdditionalNotes: additionalNotes,
            NotificationTemplateKey: notificationTemplateKey,
            ResourceId: resourceId,
            IndividualRecipients: individualRecipients
        }
        let response = await Remote.postDataAsync('/Notification/Add', notificationViewModel);

        if (response.ok) {
            window.Dialog.alert("Notified to all user(s)");
            this.grid.onDialogCancel(e);
        } else {
            this.setState({
                showLoading: false
            })
            let message = await Remote.parseErrorMessage(response);
            window.Dialog.alert(message);

            return;
        }

        this.props.notifyCallBack(e, 'Notified');

        this.setState({
            showLoading: false
        })
    }

    onSkip = (e: any) => {
        this.props.skipCallBack(e, 'Skipped');
    }

    //onAutoCompleteChange(e: any) {
    //    this.setState({ selectedItemAutoComplete: e });
    //}

    //onAutoCompleteDistributionUserChange(e: any) {
    //    this.setState({ selectedItemAutoCompleteDistributionUser: e });
    //}

    /* final render */
    render() {
        const show = {
            display: 'block !important',
        }
        const hidden = {
            display: 'none',
        }
        let userAddButton = {
            text: "Add New",
            icon: "",
            action: (data: any, grid: any) => {
                this.addNewDistributionList(data)
            }
        }
        const { additionalNote } = this.state;
        return (
            <div id="notifyDialogContainer">

                {this.state.showLoading && <SpinnerPage />}

                {this.props.showNotifyInstructionText && <div className="alert alert-primary">
                    Select the distribution list and/or Add Individual Recipient to Notify. You can view the members of each list by clicking on pull down menu available for each distribution list and clicking on View Members
                </div>}

                {this.props.showSuccessMessageAndNotifyInstructionText && <div className="alert alert-primary">The Mod has been saved successfully.
                   Please choose the distribution list to notify other people of this action.
                   If you have not created any distribution list, you can still select individual recipients by adding them to Individual Recipients list.
                   You may save that list by clicking on Create a new distribution list.
                   If you prefer not to notify other people, click on Skip button.</div>}
                <KendoGrid ref={(c) => { this.grid = c }}
                    showGridAction={true}
                    dataURL={"/DistributionList/Get"}
                    fieldUrl="/GridFields/DistributionList"
                    identityField="userGuid"
                    addRecord={userAddButton}
                    //addNewDistribution={this.addNewDistributionList}
                    gridMenu={this.distributionGridMenu}
                    showAddNewButton={this.props.showAddNewButton}
                    showAdvanceSearchBox={false}
                    showColumnMenu={false}
                    rowMenus={this.getRowMenus()}
                    gridWidth={document.getElementById(this.props.parentDomId).clientWidth}
                    parent={"notifyDialogContainer"} />

                <div className="row">
                    <div className="col-sm-12 form-group  mt-3">
                        {this.props.showAdditionalIndividualRecipient &&
                            <div className="form-value">
                                <label className="control-label control-label-read"><b>Add Individual Recipient</b></label>
                                {/* <AutoComplete id="autoCUsers" ref={(c) => { this.autoComplete = c; }} dataUrl={"/Iam/User/GetUsersDataForAutoComplete?distributionListId=" + ""} className="form-control col-6" onChange={this.onAutoCompleteChange} displayField="displayName" placeHolder="Please type firstname or lastname" allowMultiple={true} showTooltip={true} tooltipAlignment="top" tooltipTemplate="`Name : ${data.displayName}\r\nJob Title : ${data.jobTitle}\r\nDepartment : ${data.department}\r\nEmail : ${data.workEmail}`" valueField="userGuid" /> */}
                                <MultiSelectDropDown
                                    ref={(c) => { this.multiSelectDropDown = c }}
                                    dataUrl="/Iam/User/GetUsersData" />
                            </div>
                        }
                        {this.props.showCreateDistributionLink && <a className="mt-2" onClick={this.addNewDistributionList} href="javascript:void(0)">Create a new distribution list</a>}
                    </div>
                </div>
                {this.props.showAdditionalNote &&
                    <div className="row">
                        <div className="col-sm-12 form-group">
                            <label className="control-label control-label-read"><b>Additional Notes</b></label>
                            <div className="form-value">
                                <textarea className="form-control" value={additionalNote} onChange={this.handleAdditionalNotesChange} />
                            </div>
                        </div>
                    </div>
                }
                <div style={!this.props.showOnNotifySkipButton ? hidden : show} className="col-12 text-right">
                    <Button id="btnDistributionNotify" primary={true} onClick={this.onNotify} inputMode="submit">Notify</Button>
                    &nbsp;
                    <Button primary={false} onClick={this.onSkip} inputMode="submit">Skip</Button>
                </div>

                {this.renderAddDialog()}
                {this.renderEditDialog()}
                {this.renderViewMemberDialog()}
            </div>
        );
    }
} 