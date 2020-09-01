"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const Listbox_1 = require("./Listbox");
class MultiselectListbox extends React.Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (React.createElement("div", { className: "w-100 row multiselect-listbox" },
            React.createElement("div", { className: "col-5" },
                React.createElement(Listbox_1.Listbox, { data: this.props.data, title: "All Ventors", description: "Please select vendors and move right to select." })),
            React.createElement("div", { className: "col-2 text-center row", style: { marginLeft: '15px' } },
                React.createElement("div", { className: "col-12 text-center" },
                    React.createElement("a", { className: "action-button", href: "#" }, "Up")),
                React.createElement("div", { className: "col-12 text-center" },
                    React.createElement("a", { className: "action-button", href: "#" }, "Down")),
                React.createElement("div", { className: "col-12 text-center" },
                    React.createElement("a", { className: "action-button", href: "#" }, ">")),
                React.createElement("div", { className: "col-12 text-center" },
                    React.createElement("a", { className: "action-button", href: "#" }, ">>")),
                React.createElement("div", { className: "col-12 text-center" },
                    React.createElement("a", { className: "action-button", href: "#" }, "<")),
                React.createElement("div", { className: "col-12 text-center" },
                    React.createElement("a", { className: "action-button", href: "#" }, "<<"))),
            React.createElement("div", { className: "col-5" },
                React.createElement(Listbox_1.Listbox, { data: this.props.data, title: "Selected Vendors", description: "Your selected vendors to search" }))));
    }
}
exports.MultiselectListbox = MultiselectListbox;
//# sourceMappingURL=MultiselectListbox.js.map