import * as React from "react"
declare var window: any;
declare var $: any;

interface ICheckBoxProps {
    initialChecked?: boolean,
    labelName: string,
    disableControl: boolean,
}

interface ICheckBoxState {
    checked: boolean,
}

let CheckBoxElement: any;
export class CheckBox extends React.Component<ICheckBoxProps, ICheckBoxState> {

    constructor(props: any) {
        super(props);

        this.state = { checked: this.props.initialChecked }
        CheckBoxElement = (props: any) => (
            <input type="checkbox" {...props} />
        );
        this.handleCheckboxChange = this.handleCheckboxChange.bind(this);
    }
    handleCheckboxChange = (event: any) => {
        this.setState({ checked: event.target.checked });
    }

    render() {

        return (
            <div>
                {!this.props.disableControl &&
                    <div>
                        <CheckBoxElement
                            checked={this.state.checked}
                            className="k-checkbox"
                            id="isPublic"
                            onChange={this.handleCheckboxChange} />
                        <label htmlFor="isPublic" className="k-checkbox-label">{this.props.labelName}</label>
                    </div>
                }
            </div>
        );

    }
}