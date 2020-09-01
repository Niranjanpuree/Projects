import * as React from "react";
import * as ReactDOM from "react-dom";
import { FarClause } from "./farClause";

declare var window: any;
declare var $: any;

function loadFarClause(domToRender: string, farClauseGuid: any, crudType: any) {
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    return ReactDOM.render(<FarClause dataUrl="" farClauseGuid={farClauseGuid} crudType={crudType} />, document.getElementById(domToRender));
}

window.loadFarClause = { loadFarClauseList: loadFarClause };