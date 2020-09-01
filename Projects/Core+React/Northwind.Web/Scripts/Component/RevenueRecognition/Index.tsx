import * as React from "react";
import * as ReactDOM from "react-dom";
import { DetailGrid } from "./DetailGrid";

declare var window: any;
declare var document: any;

function loadDetailGrid(
    domToRender: string,
    parentGuid: any,
    cssUrl: string
) {
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    ReactDOM.render(<DetailGrid
        parentDomId={domToRender}
        parentGuid={parentGuid}
        cssUrl={cssUrl} />,
        document.getElementById(domToRender));
}
window.detailGrid = { pageView: { loadDetailGrid: loadDetailGrid } };