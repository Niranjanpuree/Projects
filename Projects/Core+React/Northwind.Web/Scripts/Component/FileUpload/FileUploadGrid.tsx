import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../Common/Remote/Remote"
import { Grid, GridColumn as Column, GridToolbar } from '@progress/kendo-react-grid';
import FileCommandCell from './FileCommandCell'


declare var window: any;
declare var $: any;

interface IFileUploadGridProps
{
    parentDomId: any;
    selectedFileData?: any[];
    onFileRemove: Function;
    onChildItemChange: Function;
    container: any;
    isRenderedFromExternalDialog?: boolean;
    isRenderedFromFileUploadDialog?: boolean;
    isAddPage?: boolean;
    isEditPage?: boolean;
    isDetailPage?: boolean;
    isDialogDetailPage?: boolean;
    resetGridPageWhenFileAdd?: boolean;
}

interface IFileUploadGridState
{
    skip: any,
    take: any,
    data: any[],
    editID: any,
    columns: any[],
    show: false,
    clickEnableEdit: boolean
}

export class FileUploadGrid extends React.Component<IFileUploadGridProps, IFileUploadGridState> {

    columns = [
        {
            title: 'Id',
            field: 'contractResourceFileGuid',
            show: false,
            editable: false,
            clickable: false
        },
        {
            title: 'File Name',
            field: 'uploadFileName',
            show: true,
            editable: false,
            clickable: true
        },
        {
            title: 'Description',
            field: 'description',
            show: true,
            editable: true,
            clickable: false,
        },
        {
            title: 'Updated By',
            field: 'updatedBy',
            show: true,
            editable: false,
            clickable: false,
        },
        {
            title: 'Updated On',
            field: 'updatedOn',
            show: true,
            editable: false,
            clickable: false,
        },
        {
            title: 'File size',
            field: 'fileSize',
            show: true,
            editable: false,
            clickable: false
        }
    ]

    constructor(props: any)
    {
        super(props);
        this.remove = this.remove.bind(this);
        this.state = {
            skip: 0,
            take: 5,
            data: this.props.selectedFileData,
            editID: null,
            columns: this.columns,
            show: false,
            clickEnableEdit: false
        };

        this.onDescriptionBlur = this.onDescriptionBlur.bind(this);
        this.onTextAreaChange = this.onTextAreaChange.bind(this);
        this.updateDiscription = this.updateDiscription.bind(this);
    }

    render()
    {

        let data = this.state.data;
        let skip = this.state.skip;
        let isPagable = false;

        if ((this.props.isDetailPage && !this.props.isRenderedFromExternalDialog) ||// if detail page of contract/project do pagination
            (this.props.isDetailPage && this.props.isRenderedFromExternalDialog && this.props.isDialogDetailPage)
        )
        {
            if (this.state.skip === data.length)
            {
                skip = 0
            }
            data = data.slice(skip, skip + this.state.take);
            isPagable = true;
        }

        return (
            <Grid
                data={data.map((item) =>
                    Object.assign({
                        inEdit: item.contractResourceFileGuid === this.state.editID
                    }, item)
                )}
                resizable
                skip={this.state.skip}
                take={this.state.take}
                total={this.props.selectedFileData.length}
                pageable={isPagable}
                onPageChange={this.pageChange}
                editField="inEdit"
                onRowClick={this.rowClick}
                onItemChange={this.itemChange}
                className="uploaded-grid"
            >
                {this.state.columns.map((column: any, index: any) => column.show && (<Column key={index} field={column.field} title={column.title} editable={column.editable} width={column.field == "description" ? "200px" : ""} cell={(p: any, index: number) =>
                {
                    if (!column.editable && !column.clickable)
                    {
                        return (<td>{p.dataItem[column.field]}</td>)
                    }
                    else if (column.clickable)
                    {
                        let downloadUrl = `/ContractResourceFile/DownloadDocument/${p.dataItem["contractResourceFileGuid"]}`
                        downloadUrl = p.dataItem["filePath"] === '' ? 'javascript:void(0)' : downloadUrl;
                        return (
                            <td>
                                <a href={downloadUrl} target="_blank">
                                    {eval("p.dataItem." + p.field)}
                                </a>
                            </td>)
                    }
                    else
                    {
                        if (
                            (
                                ((this.props.isDetailPage || !this.props.isDialogDetailPage) &&
                                    !this.props.isRenderedFromExternalDialog && //either contract/project  non dialog
                                    this.state.clickEnableEdit &&
                                    p.dataItem.editingRow == true &&
                                    p.columnIndex == 1) || this.props.isRenderedFromFileUploadDialog ||
                                (this.props.isRenderedFromExternalDialog && (this.props.isAddPage || this.props.isEditPage))
                            ) ||
                            (!this.props.isDetailPage && p.columnIndex == 1)
                        )
                            return (<td>
                                <textarea itemID={p.dataItem.contractResourceFileGuid}
                                    onBlur={this.onDescriptionBlur}
                                    defaultValue={p.dataItem.description || ''}
                                    autoFocus={p.dataItem.editingRow == true && this.props.isDetailPage} className="form-control">
                                </textarea>
                            </td>)
                        else
                            return (<td>{p.dataItem[column.field]}</td>)
                    }
                }} />))}
                <Column
                    groupable={false}
                    sortable={false}
                    filterable={false}
                    resizable={false}
                    field="_command"
                    title=" "
                    width="180px"
                    cell={FileCommandCell(null, this.remove, null, null, null)}
                />
            </Grid >
        );
    }

