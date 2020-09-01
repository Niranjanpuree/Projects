import * as React from "react"
import * as ReactDOM from "react-dom"
import { PDFExport, savePDF } from '@progress/kendo-react-pdf';
import NumberFormat from 'react-number-format';
import { Remote } from "../../Common/Remote/Remote";
import PageTemplate from "./PageTemplate";
import CheckedUnCheckedImage from "./CheckedUnCheckedImage";
import CheckBoxForMultiOption from "./CheckBoxForMultiOption";
import ShowYesNoCheckBoxByLogic from "./ShowYesNoCheckBoxByLogic";
//import {QuestionsConfig } from "./QuestionsConfig";      
import { Questions } from './QuestionsConfig'
import SpinnerPage from "../FileUpload/SpinnerPage";


declare var window: any;
declare var $: any;

interface IContractBriefProps {
    cdnUrl: any;
    contractId: any;
    domToRender: any;
    updatedBy: any;
    updatedOn: any;
    currency: any;
}

interface IContractBriefState {
    showLoading: boolean,

    projectCp: any,
    projectCms: any,
    projectModCP: any[],
    projectModCPEntity: any,  // for Briefed through Mod. No and its effective date..
    questionaires: any[],
    selectedFars: any[],
    showOtherContractType: boolean,
    otherContractTypeValue: any,
    popStart: any,
    popEnd: any,

    //questionaire
    procurementRegulations: string,
    earnedValueManagement: string,
    selectedTruthInNegotiation: string,
    briefStatementOfScopeOfWork: any,
    termsOfTheCostSharingArrangement: string,
    limitationsForLeverOfEffortClause: boolean,
    limitationsForLeverOfEffortClauseValue: any,
    containCeilingsOnTheIndirectCosts: boolean,
    containCeilingsOnTheIndirectCostsValue: any,
    isFCCM: boolean,
    containRestrictions: boolean,
    containRestrictionsValue: any,
    containPrecontract: boolean,
    containPrecontractValue: any
    costUnallowedByTerms: string,
    profitOrFeeProvision: string,
    otherSpecialProvision: string,

    //fars
    costAccountingStandards: string,
    isContractClausesIncoporatedByReference: boolean,
    hasPenaltiesForUnallowableCosts: boolean,
    hasRestrictionsOnOvertime: boolean,
}

export class ContractBrief extends React.Component<IContractBriefProps, IContractBriefState> {

    pdfExportComponent: PDFExport;

    constructor(props: any) {
        super(props);

        this.exportPDFWithComponent = this.exportPDFWithComponent.bind(this);
        this.exportPDFWithMethod = this.exportPDFWithMethod.bind(this);

        this.state = {
            showLoading: false,

            projectCp: {},
            projectCms: {},
            projectModCP: [],
            projectModCPEntity: {},
            questionaires: [],
            selectedFars: [],
            showOtherContractType: false,
            otherContractTypeValue: null,
            popStart: null,
            popEnd: null,

            //questionaire
            procurementRegulations: '',
            earnedValueManagement: '',
            selectedTruthInNegotiation: '',
            briefStatementOfScopeOfWork: null,
            termsOfTheCostSharingArrangement: '',
            limitationsForLeverOfEffortClause: false,
            limitationsForLeverOfEffortClauseValue: null,
            containCeilingsOnTheIndirectCosts: false,
            containCeilingsOnTheIndirectCostsValue: null,
            isFCCM: false,
            containRestrictions: false,
            containRestrictionsValue: null,
            containPrecontract: false,
            containPrecontractValue: null,
            costUnallowedByTerms: '',
            profitOrFeeProvision: '',
            otherSpecialProvision: '',

            //fars
            costAccountingStandards: '',
            isContractClausesIncoporatedByReference: false,
            hasPenaltiesForUnallowableCosts: false,
            hasRestrictionsOnOvertime: false,
        };
    }

