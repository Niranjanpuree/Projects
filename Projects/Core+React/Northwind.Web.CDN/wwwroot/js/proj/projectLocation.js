/// <reference path="../../../../northwind.web/wwwroot/js/proj/office.js" />
$(document).ready(function () {

    $('.selected-pop').click(function () {
        var textSelectedIndex = $(this).find(".k-state-selected").index();
        var textSelected = $(this).find(".k-state-selected").text();
        var textToFind = "option:contains('" + textSelected + "')";
        var value = $(this).find(textToFind).val();

        if (textSelectedIndex != 0) {
            $('#tbodySyllabuss').AddNew();
        }

        $("table tbody").find('input[name="stateValue"]').each(function () {
            
            //for setting the hidden state value
            var parenttr = $(this).parents("tr");
            var closestTrIndex = $(this).closest("tr").index();
            var currentValue = $(this).val();

            if (closestTrIndex === textSelectedIndex) {
                if (closestTrIndex === 0) {
                    var val = $(this).val(value);
                }
                else if (currentValue != value) {
                    var val = $(this).val(value);
                }
                else {
                    parenttr.remove();
                }
            }
        });

    })

    $('.k-i-arrow-60-left').click(function () {
        alert('');
    });
});