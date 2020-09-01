import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../../../Common/Remote/Remote";
declare var $: any;
declare var window: any;

interface IBillingTabPros
{
    projectId: any;
}

interface IBillingTabState
{

}

export class BillingTab extends React.Component<IBillingTabPros, IBillingTabState> {
    constructor(props: any)
    {
        super(props);

    }

    render()
    {

        return (
            <div>POTab projectid: {this.props.projectId}</div>
        );
    }
}