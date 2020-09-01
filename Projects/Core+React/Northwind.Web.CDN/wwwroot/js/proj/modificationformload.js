$(document).ready(function () {
    InitialLoad();
    function InitialLoad() {
        $("#EffectiveDate").kendoDatePicker();
        $("#EnteredDate").kendoDatePicker();
        $("#POPStart").kendoDatePicker();
        $("#POPEnd").kendoDatePicker();
        checkedload('IsAwardAmount');
        checkedload('IsFundingAmount');
        checkedload('IsPOP');
    }
    $('#IsAwardAmount,#IsFundingAmount,#IsPOP').click(function () {
        var id = $(this).attr('id');
        checkedload(id);
    })
    function checkedload(id) {
        var id = id;
        if ($('#' + id).is(':checked')) {
            switch (id) {
                case "IsAwardAmount":
                    $('#idhideAwardAmount').fadeIn('slow');
                    $("#AwardAmount").prop('required', true);
                    $('input[type=number][name=AwardAmount]').prop('required', true);
                    break;
                case "IsFundingAmount":
                    $('#idhideFundingAmount').fadeIn('slow');
                    $("#FundingAmount").prop('required', true);
                    $('input[type=number][name=FundingAmount]').prop('required', true);
                    break;
                case "IsPOP":
                    $('#idHidePop').fadeIn('slow');
                    $("#POPStart").prop('required', true);
                    $("#POPEnd").prop('required', true);
                    break;
            }
        }
        else {
            switch (id) {
                case "IsAwardAmount":
                    $('#idhideAwardAmount').css('display', 'none');
                    $("#AwardAmount").prop('required', false);
                    $("#AwardAmount").val(null);
                    $('input[type=number][name=AwardAmount]').val('0.00')
                    break;
                case "IsFundingAmount":
                    $('#idhideFundingAmount').css('display', 'none');
                    $("#FundingAmount").prop('required', false);
                    $("#FundingAmount").val(null);
                    $('input[type=number][name=FundingAmount]').val('0.00')
                    break;
                case "IsPOP":
                    $('#idHidePop').css('display', 'none');
                    $("#POPStart").prop('required', false);
                    $("#POPStart").val(null);
                    $("#POPEnd").prop('required', false);
                    $("#POPEnd").val(null);
                    break;
            }
        }
    }
});// end of document..