import "core-js"
import "regenerator-runtime/runtime";
import * as React from "react";
import * as ReactDOM from "react-dom";
import { Condition, Resource, Attribute, Operator } from "../Entities/Condition";
import { AdvancedSearchHeader } from "../AdvancedSearch/AdvancedSearchHeader";
import { AdvancedSearchRow } from "../AdvancedSearch/AdvancedSearchRow";



export interface IAdvancedSearchProps {
    selectedConditions?: Array<Condition>;
    onApply?: any;
    onConditionChange?: any;
    resourceIds: Array<string>;
}

export interface IAdvancedSearchState {
    hidden: boolean,
    attributes: Array<Attribute>,
    operators: Array<Operator>,
    selectedConditions: Array<Condition>,
    rows: Array<JSX.Element>,
}

export class AdvancedSearch extends React.Component<IAdvancedSearchProps, IAdvancedSearchState> {
    
    constructor(props: IAdvancedSearchProps) {
        super(props);
        let arr = new Array<JSX.Element>();      
        
        this.state = {
            hidden: false,            
            operators: new Array<Operator>(),
            attributes: new Array<Attribute>(),
            rows: arr,
            selectedConditions: this.props.selectedConditions || [],
        }
        ;
        this.addNewRow = this.addNewRow.bind(this);
        this.onApply = this.onApply.bind(this);
        this.onConditionChange = this.onConditionChange.bind(this);
        this.onRemove = this.onRemove.bind(this);
    }

    render() {      
        
        return (!this.state.hidden && <div>
            <AdvancedSearchHeader />
            {this.state.rows.map(row=>row)}            
            <div className="row">
                <div className="col">
                    <a href="#" className="btn btn-primary" onClick={this.addNewRow}>Add Another Condition</a>
                </div>
            </div>
            </div>
        );
    }

    componentDidMount() {       
         this.loadDefaults();       
    }

    componentWillUpdate() {
        if (this.state.selectedConditions != this.props.selectedConditions) {
            this.setState({selectedConditions: this.props.selectedConditions})
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
    async loadDefaults() {
       Promise.all([this.fetchAttributes(),this.fetchOperators()]
               ).then((results) => {
            
            let attributes: Array<Attribute> = results[0];
            let operators: Array<Operator> = results[1];           
            this.setState({ attributes: attributes, operators: operators });
            if (this.props.selectedConditions && this.props.selectedConditions.length > 0) {
                var rows = this.props.selectedConditions.map((x, index) => {
                    var rowIndex = index + 1;
                    return <AdvancedSearchRow key={rowIndex} rowIndex={rowIndex} attributes={attributes} operators={operators} selectedAttribute={x.Attribute} selectedOperator={x.Operator} selectedValue={x.Value} selectedValue2={x.Value2} onConditionChange={this.onConditionChange} onRemove={this.onRemove}/>
                });
                this.setState({ rows: rows });
            }
            else {
                this.addNewRow();
            }


        }).catch(() => alert('Some error'));
    }

    /**
     * Adds a new row component and updates the state
     * */
    addNewRow() {
        

        let currentRows = this.state.rows;
        let count = currentRows.length + 1;
        currentRows.push(<AdvancedSearchRow rowIndex={count} key={count} attributes={this.state.attributes} operators={this.state.operators} onConditionChange={this.onConditionChange} onRemove={this.onRemove} />);
        this.setState({ rows: currentRows });
    }

    onRemove(rowIndex: number) {
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

    async fetchAttributes(): Promise<Array<Attribute>> {
        let param = this.props.resourceIds.map(x => `resourceIds=${x}`).join("&");
        let result = await fetch(`/ResourceAttribute/Get?${param}`, { headers: { 'Cache-Control': 'no-cache'} });
        let json = await result.json();
        json = json.filter((x: any) => {
            return x.searchable === true;
        })
        let currentAttributeList = json.map((x: any) => {
            return new Attribute(x.resourceAttributeGuid, x.name, x.title, x.attributeType, x.isEntityLookup, x.entity);
        });
        return Promise.resolve<Array<Attribute>>(currentAttributeList);
            
    }

    /**
     * fetch API to get the list of Operators 
     * @returns Promise containining array of operators
     */
    async fetchOperators(): Promise<Array<Operator>> {
        let result = await fetch("/QueryOperator/Get", { headers: { 'Cache-Control': 'no-cache' } });
        let json = await result.json();
        let currentOperatorList = json.map((x: any) => {
            return new Operator(x.queryOperatorGuid, x.name, x.title, x.type);
        });
        return Promise.resolve<Array<Operator>>(currentOperatorList);           
    }
    
    onApply()
    {
        this.props.onApply(this.state.selectedConditions);
        
    }

    onConditionChange(rowIndex: any, condition: Condition) {
        let currentConditions = this.state.selectedConditions;
        if (currentConditions.length < rowIndex) {
            currentConditions.push(condition)
        }
        currentConditions[rowIndex - 1] = condition;
        this.setState({ selectedConditions: currentConditions });
        this.props.onConditionChange(currentConditions);
    }

    

    

}



