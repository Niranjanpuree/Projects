import * as React from "react";
import * as ReactDOM from "react-dom";
import { NotificationSummary } from "./NotificationSummary";

declare var window: any;
declare var document: any;

function loadNotificationSummary(siteUrl?: string) {
    ReactDOM.render(<NotificationSummary parentDomId="notificationSummary" siteUrl={siteUrl} />, document.getElementById("notificationSummary"));
}
window.notificationSummary = { loadNotificationSummary: loadNotificationSummary };