import * as React from "react";
import * as ReactDOM from "react-dom";
import { Input } from "@progress/kendo-react-inputs";
import { DropDownList, MultiSelect } from "@progress/kendo-react-dropdowns";
import { Button } from "@progress/kendo-react-buttons";
import { Bucket } from "../Layout/Bucket";
import { AdvancedSearchDialog } from "../AdvancedSearch/AdvancedSearchDialog";
import { ResourceAction, Resource, Condition } from "../Entities/Condition";
import { IPolicyRule } from "../Entities/IPolicyRule";
import { TreeView, processTreeViewItems, handleTreeViewCheckChange } from '@progress/kendo-react-treeview';

export interface IPolicyRuleProps {
    title?: string;
    rule: IPolicyRule   
    resourceList: Array<Resource>;
    index: number;
    onChange: any;
    onClone: any;
}

export interface IPolicyRuleState {
    resources?: Array<Resource>;
    actions?: Array<ResourceAction>;
    conditions?: Array<Condition>;
    selectedResource?: Resource;
    advancedSearchResources?: Array<string>;
    selectedActions?: Array<ResourceAction>;
    effects?: Array<string>;
    selectedEffect: string;
    actionTreeView: any;
    expand: any;    
    check: any;
}

export class PolicyRule extends React.Component<IPolicyRuleProps,IPolicyRuleState> {

    constructor(props:IPolicyRuleProps) {
        super(props)
        this.state = {
            resources: this.props.resourceList,
            actions: [],
            selectedResource: null,
            selectedActions:[],
            effects: [],
            selectedEffect: "Deny",
            advancedSearchResources: [],
            conditions: [],
            actionTreeView: [],
            expand: {idField:"data.ActionId", ids:[]},           
            check: {idField:"data.ActionId", operationField:'marked', ids:[]}
        }
        
        
        this.handleResourceChange = this.handleResourceChange.bind(this);
        this.handleActionChange = this.handleActionChange.bind(this);
        this.handleConditionChange = this.handleConditionChange.bind(this);
        this.handleCloneClick = this.handleCloneClick.bind(this);
        this.handleRemoveClick = this.handleRemoveClick.bind(this);
        this.handleCheckChange = this.handleCheckChange.bind(this);
        this.handleExpandChange = this.handleExpandChange.bind(this);
        this.convertActionsToTree = this.convertActionsToTree.bind(this);
        this.handleEffectChange = this.handleEffectChange.bind(this);
       
    }

    defaultResourceItem = new Resource("", "", "Select a resource...");
    
    componentDidMount() {
        let effects = new Array<string>();
        effects.push("Allow");
        effects.push("Deny");       
        this.setState({ effects: effects });
        this.convertActionsToTree();
    }

    render() {
        const { expand,check } = this.state;
        return (
            <Bucket title={this.props.title} key={`bucket-${this.props.index}`}>
                <div className="container-fluid">
                    <div className="form-group row pt-3">
                        <div className="col-2">
                            <label htmlFor="resourceList">Select resource</label>
                        </div>
                        <div className="col-md-5">
                            <DropDownList name="resourceList" key={`resourceList-${this.props.index}`} data={this.state.resources} defaultItem={this.defaultResourceItem} defaultValue={this.defaultResourceItem} dataItemKey="ResourceId" textField="ResourceTitle" onChange={this.handleResourceChange} />
                        </div>
                    </div>
                    { (this.state.selectedResource && this.state.selectedResource.ResourceId != "") && 
                    
                        <div className="row form-group">
                            <div className="col-2">
                                <label htmlFor="actionList">Select Actions</label>
                            </div>
                        <div className="col-md-5">
                            <TreeView data={processTreeViewItems(this.state.actionTreeView, { check, expand })}
                                textField="data.ActionTitle"
                                checkField="marked"
                                expandIcons={true}
                                checkboxes={true}
                                onCheckChange={this.handleCheckChange}
                                onExpandChange={this.handleExpandChange}
                            />

                            {/*<MultiSelect name="actionList" key={`actionList-${this.props.index}`} data={this.state.actions} dataItemKey="ActionId" textField="ActionTitle" onChange={this.handleActionChange}/>*/}
                            </div>
                </div>
                }
                    

                    <div className="row form-group">
                        <div className="col-2">
                            <label htmlFor="advancedSearchDialog">State Conditions</label>
                        </div>
                        <div className="col-5">
                            <AdvancedSearchDialog key={`advancedSearchDialog-${this.props.index}`} selectedConditions={this.state.conditions} resourceIds={this.state.advancedSearchResources} onConditionChange={this.handleConditionChange} />
                        </div>
                    </div>

                    <div className="row form-group">
                        <div className="col-2">
                            <label htmlFor="effectList">Effect</label>
                        </div>
                        <div className="col-5">
                            <DropDownList key={`effects-${this.props.index}`} data={this.state.effects} value={this.state.selectedEffect} onChange={this.handleEffectChange} />
                        </div>
                    </div>
                    <div className="row">
                        <div className="col text-right">
                            <Button onClick={this.handleCloneClick} type="button">Clone This</Button>&nbsp;&nbsp;
                            <Button onClick={this.handleRemoveClick} type="button">Remove This</Button>
                           
                        </div>
                    </div>
                    <div className="row">
                        <div className="col">&nbsp;</div>
                    </div>
                </div>


            </Bucket>);
    }

