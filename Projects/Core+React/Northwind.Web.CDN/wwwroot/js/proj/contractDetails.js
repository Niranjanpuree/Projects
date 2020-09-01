$(document).ready(function () {

    InitialLoad();
    function InitialLoad() {
        $("#tabstrip").kendoTabStrip({
            animation: {
                open: {
                    effects: "fadeIn"
                }
            },
            //            select: onSelect
        });

        $(".contractPanel").kendoPanelBar({
            collapse: onCollapse
        });

        //hide project and mod list if not IDIQ contract..
        if ($("#IsIDIQContractHidden").val() == "False") {
            var tabStrip = $("#tabstrip").kendoTabStrip().data("kendoTabStrip");
            $($("#tabstrip").data("kendoTabStrip").items()[1]).attr("style", "display:none");
            $("#idAddProject").remove();
        }
    }

    var onCollapse = function (e) {
        // detach collapse event handler via unbind()
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };

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
                window.location.href = "/contract/Details?id=" + values.model.contractGuid;
            },
            error: function (values) {
                $.notify(values.message, values.status);
            }
        });
    }
    var ajaxPostDelete = function (url, data) {
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
                window.location.href = "/contract";
            },
            error: function (values) {
                $.notify(values.message, values.status);
            }
        });
    }

    var selectedIds = function () {
        var ids = [];
        ids.push($("#ContractGuid").val());
        return ids;
    };

    $(document).on("click",
        "#editContract",
        function (event) {
            window.location.href = "/contract/Edit/" + $("#ContractGuid").val();
        });

    $(document).on("click",
        "#contractList",
        function (event) {
            window.location.href = "/contract";
        });

    $(document).on("click", "#DisableContract", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable this contract?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Contract/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#EnableContract", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable this contract?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Contract/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteContract", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete this contract?",
            title: "Confirm",
            ok: function (e) {
                ajaxPostDelete('/Contract/Delete', ids);
            },
            cancel: function (e) {
            }
        });
    });
    //end

    //ContractQuestionaire
    $(document).on("click", "#idAddContractQuestionaire", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        var projectNumber = $('.Get-ProjectNumber').attr("id");
        data.url = "/FarContract/Add?id=" + contractGuid;
        data.submitURL = "/FarContract/Add";
        var options = {
            title: projectNumber + ': Add Contract Clauses and Special Provisions',
            events: [
                {
                    text: "Save",
                    primary: true,
                    action: function (e, values) {
                        window.location.href = "/contract/Details?id=" + contractGuid;
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
        Dialog.openDialogNestedListSubmit(data, options);
    });

    $(document).on("click", "#idEditContractQuestionaire", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        var projectNumber = $('.Get-ProjectNumber').attr("id");
        data.url = "/FarContract/Edit?id=" + contractGuid;
        data.submitURL = "/FarContract/Edit";
        var options = {
            title: projectNumber + ': Edit Contract Clauses and Special Provisions',
            events: [
                {
                    text: "Update",
                    primary: true,
                    action: function (e, values) {
                        $.notify(values.message, values.status);
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
        Dialog.openDialogNestedListSubmit(data, options);
    });

    $(document).on("click", "#idViewContractQuestionaire", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        var projectNumber = $('.Get-ProjectNumber').attr("id");
        data.url = "/FarContract/Detail?id=" + contractGuid;
        data.submitURL = "/ContractQuestionaire/Detail";
        var options = {
            title: projectNumber + ': Contract Clauses and Special Provisions Details',
            events: [
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
    //end

    //EmployeeBillingRates
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
                    action: function (e, values) {
                        window.location.href = "/contract/Details?id=" + contractGuid;
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

    $(document).on("click", "#idViewEmployeeBillingRates", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/EmployeeBillingRates/Detail?id=" + contractGuid;
        data.submitURL = "/EmployeeBillingRates/Detail";
        var options = {
            title: 'Details of employee billing rates',
            events: [
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
                    action: function (e, values) {
                        saveUpdatedGrid();
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
                                                window.location.href = "/contract/Details?id=" + contractGuid;
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
                                                window.location.href = "/contract/Details?id=" + contractGuid;
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
    //end

    //ContractWBS
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
                        window.location.href = "/contract/Details?id=" + contractGuid;
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

    $(document).on("click", "#idViewContractWBS", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/WorkBreakdownStructure/Detail?id=" + contractGuid;
        data.submitURL = "/WorkBreakdownStructure/Detail";
        var options = {
            title: 'Details of the work breakdown structure',
            events: [
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
                    action: function (e, values) {
                        saveUpdatedGrid();
                        window.location.href = "/contract/Details?id=" + contractGuid;
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
                                                window.location.href = "/contract/Details?id=" + contractGuid;
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
        data.url = "/WorkBreakdownStructure/Detail?id=" + contractGuid;
        data.submitURL = "/WorkBreakdownStructure/Detail";
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
                                                window.location.href = "/contract/Details?id=" + contractGuid;
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
                    action: function (e, values) {
                        window.location.href = "/contract/Details?id=" + contractGuid;
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

    $(document).on("click", "#idViewLaborCategoryRates", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/SubcontractorBillingRates/Detail?id=" + contractGuid;
        data.submitURL = "/SubcontractorBillingRates/Detail";
        var options = {
            title: 'Details of subcontractor billing rates',
            events: [
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
                    action: function (e, values) {
                        saveUpdatedGrid();
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
                                                window.location.href = "/contract/Details?id=" + contractGuid;
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
                                                window.location.href = "/contract/Details?id=" + contractGuid;
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
    //end

    function saveUpdatedGrid() {
        var uploadGrid = document.getElementsByClassName("UploadGrid");
        var gridName = uploadGrid[0].getAttribute("data-gridname");
        var gridId = $(gridName);
        var controller = uploadGrid[0].getAttribute("data-controller");
        var displayedData = gridId.data().kendoGrid.dataSource.view();
        var displayedDataAsJSON = JSON.stringify(displayedData);
        var url = "/" + controller + "/Get";
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

    //Job Request Form
    $(document).on("click", "#idAddJobRequest", function () {
        window.location.href = "/JobRequest/Add/" + $("#ContractGuid").val();
    });

    $(document).on("click", "#idEditJobRequest", function () {
        window.location.href = "/JobRequest/Edit/" + $("#ContractGuid").val();
    });

    $(document).on("click", "#idViewJobRequest", function () {
        window.location.href = "/JobRequest/Detail/" + $("#ContractGuid").val();
    });
    //end
    // Revenue Recognition - START'S
    $(document).on("click", "#idAddRevenueRecognition", function () {
        var data = {}
        //var contractGuid = $("#ContractGuid").val();
        var revenueGuid = $("#RevenueRecognitionModel_RevenueRecognizationGuid").val();
        data.url = "/RevenueRecognition/Add?id=" + revenueGuid;
        var options = {
            title: 'Create Revenue Recognition ',
            events: [
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditRevenueRecognition", function () {
        var data = {}
        var RevenueGuid = $("#RevenueRecognitionModel_RevenueRecognizationGuid").val();
        data.url = "/RevenueRecognition/Edit/" + RevenueGuid;
        var options = {
            title: 'Update Revenue Recognition ',
            events: [
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idViewRevenueRecognition", function () {
        var data = {}
        var RevenueGuid = $("#RevenueRecognitionModel_RevenueRecognizationGuid").val();
        var contractNumber = $('.getContractNumber').attr("id");
        data.url = "/RevenueRecognition/Detail/" + RevenueGuid;
        var options = {
            title: 'Detail Revenue Recognition Contract No : ' + contractNumber,
            events: [
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

    $(document).on("click", "#idViewHistoryRevenueRecognition", function () {
        var data = {}
        //var RevenueGuid = $("#RevenueRecognitionModel_RevenueRecognizationGuid").val();
        var contractGuid = $("#ContractGuid").val();
        var contractNumber = $('.getContractNumber').attr("id");
        data.url = "/RevenueRecognition/DetailList/" + contractGuid;
        var options = {
            title: 'Detail Revenue Recognition Contract No : ' + contractNumber,
            events: [
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

    $(document).on("click", "#idDeleteRevenueRecognition", function () {
        var id = $("#RevenueRecognitionModel_RevenueRecognizationGuid").val();
        Dialog.confirm({
            text: "Are you sure you to  remove completely ?",
            title: "Confirm",
            ok: function (e) {
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
                    url: '/RevenueRecognition/Delete/',
                    type: "POST",
                    data: JSON.stringify(id),
                    success: function (values) {
                        $.notify(values.message, values.status);
                        window.location.href = "/contract/Details?id=" + values.model.contractGuid;
                    },
                    error: function (values) {
                        $.notify(values.message, values.status);
                    }
                });
            },
            cancel: function (e) {
                $('html').removeClass('htmlClass');
            }
        });
    });
    // Revenue Recognition - END'S

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


    $(document).on("mouseover", ".getDetailOfContact", function (e) {
        $.ajax({
            url: '/Contract/DetailsOfContact/',
            type: 'GET',
            data: { id: this.id },
            success: function (response) {
                var htmldetail = "";
                htmldetail += "<span class='popover-detail-container'>";
                htmldetail += "<span><label>Name</label> : " + response.firstName + " " + response.middleName + " " + response.lastName + "</span>";
                if (response.emailAddress != null && response.emailAddress != "N/A")
                    htmldetail += "<span><label>Email </label> : <a class='ml-1 text-lowercase' href='mailto:" + response.emailAddress + "' target='_blank'>" + response.emailAddress + "</a></span>";
                if (response.emailAddress == "N/A")
                    htmldetail += "<span><label>Email</label> : " + response.emailAddress + "</span>";
                if (response.phoneNumber != null)
                    htmldetail += "<span><label>Phone</label> : " + response.phoneNumber + "</span>";
                htmldetail += "</span>";
                $('.tooltipcontact').html(htmldetail);
                return response;
            }
        });
    });

    $(document).on("mouseover", ".getDetailOfAgency", function (e) {
        $.ajax({
            url: '/Contract/DetailsOfAgency/',
            type: 'GET',
            data: { id: this.id },
            success: function (response) {
                var htmldetail = "";
                htmldetail += "<span class='popover-detail-container'>";
                htmldetail += "<span><label>Name</label> : " + response.customerName + "</span>";
                htmldetail += "<span><label>Address</label> : " + response.address + "</span>";
                if (response.primaryEmail != null && response.primaryEmail != "N/A")
                    htmldetail += "<span><label>Email </label> : <a class='ml-1 text-lowercase' href='mailto:" + response.primaryEmail + "' target='_blank'>" + " " + response.primaryEmail + "</a></span>";
                if (response.primaryEmail != null && response.primaryEmail == "N/A")
                    htmldetail += "<span><label>Email</label> : " + response.primaryEmail + "</span>";
                if (response.primaryPhone != null)
                    htmldetail += "<span><label>Phone</label> : " + response.primaryPhone + "</span>";
                htmldetail += "</span>";
                $('.tooltipagency').html(htmldetail);
                return response;
            }
        });
    });

    $(document).on("mouseover", ".tooltipdetail", function (e) {
        $(this).parent().siblings(".popover-detail").addClass("active");
    });

    $(document).on("mouseout", ".tooltipdetail", function (e) {
        $('.tooltipdetail').parent().siblings(".popover-detail").removeClass("active");
    });

    $(document).on("click", "#idAddProject", function () {
        var contractGuid = $('#ContractGuid').val();
        window.location.href = "/project/add?ContractGuid=" + contractGuid;
    });


    $(document).on("click", "#idContractCloseOut", function () {
        var contractGuid = $("#ContractGuid").val();
        window.location.href = "/ContractCloseOut/Add?id=" + contractGuid;
    });

    $(document).on("click", "#idViewContractCloseOut", function () {
        var contractGuid = $("#ContractGuid").val();
        window.location.href = "/ContractCloseOut/Detail?id=" + contractGuid;
    });


    $(document).on("click", "#idAddContractNotice", function () {
        var data = {}
        var contractGuid = $('#ContractGuid').val();
        var ContractNumber = $('.getContractNumber').attr("id");
        data.url = "/ContractNotice/add?ContractGuid=" + contractGuid;
        data.submitURL = "/ContractNotice/add";
        var options = {
            title: 'Add Contract Notice for : ' + ContractNumber,
            height: '85%',
            events: [
                {
                    text: "Save",
                    primary: true,
                    action: function (e, values) {
                    }
                },
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        //var dialog = $("#dialog").data("kendoDialog");
        //dialog.close();
        Dialog.openDialogSubmit(data, options);
    });
});