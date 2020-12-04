function ObjAvisoNotificacaoUsuario() {

    this.init = function () {
        $(".content-load-usuarios").each(function () {
            DefaultSistema.carregarConteudo(
                $(this),
                AvisoNotificacaoUsuario.autoCompleteNotificacaoUsuario
            );
        });
    };
    
    this.autoCompleteNotificacaoUsuario = function () {

        var element = $("#novoUsuarioEspecifico"); //<--chama o campo texto para torna-lo um auto complete

        // Função é acionada quando selecionamos um item
        var funcSelection = function (data) {
            $("#idUsuarioEspecifico").val(data.id);
            $("#nomeUsuarioEspecifico").val(data.nome);
            $("#loginUsuarioEspecifico").val(data.login);
            $("#emailUsuarioEspecifico").val(data.email);
        };

        //Função é chamada quando carregamos o HTML
        var funcLoad = function (term) {
        };

        // Chamada quando não encontramos um item na busca
        var callbackNotFound = function (term) {
            return "<a href='#' id='newUsuario'>Nenhum registro encontrado para (" + term + ")</a>";

        };

        // Pode ser utilizada para customizar o layout da lista de itens
        var callbackLayout = function (term) {
            //
        };

        AppAutoComplete.url = element.attr("data-url");
        AppAutoComplete.title = element.attr("data-title");
        AppAutoComplete.quantityItems = 3;

        AppAutoComplete.loadSelect2(element, funcSelection, callbackNotFound, "", funcLoad);

    };

    this.retornoAposAdicionar = function () {
        DefaultSistema.iniciarPluginsAposAjax();
        AvisoNotificacao.showHideAbasAjax();
        AvisoNotificacaoUsuario.autoCompleteNotificacaoUsuario();
    }

};


var AvisoNotificacaoUsuario = new ObjAvisoNotificacaoUsuario();

$(document).ready(function () {
    AvisoNotificacaoUsuario.init();
});
