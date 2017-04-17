
$('#postBack').prop('disabled', true);

$('table.table input[type=checkbox]').on('change', function () {
    var checked = $(this).closest('table').find('input[type=checkbox]:checked').length === 0;

    $('#postBack').prop('disabled', checked);
});


//script alters with action of removing users from session
//$("#uncheck").click(function () {
//    $('table.table input[type=checkbox]').each(function () {
//        this.checked = false;
//    });
//});

//$(document).ready(function () {
//    $('#checkall').click(function () {
//        var checked = $(this).prop('checked');
//        $('#checkboxes').find('input:checkbox').prop('checked', checked);
//    });
//})