    componentDidMount() {
        let sender = this;
        sender.setState({ showLoading: true })
        Remote.get("/Contract/GetContractBriefData?contractGuid=" + this.props.contractId,
            (response: any) => {
                sender.setState({
                    projectCp: response.projectCp,
                    projectModCP: response.projectModCP,
                    projectModCPEntity: response.projectModCPEntity,
                    projectCms: response.projectCms,
                    questionaires: response.questionaires,
                    selectedFars: response.selectedFars,

                    showLoading: false,
                })

                //Pop start and end logic..
                if (sender.state.projectCp !== null) {
                    if (sender.state.projectCp.popStartDate === undefined || sender.state.projectCp.popStartDate == '0001-01-01T00:00:00') {
                        this.setState({
                            popStart: ''
                        })
                    }
                    else {
                        sender.setState({
                            popStart: new Intl.DateTimeFormat('en-US').format(new Date(this.state.projectCp.popStartDate))
                        })
                    }

                    if (sender.state.projectCp.popEndDate === undefined || sender.state.projectCp.popEndDate == '0001-01-01T00:00:00') {
                        this.setState({
                            popEnd: ''
                        })
                    }
                    else {
                        sender.setState({
                            popEnd: new Intl.DateTimeFormat('en-US').format(new Date(this.state.projectCp.popEndDate))
                        })
                    }
                }

                //to show other if not matched with appropriate contract type..

                if (this.state.projectCp !== null) {
                    var contractType = this.state.projectCp.contractType;
                    if (contractType !== 'CPFF' &&
                        contractType !== 'FIXED PRICE' &&
                        contractType !== 'T&M' &&
                        contractType !== 'PHASED FIX FEE' &&
                        contractType !== 'PROPOSAL' &&
                        contractType !== 'INDIRECT') {
                        this.setState({
                            showOtherContractType: true,
                            otherContractTypeValue: contractType,
                        })
                    }
                }
                //Questionaire List..
                this.state.questionaires.forEach(function (question: any) {
                    let selectedProcurementRegulations: any[] = [];
                    let selectedEarnedValueManagement: any[] = [];

                    if (question.question === Questions.Q11) {  //question 11
                        if (question.multiSelectAnswer.length > 0) {
                            question.multiSelectAnswer.forEach(function (multiAns: any) {
                                if (multiAns.isSelected && multiAns.name !== 'OTHERS') {
                                    selectedProcurementRegulations.push(multiAns.name);
                                }
                                else if (multiAns.isSelected && multiAns.name === 'OTHERS') {
                                    selectedProcurementRegulations.push(question.textanswer);
                                }
                            });
                        }

                        sender.setState({
                            procurementRegulations: selectedProcurementRegulations.join('*')
                        });

                    }
                    else if (question.question === Questions.Q14) { //question 15
                        if (question.multiSelectAnswer.length > 0) {
                            question.multiSelectAnswer.forEach(function (multiAns: any) {
                                if (multiAns.isSelected) {
                                    selectedEarnedValueManagement.push(multiAns.name);
                                }
                            });
                        }

                        sender.setState({
                            earnedValueManagement: selectedEarnedValueManagement.join('*')
                        });
                    }

                    else if (question.question === Questions.Q18) { // question 18
                        sender.setState({
                            termsOfTheCostSharingArrangement: question.textanswer
                        });
                    }
                    else if (question.question === Questions.Q19) {   //question 19
                        sender.setState({
                            limitationsForLeverOfEffortClause: question.yesNoAnswer === 'Yes' ? true : false,
                            limitationsForLeverOfEffortClauseValue: question.yesNoAnswer === 'Yes' ? question.textanswer : ''
                        });
                    }
                    else if (question.question === Questions.Q20) {  //question 20
                        sender.setState({
                            containCeilingsOnTheIndirectCosts: question.yesNoAnswer === 'Yes' ? true : false,
                            containCeilingsOnTheIndirectCostsValue: question.yesNoAnswer === 'Yes' ? question.textanswer : ''
                        });
                    }
                    else if (question.question === Questions.Q21) {  //question 21
                        sender.setState({
                            isFCCM: question.yesNoAnswer === 'Yes' ? true : false,
                        });
                    }
                    else if (question.question === Questions.Q23) {  //question 23
                        sender.setState({
                            containPrecontract: question.yesNoAnswer === 'Yes' ? true : false,
                            containPrecontractValue: question.yesNoAnswer === 'Yes' ? question.textanswer : ''
                        });
                    }
                    else if (question.question === Questions.Q25) {  //question 25
                        sender.setState({
                            containRestrictions: question.yesNoAnswer === 'Yes' ? true : false,
                            containRestrictionsValue: question.yesNoAnswer === 'Yes' ? question.textanswer : ''
                        });
                    }
                    else if (question.question === Questions.Q26) {  //question 26
                        sender.setState({
                            costUnallowedByTerms: question.textanswer
                        });
                    }
                    else if (question.question === Questions.Q27) { //question 27
                        sender.setState({
                            profitOrFeeProvision: question.textanswer
                        });
                    }
                    else if (question.question === Questions.Q28) { //question 28
                        sender.setState({
                            otherSpecialProvision: question.textanswer
                        });
                    }
                });

                //Fars..
                sender.state.selectedFars.forEach(function (far: any) {

                    // Cost Accounting Standards (CAS)
                    if (far.farClauseNumber === '52.230-1' ||
                        far.farClauseNumber === '52.230-2' ||
                        far.farClauseNumber === '52.230-3' ||
                        far.farClauseNumber === '52.230-4' ||
                        far.farClauseNumber === '52.230-5' ||
                        far.farClauseNumber === '52.230-6') {

                        var val = "FAR" + far.farClauseNumber;
                        sender.setState({
                            costAccountingStandards: sender.state.costAccountingStandards === '' ? val : sender.state.costAccountingStandards + '*' + val  //concate far clause number with *
                        });
                    }
                    else if (far.farClauseNumber === '52.215-10' || far.farClauseNumber === '52.215.22' ||
                        far.farClauseNumber === '52.215-11' || far.farClauseNumber === '52.215.23' ||
                        far.farClauseNumber === '52.215-12' || far.farClauseNumber === '52.215.24' ||
                        far.farClauseNumber === '52.215-13' || far.farClauseNumber === '52.215.25') {

                        let farNumber: any = '';

                        if (far.farClauseNumber === '52.215-10' || far.farClauseNumber === '52.215.22') {
                            farNumber = 'FAR 52.215.22 (FAR 52.215-10 Effective 10/10/97)';
                        }
                        else if (far.farClauseNumber === '52.215-11' || far.farClauseNumber === '52.215.23') {
                            farNumber = 'FAR 52.215.23 (FAR 52.215-11 Effective 10/10/97)';
                        }
                        else if (far.farClauseNumber === '52.215-12' || far.farClauseNumber === '52.215.24') {
                            farNumber = 'FAR 52.215.24 (FAR 52.215-12 Effective 10/10/97)';
                        }
                        else if (far.farClauseNumber === '52.215-13' || far.farClauseNumber === '52.215.25') {
                            farNumber = 'FAR 52.215.25 (FAR 52.215-13 Effective 10/10/97)';
                        }

                        sender.setState({
                            selectedTruthInNegotiation: sender.state.selectedTruthInNegotiation === '' ? farNumber : sender.state.selectedTruthInNegotiation + '*' + farNumber  //concate value with *
                        });
                    }
                    else if (far.farClauseNumber === '52.252-2') {
                        sender.setState({
                            isContractClausesIncoporatedByReference: true
                        });
                    }
                    else if (far.farClauseNumber === '52.242-3') {
                        sender.setState({
                            hasPenaltiesForUnallowableCosts: true
                        });
                    }
                    else if (far.farClauseNumber === '52.222-2') {
                        sender.setState({
                            hasRestrictionsOnOvertime: true
                        });
                    }
                })

            },
            (err: any) => {
                window.Dialog.alert("Something went wrong");
                sender.setState({ showLoading: false });
            });
    }

