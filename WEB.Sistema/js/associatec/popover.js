function ObjPopover() {

    this.iniciarPopover = function (conteudo) {
            
        if (!conteudo) {
            conteudo = $('body');
        }

        $(conteudo).find(".show-popover").each(function () {
            Popovers.ativarPopover($(this));
        });
    }

    //Ativar os popovers
    this.ativarPopover = function (element) {
        
        var titulo = $(element).attr("data-title");

        var url = $(element).data("url");

        var posicao = $(element).attr("data-placement");

        var dataPost = $(element).attr("data-post");

        if(typeof(dataPost) == 'undefined'){
            dataPost = [];
        }else{
            dataPost = $.parseJSON(dataPost);
        }

        var flagRequest = true;

        if (url.indexOf('#') != -1) {

            flagRequest = false;

        }

        $(element).on("click", function () {

            $(element).popover({
                html: true,
                title: titulo,
                placement: posicao,
                content: function(){

                    if (flagRequest == false) {

                        return $(url).html();
                    }

                    return '<div class="carregando"></div>'
                }
            }).popover('show');

            if (flagRequest == false) {
                return;
            }

            $.ajax({
                url: url,
                type: 'POST',
                data: dataPost,
                success: function (response) {

                    $(element).popover('destroy');
                    $(".popover").hide();

                    setTimeout(function () {

                        $(element).popover({ html: true, placement: posicao, title: titulo, content: response });
                        $(element).popover('show');

                        DefaultSistema.iniciarPlugins($(".popover"));

                    }, 500);
                }
            });
        });

        $(element).on("mouseout", function () {
            //element.popover('destroy');
        });
    }

    this.closePopover = function () {
        $(".popover").popover("destroy");
    }
}

var Popovers = new ObjPopover();

$(document).ready(function () {
    Popovers.iniciarPopover();
});