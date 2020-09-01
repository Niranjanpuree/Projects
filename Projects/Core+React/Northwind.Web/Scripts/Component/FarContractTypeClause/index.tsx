import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { FarContractTypeClause } from "./farContractTypeClause";

declare var window: any;
declare var $: any;

if (document.getElementById("farContractTypeClauseDatas")) {
    ReactDOM.render(<FarContractTypeClause dataUrl="/Admin/FarContractTypeClause/Get" />, document.getElementById("farContractTypeClauseDatas"));
}