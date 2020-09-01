$(document).ready(function () {


    var panelBar = $(".contractPanel").kendoPanelBar({
        collapse: onCollapse
    });
    InitialLoad();

    function InitialLoad() {


        $("#drpCountry option").each(function () {
            if ($(this).text() == "United States") {
                $(this).attr('selected', 'selected');
            }
        });
        showHideComponent();

        $("#POPStart").kendoDatePicker();
        $("#POPEnd").kendoDatePicker();

        if ($("#hiddenCompanyPresidentName").val() || $("#hiddenRegionalManagerName").val()) {
            $("#companyPresident").html($("hiddenCompanyPresidentName").val());
            $("#regionalManager").html($("hiddenRegionalManagerName").val());
        }
        if ($("#hiddenDeputyRegionalManagerName").val()) {
            $("#deputyRegionalManager").html($("hiddenDeputyRegionalManagerName").val());
        }
        if ($("#hiddenBDRegionalManagerName").val()) {
            $("#bdRegionalManager").html($("hiddenBDRegionalManagerName").val());
        }
        if ($("#hiddenHSRegionalManagerName").val()) {
            $("#hsRegionalManager").html($("hiddenHSRegionalManagerName").val());
        }

        if ($("#hiddenCompanyName").val() || $("#hiddenRegionName").val() || $("#hiddenOfficeName").val()) {
            $("#companyName").html($("hiddenCompanyName").val());
            $("#regionName").html($("hiddenRegionName").val());
            $("#officeName").html($("hiddenOfficeName").val());
        }

        //reload dropdown of awarding agency and funding agency contacts if page refresh..
        if ($("#hiddenAwardingAgencyOffice").val()) {
            var label = $("#AwardingAgencyOffice").val();
            var value = $("#hiddenAwardingAgencyOffice").val();
            getAllContactByCustomer("Awarding", label, value);

        }
        if ($("#hiddenFundingAgencyOffice").val()) {
            var label = $("#FundingAgencyOffice").val();
            var value = $("#hiddenFundingAgencyOffice").val();
            getAllContactByCustomer("Funding", label, value);
        }
    }

    var onCollapse = function (e) {
        // detach collapse event handler via unbind()
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };

    if ($("#ORGID").val()) {
        loadRegionAndOfficeInitiallyByOrgId();
    }

    $("#ORGID").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('Contract', 'GetOrganizationData', $('#ORGID').val(),
                function (data) {
                    if (!data.data.length) {
                        //                        var temp = $("#ORGID").val();
                        //                        var result = [{ label: "No results for :" + temp, value: response.term }];
                        var result = { value: "", label: "No results found" };
                        $("#ORGID").val('');
                        $("#hiddenORGID").val('');

                        $("#companyPresident").html("");
                        $("#regionalManager").html("");
                        $("#deputyRegionalManager").html("");
                        $("#hsRegionalManager").html("");
                        $("#bdRegionalManager").html("");
                        $("#companyName").html("");
                        $("#regionName").html("");
                        $("#officeName").html("");

                        $("#hiddenCompanyPresident").val("");
                        $("#hiddenRegionalManagerName").val("");
                        $("#hiddenDeputyRegionalManagerName").val("");
                        $("#hiddenHSRegionalManagerName").val("");
                        $("#hiddenBDRegionalManagerName").val("");
                        $("#hiddenCompanyPresidentGuid").val("");
                        $("#hiddenRegionalManagerGuid").val("");
                        $("#hiddenCompanyName").val("");
                        $("#hiddenRegionName").val("");
                        $("#hiddenOfficeName").val("");

                        $("#CompanyPresidentBlock").hide("slow");
                        $("#DeputyRegionalManagerBlock").hide("slow");
                        $("#RegionalManagerBlock").hide("slow");
                        $("#HealthAndSafetyRegionalManagerBlock").hide("slow");
                        $("#BusinessDevelopmentRegionalManagerBlock").hide("slow");
                        $("#CompanyNameBlock").hide("slow");
                        $("#RegionNameBlock").hide("slow");
                        $("#OfficeNameBlock").hide("slow");
                        $("#OperationManagerBlock").hide("slow");

                        response(result);
                    }
                    else {
                        $("#hiddenORGID").val('');
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#ORGID").val(ui.item.label);
            $("#hiddenORGID").val(ui.item.value);  // actual org id stores here and later post to server..
            var value = ui.item.label.split('-')[0].split('.');
            var companyCode = value[0];
            var regionCode = value[1];
            var officeCode = value[2].trim();
            var token = getCookieValue('X-CSRF-TOKEN');
            var reqValToken = getCookieValue('RequestVerificationToken');
            var data = {};
            data.companyCode = companyCode;
            data.regionCode = regionCode;
            data.officeCode = officeCode;

            $.ajax({
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': token,
                    'RequestVerificationToken': reqValToken
                },
                type: "POST",
                url: '/Contract/GetCompanyRegionOfficeNameByCode',
                data: JSON.stringify(data),
                dataType: "json",
                async: true,
                cache: false,
                success: function (response) {
                    $("#CompanyPresidentBlock").show("slow");
                    $("#RegionalManagerBlock").show("slow");
                    $("#DeputyRegionalManagerBlock").show("slow");
                    $("#BusinessDevelopmentRegionalManagerBlock").show("slow");
                    $("#HealthAndSafetyRegionalManagerBlock").show("slow");
                    $("#CompanyNameBlock").show("slow");
                    $("#RegionNameBlock").show("slow");
                    $("#OfficeNameBlock").show("slow");
                    $("#OperationManagerBlock").show("slow");

                    $("#companyPresident").html(response.data.companyPresident.displayName + ' (' + response.data.companyPresident.jobTitle + ')');

                    $("#companyName").html(response.data.companyName);
                    $("#regionName").html(response.data.regionName);
                    $("#officeName").html(response.data.officeName);

                    $("#hiddenCompanyPresident").val(response.data.companyPresident.displayName);
                    $("#hiddenCompanyPresidentGuid").val(response.data.companyPresident.userGuid);

                    if (response.data.regionManager !== null) {
                        $("#regionalManager").html(response.data.regionManager.displayName + (response.data.regionManager.jobTitle == null ? '' : '(' + response.data.regionManager.jobTitle + ')'));
                        $("#hiddenRegionalManagerName").val(response.data.regionManager.displayName);
                        $("#hiddenRegionalManagerGuid").val(response.data.regionManager.userGuid);
                    }
                    else {
                        $("#regionalManager").html('');
                        $("#hiddenRegionalManagerName").val('');
                        $("#hiddenRegionalManagerGuid").val('');
                    }
                    if (response.data.deputyRegionalManager !== null) {
                        $("#deputyRegionalManager").html(response.data.deputyRegionalManager.displayName + (response.data.deputyRegionalManager.jobTitle == null ? '' : '(' + response.data.deputyRegionalManager.jobTitle + ')'));
                        $("#hiddenDeputyRegionalManagerName").val(response.data.deputyRegionalManager.displayName);
                        $("#hiddenDeputyRegionalManagerGuid").val(response.data.deputyRegionalManager.userGuid);
                    }
                    else {
                        $("#deputyRegionalManager").html('');
                        $("#hiddenDeputyRegionalManagerName").val('');
                        $("#hiddenDeputyRegionalManagerGuid").val('');
                    }
                    if (response.data.hsRegionalManager !== null) {
                        $("#hsRegionalManager").html(response.data.hsRegionalManager.displayName + (response.data.hsRegionalManager.jobTitle == null ? '' : '(' + response.data.hsRegionalManager.jobTitle + ')'));
                        $("#hiddenHSRegionalManagerName").val(response.data.hsRegionalManager.displayName);
                        $("#hiddenHSRegionalManagerGuid").val(response.data.hsRegionalManager.userGuid);
                    }
                    else {
                        $("#hsRegionalManager").html('');
                        $("#hiddenHSRegionalManagerName").val('');
                        $("#hiddenHSRegionalManagerGuid").val('');
                    }

                    if (response.data.bdRegionalManager !== null) {
                        $("#bdRegionalManager").html(response.data.bdRegionalManager.displayName + (response.data.bdRegionalManager.jobTitle == null ? '' : '(' + response.data.bdRegionalManager.jobTitle + ')'));
                        $("#hiddendeputyRegionalManagerName").val(response.data.bdRegionalManager.displayName);
                        $("#hiddendeputyRegionalManagerGuid").val(response.data.bdRegionalManager.userGuid);
                    }
                    else {
                        $("#bdRegionalManager").html('');
                        $("#hiddendeputyRegionalManagerName").val('');
                        $("#hiddendeputyRegionalManagerGuid").val('');
                    }
                    if (response.data.operationManager != null) {
                        var displayName = response.data.operationManager.displayName == null ? "Not Entered" : response.data.operationManager.displayName;
                        var jobTitle = response.data.operationManager.jobTitle == null ? "" : ' (' + response.data.operationManager.jobTitle + ')';
                        $("#OperationManagerName").html(displayName + jobTitle);
                        $("#hiddenOperationManagerName").val(response.data.operationManager.displayName);
                        $("#hiddenOperationManagerGuid").val(response.data.operationManager.userGuid);
                    }
                    else {
                        $("#OperationManagerName").html("Not Entered");
                        $("#hiddenOperationManagerName").val('');
                        $("#hiddenOperationManagerName").val('');
                    }
                    $("#hiddenCompanyName").val(response.data.companyName);
                    $("#hiddenRegionName").val(response.data.regionName);
                    $("#hiddenOfficeName").val(response.data.officeName);
                }
            });
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#ORGID").val(ui.item.label);
        }
    });

    function loadRegionAndOfficeInitiallyByOrgId() {
        var orgVal = $("#ORGID").val();
        var orgId = $("#hiddenORGID").val();  // actual org id stores here and later post to server..
        var value = orgVal.split('-')[0].split('.');
        var companyCode = value[0];
        var regionCode = value[1];
        var officeCode = value[2].trim();

        var data = {};
        data.companyCode = companyCode;
        data.regionCode = regionCode;
        data.officeCode = officeCode;
        var token = getCookieValue('X-CSRF-TOKEN');
        var reqValToken = getCookieValue('RequestVerificationToken');


        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': reqValToken
            },
            type: "POST",
            url: '/Contract/GetCompanyRegionOfficeNameByCode',
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (response) {
                $("#CompanyPresidentBlock").show("slow");
                $("#RegionalManagerBlock").show("slow");
                $("#DeputyRegionalManagerBlock").show("slow");
                $("#HealthAndSafetyRegionalManagerBlock").show("slow");
                $("#BusinessDevelopmentRegionalManagerBlock").show("slow");
                $("#CompanyNameBlock").show("slow");
                $("#RegionNameBlock").show("slow");
                $("#OfficeNameBlock").show("slow");
                $("#OperationManagerBlock").show("slow");

                $("#companyPresident").html(response.data.companyPresident.displayName + ' (' + response.data.companyPresident.jobTitle + ')');

                $("#companyName").html(response.data.companyName);
                $("#regionName").html(response.data.regionName);
                $("#officeName").html(response.data.officeName);

                $("#hiddenCompanyPresident").val(response.data.companyPresident.displayName);
                $("#hiddenCompanyPresidentGuid").val(response.data.companyPresident.userGuid);

                if (response.data.regionManager !== null) {
                    $("#regionalManager").html(response.data.regionManager.displayName + (response.data.regionManager.jobTitle == null ? '' : '(' + response.data.regionManager.jobTitle + ')'));
                    $("#hiddenRegionalManagerName").val(response.data.regionManager.displayName);
                    $("#hiddenRegionalManagerGuid").val(response.data.regionManager.userGuid);
                }
                if (response.data.deputyRegionalManager !== null) {
                    $("#deputyRegionalManager").html(response.data.deputyRegionalManager.displayName + (response.data.deputyRegionalManager.jobTitle == null ? '' : '(' + response.data.deputyRegionalManager.jobTitle + ')'));
                    $("#hiddenDeputyRegionalManagerName").val(response.data.deputyRegionalManager.displayName);
                    $("#hiddenDeputyRegionalManagerGuid").val(response.data.deputyRegionalManager.userGuid);
                }
                if (response.data.hsRegionalManager !== null) {
                    $("#hsRegionalManager").html(response.data.hsRegionalManager.displayName + (response.data.hsRegionalManager.jobTitle == null ? '' : '(' + response.data.hsRegionalManager.jobTitle + ')'));
                    $("#hiddenHSRegionalManagerName").val(response.data.hsRegionalManager.displayName);
                    $("#hiddenHSRegionalManagerGuid").val(response.data.hsRegionalManager.userGuid);
                }

                if (response.data.bdRegionalManager !== null) {
                    $("#bdRegionalManager").html(response.data.bdRegionalManager.displayName + (response.data.bdRegionalManager.jobTitle == null ? '' : '(' + response.data.bdRegionalManager.jobTitle + ')'));
                    $("#hiddendeputyRegionalManagerName").val(response.data.bdRegionalManager.displayName);
                    $("#hiddendeputyRegionalManagerGuid").val(response.data.bdRegionalManager.userGuid);
                }

                $("#hiddenCompanyName").val(response.data.companyName);
                $("#hiddenRegionName").val(response.data.regionName);
                $("#hiddenOfficeName").val(response.data.officeName);
            }
        });
    }

    $("#ProjectManager").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#ProjectManager').val(),
                function (data) {
                    if (!data.data.length) {
                        //                        var temp = $("#ProjectManager").val();
                        //                        var result = [{ label: "No results for :" + temp, value: response.term }];
                        var result = { value: "", label: "No results found" };
                        $("#ProjectManager").val('');
                        $("#hiddenProjectManager").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#ProjectManager").val(ui.item.label);
            $("#hiddenProjectManager").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#ProjectManager").val(ui.item.label);
        }
    });

    $("#ProjectControl").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#ProjectControl').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#ProjectControl").val('');
                        $("#hiddenProjectControl").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#ProjectControl").val(ui.item.label);
            $("#hiddenProjectControl").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#ProjectControl").val(ui.item.label);
        }
    });

    $("#AccountingRepresentative").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#AccountingRepresentative').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#AccountingRepresentative").val('');
                        $("#hiddenAccountingRepresentative").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#AccountingRepresentative").val(ui.item.label);
            $("#hiddenAccountingRepresentative").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#AccountingRepresentative").val(ui.item.label);
        }
    });

    $("#ContractRepresentative").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#ContractRepresentative').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#ContractRepresentative").val('');
                        $("#hiddenContractRepresentative").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#ContractRepresentative").val(ui.item.label);
            $("#hiddenContractRepresentative").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#ContractRepresentative").val(ui.item.label);
        }
    });

    //auto complete for sub contract administrative
    $("#SubContractAdministrative").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#SubContractAdministrative').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#SubContractAdministrative").val('');
                        $("#hiddenSubContractAdministrative").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#SubContractAdministrative").val(ui.item.label);
            $("#hiddenSubContractAdministrative").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#SubContractAdministrative").val(ui.item.label);
        }
    });

    //auto complete for purchasing representative
    $("#PurchasingRepresentative").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#PurchasingRepresentative').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#PurchasingRepresentative").val('');
                        $("#hiddenPurchasingRepresentative").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#PurchasingRepresentative").val(ui.item.label);
            $("#hiddenPurchasingRepresentative").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#PurchasingRepresentative").val(ui.item.label);
        }
    });

    //auto complete for human resource representative
    $("#HumanResourceRepresentative").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#HumanResourceRepresentative').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#HumanResourceRepresentative").val('');
                        $("#hiddenHumanResourceRepresentative").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#HumanResourceRepresentative").val(ui.item.label);
            $("#hiddenHumanResourceRepresentative").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#HumanResourceRepresentative").val(ui.item.label);
        }
    });

    //auto complete for quality representative
    $("#QualityRepresentative").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#QualityRepresentative').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#QualityRepresentative").val('');
                        $("#hiddenQualityRepresentative").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#QualityRepresentative").val(ui.item.label);
            $("#hiddenQualityRepresentative").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#QualityRepresentative").val(ui.item.label);
        }
    });

    //auto complete for safety officer
    $("#SafetyOfficer").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('iam/user', 'GetUsersData', $('#SafetyOfficer').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#SafetyOfficer").val('');
                        $("#hiddenSafetyOfficer").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#SafetyOfficer").val(ui.item.label);
            $("#hiddenSafetyOfficer").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#SafetyOfficer").val(ui.item.label);
        }
    });

    $("#NAICSCode").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('Contract', 'GetNAICSCodeData', $('#NAICSCode').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#NAICSCode").val('');
                        $("#hiddenNAICSCode").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#NAICSCode").val(ui.item.label);
            $("#hiddenNAICSCode").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#NAICSCode").val(ui.item.label);
        }
    });

    $("#PSCCode").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('Contract', 'GetPSCCodeData', $('#PSCCode').val(),
                function (data) {
                    if (!data.data.length) {
                        var result = { value: "", label: "No results found" };
                        $("#PCSCode").val('');
                        $("#hiddenPCSCode").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#PSCCode").val(ui.item.label);
            $("#hiddenPSCCode").val(ui.item.value);  // actual  id stores here and later post to server..
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#PSCCode").val(ui.item.label);
        }
    });

    $("#AwardingAgencyOffice").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('Admin/Customer', 'GetOfficeData', $('#AwardingAgencyOffice').val(),
                function (data) {
                    if (!data.data.length) {
                        reSetAwardingOfficeInformation();
                        showHideCopyToFundingOfficeCheckBox();
                        var result = { value: "", label: "No results found" };
                        $("#AwardingAgencyOffice").val('');
                        $("#hiddenAwardingAgencyOffice").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#AwardingAgencyOffice").val(ui.item.label);
            getAllContactByCustomer("Awarding", ui.item.label, ui.item.value);
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#AwardingAgencyOffice").val(ui.item.label);
        }
    });

    $("#FundingAgencyOffice").autocomplete({
        minLength: 2,
        source: function (request, response) {
            BindSearch('Admin/Customer', 'GetOfficeData', $('#FundingAgencyOffice').val(),
                function (data) {
                    if (!data.data.length) {
                        reSetFundingOfficeInformation();
                        var result = { value: "", label: "No results found" };
                        $("#FundingAgencyOffice").val('');
                        $("#hiddenFundingAgencyOffice").val('');
                        response(result);
                    }
                    else {
                        response(data.data);
                    }
                });
        },
        select: function (event, ui) {
            if (ui.item.value == "No results found") {
                return false;
            }
            $("#FundingAgencyOffice").val(ui.item.label);
            getAllContactByCustomer("Funding", ui.item.label, ui.item.value);
            return false;
        },
        focus: function (event, ui) {
            event.preventDefault();
            $("#FundingAgencyOffice").val(ui.item.label);
        }
    });

    $(".AddCustomer").on('click',
        function (evt) {
            var id = $(this).attr('id');
            if (id == "AddNewCustomerFunding") {
                if ($("#copyToFundingOffice").is(":checked")) {
                    return false;
                }
            }
            switch (id) {
                default:
                    var data = {}
                    data.url = "/Admin/Customer/Add/";
                    data.submitURL = "/Admin/Customer/Add/";
                    var options = {
                        title: 'Add New Customer',
                        events: [
                            {
                                text: "Save",
                                primary: true,
                                action: function (e, values) {
                                    switch (id) {
                                        case "AddNewCustomerOffice":
                                            $("#AwardingAgencyOffice").val(values.customer.customerName);
                                            $("#hiddenAwardingAgencyOffice").val(values.customer.customerGuid);

                                            $("#drpAwardingOffice_ContractRepresentative").val(null);
                                            $('#drpAwardingOffice_ContractRepresentative').empty();

                                            $("#drpAwardingOffice_ContractTechnicalRepresent").val(null);
                                            $("#drpAwardingOffice_ContractTechnicalRepresent").empty();

                                            break;
                                        case "AddNewCustomerFunding":
                                            $("#FundingAgencyOffice").val(values.customer.customerName);
                                            $("#hiddenFundingAgencyOffice").val(values.customer.customerGuid);

                                            $("#drpFundingOffice_ContractRepresentative").val(null);
                                            $("#drpFundingOffice_ContractRepresentative").empty();

                                            $("#drpFundingOffice_ContractTechnicalRepresent").val(null);
                                            $("#drpFundingOffice_ContractTechnicalRepresent").empty();

                                            break;
                                    }
                                }
                            },
                            {
                                text: "Cancel",
                                action: function (e) {
                                }
                            }
                        ]
                    };
                    Dialog.openDialogSubmit(data, options);
                    break;
            }
        });

    $(".AddCustomerContact").on('click',
        function (evt) {
            var CustomerGuid = null;
            var id = $(this).attr('id');
            var ContactType = null;
            switch (id) {
                case "AddNewCustomerContractRepresentative":
                    ContactType = "Contract Representative"
                    CustomerGuid = $("#hiddenAwardingAgencyOffice").val();
                    break;
                case "AddNewCustomerContractTechnicalRepresent":
                    ContactType = "Technical Contract Representative"
                    CustomerGuid = $("#hiddenAwardingAgencyOffice").val();
                    break;
                case "AddNewFundingContractRepresent":
                    if ($("#copyToFundingOffice").is(":checked")) {
                        return false;
                    } else {
                        ContactType = "Contract Representative"
                        CustomerGuid = $("#hiddenFundingAgencyOffice").val();
                    }
                    break;
                case "AddNewFundingTechnicalRepresent":
                    if ($("#copyToFundingOffice").is(":checked")) {
                        return false;
                    } else {
                        ContactType = "Technical Contract Representative"
                        CustomerGuid = $("#hiddenFundingAgencyOffice").val();
                    }
                    break;
            }
            if (CustomerGuid == '' || CustomerGuid == '00000000-0000-0000-0000-000000000000') {
                Dialog.alert("Please Select Customer First !!");
                return false;
            }
            var data = {}
            data.url = "/Admin/Customercontact/Add?CustomerGuid=" + CustomerGuid + '&ContactType=' + ContactType;
            data.submitURL = "/Admin/Customercontact/Add/";
            var options = {
                title: 'Add New Contact',
                events: [
                    {
                        text: "Save",
                        primary: true,
                        action: function (e, values) {
                            switch (id) {
                                case "AddNewCustomerContractRepresentative":
                                    var options = $('#drpAwardingOffice_ContractRepresentative');
                                    var value = values.customerContact.contactguid;
                                    var text = values.customerContact.fullName + ' - ' + values.customerContact.customerPhoneNumber;
                                    options.append($('<option selected/>').val(value).text(text));

                                    var notSelectedValue = $('<option/>').val(value).text(text);
                                    $('#drpAwardingOffice_ContractTechnicalRepresent').append(notSelectedValue);

                                    showHideCopyToFundingOfficeCheckBox();
                                    break;

                                case "AddNewCustomerContractTechnicalRepresent":
                                    var options = $('#drpAwardingOffice_ContractTechnicalRepresent');
                                    var value = values.customerContact.contactguid;
                                    var text = values.customerContact.fullName + ' - ' + values.customerContact.customerPhoneNumber;
                                    options.append($('<option selected/>').val(value).text(text));

                                    var notSelectedValue = $('<option/>').val(value).text(text);
                                    $('#drpAwardingOffice_ContractRepresentative').append(notSelectedValue);
                                    break;

                                case "AddNewFundingContractRepresent":
                                    var options = $('#drpFundingOffice_ContractRepresentative');
                                    var value = values.customerContact.contactguid;
                                    var text = values.customerContact.fullName + ' - ' + values.customerContact.customerPhoneNumber;
                                    options.append($('<option selected/>').val(value).text(text));

                                    var notSelectedValue = $('<option/>').val(value).text(text);
                                    $('#drpFundingOffice_ContractTechnicalRepresent').append(notSelectedValue);
                                    break;

                                case "AddNewFundingTechnicalRepresent":
                                    var options = $('#drpFundingOffice_ContractTechnicalRepresent');
                                    var value = values.customerContact.contactguid;
                                    var text = values.customerContact.fullName + ' - ' + values.customerContact.customerPhoneNumber;
                                    options.append($('<option selected/>').val(value).text(text));

                                    var notSelectedValue = $('<option/>').val(value).text(text);
                                    $('#drpFundingOffice_ContractRepresentative').append(notSelectedValue);
                                    break;
                            }
                        }
                    },
                    {
                        text: "Cancel",
                        action: function (e) {
                        }
                    }
                ]
            };
            Dialog.openDialogSubmit(data, options);
        });

    function getAllContactByCustomer(officeTypeName, label, value) {
        $.ajax({
            type: "GET",
            url: '/Contract/GetAllContactByCustomer?customerId=' + value,
            dataType: "json",
            async: true,
            cache: false,
            success: function (data) {
                $("#" + officeTypeName + "AgencyOffice").val(label);
                $("#hidden" + officeTypeName + "AgencyOffice").val(value);  // actual  id stores here and later post to server..

                $("#drp" + officeTypeName + "Office_ContractRepresentative").val('');   // reset drp control in every new selection..
                $("#drp" + officeTypeName + "Office_ContractRepresentative").html('');

                $("#drp" + officeTypeName + "Office_ContractTechnicalRepresent").val('');   // reset drp control in every new selection..
                $("#drp" + officeTypeName + "Office_ContractTechnicalRepresent").html('');
                var obArrcontractRepresentative = [];
                var obArrTechnicalContractRepresentative = [];

                //fill drp control..
                if (data.contractRepresentative.length > 0) {
                    $('#drp' + officeTypeName + 'Office_ContractRepresentative').html("");
                    var markup = "<option selected='selected' value>--Select--</option>";
                    for (var x = 0; x < data.contractRepresentative.length; x++) {
                        var ob = {};
                        ob.id = data.contractRepresentative[x].keys;
                        ob.text = data.contractRepresentative[x].values;
                        obArrcontractRepresentative.push(ob);

                        for (var x = 0; x < data.contractRepresentative.length; x++) {
                            markup += "<option value=" + data.contractRepresentative[x].keys + ">" + data.contractRepresentative[x].values + "</option>";
                        }

                        $('#drp' + officeTypeName + 'Office_ContractRepresentative').html(markup).show();
                        $('#drp' + officeTypeName + 'Office_ContractRepresentative').show();


                        if ($("#hidden" + officeTypeName + "Office_ContractRepresentativeHiddenGuid").val()) {
                            $('#drp' + officeTypeName + 'Office_ContractRepresentative').val(
                                $("#hidden" + officeTypeName + "Office_ContractRepresentativeHiddenGuid").val());   // if reload then preserve the selected values..
                        } else {
                            $('#drp' + officeTypeName + 'Office_ContractRepresentative').get(0).selectedIndex = 0;
                        }
                    }
                }
                else {
                    var markup = "<option selected='selected' value>Representative is not assigned (Please Add New)</option>";
                    $('#drp' + officeTypeName + 'Office_ContractRepresentative').html(markup).show();
                    $('#drp' + officeTypeName + 'Office_ContractRepresentative').show();
                }
                // fill another drp control..
                if (data.technicalContractRepresentative.length > 0) {
                    $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').html("");
                    var markup = "<option selected='selected' value>--Select--</option>";
                    for (var x = 0; x < data.technicalContractRepresentative.length; x++) {
                        var ob = {};
                        ob.id = data.technicalContractRepresentative[x].keys;
                        ob.text = data.technicalContractRepresentative[x].values;

                        obArrTechnicalContractRepresentative.push(ob);

                        for (var x = 0; x < data.technicalContractRepresentative.length; x++) {
                            markup += "<option value=" + data.technicalContractRepresentative[x].keys + ">" + data.technicalContractRepresentative[x].values + "</option>";
                        }
                        $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').html(markup).show();
                        $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').show();

                        if ($("#hidden" + officeTypeName + "Office_ContractTechnicalRepresentHiddenGuid").val()) {
                            $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').val(
                                $("#hidden" + officeTypeName + "Office_ContractTechnicalRepresentHiddenGuid").val());
                        } else {
                            $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').get(0).selectedIndex = 0;
                        }
                    }
                }
                else {
                    var markup = "<option selected='selected' value>Representative is not assigned (Please Add New)</option>";
                    $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').html(markup).show();
                    $('#drp' + officeTypeName + 'Office_ContractTechnicalRepresent').show();
                }
                showHideCopyToFundingOfficeCheckBox();
            }
        });
    }
    function BindSearch(controller, actionMethod, data, callback) {
        var token = getCookieValue('X-CSRF-TOKEN');
        var reqValToken = getCookieValue('RequestVerificationToken');
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': reqValToken
            },
            type: "POST",
            url: '/' + controller + '/' + actionMethod,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (response) {
                callback(response);
            }
        });
    }
    //    Place of performance section

    var selectedStatesIds = $("#hiddenPlaceOfPerformanceSelectedIds").val() || '';
    var dataSourceSelectedStates = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/Contract/GetSelectedStatesByStateIds?statesIds=" + selectedStatesIds,
                dataType: "json"
            }
        }
    });

    var states = $('#hiddenPlaceOfPerformanceSelectedIds').val().split(',');
    var arrStates = [];
    states.map(function (v, i) {
        arrStates.push({
            field: "value",
            operator: "neq",
            value: v
        });
    });

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/Contract/GetStatesByCountryId?countryId=",
                dataType: "json"
            }
        },
        filter: arrStates
    });
    $("#OptionnalLoadPlaceOfPerformance").kendoListBox({
        dataSource: dataSource,
        dataTextField: "text",
        dataValueField: "value",
        autoBind: false,
        selectable: "multiple",
        connectWith: "PlaceOfPerformance",
        toolbar: {
            tools: ["transferTo", "transferFrom", "transferAllTo", "transferAllFrom"]
        }
    });
    dataSource.read();
    $("#PlaceOfPerformance").kendoListBox({
        dataSource: dataSourceSelectedStates,
        dataTextField: "text",
        dataValueField: "value",
        autoBind: false,
        selectable: "multiple",
        connectWith: "OptionnalLoadPlaceOfPerformance",
    });
    dataSourceSelectedStates.read();

    $(document).on("change",
        "#drpCountry",
        function (event, wasTriggered) {
            var countryId = $(this).val();
            var optionalList = $("#OptionnalLoadPlaceOfPerformance").data("kendoListBox");

            if (!wasTriggered) {
                //reset 
                var selectedList = $("#PlaceOfPerformance").data("kendoListBox");
                selectedList.dataSource.options.transport.read.url = "/Contract/GetStatesByCountryId?countryId=";
                $("#PlaceOfPerformance").data('kendoListBox').dataSource.read();
                $("#PlaceOfPerformance").data('kendoListBox').refresh();

                if (countryId) {
                    optionalList.dataSource.options.transport.read.url = "/Contract/GetStatesByCountryId?countryId=" + countryId;
                    $("#OptionnalLoadPlaceOfPerformance").data('kendoListBox').dataSource.read();
                    $("#OptionnalLoadPlaceOfPerformance").data('kendoListBox').refresh();
                }
            } else {
                //nothing..
            }
            if (countryId) {
                optionalList.dataSource.options.transport.read.url = "/Contract/GetStatesByCountryId?countryId=" + countryId;
                $("#OptionnalLoadPlaceOfPerformance").data('kendoListBox').dataSource.read();
                $("#OptionnalLoadPlaceOfPerformance").data('kendoListBox').refresh();
            }
        });

    $("#drpCountry").trigger("change", true); // reload auto when page refresh..
    //    end of palce of performance..


    $("#copyToFundingOffice").click(function () {
        if ($(this).is(":checked")) {
            $("#FundingAgencyOffice").val(null);

            $("#hiddenFundingAgencyOffice").val($("#hiddenAwardingAgencyOffice").val());

            $("#drpFundingOffice_ContractRepresentative").html(
                $("#drpAwardingOffice_ContractRepresentative").html());
            $("#drpFundingOffice_ContractRepresentative")
                .val(null);

            $("#hiddenFundingOffice_ContractRepresentativeHiddenGuid").val(null);

            $("#drpFundingOffice_ContractTechnicalRepresent").html(
                $("#drpAwardingOffice_ContractTechnicalRepresent").html());
            $("#drpFundingOffice_ContractTechnicalRepresent")
                .val(null);

            $("#hiddenFundingOffice_ContractTechnicalRepresentHiddenGuid").val(null);

            $("#FundingAgencyOffice").attr("disabled", true);
            $("#drpFundingOffice_ContractRepresentative").attr("disabled", true);
            $("#drpFundingOffice_ContractTechnicalRepresent").attr("disabled", true);
        } else {
            reSetFundingOfficeInformation();
        }
    });

    function showHideComponent() {
        $("#SubContractBlock").hide("fast");

        $("#CompanyPresidentBlock").hide();
        $("#RegionalManagerBlock").hide();
        $("#DeputyRegionalManagerBlock").hide();
        $("#HealthAndSafetyRegionalManagerBlock").hide();
        $("#BusinessDevelopmentRegionalManagerBlock").hide();

        $("#CompanyNameBlock").hide();
        $("#RegionNameBlock").hide();
        $("#OfficeNameBlock").hide();
        $("#OperationManagerBlock").hide();

        var hasCompanyPresident = $("#hiddenCompanyPresidentName").val();
        var hasRegionalManager = $("#hiddenRegionalManagerName").val();
        var hasDeputyRegionalManager = $("#hiddenDeputyRegionalManagerName").val();
        var hasHSRegionalManager = $("#hiddenHSRegionalManagerName").val();
        var hasBDRegionalManager = $("#hiddenBDRegionalManagerName").val();
        var hasOperationManager = $("#OperationManagerName").val();



        var hasCompanyName = $("#hiddenCompanyName").val();
        var hasRegionName = $("#hiddenRegionName").val();
        var hasOfficeName = $("#hiddenOfficeName").val();

        if (hasCompanyPresident.length && hasRegionalManager.length) {
            $("#CompanyPresidentBlock").show();
            $("#RegionalManagerBlock").show();
        }
        if (hasDeputyRegionalManager.length) {
            $("#DeputyRegionalManagerBlock").show();
        }
        if (hasHSRegionalManager.length) {
            $("#HealthAndSafetyRegionalManagerBlock").show();
        }
        if (hasBDRegionalManager.length) {
            $("#BusinessDevelopmentRegionalManagerBlock").show();
        }
        if (hasOperationManager.length) {
            $("#OperationManagerBlock").show();
        }
        if (hasCompanyName.length && hasRegionName.length && hasOfficeName.length) {
            $("#CompanyNameBlock").show();
            $("#RegionNameBlock").show();
            $("#OfficeNameBlock").show();
        }

        //show hide IsIdiqContract
        function showHideIsIdiqContract(switchVal) {
            switch (switchVal) {
                case 'False':
                    $("#idBlueSkyAward").hide("slow");
                    $(".idiq-EnableShow").show("slow");
                    $("#BlueSkyAward_Amount").prop('required', false);  //validation
                    $("#BlueSkyAward_Amount").val('');
                    break;
                case 'True':
                    $("#idBlueSkyAward").show("slow");
                    $(".idiq-EnableShow").hide("slow");
                    //$("#BlueSkyAward_Amount").prop('required', true); //validation
                    break;
            }
        }

        $(".IsIdiqContract").change(function () {
            showHideIsIdiqContract($(this).val());
        });

        if ($('input[name="BasicContractInfo.IsIDIQContract"]:checked').val() == 'True') {
            showHideIsIdiqContract("True");
        } else {
            showHideIsIdiqContract("False");
        }
        //end of IsIdiqContract


        //show hide IsPrimeContract
        function showHideIsPrimeContract(switchVal) {
            switch (switchVal) {
                case 'False':
                    $("#SubContractBlock").show("slow");
                    //$("#SubContractNumber").prop('required', true);
                    $("#SubContractNumber").parent().addClass("form-required");
                    break;
                case 'True':
                    $("#SubContractBlock").hide("slow");
                    $("#SubContractBlock :input").val('');
                    //$("#SubContractNumber").prop('required', false);
                    $("#SubContractNumber").parent().removeClass("form-required");
                    break;
            }
        }

        $(".IsPrimeContract").change(function () {
            showHideIsPrimeContract($(this).val());
        });

        if ($('input[name="BasicContractInfo.IsPrimeContract"]:checked').val() == 'True') {
            showHideIsPrimeContract("True");
        } else {
            showHideIsPrimeContract("False");
        }
        //end of IsPrimeContract

        //$(document).on("change",
        //    "#drpAwardingOffice_ContractRepresentative",
        //    function (event) {
        //        if ($(this).val()) {
        //            showHideCopyToFundingOfficeCheckBox();
        //        }
        //    });

        $(document).on("change",
            "#AwardingAgencyOffice",
            function (event) {
                if ($(this).val()) {
                    showHideCopyToFundingOfficeCheckBox();
                }
            });
        //show hide QualityLevelRequirements
        function showHideQualityLevelRequirements(switchVal) {
            switch (switchVal) {
                case 'False':
                    $("#QualityLevelBlock").hide("slow");
                    $("#QualityLevelBlock :input").val('');
                    $("#QualityLevelBlock").removeClass("form-required").removeClass("form-error");
                    // $("#drpQualityLevel").prop('required', false);
                    break;
                case 'True':
                    $("#QualityLevelBlock").show("slow");
                    $("#QualityLevelBlock").addClass("form-required");
                    // $("#drpQualityLevel").prop('required', true);
                    break;
            }
        }

        $(".QualityLevelRequirements").change(function () {
            showHideQualityLevelRequirements($(this).val());
        });

        if ($('input[name="BasicContractInfo.QualityLevelRequirements"]:checked').val() == 'True') {
            showHideQualityLevelRequirements("True");
        } else {
            showHideQualityLevelRequirements("False");
        }
        //end of QualityLevelRequirements


        // Section Financial Information Pannel -- Starts


        //show hide selfperformance & SBA
        $("#setAside").change(function () {
            showHideSetAside($(this).val());
        });
        function showHideSetAside(switchVal) {
            switch (switchVal) {
                case '8(a)':
                    $("#ShowSBAfield").show("slow");
                    $("#selfPerformancePercent").addClass('form-required');
                    break;
                default:
                    $("#ShowSBAfield").hide("slow");
                    $("#ShowSBAfield :input").not(".SBA").val('');
                    $("#selfPerformancePercent").removeClass('form-required');
                    break;
            }
        }
        if ($('#setAside').val() == '8(a)') {
            showHideSetAside("8(a)");
        } else {
            showHideSetAside("default");
        }
        //end of setAside

        //show hide Overhead , G&A & fee..
        $("#contractType").change(function () {
            showHideContractType($(this).val());
        });
        function showHideContractType(switchVal) {
            switch (switchVal) {
                // When Cost Plus
                case 'CostPlusAwardFee':
                    $("#idCostPrice").show("slow");
                    $("#idFixedPrice").show("slow");


                    //validation..
                    $("#overHead").addClass('form-required');
                    $("#gaPercent").addClass('form-required');
                    $("#idFixedPrice").addClass('form-required');  // fee percent..

                    break;

                case 'CostPlusFixedFee':
                    $("#idCostPrice").show("slow");
                    $("#idFixedPrice").show("slow");


                    //validation..
                    $("#overHead").addClass('form-required');
                    $("#gaPercent").addClass('form-required');
                    $("#idFixedPrice").addClass('form-required');  // fee percent..

                    break;

                case 'CostPlusIncentiveFee':
                    $("#idCostPrice").show("slow");
                    $("#idFixedPrice").show("slow");


                    //validation..
                    $("#overHead").addClass('form-required');
                    $("#gaPercent").addClass('form-required');
                    $("#idFixedPrice").addClass('form-required');  // fee percent..

                    break;

                // When Fixed Price
                case 'FirmFixedPrice':
                    $("#idCostPrice").hide("slow");
                    $("#idFixedPrice").show("slow");

                    $("#idCostPrice :input").val('');

                    //validation..
                    $("#overHead").removeClass('form-required');
                    $("#gaPercent").removeClass('form-required');

                    $("#idFixedPrice").addClass('form-required');  // fee percent..

                    break;

                default:
                    $("#idCostPrice").hide("slow");
                    $("#idFixedPrice").hide("slow");

                    $("#idCostPrice :input").val('');
                    $("#idFixedPrice :input").val('');

                    //validation..
                    $("#overHead").removeClass('form-required');
                    $("#gaPercent").removeClass('form-required');
                    $("#idFixedPrice").removeClass('form-required');  // fee percent..
                    break;
            }
        }

        if (($('#contractType').val() == 'CostPlusAwardFee') || ($('#contractType').val() == 'CostPlusIncentiveFee') || ($('#contractType').val() == 'CostPlusFixedFee')) {
            showHideContractType("CostPlusAwardFee");
        } else if ($('#contractType').val() == 'FirmFixedPrice') {
            showHideContractType("FirmFixedPrice");
        } else {
            showHideContractType("default");
        }
        // end of  contractType ..
    }

    function reSetFundingOfficeInformation() {
        $("#FundingAgencyOffice").val('');
        $("#hiddenFundingAgencyOffice").val('');

        $("#drpFundingOffice_ContractRepresentative").empty();
        $("#hiddenFundingOffice_ContractRepresentativeHiddenGuid").val('');

        $("#drpFundingOffice_ContractTechnicalRepresent").empty();
        $("#hiddenFundingOffice_ContractTechnicalRepresentHiddenGuid").val('');

        $("#FundingAgencyOffice").attr("disabled", false);
        $("#drpFundingOffice_ContractRepresentative").attr("disabled", false);
        $("#drpFundingOffice_ContractTechnicalRepresent").attr("disabled", false);

    }
    function reSetAwardingOfficeInformation() {
        $("#AwardingAgencyOffice").val('');
        $("#hiddenAwardingAgencyOffice").val('');

        $("#drpAwardingOffice_ContractRepresentative").empty();
        $("#hiddenAwardingOffice_ContractRepresentativeHiddenGuid").val('');

        $("#drpAwardingOffice_ContractTechnicalRepresent").empty();
        $("#hiddenAwardingOffice_ContractTechnicalRepresentHiddenGuid").val('');
    }

    showHideCopyToFundingOfficeCheckBox(); // when page load for add new contract..
    function showHideCopyToFundingOfficeCheckBox() {
        $("#copyToFundingOffice").attr("disabled", true);
        if ($("#hiddenAwardingAgencyOffice").val() != null &&
            $("#hiddenAwardingAgencyOffice").val() != "" &&
            $("#hiddenAwardingAgencyOffice").val() != "00000000-0000-0000-0000-000000000000") {
            $("#copyToFundingOffice").attr("disabled", false);
        }
    }
    $('.keyUpAction').on('keyup', function () {
        var length = $(this).val().length;
        var emptyGuid = '00000000-0000-0000-0000-000000000000';
        if (length === 0) {
            $(this).siblings(".hiddenValue").attr('value', emptyGuid)
        }
    });



    //Financial Information Pannel -- Ends
});  // end of document..

