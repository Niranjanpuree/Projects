import * as React from "react";
import CheckedUnCheckedImage from "./CheckedUnCheckedImage";

function ShowYesNoCheckBoxByLogic(props: any) {
    return (
        <>
            <div className="pdf-checkbox">
                <CheckedUnCheckedImage firstValue={props.stateValue} secondValue={true} />
                <label>Yes</label>
            </div>
            <div className="pdf-checkbox">
                <CheckedUnCheckedImage firstValue={props.stateValue} secondValue={false} />
                <label>No</label>
            </div>
        </>
    );
}

export default ShowYesNoCheckBoxByLogic;