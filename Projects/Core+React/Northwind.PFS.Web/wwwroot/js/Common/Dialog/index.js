"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const ReactDOM = require("react-dom");
const Dialog_1 = require("./Dialog");
const Events_1 = require("./Events");
if (document.getElementsByClassName("kdialog").length > 0) {
    for (let i = 0; i < document.getElementsByClassName("kdialog").length; i++) {
        var element = document.getElementsByClassName("kdialog")[i];
        ReactDOM.render(React.createElement(Dialog_1.KendoDialog, { key: element.getAttribute("key"), uniqueKey: element.getAttribute("key"), actionText: element.getAttribute("actionText"), dialogTitle: element.getAttribute("dialogTitle"), dialogHeight: element.getAttribute("dialogHeight"), dialogWidth: element.getAttribute("dialogWidth"), method: element.getAttribute("method"), getUrl: element.getAttribute("getUrl"), postUrl: element.getAttribute("postUrl"), buttons: Events_1.DialogEvents.buttons, register: () => { } }), document.getElementById(element.id));
    }
}
class LoadDialog {
    static loadDialog() {
        for (let i = 0; i < document.getElementsByClassName("kendoDialogMenu").length; i++) {
            var element = document.getElementsByClassName("kendoDialogMenu")[i];
            if (element.tagName == "li") {
                ReactDOM.render(React.createElement(Dialog_1.KendoDialog, { key: element.getAttribute("key"), uniqueKey: element.getAttribute("key"), actionText: element.getAttribute("actionText"), dialogTitle: element.getAttribute("dialogTitle"), dialogHeight: element.getAttribute("dialogHeight"), dialogWidth: element.getAttribute("dialogWidth"), method: element.getAttribute("method"), getUrl: element.getAttribute("getUrl"), postUrl: element.getAttribute("postUrl"), buttons: Events_1.DialogEvents.buttons, register: () => { } }), document.getElementById(element.id));
            }
        }
    }
}
exports.LoadDialog = LoadDialog;
window.LoadDialog = LoadDialog;
//# sourceMappingURL=index.js.map