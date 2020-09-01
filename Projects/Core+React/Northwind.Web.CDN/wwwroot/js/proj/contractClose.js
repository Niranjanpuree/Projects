$(document).ready(function () {

    var panelBar = $(".contractClosePanel").kendoPanelBar({
        collapse: onCollapse
    });

    var onCollapse = function (e) {
        // detach collapse event handler via unbind()
        panelBar.data("kendoPanelBar").unbind("collapse", onCollapse);
    };
    hideShow();

    $('.checked-Value').click(function () {
        hideShow();
    })

    $('.enable-hide').click(function () {
        hideShow();
    })

    $('.enable-hideCR').click(function () {
        hideShow();
    })

    $('.enable-hideAR').click(function () {
        hideShow();
    })

    function hideShow() {

        var countPM = [];
        var countCR = [];
        var countAR = [];

        //#region projectManager
        if ($('.checked-Value:checked').val() == "Yes") {
            $(".hide-SubQuestions").show("fast");
        }
        else {
            $(".hide-SubQuestions").hide("fast");
        }

        if ($('.enable-hide:checked').val() == "No") {
            $(".hide-ProjectManagerNotes").show("fast");
        }

        $('.enable-hide:checked').each(function () {
            var value = $(this).val().toUpperCase();
            if (value == "NO") {
                countPM.push(countPM + 1);
            }
        });

        if (countPM.length > 0) {
            $(".hide-ProjectManagerNotes").show("fast");
        }
        else {
            $(".hide-ProjectManagerNotes").hide("fast");
        }
        var representativeType = $('#RepresentativeType').val();
        switch (representativeType)
        {
            case "PROJECT-MANAGER":
                break;
            case "CONTRACT-REPRESENTATIVE":
                break;
            case "ACCOUNTING-REPRESENTATIVE":
                break;
        }
        ///ContractResourceFile/GetFilesByResourceId ? resourceId = " + sender.props.resourceId

        //#endregion projectManager

        //#region contractRepresentative
        $('.enable-hideCR:checked').each(function () {
            var value = $(this).val().toUpperCase();
            if (value == "NO") {
                countCR.push(countCR + 1);
            }
        });

        if (countCR.length > 0) {
            $(".hide-ContractRepresentNotes").show("fast");
        }
        else {
            $(".hide-ContractRepresentNotes").hide("fast");
        }
        //#endregion contractRepresentative

        //#region AccountRepresentative
        $('.enable-hideAR:checked').each(function () {
            var value = $(this).val().toUpperCase();
            if (value == "NO") {
                countAR.push(countAR + 1);
            }
        });

        if (countAR.length > 0) {
            $(".hide-AccountRepresentNotes").show("fast");
        }
        else {
            $(".hide-AccountRepresentNotes").hide("fast");
        }
        //#endregion AccountRepresentative


    }

    $("#contractCloseForm").submit(function (e) {
        var isSubmitableForm = true;
        var inputName = "";
        $('.form-required').each(function (e) {
            if ($(this).find('input').prop('disabled') == false) {
                var isChecked = $(this).find('input:radio:checked').val();
                var checkedName = $(this).find('input').attr('name');
                if (inputName != checkedName) {
                    if (isChecked == null || isChecked == "undefined") {
                        $(this).closest('.validateMsg').find('.text-danger').text('This field is required')
                        isSubmitableForm = false;
                    }
                    else {
                        $(this).closest('.validateMsg').find('.text-danger').text('')
                    }
                    inputName = checkedName;
                }
            }
        });
        $('.form-requiredSubQuestion').each(function (e) {
            if ($(this).find('input').prop('disabled') == false) {

                var checkedValue = $("input:radio.checked-Value:checked").val();
                var checkedName = $(this).find('input').attr('name');
                if (inputName != checkedName) {
                    if (checkedValue != null) {
                        if (checkedValue.toUpperCase() == "YES") {
                            var isChecked = $(this).find('input:radio:checked').val();
                            if (isChecked == null || isChecked == "undefined") {
                                $(this).closest('.validateMsg').find('.text-danger').text('This field is required')
                                isSubmitableForm = false;
                            }
                            else {
                                $(this).closest('.validateMsg').find('.text-danger').text('')
                            }
                        }
                    }
                    inputName = checkedName;
                }
            }
        });

        if (!isSubmitableForm)
            return false;

        e.preventDefault();
        var contractform = $('body').find('form')
        let fileForm = new FormData();

        var contractFormData = $(contractform).serialize();
        var formData = contractFormData.split("&");
        for (var i in formData) {
            var decodedModel = (decodeURIComponent(formData[i]).replace(/%5B/g, '[').replace(/%5D/g, ']'));
            var keyVal = decodedModel.split("=")
            if (keyVal.length > 1) {
                fileForm.append(keyVal[0], decodeURIComponent(keyVal[1]));
            }
        }
        var token = getCookieValue('X-CSRF-TOKEN');
        var reqValToken = getCookieValue('RequestVerificationToken');
        var url = "/ContractCloseOut/Add";
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
                    $('body').html('')
                    return $('body').html(response);
                }

                var redirectUrl = '';
                if (response.taskOrder) {
                    redirectUrl = "/Project/Details/" + response.contractGuid;
                }
                else {
                    redirectUrl = "/Contract/Details/" + response.contractGuid;
                }
                //finally saved the uploaded file in the server...
                window.uploader.onSubmitFiles(response.resourceId, response.uploadPath, false, response.parentId, undefined, undefined, undefined, redirectUrl);


            },
            error: function (data) {
                alert("Some error occurred ")
            }
        });
    })

});