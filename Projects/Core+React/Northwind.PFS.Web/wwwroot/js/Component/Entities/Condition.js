"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Condition {
}
exports.Condition = Condition;
class Resource {
    constructor(resourceId, resourceName, resourceTitle) {
        this.ResourceId = resourceId;
        this.ResourceName = resourceName;
        this.ResourceTitle = resourceTitle;
    }
}
exports.Resource = Resource;
class Attribute {
    constructor(attributeId, attributeName, attributeTitle, attributeType, isEntityLookup, entity) {
        this.AttributeId = attributeId;
        this.AttributeName = attributeName;
        this.AttributeTitle = attributeTitle;
        this.AttributeType = attributeType;
        this.IsEntityLookyup = isEntityLookup;
        this.Entity = entity;
    }
}
exports.Attribute = Attribute;
class ResourceAction {
    constructor(actionId, actionName, actionTitle, actionType) {
        this.ActionId = actionId;
        this.ActionName = actionName;
        this.ActionTitle = actionTitle;
        this.ActionType = actionType;
    }
}
exports.ResourceAction = ResourceAction;
class Operator {
    constructor(operatorId, operatorName, operatorTitle, operatorType) {
        this.OperatorId = operatorId;
        this.OperatorName = operatorName;
        this.OperatorTitle = operatorTitle;
        this.OperatorType = operatorType;
    }
}
exports.Operator = Operator;
//# sourceMappingURL=Condition.js.map