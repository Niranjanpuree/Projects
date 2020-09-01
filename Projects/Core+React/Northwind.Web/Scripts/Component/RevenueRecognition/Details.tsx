import * as React from "react";
import * as ReactDOM from "react-dom";
import { DetailGrid } from "./DetailGrid"
import { Remote } from "../../Common/Remote/Remote"
import { KendoGrid as KendoGrid } from "../../Common/Grid/KendoGrid";
import { Button } from "@progress/kendo-react-buttons";
import { Dialog as KendoDialog, DialogActionsBar } from "@progress/kendo-react-dialogs"
import { any } from "prop-types";

declare var window: any;
declare var document: any;
declare var exportdataToPDF: any;

interface IDetailGridProps {
    parentDomId: string,
    revenueDetail: any,
    parentGuid: any,
    cssUrl: string
}

interface IDetailGridState {
    isbackToList: boolean,
    pageBreakList: any[]
}

export class Details extends React.Component<IDetailGridProps, IDetailGridState>{

    constructor(props: any) {

        super(props);

        this.state = {
            isbackToList: false,
            pageBreakList: [],

        }
        this.backToList = this.backToList.bind(this);
        this.exportToPdf = this.exportToPdf.bind(this);
    }

    backToList(e: any) {
        this.setState({ isbackToList: true });
    }

    exportToPdf(e: any) {
        if (exportdataToPDF) {
            exportdataToPDF(this.state.pageBreakList);
        }
    }

    renderBackToList() {
        if (this.state.isbackToList) {
            return (<DetailGrid
                parentDomId={this.props.parentDomId}
                parentGuid={this.props.parentGuid}
                cssUrl={this.props.cssUrl}
            />)
        }
    }

