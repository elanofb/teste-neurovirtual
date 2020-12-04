function ObjAvisoNotificacaoPerfil() {

    this.init = function () {
        $(".content-load-perfis").each(function () {
            DefaultSistema.carregarConteudo(
                $(this),
                AvisoNotificacaoPerfil.autoCompletePerfilUsuario
            );
        });
    };

    /**
    * Autocomplete do perfil
    */

    this.autoCompletePerfilUsuario = function () {

        var element = $("#novoPerfilEspecifico"); //<--chama o campo texto para torna-lo um auto complete

        // Função é acionada quando selecionamos um item
        var funcSelection = function (data) {
            $("#idPerfilEspecifico").val(data.id);
            $("#nomePerfilEspecifico").val(data.nome);
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
        AvisoNotificacaoPerfil.autoCompletePerfilUsuario();
    }

};


var AvisoNotificacaoPerfil = new ObjAvisoNotificacaoPerfil();

$(document).ready(function () {
    AvisoNotificacaoPerfil.init();
});
