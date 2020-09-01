import * as React from "react";
import * as ReactDOM from "react-dom";
import { DropDownList, MultiSelect, ComboBox } from '@progress/kendo-react-dropdowns';
import { Input } from '@progress/kendo-react-inputs';
import { DatePicker } from '@progress/kendo-react-dateinputs';
import { Condition, Resource, Attribute, Operator } from "../Entities/Condition";
import { DropdownListItem } from "../Entities/DropdownListItem";
import { Remote } from "../../Common/Remote/Remote";

declare var window: any;

export interface IAdvancedSearchRowProps {
    rowIndex?: number;
    attributes?: Array<Attribute>;
    operators?: Array<Operator>;
    onConditionChange?: any;
    onRemove?: any;
    selectedAttribute?: Attribute;
    selectedOperator?: Operator;
    selectedValue?: any;
    selectedValue2?: any;
    
}

export interface IAdvancedSearchRowState {
    attributeValue?: any,
    operatorValue?: any,
    operatorData?: any,
    attributeData?: any,
   
    selectedValue?: any,
    selectedValue2?: any,
    selectedAttributeType?: number,
    selectedOperatorType?: number,
    condition: Condition,

    attributeOptions: any[];
}



export class AdvancedSearchRow extends React.Component<IAdvancedSearchRowProps, IAdvancedSearchRowState> {
    constructor(props: Readonly<IAdvancedSearchRowProps>) {
        super(props);
        
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
            }

       

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

    defaultAttributeDropdown = new DropdownListItem("", "Select attribute...");
    defaultOperatorDropdown = new DropdownListItem("","Select operator...");

