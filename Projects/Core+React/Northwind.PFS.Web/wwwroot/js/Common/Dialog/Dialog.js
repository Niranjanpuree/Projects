"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
const kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
const Remote_1 = require("../Remote/Remote");
class KendoDialog extends React.Component {
    constructor(props) {
        super(props);
        this.getFormData = function (form) {
            var unindexed_array = form.serializeArray();
            let indexed_array = [];
            $.map(unindexed_array, function (n, i) {
                indexed_array[n['name']] = n['value'];
            });
            return indexed_array;
        };
        this.state = {
            visible: this.props.visible || false,
            formText: '',
            registered: false
        };
        this.onButtonClick = this.onButtonClick.bind(this);
        this.openForm = this.openForm.bind(this);
        this.getClickedButton = this.getClickedButton.bind(this);
    }
    onButtonClick(e) {
        let dlg = this;
        let evt = this.getClickedButton(e);
        let attr = e.currentTarget.attributes;
        var buttonIndex = e.target.getAttribute("itemid");
        let button = undefined;
        if (this.props.buttons.length == 1 && this.props.buttons[0].length > 1) {
            button = this.props.buttons[0][buttonIndex];
        }
        else {
            button = this.props.buttons[buttonIndex];
        }
        if (buttonIndex > -1) {
            if (button.requireValidation) {
                if ($("form")[0].checkValidity()) {
                    var encTypeFormData = false;
                    if ($("form").attr("enctype") == "multipart/form-data") {
                        encTypeFormData = true;
                    }
                    let postValue = JSON.stringify(this.serializeToJson($("form")));
                    if (this.props.postUrl != "") {
                        let dialogController = this;
                        $.ajax({
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                            },
                            url: this.props.postUrl,
                            type: this.props.method,
                            data: (dialogController.props.postData != undefined && dialogController.props.postData.length > 0) ? JSON.stringify(dialogController.props.postData) : postValue,
                            dataType: 'json',
                            success: function (data) {
                                dialogController.setState({ visible: false }, this.forceUpdate);
                                button.action(evt, data);
                            },
                            error: function (data, msg) {
                                let message = "";
                                if (data.responseJSON.Message) {
                                    message += data.responseJSON.Message + "<br/>";
                                }
                                for (let i in data.responseJSON) {
                                    if (data.responseJSON[i] != undefined && data.responseJSON[i].errors != undefined && data.responseJSON[i].errors.length > 0) {
                                        message += data.responseJSON[i].errors[0].errorMessage + "<br/>";
                                    }
                                }
                                window.Dialog.alert(message);
                            }
                        });
                    }
                    else {
                        this.setState({ visible: false, formText: '' }, this.forceUpdate);
                        button.action(evt);
                    }
                }
                else {
                    let invalidFocused = false;
                    $("form").find("input, textarea").each(function (e, index) {
                        if ($(this).attr("data-val-required")) {
                            if ($("#" + $(this).attr("id") + "-error").length > 0) {
                                if ($(this).is(":invalid")) {
                                    $("#" + $(this).attr("id") + "-error").html($(this).attr("data-val-required"));
                                    $(this).on("blur", function () {
                                        $("#" + $(this).attr("id") + "-error").html($(this).attr("data-val-required"));
                                    });
                                    if (invalidFocused == false) {
                                        $(this).focus();
                                        invalidFocused = true;
                                    }
                                }
                            }
                            else {
                                if ($(this).is(":invalid")) {
                                    let objValidation = $('[data-valmsg-for="' + $(this).attr("id") + '"]');
                                    objValidation.html($(this).attr("data-val-required"));
                                    $(this).on("blur", function () {
                                        objValidation.html($(this).attr("data-val-required"));
                                    });
                                    if (invalidFocused == false) {
                                        $(this).focus();
                                        invalidFocused = true;
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else {
                let postValue = dlg.serializeToJson($("form"));
                this.setState({ visible: false, formText: '' }, this.forceUpdate);
                button.action(evt, postValue);
            }
        }
    }
    getClickedButton(e) {
        let index = parseInt(e.target.getAttribute("itemid"));
        var btnEv = Object.assign({}, e, { button: this.props.buttons[index] });
        return btnEv;
    }
    serializeToJson(serializer) {
        var data = serializer.serialize().split("&");
        var obj = {};
        for (var key in data) {
            if (obj[data[key].split("=")[0]]) {
                if (typeof (obj[data[key].split("=")[0]]) == "string") {
                    let val = obj[data[key].split("=")[0]];
                    obj[data[key].split("=")[0]] = [];
                    obj[data[key].split("=")[0]].push(val);
                }
                obj[data[key].split("=")[0]].push(decodeURIComponent(data[key].split("=")[1]));
            }
            else {
                obj[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
            }
        }
        return obj;
    }
    renderButtons() {
        let buttons = [];
        if (this.props.buttons.length == 1) {
            if (this.props.buttons[0].length > 0) {
                this.props.buttons[0].forEach((e, index) => {
                    buttons.push(React.createElement(kendo_react_buttons_1.Button, { key: this.props.uniqueKey + "button" + index, primary: e.primary, itemID: index + "", onClick: this.onButtonClick }, e.text));
                });
            }
            else {
                this.props.buttons.forEach((e, index) => {
                    buttons.push(React.createElement(kendo_react_buttons_1.Button, { key: this.props.uniqueKey + "button" + index, primary: e.primary, itemID: index + "", onClick: this.onButtonClick }, e.text));
                });
            }
        }
        else {
            this.props.buttons.forEach((e, index) => {
                buttons.push(React.createElement(kendo_react_buttons_1.Button, { key: this.props.uniqueKey + "button" + index, primary: e.primary, itemID: index + "", onClick: this.onButtonClick }, e.text));
            });
        }
        return buttons;
    }
    componentDidMount() {
        if (!this.props || !this.props.getUrl || this.props.getUrl == "")
            return;
        let sender = this;
        Remote_1.Remote.getText(this.props.getUrl, (result) => {
            sender.setState(Object.assign({}, this.state, { visible: this.state.visible, formText: result }));
        }, (error) => {
            window.Dialog.alert(error, "Error");
        });
    }
    renderform() {
        return this.state.formText;
    }
    openForm() {
        let dialogController = this;
        this.setState({ visible: true });
        if (!this.props || !this.props.getUrl || this.props.getUrl === "")
            return;
        if (!(!this.props && !this.props.getUrl && this.props.getUrl === "")) {
            let prop = this.props;
            if (prop.getMethod.toLowerCase() === "get") {
                Remote_1.Remote.getText(this.props.getUrl, (result) => {
                    let arr = [];
                    var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    while (extractscript && extractscript.length > 0) {
                        arr.push(extractscript[0]);
                        result = result.replace(extractscript[0], "");
                        var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    }
                    if (arr.length > 0) {
                        dialogController.setState({ formText: result, visible: true }, this.forceUpdate);
                        for (let cnt in arr) {
                            window.eval($(arr[cnt]).html());
                        }
                    }
                    else {
                        dialogController.setState({ formText: result, visible: true }, this.forceUpdate);
                    }
                }, (error) => { window.Dialog.alert(error, "Error"); });
            }
            else {
                Remote_1.Remote.postDataText(this.props.getUrl, prop.postData, (result) => {
                    let arr = [];
                    var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    while (extractscript && extractscript.length > 0) {
                        arr.push(extractscript[0]);
                        result = result.replace(extractscript[0], "");
                        var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    }
                    if (extractscript && extractscript.length > 0) {
                        dialogController.setState({ formText: result, visible: true }, this.forceUpdate);
                        for (let cnt in arr) {
                            window.eval($(arr[cnt]).html());
                        }
                    }
                    else {
                        dialogController.setState({ formText: result, visible: true }, this.forceUpdate);
                    }
                }, (error) => {
                    window.Dialog.alert(error, "Error");
                });
            }
        }
    }
    shouldComponentUpdate() {
        if (this.state.registered == false && this.props.register) {
            this.props.register(this);
            this.setState({ registered: true });
            return true;
        }
        return false;
    }
    render() {
        return (React.createElement("div", null, this.state.visible &&
            React.createElement(kendo_react_dialogs_1.Dialog, { key: this.props.uniqueKey, title: this.props.dialogTitle, width: this.props.dialogWidth, height: this.props.dialogHeight },
                React.createElement("div", { className: "container" },
                    React.createElement("div", { dangerouslySetInnerHTML: { __html: unescape(this.state.formText) } })),
                this.props.children,
                React.createElement(kendo_react_dialogs_1.DialogActionsBar, null, this.renderButtons()))));
    }
}
exports.KendoDialog = KendoDialog;
//# sourceMappingURL=Dialog.js.map