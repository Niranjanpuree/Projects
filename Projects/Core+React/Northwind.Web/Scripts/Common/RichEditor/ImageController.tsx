import 'core-js';
import * as React from "react"
import * as ReactDOM from "react-dom"
import { Dialog, DialogActionsBar } from '@progress/kendo-react-dialogs';
import { Button } from '@progress/kendo-react-buttons';
import { Upload } from '@progress/kendo-react-upload';
import { FileBrowser } from './FileBrowser';

interface IImageControllerProp {
    onClose: any;
    onUpdate: any;
    filePath: string;
}


export class ImageProp {
    href: string;
    alt: string;
    width: any;
    height: any;
    vspace: any;
    hspace: any;
    align: any;
}


interface IImageControllerState {
    image: ImageProp;
    showFileDialog: boolean;
}

export class ImageController extends React.Component<IImageControllerProp, IImageControllerState>{

    constructor(props: any) {
        super(props);

        this.state = {
            image: this.getImageInitProp(),
            showFileDialog: false
        }

        this.renderForm = this.renderForm.bind(this);
        this.getImageInitProp = this.getImageInitProp.bind(this);
        this.onFileBrowserShow = this.onFileBrowserShow.bind(this);
        this.renderFileBrowser = this.renderFileBrowser.bind(this);
        this.onCloseFileDialog = this.onCloseFileDialog.bind(this);
        this.onFileSelected = this.onFileSelected.bind(this);
        this.onUpdate = this.onUpdate.bind(this);
    }

    getImageInitProp() {
        let img = new ImageProp();
        img.align = undefined;
        img.alt = undefined;
        img.height = undefined;
        img.href = undefined;
        img.hspace = undefined;
        img.vspace = undefined;
        img.width = undefined;
        return img;
    }


    renderFileBrowser() {
        if (this.state.showFileDialog) {
            return (<FileBrowser onCloseFileDialog={this.onCloseFileDialog} filePath={this.props.filePath} onFileSelected={this.onFileSelected} />);
        }
    }

    onFileBrowserShow(e: any) {
        this.setState({ showFileDialog: true })
    }

    onCloseFileDialog(e: any) {
        this.setState({ showFileDialog: false })
    }

    onFileSelected(e: any) {
        var image = this.state.image;
        if (!image) {
            image = this.getImageInitProp();
        }
        this.setState({ image: { ...image, href: e.relativePath }, showFileDialog: false })
    }

    onUpdate(e: any) {
        if (this.props.onUpdate) {
            this.props.onUpdate(this.state.image);
        }
    }

    renderForm() {
        return (<div className="col-12">
            <h4 className="alert-heading">Image</h4>
            <form>
                <div className="form-group">
                    <div className="row">
                        <label className="col-3" htmlFor="exampleInputEmail1">Image Url:</label>
                        <input type="text" className="col-7 form-control" placeholder="Enter Image Url" defaultValue={this.state.image.href} onChange={(e: any) => { debugger; this.setState({ image: { ...this.state.image, href: e.target.value } }, this.forceUpdate) }} />
                        <button type="button" className="btn col-2" onClick={this.onFileBrowserShow}>...</button>
                    </div>
                </div>
                <div className="form-group">
                    <div className="row">
                        <label className="col-3" htmlFor="exampleInputEmail1">Alt Text:</label>
                        <input type="text" className="col-8 form-control" aria-describedby="emailHelp" placeholder="Enter Alternative Text" />
                    </div>
                </div>
                <div className="row">
                    <div className="col-6">
                        <div className="form-group col-12">
                            <div className="row">
                                <div className="col-6 row">
                                    <label htmlFor="exampleInputPassword1 col-6">Width</label>
                                </div>
                                <input type="text" className="form-control col-6" placeholder="Width" />
                            </div>
                        </div>
                        <div className="form-group col-12">
                            <div className="row">
                                <div className="col-6 row">
                                    <label htmlFor="exampleInputPassword1 col-6">Height</label>
                                </div>
                                <input type="text" className="form-control col-6" placeholder="Height" />
                            </div>
                        </div>
                        <div className="form-group col-12">
                            <div className="row">
                                <div className="col-6 row">
                                    <label htmlFor="exampleInputPassword1 col-6">Border</label>
                                </div>
                                <input type="text" className="form-control col-6" placeholder="Border" />
                            </div>
                        </div>
                        <div className="form-group col-12">
                            <div className="row">
                                <div className="col-6 row">
                                    <label htmlFor="exampleInputPassword1 col-6">HSpace</label>
                                </div>
                                <input type="text" className="form-control col-6" placeholder="HSpace" />
                            </div>
                        </div>
                        <div className="form-group col-12">
                            <div className="row">
                                <div className="col-6 row">
                                    <label htmlFor="exampleInputPassword1 col-6">VSpace</label>
                                </div>
                                <input type="text" className="form-control col-6" placeholder="VSpace" />
                            </div>
                        </div>
                    </div>
                    <div className="col-6" style={{ marginLeft: "-35px", marginBottom: "15px", whiteSpace: "normal", textAlign: "justify" }}>
                        {this.state.image && this.state.image.href && this.state.image.href != undefined && <img {...this.state.image} />}
                        Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.
                    </div>
                    <div className="col-12">
                        <div className="form-group row">
                            <div className="col-3">
                                <label htmlFor="exampleInputPassword1">Alignment</label>
                            </div>
                            <select className="form-control col-6" placeholder="Alignment">
                                <option value="left">Left</option>
                                <option value="right">Right</option>
                                <option value="middle">Middle</option>
                                <option value="top">Top</option>
                                <option value="bottom">Bottom</option>
                            </select>
                        </div>
                    </div>
                </div>
            </form>
        </div>);
    }

    render() {
        return (<Dialog width={450} height={510}>
            {this.renderForm()}
            {this.renderFileBrowser()}
            <DialogActionsBar>
                <Button onClick={this.onUpdate}>Update</Button>
                <Button onClick={this.props.onClose}>Cancel</Button>
            </DialogActionsBar>
        </Dialog>)
    }

}