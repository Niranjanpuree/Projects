"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("core-js");
const React = require("react");
const kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
const AdvancedSearch_1 = require("./AdvancedSearch");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
class AdvancedSearchDialog extends React.Component {
    constructor(props) {
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
        return (React.createElement("div", { className: "advanced-search-box" },
            !this.state.visible && this.state.mode == 'new' && React.createElement("a", { href: "#", onClick: this.toggleDialog }, "Advanced Search"),
            !this.state.visible && this.state.mode == 'edit' && React.createElement("div", null,
                React.createElement("a", { href: "#", onClick: this.toggleDialog }, "Advanced Search")),
            this.state.visible && React.createElement(kendo_react_dialogs_1.Dialog, { title: "Advanced Search", width: "80%", height: "70%" },
                React.createElement(AdvancedSearch_1.AdvancedSearch, { selectedConditions: this.state.selectedConditions, onConditionChange: this.onConditionChange, resourceIds: this.props.resourceIds }),
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null,
                    React.createElement(kendo_react_buttons_1.Button, { primary: true, onClick: this.onApply, type: "button" }, "Apply"),
                    React.createElement(kendo_react_buttons_1.Button, { onClick: this.toggleDialog, type: "button" }, "Cancel")))));
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
    onApply(event) {
        console.log(this.state.selectedConditions);
        if (this.props.onApply) {
            this.props.onApply(this.state.selectedConditions);
        }
        this.changeMode();
        this.setState({ visible: false });
    }
    isValue2Required(condition) {
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
    onConditionChange(selectedConditions) {
        this.setState({ selectedConditions: selectedConditions });
        if (this.props.onConditionChange)
            this.props.onConditionChange(this.state.selectedConditions);
    }
    updateSearchConditions(conditions) {
        this.setState({ selectedConditions: conditions });
    }
}
exports.AdvancedSearchDialog = AdvancedSearchDialog;
//# sourceMappingURL=AdvancedSearchDialog.js.map