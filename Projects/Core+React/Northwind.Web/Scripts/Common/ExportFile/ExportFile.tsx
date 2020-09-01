import * as React from "react"
import * as ReactDOM from "react-dom"
import { Remote } from "../../Common/Remote/Remote";
import { KendoGrid } from "../../Common/Grid/KendoGrid";
import { window } from "../Grid";

interface IFileProp {
    exportURL: any;
    columnKey: any;
}

interface IExportFileState {
    columns: any;
}

export class ExportFile extends React.Component<IFileProp, IExportFileState> {
    constructor(props: IFileProp) {
        super(props);

        this.setState({
            columns : []
        });
        this.onExportClick = this.onExportClick.bind(this);
        this.loadHeader = this.loadHeader.bind(this);
    }

    componentDidMount() {
        this.loadHeader()
    }

    loadHeader() {
        var fieldUrl = "/GridFields/" + this.props.columnKey;
        Remote.get(fieldUrl, (fieldList: any) => {
            var column = [];
            for (var i in fieldList) {
                if (Number(i) == fieldList.length - 1) {
                    column.push({
                        //                        ...arrData[i],
                        field: fieldList[i].fieldName,
                        title: fieldList[i].fieldLabel,
                        locked: false,
                        lockable: false
                    });
                }
                else {
                    column.push({
                        //                        ...arrData[i],
                        field: fieldList[i].fieldName,
                        title: fieldList[i].fieldLabel
                    });
                }
            }
            this.setState({ columns: column });
        }, (error: string) => { alert(error) });
       
    }

    onExportClick() {
        this.loadHeader();
        let $this = this;
        var gridColumns = this.state.columns;
        var data: any[] = [];
        var url = this.props.exportURL + "&isExport=true";
        Remote.get(url, (dataList: any) => {
            for (var i = 0; i < dataList.data.length; i++) {
                data.push(dataList.data[i]);
            }
            var grid = $("<div id='pdfGrid'></div>");
            grid.kendoGrid({
                toolbar: ["pdf", "excel"],
                pdf: {
                    allPages: true,
                    avoidLinks: true,
                    paperSize: "A4",
                    margin: { top: "2cm", left: "1cm", right: "1cm", bottom: "1cm" },
                    landscape: true,
                    repeatHeaders: true,
                    template: $("#page-template").html(),
                    scale: 0.8
                },
                dataSource: data,
                height: 550,
                sortable: true,
                pageable: {
                    refresh: true,
                    pageSizes: true,
                    buttonCount: 5
                },
                columns: gridColumns
            });
            grid.data("kendoGrid").saveAsExcel();
        }, (error: string) => { alert(error) });
    }

    render() {
        return (
            <button className="k-button k-primary" onClick={this.onExportClick} >Export</button>
        )
    }
}