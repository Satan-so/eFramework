var pageIndex = parseInt($('#pageIndex')[0].value);
var pageSize = $('#pageSize')[0].value;
var infoCount = $('#infoCount')[0].value;
var pageCount = Math.ceil(infoCount / pageSize);

$('.pager .info-count').html(infoCount);
$('.pager .page-count').html(pageIndex + '/' + pageCount);

$('.pager .start-page').click(function () {
    $('#pageIndex')[0].value = 1;
    $('form')[0].submit();
});

$('.pager .end-page').click(function () {
    $('#pageIndex')[0].value = pageCount;
    $('form')[0].submit();
});

$('.pager .pre-page').click(function () {
    $('#pageIndex')[0].value = pageIndex - 1;
    $('form')[0].submit();
});

$('.pager .next-page').click(function () {
    $('#pageIndex')[0].value = pageIndex + 1;
    $('form')[0].submit();
});