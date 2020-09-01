"use strict";
var Observable_1 = require('rxjs/Observable');
require('rxjs/add/operator/map');
require('rxjs/add/observable/fromEvent');
var ModalInstance = (function () {
    function ModalInstance(element) {
        this.element = element;
        this.suffix = '.ng2-bs3-modal';
        this.shownEventName = 'shown.bs.modal' + this.suffix;
        this.hiddenEventName = 'hidden.bs.modal' + this.suffix;
        this.visible = false;
        this.init();
    }
    ModalInstance.prototype.open = function () {
        return this.show();
    };
    ModalInstance.prototype.close = function () {
        this.result = ModalResult.Close;
        return this.hide();
    };
    ModalInstance.prototype.dismiss = function () {
        this.result = ModalResult.Dismiss;
        return this.hide();
    };
    ModalInstance.prototype.destroy = function () {
        var _this = this;
        return this.hide().then(function () {
            if (_this.$modal) {
                _this.$modal.data('bs.modal', null);
                _this.$modal.remove();
            }
        });
    };
    ModalInstance.prototype.show = function () {
        var promise = toPromise(this.shown);
        this.resetData();
        this.$modal.modal();
        return promise;
    };
    ModalInstance.prototype.hide = function () {
        if (this.$modal && this.visible) {
            var promise = toPromise(this.hidden);
            this.$modal.modal('hide');
            return promise;
        }
        return Promise.resolve(this.result);
    };
    ModalInstance.prototype.init = function () {
        var _this = this;
        this.$modal = jQuery(this.element.nativeElement);
        this.$modal.appendTo('body');
        this.shown = Observable_1.Observable.fromEvent(this.$modal, this.shownEventName)
            .map(function () {
            _this.visible = true;
        });
        this.hidden = Observable_1.Observable.fromEvent(this.$modal, this.hiddenEventName)
            .map(function () {
            var result = (!_this.result || _this.result === ModalResult.None)
                ? ModalResult.Dismiss : _this.result;
            _this.result = ModalResult.None;
            _this.visible = false;
            return result;
        });
    };
    ModalInstance.prototype.resetData = function () {
        this.$modal.removeData();
        this.$modal.data('backdrop', booleanOrValue(this.$modal.attr('data-backdrop')));
        this.$modal.data('keyboard', booleanOrValue(this.$modal.attr('data-keyboard')));
    };
    return ModalInstance;
}());
exports.ModalInstance = ModalInstance;
function booleanOrValue(value) {
    if (value === 'true')
        return true;
    else if (value === 'false')
        return false;
    return value;
}
function toPromise(observable) {
    return new Promise(function (resolve, reject) {
        observable.subscribe(function (next) {
            resolve(next);
        });
    });
}
(function (ModalResult) {
    ModalResult[ModalResult["None"] = 0] = "None";
    ModalResult[ModalResult["Close"] = 1] = "Close";
    ModalResult[ModalResult["Dismiss"] = 2] = "Dismiss";
})(exports.ModalResult || (exports.ModalResult = {}));
var ModalResult = exports.ModalResult;
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwtaW5zdGFuY2UuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi9zcmMvY29tcG9uZW50cy9tb2RhbC1pbnN0YW5jZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiO0FBQ0EsMkJBQTJCLGlCQUFpQixDQUFDLENBQUE7QUFDN0MsUUFBTyx1QkFBdUIsQ0FBQyxDQUFBO0FBQy9CLFFBQU8sK0JBQStCLENBQUMsQ0FBQTtBQUl2QztJQVlJLHVCQUFvQixPQUFtQjtRQUFuQixZQUFPLEdBQVAsT0FBTyxDQUFZO1FBVi9CLFdBQU0sR0FBVyxnQkFBZ0IsQ0FBQztRQUNsQyxtQkFBYyxHQUFXLGdCQUFnQixHQUFHLElBQUksQ0FBQyxNQUFNLENBQUM7UUFDeEQsb0JBQWUsR0FBVyxpQkFBaUIsR0FBRyxJQUFJLENBQUMsTUFBTSxDQUFDO1FBTWxFLFlBQU8sR0FBWSxLQUFLLENBQUM7UUFHckIsSUFBSSxDQUFDLElBQUksRUFBRSxDQUFDO0lBQ2hCLENBQUM7SUFFRCw0QkFBSSxHQUFKO1FBQ0ksTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsQ0FBQztJQUN2QixDQUFDO0lBRUQsNkJBQUssR0FBTDtRQUNJLElBQUksQ0FBQyxNQUFNLEdBQUcsV0FBVyxDQUFDLEtBQUssQ0FBQztRQUNoQyxNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksRUFBRSxDQUFDO0lBQ3ZCLENBQUM7SUFFRCwrQkFBTyxHQUFQO1FBQ0ksSUFBSSxDQUFDLE1BQU0sR0FBRyxXQUFXLENBQUMsT0FBTyxDQUFDO1FBQ2xDLE1BQU0sQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLENBQUM7SUFDdkIsQ0FBQztJQUVELCtCQUFPLEdBQVA7UUFBQSxpQkFPQztRQU5HLE1BQU0sQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLENBQUMsSUFBSSxDQUFDO1lBQ3BCLEVBQUUsQ0FBQyxDQUFDLEtBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDO2dCQUNkLEtBQUksQ0FBQyxNQUFN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