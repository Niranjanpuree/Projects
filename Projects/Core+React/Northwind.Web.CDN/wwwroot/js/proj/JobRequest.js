$(document).ready(function () {
    var gridId = $("#jobRequestGrid");
    var clause = document.getElementsByClassName("clause");
    var farClauseID = clause[0].getAttribute("data-farClauseID");
    var isEditable = clause[0].getAttribute("data-isEditable");
    var panelBar = $(".contractPanel").kendoPanelBar({
        collapse: onCollapse
    });
    $(document).on("mouseover", ".tooltipdetail", function (e) {
        $(this).parent().siblings(".popover-detail").addClass("active");
    });

    $(document).on("mouseout", ".tooltipdetail", function (e) {
        $('.tooltipdetail').parent().siblings(".popover-detail").removeClass("active");
    });
    //disable save button by default
    $(':input[type="submit"]').prop('disabled', true);

    //$(':input[type="submit"]').prop('disabled', true);
    //$(":submit").attr("disabled", true);

    $(document).on('click', '#job-request-agreement', function () {
        var isValidGuid = farClauseID;
        if ($(this).is(":checked") && isValidGuid.toLocaleLowerCase() == "true" && isEditable.toLocaleLowerCase() == "true")
            $(':input[type="submit"]').prop('disabled', false);
        else
            $(':input[type="submit"]').prop('disabled', true);
    });

    var onCollapse = function (e) {
        // detach collapse event handler via unbind()
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };

    InitialLoad();
    function InitialLoad() {
        LoadGrids();
    }
    //Reload grid..
    var ReloadGrid = function () {
        $(gridId).data('kendoGrid').dataSource.read();
        $(gridId).data('kendoGrid').refresh();
    }
    //Generic ajax post for commands..
    var ajaxPost = function (url, data) {
        var token = getCookieValue('X-CSRF-TOKEN');
        var reqValToken = getCookieValue('RequestVerificationToken');
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': reqValToken
            },
            dataType: 'json',
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            success: function (values) {
                $.notify(values.message, values.status);
                ReloadGrid();
            },
            error: function (values) {
                $.notify(values.message, values.status);
            }
        });
    }

    function LoadGrids() {
        var searchValue = $("#SearchValue").val() || '';
        var filterBy = $("#ddl-jobStatus option:selected").val();
        var dataUrl = "/JobRequest/Get?SearchValue=" + searchValue + '&sortField=ProjectNumber&sortDirection=asc&additionalFilterValue=' + filterBy;
        $($(gridId)).kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        contentType: "application/json",
                        url: dataUrl,
                        dataType: "json"
                    }
                },
                batch: true,
                schema: {
                    data: "data",
                    total: "total",
                    model: {
                        id: "jobRequestGuid",
                        fields: {
                            jobRequestTitle: { type: "string" },
                            initiatedBy: { type: "string" },
                            jobRequestStatus: { type: "string" },
                            contractReview: { type: "string" },
                            JobRequestControlReview: { type: "string" },
                            JobRequestManagerReview: { type: "string" },
                            accountingReview: { type: "string" }
                            //isActiveStatus: { type: "string" }
                        }
                    }
                },
                serverPaging: true,
                pageSize: 10
            },
            height: 650,
            scrollable: true,
            sortable: true,
            resizable: true,
            dataBound: function () {
                new GridExtention().loadGridMenu($(gridId));
                var totalheader = $(".k-header-column-menu").length;
                $(".k-header-column-menu").each(function (i) {
                    if (i < parseInt(totalheader - 1)) {
                        $(this).remove();
                    }
                });
            },
            columnMenu: false,
            filterable: false,
            pageable: {
                input: true,
                numeric: true
            },
            columns: [
                { selectable: true, width: "35px", locked: false, lockable: true },
                { name: 'open', width: 30, click: open, template: "<div class='gridToolBar'></div>" },
                { field: "jobRequestTitle", title: "Job Request", width: 150 },
                { field: "initiatedBy", title: "Initiated By", width: 150 },
                { field: "jobRequestStatus", title: "Job Request Status", width: 250 },
                { field: "contractReview", title: "Contract Review", width: 150 },
                { field: "projectControlReview", title: "Control Review", width: 150 },
                { field: "projectManagerReview", title: "Manager Review", width: 150 },
                { field: "accountingReview", title: "Accounting Review", width: 150 },
                //{ field: "isActiveStatus", title: "Status", width: 150 }
            ],
            editable: "",
            progress: false,
            events: [
                {
                    title: "Detail",
                    event: "onClick",
                    icon: 'info',
                    onClick: function (e) {
                        window.location.href = "/JobRequest/Detail/" + e.item.options.data.contractGuid;
                    }
                },
                {
                    title: "Edit",
                    event: "onClick",
                    icon: 'pencil',
                    onClick: function (e) {
                        window.location.href = "/JobRequest/Edit/" + e.item.options.data.contractGuid;
                    }
                },
                {
                    title: "Delete",
                    event: "onDelete",
                    icon: 'delete',
                    onDelete: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.jobRequestGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  delete JobRequest?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/JobRequest/Delete', ids);
                                ReloadGrid();
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Disable",
                    event: "onDisable",
                    icon: 'cancel',
                    onDisable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.jobRequestGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  disable JobRequest?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/JobRequest/Disable', ids);
                                ReloadGrid();
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                },
                {
                    title: "Enable",
                    event: "onEnable",
                    icon: 'check',
                    onEnable: function (e) {
                        var data = {}
                        data.data = e.item.options.data;
                        var id = e.item.options.data.jobRequestGuid;
                        var ids = [];
                        ids.push(id);
                        Dialog.confirm({
                            text: "Are you sure you want to  enable JobRequest?",
                            title: "Confirm",
                            ok: function (e) {
                                ajaxPost('/JobRequest/Enable', ids);
                                ReloadGrid();
                            },
                            cancel: function (e) {
                            }
                        });
                    }
                }
            ]
        }).data("kendoGrid");

    } // end of grid..

    $("#ddl-jobStatus").on('change', function () {
        var filterBy = $("#ddl-jobStatus option:selected").val();
        LoadGrids();
    });

    $('#btnSearch').click(function (e) {
        var searchvalue = $("#SearchValue").val();
        window.location.href = "/JobRequest/Index?SearchValue=" + searchvalue;
    });

    $('#clearSearch').click(function () {
        window.location.href = "/JobRequest/Index";
    });

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
    $("#CompanySelected").kendoMultiSelect({
        autoClose: false
    }).data("kendoMultiSelect");

    //Work Breakdown Structure
    $(document).on("click", "#idAddContractWBS", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/WorkBreakdownStructure/Add?id=" + contractGuid;
        data.submitURL = "/WorkBreakdownStructure/Add";
        var options = {
            title: 'Add work breakdown structure',
            events: [
                {
                    text: "Save",
                    primary: true,
                    action: function (e, values) {
                        window.location.reload();
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                        $('html').removeClass('htmlClass');
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditContractWBS", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/WorkBreakdownStructure/Edit?id=" + contractGuid;
        data.submitURL = "/WorkBreakdownStructure/Edit";
        var options = {
            title: 'Edit work breakdown structure',
            events: [
                {
                    text: "Update",
                    primary: true,
                    requreValidation: true,
                    action: function (e, values) {
                        saveUpdatedGrid();
                    }
                },
                {
                    text: "Upload New File",
                    action: function (e) {
                        Dialog.confirm({
                            text: "Are you sure you want to add a new Work Breakdown Structure? This will permanently overwrite everything from the previous Work Breakdown Structure.",
                            title: "Upload New File Confirmation",
                            ok: function (e) {
                                var data = {}
                                var contractGuid = $("#ContractGuid").val();
                                data.url = "/WorkBreakdownStructure/Add?id=" + contractGuid;
                                data.submitURL = "/WorkBreakdownStructure/Add";
                                var options = {
                                    title: 'Add work breakdown structure',
                                    events: [
                                        {
                                            text: "Save",
                                            primary: true,
                                            action: function (e, values) {
                                                window.location.reload();
                                            }
                                        },
                                        {
                                            text: "Cancel",
                                            action: function (e) {
                                                $('html').removeClass('htmlClass');
                                            }
                                        }
                                    ]
                                };
                                Dialog.openDialogGridSubmit(data, options);
                            },
                            cancel: function () {
                                $('html').removeClass('htmlClass');
                            }
                        })
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                        $('html').removeClass('htmlClass');
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idViewWBSNonCSV", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/WorkBreakDownStructure/Detail?id=" + contractGuid;
        data.submitURL = "/WorkBreakDownStructure/Detail";
        var options = {
            title: 'Edit work breakdown structure',
            events: [
                {
                    text: "Upload New File",
                    action: function (e) {
                        Dialog.confirm({
                            text: "Are you sure you want to add a new Work Breakdown Structure? This will permanently overwrite everything from the previous Work Breakdown Structure.",
                            title: "Upload New File Confirmation",
                            ok: function (e) {
                                var data = {}
                                var contractGuid = $("#ContractGuid").val();
                                data.url = "/WorkBreakDownStructure/Add?id=" + contractGuid;
                                data.submitURL = "/WorkBreakDownStructure/Add";
                                var options = {
                                    title: 'Add work breakdown structure',
                                    events: [
                                        {
                                            text: "Save",
                                            primary: true,
                                            action: function (e, values) {
                                                window.location.reload();
                                            }
                                        },
                                        {
                                            text: "Cancel",
                                            action: function (e) {
                                            }
                                        }
                                    ]
                                };
                                Dialog.openDialogGridSubmit(data, options);
                            },
                            cancel: function () {
                            }
                        })
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });
    //end

    //Employee Billing Rates
    $(document).on("click", "#idAddEmployeeBillingRates", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/EmployeeBillingRates/Add?id=" + contractGuid;
        data.submitURL = "/EmployeeBillingRates/Add";
        var options = {
            title: 'Add employee billing rates',
            events: [
                {
                    text: "Save",
                    primary: true,
                    requreValidation: true,
                    action: function (e, values) {
                        window.location.reload();
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                        $('html').removeClass('htmlClass');
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditEmployeeBillingRates", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/EmployeeBillingRates/Edit?id=" + contractGuid;
        data.submitURL = "/EmployeeBillingRates/Edit";
        var options = {
            title: 'Edit employee billing rates',
            events: [
                {
                    text: "Update",
                    primary: true,
                    requreValidation: true,
                    action: function (e, values) {
                        saveUpdatedGrid();
                        window.location.reload();
                    }
                },
                {
                    text: "Upload New File",
                    action: function (e) {
                        Dialog.confirm({
                            text: "Are you sure you want to add a new Employee Billing Rate? This will permanently overwrite everything from the previous Employee Billing Rate.",
                            title: "Upload New File Confirmation",
                            ok: function (e) {
                                var data = {}
                                var contractGuid = $("#ContractGuid").val();
                                data.url = "/EmployeeBillingRates/Add?id=" + contractGuid;
                                data.submitURL = "/EmployeeBillingRates/Add";
                                var options = {
                                    title: 'Add employee billing rates',
                                    events: [
                                        {
                                            text: "Save",
                                            primary: true,
                                            action: function (e, values) {
                                                window.location.reload();
                                            }
                                        },
                                        {
                                            text: "Cancel",
                                            action: function (e) {
                                                $('html').removeClass('htmlClass');
                                            }
                                        }
                                    ]
                                };
                                Dialog.openDialogGridSubmit(data, options);
                            },
                            cancel: function () {
                                $('html').removeClass('htmlClass');
                            }
                        })
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                        $('html').removeClass('htmlClass');
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idViewEBRNonCSV", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/EmployeeBillingRates/Detail?id=" + contractGuid;
        data.submitURL = "/EmployeeBillingRates/Detail";
        var options = {
            title: 'Edit employee billing rates',
            events: [
                {
                    text: "Upload New File",
                    action: function (e) {
                        Dialog.confirm({
                            text: "Are you sure you want to add a new Employee Billing Rate? This will permanently overwrite everything from the previous Employee Billing Rate.",
                            title: "Upload New File Confirmation",
                            ok: function (e) {
                                var data = {}
                                var contractGuid = $("#ContractGuid").val();
                                data.url = "/EmployeeBillingRates/Add?id=" + contractGuid;
                                data.submitURL = "/EmployeeBillingRates/Add";
                                var options = {
                                    title: 'Add employee billing rates',
                                    events: [
                                        {
                                            text: "Save",
                                            primary: true,
                                            action: function (e, values) {
                                                window.location.reload();
                                            }
                                        },
                                        {
                                            text: "Cancel",
                                            action: function (e) {
                                            }
                                        }
                                    ]
                                };
                                Dialog.openDialogGridSubmit(data, options);
                            },
                            cancel: function () {
                            }
                        })
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });
    //end

    //Subcontractor Billing Rates
    $(document).on("click", "#idAddLaborCategoryRates", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/SubcontractorBillingRates/Add?id=" + contractGuid;
        data.submitURL = "/SubcontractorBillingRates/Add";
        var options = {
            title: 'Add subcontractor billing rates',
            events: [
                {
                    text: "Save",
                    primary: true,
                    requreValidation: true,
                    action: function (e, values) {
                        window.location.reload();
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                        $('html').removeClass('htmlClass');
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditLaborCategoryRates", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/SubcontractorBillingRates/Edit?id=" + contractGuid;
        data.submitURL = "/SubcontractorBillingRates/Edit";
        var options = {
            title: 'Edit subcontractor billing rates',
            events: [
                {
                    text: "Update",
                    primary: true,
                    requreValidation: true,
                    action: function (e, values) {
                        saveUpdatedGrid();
                        window.location.reload();
                    }
                },
                {
                    text: "Upload New File",
                    action: function (e) {
                        Dialog.confirm({
                            text: "Are you sure you want to add a new Subcontractor Billing Rate? This will permanently overwrite everything from the previous Subcontractor Billing Rate.",
                            title: "Upload New File Confirmation",
                            ok: function (e) {
                                var data = {}
                                var contractGuid = $("#ContractGuid").val();
                                data.url = "/SubcontractorBillingRates/Add?id=" + contractGuid;
                                data.submitURL = "/SubcontractorBillingRates/Add";
                                var options = {
                                    title: 'Add subcontractor billing rates',
                                    events: [
                                        {
                                            text: "Save",
                                            primary: true,
                                            action: function (e, values) {

                                                window.location.reload();
                                            }
                                        },
                                        {
                                            text: "Cancel",
                                            action: function (e) {
                                                $('html').removeClass('htmlClass');
                                            }
                                        }
                                    ]
                                };
                                Dialog.openDialogGridSubmit(data, options);
                            },
                            cancel: function () {
                                $('html').removeClass('htmlClass');
                            }
                        })
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                        $('html').removeClass('htmlClass');
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idViewSBRNonCSV", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/SubcontractorBillingRates/Detail?id=" + contractGuid;
        data.submitURL = "/SubcontractorBillingRates/Detail";
        var options = {
            title: 'Edit subcontractor billing rates',
            events: [
                {
                    text: "Upload New File",
                    action: function (e) {
                        Dialog.confirm({
                            text: "Are you sure you want to add a new Subcontractor Billing Rate? This will permanently overwrite everything from the previous Subcontractor Billing Rate.",
                            title: "Upload New File Confirmation",
                            ok: function (e) {
                                var data = {}
                                var contractGuid = $("#ContractGuid").val();
                                data.url = "/SubcontractorBillingRates/Add?id=" + contractGuid;
                                data.submitURL = "/SubcontractorBillingRates/Add";
                                var options = {
                                    title: 'Add subcontractor billing rates',
                                    events: [
                                        {
                                            text: "Save",
                                            primary: true,
                                            action: function (e, values) {
                                                window.location.reload();
                                            }
                                        },
                                        {
                                            text: "Cancel",
                                            action: function (e) {
                                            }
                                        }
                                    ]
                                };
                                Dialog.openDialogGridSubmit(data, options);
                            },
                            cancel: function () {
                            }
                        })
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });
    //end

    function saveUpdatedGrid() {
        var uploadGrid = document.getElementsByClassName("UploadGrid");
        var gridName = uploadGrid[0].getAttribute("data-gridname");
        var jobrequest = uploadGrid[0].getAttribute("data-jobrequest");
        var gridId = $(gridName);
        var gridIdJob = $(jobrequest);
        var controller = uploadGrid[0].getAttribute("data-controller");
        var displayedData = gridId.data().kendoGrid.dataSource.view();
        var displayedDataAsJSON = JSON.stringify(displayedData);
        var url = "/" + controller + "/Get/";
        var token = getCookieValue('X-CSRF-TOKEN');
        var reqValToken = getCookieValue('RequestVerificationToken');
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': reqValToken 
            },
            dataType: 'json',
            url: url,
            type: "POST",
            data: JSON.stringify(displayedDataAsJSON),
            success: function (values) {
                $(gridIdJob).data('kendoGrid').dataSource = $(gridId).data('kendoGrid').dataSource;
                $(gridIdJob).data('kendoGrid').refresh();
                LoadWBSGrid();
                $.notify("Sucessfully changed", values.status);
            },
            error: function (ee) {
                var message = "";
                for (var i in ee.responseJSON) {
                    if (message != "")
                        message += "";
                    message += ee.responseJSON[i];
                }
                $('#loading').hide();
                Dialog.alert(message);
            }
        });
    };

    $(".IsIntercompanyWorkOrder").change(function () {
        if ($('input[name="IsIntercompanyWorkOrder"]:checked').val() == "False") {
            $(".interCompanyDependent").hide('slow');
            var multiSelect = $('#CompanySelected').data("kendoMultiSelect");
            multiSelect.value([]);
        }
        else {
            $(".interCompanyDependent").show('slow');
        }
    });

    if ($('input[name="IsIntercompanyWorkOrder"]:checked').val() == "False")
        $(".interCompanyDependent").hide();
    else
        $(".interCompanyDependent").show();
});

