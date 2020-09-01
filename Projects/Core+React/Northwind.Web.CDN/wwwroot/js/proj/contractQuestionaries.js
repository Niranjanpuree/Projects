$(document).ready(function () {

    var panelBar = $(".panelBody").kendoPanelBar({
        collapse: onCollapse
    });

    var onCollapse = function (e) {
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };

    $(".customdatepicker").kendoDatePicker();
    initialload();
    function initialload() {


          $(".checkboxes").prop('required', false);
           

        if (($('input[class="OTHERS k-checkbox"]:checked').val()) || ($('input[class="FAR k-checkbox"]:checked').val()) || ($('input[class="NASA k-checkbox"]:checked').val()) || ($('input[class="DFARS k-checkbox"]:checked').val())) {
            $(".FAR").prop('required', false);
            $(".DFARS").prop('required', false);
            $(".NASA").prop('required', false);
            $(".OTHERS").prop('required', false);
        }
        else {
            $(".FAR").prop('required', false);
            $(".DFARS").prop('required', false);
            $(".NASA").prop('required', false);
            $(".OTHERS").prop('required', false);
        }

        if ($('input[class="OTHERS k-checkbox"]:checked').val()) {
            $("#Others").show("slow");
        }

        else {
            $("#Others").hide("fast");
        }
        if ($('input[class="Test"]:checked').val() == 'Yes') {
            $("#HideReportDate").show("slow");
            $(".ReportNextReportDate").prop('required', false);
        }
        else {
            $("#HideReportDate").hide("fast");
            $(".ReportNextReportDate").prop('required', false);
            $(".ReportNextReportDate").val(null);
            $(".ReportLastReportDate").val(null);
        }
        if ($('input[class="IsGSAschedulesale k-radio"]:checked').val() == 'Yes') {
            $("#HideIsGSAschedulesale").show("slow");
            $(".GSANextReportDate").prop('required', false);
        }
        else {
            $("#HideIsGSAschedulesale").hide("fast");
            $(".GSANextReportDate").prop('required', false);
            $(".GSANextReportDate").val(null);
            $(".GSAlastReportDate").val(null);
        }
        if ($('input[name="ContractQuestionaires.IsSBsubcontract"]:checked').val() == 'True') {
            $("#HideIsSBsubcontract").show("slow");
            $(".SBNextReportDate").prop('required', false);
        }
        else {
            $("#HideIsSBsubcontract").hide("fast");
            $(".SBNextReportDate").prop('required', false);
            $(".SBNextReportDate").val(null);
            $(".SBLastReportDate").val(null);
        }
        if ($('input[class="IsGQAC k-radio"]:checked').val() == 'Yes') {
            $("#HideIsGQAC").show("slow");
            $(".GQACNextReportDate").prop('required', false);
        }
        else {
            $("#HideIsGQAC").hide("fast");
            $(".GQACNextReportDate").prop('required', false);
            $(".GQACNextReportDate").val(null);
            $(".GQACLastReportDate").val(null);
        }
        if ($('input[name="ContractQuestionaires.IsCPARS"]:checked').val() == 'True') {
            $("#HideIsCPARS").show("slow");
            $(".CPARSNextReportDate").prop('required', false);
        }
        else {
            $("#HideIsCPARS").hide("fast");
            $(".CPARSNextReportDate").prop('required', false);
            $(".CPARSNextReportDate").val(null);
            $(".CPARSLastReportDate").val(null);
        }
        if ($('input[class="IsWarranties k-radio"]:checked').val() == 'Yes') {
            $("#HideIsWarranties").show("slow");
            $(".WarrantyProvisionDescription").prop('required', false);
        }
        else {
            $("#HideIsWarranties").hide("fast");
            $(".WarrantyProvisionDescription").prop('required', false);
            $(".WarrantyProvisionDescription").val(null);
        }
        if ($('input[class="Limitations k-radio"]:checked').val() == 'Yes') {
            $("#HideIsLimitations").show("slow");
            $(".LimitationsClass").prop('required', false);
        }
        else {
            $("#HideIsLimitations").hide("fast");
            $(".LimitationsClass").prop('required', false);
            $(".LimitationsClass").val(null);
        }
        if ($('input[class="EffortClause k-radio"]:checked').val() == 'Yes') {
            $("#HideIsEffortClause").show("slow");
            $(".EffortClauseClass").prop('required', false);
        }
        else {
            $("#HideIsEffortClause").hide("fast");
            $(".EffortClauseClass").prop('required', false);
            $(".EffortClauseClass").val(null);
        }
        if ($('input[class="CostRestrictions k-radio"]:checked').val() == 'Yes') {
            $("#HideIsCostRestriction").show("slow");
            $(".CostRestrictionsClass").prop('required', false);
        }
        else {
            $("#HideIsCostRestriction").hide("fast");
            $(".CostRestrictionsClass").prop('required', false);
            $(".CostRestrictionsClass").val(null);
        }
        if ($('input[class="Restrictions k-radio"]:checked').val() == 'Yes') {
            $("#HideIsRestrictions").show("slow");
            $(".Restrictionclass").prop('required', false);
        }
        else {
            $("#HideIsRestrictions").hide("fast");
            $(".Restrictionclass").prop('required', false);
            $(".Restrictionclass").val(null);
        }

    }
    events();
    function events() {
        $(".Test").change(function () {
              switch ($(this).val()) {
                case 'Yes':
                    $("#HideReportDate").show("slow");
                    $(".ReportNextReportDate").prop('required', false);
                    break;
                  case 'NO':
                  case 'No':
                    $("#HideReportDate").hide("slow");
                    $(".ReportNextReportDate").prop('required', false);
                    $(".ReportNextReportDate").val(null);
                    $(".ReportLastReportDate").val(null);
                    break;
            }
        });
        $(".IsGSAschedulesale").change(function () {
             switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsGSAschedulesale").show("slow");
                    $(".GSANextReportDate").prop('required', false);
                    break;
                 case 'NO':
                 case 'No':
                    $("#HideIsGSAschedulesale").hide("slow");
                    $(".GSANextReportDate").prop('required', false);
                     $(".HideIsGSAschedulesale").val(null);
                    $(".GSALastReportDate").val(null);
                    break;
            }
        });
        $(".IsSBsubcontract").change(function () {
            switch ($(this).val()) {
                case 'True':
                    $("#HideIsSBsubcontract").show("slow");
                    $(".SBNextReportDate").prop('required', false);
                    break;
                case 'False':
                    $("#HideIsSBsubcontract").hide("slow");
                    $(".SBNextReportDate").prop('required', false);
                    $(".SBNextReportDate").val(null);
                    $(".SBLastReportDate").val(null);
                    break;
            }
        });
       
        $(".Restrictions").change(function () {
            switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsRestrictions").show("slow");
                    $(".Restrictionclass").prop('required', false);
                    break;
                case 'NO':
                case 'No':
                    $("#HideIsRestrictions").hide("slow");
                    $(".Restrictionclass").prop('required', false);
                    $(".ID1").val(null);
                    break;
            }
        });

        $(".CostRestrictions").change(function () {
            
            switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsCostRestriction").show("slow");
                    $(".CostRestrictionsClass").prop('required', false);
                    break;
                case 'NO':
                case 'No':
                    $("#HideIsCostRestriction").hide("slow");
                    $(".CostRestrictionsClass").prop('required', false);
                    $(".ID2").val(null);
                    $(".CostRestrictionsClass").val(null);
                    break;
            }
        });

        $(".EffortClause").change(function () {
            switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsEffortClause").show("slow");
                    $(".EffortClauseClass").prop('required', false);
                    break;
                case 'NO':
                case 'No':
                    $("#HideIsEffortClause").hide("slow");
                    $(".EffortClauseClass").prop('required', false);
                    $(".ID3").val(null);
                    $(".EffortClauseClass").val(null);
                    break;
            }
        });

        $(".Limitations").change(function () {
            switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsLimitations").show("slow");
                    $(".LimitationsClass").prop('required', false);
                    break;
                case 'NO':
                case 'No':
                    $("#HideIsLimitations").hide("slow");
                    $(".LimitationsClass").prop('required', false);
                    $(".ID4").val(null);
                    $(".LimitationsClass").val(null);
                    break;
            }
        });

        $(".IsGQAC").change(function () {
            switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsGQAC").show("slow");
                    $(".GQACNextReportDate").prop('required', false);
                    break;
                case 'NO':
                case 'No':
                    $("#HideIsGQAC").hide("slow");
                    $(".GQACNextReportDate").prop('required', false);
                    $(".HideIsGQAC").val(null);
                    $(".GQACLastReportDate").val(null);
                    break;
            }
        });
        $(".IsCPARS").change(function () {
            switch ($(this).val()) {
                case 'True':
                    $("#HideIsCPARS").show("slow");
                    $(".CPARSNextReportDate").prop('required', false);
                    break;
                case 'False':
                    $("#HideIsCPARS").hide("slow");
                    $(".CPARSNextReportDate").prop('required', false);
                    $(".CPARSNextReportDate").val(null);
                    $(".CPARSLastReportDate").val(null);
                    break;
            }
        });
        $(".IsWarranties").change(function () {
            switch ($(this).val()) {
                case 'Yes':
                    $("#HideIsWarranties").show("slow");
                    $(".WarrantyProvisionDescription").prop('required', false);
                    break;
                case 'NO':
                case 'No':
                    $("#HideIsWarranties").hide("slow");
                    $(".WarrantyProvisionDescription").prop('required', false);
                    $(".WarrantyProvisionDescription").val(null);
                    $('input[class="k-radio WarrantyProvisioncheckbox"]').prop('checked', false);
                    break;
            }
        });
        $(".OTHERS").change(function () {
            if (this.checked) {
                $("#Others").show("slow");
                $(".FAR").prop('required', false);
            }
            else {
                $("#Others").hide("fast");
                $("#textareaforothers").val(null);
                if (($('input[class="OTHERS k-checkbox"]:checked').val()) || ($('input[class="FAR k-checkbox"]:checked').val()) || ($('input[class="NASA k-checkbox"]:checked').val()) || ($('input[class="DFARS k-checkbox"]:checked').val())) {
                    $(".FAR").prop('required', false);
                }
                else {
                    $(".FAR").prop('required', false);
                }
            }
            
        });
        $(".FAR").change(function () {
            if (this.checked) {
                $(".FAR").prop('required', false);
                $("#FAR").parent().find('.error').remove();

            }
            else {
                if (($('input[class="OTHERS k-checkbox"]:checked').val()) || ($('input[class="FAR k-checkbox"]:checked').val()) || ($('input[class="NASA k-checkbox"]:checked').val()) || ($('input[class="DFARS k-checkbox"]:checked').val())) {
                    $(".FAR").prop('required', false);
                }
                else {
                    $(".FAR").prop('required', false);
                }
            }

        });
        $(".DFARS").change(function () {
             if (this.checked) {
                $(".FAR").prop('required', false);
            }
            else {
                if (($('input[class="OTHERS k-checkbox"]:checked').val()) || ($('input[class="FAR k-checkbox"]:checked').val()) || ($('input[class="NASA k-checkbox"]:checked').val()) || ($('input[class="DFARS k-checkbox"]:checked').val())) {
                    $(".FAR").prop('required', false);
                }
                else {
                    $(".FAR").prop('required', false);
                }
            }

        });
        $(".NASA").change(function () {
            if (this.checked) {
                $(".FAR").prop('required', false);
            }
            else {
                if (($('input[class="OTHERS k-checkbox"]:checked').val()) || ($('input[class="FAR k-checkbox"]:checked').val()) || ($('input[class="NASA k-checkbox"]:checked').val()) || ($('input[class="DFARS k-checkbox"]:checked').val())) {
                    $(".FAR").prop('required', false);
                }
                else {
                    $(".FAR").prop('required', false);
                }
            }

        });
    }

});