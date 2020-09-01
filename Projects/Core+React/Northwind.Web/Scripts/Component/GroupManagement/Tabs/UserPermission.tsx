import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";
import { LoadingPanel } from "../../../Common/Grid/LoadingPanel";
import { count } from "@progress/kendo-data-query/dist/npm/array.operators";
import { number } from "prop-types";

declare var window: any;

interface IUserPermissionProp {
    groupGuid: any;
    userName: string;
    

}

interface IUserPermissionState {
    resources: any[];
    pending: boolean;
    actionAccessCount: any;
}

export class UserPermission extends React.Component<IUserPermissionProp, IUserPermissionState>{

    constructor(props: IUserPermissionProp) {
        super(props);

        this.state = {
            resources: [],
            pending: false,
            actionAccessCount:0
        }

        this.renderResource = this.renderResource.bind(this);
        this.renderResourceAction = this.renderResourceAction.bind(this);
        
        //this.init()
    }

    componentDidMount() {
        $('#loading').show();
        this.init();
        this.getAccessCount();
        
    }

    async init() {
        this.setState({ pending: true });
        let result = await Remote.getAsync(`user/UserPermissions?userGuid=${this.props.groupGuid}`);
        if (result.ok) {
            let json = await result.json();
            this.setState({ resources: json, pending: false }, this.forceUpdate);
            $('#loading').hide();
        }
        else {
            $('#loading').hide();
            let s = await result.text();
            let error = await Remote.parseErrorMessage(result);
            this.setState({ pending: false }, this.forceUpdate);
            window.Dialog.alert(error);
        }
    }



    renderResourceAction(resource: any, index: number, appIndex: number) {
        let items: any[] = [];
        
        resource.actions.map((v: any, i: number) => {
            
           
            items.push(<li key={"a" + appIndex + "r" + index + "a" + i}>
                <div className="form-group">
                    <input className="k-checkbox" type="checkbox" name="actions" id={resource.name + v.name} value={v.actionGuid} checked={v.selected} readOnly />
                    <label className=" k-checkbox-label" htmlFor={resource.name + v.name}>
                        {v.title}
                    </label>
                </div></li>);
        })
        console.log(count)
       // this.setState({ actionAccessCount: count})
        return items;
    }
    renderResource(resourece: any, index: number) {
        let items: any[] = [];
        resourece.resources.map((v: any, i: number) => {
            items.push(<li key={"a" + index + "r" + i} className="mb-2 col-md-4 col-sm-6">
                <h5 className="gray-700">{v.title}</h5>
                <ul className="list-unstyled">
                    {this.renderResourceAction(v, i, index)}
                </ul>
            </li>);
        })

        return items;
    }
   
    renderApplication() {
        let items: any[] = [];
        this.state.resources.map((v: any, i: number) => {
            items.push(<div key={"col" + i} className="mt-3">
                <h4>{v.application}</h4>
                <ul className="list-unstyled row pl-0">
                    {this.renderResource(v, i)}
                </ul>
  </div>);
        })
        return items;
    }

    getAccessCount() {
        let sender = this;
        Remote.get("/iam/user/AssignedGroupsCount/" + this.props.groupGuid, (data: any) => {
            sender.setState({ actionAccessCount: data.count });
        }, (data: any) => { window.Dialog.alert(data); })
    }

    render() {
        return (<form method="post">
            <div>
                {this.state.resources.length > 0 && <div className="text-muted">{this.props.userName} has permissions to following {this.state.actionAccessCount} actions. These actions are not editable at the user level and are driven by user’s group permissions. If you are not sure why a user has access to certain actions and not to others, please review permissions for each group that user belongs to. </div>}
                {this.state.resources.length == 0 && <div className="text-muted">{this.props.userName} doesn’t belong to any groups and thus have no permissions in the system”  </div>}
                
                {this.renderApplication()}
            
                {this.state.pending && <LoadingPanel />}
            </div></form>);
    }
}