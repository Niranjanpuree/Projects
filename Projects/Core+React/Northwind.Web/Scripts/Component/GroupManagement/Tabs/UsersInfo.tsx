import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";

declare var window: any;
declare var $: any;

interface IUsersInfoProps {
    userGuid: string;
  
}

interface IUsersInfoState {
   
    model: any;
   }

export class UsersInfo extends React.Component<IUsersInfoProps, IUsersInfoState>
{
    
    constructor(props: IUsersInfoProps) {
        super(props);
        this.state = {
           
            model: {}
           
        }
        this.init();
    }

    init() {
        Remote.get("/IAM/user/Detail?id=" + this.props.userGuid, (data: any) => {
            this.setState({ model: data })
            console.log(this.state.model.result)
            }, (data: any) => { window.Dialog.alert(data); })
    }


    render() {
       
        return (
            <div>

                <div className="panels">
                    <fieldset>
                        <legend>Personal Information</legend>
                        <div className="row">
                        <div className="form-group col-md-4">
                            <label html-for="Lastname" className="control-label control-label-read">Username </label>
                            <h6>{this.state.model.result && this.state.model.result.username}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label html-for="Lastname" className="control-label control-label-read">FirstName </label>
                            <h6>{this.state.model.result && this.state.model.result.firstname}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label html-for="Lastname" className="control-label control-label-read">LastName </label>
                            <h6>{this.state.model.result && this.state.model.result.lastname}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="PersonalEmail" className="control-label control-label-read">Email</label>
                            <h6>{this.state.model.result && this.state.model.result.personalEmail}</h6>
                        </div>

                        <div className="form-group col-md-4">
                            <label asp-for="HomePhone" className="control-label control-label-read">Phone</label>
                            <h6>{this.state.model.result && this.state.model.result.homePhone}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="MobilePhone" className="control-label control-label-read">Mobile</label>
                            <h6>{this.state.model.result && this.state.model.result.mobilePhone}</h6>
                        </div>
                        </div>
                    </fieldset>
                </div>
                <div className="panels">
                    <fieldset>
                        <legend>Work Information</legend>
                        <div className="row">
                        <div className="form-group col-md-4">
                            <label asp-for="Company" className="control-label control-label-read">Company</label>
                            <h6>{this.state.model.result && this.state.model.result.company}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="Department" className="control-label control-label-read">Department</label>
                            <h6>{this.state.model.result && this.state.model.result.department}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="JobTitle" className="control-label control-label-read">Job Title</label>
                            <h6>{this.state.model.result && this.state.model.result.jobTitle}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="JobStatus" className="control-label control-label-read">Job Status</label>
                            <h6>{this.state.model.result && this.state.model.result.jobStatus}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="Manager" className="control-label control-label-read">Manager :</label>
                            <h6>{this.state.model.result && this.state.model.result.manager}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="Group" className="control-label control-label-read">Group</label>
                            <h6>{this.state.model.result && this.state.model.result.group}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="WorkEmail" className="control-label control-label-read">Work Email</label>
                            <h6>{this.state.model.result && this.state.model.result.workEmail}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="WorkPhone" className="control-label control-label-read">Work Phone</label>
                            <h6>{this.state.model.result && this.state.model.result.workPhone}</h6>
                        </div>
                        <div className="form-group col-md-4">
                            <label asp-for="Extension" className="control-label control-label-read"> Extension </label>
                            <h6>{this.state.model.result && this.state.model.result.extension}</h6>
                            <input asp-for="UserGuid" type="hidden" />
                        </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        );
    }
}