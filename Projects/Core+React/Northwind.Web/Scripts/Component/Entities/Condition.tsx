export class Condition {
    ConditionId?: string;
    Attribute?: Attribute;
    Operator?: Operator;
    IsEntity: boolean;
    Value?: any;
    Value2?: any;
}

export class Resource {
    ResourceId: string;
    ResourceName: string;
    ResourceTitle: string;

    constructor(resourceId: string, resourceName: string, resourceTitle: string) {
        this.ResourceId = resourceId;
        this.ResourceName = resourceName;
        this.ResourceTitle = resourceTitle;          
    }
}


export class Attribute {
    AttributeId: string;
    AttributeName: string;
    AttributeTitle: string;
    AttributeType: number;
    IsEntityLookyup: boolean;
    Entity: string;

    constructor(attributeId: any, attributeName: string, attributeTitle: string, attributeType: number, isEntityLookup: boolean, entity: string) {
        this.AttributeId = attributeId;
        this.AttributeName = attributeName;
        this.AttributeTitle = attributeTitle;
        this.AttributeType = attributeType;
        this.IsEntityLookyup = isEntityLookup;
        this.Entity = entity;
    }
}

export class ResourceAction {
    ActionId: string
    ActionName: string;
    ActionTitle: string;
    ActionType: string;

    constructor(actionId: string, actionName: string, actionTitle: string, actionType:string) {
        this.ActionId = actionId;
        this.ActionName = actionName;
        this.ActionTitle = actionTitle;
        this.ActionType = actionType;
    }
}

export class Operator {
    OperatorId: string;
    OperatorName: number;
    OperatorTitle: string;
    OperatorType: number;

    constructor(operatorId: string, operatorName: number, operatorTitle: string, operatorType: number) {
        this.OperatorId = operatorId;
        this.OperatorName = operatorName;
        this.OperatorTitle = operatorTitle;
        this.OperatorType = operatorType;

    }
}