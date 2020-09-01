import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom";
import { Remote } from "../Remote/Remote"
declare var $: any;

interface IRichEditorProps {
    defaultText?: string;
    filePath: string;
}

interface IReachEditorState {
    editorId: string;
}



export class RichEditor extends React.Component<IRichEditorProps, IReachEditorState> {

    constructor(props: IRichEditorProps) {
        super(props);

        this.state = {
            editorId: "editor_" + (new Date()).getTime()
        }
        
        this.onExecute = this.onExecute.bind(this);
        this.getValue = this.getValue.bind(this);
    }

    componentDidMount() {
        let token = Remote.getCookieValue('X-CSRF-TOKEN');
        let reqValToken = Remote.getCookieValue('RequestVerificationToken');
        let sender = this;
        $("#" + sender.state.editorId).kendoEditor({
            tools: [
                {
                    name: "maximize",
                    tooltip: "Maximize",
                    exec: function (e: any) {
                        var editor = $(this).data("kendoEditor");
                        editor.wrapper.css({ width: $("body").width(), height: $(document).height() });
                        window.scroll(0, 500);
                    }
                },

                {
                    name: "restore",
                    tooltip: "Restore",
                    exec: function (e:any) {
                        var editor = $(this).data("kendoEditor");
                       
                        editor.wrapper.css({ width: "100%", height: "200" });
                    }
                },
                
                "bold",
                "italic",
                "underline",
                "justifyLeft",
                "justifyCenter",
                "justifyRight",
                "justifyFull",
                "createLink",
                "insertImage",
                "viewHtml"

               
            ],
            imageBrowser: {
                transport: {
                    read:
                    {
                        url: "/RichEditor/Folders",
                        headers: {
                            'X-CSRF-TOKEN': token,
                            'RequestVerificationToken': reqValToken
                        }
                    },
                    destroy: {
                        url: "/RichEditor/Folders/Delete",
                        headers: {
                            'X-CSRF-TOKEN': token,
                            'RequestVerificationToken': reqValToken
                        }
                    },
                    create: {
                        url: " /RichEditor/Folders",
                        headers: {
                            'X-CSRF-TOKEN': token,
                            'RequestVerificationToken': reqValToken
                        }
                    },
                    uploadUrl  : "/RichEditor/Upload",
                  
                 
                    thumbnailUrl: "/RichEditor/thumbnail",
                 
                    imageUrl:
                   "/RichEditor/Files?file={0}",
                       
                },
                path: "/"
            }
        });
    }

   

    onExecute(e: any) {
        console.log(e);
        
    }

    getValue() {
        return $("#" + this.state.editorId).val();
    }

    render() {
        
        return (
            <textarea id={this.state.editorId} name="body"  defaultValue={this.props.defaultText}  className="editor_box"></textarea>
        );
    }

}