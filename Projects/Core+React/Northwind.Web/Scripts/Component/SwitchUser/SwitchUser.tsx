import 'core-js';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { Bucket } from '../Layout/Bucket';
import { Input } from '@progress/kendo-react-inputs';
import { Button } from '@progress/kendo-react-buttons';
import { Dialog, DialogActionsBar, DialogProps } from '@progress/kendo-react-dialogs';
import { KendoGrid } from '../../Common/Grid/KendoGrid';
import { Remote } from '../../Common/Remote/Remote';
declare var window: any;

export interface ISwitchUserProps
{
    SiteUrl: string;
}

export interface ISwitchUserState
{
    showDialog: boolean;
    showSwitchBackUserButton: boolean;
}

export class SwitchUser extends React.Component<ISwitchUserProps, ISwitchUserState> {

    constructor(props: ISwitchUserProps)
    {
        super(props);
        this.state = {
            showDialog: true,
            showSwitchBackUserButton: false
        }

        this.getUserGrid = this.getUserGrid.bind(this);
        this.onDialogClose = this.onDialogClose.bind(this);
        this.onClick = this.onClick.bind(this);
        this.onSwitchBack = this.onSwitchBack.bind(this);
        this.onGridRowClick = this.onGridRowClick.bind(this);
    }

    componentDidMount()
    {
        let sender = this;
        Remote.postData(this.props.SiteUrl + "/Login/isSimulatedUser", {}, (data: any) =>
        {
            sender.setState({ showSwitchBackUserButton: data.status });
        }, (error: any) => { });
    }

    getUserGrid()
    {

    }

    onDialogClose(e: any)
    {
        this.setState({ showDialog: false });
    }

    onClick(e: any)
    {
        this.setState({ showDialog: true });
    }

    onSwitchBack(e: any)
    {
        Remote.postData(this.props.SiteUrl + "/Login/SwitchBackToNormaluser", {}, (data: any) => { window.location.reload(1) }, (error: any) => { } );
    }

    onGridRowClick(e: any)
    {
        Remote.postData(this.props.SiteUrl + "/Login/SwitchUser", e.dataItem, (data: any) => { window.location.reload(1) }, (error: any) => { });
    }

    render()
    {
        if (this.state.showDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '75%',
                width: '80%',
                title: 'Switch User',
                onClose: (e: any) =>
                {
                    sender.setState({ showDialog: false });
                },
                closeIcon: true
            }


            return (<Dialog {...prp}>
                <div className="">
                    <h5 className="">Select user to switch</h5> {this.state.showSwitchBackUserButton && <div style={{ display: 'inline', float: 'right' }}><Button className="btn btn-primary" onClick={this.onSwitchBack}>Switch to Original User</Button></div>}
                </div>
                <div id="gridUserSwich">
                    <KendoGrid dataURL={this.props.SiteUrl + "/IAM/User/GetUserForSwitchUser"} fieldUrl={this.props.SiteUrl + "/GridFields/SwitchUser"} exportFieldUrl={"/Export/SwitchUser"} showAddNewButton={false} showColumnMenu={false} showAdvanceSearchBox={false} showGridAction={false} showSelection={false} parent="gridUserSwich" identityField="userGuid" onRowClick={this.onGridRowClick} />
                </div>
                <DialogActionsBar>
                    <button className="k-button" onClick={this.onDialogClose}>Cancel</button>
                </DialogActionsBar>
            </Dialog>)
        }
        else {
            return (<></>)
        }
    }
}