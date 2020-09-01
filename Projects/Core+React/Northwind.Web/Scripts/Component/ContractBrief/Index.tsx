import * as React from "react";
import * as ReactDOM from "react-dom";
import { ContractBrief } from "./ContractBrief";
declare var window: any;
declare var document: any;

function loadExportPdf(domToRender: string, cdnUrl: string, contractId: any, currency: any, updatedBy?: string, updatedOn?: string) {
    ReactDOM.unmountComponentAtNode(document.getElementById(domToRender));
    return ReactDOM.render(<ContractBrief key={"exportPdf" + (new Date()).getTime()} domToRender={domToRender}
        contractId={contractId}
        cdnUrl={cdnUrl}
        updatedBy={updatedBy}
        updatedOn={updatedOn}
        currency={currency}
    />, document.getElementById(domToRender));
}
window.loadExportData = { loadExportPdf: loadExportPdf };