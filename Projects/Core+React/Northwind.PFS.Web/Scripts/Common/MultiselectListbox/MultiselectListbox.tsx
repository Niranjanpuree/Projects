import * as React from "react"
import * as ReactDOM from "react-dom"
import { Listbox } from "./Listbox"
declare var $: any;
declare var window: any;

interface IMultiselectListboxPros
{
    data: any[];
    selectedData: any[];
}

interface IMultiselectListboxState
{
    selectedData: any[];
}

export class MultiselectListbox extends React.Component<IMultiselectListboxPros, IMultiselectListboxState> {

    constructor(props: any)
    {
        super(props);

    }

    render()
    {

        return (
            <div className="w-100 row multiselect-listbox">
                <div className="col-5">
                    <Listbox data={this.props.data} title="All Ventors" description="Please select vendors and move right to select." />
                </div>
                <div className="col-2 text-center row" style={{ marginLeft: '15px' }}>
                    <div className="col-12 text-center"><a className="action-button" href="#">Up</a></div>
                    <div className="col-12 text-center"><a className="action-button" href="#">Down</a></div>
                    <div className="col-12 text-center"><a className="action-button" href="#">&gt;</a></div>
                    <div className="col-12 text-center"><a className="action-button" href="#">&gt;&gt;</a></div>
                    <div className="col-12 text-center"><a className="action-button" href="#">&lt;</a></div>
                    <div className="col-12 text-center"><a className="action-button" href="#">&lt;&lt;</a></div>
                </div>
                <div className="col-5">
                    <Listbox data={this.props.data} title="Selected Vendors" description="Your selected vendors to search" />
                </div>
            </div>
        );
    }
}