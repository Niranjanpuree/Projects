import * as React from "react";
import * as ReactDOM from "react-dom";
import { MessageInbox } from "./MessageInbox";


declare var window: any;
declare var document: any;

function loadMessageInbox(notificationMessageGuid:any) {
    ReactDOM.render(<MessageInbox parentDomId="messageInbox" notificationMessageGuid={notificationMessageGuid} />, document.getElementById("messageInbox"));
}
window.messageInbox = { pageView: { loadMessageInbox: loadMessageInbox } };