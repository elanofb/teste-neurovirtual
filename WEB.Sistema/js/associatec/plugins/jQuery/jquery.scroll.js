﻿(function ($) {

    $.fn.scrollPagination = function (options) {

        var settings = {
            nop: 1,
            offset: 0,
            error: '',
            delay: 500,
            scroll: true,
            url: true,
            classButton: 'ver-mais'
        };

        if (options) {
            $.extend(settings, options);
        }

        var div = $(this);
        var offset = settings.offset;
        var busy = false;

        $initmessage = '';
        if (settings.scroll == true) {
            $initmessage = '';
        }

        // Append custom messages and extra UI
        div.append('<div class="content"></div><div class="clear"></div>');

        function getData() {

            $.post(settings.url + '?page=' + offset, {}, function (data) {

                div.find('.carregando').html($initmessage);

                if (data == "") {
                    div.find('.carregando').html(settings.error);

                } else {
                    offset = offset + settings.nop;

                    div.find(".content").append(data);

                    busy = false;

                    $('.' + settings.classButton).show();
                    $(".carregando").hide();
                }
            });

        }

        getData();

        if (settings.scroll == true) {
            $(window).scroll(function () {

                if ($(window).scrollTop() + $(window).height() > div.height() && !busy) {

                    busy = true;
                    div.find('.carregando').first().show();

                    setTimeout(function () {
                        getData();
                    }, settings.delay);

                }
            });
        } else {
            $('.' + settings.classButton).on("click", function () {

                var button = $(this);

                button.hide();
                button.parent().find(".carregando").show();

                if ($(window).scrollTop() + $(window).height() > div.height() && !busy) {

                    busy = true;
                    //div.find('.carregando').first().show();

                    setTimeout(function () {
                        getData();

                    }, settings.delay);

                }
            });
        }
    }

})(jQuery);