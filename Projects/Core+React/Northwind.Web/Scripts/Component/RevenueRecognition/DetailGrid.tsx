import * as React from "react";
import * as ReactDOM from "react-dom";
import { Details } from "./Details"
import { Remote } from "../../Common/Remote/Remote"
import { KendoGrid as KendoGrid } from "../../Common/Grid/KendoGrid";
import { Dialog as KendoDialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Guid } from "guid-typescript";
import { any } from "prop-types";

declare var window: any;
declare var document: any;

interface IDetailGridProps {
    parentDomId: string,
    parentGuid: any,
    cssUrl: string
}

interface IDetailGridState {
    checkBoxProps: any,
    dataURL: string,
    fieldURL: string,
    showDialog: boolean,
    revenueDetail: any,
    loadDetailData: boolean,
    resourceGuid: any
}

export class DetailGrid extends React.Component<IDetailGridProps, IDetailGridState>{
    grid: KendoGrid = null;
    topActionMenu: any[] = [];

    constructor(props: any) {

        super(props);

        let checkBoxProps = {
            initialChecked: false,
            disableControl: false,
        }

        this.topActionMenu = [
            {
                text: 'Export', icon: 'export', action: (data: any, grid: any) => {

                    grid.getSelectedItems((items: any[]) => {
                        if (items.length == 0) {
                            window.Dialog.alert("Please select at least a row")
                            return;
                        } else {
                            let sender = this;
                            let idArr: any[] = [];
                            let i: number = 0;
                            for (i; i < items.length; i++) {
                                idArr.push(items[i].revenueRecognizationGuid);
                            }
                            Remote.postData('/revenuerecognition/ExportDetailList', idArr,
                                (e: any) => {
                                    sender.setState({ revenueDetail: e.result, loadDetailData: false, showDialog: true })
                                },
                                (err: any) => { window.Dialog.alert(err) });
                        }
                        //grid.reloadData();
                    });

                }
            }
        ];

        this.state = {
            checkBoxProps: checkBoxProps,
            revenueDetail: any,
            loadDetailData: false,
            showDialog: false,
            resourceGuid: Guid.EMPTY,
            dataURL: "/RevenueRecognition/GetDetailList/" + this.props.parentGuid,
            fieldURL: "/GridFields/RevenueRecognition"
        }
    }


    getRowMenus() {
        let rowMenus = [
            {
                text: "Details",
                icon: "info",
                action: (data: any, grid: any) => {
                    let idArr: any[] = [];
                    let i: number = 0;
                    idArr.push(data.dataItem.revenueRecognizationGuid);
                    Remote.postData('/revenuerecognition/ExportDetailList', idArr,
                        (e: any) => {
                            this.setState(prevState => ({
                                revenueDetail: e.result,
                                showDialog: true,
                                loadDetailData: true,
                                resourceGuid: data.dataItem.revenueRecognizationGuid
                            }));
                        },
                        (err: any) => { window.Dialog.alert(err) });

                }
            }
        ];
        return rowMenus;
    }

    renderDetailDialog() {
        if (this.state.showDialog) {
            return (<Details parentDomId={this.props.parentDomId} parentGuid={this.props.parentGuid} revenueDetail={this.state.revenueDetail} cssUrl={this.props.cssUrl} />);
        }
    }

    renderGrid() {
        if (!this.state.showDialog) {
            return (<KendoGrid ref={(c) => { this.grid = c }}
                showGridAction={true}
                dataURL={this.state.dataURL}
                fieldUrl={this.state.fieldURL}
                identityField="RevenueRecognizationGuid"
                gridMenu={this.topActionMenu}
                showAdvanceSearchBox={false}
                showColumnMenu={false}
                rowMenus={this.getRowMenus()}
                gridWidth={document.getElementById(this.props.parentDomId).clientWidth} />);
        }
    }

    render() {
        return (<div>
            {this.renderGrid()}
            {this.renderDetailDialog()}
        </div>);
    }
}