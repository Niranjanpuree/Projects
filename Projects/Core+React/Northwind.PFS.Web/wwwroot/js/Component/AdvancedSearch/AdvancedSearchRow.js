"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_dropdowns_1 = require("@progress/kendo-react-dropdowns");
const kendo_react_inputs_1 = require("@progress/kendo-react-inputs");
const kendo_react_dateinputs_1 = require("@progress/kendo-react-dateinputs");
const Condition_1 = require("../Entities/Condition");
const DropdownListItem_1 = require("../Entities/DropdownListItem");
const Remote_1 = require("../../Common/Remote/Remote");
class AdvancedSearchRow extends React.Component {
    constructor(props) {
        super(props);
        this.defaultAttributeDropdown = new DropdownListItem_1.DropdownListItem("", "Select attribute...");
        this.defaultOperatorDropdown = new DropdownListItem_1.DropdownListItem("", "Select operator...");
        this.state =
            {
                attributeValue: this.defaultAttributeDropdown,
                operatorValue: this.defaultOperatorDropdown,
                operatorData: [],
                attributeData: [],
                condition: {
                    IsEntity: false
                },
                selectedValue: "",
                selectedValue2: "",
                attributeOptions: []
            };
        this.onAttributeChange = this.onAttributeChange.bind(this);
        this.onOperatorChange = this.onOperatorChange.bind(this);
        this.onValueChange = this.onValueChange.bind(this);
        this.onValue2Change = this.onValue2Change.bind(this);
        //this.generateControlsByAttributeAndOperatorType = this.generateControlsByAttributeAndOperatorType.bind(this);
        this.renderValue = this.renderValue.bind(this);
        this.renderValue2 = this.renderValue2.bind(this);
        this.remove = this.remove.bind(this);
        this.isDate = this.isDate.bind(this);
    }
    render() {
        return (React.createElement("div", { className: "row form-group" },
            React.createElement("div", { className: "col-sm-3" },
                React.createElement(kendo_react_dropdowns_1.DropDownList, { className: "form-control", data: this.state.attributeData, value: this.state.attributeValue, textField: "text", dataItemKey: "id", onChange: this.onAttributeChange })),
            React.createElement("div", { className: "col-sm-3" },
                React.createElement(kendo_react_dropdowns_1.DropDownList, { className: "form-control", data: this.state.operatorData, value: this.state.operatorValue, textField: "text", dataItemKey: "id", onChange: this.onOperatorChange })),
            React.createElement("div", { className: "col-sm-2" }, this.renderValue()),
            React.createElement("div", { className: "col-sm-2" }, this.renderValue2()),
            React.createElement("div", { className: "col-sm-2 text-right" },
                React.createElement("a", { href: "#", onClick: this.remove },
                    React.createElement("i", { className: "k-icon k-i-delete" })))));
    }
    /**
     * Given the list of properities of attributes and operators,
     * we need to load the dropdownlist items and change their
     * selected state.*/
    componentDidMount() {
        //translate given array of attributes to array of dropdownlist item 
        let attrList = this.translateAttributesToDropDown(this.props.attributes);
        let selectedAttribute = this.props.selectedAttribute;
        let selectedOperator = this.props.selectedOperator;
        let selectedValue = this.props.selectedValue;
        let selectedValue2 = this.props.selectedValue2;
        let attributeValue = this.state.attributeValue;
        let operatorValue = this.state.operatorValue;
        let filteredOprList = new Array();
        //if any selected attribute is present in props
        //then show this as selected attribute in dropdown 
        //filter the list of operators based on that attribute type
        //also show appropriate control for values
        if (selectedAttribute) {
            attributeValue = new DropdownListItem_1.DropdownListItem(selectedAttribute.AttributeName, selectedAttribute.AttributeTitle);
            filteredOprList = this.filterOperatorByAttributeType(this.props.operators, selectedAttribute.AttributeType);
            operatorValue = new DropdownListItem_1.DropdownListItem(selectedOperator.OperatorName, selectedOperator.OperatorTitle);
            if (selectedAttribute && (selectedAttribute.AttributeType === 6 || selectedAttribute.AttributeType === 7 || selectedAttribute.AttributeType === 5)) {
                let postValue = {
                    attributeId: selectedAttribute.AttributeId,
                    isEntityLookup: selectedAttribute.IsEntityLookyup,
                    entity: selectedAttribute.Entity,
                };
                Remote_1.Remote.postData("/ResourceAttribute/GetOptions", postValue, (data) => {
                    this.setState({
                        attributeData: attrList,
                        operatorValue: operatorValue,
                        attributeValue: attributeValue,
                        operatorData: filteredOprList,
                        selectedAttributeType: selectedAttribute.AttributeType,
                        selectedOperatorType: selectedOperator.OperatorName,
                        selectedValue: selectedValue,
                        selectedValue2: selectedValue2,
                        attributeOptions: data
                    });
                }, (error) => {
                    window.Dialog.alert(error);
                });
            }
            else {
                this.setState({
                    attributeData: attrList,
                    operatorValue: operatorValue,
                    attributeValue: attributeValue,
                    operatorData: filteredOprList,
                    selectedAttributeType: selectedAttribute.AttributeType,
                    selectedOperatorType: selectedOperator.OperatorName,
                    selectedValue: selectedValue,
                    selectedValue2: selectedValue2
                });
            }
        }
        else {
            this.setState({
                attributeData: attrList, operatorValue: operatorValue, attributeValue: attributeValue, operatorData: filteredOprList
            });
        }
    }
    filterOperatorByAttributeType(operators, attributeType) {
        let filteredOperators = operators.filter(x => {
            return x.OperatorType == attributeType;
        });
        return this.translateOperatorsToDropDown(filteredOperators);
    }
    translateAttributesToDropDown(attributes) {
        let attributeData = attributes.map(x => {
            let item = new DropdownListItem_1.DropdownListItem(x.AttributeName, x.AttributeTitle);
            return item;
        });
        return attributeData;
    }
    translateOperatorsToDropDown(operators) {
        let operatorData = operators.map(x => {
            let item = new DropdownListItem_1.DropdownListItem(x.OperatorName, x.OperatorTitle);
            return item;
        });
        return operatorData;
    }
    renderValue() {
        var attributeType = this.state.selectedAttributeType;
        var operatorType = this.state.selectedOperatorType;
        let inputControl = React.createElement("span", null);
        switch (attributeType) {
            case 1:
            case 2:
            case 3:
                inputControl = React.createElement(kendo_react_inputs_1.Input, { className: "form-control", required: true, validationMessage: "This is a required field", value: this.state.selectedValue, onChange: this.onValueChange, key: "damnIt" });
                break;
            case 4:
                let date = null;
                if (this.state.selectedValue) {
                    try {
                        date = new Date(this.state.selectedValue);
                    }
                    catch (e) {
                        date = new Date();
                    }
                }
                inputControl = React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onValueChange, value: date });
                break;
            case 5:
                inputControl = React.createElement(kendo_react_dropdowns_1.ComboBox, { className: "form-control", data: this.state.attributeOptions, textField: "name", dataItemKey: "value", filterable: true, value: this.state.selectedValue, onChange: this.onValueChange });
                break;
            case 6:
                if (operatorType == 33) {
                    let selectedValue = [];
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue.push(this.state.selectedValue);
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue;
                    }
                    inputControl = React.createElement(kendo_react_dropdowns_1.MultiSelect, { className: "form-control", data: this.state.attributeOptions, textField: "name", dataItemKey: "value", filterable: true, value: selectedValue, onChange: this.onValueChange });
                }
                else if (operatorType == 32) {
                    let selectedValue = {};
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue;
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue[0];
                    }
                    inputControl = React.createElement(kendo_react_dropdowns_1.ComboBox, { className: "form-control", data: this.state.attributeOptions, textField: "name", dataItemKey: "value", filterable: true, value: selectedValue, onChange: this.onValueChange });
                }
                break;
            case 7:
                if (operatorType == 35) {
                    let selectedValue = [];
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue.push(this.state.selectedValue);
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue;
                    }
                    inputControl = React.createElement(kendo_react_dropdowns_1.MultiSelect, { className: "form-control", data: this.state.attributeOptions, textField: "name", dataItemKey: "value", filterable: true, value: selectedValue, onChange: this.onValueChange });
                }
                else if (operatorType == 34) {
                    let selectedValue = {};
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue;
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue[0];
                    }
                    inputControl = React.createElement(kendo_react_dropdowns_1.ComboBox, { className: "form-control", data: this.state.attributeOptions, textField: "name", dataItemKey: "value", filterable: true, value: selectedValue, onChange: this.onValueChange });
                }
                break;
                break;
        }
        return inputControl;
    }
    renderValue2() {
        var attributeType = this.state.selectedAttributeType;
        var operatorType = this.state.selectedOperatorType;
        let inputControl = React.createElement("span", null);
        switch (attributeType) {
            case 1:
            case 2:
            case 3:
                if (operatorType == 21 || operatorType == 29) {
                    inputControl = React.createElement(kendo_react_inputs_1.Input, { required: true, className: "form-control", validationMessage: "This is a required field", defaultValue: this.state.selectedValue2, onChange: this.onValue2Change });
                }
                break;
            case 4:
                if (operatorType == 13) {
                    let date = null;
                    if (this.state.selectedValue2) {
                        try {
                            date = new Date(this.state.selectedValue2);
                        }
                        catch (e) {
                            date = new Date();
                        }
                    }
                    inputControl = React.createElement(kendo_react_dateinputs_1.DatePicker, { className: "form-control", onChange: this.onValue2Change, value: date });
                }
                break;
        }
        return inputControl;
    }
    remove() {
        this.props.onRemove(this.props.rowIndex);
    }
    onAttributeChange(event) {
        //When the dropdown changes, get the attribute that was selected
        // drop down doesn't know the type, so look up in props
        // also set the operator drop down to default 'Select operator...'
        let selectedAttribute = this.props.attributes.filter(y => {
            return y.AttributeName == event.target.value.id;
        });
        var filteredOperators = this.filterOperatorByAttributeType(this.props.operators, selectedAttribute[0].AttributeType);
        this.setState({ attributeValue: event.target.value, operatorData: filteredOperators, operatorValue: this.defaultOperatorDropdown, selectedValue: '', selectedValue2: '', selectedAttributeType: 0 });
    }
    onOperatorChange(event) {
        let attributeType = 1;
        let operatorType = 1;
        let selectedOperatorValue = event.target.value;
        let sender = this;
        let selectedAttribute = this.props.attributes.filter((y) => {
            return y.AttributeName == this.state.attributeValue.id;
        });
        let selectedOperator = this.props.operators.filter((y) => {
            return y.OperatorName == event.target.value.id;
        });
        if (selectedAttribute && selectedAttribute[0]) {
            attributeType = selectedAttribute[0].AttributeType;
        }
        if (selectedOperator && selectedOperator[0]) {
            operatorType = selectedOperator[0].OperatorName;
        }
        if (selectedAttribute.length > 0 && (selectedAttribute[0].AttributeType === 6 || selectedAttribute[0].AttributeType === 7 || selectedAttribute[0].AttributeType === 5)) {
            let postValue = {
                attributeId: selectedAttribute[0].AttributeId,
                isEntityLookup: selectedAttribute[0].IsEntityLookyup,
                entity: selectedAttribute[0].Entity,
            };
            Remote_1.Remote.postData("/ResourceAttribute/GetOptions", postValue, (data) => {
                var newCondition = new Condition_1.Condition();
                if (selectedAttribute[0]) {
                    newCondition.Attribute = selectedAttribute[0];
                    newCondition.IsEntity = selectedAttribute[0].IsEntityLookyup;
                }
                if (selectedOperator[0]) {
                    newCondition.Operator = selectedOperator[0];
                }
                newCondition.Value = '';
                newCondition.Value2 = '';
                sender.props.onConditionChange(sender.props.rowIndex, newCondition);
                sender.setState({ operatorValue: selectedOperatorValue, selectedAttributeType: attributeType, selectedOperatorType: operatorType, attributeOptions: data, selectedValue: '', selectedValue2: '' });
            }, (error) => {
                window.Dialog.alert(error);
            });
        }
        else {
            var newCondition = new Condition_1.Condition();
            if (selectedAttribute[0]) {
                newCondition.Attribute = selectedAttribute[0];
                newCondition.IsEntity = selectedAttribute[0].IsEntityLookyup;
            }
            if (selectedOperator[0]) {
                newCondition.Operator = selectedOperator[0];
            }
            newCondition.Value = '';
            newCondition.Value2 = '';
            this.props.onConditionChange(this.props.rowIndex, newCondition);
            this.setState({ operatorValue: selectedOperatorValue, selectedAttributeType: attributeType, selectedOperatorType: operatorType, selectedValue: '', selectedValue2: '' });
        }
    }
    onValueChange(event) {
        let selectedDropDownAttribute = this.state.attributeValue;
        let selectedAttribute = this.props.attributes.filter((x) => {
            return x.AttributeName == selectedDropDownAttribute.id;
        });
        let selectedDropDownOperator = this.state.operatorValue;
        let selectedOperator = this.props.operators.filter((x) => {
            return x.OperatorName == selectedDropDownOperator.id;
        });
        var newCondition = new Condition_1.Condition();
        newCondition.Attribute = selectedAttribute[0];
        newCondition.Operator = selectedOperator[0];
        newCondition.IsEntity = selectedAttribute[0].IsEntityLookyup;
        if (event.target.value == "") {
            newCondition.Value = '';
        }
        else {
            newCondition.Value = event.target.value;
            newCondition.Value2 = this.state.selectedValue2;
        }
        this.setState({ selectedValue: newCondition.Value });
        this.props.onConditionChange(this.props.rowIndex, newCondition);
    }
    onValue2Change(event) {
        let selectedDropDownAttribute = this.state.attributeValue;
        let selectedAttribute = this.props.attributes.filter((x) => {
            return x.AttributeName == selectedDropDownAttribute.id;
        });
        let selectedDropDownOperator = this.state.operatorValue;
        let selectedOperator = this.props.operators.filter((x) => {
            return x.OperatorName == selectedDropDownOperator.id;
        });
        var newCondition = new Condition_1.Condition();
        newCondition.Attribute = selectedAttribute[0];
        newCondition.Operator = selectedOperator[0];
        newCondition.IsEntity = selectedAttribute[0].IsEntityLookyup;
        newCondition.Value = this.state.selectedValue;
        if (event.target.value == "") {
            newCondition.Value = this.state.selectedValue;
            newCondition.Value2 = '';
        }
        else {
            newCondition.Value = this.state.selectedValue;
            newCondition.Value2 = event.target.value;
        }
        this.setState({ selectedValue2: newCondition.Value2 });
        this.props.onConditionChange(this.props.rowIndex, newCondition);
    }
    isDate(value) {
        try {
            var d = new Date(value);
            if (d.getDate())
                return true;
            else
                return false;
        }
        catch (e) {
            return false;
        }
    }
    formatNumber(v) {
        if (v < 10)
            return "0" + v;
        else
            return v;
    }
}
exports.AdvancedSearchRow = AdvancedSearchRow;
//# sourceMappingURL=AdvancedSearchRow.js.map