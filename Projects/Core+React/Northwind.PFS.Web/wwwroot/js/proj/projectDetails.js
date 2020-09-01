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
        $(".ProjectPanel").kendoPanelBar({
            collapse: onCollapse
        });
    }

    var onCollapse = function (e) {
        // detach collapse event handler via unbind()
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };

    $(document).on("click",
        "#editProject",
        function (event) {
            window.location.href = "/project/Edit/" + $("#ContractGuid").val();
        });

    //Generic ajax post for commands..
    var ajaxPost = function (url, data) {
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            dataType: 'json',
            url: url,
            type: "POST",
            data: JSON.stringify(data),
            success: function (values) {
                $.notify(values.message, values.status);
                location.reload();
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
    $(document).on("click", "#DisableProject", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  disable this Project?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Project/Disable', ids);
            },
            cancel: function (e) {
            }
        });
    });


    $(document).on("click", "#EnableProject", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  enable this Project?",
            title: "Confirm",
            ok: function (e) {
                ajaxPost('/Project/Enable', ids);
            },
            cancel: function (e) {
            }
        });
    });

    $(document).on("click", "#DeleteProject", function () {
        var ids = selectedIds();
        if (ids <= 0) {
            Dialog.alert("No rows selected !!");
            return false;
        }
        Dialog.confirm({
            text: "Are you sure you want to  delete this Project?",
            title: "Confirm",
            ok: function (e) {
                $.ajax({
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    dataType: 'json',
                    url: '/Project/Delete',
                    type: "POST",
                    data: JSON.stringify(ids),
                    success: function (values) {
                        window.location.href = "/contract";
                    },
                    error: function (values) {
                        $.notify(values.message, values.status);
                    }
                });
            },
            cancel: function (e) {
            }
        });
    });

    $(".navigateButton").each(function (e) {
        if (!$(this).attr("data-val")) {
            $(this).removeAttr("href");
        }
    });

    //this method is same as exist in contract feature..
    //$(document).on("mouseover", ".getDetailOfUser", function (e) {
    //    $.ajax({
    //        url: '/Contract/DetailsOfUser/',
    //        type: 'GET',
    //        data: { id: this.id },
    //        async: true,
    //        success: function (response) {
    //            var htmldetail = "";
    //            htmldetail += "<span class='popover-detail-container'>";
    //            htmldetail += "<span><label>Name</label> : " + response.displayName + "</span>";
    //            htmldetail += "<span><label>Title</label> : " + response.jobTitle + "</span>";
    //            if (response.workEmail != null)
    //                htmldetail += "<span><label>Email</label> : <a href='mailto:" + response.workEmail + "' target='_blank'>" + response.workEmail + "</a></span>";
    //            if (response.workPhone != null)
    //                htmldetail += "<span><label>Phone</label> : " + response.workPhone + "</span>";
    //            htmldetail += "</span>";
    //            $('.tooltipuser').html(htmldetail);
    //            return response;
    //        }
    //    });
    //});

    $(document).on("mouseover", ".getDetailOfContact", function (e) {
        $.ajax({
            url: '/Contract/DetailsOfContact/',
            type: 'GET',
            data: { id: this.id },
            success: function (response) {
                var htmldetail = "";
                htmldetail += "<span class='popover-detail-container'>";
                htmldetail += "<span><label>Name</label> : " + response.firstName + " " + response.middleName + " " + response.lastName + "</span>";
                if (response.emailAddress.length > 0 && response.emailAddress != "N/A")
                    htmldetail += "<span><label>Email</label> : <a class='ml-1 text-lowercase' href='mailto: " + response.emailAddress + "' target='_blank'>" + " " + response.emailAddress + "</a></span>";
                if (response.emailAddress.length > 0 && response.emailAddress == "N/A")
                    htmldetail += "<span><label>Email</label> : " + response.emailAddress + "' target='_blank'>" + " " + response.emailAddress + "</span>";
                if (response.phoneNumber.length > 0)
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
                if (response.primaryEmail.length > 0 && response.primaryEmail != "N/A")
                    htmldetail += "<span><label>Email</label> : <a class='ml-1 text-lowercase' href='mailto: " + response.primaryEmail + "' target='_blank'>" + " " + response.primaryEmail + "</a></span>";
                if (response.primaryEmail.length > 0 && response.primaryEmail == "N/A")
                    htmldetail += "<span><label>Email</label> : " + response.primaryEmail + "</span>";
                if (response.primaryPhone.length > 0)
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

    $(document).on("click", "#idAddContractMod", function () {
        var data = {}
        var contractGuid = $('#ContractGuid').val();
        var ContractNumber = $('.getContractNumber').attr("id");
        data.url = "/ContractModification/add?ContractGuid=" + contractGuid;
        data.submitURL = "/ContractModification/add";
        var options = {
            title: 'Add Contract Modification : ' + ContractNumber,
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
                    }
                }
            ]
        };
        Dialog.openDialogSubmit(data, options);
    });

    $(document).on("click", "#idAddProject", function () {
        var contractGuid = $('#ContractGuid').val();
        window.location.href = "/project/add?ContractGuid=" + contractGuid;
    });

    //ContractQuestionaire
    $(document).on("click", "#idAddContractQuestionaire", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "/ContractQuestionaire/Add?id=" + contractGuid;
        data.submitURL = "/ContractQuestionaire/Add";
        var options = {
            title: 'Applicable FAR Clauses',
            events: [
                {
                    text: "Save",
                    primary: true,
                    action: function (e, values) {
                        $.notify(values.message, values.status);
                        window.location.href = "/project/Details?id=" + contractGuid;
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

    $(document).on("click", "#idEditContractQuestionaire", function () {
        var data = {}
        var ContractQuestionaireGuid = $("#ContractQuestionaire_ContractQuestionaireGuid").val();
        data.url = "/ContractQuestionaire/Edit?id=" + ContractQuestionaireGuid;
        data.submitURL = "/ContractQuestionaire/Edit";
        var options = {
            title: 'Detail information of the Applicable FAR Clauses ',
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
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idViewContractQuestionaire", function () {
        var data = {}
        var ContractQuestionaireGuid = $("#ContractQuestionaire_ContractQuestionaireGuid").val();
        data.url = "/ContractQuestionaire/Detail?id=" + ContractQuestionaireGuid;
        data.submitURL = "/ContractQuestionaire/Detail";
        var options = {
            title: 'Detail information of the Applicable FAR Clauses ',
            events: [
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
                        $.notify(values.message, values.status);
                        window.location.href = "/project/Details?id=" + contractGuid;
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

    $(document).on("click", "#idViewEmployeeBillingRates", function () {
        var data = {}
        var ContractGuid = $("#ContractGuid").val();
        data.url = "/EmployeeBillingRates/Detail?id=" + ContractGuid;
        data.submitURL = "/EmployeeBillingRates/Detail";
        var options = {
            title: 'Details of employee billing rates ',
            events: [
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditEmployeeBillingRates", function () {
        var data = {}
        var ContractGuid = $("#ContractGuid").val();
        data.url = "/EmployeeBillingRates/Edit?id=" + ContractGuid;
        data.submitURL = "/EmployeeBillingRates/Edit";
        var options = {
            title: 'Edit employee billing rates ',
            events: [
                {
                    text: "Update",
                    primary: true,
                    action: function (e, values) {
                        saveUpdatedGrid();
                        //window.location.href = "/project/Details?id=" + ContractGuid;
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
                                                $.notify(values.message, values.status);
                                                window.location.href = "/project/Details?id=" + ContractGuid;
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
                                                $.notify(values.message, values.status);
                                                window.location.href = "/project/Details?id=" + contractGuid;
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
                        $.notify(values.message, values.status);
                        window.location.href = "/project/Details?id=" + contractGuid;
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

    $(document).on("click", "#idViewContractWBS", function () {
        var data = {}
        var ContractGuid = $("#ContractGuid").val();
        data.url = "/WorkBreakdownStructure/Detail?id=" + ContractGuid;
        data.submitURL = "/WorkBreakdownStructure/Detail";
        var options = {
            title: 'Details of work breakdown structure',
            events: [
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditContractWBS", function () {
        var data = {}
        var ContractGuid = $("#ContractGuid").val();
        data.url = "/WorkBreakdownStructure/Edit?id=" + ContractGuid;
        data.submitURL = "/WorkBreakdownStructure/Edit";
        var options = {
            title: 'Edit work breakdown structure',
            events: [
                {
                    text: "Update",
                    primary: true,
                    action: function (e, values) {
                        saveUpdatedGrid();
                        //window.location.href = "/project/Details?id=" + ContractGuid;
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
                                                $.notify(values.message, values.status);
                                                window.location.href = "/project/Details?id=" + contractGuid;
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
                                                $.notify(values.message, values.status);
                                                window.location.href = "/project/Details?id=" + contractGuid;
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
                    action: function (e, values) {
                        $.notify(values.message, values.status);
                        window.location.href = "/project/Details?id=" + contractGuid;
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

    $(document).on("click", "#idViewLaborCategoryRates", function () {
        var data = {}
        var ContractGuid = $("#ContractGuid").val();
        data.url = "/SubcontractorBillingRates/Detail?id=" + ContractGuid;
        data.submitURL = "/SubcontractorBillingRates/Detail";
        var options = {
            title: 'Details of subcontractor billing rates',
            events: [
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idEditLaborCategoryRates", function () {
        var data = {}
        var ContractGuid = $("#ContractGuid").val();
        data.url = "/SubcontractorBillingRates/Edit?id=" + ContractGuid;
        data.submitURL = "/SubcontractorBillingRates/Edit";
        var options = {
            title: 'Edit subcontractor billing rates',
            events: [
                {
                    text: "Update",
                    primary: true,
                    action: function (e, values) {
                        saveUpdatedGrid();
                        //window.location.href = "/project/Details?id=" + ContractGuid;
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
                                                $.notify(values.message, values.status);
                                                window.location.href = "/project/Details?id=" + ContractGuid;
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

    $(document).on("click", "#idViewSBRNonCSV", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
        data.url = "SubcontractorBillingRates/Detail?id=" + contractGuid;
        data.submitURL = "SubcontractorBillingRates/Detail";
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
                                                $.notify(values.message, values.status);
                                                window.location.href = "/project/Details?id=" + contractGuid;
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
        var gridId = $(gridName);
        var controller = uploadGrid[0].getAttribute("data-controller");
        var displayedData = gridId.data().kendoGrid.dataSource.view();
        var displayedDataAsJSON = JSON.stringify(displayedData);
        var url = "/" + controller + "/Get";
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
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

    ////Hide the fields that do not contain a value
    //if ($("#IsPrimeContractHidden").val() === "True")
    //    $(".SubContractNumber").hide();
    //else
    //    $(".SubContractNumber").show();

    //if ($("#QualityLevelRequirementsHidden").val() === "True")
    //    $(".QualityLevel").show();
    //else
    //    $(".QualityLevel").hide();

    //if ($("#IsIDIQContractHidden").val() === "True")
    //    $(".BlueSkyAward_Amount").show();
    //else
    //    $(".BlueSkyAward_Amount").hide();

    //if ($("#SetAsideHidden").val() === "SDB Set Aside 8(a)")
    //    $(".setAsideDependent").show();
    //else
    //    $(".setAsideDependent").hide();

    //if (($("#ContractTypeVal").val() === "Cost Plus Award Fee") || ($("#ContractTypeVal").val() === "Cost Plus Fixed Fee") || ($("#ContractTypeVal").val() === "Cost Plus Incentive Fee"))
    //    $(".contractTypeDependent").show();
    //else
    //    $(".contractTypeDependent").hide();

    //if (($("#ContractTypeVal").val() === "Cost Plus Award Fee") || ($("#ContractTypeVal").val() === "Cost Plus Fixed Fee") || ($("#ContractTypeVal").val() === "Cost Plus Incentive Fee") || ($("#ContractTypeVal").val() === "Firm Fixed Price"))
    //    $(".contractTypeDoubleDependent").show();
    //else
    //    $(".contractTypeDoubleTypeDependent").hide();
    ////end 

    // Revenue Recognition - START'S
    $(document).on("click", "#idAddRevenueRecognition", function () {
        var data = {}
        var contractGuid = $("#ContractGuid").val();
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
        var ContractNumber = $('.getContractNumber').attr("id");
        data.url = "/RevenueRecognition/Detail/" + RevenueGuid;
        data.submitURL = "/RevenueRecognition/SaveRevenueRecognition";
        var options = {
            title: 'Detail Revenue Recognition Contract No : ' + ContractNumber,
            events: [
                {
                    text: "Cancel",
                    action: function (e) {
                    }
                }
            ]
        };
        Dialog.openDialogGridSubmit(data, options);
    });

    $(document).on("click", "#idDeleteRevenueRecognition", function () {
        var id = $("#RevenueRecognitionModel_RevenueRecognizationGuid").val();
        //var ids = [];
        //ids.push(id);
        Dialog.confirm({
            text: "Are you sure you to  remove completely ?",
            title: "Confirm",
            ok: function (e) {
                $.ajax({
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    },
                    dataType: 'json',
                    url: '/RevenueRecognition/Delete/',
                    type: "POST",
                    data: JSON.stringify(id),
                    success: function (values) {
                        $.notify(values.message, values.status);
                        window.location.href = "/project/Details?id=" + values.model.contractGuid;
                    },
                    error: function (values) {
                        $.notify(values.message, values.status);
                    }
                });
            },
            cancel: function (e) {
            }
        });
    });
    // Revenue Recognition - END'S

    //$(document).on("click", ".remove-file", function (e) {
    //    var ContractGuid = $("#ContractGuid").val();
    //    var id = this.id;
    //    $.ajax({
    //        url: '/project/_RemoveFiles/',
    //        type: 'POST',
    //        data: { id: ContractGuid, path: this.name },
    //        success: function (response) {
    //            debugger
    //            $('#' + id).parent("div").hide();
    //        }
    //    });
    //});
});