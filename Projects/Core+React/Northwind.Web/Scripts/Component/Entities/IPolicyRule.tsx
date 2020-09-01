import { Resource, ResourceAction, Condition} from "../Entities/Condition"

export interface IPolicyRule {
    resources?: Array<Resource>;
    actions?: Array<ResourceAction>;
    conditions?: Array<Condition>;
    effect?: string;
}