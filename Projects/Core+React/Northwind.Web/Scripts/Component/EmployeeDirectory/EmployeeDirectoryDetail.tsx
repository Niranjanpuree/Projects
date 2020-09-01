import * as React from "react"
import * as ReactDom from "react-dom"
import { KendoDialog } from "../../Common/Dialog/Dialog"
import { Dialog, DialogProps } from "@progress/kendo-react-dialogs"
import { Remote } from "../../Common/Remote/Remote";
import { any } from "prop-types";

declare var $: any;
interface IEmployeeDirectoryDetailProps {
    userGuid: any;
}
interface IEmployeeDirectoryDetailState {
    employeeDetail: any;
}
export class EmployeeDirectoryDetail extends React.Component<IEmployeeDirectoryDetailProps, IEmployeeDirectoryDetailState>{
    constructor(props: IEmployeeDirectoryDetailProps) {
        super(props);
        this.state = {
            employeeDetail: any,
        };
        this.loadDetailData = this.loadDetailData.bind(this);
    }
    loadDetailData() {
        let sender = this;
        var url = "/EmployeeDirectory/Details?id=" + this.props.userGuid;
        Remote.get(url, (data: any) => {
            sender.setState({ employeeDetail: data.result });
        }, (error: string) => { alert(error) });
    }
    componentDidMount() {
        this.loadDetailData()
    }
    render() {
        return (
            <div className="employee-profile">
                <div className="row">
                    <div className="col-auto">
                        <div className="employee-img">
                            <img src={("/img/nwg-1600px-w.png")} alt="" />
                        </div>
                    </div>
                    <div className="col">
                        <h2 className="employee-name mb-0 text-truncate">{this.state.employeeDetail.displayname}</h2>
                        <small className="employee-position">{this.state.employeeDetail.jobTitle}</small>

                        <div className="row">
                            <div className="col-12">
                                <h4 className="my-4 border-bottom">Info Detail</h4>
                            </div>
                            <div className="form-group col-md-4">
                                <label htmlFor="staticEmail" className="control-label mb-0">First Name:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.firstname}</h5>
                            </div>
                            <div className="form-group col-md-4">
                                <label htmlFor="staticEmail" className="control-label mb-0">Last Name:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.lastname}</h5>
                            </div>
                            <div className="form-group col-md-4">
                                <label htmlFor="staticEmail" className="control-label mb-0">Display Name:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.displayname}</h5>
                            </div>
                            <div className="form-group col-md-4">
                                <label htmlFor="staticEmail" className="control-label mb-0">Given Name: </label>
                                <h5 className="mb-0">{this.state.employeeDetail.givenname}</h5>
                            </div>
                            <div className="form-group col-md-4">
                                <label htmlFor="staticEmail" className="control-label mb-0">Email:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.personalEmail}</h5>
                            </div>
                            <div className="form-group col-md-4">
                                <label htmlFor="staticEmail" className="control-label mb-0">Mobile</label>
                                <h5 className="mb-0">{this.state.employeeDetail.mobilePhone}</h5>
                            </div>
                        </div>
                        <div className="row">
                            <div className="col-12">
                                <h4 className="my-4 border-bottom"> Work Detail</h4>
                            </div>
                            <div className="form-group col-md-12">
                                <label htmlFor="staticEmail" className="control-label mb-0">Company:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.company}</h5>
                            </div>
                            <div className="form-group col-md-12">
                                <label htmlFor="staticEmail" className="control-label mb-0">Department:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.department}</h5>
                            </div>
                            <div className="form-group col-md-12">
                                <label htmlFor="staticEmail" className="control-label mb-0">Manager:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.manager}</h5>
                            </div>
                            <div className="form-group col-md-12">
                                <label htmlFor="staticEmail" className="control-label mb-0">Group:</label>
                                <h5 className="mb-0">{this.state.employeeDetail.group}</h5>
                            </div>
                            <div className="form-group col-md-6">
                                <label htmlFor="staticEmail" className="control-label mb-0">Phone (Office):</label>
                                <h5 className="mb-0">{this.state.employeeDetail.workPhone}</h5>
                            </div>
                            <div className="form-group col-md-12">
                                <label htmlFor="staticEmail" className="control-label mb-0">Email (Office) :</label>
                                <h5 className="mb-0">{this.state.employeeDetail.workEmail}</h5>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}