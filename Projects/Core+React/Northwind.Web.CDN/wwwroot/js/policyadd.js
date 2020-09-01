var PolicyAdd = /** @class */ (function () {
    function PolicyAdd(resourceListHtmlRef, actionListHtmlRef, addConditionsRef, panelBarRef, dialog) {
        var _this = this;
        this.attachEvents = function () {
            _this._addConditionsLink.on('click', _this.openConditionDialog);
            _this._resourceSelectBox.on('change', _this.onResourceChange);
            _this._panelBar.kendoPanelBar({
                expandMode: "multiple"
            });
        };
        this.openConditionDialog = function () {
            _this._conditionDialog.show();
        };
        this.onResourceChange = function () {
            var resourceId = $(_this._resourceSelectBox).val();
            if (!_this._kendoSelect) {
                _this._kendoSelect = _this._actionSelectBox.kendoMultiSelect({
                    placeholder: "Select Actions...",
                    dataTextField: "name",
                    dataValueField: "title",
                    autoClose: false,
                    dataSource: {
                        type: "json",
                        serverFiltering: true,
                        transport: {
                            read: {
                                url: "/IAM/Policy/GetActions",
                                type: "get",
                                data: {
                                    resourceId: resourceId,
                                }
                            }
                        }
                    }
                });
            }
            else {
                _this._kendoSelect.data('kendoMultiSelect').dataSource.read({ resourceId: resourceId });
            }
        };
        this._kendoSelect = null;
        this._resourceSelectBox = resourceListHtmlRef,
            this._actionSelectBox = actionListHtmlRef;
        this._addConditionsLink = addConditionsRef;
        this._panelBar = panelBarRef;
        this._conditionDialog = dialog;
        this.attachEvents();
    }
    return PolicyAdd;
}());
//# sourceMappingURL=policyadd.js.map