// LOGOUT

(function ($) {
    $.fn.logout = function (options) {
        var base = this;

        $(base).click(function (e) {
            e.preventDefault();

            $.ajax({
                type: "POST",
                url: '/KadenaWebService.asmx/' + base.attr("data-function"),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    window.location.reload(false);
                },
                error: function (xhr, ajaxOptions, thrownError) { }
            });
        });
    }
}(jQuery));

// custom script initialization

var customScripts = {
    logoutInit: function () {
        $("#js-logout").logout();
    },
    init: function () {
        var base = this;

        base.logoutInit();        
    }
}

$(document).ready(function () {
    customScripts.init();
});