    componentDidUpdate(prevProps: any, prevState: any, snapshot: any)
    {
        if (prevState.data.length != this.props.selectedFileData.length)
        {
            this.setState({ data: this.props.selectedFileData, });
            if (this.props.resetGridPageWhenFileAdd)
            {
                this.setState({
                    skip: 0,
                    take: 5,
                })
            }
            return true;
        }
        else
        {
            let i: number = 0;
            for (i; i < prevState.data.length; i++)
            {
                if (prevState.data[i].description !== this.props.selectedFileData[i].description)
                {
                    this.setState({ data: this.props.selectedFileData, });
                    return true;
                }
            }
        }
        return false;
    }

    remove(dataItem: any)
    {
        let data = this.state.data;
        let index = data.findIndex((p: any) => p === dataItem || dataItem.contractResourceFileGuid && p.contractResourceFileGuid === dataItem.contractResourceFileGuid);
        this.props.onFileRemove(dataItem, index, this.props.container);
    }

    rowClick = (e: any) =>
    {
        let data = this.state.data;
        let index = data.findIndex((p: any) => p === e.dataItem || e.dataItem.contractResourceFileGuid && p.contractResourceFileGuid === e.dataItem.contractResourceFileGuid);
        data.map((d: any) =>
        {
            d.editingRow = false;
        });
        data[index].editingRow = true;
        this.setState({
            editID: e.dataItem.contractResourceFileGuid,
            clickEnableEdit: true,
            data: data
        });
    };

    itemChange = (e: any) =>
    {
        const data = this.state.data.slice();
        const index = data.findIndex(d => d.contractResourceFileGuid === e.dataItem.contractResourceFileGuid);
        data[index] = { ...data[index], [e.field]: e.value };
        this.props.onChildItemChange(data[index], index, this.props.container);

        this.setState({
            data: data,
        });
    };

    closeEdit = (e: any) =>
    {
        if (e.target === e.currentTarget)
        {
            this.setState({ editID: null });
        }
    };

    addRecord = () =>
    {
        const newRecord = { contractResourceFileGuid: this.state.data.length + 1 };
        const data = this.state.data.slice();
        data.unshift(newRecord);
    };

    updateDiscription(data: any)
    {
        Remote.postData("/ContractResourceFile/UploadFile?resourceId=" + data.contractResourceFileGuid + "&description=" + data.description, '',
            (response: any) =>
            {
            },
            (err: any) => { window.Dialog.alert(err) });
    }

    onDescriptionBlur(e: any)
    {
        let text = e.target.value;
        let contractFileGuid = e.target.getAttribute("itemid");
        let data = this.state.data.slice();
        const index = data.findIndex(d => d.contractResourceFileGuid === contractFileGuid);
        data[index] = { ...data[index], description: e.target.value };
        this.props.onChildItemChange(data[index], index, this.props.container)

        this.setState({
            clickEnableEdit: false
        })
        this.updateDiscription(data[index]);
    }

    onTextAreaChange(e: any)
    {
        let text = e.target.value;
        let contractFileGuid = e.target.getAttribute("itemid");
        let data = this.state.data.slice();
        const index = data.findIndex(d => d.contractResourceFileGuid === contractFileGuid);
        data[index] = { ...data[index], description: e.target.value };

        this.setState({
            data: data,
        });
    }

    pageChange = (event: any) =>
    {
        this.setState({
            skip: event.page.skip,
            take: event.page.take
        });
    }
} 