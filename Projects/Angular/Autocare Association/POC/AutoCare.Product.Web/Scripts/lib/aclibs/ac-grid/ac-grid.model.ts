export class AcGridColumn {
    header: string;
    field: any;
    sortable: boolean;
    filterable: boolean;
    columnClass: string;
    columnStyle: string;
    pipe: string;
    selectable: Selectable
}

export class Selectable {
    selected: boolean = true;
}