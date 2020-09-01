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
var kendo_react_dropdowns_1 = require("@progress/kendo-react-dropdowns");
var kendo_react_inputs_1 = require("@progress/kendo-react-inputs");
var kendo_react_dateinputs_1 = require("@progress/kendo-react-dateinputs");
var AdvancedSearchHeader = /** @class */ (function (_super) {
    __extends(AdvancedSearchHeader, _super);
    function AdvancedSearchHeader() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    AdvancedSearchHeader.prototype.render = function () {
        return (React.createElement("div", { className: "row form-group" },
            React.createElement("div", { className: "col-sm-4" },
                React.createElement("label", { htmlFor: "conditionAttribute" }, "Attribute")),
            React.createElement("div", { className: "col-sm-4" },
                React.createElement("label", { htmlFor: "Operator" }, "Operator")),
            React.createElement("div", { className: "col-sm-4" },
                React.createElement("label", { htmlFor: "Value" }, "Value"))));
    };
    return AdvancedSearchHeader;
}(React.Component));
exports.AdvancedSearchHeader = AdvancedSearchHeader;
var AdvancedSearchAddNewRow = /** @class */ (function (_super) {
    __extends(AdvancedSearchAddNewRow, _super);
    function AdvancedSearchAddNewRow() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    AdvancedSearchAddNewRow.prototype.render = function () {
        return (React.createElement("div", { className: "row" },
            React.createElement("div", { className: "col" },
                React.createElement("a", { href: "#" }, "Add Another Condition"))));
    };
    return AdvancedSearchAddNewRow;
}(React.Component));
exports.AdvancedSearchAddNewRow = AdvancedSearchAddNewRow;
var AdvancedSearch = /** @class */ (function (_super) {
    __extends(AdvancedSearch, _super);
    function AdvancedSearch(props) {
        var _this = _super.call(this, props) || this;
        var arr = new Array();
        _this.state = {
            hidden: false,
            operators: new Array(),
            attributes: new Array(),
            rows: arr,
            selectedConditions: [],
        };
        _this.addRow = _this.addRow.bind(_this);
        return _this;
    }
    AdvancedSearch.prototype.render = function () {
        return (!this.state.hidden && React.createElement("div", { className: "container-fluid" },
            React.createElement(AdvancedSearchHeader, null),
            this.state.rows.map(function (row) { return row; }),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col" },
                    React.createElement("a", { href: "#", onClick: this.addRow }, "Add Another Condition")))));
    };
    AdvancedSearch.prototype.componentDidMount = function () {
        var _this = this;
        var currentAttributeList = this.state.attributes;
        var currentOperatorList = this.state.operators;
        fetch("/ResourceAttribute/Get?resourceIds=Contract")
            .then(function (res) { return res.json(); })
            .then(function (result) {
            currentAttributeList = result.map(function (x) {
                return new Attribute(x.resourceAttributeGuid, x.name, x.title, x.attributeType);
            });
            _this.setState({ attributes: currentAttributeList });
            _this.addRow();
        });
        fetch("/QueryOperator/Get")
            .then(function (res) { return res.json(); })
            .then(function (result) {
            currentOperatorList = result.map(function (x) {
                return new Operator(x.queryOperatorGuid, x.name, x.title, x.type);
            });
            _this.setState({ operators: currentOperatorList });
        });
    };
    AdvancedSearch.prototype.addRow = function () {
        var currentRows = this.state.rows;
        var count = currentRows.length + 1;
        currentRows.push(React.createElement(AdvancedSearchRow, { key: count, attributes: this.state.attributes, operators: this.state.operators }));
        this.setState({ rows: currentRows });
    };
    return AdvancedSearch;
}(React.Component));
exports.AdvancedSearch = AdvancedSearch;
var AdvancedSearchRow = /** @class */ (function (_super) {
    __extends(AdvancedSearchRow, _super);
    function AdvancedSearchRow(props) {
        var _this = _super.call(this, props) || this;
        _this.state =
            {
                attributeValue: { text: 'Select attribute...', id: '' },
                operatorValue: { text: 'Select operator...', id: '' },
                operatorData: [],
                attributeData: [],
                inputControl: React.createElement(kendo_react_inputs_1.Input, null),
                inputControl2: React.createElement("span", null)
            };
        _this.onAttributeChange = _this.onAttributeChange.bind(_this);
        _this.onOperatorChange = _this.onOperatorChange.bind(_this);
        return _this;
    }
    AdvancedSearchRow.prototype.render = function () {
        return (React.createElement("div", { className: "row form-group" },
            React.createElement("div", { className: "col-sm-3" },
                React.createElement(kendo_react_dropdowns_1.DropDownList, { data: this.state.attributeData, value: this.state.attributeValue, textField: "text", dataItemKey: "id", onChange: this.onAttributeChange })),
            React.createElement("div", { className: "col-sm-3" },
                React.createElement(kendo_react_dropdowns_1.DropDownList, { data: this.state.operatorData, value: this.state.operatorValue, textField: "text", dataItemKey: "id", onChange: this.onOperatorChange })),
            React.createElement("div", { className: "col-sm-3" }, this.state.inputControl),
            React.createElement("div", { className: "col-sm-3" }, this.state.inputControl2)));
    };
    AdvancedSearchRow.prototype.componentDidMount = function () {
        this.setState({ attributeData: this.getAttributeData() });
    };
    AdvancedSearchRow.prototype.getAttributeData = function () {
        var attributeData = this.props.attributes.map(function (x) {
            var item = {};
            item["text"] = x.AttributeTitle;
            item["id"] = x.AttributeName;
            return item;
        });
        return attributeData;
    };
    AdvancedSearchRow.prototype.getOperatorData = function (operators) {
        var attributeData = operators.map(function (x) {
            var item = {};
            item["text"] = x.OperatorTitle;
            item["id"] = x.OperatorName;
            return item;
        });
        return attributeData;
    };
    AdvancedSearchRow.prototype.onAttributeChange = function (event) {
        var defaultOperatorValue = { text: 'Select operator...', id: '' };
        var selectedAttribute = this.props.attributes.filter(function (y) {
            return y.AttributeName == event.target.value.id;
        });
        var operatorData = this.props.operators.filter(function (x) {
            return x.OperatorType == selectedAttribute[0].AttributeType;
        });
        operatorData = this.getOperatorData(operatorData);
        this.setState({ attributeValue: event.target.value, operatorData: operatorData, operatorValue: defaultOperatorValue });
    };
    AdvancedSearchRow.prototype.onOperatorChange = function (event) {
        var _this = this;
        this.setState({ operatorValue: event.target.value });
        //find out attribute and operator type, so we can render correct controls for values
        var attributeType = 1;
        var operatorType = 1;
        var selectedAttribute = this.props.attributes.filter(function (y) {
            return y.AttributeName == _this.state.attributeValue.id;
        });
        var selectedOperator = this.props.operators.filter(function (y) {
            return y.OperatorName == event.target.value.id;
        });
        if (selectedAttribute && selectedAttribute[0]) {
            attributeType = selectedAttribute[0].AttributeType;
        }
        if (selectedOperator && selectedOperator[0]) {
            operatorType = selectedOperator[0].OperatorName;
        }
        console.log('attrType:' + attributeType);
        console.log('operatorType:' + operatorType);
        //TODO: Remove all numeric reference and create enum. 
        var inputControl = React.createElement(kendo_react_inputs_1.Input, null);
        var inputControl2 = React.createElement("span", null);
        switch (attributeType) {
            case 1:
            case 2:
            case 3:
                inputControl = React.createElement(kendo_react_inputs_1.Input, null);
                if (operatorType == 21 || operatorType == 29) {
                    inputControl2 = React.createElement(kendo_react_inputs_1.Input, null);
                }
                break;
            case 4:
                inputControl = React.createElement(kendo_react_dateinputs_1.DatePicker, null);
                if (operatorType == 13) {
                    inputControl2 = React.createElement(kendo_react_dateinputs_1.DatePicker, null);
                }
                break;
            case 5:
                inputControl = React.createElement(kendo_react_dropdowns_1.DropDownList, null);
            case 6:
                inputControl = React.createElement(kendo_react_dropdowns_1.MultiSelect, null);
        }
        ;
        this.setState({ inputControl: inputControl, inputControl2: inputControl2 });
    };
    return AdvancedSearchRow;
}(React.Component));
exports.AdvancedSearchRow = AdvancedSearchRow;
var Condition = /** @class */ (function () {
    function Condition() {
    }
    return Condition;
}());
var Resource = /** @class */ (function () {
    function Resource() {
    }
    return Resource;
}());
var Attribute = /** @class */ (function () {
    function Attribute(attributeId, attributeName, attributeTitle, attributeType) {
        this.AttributeId = attributeId;
        this.AttributeName = attributeName;
        this.AttributeTitle = attributeTitle;
        this.AttributeType = attributeType;
    }
    return Attribute;
}());
var Operator = /** @class */ (function () {
    function Operator(operatorId, operatorName, operatorTitle, operatorType) {
        this.OperatorId = operatorId;
        this.OperatorName = operatorName;
        this.OperatorTitle = operatorTitle;
        this.OperatorType = operatorType;
    }
    return Operator;
}());
//# sourceMappingURL=AdvancedSearch.js.map