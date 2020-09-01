import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
declare var $: any;
declare var window: any;

interface IRAMTabPros
{
    projectId: any;
}

interface IRAMTabState
{

}

export class RAMTab extends React.Component<IRAMTabPros, IRAMTabState> {
    constructor(props: any)
    {
        super(props);

    }

    render()
    {

        return (
            <div>RAMTab projectid: {this.props.projectId}</div>
        );
    }
}