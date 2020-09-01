$(document).ready(function () {
    InitialLoadMethod();
    function InitialLoadMethod() {
        var customerTypevalue = $('#CustomerTypeGuid').find('option:selected').text();
        switch (customerTypevalue) {
            case "Commercial":
                $(".Department").val(null).prop('required', false);
                $(".Agency").val(null).prop('required', false);
                $(".CustomerCode").val(null).prop('required', false);
                $('#HideDepartment').css('display', 'none');
                $('#idHideCustomerCode').css('display', 'none');
                break;
            case "Federal":
                $('#HideDepartment').fadeIn('slow');
                $('#idHideCustomerCode').fadeIn('slow');
                $(".Department").prop('required', true);
                $(".CustomerCode").prop('required', true);
                $(".Agency").prop('required', true);
                break;
            default:
                $(".Department").val(null).prop('required', false);
                $(".Agency").val(null).prop('required', false);
                $(".CustomerCode").val(null).prop('required', false);
                $('#HideDepartment').css('display', 'none');
                $('#idHideCustomerCode').css('display', 'none');
                break;
        }
    }
    $("#CustomerTypeGuid").change(function () {
        InitialLoadMethod();
    });
    $(".Department").change(function () {
        var DepartmentValue = $(this).val();
        getCustomerByDepartment(DepartmentValue);
    });
    function getCustomerByDepartment(DepartmentValue) {
        selectedValue = null
        if (arguments.length > 0) {
            selectedValue = arguments[1];
        }
        $.ajax({
            url: '/UsCustomerOfficeList/GetCustomerByDepartment',
            type: 'GET',
            dataType: 'json',
            data: { DepartmentValue: DepartmentValue },
            success: function (result) {
                var options = $('.Agency');
                $('option', options).remove();
                $.each(result, function () {
                    options.append($('<option/>').val(this.keys).text(this.values));
                });
                if (selectedValue != null) {
                    $('.Agency').val(selectedValue);
                }
            }
        });
    }
    $(document).ajaxStop(function (DepartmentValue) {
        $("#loading").hide();
    });
    $(".idcountry").change(function () {
        var value = $(".idcountry").val();
        getStatesByCountryId(value);
    });
    function getStatesByCountryId(value) {
        $.ajax({
            dataType: 'json',
            type: "GET",
            url: "/States/GetStatesByCountryId?countryId=" + value,
            success: function (data) {
                if (data.length > 0) {
                    var options = $('.idstate');
                    $('option', options).remove();
                    $.each(data, function () {
                        options.append($('<option/>').val(this.keys).text(this.values));
                    });

                } else {
                    $(".idstate").html("<option value=''>No record found </option>");
                    $(".idstate").prop('required', false);
                }
            }
        });
    }


    $(".CustomerCode").change(function () {
        onchangeCustomerCode();
    });
    function onchangeCustomerCode() {
        var customerCode = $('.CustomerCode').val();
        $.ajax({
            url: '/UsCustomerOfficeList/GetCustomerDetailBySixDigitCode',
            type: 'GET',
            dataType: 'json',
            data: { sixDigitCode: customerCode },
            success: function (result) {
                if (result.length > 0) {
                    var value = result[0];
                    populateTheValue(value);
                }
            }
        });
    }
    function populateTheValue(value) {
        $(".CustomerName").val(value.contractingOfficeName);
        $(".Department").val(value.departmentName);
        $(".Address").val(value.addressCity);
        $(".City").val(value.addressCity);
        $(".ZipCode").val(value.zipCode);
        getCustomerByDepartment(value.departmentName, value.customerName);
        getStatesByAbbreviation(value.addressState)
    }
    function getStatesByAbbreviation(abb) {
        $.ajax({
            dataType: 'json',
            type: "GET",
            url: "/States/GetStatesByAbbreviation?Abbreviation=" + abb,
            success: function (data) {
                if (data.values == null) {
                    $('.idstate').val(null);
                }
                else {
                    $('.idstate').val(data.keys);
                }
            }
        });
    }
});