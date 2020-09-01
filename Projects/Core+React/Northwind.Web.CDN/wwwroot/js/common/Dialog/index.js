"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var ReactDOM = require("react-dom");
var Dialog_1 = require("./Dialog");
var Events_1 = require("./Events");
require("@progress/kendo-theme-bootstrap/dist/all.css");
if (document.getElementsByClassName("kdialog").length > 0) {
    for (var i = 0; i < document.getElementsByClassName("kdialog").length; i++) {
        var element = document.getElementsByClassName("kdialog")[i];
        ReactDOM.render(React.createElement(Dialog_1.KendoDialog, { key: element.getAttribute("key"), uniqueKey: element.getAttribute("key"), actionText: element.getAttribute("actionText"), dialogTitle: element.getAttribute("dialogTitle"), dialogHeight: element.getAttribute("dialogHeight"), dialogWidth: element.getAttribute("dialogWidth"), method: element.getAttribute("method"), getUrl: element.getAttribute("getUrl"), postUrl: element.getAttribute("postUrl"), buttons: Events_1.DialogEvents.buttons, register: function () { } }), document.getElementById(element.id));
    }
}
var LoadDialog = /** @class */ (function () {
    function LoadDialog() {
    }
    LoadDialog.loadDialog = function () {
        for (var i = 0; i < document.getElementsByClassName("kendoDialogMenu").length; i++) {
            var element = document.getElementsByClassName("kendoDialogMenu")[i];
            if (element.tagName == "li") {
                ReactDOM.render(React.createElement(Dialog_1.KendoDialog, { key: element.getAttribute("key"), uniqueKey: element.getAttribute("key"), actionText: element.getAttribute("actionText"), dialogTitle: element.getAttribute("dialogTitle"), dialogHeight: element.getAttribute("dialogHeight"), dialogWidth: element.getAttribute("dialogWidth"), method: element.getAttribute("method"), getUrl: element.getAttribute("getUrl"), postUrl: element.getAttribute("postUrl"), buttons: Events_1.DialogEvents.buttons, register: function () { } }), document.getElementById(element.id));
            }
        }
    };
    return LoadDialog;
}());
exports.LoadDialog = LoadDialog;
window.LoadDialog = LoadDialog;
//# sourceMappingURL=index.js.map