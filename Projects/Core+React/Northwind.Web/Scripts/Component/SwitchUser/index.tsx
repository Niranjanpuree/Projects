import 'react-app-polyfill/ie11';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { SwitchUser } from "./SwitchUser";
declare var window: any;

function showSwitchUserDialog(siteUrl: string)
{
    ReactDOM.unmountComponentAtNode(document.getElementById("switchUserLink"));
    ReactDOM.render(<SwitchUser SiteUrl={siteUrl} />, document.getElementById("switchUserLink"));
}


window.switchUser = { showSwitchUserDialog: showSwitchUserDialog }