import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { generatePath } from "react-router";
import { OfficeDirectory } from "./OfficeDirectory";
declare var window: any;
declare var $: any;

if (document.getElementById("UserOfficeGrid")) {
    ReactDOM.render(<OfficeDirectory dataUrl="/OfficeDirectory/Get" exportUrl={"/OfficeDirectory/Get?searchValue=&filterBy=All&pageSize=100&skip=0&sortField=&sortDirection=asc"} />, document.getElementById("UserOfficeGrid"));
}