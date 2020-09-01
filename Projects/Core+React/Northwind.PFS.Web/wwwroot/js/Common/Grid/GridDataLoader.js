"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const LoadingPanel_1 = require("./LoadingPanel");
const Remote_1 = require("../Remote/Remote");
class GridDataLoader extends React.Component {
    constructor(props) {
        super(props);
        this.requestDataIfNeeded = () => {
            if (this.state.pending || (this.makeQueryString(this.props.dataState) + JSON.stringify(this.props.postValue)) === this.state.lastSuccess) {
                return;
            }
            let pending = this.makeQueryString(this.props.dataState) + JSON.stringify(this.props.postValue);
            this.setState({ lastSuccess: pending });
            this.setState({ pending: pending });
            pending = this.makeQueryString(this.props.dataState);
            let caller = this;
            let separator = "?";
            if (this.props.baseURL.indexOf("?") >= 0) {
                separator = "&";
            }
            let url = this.props.baseURL + separator + pending + "&t=" + (new Date()).getTime();
            if (this.props.switchStatus == true) {
                url = this.props.baseURL + separator + pending + "&switchOn=1&t=" + (new Date()).getTime();
            }
            this.url = url;
            if (this.props.method.toLowerCase() === "post" || (this.props.postValue && this.props.postValue.length > 0)) {
                Remote_1.Remote.postData(url, this.props.postValue, (json) => {
                    if (caller.makeQueryString(caller.props.dataState) + JSON.stringify(caller.props.postValue) === caller.state.lastSuccess) {
                        caller.setState({ pending: '' });
                        caller.props.onDataRecieved({
                            data: json.result,
                            total: json.count
                        }, caller);
                    }
                    else {
                        caller.requestDataIfNeeded();
                    }
                }, (error) => {
                    caller.setState({ pending: '' });
                    this.props.onDataRecieved({
                        data: [],
                        total: 0
                    }, this);
                    window.Dialog.alert(error, "Error");
                });
            }
            else {
                Remote_1.Remote.get(url, (json) => {
                    if (caller.makeQueryString(caller.props.dataState) + JSON.stringify(this.props.postValue) === caller.state.lastSuccess) {
                        caller.setState({ pending: '' });
                        this.props.onDataRecieved({
                            data: json.result,
                            total: json.count
                        }, this);
                    }
                    else {
                        caller.requestDataIfNeeded();
                    }
                }, (error) => {
                    caller.setState({ pending: '' });
                    this.props.onDataRecieved({
                        data: [],
                        total: 0
                    }, this);
                    window.Dialog.alert(error, "Error");
                });
            }
        };
        this.state = {
            lastSuccess: '',
            pending: '',
            componentLoaded: false
        };
        this.reloadData = this.reloadData.bind(this);
        this.getDataUrl = this.getDataUrl.bind(this);
    }
    getDataUrl() {
        return this.url;
    }
    reloadData() {
        this.setState({ lastSuccess: '', pending: '' }, this.requestDataIfNeeded);
    }
    componentDidMount() {
    }
    makeQueryString(state) {
        let data = "";
        for (let i in state) {
            if (data != "")
                data += "&";
            data += i + "=" + eval("state." + i);
        }
        return data;
    }
    componentWillMount() {
        this.requestDataIfNeeded();
        this.setState({ componentLoaded: true });
    }
    shouldComponentUpdate() {
        if (this.state.componentLoaded) {
            this.requestDataIfNeeded();
            return true;
        }
        return false;
    }
    render() {
        return this.state.pending && React.createElement(LoadingPanel_1.LoadingPanel, null);
    }
}
exports.GridDataLoader = GridDataLoader;
//# sourceMappingURL=GridDataLoader.js.map