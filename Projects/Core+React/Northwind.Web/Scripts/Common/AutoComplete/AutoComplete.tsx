import * as React from "react";
import { Remote } from "../../Common/Remote/Remote";
declare var window: any;
declare var $: any;

export declare type TooltipAligment = 'top' | 'bottom';

interface IAutoCompleteProps
{
    id: string;
    dataUrl: string;
    className: string;
    defaultValue?: any;
    onChange: any;
    onClick?: any;
    valueField: string;
    displayField: string;
    placeHolder: string;
    allowMultiple?: boolean
    selectedItems?: any[];
    showTooltip?: boolean;
    tooltipTemplate?: string;
    tooltipAlignment?: TooltipAligment;
}

interface IAutoCompleteState
{
    popupVisible: boolean;
    data: any[];
    value: string;
    values: any[];
    tooltipIndex: number;
}

export class AutoComplete extends React.Component<IAutoCompleteProps, IAutoCompleteState>{
    input: any;
    constructor(props: IAutoCompleteProps)
    {
        super(props);

        this.state = {
            popupVisible: false,
            data: [],
            value: '',
            values: this.props.selectedItems ? this.props.selectedItems : [],
            tooltipIndex: -1
        };

        this.onChange = this.onChange.bind(this);
        this.onItemClicked = this.onItemClicked.bind(this);
        this.selectItem = this.selectItem.bind(this);
        this.onKeyPressed = this.onKeyPressed.bind(this);
        this.clearValue = this.clearValue.bind(this);
        this.renderPills = this.renderPills.bind(this);
        this.removePill = this.removePill.bind(this);
        this.renderTooltipText = this.renderTooltipText.bind(this);
        this.onTooltipEnter = this.onTooltipEnter.bind(this);
        this.onTooltipExit = this.onTooltipExit.bind(this);
        this.isSelectedItem = this.isSelectedItem.bind(this);
        this.onClick = this.onClick.bind(this);
    }

    async onChange(e: any)
    {
        let value = e.target.value;
        if (value.length === 0)
            return;
        let result = await Remote.getAsync(this.props.dataUrl + "&searchValue=" + value + "&take=10");
        if (result.ok) {
            let json = await result.json();
            this.setState({ popupVisible: true, data: json }, () => {
                document.onclick = (e: any) => {
                    if (this.state.data.length > 0) {
                        this.setState({ data: [] }, () => { this.input.value = ''; this.forceUpdate })
                    }
                }
            });
        }
        
    }

    onKeyPressed(e: any)
    {
        if (e.charCode === 13) {
            if (this.state.data.length > 0) {
                this.selectItem(this.state.data[0])
            }
        }
    }

    clearValue()
    {
        this.input.value = "";
    }

    selectItem(item: any)
    {
        if (!this.props.allowMultiple) {
            this.props.onChange(item);
            this.input.value = item[this.props.displayField];
            this.setState({ popupVisible: false });
        }
        else {
            this.props.onChange(this.state.values);
            this.setState({ popupVisible: false });
        }
    }

    onItemClicked(e: any)
    {
        if (!this.props.allowMultiple) {
            let itemId = e.target.getAttribute("itemid");
            let item = this.state.data[itemId];
            this.selectItem(item);
        }
        else {
            let itemId = e.target.getAttribute("itemid");
            let item = this.state.data[itemId];
            let values: any[] = this.state.values;
            values.push(item);
            this.setState({ values: values, data: [] }, this.clearValue);
            this.selectItem(item);
        }
        
    }

    componentDidMount()
    {
        
    }

    renderPopup()
    {
        let data = this.state.data;
        if (!data.map) {
            data = [];
        }
       
        let additionalCSS = "";
        if (data.length > 0) {
            additionalCSS=" show"
        }
        if (data.length > 0) {
            return (
                <div className={"dropdown-menu" + additionalCSS} aria-labelledby={this.props.id}>
                    {data.map((v: any, i: number) =>
                    {
                        let css = this.isSelectedItem(v);
                        if (css === "") {
                            return (<a className={"dropdown-item " + css} href="#" key={i} itemID={i + ""} onClick={this.onItemClicked}>{v[this.props.displayField]}</a>)
                        }
                        else {
                            return (<a className={"dropdown-item " + css} href="#" key={i} itemID={i + ""} onClick={(e: any) => { e.preventDefault() }}>{v[this.props.displayField]}</a>)                        
                        }
                    })}
                </div>
            )
        }
        else {
            return (<div className={"dropdown-menu" + additionalCSS} aria-labelledby={this.props.id} style={{ display: 'none' }}></div>)
        }
        
    }

