// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#file-upload').change(function () {
        var file = $('#file-upload')[0].files[0].name;
        $('#custom-file-upload-name').text(file);
    });
});