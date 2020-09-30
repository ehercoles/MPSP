// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.cpf').mask('000.000.000-00', { placeholder: "___.___.___-__" });

    $('#Sigiloso').on('change', function () {
        if (!this.checked) {
            $('.parte').val('');
            $('.parte').attr('required', false);
            $('.parte').attr('disabled', true);
            $('.parteVal').removeClass('field-validation-error');
            $('.parteVal').addClass('field-validation-valid');
            $('.parteVal').html('');
        } else {
            $('.parte').attr('required', true);
            $('.parte').attr('disabled', false);
        }
    });
});

function appendLeadingZeros(x) {
    x.value = (("00000" + x.value).slice(-5));
}
