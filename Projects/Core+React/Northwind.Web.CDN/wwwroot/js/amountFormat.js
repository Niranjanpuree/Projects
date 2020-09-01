$(document).ready(function () {

    function amountFormat(className) {
        className.val(function (index, value) {
            var outputValue = "";
            if (value.includes(".")) {
                var split = value.split(".");
                var formatedValue = split[0].replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                outputValue = formatedValue + "." + split[1];
            }
            else {
                outputValue = value.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
            if (value.charAt(0) === "-") {
                return "-" + outputValue;
            }
            else {
                return outputValue;
            }
        });
    }

    function InitialLoad() {
        amountFormat($('.amountStyle'));
    }

    InitialLoad();

    $('.amountStyle').on('keyup', function () {
        amountFormat($('.amountStyle'));
    });

}); 