import * as React from "react";
import * as ReactDOM from "react-dom";
import { KendoDialog } from "./Dialog";
import { DialogEvents } from "./Events";
// import '@progress/kendo-theme-bootstrap/dist/all.css';
declare var window: any;

if (document.getElementsByClassName("kdialog").length > 0) {
    for (let i = 0; i < document.getElementsByClassName("kdialog").length; i++) {
        var element = document.getElementsByClassName("kdialog")[i];
        ReactDOM.render(<KendoDialog key={element.getAttribute("key")} uniqueKey={element.getAttribute("key")} actionText={element.getAttribute("actionText")} dialogTitle={element.getAttribute("dialogTitle")} dialogHeight={element.getAttribute("dialogHeight")} dialogWidth={element.getAttribute("dialogWidth")} method={element.getAttribute("method")} getUrl={element.getAttribute("getUrl")} postUrl={element.getAttribute("postUrl")} buttons={DialogEvents.buttons} register={() => { }} />, document.getElementById(element.id));
    }
}

export class LoadDialog {

    static loadDialog() {
        for (let i = 0; i < document.getElementsByClassName("kendoDialogMenu").length; i++) {
            var element = document.getElementsByClassName("kendoDialogMenu")[i];
            if (element.tagName == "li") {
                ReactDOM.render(<KendoDialog key={element.getAttribute("key")} uniqueKey={element.getAttribute("key")} actionText={element.getAttribute("actionText")} dialogTitle={element.getAttribute("dialogTitle")} dialogHeight={element.getAttribute("dialogHeight")} dialogWidth={element.getAttribute("dialogWidth")} method={element.getAttribute("method")} getUrl={element.getAttribute("getUrl")} postUrl={element.getAttribute("postUrl")} buttons={DialogEvents.buttons} register={() => { }} />, document.getElementById(element.id));
            }

        }
    }
}

window.LoadDialog = LoadDialog;