    render() {
        return (
            <div className="row form-group">
                <div className="col-sm-3">
                    <DropDownList className="form-control" data={this.state.attributeData} value={this.state.attributeValue} textField="text" dataItemKey="id" onChange={this.onAttributeChange} />
                </div>
                <div className="col-sm-3">
                    <DropDownList className="form-control" data={this.state.operatorData} value={this.state.operatorValue} textField="text" dataItemKey="id" onChange={this.onOperatorChange} />
                </div>
                <div className="col-sm-2">
                    {this.renderValue()}
                </div>
                <div className="col-sm-2">
                    {this.renderValue2()}
                </div>
                <div className="col-sm-2 text-right">
                    <a href="#" onClick={this.remove}><i className="k-icon k-i-delete"></i></a>
                </div>
            </div>)
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
        let filteredOprList = new Array<DropdownListItem>();

        //if any selected attribute is present in props
        //then show this as selected attribute in dropdown 
        //filter the list of operators based on that attribute type
        //also show appropriate control for values
        if (selectedAttribute) {
            attributeValue = new DropdownListItem(selectedAttribute.AttributeName, selectedAttribute.AttributeTitle);
            filteredOprList = this.filterOperatorByAttributeType(this.props.operators, selectedAttribute.AttributeType);
            operatorValue = new DropdownListItem(selectedOperator.OperatorName, selectedOperator.OperatorTitle);
            
            if (selectedAttribute && (selectedAttribute.AttributeType === 6 || selectedAttribute.AttributeType === 7 || selectedAttribute.AttributeType === 5)) {
                let postValue = {
                    attributeId: selectedAttribute.AttributeId,
                    isEntityLookup: selectedAttribute.IsEntityLookyup,
                    entity: selectedAttribute.Entity,
                }
                Remote.postData("/ResourceAttribute/GetOptions", postValue, (data: any) => {
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

                }, (error: any) => {
                    window.Dialog.alert(error)
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

    private filterOperatorByAttributeType(operators: Array<Operator>, attributeType: number): Array<DropdownListItem> {
        let filteredOperators = operators.filter(x => {
            return x.OperatorType == attributeType;
        });

        return this.translateOperatorsToDropDown(filteredOperators);
        
    }

    private translateAttributesToDropDown(attributes: Array<Attribute>): Array<DropdownListItem>  {
        let attributeData = attributes.map(x => {
            let item = new DropdownListItem(x.AttributeName,x.AttributeTitle) ;            
            return item;
        });
        return attributeData;
    }

    private translateOperatorsToDropDown(operators: Array<Operator>): Array<DropdownListItem> {
        let operatorData = operators.map(x => {
            let item = new DropdownListItem(x.OperatorName, x.OperatorTitle);
            
            return item;
        });
        return operatorData;

    }

    renderValue() {
        var attributeType = this.state.selectedAttributeType;
        var operatorType = this.state.selectedOperatorType;
        let inputControl = <span></span>
        switch (attributeType) {
            case 1:
            case 2:
            case 3:
                inputControl = <Input className="form-control" required={true} validationMessage="This is a required field" value={this.state.selectedValue} onChange={this.onValueChange} key="damnIt" />;
                break;
            case 4:
                let date = null;
                if (this.state.selectedValue) {
                    try {
                        date = new Date(this.state.selectedValue);
                    } catch (e) {
                        date = new Date();
                    }
                }
                inputControl = <DatePicker  className="form-control" onChange={this.onValueChange} value={date} />;                
                break;
            case 5:
                inputControl = <ComboBox className="form-control" data={this.state.attributeOptions} textField="name" dataItemKey="value" filterable={true} value={this.state.selectedValue} onChange={this.onValueChange}/>;
                break;
            case 6:
                if (operatorType == 33) {
                    let selectedValue = [];
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue.push(this.state.selectedValue)
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue;
                    }
                    inputControl = <MultiSelect className="form-control" data={this.state.attributeOptions} textField="name" dataItemKey="value" filterable={true} value={selectedValue} onChange={this.onValueChange}/>;
                }
                else if (operatorType == 32) {
                    let selectedValue = {};
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue[0];
                    }
                    inputControl = <ComboBox className="form-control" data={this.state.attributeOptions} textField="name" dataItemKey="value" filterable={true} value={selectedValue} onChange={this.onValueChange}/>;
                }
                break;
            case 7:
                if (operatorType == 35) {
                    let selectedValue = [];
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue.push(this.state.selectedValue)
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue;
                    }
                    inputControl = <MultiSelect className="form-control" data={this.state.attributeOptions} textField="name" dataItemKey="value" filterable={true} value={selectedValue} onChange={this.onValueChange}/>;
                }
                else if (operatorType == 34) {
                    let selectedValue = {};
                    if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && !this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue
                    }
                    else if (typeof (this.state.selectedValue) === "object" && this.state.selectedValue && this.state.selectedValue.length) {
                        selectedValue = this.state.selectedValue[0];
                    }
                    inputControl = <ComboBox className="form-control" data={this.state.attributeOptions} textField="name" dataItemKey="value" filterable={true} value={selectedValue} onChange={this.onValueChange}/>;
                }
                break;
                break;
        }
        return inputControl

    }

    renderValue2() {
        var attributeType = this.state.selectedAttributeType;
        var operatorType = this.state.selectedOperatorType;
        let inputControl = <span></span>
        switch (attributeType) {
            case 1:
            case 2:
            case 3:
               
                if (operatorType == 21 || operatorType == 29) {
                   
                    inputControl = <Input required={true} className="form-control" validationMessage="This is a required field" defaultValue={this.state.selectedValue2} onChange={this.onValue2Change} />;
                }
                break;
            case 4:
                if (operatorType == 13) {
                    let date = null;
                    if (this.state.selectedValue2) {
                        try {
                            date = new Date(this.state.selectedValue2);
                        } catch (e) {
                            date = new Date();
                        }
                    }
                    inputControl = <DatePicker className="form-control" onChange={this.onValue2Change} value={date} />;
                }
                break;
            
        }
        return inputControl;

    }

    remove() {
        this.props.onRemove(this.props.rowIndex);
    }
    
    onAttributeChange(event: any) {
        //When the dropdown changes, get the attribute that was selected
        // drop down doesn't know the type, so look up in props
        // also set the operator drop down to default 'Select operator...'
        let selectedAttribute = this.props.attributes.filter(y => {
            return y.AttributeName == event.target.value.id;
        });
        var filteredOperators = this.filterOperatorByAttributeType(this.props.operators, selectedAttribute[0].AttributeType);        
        this.setState({ attributeValue: event.target.value, operatorData: filteredOperators, operatorValue: this.defaultOperatorDropdown, selectedValue: '', selectedValue2: '', selectedAttributeType: 0 });
    }

    onOperatorChange(event: any) {
        let attributeType = 1;
        let operatorType = 1;
        let selectedOperatorValue = event.target.value;
        let sender = this;
        let selectedAttribute = this.props.attributes.filter((y: any) => {
            return y.AttributeName == this.state.attributeValue.id;
        });

        let selectedOperator = this.props.operators.filter((y: any) => {
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
            }
            Remote.postData("/ResourceAttribute/GetOptions", postValue, (data: any) => {

                var newCondition = new Condition();
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
            }, (error: any) => {
                window.Dialog.alert(error)
            });
        }
        else {
            var newCondition = new Condition();
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

    onValueChange(event: any) {
        let selectedDropDownAttribute = this.state.attributeValue;

        let selectedAttribute = this.props.attributes.filter((x: Attribute) => {
            return x.AttributeName == selectedDropDownAttribute.id;
        });


        let selectedDropDownOperator = this.state.operatorValue;

        let selectedOperator = this.props.operators.filter((x: Operator) => {
            return x.OperatorName == selectedDropDownOperator.id;
        });
        var newCondition = new Condition();
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

    onValue2Change(event: any) {
        let selectedDropDownAttribute = this.state.attributeValue;

        let selectedAttribute = this.props.attributes.filter((x: Attribute) => {
            return x.AttributeName == selectedDropDownAttribute.id;
        });


        let selectedDropDownOperator = this.state.operatorValue;

        let selectedOperator = this.props.operators.filter((x: Operator) => {
            return x.OperatorName == selectedDropDownOperator.id;
        });

        var newCondition = new Condition();
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

    isDate(value: any) {
        try {
            var d = new Date(value);
            if (d.getDate())
                return true;
            else
                return false;
        } catch (e) {
            return false;
        }
    }

    formatNumber(v: number) {
        if (v < 10)
            return "0" + v;
        else
            return v;
    }
}