    handleResourceChange(event: any) {       
        
        let selectedResource = event.target.value; 
        if (selectedResource.ResourceId == "") {
            this.setState({ actions: [], selectedActions:[] });
            return;
        }
        let advResources = new Array<string>();
        advResources.push(selectedResource.ResourceName)
        advResources.push("User");        
        Promise.all([this.fetchActions(selectedResource.ResourceId)]).then((result) => {
            this.setState({ actions: result[0], selectedResource: selectedResource, advancedSearchResources: advResources });
            this.convertActionsToTree();         

            this.props.onChange(this.props.index, { resources: [selectedResource], actions: this.state.selectedActions, conditions: this.state.conditions, selectedEffect: this.state.selectedEffect });

        }).catch(function (error) {
            console.log(error);
            });


        
        
        
    }

    handleActionChange(event: any) {

    }

    handleConditionChange(event: any) {

    }

    handleCloneClick(event:any) {       
        this.props.onClone(this.props.index);
    }

    handleRemoveClick() {

    }

    handleCheckChange(event: any) {
        event.item.marked = !event.item.marked;
        let marked = event.item.marked;
        if (event.item.items) {
            event.item.items.forEach((itm: any, idx: any) => {
                itm.marked = marked
            })
        }
    }

    getSelectedActionsBasedOnCheckState(): Array<ResourceAction> {
        var checkState = this.state.check;
        var checkStateIds = checkState.ids;
        let selectedActions = this.state.actions.filter((x: any) => {
            return checkStateIds.includes(x.ActionId);
        });
        return selectedActions;
    }

    handleExpandChange(event: any) {
        
        let ids = this.state.expand.ids.slice();
       
        const index = ids.indexOf(event.item.data.ActionId);

        // Add or remove the item ID (i.e. the item text) depending on
        // whether the item is expanded or collapsed.
        index === -1 ? ids.push(event.item.data.ActionId) : ids.splice(index, 1);

        this.setState({ expand: { ids, idField: "data.ActionId" } });
    }

    handleEffectChange =(event: any)=> {
        this.setState({ selectedEffect: event.target.value }, function () { 
            let selectedActions = this.getSelectedActionsBasedOnCheckState();
            this.props.onChange(this.props.index, { resources: [this.state.selectedResource], actions: selectedActions, conditions: this.state.conditions, effect: this.state.selectedEffect });
        });
    }

    convertActionsToTree() {
        let currentActions = this.state.actions;
        let readActions = currentActions.filter(x => {
            if (x.ActionType.toLowerCase() == "read") {
                return { data: x };
            }
        });

        let readActionsTree = readActions.map(x => { return { data: x } });
        let writeActions = currentActions.filter(x => {
            if (x.ActionType.toLowerCase() == "write") {
                return { data: x }
            }
        });
        let writeActionsTree = writeActions.map(x => { return { data: x } });

        let tree = [
            {
                data: { ActionTitle: 'Read', ActionId:"read-actions", ActionName:"", ActionType:"read" },
                opened: true,
                items: readActionsTree
            },
            {
                data: { ActionTitle: 'Write', ActionId:"write-actions",ActionName:"",ActionType:"write" },
                opened: true,
                items: writeActionsTree
            }];
        this.setState({ actionTreeView: tree });
    }    

    async fetchActions(resourceId:string): Promise<Array<ResourceAction>> {
        let result = await fetch(`/Resource/GetActions?resourceId=${resourceId}`, { headers: { 'Cache-Control': 'no-cache'}});
        let json = await result.json();
        let returnValue = json.map((x:any) => {
            return new ResourceAction(x.actionGuid, x.name, x.title,x.actionType);
        });
        return Promise.resolve<Array<ResourceAction>>(returnValue);
    }
}