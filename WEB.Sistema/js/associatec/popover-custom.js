function PopoverCustomClass() {

    this.init = function () {

        this.iniciarPopoverBody();

    };

    //Inicia os conteúdos nos links de ajuda com popover customizado
    this.iniciarPopoverBody = function () {

        $('.popover-item').each(function () {
        
            var pop = $(this);
            
            var divConteudo = pop.data("url");
            
            var titulo = pop.data("title");
            
            pop.popover({

                title: (titulo),

                html: true,

                container: 'body',

                content: function () {
                    return $(divConteudo).html();
                }
            });
        });

    }
};

var PopoverCustom = new PopoverCustomClass();

$(document).ready(function () {
    PopoverCustom.init();
});
