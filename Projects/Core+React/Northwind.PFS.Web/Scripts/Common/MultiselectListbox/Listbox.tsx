import * as React from "react"
import * as ReactDOM from "react-dom"
declare var $: any;
declare var window: any;

interface IListboxPros
{
    data: any[];
    title: string;
    description?: string;
}

interface IListboxState
{
    data: any[];
}

export class Listbox extends React.Component<IListboxPros, IListboxState> {
    constructor(props: any)
    {
        super(props);
        let data: any[] = this.props.data;
        data.map((v) =>
        {
            v.selected = false;
        })
        this.state = {
            data: data
        }
        this.onItemClicked = this.onItemClicked.bind(this);
        this.selectOne = this.selectOne.bind(this);
        this.selectOnCtrlPress = this.selectOnCtrlPress.bind(this);
        this.selectOnShiftPress = this.selectOnShiftPress.bind(this);
    }

    renderOptions()
    {
        let items: any[] = [];
        this.state.data.map((d: any, i: number) =>
        {
            let css = "";
            if(d.selected) css = "selected"
            items.push(<a className={"options " + css} key={i} itemID={"" + i} onClick={this.onItemClicked}>
                {d.name}
            </a>);
        })
        return items;
    }

    selectOne(e: any)
    {
        let itemId = e.target.getAttribute("itemid");
        let data = this.state.data;
        data[itemId].selected = !data[itemId].selected;
        this.setState({ data: data })
    }

    selectOnCtrlPress(e: any)
    {

    }

    selectOnShiftPress(e: any)
    {

    }

    onItemClicked(e: any)
    {
        if (e.altKey) {
            this.selectOnCtrlPress(e);
        }
        else if (e.shiftKey) {
            this.selectOnShiftPress(e);
        }
        else {
            this.selectOne(e);
        }
    }

    render()
    {

        return (
            <div className="xylontech-list-box w-100 h-100">
                <span className="header col-12">{this.props.title}</span>
                <div className="description col-12">{this.props.description}</div>
                <div className="list col-12">
                    <div className="list-scroller">
                        <div className="w-100">
                            {this.renderOptions()}
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}