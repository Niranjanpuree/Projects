import 'react-app-polyfill/ie11';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { RichEditor } from "./RichEditor";
declare var window: any;

function loadRichEditor(id: string, defaultText: string, filePath: string) {
    return ReactDOM.render(<RichEditor defaultText={defaultText} filePath={filePath} />, document.getElementById(id));
}

window.richEditor = {
    loadRichEditor: loadRichEditor
};