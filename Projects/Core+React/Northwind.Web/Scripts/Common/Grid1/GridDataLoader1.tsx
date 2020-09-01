import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { toODataString } from '@progress/kendo-data-query';
import { LoadingPanel } from "./LoadingPanel";
import { Remote } from '../Remote/Remote';
declare var window: any;

interface IGridLoaderProp {
    dataState: any;
    onDataRecieved: any;
    baseURL: any;
    method: any;
    switchStatus: boolean;
}
interface IGridLoaderState {
    lastSuccess: any;
    pending: any;
    componentLoaded: boolean;
}

export class GridDataLoader1 extends React.Component<IGridLoaderProp, IGridLoaderState> {

    constructor(props: IGridLoaderProp) {
        super(props);
        this.state = {
            lastSuccess: '',
            pending: '',
            componentLoaded: false
        }
    }

    reloadData() {
        this.setState({ lastSuccess: '', pending: '' }, this.requestDataIfNeeded)
    }

    requestDataIfNeeded = () => {
        if (this.state.pending || this.makeQueryString(this.props.dataState) === this.state.lastSuccess) {
            return;
        }
        let pending = this.makeQueryString(this.props.dataState);
        this.setState({ lastSuccess: pending })
        this.setState({ pending: pending });
        let caller = this;
        let separator = "?";
        if (this.props.baseURL.indexOf("?") >= 0) {
            separator = "&"
        }
        let url = this.props.baseURL + separator + pending + "&t=" + (new Date()).getTime();
        if (this.props.switchStatus == true) {
            url = this.props.baseURL + separator + pending + "&switchOn=1&t=" + (new Date()).getTime();
        }
        Remote.get(url, (json: any) => {
            if (caller.makeQueryString(caller.props.dataState) === caller.state.lastSuccess) {
                caller.setState({ pending: '' });
                this.props.onDataRecieved({
                    data: json.result,
                    total: json.count
                }, this);
            } else {
                caller.requestDataIfNeeded();
            }
        }, (error: any) => {
            window.Dialog.alert(error, "Error");
            });

    }

    componentDidMount() {
        
    }

    makeQueryString(state: any) {
        let data: string = "";
        for (let i in state) {
            if (data != "")
                data += "&"
            data += i +"=" + eval("state." + i)
        }
        return data;
    }

    componentWillMount() {
        this.requestDataIfNeeded();
        this.setState({ componentLoaded: true })
    }

    render() {
        if (this.state.componentLoaded) {
            this.requestDataIfNeeded();
        }
        return this.state.pending && <LoadingPanel />;
    }
}

