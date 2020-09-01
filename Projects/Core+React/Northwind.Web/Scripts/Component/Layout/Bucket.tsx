import * as React from "react";
import * as ReactDOM from "react-dom";
import { PanelBar, PanelBarItem, PanelBarItemProps } from "@progress/kendo-react-layout";

export interface IBucketProps {
    title?: string;
}

export class Bucket extends React.Component<IBucketProps> {
    constructor(props:IBucketProps) {
        super(props);
    }
    render()
    {
        return (
       <PanelBar animation={false}>
            <PanelBarItem title={this.props.title} expanded={true}>
                {this.props.children}
            </PanelBarItem>
        </PanelBar>);
         //<div>{this.props.children}</div>
       

    }
}