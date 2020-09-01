import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../Common/Remote/Remote"
import { Input } from "@progress/kendo-react-inputs";
import { Button } from "@progress/kendo-react-buttons";
import { process } from "@progress/kendo-data-query";
import { Link, BrowserRouter, Redirect } from "react-router-dom";
import { KendoGrid } from "../../Common/Grid/KendoGrid";
import { KendoDialog as Dialog } from "../../Common/Dialog/Dialog";
import { MultiSelectDropDown } from "../../Common/MultiSelectDropDown/MultiSelectDropDown";
import { validateTitle } from "./Validate";
declare var window: any;
declare var $: any;

interface IDistributionUserDialogProps {
    parentDomId: any,
    distributionListGuid?: string,
    gridDataURL: string,
    dataURL: string,
}

interface IDistributionUserDialogState {
    dialogProps: any,
    showDialog: boolean,
    title?: string,
    titleErr: any,
    distributionListGuid?: string,
}

export class DistributionUserDialog extends React.Component<IDistributionUserDialogProps, IDistributionUserDialogState> {
    grid: KendoGrid = null;
    userSelectionGrid: KendoGrid = null;
    multiSelectDropDown: MultiSelectDropDown = null;
    dialog: Dialog;
    buttons: any[] = [];
    distributionGridMenu: any[] = [];

    constructor(props: any) {
        super(props);

        let sender = this;

        let dialogProps = {
            actionText: '',
            dialogTitle: '',
            buttons: this.buttons,
            dialogHeight: '50%',
            dialogWidth: '50%',
            uniqueKey: '',
            visible: true,
        }

        this.buttons[1] = ({
            primary: false,
            requireValidation: false,
            text: "Cancel",
            action: (data: any) => {
                sender.setState({
                    showDialog: false,
                    title: '',
                    titleErr: {},
                });
            }
        });

        this.state = {
            dialogProps: dialogProps,
            showDialog: false,
            title: '',
            distributionListGuid: '',
            titleErr: '',
        };
    }

    afterRenderShowHideButtons(isAddButton: boolean) {
        let sender = this;
        if (isAddButton) {
            this.buttons[0] = ({
                primary: true,
                requireValidation: false,
                text: "Add",
                action: (data: any) => {

                    //check validation of inputs..
                    if (sender.checkFormStatus(sender)) {

                        let userSelectionGrid = sender.userSelectionGrid.getSelectedItems();
                        let distributionViewModel = {
                            UserViewModels: userSelectionGrid,
                            Title: sender.state.title
                        }
                        Remote.postData(this.props.dataURL,
                            distributionViewModel,
                            (response: any) => { sender.grid.refresh(); },
                            (err: any) => { window.Dialog.alert(err) });

                        sender.setState({
                            showDialog: false,
                            title: '',
                        });
                    } else {
                        sender.setState({
                            showDialog: true,
                            title: '',
                        });
                        this.dialog.openForm();
                    }
                }
            });
        } else {
            this.buttons[0] = ({
                primary: true,
                requireValidation: false,
                text: "Edit",
                action: (data: any) => {
                    //check validation of inputs..
                    if (this.checkFormStatus(sender)) {
                        let userSelectionGrid = sender.userSelectionGrid.getSelectedItems();
                        let distributionViewModel = {
                            DistributionListGuid: sender.state.distributionListGuid,
                            UserViewModels: userSelectionGrid,
                            Title: sender.state.title
                        }
                        Remote.postData(this.props.dataURL,
                            distributionViewModel,
                            (response: any) => { sender.grid.refresh(); },
                            (err: any) => { window.Dialog.alert(err) });

                        sender.setState({
                            title: '',
                            showDialog: false
                        });
                    } else {
                        sender.setState({
                            title: '',
                            showDialog: true
                        });
                        this.dialog.openForm();
                    }
                }
            });
        }
    }

    formSubmit(e: any) {
        e.preventDefault();
    }

    addDistributionList() {
         ;
        this.setState(prevState => ({
            showDialog: true,
            dialogProps: {
                ...prevState.dialogProps,
                dialogTitle: 'Add Distribution'
            },
        }));
        this.afterRenderShowHideButtons(true);
    }
    editDistributionList() {
        this.setState(prevState => ({
            showDialog: true,
            dialogProps: {
                ...prevState.dialogProps,
                dialogTitle: 'Edit Distribution',
            },
            distributionListGuid: this.props.distributionListGuid,
            title: this.state.title,
        }));
        this.afterRenderShowHideButtons(false);
    }
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

    onSubmit = (e: any) => {
        e.preventDefault();
    };

    render() {
        const { title, titleErr, showDialog } = this.state;
        if (showDialog) {
            return (<Dialog ref={(c) => { this.dialog = c }} {...this.state.dialogProps}>
                <form
                    className="form-group"
                    onSubmit={this.onSubmit}
                    noValidate
                >
                    <div className="col-6">
                        <input type="text"
                            required
                            placeholder="Distribution Title"
                            className="form-control"
                            name="title"
                            value={title}
                            onChange={this.handleTitleChange} />
                        {titleErr.status && <div className="text-danger">{titleErr.value}</div>}
                    </div>
                </form >
                <KendoGrid ref={(c) => { this.userSelectionGrid = c }}
                    selectionField="status"
                    identityField="userGuid"
                    showGridAction={true}
                    showSearchBox={true}
                    dataURL={this.props.gridDataURL}
                    fieldUrl="/GridFields/DistributionUser"
                    gridWidth={document.getElementById(this.props.parentDomId).clientWidth} />
            </Dialog >);
        }
        else { return (<div></div>) }
    }
}