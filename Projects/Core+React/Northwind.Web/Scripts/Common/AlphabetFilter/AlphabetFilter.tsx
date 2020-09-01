import * as React from "react"
import * as ReactDOM from "react-dom"
import { Input, Switch } from "@progress/kendo-react-inputs";
import { SplitButton, Button } from "@progress/kendo-react-buttons";
import { Remote } from "../Remote/Remote";

interface IAlphabetProps {
    filterBy: any;
    onClickFilter: any;
    parent?: any;
}
export class AlphabetFilter extends React.Component<IAlphabetProps> {
    constructor(props: IAlphabetProps) {
        super(props);
    }

    render() {
        let alphabet = ["All", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
        const renderAlphabet = alphabet.map(alpha => {
            return (
                <li key={alpha} className={this.props.filterBy === alpha ? 'list-inline-item k-link active' : 'list-inline-item'} onClick={(m) => this.props.onClickFilter(alpha, this.props.parent)}> {alpha}</li >
            )
        });

        return (
            <ul className="list-inline alphabetical-sort">
                {renderAlphabet}
            </ul>
        )
    }
}