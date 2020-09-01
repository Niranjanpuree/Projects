$(document).ready(function () {
    $('.customdatepickers').kendoDatePicker();
    var crudType = $('#CrudType');
    var revenueRecognition = $('#RevenueRecognizationGuid');
    var revenuePannel = $(".revenuePanel");
    var nextbutton = $("#nextBtn");
    var previousbutton = $("#prevBtn");
    var saveButton = $('#idsaveBtn');
    var notifyButton = $('#idnotifyBtn');
    var sendNotificationButton = $('#idNotify');
    var accountRepresentative = $('#IsAccountRepresentive');
    var url = "/RevenueRecognition/SaveRevenueRecognition";
    var contractGuid = $('#ContractGuid').val();
    var currentStage = $('#CurrentStage');
    var stage0 = "#tab_0";
    var stage1 = "#tab_1";
    var stage2 = "#tab_2";
    var stage3 = "#tab_3";
    var stage4 = "#tab_4";
    var stage5 = "#tab_5";
    var isTaskorder = $('#isTaskOrder').val;

    function hideAddButton(e) {
        $('#idAddRevenueRecognition').hide();
        $('#idEditRevenueRecognition').show();
        //$('#idViewRevenueRecognition').show();
        //$('#RevenueRecognitionModel_RevenueRecognizationGuid').val(e.revenueGuid);
    }
    function hideEditButton(e) {
        $('#idAddRevenueRecognition').hide();
        $('#idEditRevenueRecognition').hide();
        $('#idHideUpdatedBy').hide();
        $('#idViewRevenueRecognition').show();
        var e = '<small><b> Updated by </b>' + e.updatedby + '<b> on </b>' + e.updatedon + '</small >'
        $('#idViewUpdateBy').append(e);
        $('#idViewUpdateBy').show();
    }
    function dialogClose() {
        var dialog = $("#dialog").data("kendoDialog");
        dialog.close();
    }

    function loadNotification(key, resourceId, contractGuid) {
        callback = '/contract/details/' + contractGuid
        window.loadDistributionListDialog.pageView.loadDistributionListDialog('distributionList',
            key,
            resourceId,
            true,
            true,
            false,
            false,
            true,
            callback,
            callback,
            true);
    }

    function SaveData() {
        var valid = $('#ajaxForm').valid();
        var key = "";
        var key5 = "";
        key = "RevenueRecognition.ContractReview";
        key5 = "RevenueRecognition.ContractCompleted";

        if (valid) {
            var formData = $('#ajaxForm').serialize();
            var token = getCookieValue('X-CSRF-TOKEN');
            var reqValToken = getCookieValue('RequestVerificationToken');

            $('#loading').show();
            $.ajax({
                headers: {
                    'X-CSRF-TOKEN': token,
                    'RequestVerificationToken': reqValToken
                },
                type: 'post',
                url: url,
                dataType: 'json',
                data: JSON.stringify(formData),
                success: function (e) {
                    switch (e.currentStage) {
                        case stage4:
                            if (accountRepresentative.val() != "True") {
                                dialogClose();
                                hideEditButton(e);
                                //window.location.href = "/contract/Details?id=" + e.contractGuid;
                                loadNotification(key, e.revenueGuid, e.contractGuid);
                            }
                            else {
                                loadNextTab();
                            }
                            break;
                        case stage5:
                            dialogClose();
                            hideEditButton(e);
                            if (e.isnotify) {
                                loadNotification(key5, e.revenueGuid, e.contractGuid);
                            }
                            else {
                                $.notify("Successfully Saved", "success");
                            }
                            break;
                        default:
                            crudType.val(e.crudType);
                            revenueRecognition.val(e.revenueGuid);
                            hideAddButton(e);
                            loadNextTab();
                            break;
                    }
                    $('#loading').hide();
                },
                error: function (ee) {
                    //dialogClose();
                    $('#loading').hide();
                    var message = "";
                    for (var i in ee.responseJSON) {
                        if (message != "")
                            message += "<br>";
                        message += ee.responseJSON[i];
                    }
                    Dialog.alert(message);

                }
            });
        }
    }

    function loadTabNextPrevious(loadtab) {
        var tabStrip = $("#tabStrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.disable(tabStrip.tabGroup.children().eq(0));
        var selected = $("#tabStrip").kendoTabStrip().data("kendoTabStrip").select().attr("id");
        var arr = selected.split('_');
        switch (loadtab) {
            case "previous":
                var tabid = "#tab_" + (parseInt(arr[1]) - 1);
                break;
            default:
                var tabid = "#tab_" + (parseInt(arr[1]) + 1);
                break;
        }
        var tabToActivate = $(tabid);
        var tabStrip = $("#tabStrip").kendoTabStrip().data("kendoTabStrip").activateTab(tabToActivate);
        currentStage.val(tabid);
        switch (tabid) {
            case stage0:
                previousbutton.hide();
                saveButton.hide();
                nextbutton.show();
                break;
            case stage4:
                if (accountRepresentative.val() == "True") {
                    nextbutton.show();
                    notifyButton.hide();
                }
                else {
                    saveButton.hide();
                    if (sendNotificationButton.is(":checked")) {
                        nextbutton.hide();
                        notifyButton.show();
                    }
                    else {
                        notifyButton.hide();
                        nextbutton.hide();
                    }
                }
                break;
            case stage5:
                nextbutton.hide();
                saveButton.show();
                break;
            default:
                previousbutton.show();
                notifyButton.hide();
                saveButton.hide();
                nextbutton.show();
                break;
        }
    }

    function TabLoad() {
        $('#prevBtn,#idsaveBtn').hide();
        var tabToActivate = $("#tab_0");
        var tabStrip = $("#tabStrip").kendoTabStrip().data("kendoTabStrip");
        tabStrip.enable(tabStrip.activateTab(tabToActivate));
        tabStrip.disable(tabStrip.tabGroup.children().eq(1));
        tabStrip.disable(tabStrip.tabGroup.children().eq(2));
        tabStrip.disable(tabStrip.tabGroup.children().eq(3));
        tabStrip.disable(tabStrip.tabGroup.children().eq(4));
        tabStrip.disable(tabStrip.tabGroup.children().eq(5));
    }

    function loadNextTab() {
        loadTabNextPrevious("next");
    }

    function InitialLoad() {
        var IsModAdministrative = $('.IsModAdministrative:checked').val();
        switch (IsModAdministrative) {
            default:
                $('#idHIdeScopeContract').css('display', 'none');
                break;
            case "No":
                $('#idHIdeScopeContract').fadeIn('slow');
                break;
        }
        var IsCurrentFiscalYearOfNorthWind = $('.IsCurrentFiscalYearOfNorthWind:checked').val();
        switch (IsCurrentFiscalYearOfNorthWind) {
            default:
                $('#idHideFiscalYearInfo').css('display', 'none');
                $('#idsaveBtn').hide();
                $('#idnotifyBtn').hide();
                $('#nextBtn').show();
                break;
            case "False":
                $('#idHideFiscalYearInfo').fadeIn('slow');
                $('#prevBtn').hide();
                $('#idsaveBtn').show();
                $('#nextBtn').hide();
                $('#idnotifyBtn').hide();
                break;
        }

        var IsTerminationClauseGovernmentStandard = $('.IsTerminationClauseGovernmentStandard:checked').val();
        switch (IsTerminationClauseGovernmentStandard) {
            default:
                $('#idHideTermianlClause').css('display', 'none');
                break;
            case "Other":
                $('#idHideTermianlClause').fadeIn('slow');
                break;
        }
        var IsContractTermExpansion = $('.IsContractTermExpansion:checked').val();
        switch (IsContractTermExpansion) {
            default:
                $('#idHideExtensionPeriod').css('display', 'none');
                break;
            case "True":
                $('#idHideExtensionPeriod').fadeIn('slow');
                break;
        }

        var IsMultiRevenueStream = $('.IsMultiRevenueStream:checked').val();
        switch (IsMultiRevenueStream) {
            default:
                $('#idHideWBSGrid').css('display', 'none');
                break;
            case "True":
                $('#idHideWBSGrid').fadeIn('slow');
                break;
        }
        var HasWarrenty = $('.HasWarrenty:checked').val();
        switch (HasWarrenty) {
            default:
                $('#idHIdeWarrantyTerms').css('display', 'none');
                break;
            case "True":
                $('#idHIdeWarrantyTerms').fadeIn('slow');
                break;
        }
        var HasOptionToPurchageAdditionalGoods = $('.HasOptionToPurchageAdditionalGoods:checked').val();
        switch (HasOptionToPurchageAdditionalGoods) {
            default:
                $('#idHideIsDiscountPurchase').css('display', 'none');
                break;
            case "True":
                $('#idHideIsDiscountPurchase').fadeIn('slow');
                break;
        }

        var IsPricingVariation = $('.IsPricingVariation:checked').val();
        switch (IsPricingVariation) {
            default:
                $('#idHIdePricingExplanation').css('display', 'none');
                break;
            case "True":
                $('#idHIdePricingExplanation').fadeIn('slow');
                break;
        }
        var HasMultipleContractObligations = $('.HasMultipleContractObligations:checked').val();
        switch (HasMultipleContractObligations) {
            default:
                $('#idHideMultipleObligation').css('display', 'none');
                break;
            case "Multiple":
                $('#idHideMultipleObligation').fadeIn('slow');
                break;
        }
        //Ends
    }
    TabLoad();
    InitialLoad();

    var panelBar = revenuePannel.kendoPanelBar({
        collapse: onCollapse
    });
    var onCollapse = function (e) {
        // detach collapse event handler via unbind()
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };
    nextbutton.click(function () {
        SaveData();
    })
    previousbutton.click(function () {
        loadTabNextPrevious("previous");
    })

    sendNotificationButton.click(function () {
        if ($(this).is(":checked")) {
            notifyButton.show();
        }
        else {
            notifyButton.hide();
        }
    });
    notifyButton.click(function () {
        SaveData();
    })
    saveButton.click(function () {
        currentStage.val(stage5);
        SaveData();
    })

    $("#idCanceBtn").click(function () {
        $('html').removeClass('htmlClass');
        dialogClose();
        //window.location.href = "/contract/Details?id=" + contractGuid;
    });
    //on change function starts here
    $(".IsModAdministrative,.IsCurrentFiscalYearOfNorthWind").change(function () {
        InitialLoad();
    })
    $(".IsTerminationClauseGovernmentStandard,.IsContractTermExpansion").change(function () {
        InitialLoad();
    })
    $(".IsMultiRevenueStream,.HasWarrenty,.HasOptionToPurchageAdditionalGoods").change(function () {
        InitialLoad();
    })
    $(".IsPricingVariation").change(function () {
        InitialLoad();
    })
    $(".HasMultipleContractObligations").change(function () {
        var HasMultipleContractObligations = $(this).val();
        switch (HasMultipleContractObligations) {
            default:
                $('#idHideMultipleObligation').css('display', 'none');
                break;
            case "Multiple":
                $('#idHideMultipleObligation').fadeIn('slow');
                break;
        }
    })
    //ends
    // Add And Remove Row Dynamically
    $('#idAddExtensionPeriod').click(function () {
        var data = $('#tbodySyllabus').AddNew();
    });
    $('#idAddPerfoemanceObli').click(function () {
        $('#tbodySyllabuss').AddNew();
        //$('.idhideCheckboxs').fadeIn('slow');
    });
    // Find and remove selected table rows
    $("#idDeleteExtensionPeriod").click(function () {
        var ids = [];
        var removetr = [];
        $("table tbody").find('input[name="record"]').each(function () {
            if ($(this).is(":checked") && $(this).closest("tr").index() != 0) {
                var parenttr = $(this).parents("tr");
                var val = $(this).closest("td").find('input[type="hidden"]').val();
                if (val.length != 0) {
                    ids.push(val);
                    removetr.push(parenttr);
                }
                else {
                    parenttr.remove();
                }
            }
        });
        if (ids.length > 0) {
            var data = JSON.stringify(ids)
            $.ajax({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                dataType: 'json',
                type: "POST",
                url: "/RevenueRecognition/DeleteRevenueExtensionPeriod",
                data: data,
                success: function (e) {
                    for (var i = 0; i < removetr.length; i++) {
                        var idtr = removetr[i];
                        idtr.remove();
                    }
                },
                error: function (e) {
                }
            });
        }
    });
    $("#idDeletePerfoemanceObli").click(function () {
        var ids = [];
        var removetr = [];
        $("table tbody").find('input[name="record"]').each(function () {
            if ($(this).is(":checked") && $(this).closest("tr").index() != 0) {
                var parenttr = $(this).parents("tr");
                var val = $(this).closest("tr").find('input[type="hidden"]').val();
                if (val.length != 0) {
                    ids.push(val);
                    removetr.push(parenttr);
                }
                else {
                    parenttr.remove();
                }
            }
        });
        if (ids.length > 0) {
            var data = JSON.stringify(ids)
            $.ajax({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                dataType: 'json',
                type: "POST",
                url: "/RevenueRecognition/DeleteRevenueObligation",
                data: data,
                success: function (e) {
                    for (var i = 0; i < removetr.length; i++) {
                        var idtr = removetr[i];
                        idtr.remove();
                    }
                },
                error: function (e) {
                }
            });
        }
    });
    //ends

});// end of document..