    render() {
        //Briefed through Mod.No dated..
        let sender = this;
        let awardDate = '';
        if (sender.state.projectModCPEntity !== null) {
            if (this.state.projectModCPEntity.awardDate !== undefined)
                awardDate = new Intl.DateTimeFormat('en-US').format(new Date(this.state.projectModCPEntity.awardDate));
        }

        let prevTotalFundingModAmount = 0;
        let prevTotalModAmount = 0;

        return (
            <div>
                {sender.state.showLoading && <SpinnerPage />}
                {sender.state.projectCp === null &&
                    <div className="container">
                        <div className="contract-briefings mt-5 text-center">
                            <h4 className="text-muted">Contract Brief pulls financial information directly from cost point. Contract Brief can not be generated because this project was not found in cost point.</h4>
                        </div>
                    </div>
                }
                {sender.state.projectCp !== null && <div>
                    <div className="example-config text-right container mb-3">
                        <button className="k-button btn-primary" onClick={this.exportPDFWithComponent}>Export to pdf</button>
                        { /*
                        &nbsp;
                        <button className="k-button" onClick={this.exportPDFWithMethod}>Export with method</button>
                    */}
                    </div>
                    <h4 className="underline text-center">Contract Briefs</h4>
                    <div id='pdfDiv' className="container">
                        <PDFExport ref={(component) => this.pdfExportComponent = component} paperSize="letter" margin={{ top: '2cm', left: '1cm', right: '1cm', bottom: '1cm' }} scale={0.6} pageTemplate={PageTemplate} fileName={'Contract Brief ' + sender.state.projectCp.projectNumber + '.pdf'}>

                            <div className="contract-briefings">
                                <ol className="p-0">
                                    <li>
                                        <div className="row form-group">
                                            <div className="pdf-label col-3">Contractor Name:</div>
                                            <div className="col pdf-value">{this.state.projectCms.contractorName}</div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row">
                                            <div className="col-6 form-group">
                                                <div className="row">
                                                    <label className="col-6 pdf-label"> Contract Number:</label>
                                                    <div className="pdf-value col-6">{this.state.projectCms.contractNumber}</div>
                                                </div>
                                            </div>
                                            <div className="col-6 form-group">
                                                <div className="row"> <label className="col-6 pdf-label text-right"> Date of Award:</label>
                                                    <div className="pdf-value col-6">{this.state.projectCms.dateOfAward}</div>
                                                </div>
                                            </div>
                                            <div className="col-6 form-group">
                                                <div className="row"> <label className="col-6 pdf-label"> Contractor Job No.</label>
                                                    <div className="pdf-value col-6">{this.state.projectCms.contractJobNumber}</div>
                                                </div>
                                            </div>
                                            <div className="col-6 form-group">
                                                <div className="row"> <label className="col-6 pdf-label text-right">FY Funds:</label>
                                                    <div className="pdf-value col-6"><NumberFormat value={this.state.projectCp ? 0 : this.state.projectCp.fundedAmount} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + ' '} /></div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row">
                                            <div className="col-6 form-group">
                                                <div className="row">
                                                    <label className="col-6 pdf-label"> Briefed through Mod. No.</label>
                                                    <div className="col-6 pdf-value">{this.state.projectModCPEntity ? this.state.projectModCPEntity.modNumber : ''}</div>
                                                </div>
                                            </div>
                                            <div className="col-6 form-group">
                                                <div className="row">
                                                    <label className="col-6 pdf-label text-right"> Dated: </label>
                                                    <div className="col-6 pdf-value">{awardDate}</div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row form-group">
                                            <div className="col-2 pdf-label">Contract Type</div>
                                            <div className="col">
                                                <div className="row">
                                                    <div className="pdf-checkbox">
                                                        <CheckedUnCheckedImage firstValue={this.state.projectCp.contractType} secondValue="CPFF" />
                                                        <label className="k-checkbox-label-1">CPFF</label>
                                                    </div>
                                                    <div className="pdf-checkbox">
                                                        <CheckedUnCheckedImage firstValue={this.state.projectCp.contractType} secondValue="FIXED PRICE" />
                                                        <label className="k-checkbox-label-1"> Fixed Price</label>
                                                    </div>
                                                    <div className="pdf-checkbox">
                                                        <CheckedUnCheckedImage firstValue={this.state.projectCp.contractType} secondValue="T&M" />
                                                        <label className="k-checkbox-label-1"> T&M</label>
                                                    </div>
                                                    <div className="pdf-checkbox">
                                                        <CheckedUnCheckedImage firstValue={this.state.projectCp.contractType} secondValue="PHASE FIX FEE" />
                                                        <label className="k-checkbox-label-1"> Phase Fix Fee</label>
                                                    </div>

                                                </div>
                                                <div className="row">
                                                    <div className="pdf-checkbox">
                                                        <CheckedUnCheckedImage firstValue={this.state.projectCp.contractType} secondValue="PROPOSAL" />
                                                        <label className="k-checkbox-label-1"> Proposal</label>
                                                    </div>
                                                    <div className="pdf-checkbox">
                                                        <CheckedUnCheckedImage firstValue={this.state.projectCp.contractType} secondValue="INDIRECT" />
                                                        <label className="k-checkbox-label-1"> Indirect</label>
                                                    </div>

                                                    <div className="col pdf-checkbox pl-0">
                                                        <div className="row">
                                                            <div className="col-auto">
                                                                {this.state.showOtherContractType && <CheckedUnCheckedImage firstValue={true} secondValue={true} />}
                                                                {!this.state.showOtherContractType && <CheckedUnCheckedImage firstValue={true} secondValue={false} />}
                                                                <label className="k-checkbox-label-1"> Other</label>
                                                            </div>
                                                            <div className="col pdf-value">{this.state.otherContractTypeValue}</div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row">
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-auto pdf-label">
                                                        Estimated Cost:
                                            </div>
                                                    <div className="col pdf-value text-center">
                                                        <NumberFormat value={this.state.projectCp.estimatedCost} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + ' '} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-auto pdf-label">
                                                        Est. Fee:
                                            </div>
                                                    <div className="col pdf-value text-center">
                                                        <NumberFormat value={this.state.projectCp.estimatedFee} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + ' '} />
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-auto pdf-label">
                                                        Total Price:
                                                </div>
                                                    <div className="col pdf-value text-center">
                                                        <NumberFormat value={this.state.projectCp.contractAmount} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + ' '} />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row form-group">
                                            <div className="col-auto pdf-label">Period of Performance From:</div>
                                            <div className="col-auto pdf-value">{this.state.popStart}</div>
                                            <label className="col-auto pdf-label">To:</label>
                                            <div className="col-auto pdf-value">{this.state.popEnd}</div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row form-group">
                                            <div className="col-auto pdf-label">
                                                Is this a Subcontract?:
                                        </div>
                                            <div className="col">
                                                <div className="pdf-checkbox">
                                                    <CheckedUnCheckedImage firstValue={this.state.projectCms.isSubContract} secondValue={true} />
                                                    <label className="k-checkbox-label-1">Yes (Go to Item 8)</label>
                                                </div>
                                                <div className="pdf-checkbox">
                                                    <CheckedUnCheckedImage firstValue={this.state.projectCms.isSubContract} secondValue={false} />
                                                    <label className="k-checkbox-label-1">No (Go to Item 9)</label>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="row">
                                            <div className="col-8 form-group">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Prime Contractor:
                                            </div>
                                                    <div className="col pdf-value"></div>
                                                </div>
                                            </div>
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-5 pdf-label">
                                                        Contract Type
                                                </div>
                                                    <div className="col pdf-value"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8 form-group">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Prime Contract No.
                                                </div>
                                                    <div className="col pdf-value"> {this.state.projectCp.contractNumber}
                                                    </div>
                                                </div>
                                            </div>
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-5 pdf-label text-right">
                                                        Phone
                                                </div>
                                                    <div className="col pdf-value">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Address
                                                </div>
                                                    <div className="col">
                                                        <div className="row">
                                                            <div className="col-12 pdf-value form-group"></div>
                                                            <div className="col-12 pdf-value form-group"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row form-group">
                                                    <div className="col-4 pdf-label">
                                                        Point of Contact
                                                </div>
                                                    <div className="col pdf-value">
                                                        <a href="mailto:cindy.cropper@inl.gov"></a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row form-group">
                                                    <div className="col-4 pdf-label">
                                                        Cognizant
                                                </div>
                                                    <div className="col pdf-value">

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row form-group">
                                                    <div className="col-4 pdf-label">

                                                    </div>
                                                    <div className="col pdf-value">

                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </li>
                                    <li>
                                        <div className="row">
                                            <div className="col-8 form-group">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Acquistion Agency
                                            </div>
                                                    <div className="col pdf-value"></div>
                                                </div>
                                            </div>
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-5 pdf-label">
                                                        Phone
                                                </div>
                                                    <div className="col pdf-value"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Address
                                                </div>
                                                    <div className="col">
                                                        <div className="row">
                                                            <div className="col-12 pdf-value form-group"></div>
                                                            <div className="col-12 pdf-value form-group"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8 form-group">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Point of Contact
                                                </div>
                                                    <div className="col pdf-value">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </li>
                                    <li>
                                        <div className="row">
                                            <div className="col-8 form-group">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Administrative Contract Office:
                                            </div>
                                                    <div className="col pdf-value"></div>
                                                </div>
                                            </div>
                                            <div className="col-4 form-group">
                                                <div className="row">
                                                    <div className="col-5 pdf-label">
                                                        Phone
                                                </div>
                                                    <div className="col pdf-value"></div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Address
                                                </div>
                                                    <div className="col">
                                                        <div className="row">
                                                            <div className="col-12 pdf-value form-group"></div>
                                                            <div className="col-12 pdf-value form-group"></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-8 form-group">
                                                <div className="row">
                                                    <div className="col-4 pdf-label">
                                                        Point of Contact
                                                </div>
                                                    <div className="col pdf-value"><a href="mailto:dcmadenverseniorstaff@dcma.mil"></a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="pdf-label">
                                            Procurement Regulations:
                                        </div>
                                        <div className="form-group">
                                            All that apply

                                    <div className="row form-group pl-4 pr-3">
                                                <CheckBoxForMultiOption names="FAR*DFARS*NASA" resultValues={this.state.procurementRegulations || ''} showOtherCheckBox={true} />
                                            </div>
                                        </div>

                                    </li>
                                    <li>
                                        <div className="pdf-label">
                                            Cost Accounting Standards (CAS)
                                        </div>
                                        <div className="form-group row">
                                            <div className="col-12 form-group">
                                                Clauses contained in the contract.</div>
                                            <div className="col-12">
                                                <div className="row">
                                                    <div className="col-12">
                                                        <CheckBoxForMultiOption names="FAR52.230-1*FAR52.230-2*FAR52.230-3" resultValues={this.state.costAccountingStandards || ''} showOtherCheckBox={false} />
                                                    </div>
                                                    <div className="col-12">
                                                        <CheckBoxForMultiOption names="FAR52.230-4*FAR52.230-5*FAR52.230-6" resultValues={this.state.costAccountingStandards || ''} showOtherCheckBox={false} />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="pdf-label">
                                            Truth In Negotiation (TINA)
                                        </div>
                                        <div className="row form-group">
                                            <div className="col-12 form-group">Clauses contained in the contract.</div>
                                            <div className="col-12">
                                                <CheckBoxForMultiOption names="FAR 52.215.22 (FAR 52.215-10 Effective 10/10/97)"
                                                    resultValues={this.state.selectedTruthInNegotiation || ''} showOtherCheckBox={false} />
                                            </div>
                                            <div className="col-12">
                                                <CheckBoxForMultiOption names="FAR 52.215.23 (FAR 52.215-11 Effective 10/10/97)"
                                                    resultValues={this.state.selectedTruthInNegotiation || ''} showOtherCheckBox={false} />
                                            </div>
                                            <div className="col-12">
                                                <CheckBoxForMultiOption names="FAR 52.215.24 (FAR 52.215-12 Effective 10/10/97)"
                                                    resultValues={this.state.selectedTruthInNegotiation || ''} showOtherCheckBox={false} />
                                            </div>
                                            <div className="col-12">
                                                <CheckBoxForMultiOption names="FAR 52.215.25 (FAR 52.215-13 Effective 10/10/97)"
                                                    resultValues={this.state.selectedTruthInNegotiation || ''} showOtherCheckBox={false} />
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div className="pdf-label">
                                            Earned Value Management (EVM) and Other Program Management System Reporting Requirements
                                    </div>
                                        <div className="row form-group">
                                            <div className="col-12 form-group">
                                                Identify the EVM and other reporting requirements contained in the contract
                                        </div>
                                            <div className="col-12">
                                                <CheckBoxForMultiOption names="Earned Value Management (DFARS 252.242-7002 (MAR2005)) 252.234-7001 (MAR 1998)*Cost Performance Report (CPR) (DD Form 2734) or equivalent*Cost I Schedule Status Report (CISSR) (DD Form 2735) or equivalent (DFARS 252.242-7005(6)*Contract Fund Status Report (CFSR) (DD Form 1586) or equivalent*Contractor Cost Data Report (CCDR) (DD Forms 1921, 1921-1 and 1921-2) or equivalent"
                                                    resultValues={this.state.earnedValueManagement || ''} showOtherCheckBox={false} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="form-group">
                                        <div className="pdf-label">
                                            Brief Statement of Scope of Work
                                        </div>
                                        {this.state.projectCms.briefStatementOfScopeOfWork &&
                                            <div className="col-8 pdf-value">
                                                {this.state.projectCms.briefStatementOfScopeOfWork}
                                            </div>
                                        }
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">
                                            <div className="col-8 pdf-label">
                                                FAR 52.252-2 Contract Clauses Incoporated by reference
                                        </div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.isContractClausesIncoporatedByReference} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8 pdf-label">
                                                If this is a Time & Material (T&M) or fixed price contract attach the schedule of negotiated rates.
                                        </div>
                                            <div className="col-4">
                                                <div className="pdf-checkbox">
                                                    <CheckedUnCheckedImage firstValue={false} secondValue={true} />
                                                    <label>Attached</label>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">
                                            <div className="col-8 pdf-label">
                                                If this is a cost sharing contract, identify the terms of the cost sharing arrangement.
                                        </div>
                                            <div className="col-8">
                                                {this.state.termsOfTheCostSharingArrangement !== '' && this.state.termsOfTheCostSharingArrangement !== null && <div className="pdf-value">{this.state.termsOfTheCostSharingArrangement}</div>}
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Does the contract contain a level of effort clause?:
                                                </div>
                                                    <div className="col-12  pdf-label">If yes, identify the limitations specified in the contract.:</div>
                                                    {this.state.limitationsForLeverOfEffortClause && this.state.limitationsForLeverOfEffortClauseValue !== null && <div className="col-12"><div className="pdf-value">{this.state.limitationsForLeverOfEffortClauseValue}</div></div>}
                                                </div>
                                            </div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.limitationsForLeverOfEffortClause} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Does the contract contain ceilings on the indirect costs?
                                                </div>
                                                    <div className="col-12  pdf-label">If yes, identify the ceiling rates (attach relevant portions of the contract).</div>
                                                    {this.state.containCeilingsOnTheIndirectCosts && this.state.containCeilingsOnTheIndirectCostsValue !== null && <div className="col-12"><div className="pdf-value">{this.state.containCeilingsOnTheIndirectCostsValue}</div></div>}
                                                </div>
                                            </div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.containCeilingsOnTheIndirectCosts} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8 pdf-label">
                                                Is Facilities Capital Cost of Money (FCCM) allowable on this contract?</div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.isFCCM} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8 pdf-label">
                                                Does the contract contain the FAR Penalty Clause (52.242-3)?</div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.hasPenaltiesForUnallowableCosts} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Does the contract contain precontract or cost allowability restrictions?
                                                </div>
                                                    <div className="col-12  pdf-label">If yes, identify the relevant portions of the contract.</div>
                                                    {this.state.containPrecontract && this.state.containPrecontractValue !== null && <div className="col-12"><div className="pdf-value">{this.state.containPrecontractValue}</div></div>}
                                                </div>
                                            </div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.containPrecontract} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">
                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Does the contract contain restrictions on overtime (FAR 52.222-2)?
                                                </div>
                                                </div>
                                            </div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.hasRestrictionsOnOvertime} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Does the contract contain restrictions or special requirements for subcontracts?
                                                </div>
                                                    <div className="col-12  pdf-label">If yes, identify the relevant portions of the contract.</div>
                                                    {this.state.containRestrictions && this.state.containRestrictionsValue !== null && <div className="col-12"><div className="pdf-value">{this.state.containRestrictionsValue}</div></div>}
                                                </div>
                                            </div>
                                            <div className="col-4">
                                                <ShowYesNoCheckBoxByLogic stateValue={this.state.containRestrictions} />
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Identify any costs made specifically unallowable by the terms of the contract.:
                                                </div>
                                                    {this.state.costUnallowedByTerms !== '' && this.state.costUnallowedByTerms !== null && <div className="col-12"><div className="pdf-value">{this.state.costUnallowedByTerms}</div></div>}
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-4">
                                        <div className="row">

                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Identify any profit or fee provisions in the contract.:
                                                </div>
                                                    {this.state.profitOrFeeProvision !== '' && this.state.profitOrFeeProvision !== null && <div className="col-12"><div className="pdf-value">{this.state.profitOrFeeProvision}</div></div>}
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li className="pb-5">
                                        <div className="row">

                                            <div className="col-8">
                                                <div className="row">
                                                    <div className="col-12 pdf-label">
                                                        Identify other special provisions/limitations specified in the contract.:
                                                </div>
                                                    {this.state.otherSpecialProvision !== '' && this.state.otherSpecialProvision !== null && <div className="col-12"><div className="pdf-value">{this.state.otherSpecialProvision}</div></div>}
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ol>
                                <div className="row form-group">
                                    <div className="col-4 pdf-label">Contractor Name :</div>
                                    <div className="col pdf-value">{this.state.projectCms.contractorName}</div>
                                </div>
                                <div className="row form-group">
                                    <div className="col-4 pdf-label">Contract Number :</div>
                                    <div className="col pdf-value">{this.state.projectCms.contractNumber}</div>

                                </div>
                                <div className="row">
                                    <table className="table table-bordered mt-4">
                                        <thead>
                                            <tr>
                                                <th scope="col">Mod Number</th>
                                                <th scope="col">Date</th>
                                                <th scope="col">Change in Funding</th>
                                                <th scope="col">Total Funding</th>
                                                <th scope="col">Cost</th>
                                                <th scope="col">Profit/Fee</th>
                                                <th scope="col">Total</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {
                                                this.state.projectModCP.map((modList: any, index: any) => {
                                                    let newTotalFunding = prevTotalFundingModAmount + modList.fundedAmount;
                                                    let newTotal = prevTotalModAmount + modList.cost + modList.fee;
                                                    let isNewTotalFundingNeg: boolean = modList.newTotalFunding < 0 ? true : false;
                                                    let isFundedAmountNeg: boolean = modList.fundedAmount < 0 ? true : false;
                                                    let isCostNeg: boolean = modList.cost < 0 ? true : false;
                                                    let isTotalNeg: boolean = newTotal.cost < 0 ? true : false;
                                                    let isFeetNeg: boolean = modList.fee < 0 ? true : false;

                                                    prevTotalFundingModAmount = newTotalFunding;
                                                    prevTotalModAmount = newTotal;
                                                    return (
                                                        <tr key={index}>
                                                            <th scope="row">{modList.projectModId}</th>
                                                            <td> {new Intl.DateTimeFormat('en-US').format(new Date(modList.awardDate))}</td>
                                                            <td><NumberFormat value={modList.fundedAmount} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + (isFundedAmountNeg ? ' (' : ' ')} suffix={isFundedAmountNeg ? ')' : ''} /></td>
                                                            <td><NumberFormat value={newTotalFunding} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + (isNewTotalFundingNeg ? ' (' : ' ')} suffix={isNewTotalFundingNeg ? ')' : ''} /></td>
                                                            <td><NumberFormat value={modList.cost} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + (isCostNeg ? ' (' : ' ')} suffix={isCostNeg ? ')' : ''} /></td>
                                                            <td><NumberFormat value={modList.fee} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + (isFeetNeg ? ' (' : ' ')} suffix={isFeetNeg ? ')' : ''} /></td>
                                                            <td><NumberFormat value={newTotal} allowNegative={false} displayType='text' thousandSeparator={true} decimalScale={2} fixedDecimalScale={true} prefix={this.props.currency + (isTotalNeg ? ' (' : ' ')} suffix={isTotalNeg ? ')' : ''} /></td>
                                                        </tr>
                                                    )
                                                })}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </PDFExport>
                    </div>
                </div>}
            </div>
        );
    }

    exportPDFWithMethod = () => {
        savePDF(document.getElementById(this.props.domToRender), { paperSize: 'A4' });
    }
    exportPDFWithComponent = () => {
        this.pdfExportComponent.save();
    }
}