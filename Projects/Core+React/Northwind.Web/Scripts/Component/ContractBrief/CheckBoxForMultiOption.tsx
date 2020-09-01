import * as React from "react";
import CheckedUnCheckedImage from "./CheckedUnCheckedImage";

function CheckBoxForMultiOption(props: any) {
    var checkboxNames = props.names.split('*')
    var values = props.resultValues.split('*')
    var otherValueChecked = false;
    let otherValueArr: any[] = [];

    values.map((value: any, index: any) => {
        if (checkboxNames.indexOf(value) === -1) {
            if (value !== '') {
                otherValueArr.push(value);
                otherValueChecked = true;
            }
        }
    })
    return (
        <>
            {checkboxNames.map((element: any, index: any) => {
                let isChecked = values.indexOf(element) !== -1;
                return (
                    <div key={index} className="pdf-checkbox">
                        {isChecked && <CheckedUnCheckedImage firstValue={true} secondValue={true} />}
                        {!isChecked && <CheckedUnCheckedImage firstValue={true} secondValue={false} />}
                        <label className="k-checkbox-label-1">{element}</label>
                    </div>
                )
            })}

            {props.showOtherCheckBox &&
                <div className="col pdf-checkbox pl-0">
                    <div className="row">
                        <div className="col-auto">
                            <CheckedUnCheckedImage firstValue={true} secondValue={otherValueArr.length > 0 && otherValueChecked} />
                            <label className="k-checkbox-label-1">Others, (Specify)</label>
                        </div>
                        <div className="col pdf-value">{otherValueArr.join(', ')}</div>
                    </div>
                </div>
            }
        </>
    );
}

export default CheckBoxForMultiOption;