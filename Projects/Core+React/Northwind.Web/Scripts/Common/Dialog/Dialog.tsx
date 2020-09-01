import * as React from "react"
import * as ReactDOM from "react-dom"
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { Button } from "@progress/kendo-react-buttons";
import { translateAggregateResults } from "@progress/kendo-data-query";
import { Remote } from "../Remote/Remote";
declare var $: any;
declare var window: any;

interface IDialogProps {
    uniqueKey: string;
    actionText: string;
    dialogTitle: string;
    dialogWidth: string;
    dialogHeight: string;
    postUrl: string;
    method: string;
    getUrl: string;
    buttons: any[];
    register: any;
    visible?: boolean;
}

interface IDialogState {
    visible: boolean;
    formText: any;
    registered: boolean;
}

export class KendoDialog extends React.Component<IDialogProps, IDialogState> {
    constructor(props: any) {
        super(props);
        this.state = {
            visible: this.props.visible || false,
            formText: '',
            registered: false
        };
        this.onButtonClick = this.onButtonClick.bind(this);
        this.openForm = this.openForm.bind(this);
        this.getClickedButton = this.getClickedButton.bind(this);
    }

    async onButtonClick(e: any) {
        let dlg = this;
        let evt = this.getClickedButton(e);
        let attr = e.currentTarget.attributes;
        var buttonIndex = e.target.getAttribute("itemid");
        let button: any = undefined
        if (this.props.buttons.length == 1 && this.props.buttons[0].length > 1) {
            button = this.props.buttons[0][buttonIndex]
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
                    let postValue = this.serializeToJson($("form"));
                    
                    if (this.props.postUrl != "") {
                        let dialogController: any = this;
                        var response = await Remote.postDataAsync(this.props.postUrl, (dialogController.props.postData != undefined && dialogController.props.postData.length > 0) ? dialogController.props.postData : postValue)
                        if (response.ok) {
                            let data = await response.json();
                            if (!button.saveAndNew) {
                                dialogController.setState({ visible: false }, dialogController.forceUpdate);
                            }
                            else {
                                $("form").eq(0).trigger("reset");
                            }
                            button.action(evt, data)
                        }
                        else {
                            let err = await Remote.parseErrorMessage(response);
                            window.Dialog.alert(err);
                        }
                    }
                    else {
                        this.setState({ visible: false, formText: '' }, this.forceUpdate);
                        button.action(evt)
                    }

                }
                else {
                    let invalidFocused: boolean = false;
                    $("form").find("input, textarea").each(function (e: any, index: any) {
                        if ($(this).attr("data-val-required")) {
                            if ($("#" + $(this).attr("id") + "-error").length > 0) {
                                if ($(this).is(":invalid")) {
                                    $("#" + $(this).attr("id") + "-error").html($(this).attr("data-val-required"))
                                    $(this).on("blur", function () {
                                        $("#" + $(this).attr("id") + "-error").html($(this).attr("data-val-required"))
                                    });
                                    if (invalidFocused == false) {
                                        $(this).focus();
                                        invalidFocused = true;
                                    }
                                }
                            }
                            else {
                                if ($(this).is(":invalid")) {
                                    let objValidation = $('[data-valmsg-for="' + $(this).attr("id") + '"]')
                                    objValidation.html($(this).attr("data-val-required"))
                                    $(this).on("blur", function () {
                                        objValidation.html($(this).attr("data-val-required"))
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
                button.action(evt, postValue)
            }
        }
    }

    getClickedButton(e: any) {
        let index = parseInt(e.target.getAttribute("itemid"))
        var btnEv = {
            ...e,
            button: this.props.buttons[index]
        }
        return btnEv;
    }

    getFormData = function (form: any) {
        var unindexed_array = form.serializeArray();
        let indexed_array: any[] = [];
        $.map(unindexed_array, function (n: any, i: any) {
            indexed_array[n['name']] = n['value'];
        });
        return indexed_array;
    };

    serializeToJson(serializer: any) {
        var data = serializer.serialize().split("&");
        var obj: any = {};
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

        if (obj && obj['__RequestVerificationToken']) {
            obj['__RequestVerificationToken'] = undefined;
        }

        return obj;
    }

    renderButtons() {
        let buttons: any[] = [];
        if (this.props.buttons.length == 1) {
            if (this.props.buttons[0].length > 0) {
                this.props.buttons[0].forEach((e: any, index: number) => {
                    buttons.push(<Button type="button" key={this.props.uniqueKey + "button" + index} primary={e.primary} itemID={index + ""} onClick={this.onButtonClick}>{e.text}</Button>)
                })
            }
            else {
                this.props.buttons.forEach((e: any, index: number) => {
                    buttons.push(<Button type="button" key={this.props.uniqueKey + "button" + index} primary={e.primary} itemID={index + ""} onClick={this.onButtonClick}>{e.text}</Button>)
                })
            }
        }
        else {
            this.props.buttons.forEach((e: any, index: number) => {
                buttons.push(<Button type="button" key={this.props.uniqueKey + "button" + index} primary={e.primary} itemID={index + ""} onClick={this.onButtonClick}>{e.text}</Button>)
            })
        }
        return buttons;
    }

    componentDidMount() {
        if (!this.props || !this.props.getUrl || this.props.getUrl == "")
            return;
        let sender = this;
        Remote.getText(this.props.getUrl, (result: any) => {
            sender.setState({
                ...this.state,
                visible: this.state.visible,
                formText: result
            });
        }, (error: any) => {
            window.Dialog.alert(error, "Error");
        })
    }

    renderform() {
        return this.state.formText;
    }

    openForm() {
        let dialogController = this;
        this.setState({ visible: true })
        if (!this.props || !this.props.getUrl || this.props.getUrl === "")
            return;
        if (!(!this.props && !this.props.getUrl && this.props.getUrl === "")) {
            let prop: any = this.props;
            if (prop.getMethod.toLowerCase() === "get") {
                Remote.getText(this.props.getUrl, (result: any) => {
                    let arr: any[] = [];
                    var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    while (extractscript && extractscript.length > 0) {
                        arr.push(extractscript[0])
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
                }, (error: any) => { window.Dialog.alert(error, "Error"); });

            }
            else {
                Remote.postDataText(this.props.getUrl, prop.postData, (result: any) => {
                    let arr: any[] = [];
                    var extractscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi.exec(result);
                    while (extractscript && extractscript.length > 0) {
                        arr.push(extractscript[0])
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
                }, (error: any) => {
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
        return (<div>
            {this.state.visible &&
                <Dialog key={this.props.uniqueKey} title={this.props.dialogTitle} width={this.props.dialogWidth} height={this.props.dialogHeight}>
                    <div className="container">
                        <div dangerouslySetInnerHTML={{ __html: unescape(this.state.formText) }}></div>
                    </div>
                    {this.props.children}
                    <DialogActionsBar>
                        {this.renderButtons()}
                    </DialogActionsBar>
                </Dialog>}
        </div>
        );
    }
}