"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
class Listbox extends React.Component {
    constructor(props) {
        super(props);
        let data = this.props.data;
        data.map((v) => {
            v.selected = false;
        });
        this.state = {
            data: data
        };
        this.onItemClicked = this.onItemClicked.bind(this);
        this.selectOne = this.selectOne.bind(this);
        this.selectOnCtrlPress = this.selectOnCtrlPress.bind(this);
        this.selectOnShiftPress = this.selectOnShiftPress.bind(this);
    }
    renderOptions() {
        let items = [];
        this.state.data.map((d, i) => {
            let css = "";
            if (d.selected)
                css = "selected";
            items.push(React.createElement("a", { className: "options " + css, key: i, itemID: "" + i, onClick: this.onItemClicked }, d.name));
        });
        return items;
    }
    selectOne(e) {
        let itemId = e.target.getAttribute("itemid");
        let data = this.state.data;
        data[itemId].selected = !data[itemId].selected;
        this.setState({ data: data });
    }
    selectOnCtrlPress(e) {
    }
    selectOnShiftPress(e) {
    }
    onItemClicked(e) {
        if (e.altKey) {
            this.selectOnCtrlPress(e);
        }
        else if (e.shiftKey) {
            this.selectOnShiftPress(e);
        }
        else {
            this.selectOne(e);
        }
    }
    render() {
        return (React.createElement("div", { className: "xylontech-list-box w-100 h-100" },
            React.createElement("span", { className: "header col-12" }, this.props.title),
            React.createElement("div", { className: "description col-12" }, this.props.description),
            React.createElement("div", { className: "list col-12" },
                React.createElement("div", { className: "list-scroller" },
                    React.createElement("div", { className: "w-100" }, this.renderOptions())))));
    }
}
exports.Listbox = Listbox;
//# sourceMappingURL=Listbox.js.map