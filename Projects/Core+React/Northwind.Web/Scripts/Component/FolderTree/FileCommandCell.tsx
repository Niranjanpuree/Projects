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
