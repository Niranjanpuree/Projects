"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var LoadingPanel_1 = require("./LoadingPanel");
var GridDataLoader = /** @class */ (function (_super) {
    __extends(GridDataLoader, _super);
    function GridDataLoader(props) {
        var _this = _super.call(this, props) || this;
        _this.requestDataIfNeeded = function () {
            if (_this.state.pending || _this.makeQueryString(_this.props.dataState) === _this.state.lastSuccess) {
                return;
            }
            var pending = _this.makeQueryString(_this.props.dataState);
            _this.setState({ lastSuccess: pending });
            _this.setState({ pending: pending });
            var caller = _this;
            fetch(_this.props.baseURL + "?" + pending, { method: _this.props.method })
                .then(function (response) { return response.json(); })
                .then(function (json) {
                if (caller.makeQueryString(caller.props.dataState) === caller.state.lastSuccess) {
                    caller.setState({ pending: '' });
                    _this.props.onDataRecieved({
                        data: json.result,
                        total: json.count
                    }, _this);
                }
                else {
                    caller.requestDataIfNeeded();
                }
            });
        };
        _this.state = {
            lastSuccess: '',
            pending: '',
            componentLoaded: false
        };
        return _this;
    }
    GridDataLoader.prototype.reloadData = function () {
        this.setState({ lastSuccess: '', pending: '' }, this.requestDataIfNeeded);
    };
    GridDataLoader.prototype.componentDidMount = function () {
    };
    GridDataLoader.prototype.makeQueryString = function (state) {
        var data = "";
        for (var i in state) {
            if (data != "")
                data += "&";
            data += i + "=" + eval("state." + i);
        }
        return data;
    };
    GridDataLoader.prototype.componentWillMount = function () {
        this.requestDataIfNeeded();
        this.setState({ componentLoaded: true });
    };
    GridDataLoader.prototype.render = function () {
        if (this.state.componentLoaded) {
            this.requestDataIfNeeded();
        }
        return this.state.pending && React.createElement(LoadingPanel_1.LoadingPanel, null);
    };
    return GridDataLoader;
}(React.Component));
exports.GridDataLoader = GridDataLoader;
//# sourceMappingURL=GridDataLoader.js.map