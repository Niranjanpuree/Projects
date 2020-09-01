import * as React from 'react';
import { GridCell } from '@progress/kendo-react-grid';
declare var window: any;
declare var $: any;

export type FilenameAlignment = "Horizontal" | "Vertical";

interface IFileCommandCellProps{
    itemID?: any;
    dataItem: any;
    alignment?: FilenameAlignment;
    stylefunction: any;
    onItemClick: any;
    parent: any;
    onContextMenu?: any;
    onDoubleClick?: any;
}

interface IFileCommandCellState{
    alignment: FilenameAlignment;
}

export class FileCommandCell extends React.Component<IFileCommandCellProps, IFileCommandCellState> {
    
    constructor(props: any){
        super(props);
        this.state = {
            alignment: this.props.alignment || "Vertical"
        }
        this.onItemClick = this.onItemClick.bind(this);
    }

    onItemClick(e: any){
         let e1 = {
            ...e,
            dataItem: this.props.dataItem
        }
        this.props.onItemClick(e1, this.props.parent);
    }
        
    render() {
        let owner = this;
     let icon 
      if(!this.props.dataItem["isFile"])  
      {
          icon = <span className="k-icon k-i-folder"></span>
      }
      else
      {
          icon = <span className={this.props.stylefunction(owner.props.dataItem["uploadFileName"])}></span>
      }
        return  (
                <td  onDoubleClick={this.onItemClick}>
                {this.state.alignment === "Vertical" && <button itemID={this.props.itemID} className="col-lg-2 col-md-3 col-sm-6 text-center explorer-list" onContextMenu={this.props.onContextMenu} onDoubleClick={this.props.onDoubleClick}>
                    <div>
                        {icon}
                        <a title={owner.props.dataItem['description']}>{owner.props.dataItem["uploadFileName"]}</a>
                    </div>
                </button>}
                {this.state.alignment === "Horizontal" && <button itemID={this.props.itemID} className="explorer-list explore-list-view" onContextMenu={this.props.onContextMenu} onDoubleClick={this.props.onDoubleClick}>
                    <div>
                        {icon}
                        <a title={owner.props.dataItem['description']}>{owner.props.dataItem["uploadFileName"]}</a>
                    </div>
                </button>}
                </td>
            )
           
           
    }
   


}