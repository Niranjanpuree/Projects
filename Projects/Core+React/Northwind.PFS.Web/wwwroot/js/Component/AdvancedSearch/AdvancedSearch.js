"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
require("core-js");
require("regenerator-runtime/runtime");
const React = require("react");
const Condition_1 = require("../Entities/Condition");
const AdvancedSearchHeader_1 = require("../AdvancedSearch/AdvancedSearchHeader");
const AdvancedSearchRow_1 = require("../AdvancedSearch/AdvancedSearchRow");
class AdvancedSearch extends React.Component {
    constructor(props) {
        super(props);
        let arr = new Array();
        this.state = {
            hidden: false,
            operators: new Array(),
            attributes: new Array(),
            rows: arr,
            selectedConditions: this.props.selectedConditions || [],
        };
        this.addNewRow = this.addNewRow.bind(this);
        this.onApply = this.onApply.bind(this);
        this.onConditionChange = this.onConditionChange.bind(this);
        this.onRemove = this.onRemove.bind(this);
    }
    render() {
        return (!this.state.hidden && React.createElement("div", null,
            React.createElement(AdvancedSearchHeader_1.AdvancedSearchHeader, null),
            this.state.rows.map(row => row),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "col" },
                    React.createElement("a", { href: "#", className: "btn btn-primary", onClick: this.addNewRow }, "Add Another Condition")))));
    }
    componentDidMount() {
        this.loadDefaults();
    }
    componentWillUpdate() {
        if (this.state.selectedConditions != this.props.selectedConditions) {
            this.setState({ selectedConditions: this.props.selectedConditions });
        }
    }
    /**
     * Loads the defaults. Fetches attributes and operators using fetch API
     * to show in Attribute dropdown and Operator Dropdown to prevent loading multiple times at
     * row level.
     * If props has selectedConditions then uses the data to populate appropriate
     * attributes and operators, otherwise loads a single row with just the attributes populated
     * should be called in componentDidMount
     * */
    loadDefaults() {
        return __awaiter(this, void 0, void 0, function* () {
            Promise.all([this.fetchAttributes(), this.fetchOperators()]).then((results) => {
                let attributes = results[0];
                let operators = results[1];
                this.setState({ attributes: attributes, operators: operators });
                if (this.props.selectedConditions && this.props.selectedConditions.length > 0) {
                    var rows = this.props.selectedConditions.map((x, index) => {
                        var rowIndex = index + 1;
                        return React.createElement(AdvancedSearchRow_1.AdvancedSearchRow, { key: rowIndex, rowIndex: rowIndex, attributes: attributes, operators: operators, selectedAttribute: x.Attribute, selectedOperator: x.Operator, selectedValue: x.Value, selectedValue2: x.Value2, onConditionChange: this.onConditionChange, onRemove: this.onRemove });
                    });
                    this.setState({ rows: rows });
                }
                else {
                    this.addNewRow();
                }
            }).catch(() => alert('Some error'));
        });
    }
    /**
     * Adds a new row component and updates the state
     * */
    addNewRow() {
        let currentRows = this.state.rows;
        let count = currentRows.length + 1;
        currentRows.push(React.createElement(AdvancedSearchRow_1.AdvancedSearchRow, { rowIndex: count, key: count, attributes: this.state.attributes, operators: this.state.operators, onConditionChange: this.onConditionChange, onRemove: this.onRemove }));
        this.setState({ rows: currentRows });
    }
    onRemove(rowIndex) {
        let currentRows = this.state.rows;
        let newRows = currentRows.filter((x, indx) => {
            return indx != rowIndex - 1;
        });
        let currentConditions = this.state.selectedConditions;
        let newConditions = currentConditions.filter((x, indx) => {
            return indx != rowIndex - 1;
        });
        this.setState({ rows: newRows, selectedConditions: newConditions });
        this.props.onConditionChange(newConditions);
    }
    /** fetch API to get the list of resource attributes for a given resource
     * @returns promise
     */
    fetchAttributes() {
        return __awaiter(this, void 0, void 0, function* () {
            let param = this.props.resourceIds.map(x => `resourceIds=${x}`).join("&");
            let result = yield fetch(`/ResourceAttribute/Get?${param}`, { headers: { 'Cache-Control': 'no-cache' } });
            let json = yield result.json();
            json = json.filter((x) => {
                return x.searchable === true;
            });
            let currentAttributeList = json.map((x) => {
                return new Condition_1.Attribute(x.resourceAttributeGuid, x.name, x.title, x.attributeType, x.isEntityLookup, x.entity);
            });
            return Promise.resolve(currentAttributeList);
        });
    }
    /**
     * fetch API to get the list of Operators
     * @returns Promise containining array of operators
     */
    fetchOperators() {
        return __awaiter(this, void 0, void 0, function* () {
            let result = yield fetch("/QueryOperator/Get", { headers: { 'Cache-Control': 'no-cache' } });
            let json = yield result.json();
            let currentOperatorList = json.map((x) => {
                return new Condition_1.Operator(x.queryOperatorGuid, x.name, x.title, x.type);
            });
            return Promise.resolve(currentOperatorList);
        });
    }
    onApply() {
        this.props.onApply(this.state.selectedConditions);
    }
    onConditionChange(rowIndex, condition) {
        let currentConditions = this.state.selectedConditions;
        if (currentConditions.length < rowIndex) {
            currentConditions.push(condition);
        }
        currentConditions[rowIndex - 1] = condition;
        this.setState({ selectedConditions: currentConditions });
        this.props.onConditionChange(currentConditions);
    }
}
exports.AdvancedSearch = AdvancedSearch;
//# sourceMappingURL=AdvancedSearch.js.map