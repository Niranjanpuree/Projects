import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
import { KendoGroupableGrid, RowIconSettings, IconSetting } from "../../../../Common/Grid/KendoGroupableGrid";
import { Dialog, DialogProps, DialogActionsBar } from "@progress/kendo-react-dialogs";
declare var $: any;
declare var window: any;
declare var formatUSDate: any;
declare var numberWithCommas: any;

interface IProjectDetailsTabPros
{
    projectId: any;
    dataUrl: any;
    fieldUrl: string;
    exportUrl: string;
    currency: string;
}

interface IProjectDetailsTabState
{
    showDialog: boolean;
    viewRecord: any;
}

export class ProjectDetailsTab extends React.Component<IProjectDetailsTabPros, IProjectDetailsTabState> {
    constructor(props: any)
    {
        super(props);

        this.state = {
            showDialog: false,
            viewRecord: null
        }

        this.createRowMenu = this.createRowMenu.bind(this);
        this.closeDialog = this.closeDialog.bind(this);
        this.renderModDetailsDialog = this.renderModDetailsDialog.bind(this);
    }

    createRowMenu()
    {
        let sender = this;
        let groupRowMenu = [
            {
                text: "Details", icon: "folder-open", action: (data: any, grid: any) =>
                {
                    sender.setState({showDialog: true, viewRecord: data.dataItem })
                }
            }
        ];
        return groupRowMenu;
    }

    closeDialog(e: any)
    {
        this.setState({ showDialog: false });
    }

    renderModDetailsDialog()
    {
        if (this.state.showDialog) {
            let sender = this;
            let prp: DialogProps = {
                height: '50%',
                width: '70%',
                title: 'Add WBS Dictionary',
                onClose: (e: any) =>
                {
                    sender.setState({ showDialog: false });
                },
                closeIcon: true
            }

            let attachments: any[] = this.state.viewRecord.attachments;

            return (
                <Dialog {...prp}>
                        <div className="row">
                            <div className="col-12">
                                <h4 className="">Project/Mod Details</h4>
                                <div className="row">
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">Mod Number</label>                                    
                                    <div className="form-value">
                                        {this.state.viewRecord.modNumber}
                                    </div>
                                </div>
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">Title</label>
                                    <div className="form-value">
                                        {this.state.viewRecord.title}
                                    </div>
                                </div>
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">Award Date</label>
                                    <div className="form-value">
                                        {formatUSDate(this.state.viewRecord.awardDate)}
                                    </div>
                                </div>
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">POP Start Date</label>
                                    <div className="form-value">
                                        {formatUSDate(this.state.viewRecord.popStartDate)}
                                    </div>
                                </div>
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">POP End Date</label>
                                    <div className="form-value">
                                        {formatUSDate(this.state.viewRecord.popEndDate)}
                                    </div>
                                </div>
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">Award Amount</label>
                                    <div className="form-value">
                                        {this.state.viewRecord.currency + " " + numberWithCommas(this.state.viewRecord.awardAmount)}
                                    </div>
                                </div>
                                <div className="form-group col-sm-4">
                                    <label className="control-label control-label-read">Funded Amount</label>
                                    <div className="form-value">
                                        {this.state.viewRecord.currency + " " + numberWithCommas(this.state.viewRecord.fundedAmount)}
                                    </div>
                                </div>
                                <div className="form-group col-sm-12">
                                    <label className="control-label control-label-read">Description</label>
                                    <div className="form-value">
                                        {this.state.viewRecord.description}
                                    </div>
                                </div>
                                <div className="form-group col-sm-12">
                                    <label className="control-label control-label-read">Attachments</label>
                                    <div className="form-value">
                                        {attachments.map((v: any,index: any) =>
                                        {
                                            return (<div key={"download" + index} className="col-12 row"><a href={v.downloadLink} target="_blank">{v.title}</a></div>)
                                        })}
                                    </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    <DialogActionsBar>
                        <button className="k-button k-primary" onClick={this.closeDialog}>Cancel</button>
                    </DialogActionsBar>
                </Dialog>
                );
        }
    }

    render()
    {
        let gridMenu = [
            { text: 'Export to PDF', icon: 'pdf', action: (data: any, grid: any) => { grid.exportToPDF() } },
            { text: 'Export to Excel', icon: 'excel', action: (data: any, grid: any) => { grid.exportToExcel() } }
        ]
        let groupTotalFields = ["proj_v_cst_amt", "proj_f_cst_amt"]
        let groupRowMenu = this.createRowMenu();
        let rowIconSettings: RowIconSettings = new RowIconSettings();
        rowIconSettings.column = "contractNumber";
        rowIconSettings.icons.push(new IconSetting("isFavorite", true, "", "favorite-icon"));
        return (
            <div id="projectDetailsTab">
                <div className="row">
                    {/* <div className="col-12">Project Mods</div> */}
                </div>
                <KendoGroupableGrid groupTotalFields={groupTotalFields} rowMenus={groupRowMenu} currencySymbol={this.props.currency} showColumnMenu={false} rowIconSettings={rowIconSettings} gridMenu={gridMenu} showGridAction={true} showSearchBox={true} showAdvancedSearchDialog={true} advancedSearchEntity={["PFS-ProjectMod"]} identityField="proj_mod_id" groupField="projectNumber" columnUrl={this.props.fieldUrl} exportFieldUrl={this.props.exportUrl} dataUrl={this.props.dataUrl} container="groupableGrid" />
                {this.renderModDetailsDialog()}
            </div>
        );
    }
}