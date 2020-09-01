import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Button } from "@progress/kendo-react-buttons";
import { TabStrip, TabStripContent, TabStripTab } from "@progress/kendo-react-layout";
declare var window: any;

interface IGroupDetailsProps
{
    groupGuid: string;
}

interface IGroupDetailsState
{
    groupDetail: any;
}

export class GroupDetails extends React.Component<IGroupDetailsProps, IGroupDetailsState>
{
    isComponentLoaded: boolean;
    constructor(props: IGroupDetailsProps)
    {
        super(props);
        this.state = {
            groupDetail: { cn: '', description: '', groupGuid: '' }
        }
        this.loadGroupDetails = this.loadGroupDetails.bind(this);
    }

    componentWillMount(){
        this.isComponentLoaded = false;
    }

    async componentDidMount(){
        this.isComponentLoaded = true;
        if(this.isComponentLoaded){
           await this.loadGroupDetails()
        }
    }

    async loadGroupDetails()
    {
        var response = await Remote.getAsync("/IAM/Group/GetGroup/" + this.props.groupGuid);
        if (response.ok) {
            var groupDetail = await response.json();
            this.setState({ groupDetail: groupDetail });
        }
        else {
            window.Dialog.alert("Error", response.statusText);
        }
    }

    render()
    {
        return (
            // <div className="container-fluid">
                <div className="col-12 p-0">
                    <div className="form-group">
                        {/* <label className="control-label">Name</label> */}
                        <h4 className="form-control">{this.state.groupDetail.cn}</h4>
                    </div>
                    <div className="form-group">
                        <label>Description</label>
                        <p className="form-control">{this.state.groupDetail.description}</p>
                    </div>
                </div>
            // </div>
            );
    }
}