    renderDetailView() {
        const pdfLabel = {
            color: '#999',
            fontFamily: 'Arial, Helvetica, sans-serif'
        };

        const pdfH6 = {
            color: '#444',
            fontSize: '18px',
            padding: '5px 0 15px',
            margin: '0',
            fontFamily: 'Arial, Helvetica, sans-serif',
            marginBottom: '15px',
            lineHeight: '1.2'
        };
        const marginTop = {
            margin: '15px 0 0',
            fontFamily: 'Arial, Helvetica, sans-serif'

        }

        const pdfTitle = {
            color: '#00649B',
            display: 'block',
            fontSize: '24px',
            fontFamily: ' Arial, Helvetica, sans-serif',
            marginBottom: '15px'
        }
        const pdfUl = {
            listStyle: 'none',
            padding: '0'
        }

        const stepTitle = {
            fontFamily: ' Arial, Helvetica, sans-serif',
            color: '#444',
            fontSize: '20px',
            lineHeight: ' 1.7',
            marginTop: '20px',

        }

        const primaryText = {
            color: '#00649B'
        }

        const pdfDate = {
            fontSize: '14px',
            color: '#444',
            fontFamily: ' Arial, Helvetica, sans-serif',
            display: ' block',

        }

        const marginBottom = {
            marginBottom: '15px',
            fontFamily: 'Arial, Helvetica, sans-serif'

        }

        if (!this.state.isbackToList) {
            const detailhtml = [];
            const fileOrData = [];
            const exportbutton = [];

            let i: number = 0;
            let a: number = 0;
            let b: number = 0;
            for (i; i < this.props.revenueDetail.length; i++) {

                const isCurrentFiscalYearData = [];
                const listExtensionData = [];
                const listRevenuePerformanceObligation = [];
                const step5 = [];


                let dynamicid = "_content" + i;

                let classStep0 = "step0" + dynamicid;
                let classStep1 = "step1" + dynamicid;
                let classStep2 = "step2" + dynamicid;
                let classStep3 = "step3" + dynamicid;
                let classStep4 = "step4" + dynamicid;
                let classStep5 = "step5" + dynamicid;
                let classStep6 = "step6" + dynamicid;


                const isCurrentFiscalYearOfNorthWind = this.props.revenueDetail[i].isCurrentFiscalYearOfNorthWind;
                const isCompleted = this.props.revenueDetail[i].isCompleted;

                if (this.props.revenueDetail[i].listContractExtension != null) {
                    for (a; a < this.props.revenueDetail[i].listContractExtension.length; a++) {
                        listExtensionData.push(<h6 key={i} className="pdfH6 d-inline-block mr-3">{this.props.revenueDetail[i].listContractExtension[a].extensionDateString}</h6>)
                    }
                }

                if (isCompleted) {
                    let key: string = 'performanceObligation_' + [i];
                    if (this.props.revenueDetail[i].listRevenuePerformanceObligation != null) {
                        for (b; b < this.props.revenueDetail[i].listRevenuePerformanceObligation.length; b++) {
                            listRevenuePerformanceObligation.push(
                                <div key={key}>
                                    <div style={marginTop}>
                                        <div style={pdfLabel}>
                                            Performance Obligation represents Revenue Stream Identifier:
                                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].listRevenuePerformanceObligation[b].revenueStreamIdentifierStatus}
                                        </h6>
                                    </div>
                                    <div style={marginTop}>
                                        <div style={pdfLabel}>
                                            Performance Obligation represents right-to-payment:
                                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].listRevenuePerformanceObligation[b].rightToPaymentStatus}
                                        </h6>
                                    </div>

                                    <div style={marginTop}>
                                        <div style={pdfLabel}>
                                            Performance Obligation Routine Service:
                                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].listRevenuePerformanceObligation[b].routineServiceStatus}
                                        </h6>
                                    </div>
                                    <div style={marginTop}>
                                        <div style={pdfLabel}>
                                            Recognize revenue over time or point in time?
                                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].listRevenuePerformanceObligation[b].revenueOverTimePointInTimeStatus}
                                        </h6>
                                    </div>
                                    <div style={marginTop}>
                                        <div style={pdfLabel}>
                                            Method to recognize revenue for obligation satisfied over time?
                                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].listRevenuePerformanceObligation[b].satisfiedOverTimeStatus}
                                        </h6>
                                    </div>
                                </div>
                            )
                        }
                    }
                }

                if (isCompleted) {
                    step5.push(
                        <div>
                            <h5 style={stepTitle}>Step 5 - <span style={primaryText}>Revenue Recognition (For each performance obligation)</span></h5>
                            {listRevenuePerformanceObligation}
                            <div>
                                <div style={pdfLabel}>
                                    Step 5 Notes
                                        </div>
                                <h6 style={pdfH6}>
                                    {this.props.revenueDetail[i].step5NoteStatus}
                                </h6>
                            </div>
                        </div>
                    )
                }

                if (isCurrentFiscalYearOfNorthWind) {
                    let key: string = 'currentFiscalyear_' + [i];
                    this.state.pageBreakList.push(classStep0);
                    this.state.pageBreakList.push(classStep1);
                    this.state.pageBreakList.push(classStep2);
                    this.state.pageBreakList.push(classStep3);
                    this.state.pageBreakList.push(classStep4);
                    if (isCompleted) {
                        this.state.pageBreakList.push(classStep5);
                    }
                    isCurrentFiscalYearData.push(
                        <div key={key}>

                            <li className={classStep1}>
                                <link href={this.props.cssUrl} rel="stylesheet" />
                                <h5 style={stepTitle}>Step 1 - <span style={primaryText}>Identity Contract</span></h5>
                                <div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Enforceable agreement executed and evidenced by
                                        </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].identityContractStatus}
                                        </h6>
                                    </div>

                                    <div>
                                        <div style={pdfLabel}>
                                            Termination Clause's
                                        </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isTerminationClauseGovernmentStandardStatus}
                                        </h6>
                                    </div>

                                    <div>
                                        <div style={pdfLabel}>
                                            Identify Termination Clause
                                        </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].identifyTerminationClauseStatus}
                                        </h6>
                                    </div>

                                    <div>
                                        <div style={pdfLabel}>
                                            Contract term extension/option
                                        </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isContractTermExpansionStatus}
                                        </h6>
                                    </div>

                                    <div>
                                        <div style={pdfLabel}>
                                            Contract term extension/renewal period dates
                                        </div>
                                        <div className="psfDates">
                                            {listExtensionData}
                                        </div>
                                    </div>

                                    <div>
                                        <div style={pdfLabel}>
                                            Approach
                                        </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].approachStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Step 1 Notes
                                        </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].step1NoteStatus}
                                        </h6>
                                    </div>
                                </div>
                            </li>

                            <li className={classStep2}>
                                <link href={this.props.cssUrl} rel="stylesheet" />
                                <h5 style={stepTitle}>Step 2 - <span style={primaryText}>Identity Performance Obligations</span></h5>
                                <div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Contract Deliverables
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].identityPerformanceObligationStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Multi Revenue Stream
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isMultiRevenueStreamStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Is this a repetitive service contract with standard monthly/annual billing amount?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isRepetativeServiceStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Does the contract contain an option to purchase additional goods or services or is there a
                                            contract renewal option?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].hasOptionToPurchageAdditionalGoodsStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            If yes, is it offered at a discounted/lower rate as an incentive for our client to exercise?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isDiscountPurchaseStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Does the contract contain nonrefundable advanced payment?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isNonRefundableAdvancePaymentStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Does the contract contain a discount provision in the event certain criteria are met?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].hasDiscountProvisionStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Does the contract contain a warranty other than an assurance warranty?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].hasWarrentyStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Describe the warranty terms
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].warrantyTermsStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Estimate of probable warranty exposure other than those related to assurance type
                                            warranties:
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].estimateWarrantyExposureStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Step 2 Notes
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].step2NoteStatus}
                                        </h6>
                                    </div>
                                </div>
                            </li>

                            <li className={classStep3}>
                                <link href={this.props.cssUrl} rel="stylesheet" />
                                <h5 style={stepTitle}>Step 3 - <span style={primaryText}>Determine Transaction Price</span></h5>
                                <div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Nature of pricing arrangement (Contract Type)
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].contractTypeStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Pricing Variation
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].isPricingVariationStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Explanation
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].pricingExplanationStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Base Contract Price the company believes it will be entitled to receive:
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].baseContractPrice}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            List each additional option period with corresponding consideration the company expects to
                                            receive
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].additionalPeriodOptionStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Step 3 Note
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].step3NoteStatus}
                                        </h6>
                                    </div>
                                </div>
                            </li>

                            <li className={classStep4}>
                                <link href={this.props.cssUrl} rel="stylesheet" />
                                <h5 style={stepTitle}>Step 4 - <span style={primaryText}>Allocate Transaction Price To Performance Obligations</span></h5>
                                <div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Does the contract contain one or multiple contract obligations?
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].hasMultipleContractObligationsStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            List each obligation with the associated allocation of transaction price/CV:
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].eachMultipleObligationStatus}
                                        </h6>
                                    </div>
                                    <div>
                                        <div style={pdfLabel}>
                                            Step 4 Notes
                                    </div>
                                        <h6 style={pdfH6}>
                                            {this.props.revenueDetail[i].step4NoteStatus}
                                        </h6>
                                    </div>
                                </div>
                            </li>
                            <li className={classStep5}>
                                <link href={this.props.cssUrl} rel="stylesheet" />
                                {step5}
                            </li>

                        </div>
                    );
                }
                else {
                    this.state.pageBreakList.push(classStep0);
                }

                if (this.props.revenueDetail[i].isFile) {
                    let downLoadUrl = '/ContractResourceFile/DownloadDocument/' + this.props.revenueDetail[i].contractResourceFileGuid;
                    detailhtml.push(
                        <div>
                            <div className="row">
                                <p className="col-12 alert alert-secondary rounded-0 mt-n3">
                                    Following file was imported  on
                                   {this.props.revenueDetail[i].updatedDate} for
                                    revenue recognition. Click on the file name to download it.</p>
                            </div>
                            <div className="col-12 text-center">
                                <a href={downLoadUrl}
                                    id="@fileName" className="file-upload-name">
                                    <i className="k-icon k-i-file-txt"></i>
                                    <span className="control-label">{this.props.revenueDetail[i].fileName}</span>
                                </a>
                            </div>
                        </div>
                    );
                }
                else {
                    detailhtml.push(
                        <div id={dynamicid} className="pdfFontFamily" key={i}>
                            <div>
                                <ul id="OverViewPanel" style={pdfUl}>
                                    <li key={classStep0} className={classStep0}>
                                        <link href={this.props.cssUrl} rel="stylesheet" />
                                        <h5 style={primaryText}>Revenue Recognition Detail</h5>
                                        <div>
                                            <div>
                                                <div style={marginBottom}>
                                                    <div style={pdfLabel}>Contract Number</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].basicContractInfoModel.contractNumber}</h6>
                                                </div>
                                                <div style={marginBottom}><div style={pdfLabel}>Contract Title</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].basicContractInfoModel.contractNumber}</h6></div>
                                                <div style={marginBottom}><div style={pdfLabel}>Project Number</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].basicContractInfoModel.projectNumber}</h6></div>
                                                <div style={marginBottom}><div style={pdfLabel}>Company Name</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].basicContractInfoModel.companyName}</h6></div>
                                                <div style={marginBottom}><div style={pdfLabel}>Project Manager</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].projectManagerName}</h6></div>
                                                <div style={marginBottom}><div style={pdfLabel}>Accounting Representative</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].accountingRepresentativeName}</h6></div>
                                                <div style={marginBottom}><div style={pdfLabel}>Update On</div> <h6 style={pdfH6}>{this.props.revenueDetail[0].updatedDate}</h6></div>
                                            </div>
                                        </div>

                                        <div>
                                            <h5 style={primaryText}>Contract Overview</h5>
                                        </div>
                                        <div>
                                            <div style={marginTop}>
                                                <div style={pdfLabel}>
                                                    Is the modification an administrative only change?
                                        </div>
                                                <h6 style={pdfH6}>
                                                    {this.props.revenueDetail[i].isModAdministrativeStatus}
                                                </h6>
                                            </div>
                                            <div style={marginTop}>
                                                <div style={pdfLabel}>
                                                    Does the modification change the scope of the contract?
                                        </div>
                                                <h6 style={pdfH6}>
                                                    {this.props.revenueDetail[i].doesScopeContractChangeStatus}
                                                </h6>
                                            </div>
                                            <div style={marginTop}>
                                                <div style={pdfLabel}>
                                                    Contract within scope of ASC 606
                                        </div>
                                                <h6 style={pdfH6}>
                                                    {this.props.revenueDetail[i].isASC606Status}
                                                </h6>
                                            </div>
                                            <div style={marginTop}>
                                                <div style={pdfLabel}>
                                                    Contract extends beyond current fiscal year of North Wind
                                            </div>
                                                <h6 style={pdfH6}>
                                                    {this.props.revenueDetail[i].isCurrentFiscalYearOfNorthWindStatus}
                                                </h6>
                                            </div>
                                            <div style={marginTop}>
                                                <div style={pdfLabel}>
                                                    Overview Notes
                                            </div>
                                                <h6 style={pdfH6}>
                                                    {this.props.revenueDetail[i].overviewNotesStatus}
                                                </h6>
                                            </div>
                                        </div>
                                    </li>
                                    {isCurrentFiscalYearData}
                                </ul>
                            </div>
                        </div>
                    );
                }
            }
            if (this.props.revenueDetail.length == 1) {
                if (!this.props.revenueDetail[0].isFile) {
                    exportbutton.push(
                        <Button className="btn-primary" onClick={this.exportToPdf} >Export To PDF</Button>
                    );
                }
            }
            else {
                exportbutton.push(
                    <Button className="btn-primary" onClick={this.exportToPdf} >Export To PDF</Button>
                );
            }
            return (
                <div>
                    <div className="d-flex mb-3">
                        <Button className="mr-auto" onClick={this.backToList} >Back To List</Button>
                        {exportbutton}
                    </div>
                    {detailhtml}
                </div>
            )
        }
    }

    render() {
        return (
            <div>
                {this.renderDetailView()}
                {this.renderBackToList()}
            </div>);
    }
}