import 'core-js';
import * as React from "react";
import * as ReactDOM from "react-dom";
import { Bucket } from '../Layout/Bucket';
import { Input } from '@progress/kendo-react-inputs';
import { Button } from '@progress/kendo-react-buttons';
import { Resource, ResourceAction, Condition } from "../Entities/Condition";
import { PolicyRule } from "../Policy/PolicyRule";
import { IPolicyRule } from "../Entities/IPolicyRule";

export interface IAddPolicyProps {
    policyName?: string;
    policyDesc?: string;
    policyRules?: Array<IPolicyRule>;
}

export interface IAddPolicyState {
    policyName?: string;
    policyDesc?: string;
    policyRules?: Array<IPolicyRule>;
    readyForSave: boolean;
    resourceList: Array<Resource>;
    
}

export class AddPolicy extends React.Component<IAddPolicyProps, IAddPolicyState> {

       constructor(props: IAddPolicyProps) {
           super(props);
           this.state = {
               policyRules: new Array<IPolicyRule>(),
               readyForSave: true,
               resourceList: new Array<Resource>(),
               policyName:this.props.policyName?this.props.policyName: "",
               policyDesc:this.props.policyDesc?this.props.policyDesc: "" 
           }
           this.handlePolicyNameChange = this.handlePolicyNameChange.bind(this);
           this.handlePolicyDescChange = this.handlePolicyDescChange.bind(this);
           this.handleRuleChange = this.handleRuleChange.bind(this);
           this.handleSave = this.handleSave.bind(this);
           this.handleCancel = this.handleCancel.bind(this);
           this.handleRuleClone = this.handleRuleClone.bind(this);
        }   

        render() {
            return (
                <div className="container">
                    <div className="row mb-2 justify-content-end">
                        <div className="col text-right mr-1">
                            <Button primary={true} disabled={!this.state.readyForSave} onClick={this.handleSave} type="button"
                                >Save Policy</Button>&nbsp;&nbsp;
                            <Button onClick={this.handleCancel}>Cancel</Button>
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">                                                  
                                <Bucket title="Basic Policy Information">
                                    <div className="container-fluid">
                                        <div className="row form-group pt-3">
                                            <div className="col-2">
                                                <label htmlFor="policyName">Policy Name</label>
                                            </div>
                                        <div className="col-5">
                                            <Input onChange={this.handlePolicyNameChange} value={this.state.policyName} />
                                            </div>
                                        </div>
                                        <div className="row form-group pt-3">
                                            <div className="col-2">
                                                <label htmlFor="policyDescription">Policy Description</label>
                                            </div>
                                            <div className="col-5">
                                            <Input onChange={this.handlePolicyDescChange}/>
                                            </div>
                                        </div>
                                    </div>
                            </Bucket>
                            {this.state.policyRules.map((x, idx) => <PolicyRule title="Policy Rule" key={`rule-${idx}`} rule={x} onChange={this.handleRuleChange} resourceList={this.state.resourceList} index={idx} onClone={this.handleRuleClone}/>)}                
                        </div>
                    </div>
                </div>
            
        );


    }


    componentDidMount() {       

        Promise.all([this.fetchResources()]).then((result) => {
            if (this.props.policyRules.length < 1) {
                let policyRule = {
                    resources: new Array<Resource>(),
                    actions: new Array<ResourceAction>(),
                    conditions: new Array<Condition>(),
                    effect: "Deny"
                }
                var currentPolicyRules = this.state.policyRules;
                currentPolicyRules.push(policyRule);
                this.setState({ policyRules: currentPolicyRules, resourceList: result[0] });
            }        


        }).catch(function (error)
        {
            console.log(error);
        });
        

    }

    async fetchResources(): Promise<Array<Resource>> {
        let result = await fetch("/Resource/Get", { headers: { 'Cache-Control': 'no-cache' } });
        let json = await result.json();
        let resourceArray = json.map((x: any) => {
            return new Resource(x.resourceGuid, x.name, x.title);
        });
        return Promise.resolve<Array<Resource>>(resourceArray);
    }

    handlePolicyNameChange(event:any) {
        this.setState({ policyName: event.target.value });
        
    }

    handlePolicyDescChange(event:any) {
        this.setState({ policyDesc: event.target.value });
    }

    handleRuleChange(index: number, rule: IPolicyRule) {
        var currentRules = this.state.policyRules;
        currentRules[index] = rule;        
        this.setState({ policyRules: currentRules });
    }

    handleRuleClone(index: number) {
       
        let currentRules = this.state.policyRules;
        currentRules.push(this.state.policyRules[index]);
        this.setState({ policyRules: currentRules });

    }
    
    handleSave() { 
        console.log('Inside handle save....');
        console.log(this.state.policyRules);
        let saveModel = {
            policyName: this.state.policyName,
            policyDesc: this.state.policyDesc,
            policyRules: this.convertPolicyRulesToSaveModel(this.state.policyRules),            
        }
        
        fetch("/IAM/Policy/Save", {
            method: "POST",
            body: JSON.stringify(saveModel),
            headers: new Headers({ 'content-type': 'application/json', 'Cache-Control': 'no-cache' }),
        }).then(response=>response.json()).then(response=>console.log(response)) 
        
        

    }

    convertPolicyRulesToSaveModel(policyRules: Array<IPolicyRule>) {
        let mappedRules = policyRules.map(x => {
            let mappedActions = x.actions.map(y => {
                return {
                    name: y.ActionName,
                    title: y.ActionTitle,
                    actionGuid: y.ActionId,
                    actionType: y.ActionType


                }
            });

            let mappedResources = x.resources.map(y => {
                return {
                    resourceGuid: y.ResourceId,
                    name: y.ResourceName,
                    title: y.ResourceTitle
                }
            });
            return { resources: mappedResources, actions: mappedActions, selectedEffect: x.effect }
        });
        return mappedRules;
            
        }
        


    

    handleCancel() {

    }

    

    

}

//ReactDOM.render(<AddPolicy policyRules={[]}/>, document.getElementById("addPolicyComponentContainer"));