function afterDocumentReadyValidateAndSubmit(url, isValidationOnly) {
    $("#contractForm").submit(function (e) {

        var isSubmitableForm = true;
        $(".form-required").each(function () {
            var inputField = $(this).find("input:not(:hidden)");
            var val = $(inputField).val()

            if ($(inputField).attr("name") === 'BasicContractInfo.OrganizationName') // for auto complete 
            {
                val = $("#hiddenORGID").val();
                if (val === "" || val === undefined)
                    $("#ORGID").val('');
            }

            var selectField = $(this).find("select");
            var selectVal = $(selectField).val()

            $(this).removeClass('form-error');
            $("#PlaceOfPerformance").closest('.k-listbox').removeClass('form-error');
            $("#PlaceOfPerformance").closest('#example').find("span.list-box-error").text('')


            if ($("#PlaceOfPerformance").val().length === 0) {
                $("#PlaceOfPerformance").closest('.k-listbox').addClass('form-error');
                $("#PlaceOfPerformance").closest('#example').find("span.list-box-error").text("The Place of performance field is required")
                isSubmitableForm = false;
            }
            if (!selectVal || selectVal === undefined) {
                if (val === "" || val === undefined || val === '00000000-0000-0000-0000-000000000000') {
                    var customerInfoValidation = $(this).find(".copyAsAwardingValidation").text()
                    var label = $(this).find(".control-label,.k-selectable").text()
                    if (customerInfoValidation != "") {
                        if (!$("#copyToFundingOffice").is(":checked")) {
                            $(this).addClass('form-error');
                            $(this).find("span.text-danger").text("The " + label + " field is required")
                            isSubmitableForm = false;
                        }
                    }
                    else {
                        $(this).addClass('form-error');
                        $(this).find("span.text-danger").text("The " + label + " field is required")
                        isSubmitableForm = false;
                    }
                }
            }
        })

        if (!isSubmitableForm)
            return false;


        if (isValidationOnly) {
            return true;  // prevent form submit if Validation Only..
        }

        e.preventDefault();
        var contractform = $('body').find('form')
        let fileForm = new FormData();

        var contractFormData = $(contractform).serialize();
        var formData = contractFormData.split("&");
        for (var i in formData) {
            var keyVal = formData[i].split("=")
            if (keyVal.length > 1) {
                fileForm.append(keyVal[0], decodeURIComponent(keyVal[1]));
            }
        }

        var token = getCookieValue('X-CSRF-TOKEN');
        var reqValToken = getCookieValue('RequestVerificationToken');

        $.ajax({

            headers: {
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': reqValToken
            },
            type: 'post',
            contentType: false,
            processData: false,
            url: url,
            data: fileForm,
            beforeSend: function () {
                $('#loading').show();
            },
            success: function (response) {

                if (response.status === false || response.status === undefined) {

                    $('#loading').hide();
                    $('#loadingFileUpload').hide();

                    $(".validation-summary-errors ul").remove() //remove all existion errors first..
                    var list = $(".validation-summary-errors").append('<ul></ul>').find('ul');
                    if (response.errors[""]) {
                        response.errors[""]["errors"].forEach(function (item) {
                            $(list).append('<li> ' + item.errorMessage + '</li>');
                        })
                    }
                    return;
                }
                //preserve notification link from response..
                window.notificationLink = response.notificationLink;

                //finally saved the uploaded file in the server
                window.uploader.onSubmitFiles(response.resourceId, response.uploadPath);

                //// finally redirect to  notification page..
                //window.location.href = response.notificationLink;
            },
            done: function (data) {
                //$('#loading').hide();
            },
            error: function (data) {
                $('#loading').hide();
                $('#loadingFileUpload').hide();
                alert("Some error occurred ")
            }
        });
    })
}