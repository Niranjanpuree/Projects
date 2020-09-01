import * as React from "react"
import * as ReactDOM from "react-dom"

interface IPageTemplateProps {
    pageNum: number,
    totalPages: number
}

class PageTemplate extends React.Component<IPageTemplateProps> {
    render() {
        return (
            <div>
                <div style={{ position: "absolute", top: "30px", left: "30px", width:'100%'}}>
                    <h4 className="text-center"><u><b>Contract Briefs</b></u></h4>
                </div>

                <div style={{ position: "absolute", right: "30px", top:'30px' }}>
                    Schedule S <br />
                    Page <b>{this.props.pageNum}</b> of {this.props.totalPages}
                </div>
            </div>
        );
    }
}

export default PageTemplate;