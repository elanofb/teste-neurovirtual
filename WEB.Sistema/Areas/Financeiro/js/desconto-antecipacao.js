function DescontoAntecipacaoClass() {

    this.init = function () {
    }


    //Ouvinte do combo de tipos de documentos
    this.exibirModalDesconto = function (elemento) {

        var urlConteudo = $(elemento).data("url");

        $(elemento).webuiPopover({
            animation: 'pop',
            title:'Descontos',
            type: 'async',
            url: urlConteudo,
            width: '400',
            closeable: true,
            async: {
                type:'GET', // ajax request method type, default is GET
                //before: function(that, xhr, settings) {},//executed before ajax request
                //success: function(that, data) {}//executed after successful ajax request
                //error: function(that, xhr, data) {} //executed after error ajax request
            }
        });

    }

}

var DescontoAntecipacao = new DescontoAntecipacaoClass();

$(document).ready(function () {

    DescontoAntecipacao.init();
});