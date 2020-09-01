import * as React from "react";
import * as ReactDOM from "react-dom";
import { Remote } from "../../Common/Remote/Remote";
import { EmployeeDirectory } from "./EmployeeDirectory";

declare var window: any;
declare var $: any;

if (document.getElementById("employeeBox")) {
    ReactDOM.render(<EmployeeDirectory dataUrl="/EmployeeDirectory/Get" exportUrl="/EmployeeDirectory/Get?searchValue=&take=12&skip=0&dir=asc&filterBy=All" />, document.getElementById("employeeBox"));
}