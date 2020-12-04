var ScrollPaginationCustomJS = {
    Class: function () {
        this.paginate = function (urlList, idContent) {
            $('#' + idContent).scrollPagination({
                nop: 1,
                offset: 1,
                delay: 300,
                scroll : true,
                url: urlList
            });
        }
    }
};
var ScrollPaginationCustom = new ScrollPaginationCustomJS.Class();

$(document).ready(function () {
});