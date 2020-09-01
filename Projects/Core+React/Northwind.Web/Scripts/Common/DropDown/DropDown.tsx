import 'core-js';
import * as React from "react"
import { Remote } from "../../Common/Remote/Remote"
import { DropDownList } from '@progress/kendo-react-dropdowns';
import { filterBy } from '@progress/kendo-data-query';
declare var window: any;
declare var $: any;

interface IDropDownProps {
    dataUrl: string,
    dataVal: any,
    textVal: any,
    defaultItem:any
}

interface IDropDownState {
    value: any,
    singleValue: any,
    sourceValue: any[],
    isTooltip: boolean,
    mouseEvent: any;
    dataList: any[];
}

export class DropDown extends React.Component<IDropDownProps, IDropDownState> {
    dataList: any[];
    constructor(props: any) {
        super(props);

        this.state = {
            value: null,
            sourceValue: [],
            singleValue: '',
            isTooltip: false,
            mouseEvent: null,
            dataList: [],
        };
    }

    getData() {
        let sender = this;
        Remote.get(this.props.dataUrl,
            (response: any) => {
                sender.dataList = response.result;
                sender.setState({
                    sourceValue: response.result,
                });
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    onChange = (event: any) => {
        this.setState({
            value: event.target.value,
            mouseEvent: event
        });
    }

    componentDidMount() {
        this.getData();
    }

    onFilterChange = (event: any) => {
        this.setState({
            sourceValue: filterBy(this.dataList, event.filter)
        });
    }
    render() {
        return (
            <div className="DropDown-wrapper">
                <DropDownList
                    data={this.state.sourceValue}
                    onChange={this.onChange}
                    value={this.state.value}
                    filterable={true}
                    onFilterChange={this.onFilterChange}
                    textField={this.props.textVal}
                    dataItemKey={this.props.dataVal}
                    defaultItem={this.props.defaultItem}
                />
            </div>
        );
    }
}