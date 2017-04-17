$('#button').attr('disabled', true);
$('input:not([type=image])').keyup(function () {
    var disable = false;
    $('input:not([type=image])').each(function () {
        if ($(this).val() == "") {
            disable = true;
        }
    });
    $('#button').prop('disabled', disable);
});