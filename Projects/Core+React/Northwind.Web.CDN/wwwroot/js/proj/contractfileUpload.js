$(document).ready(function () {
    $('input[type="file"]').change(function (e) {
        var fileName = "";
        var fileArray = [];

        for (i = 0; i < e.target.files.length; i++) {
            fileName += "<span class='badge badge-pill badge-secondary ml-2'>";
            fileName += e.target.files[i].name;
            //fileName += "<i class='k-icon k-i-close k-icon-sm'></i>";
            fileName += "</span>";
            $('.file-name').html(fileName);
        }
    });
});
