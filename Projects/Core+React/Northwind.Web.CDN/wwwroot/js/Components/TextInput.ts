
class TextInput {
    private _name: string;
    private _id: string;
    private _placeholder: string;
    private _defaultValue: string;
    
    constructor(name:string, id:string, placeholder:string, defaultValue:string) {
        this._name = name;
        this._id = id;
        this._placeholder = placeholder;
        this._defaultValue = defaultValue;
        
    }

    render() {
        return `<input type="text" name="${this._name}" value="${this._defaultValue}" id="${this._id}" placeholder="${this._placeholder}">`;
    }

    

}