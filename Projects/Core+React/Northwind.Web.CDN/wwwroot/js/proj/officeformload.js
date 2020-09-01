$(document).ready(function () {

    $(".PhysicalCountryId").change(function () {
        getStatesByCountryId(".PhysicalStateId", $(this).val())
    });
    $(".MailingCountryId").change(function () {
        getStatesByCountryId(".MailingStateId", $(this).val())
    });
    $("#idisphyscialAdd").click(function () {
        if ($(this).is(":checked")) {
            $(".MailingAddress").val($(".PhysicalAddress").val());
            $(".MailingAddressLine1").val($(".PhysicalAddressLine1").val());
            $(".MailingZipCode").val($(".PhysicalZipCode").val());
            $(".MailingCity").val($(".PhysicalCity").val());
            $(".MailingCountryId").val($(".PhysicalCountryId").val());
            getStatesByCountryId(".MailingStateId", $(".PhysicalCountryId").val())
        } else {
            $(".MailingAddress").val(null);
            $(".MailingAddressLine1").val(null);
            $(".MailingZipCode").val(null);
            $(".MailingCity").val(null);
            $(".MailingCountryId").val(null);
            $(".MailingStateId").val(null);
        }
    })
    function getStatesByCountryId(element, value) {
        var inputtext = element;
        $.ajax({
            dataType: 'json',
            type: "GET",
            url: "/States/GetStatesByCountryId?countryId=" + value,
            success: function (data) {
                if (data.length > 0) {
                    var options = $(inputtext);
                    $('option', options).remove();
                    $.each(data, function () {
                        options.append($('<option/>').val(this.keys).text(this.values));
                    });
                    $(inputtext).val($(".PhysicalStateId").val());
                } else {
                    $(inputtext).html("<option value=''>No record found </option>");
                    $(inputtext).prop('required', false);
                }
            }
        });
    }

    $("#AddOfficeContact").on('click',
        function (evt) {
            var officeGuid = null;
            var ContactType = null;
            var data = {}
            data.url = "/Admin/OfficeContact/Create?OfficeGuid=" + officeGuid + '&ContactType=' + ContactType;
            data.submitURL = "/Admin/OfficeContact/Create/";
            var options = {
                title: 'Add New Contact',
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
            Dialog.openDialogSubmit(data, options);
        });
});// end of document..

 