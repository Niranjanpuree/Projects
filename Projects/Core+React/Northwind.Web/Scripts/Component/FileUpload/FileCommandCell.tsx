//import * as React from "react"
//import { GridCell } from '@progress/kendo-react-grid';

//export class FileCommandCell extends GridCell {
//    buttonClick = (e: any, command: any) => {
//        this.props.onChange({ dataItem: this.props.dataItem, e, field: this.props.field, value: command });
//    }

//    render() {
//        if (this.props.rowType !== "data") {
//            return null;
//        }

//        if (this.props.dataItem[this.props.field]) {
//            return (
//                <td>
//                    <button
//                        className="k-button k-grid-save-command"
//                        onClick={(e) => this.buttonClick(e, false)}
//                    > Close
//                    </button>
//                </td>
//            );
//        }

//        return (
//            <td>
//                <button
//                    className="k-primary k-button k-grid-edit-command"
//                    onClick={(e) => this.buttonClick(e, true)}
//                > Edit
//                </button>
//                <button
//                    className="k-button k-grid-remove-command"
//                    onClick={
//                        (e) => confirm('Confirm deleting: ' + this.props.dataItem.contractResourceFileGuid)
//                            && this.buttonClick(e, 'delete')
//                    }
//                > Remove
//                </button>
//            </td>
//        );
//    }
//}


import * as React from 'react';
import { GridCell } from '@progress/kendo-react-grid';
declare var window: any;
declare var $: any;

export default function FileCommanCell(enterEdit: any, remove: any, save: any, cancel: any, editField: any) {
    return class extends GridCell {
        render() {
            let owner = this;
            return !owner.props.dataItem[editField]
                ? (
                    <td>
                        <button
                            type="button"
                            className="k-button k-grid-remove-command"
                            onClick={(e) => {
                                // confirm('Confirm deleting: ' + this.props.dataItem.uploadFileName) && remove(this.props.dataItem)
                                window.Dialog.confirm({
                                    text: "Are you sure you want to  delete the file '" + this.props.dataItem.uploadFileName + "' permanently?",
                                    title: "Confirm",
                                    ok: function (e: any) {
                                        remove(owner.props.dataItem);
                                    },
                                    cancel: function (e: any) {
                                    }
                                });
                            }}>
                            Remove
                        </button>
                    </td>
                )
                : (
                    <td>
                        <button
                            className="k-button k-grid-save-command"
                            onClick={(e) => save(this.props.dataItem)}>
                            {this.props.dataItem.contractResourceFileGuid
                                ? 'Update'
                                : 'Add'}
                        </button>
                        <button
                            className="k-button k-grid-cancel-command"
                            onClick={(e) => cancel(this.props.dataItem)}>{this.props.dataItem.uploadFileName
                                ? 'Cancel'
                                : 'Discard'}
                        </button>
                    </td>
                );
        }
    }
};
