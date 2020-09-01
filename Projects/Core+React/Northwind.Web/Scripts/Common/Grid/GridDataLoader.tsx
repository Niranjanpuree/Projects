import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { toODataString } from '@progress/kendo-data-query';
import { LoadingPanel } from "./LoadingPanel";
import { Remote } from '../Remote/Remote';
declare var window: any;

interface IGridLoaderProp {
    dataState: any;
    postValue?: any;
    onDataRecieved: any;
    baseURL: any;
    method: any;
    switchStatus: boolean;
}
interface IGridLoaderState {
    lastSuccess: any;
    pending: any;
}

export class GridDataLoader extends React.Component<IGridLoaderProp, IGridLoaderState> {
    url: string;
    componentLoaded: boolean;
    timeoutRecords: any;
    delay: 500;

    constructor(props: IGridLoaderProp) {
        super(props);
        this.state = {
            lastSuccess: '',
            pending: ''
        }
        this.reloadData = this.reloadData.bind(this);
        this.getDataUrl = this.getDataUrl.bind(this);
    }

    getDataUrl() {
        return this.url;
    }

    reloadData() {
        this.setState({ lastSuccess: '', pending: '' }, this.requestDataIfNeeded)
    }

    requestDataIfNeeded = () => {
        if (this.state.pending || (this.makeQueryString(this.props.dataState) + JSON.stringify(this.props.postValue)) === this.state.lastSuccess) {
            return;
        }
        let pending = this.makeQueryString(this.props.dataState) + JSON.stringify(this.props.postValue);
        this.setState({ lastSuccess: pending })
        this.setState({ pending: pending });
        pending = this.makeQueryString(this.props.dataState);
        let caller = this;
        let separator = "?";
        if (this.props.baseURL.indexOf("?") >= 0) {
            separator = "&"
        }
        let url = this.props.baseURL + separator + pending + "&t=" + (new Date()).getTime();
        if (this.props.switchStatus == true) {
            url = this.props.baseURL + separator + pending + "&switchOn=1&t=" + (new Date()).getTime();
        }
        this.url = url;

        if (this.props.method.toLowerCase() === "post" || (this.props.postValue && this.props.postValue.length > 0)) {
            Remote.postData(url, this.props.postValue, (json: any) => {
                if (caller.makeQueryString(caller.props.dataState) + JSON.stringify(caller.props.postValue) === caller.state.lastSuccess) {
                    caller.setState({ pending: '' });
                    caller.props.onDataRecieved({
                        data: json.result,
                        total: json.count,
                        message: json.message
                    }, caller);
                } else {
                    caller.requestDataIfNeeded();
                }
            }, (error: any) => {
                caller.setState({ pending: '' });
                this.props.onDataRecieved({
                    data: [],
                    total: 0,
                    message: error
                }, this);
            });
        }
        else {
            Remote.get(url, (json: any) => {
                if (caller.makeQueryString(caller.props.dataState) + JSON.stringify(this.props.postValue) === caller.state.lastSuccess) {
                    caller.setState({ pending: '' });
                    this.props.onDataRecieved({
                        data: json.result,
                        total: json.count,
                        message: json.message
                    }, this);
                } else {
                    caller.requestDataIfNeeded();
                }
            }, (error: any) => {
                caller.setState({ pending: '' });
                this.props.onDataRecieved({
                    data: [],
                    total: 0,
                    message: error
                }, this);

            });
        }

    }

    componentDidMount() {

    }

    makeQueryString(state: any) {
        let data: string = "";
        for (let i in state) {
            if (data != "")
                data += "&"
            data += i + "=" + eval("state." + i)
        }
        return data;
    }

    componentWillMount() {
        let sender = this;
        this.componentLoaded = true;
        clearTimeout(this.timeoutRecords);
        this.timeoutRecords = setTimeout(() => {
            sender.requestDataIfNeeded();
        }, this.delay);

    }

    shouldComponentUpdate() {
        let sender = this;
        if (this.componentLoaded) {
            clearTimeout(this.timeoutRecords);
            this.timeoutRecords = setTimeout(() => {
                sender.requestDataIfNeeded();
            }, this.delay);
            return true;
        }
        return false;
    }

    render() {
        return this.state.pending && <LoadingPanel />;
    }
}