    clearSelection()
    {
        if (this.props.allowMultiple) {
            this.setState({ value: '', values: [] }, this.forceUpdate);
        }
    }
    
    isSelectedItem(item: any)
    {
        let css = "";
        this.state.values.map((v: any) =>
        {
            if (JSON.stringify(v) === JSON.stringify(item)) {
                css = "selected";
            }
        })
        return css;
    }

    removePill(e: any)
    {
        let itemId = e.currentTarget.getAttribute("itemid");
        let values = this.state.values;
        delete values[itemId];
        let newValues: any[] = [];
        for (let i in values) {
            if (values[i] !== null) {
                newValues.push(values[i])
            }
        }
        this.setState({ values: newValues });
        this.selectItem({});
    }

    renderTooltipText(index: number)
    {
        if (index == this.state.tooltipIndex) {
            let data = this.state.values[index];
            let alignment = this.props.tooltipAlignment || "bottom";
            return (<div key={index} className={"xylon-tooltip " + alignment}>
                {eval(this.props.tooltipTemplate)}
                </div>)
        }
        
    }

    onTooltipEnter(e: any)
    {
        let itemId = e.currentTarget.getAttribute("itemid")
        this.setState({ tooltipIndex: itemId }, this.forceUpdate)
    }

    onTooltipExit(e: any)
    {
        this.setState({ tooltipIndex: -1 }, this.forceUpdate)
    }

    renderPills()
    {
        let items: any[] = [];
        this.state.values.map((v: any, i: number) =>
        {
            if (!this.props.showTooltip || !this.props.allowMultiple) {
                items.push(<span key={i} className="multiselect-pills">{v[this.props.displayField]} <a href="javascript:void(0)" itemID={i + ""} onClick={this.removePill}><i className="k-icon k-i-close"></i></a></span>)
            }
            else {
                items.push(<div key={i} className="multiselect-pills" onMouseMove={this.onTooltipEnter} onMouseOut={this.onTooltipExit} itemID={i + ""}>
                    {this.renderTooltipText(i)}
                    {v[this.props.displayField]} <a href="javascript:void(0)" itemID={i + ""} onClick={this.removePill}><i className="k-icon k-i-close"></i></a>
                </div>)
            }
        })
        return (items)
    }

    async onClick(e: any)
    {
        this.input.focus();
        let result = await Remote.getAsync(this.props.dataUrl + "&searchValue=a&take=10");
        if (result.ok) {
            let json = await result.json();
            this.setState({ popupVisible: true, data: json }, () => {
                document.onclick = (e: any) => {
                    if (this.state.data.length > 0) {
                        this.setState({ data: [] }, () => { this.input.value = ''; this.forceUpdate })
                    }
                }
            });
        }
    }
    
    render()
    {
        if (!this.props.allowMultiple) {
            return (<span className={this.props.className} style={{ zIndex: 1 }} onClick={this.onClick}>
                <input ref={(c) => { this.input = c; }} type="text" className="dropdown-toggle" style={{ width: '100%', borderWidth: 0, outline: 0 }} onChange={this.onChange} placeholder={this.props.placeHolder} onKeyPress={this.onKeyPressed} id={this.props.id} data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" />
                {this.renderPopup()}
            </span>);
        }
        else {
            return (<div className={this.props.className + " multi-selectbox"} style={{ zIndex: 1 }} onClick={this.onClick}>
                {this.renderPills()}
                <input ref={(c) => { this.input = c; }} type="text" className="dropdown-toggle entryField" style={{ width: 'auto', borderWidth: 0, outline: 0 }} onChange={this.onChange} placeholder={this.props.placeHolder} onKeyPress={this.onKeyPressed} id={this.props.id} data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" />
                {this.renderPopup()}
            </div>);
        }
        
    }

}