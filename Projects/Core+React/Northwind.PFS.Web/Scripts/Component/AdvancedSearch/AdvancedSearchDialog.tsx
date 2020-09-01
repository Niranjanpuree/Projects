import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { AdvancedSearch } from "./AdvancedSearch";
import { Button } from "@progress/kendo-react-buttons";
import { AdvancedSearchPills } from "./AdvancedSearchPills";
import { Operator, Attribute, Condition } from "../Entities/Condition";
declare var window: any;

interface IAdvancedSearchDialogProps {
    onApply?: any;
    onConditionChange?: any;
    selectedConditions?: any;
    resourceIds: Array<string>;
    
}

interface IAdvancedSearchDialogState {
    visible: boolean;
    selectedConditions?: any;
    mode: any;
}

export class AdvancedSearchDialog extends React.Component<IAdvancedSearchDialogProps, IAdvancedSearchDialogState> {
    constructor(props: any) {
        super(props);
        this.state = {
            mode: 'new',
            visible: false,
            selectedConditions: this.props.selectedConditions
        };
        this.toggleDialog = this.toggleDialog.bind(this);
        this.onApply = this.onApply.bind(this);
        this.onConditionChange = this.onConditionChange.bind(this);
        this.changeMode = this.changeMode.bind(this);
        this.isValue2Required = this.isValue2Required.bind(this);

    }




    render() {
        

        return (<div className="advanced-search-box">

            {!this.state.visible && this.state.mode == 'new' && <a href="#" onClick={this.toggleDialog}>Advanced Search</a>}
            {!this.state.visible && this.state.mode == 'edit' && <div>
                <a href="#" onClick={this.toggleDialog}>Advanced Search</a></div>}
            {/* <AdvancedSearchPills  conditions={this.state.selectedConditions} /><AdvancedSearchPills  className="badge badge-pill badge-secondary" conditions={this.state.selectedConditions} /></div> */}



            {this.state.visible && <Dialog title="Advanced Search" width="80%" height="70%">
                <AdvancedSearch selectedConditions={this.state.selectedConditions} onConditionChange={this.onConditionChange} resourceIds={this.props.resourceIds} />
                <DialogActionsBar>
                    <Button primary={true} onClick={this.onApply} type="button">Apply</Button>
                    <Button onClick={this.toggleDialog} type="button">Cancel</Button>
                </DialogActionsBar>
            </Dialog>}
        </div>
        );


       
    }

    componentDidMount() {
        this.changeMode();
    }

    changeMode() {
        if (this.state.selectedConditions && this.state.selectedConditions.length > 0) {
            this.setState({ mode: 'edit' });
        }
        else {
            this.setState({ mode: 'new' });
        }
    }

    onApply(event: any) {
        console.log(this.state.selectedConditions);
        if (this.props.onApply) {
            this.props.onApply(this.state.selectedConditions);
        }
        this.changeMode();
        this.setState({ visible: false });
    }

    isValue2Required(condition: Condition) {
        switch (condition.Operator.OperatorName) {
            case 13:
                return true;
        }
        return false;
    }

    toggleDialog() {
        var currentState = this.state.visible;
        
        this.setState({ visible: !currentState });
    }

    onConditionChange(selectedConditions: any) {
        this.setState({ selectedConditions: selectedConditions });
        if (this.props.onConditionChange) this.props.onConditionChange(this.state.selectedConditions);
    }

    updateSearchConditions(conditions: any[]) {
        this.setState({ selectedConditions: conditions })
    }



}