import * as React from "react";
import { Remote } from "../../../Common/Remote/Remote";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Button } from "@progress/kendo-react-buttons";
import { TabStrip, TabStripContent, TabStripTab } from "@progress/kendo-react-layout";
import { LoadingPanel } from "../../../Common/Grid/LoadingPanel";
import { load } from "@telerik/kendo-intl";
declare var window: any;

interface IGroupPermissionProp {
    groupGuid: any;
    showSaveButton: any;
}

interface IGroupPermissionState {
    resources: any[];
    pending: boolean;
}

export class GroupPermission extends React.Component<IGroupPermissionProp, IGroupPermissionState>{

    constructor(props: IGroupPermissionProp) {
        super(props);

        this.state = {
            resources: [],
            pending:false
        }

        this.renderResource = this.renderResource.bind(this);
        this.renderResourceAction = this.renderResourceAction.bind(this);
        this.renderApplication = this.renderApplication.bind(this);
    }

    async componentDidMount() {
        this.setState({ pending: true });
        this.props.showSaveButton(true, this);
        let result = await Remote.getAsync(`/iam/group/resourceGroup?groupGuid=${this.props.groupGuid}`);
        if (result.ok) {
            let json = await result.json();
            this.setState({ resources: json, pending: false }, this.forceUpdate);
        }
        else {
            let s = await result.text();
            let error = await Remote.parseErrorMessage(result);
            this.setState({ pending: false }, this.forceUpdate);
            window.Dialog.alert(error);
        }
    }
        
    renderResourceAction(resource: any, index: number, appIndex: number) {
        let items: any[] = [];
        resource.actions.map((v: any, i: number) => {
            items.push(<li key={"a" + appIndex+"r" + index + "a" + i}>
                <div className="form-group">
                    <input className="k-checkbox" type="checkbox" name="actions" id={resource.name + v.name} value={v.actionGuid} checked={v.selected} onChange={(e: any) => {
                        let arr:any[] = this.state.resources;
                        console.log(this.state.resources)
                        arr[appIndex].resources[index].actions[i].selected = e.target.checked;
                        this.setState({ resources: arr });
                    }} />
                    <label className="k-checkbox-label" htmlFor={resource.name + v.name}>
                        {v.title}
                    </label>
                </div></li>);
        })
        return items;
    }

    renderResource(resourece: any, index: number) {
        let items: any[] = [];
        resourece.resources.map((v: any, i: number) => {
            items.push(<li key={"a"+index+"r"+i} className="mb-2 col-md-4 col-sm-6">
                <h5 className="gray-700">{v.title}</h5>
                <ul className="list-unstyled">
                    {this.renderResourceAction(v,i, index)}
                </ul>
            </li>);
        })

        return items;
    }

    renderApplication() {
        let items: any[] = [];
        this.state.resources.map((v: any, i: number) => {
            items.push(<div key={"col" + i} className="">
                <h4>{v.application}</h4>
                    <ul className="list-unstyled row pl-0">
                        {this.renderResource(v, i)}
                    </ul>
            </div>);
        })
        return items;
    }

    render() {
        return (<form method="post">
            <div>
                {this.renderApplication()}
                {this.state.pending && <LoadingPanel />}
        </div></form>);
    }

}
