"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var kendo_react_dialogs_1 = require("@progress/kendo-react-dialogs");
var kendo_react_buttons_1 = require("@progress/kendo-react-buttons");
var KendoDialog = /** @class */ (function (_super) {
    __extends(KendoDialog, _super);
    function KendoDialog(props) {
        var _this = _super.call(this, props) || this;
        _this.getFormData = function (form) {
            var unindexed_array = form.serializeArray();
            var indexed_array = [];
            $.map(unindexed_array, function (n, i) {
                indexed_array[n['name']] = n['value'];
            });
            return indexed_array;
        };
        _this.state = {
            visible: false,
            formText: '',
            registered: false
        };
        _this.onButtonClick = _this.onButtonClick.bind(_this);
        _this.openForm = _this.openForm.bind(_this);
        _this.getClickedButton = _this.getClickedButton.bind(_this);
        return _this;
    }
    KendoDialog.prototype.onButtonClick = function (e) {
        var dlg = this;
        var evt = this.getClickedButton(e);
        var attr = e.currentTarget.attributes;
        var buttonIndex = -1;
        for (var i = 0; i < attr.length; i++) {
            if (attr[i].name == "itemid") {
                buttonIndex = parseInt(attr[i].nodeValue);
                break;
            }
        }
        var button = undefined;
        if (this.props.buttons.length == 1 && this.props.buttons[0].length > 1) {
            button = this.props.buttons[0][buttonIndex];
        }
        else {
            button = this.props.buttons[buttonIndex];
        }
        if (buttonIndex > -1) {
            if (button.requireValidation) {
                if ($("form")[0].checkValidity()) {
                    var postValue = JSON.stringify(this.serializeToJson($("form")));
                    if (this.props.postUrl != "") {
                        var dialogController_1 = this;
                        $.ajax({
                            headers: {
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                            },
                            url: this.props.postUrl,
                            type: this.props.method,
                            data: (dialogController_1.props.postData != undefined && dialogController_1.props.postData.length > 0) ? JSON.stringify(dialogController_1.props.postData) : postValue,
                            dataType: 'json',
                            success: function (data) {
                                dialogController_1.setState({ visible: false });
                                button.action(evt, data);
                            },
                            error: function (data, msg) {
                                var message = "";
                                for (var i in data.responseJSON) {
                                    for (var j in data.responseJSON[i]) {
                                        message += data.responseJSON[i][j] + "\r\n";
                                    }
                                }
                                window.Dialog.alert(message);
                            }
                        });
                    }
                    else {
                        this.setState({ visible: false, formText: '' });
                        button.action(evt);
                    }
                }
                else {
                    var invalidFocused_1 = false;
                    $("form").find("input, textarea").each(function (e, index) {
                        if ($(this).attr("data-val-required")) {
                            if ($("#" + $(this).attr("id") + "-error").length > 0) {
                                if ($(this).is(":invalid")) {
                                    $("#" + $(this).attr("id") + "-error").html($(this).attr("data-val-required"));
                                    $(this).on("blur", function () {
                                        $("#" + $(this).attr("id") + "-error").html($(this).attr("data-val-required"));
                                    });
                                    if (invalidFocused_1 == false) {
                                        $(this).focus();
                                        invalidFocused_1 = true;
                                    }
                                }
                            }
                            else {
                                if ($(this).is(":invalid")) {
                                    var objValidation_1 = $('[data-valmsg-for="' + $(this).attr("id") + '"]');
                                    objValidation_1.html($(this).attr("data-val-required"));
                                    $(this).on("blur", function () {
                                        objValidation_1.html($(this).attr("data-val-required"));
                                    });
                                    if (invalidFocused_1 == false) {
                                        $(this).focus();
                                        invalidFocused_1 = true;
                                    }
                                }
                            }
                        }
                    });
                }
            }
            else {
                var postValue = dlg.serializeToJson($("form"));
                this.setState({ visible: false, formText: '' });
                button.action(evt, postValue);
            }
        }
    };
    KendoDialog.prototype.getClickedButton = function (e) {
        var index = parseInt(e.target.getAttribute("itemid"));
        var btnEv = __assign({}, e, { button: this.props.buttons[index] });
        return btnEv;
    };
    KendoDialog.prototype.serializeToJson = function (serializer) {
        var data = serializer.serialize().split("&");
        var obj = {};
        for (var key in data) {
            if (obj[data[key].split("=")[0]]) {
                if (typeof (obj[data[key].split("=")[0]]) == "string") {
                    var val = obj[data[key].split("=")[0]];
                    obj[data[key].split("=")[0]] = [];
                    obj[data[key].split("=")[0]].push(val);
                }
                obj[data[key].split("=")[0]].push(data[key].split("=")[1]);
            }
            else {
                obj[data[key].split("=")[0]] = data[key].split("=")[1];
            }
        }
        return obj;
    };
    KendoDialog.prototype.renderButtons = function () {
        var _this = this;
        var buttons = [];
        if (this.props.buttons.length == 1) {
            this.props.buttons[0].forEach(function (e, index) {
                buttons.push(React.createElement(kendo_react_buttons_1.Button, { key: _this.props.uniqueKey + "button" + index, primary: e.primary, itemID: index + "", onClick: _this.onButtonClick }, e.text));
            });
        }
        else {
            this.props.buttons.forEach(function (e, index) {
                buttons.push(React.createElement(kendo_react_buttons_1.Button, { key: _this.props.uniqueKey + "button" + index, primary: e.primary, itemID: index + "", onClick: _this.onButtonClick }, e.text));
            });
        }
        return buttons;
    };
    KendoDialog.prototype.componentDidMount = function () {
        var _this = this;
        if (!this.props || !this.props.getUrl || this.props.getUrl == "")
            return;
        fetch(this.props.getUrl)
            .then(function (result) {
            return result.text();
        })
            .then(function (result) {
            _this.state = __assign({}, _this.state, { visible: _this.state.visible, formText: result });
        });
    };
    KendoDialog.prototype.renderform = function () {
        return this.state.formText;
    };
    KendoDialog.prototype.openForm = function () {
        var _this = this;
        var dialogController = this;
        this.setState({ visible: true });
        if (!this.props || !this.props.getUrl || this.props.getUrl == "")
            return;
        if (!(!this.props && !this.props.getUrl && this.props.getUrl == "")) {
            var prop = this.props;
            if (prop.getMethod.toLowerCase() == "get") {
                fetch(this.props.getUrl)
                    .then(function (result) {
                    return result.text();
                })
                    .then(function (result) {
                    var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    if (extractscript && extractscript.length > 0) {
                        result = result.replace(extractscript[0], "");
                        dialogController.setState({ formText: result, visible: true }, _this.forceUpdate);
                        window.eval($(extractscript[0]).html());
                    }
                    else {
                        dialogController.setState({ formText: result, visible: true }, _this.forceUpdate);
                    }
                });
            }
            else {
                $.ajax({
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                    },
                    url: this.props.getUrl,
                    type: prop.getMethod,
                    data: JSON.stringify(prop.postData),
                    dataType: 'text',
                    success: function (data) {
                        var extractscript = /<script>(.+)<\/script>/gi.exec(data);
                        if (extractscript && extractscript.length > 0) {
                            data = data.replace(extractscript[0], "");
                            dialogController.setState({ formText: data, visible: true }, this.forceUpdate);
                            window.eval(extractscript[1]);
                        }
                        else {
                            dialogController.setState({ formText: data, visible: true }, this.forceUpdate);
                        }
                    },
                    error: function (data, msg) {
                        var message = "";
                        for (var i in data.responseJSON) {
                            for (var j in data.responseJSON[i]) {
                                message += data.responseJSON[i][j] + "\r\n";
                            }
                        }
                        window.Dialog.alert(message);
                    }
                });
            }
        }
    };
    KendoDialog.prototype.render = function () {
        if (this.state.registered == false) {
            this.props.register(this);
            this.setState({ registered: true });
        }
        return (React.createElement("div", null, this.state.visible && React.createElement(kendo_react_dialogs_1.Dialog, { title: this.props.dialogTitle, width: this.props.dialogWidth, height: this.props.dialogHeight },
            React.createElement("div", { className: "container" },
                React.createElement("div", { dangerouslySetInnerHTML: { __html: unescape(this.state.formText) } })),
            React.createElement(kendo_react_dialogs_1.DialogActionsBar, null, this.renderButtons()))));
    };
    return KendoDialog;
}(React.Component));
exports.KendoDialog = KendoDialog;
//# sourceMappingURL=Dialog.js.map