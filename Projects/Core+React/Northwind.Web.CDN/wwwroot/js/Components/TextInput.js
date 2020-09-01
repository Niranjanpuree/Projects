var TextInput = /** @class */ (function () {
    function TextInput(name, id, placeholder, defaultValue) {
        this._name = name;
        this._id = id;
        this._placeholder = placeholder;
        this._defaultValue = defaultValue;
    }
    TextInput.prototype.render = function () {
        return "<input type=\"text\" name=\"" + this._name + "\" value=\"" + this._defaultValue + "\" id=\"" + this._id + "\" placeholder=\"" + this._placeholder + "\">";
    };
    return TextInput;
}());
//# sourceMappingURL=TextInput.js.map