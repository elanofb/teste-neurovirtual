function ObjAvisoNotificacao() {

    this.init = function () {

        this.autoCompleteNotificacao();
        this.autoCompleteNotificacaoUsuario();
        this.autoCompletePerfilUsuario();
        this.showHideAbas();
        this.showHideAbasAjax();
        this.showHideAbasNotificados();
    };

    this.showHideAbasNotificados = function () {

        if ($("#flagTipoEnvioAssociado").val() == "ESP") $(".tabAssociadoNotificado").show();

        if ($("#flagTipoEnvioUsuario").val() == "USU" || $("#flagTipoEnvioUsuario").val() == "PER") {
            $(".tabUsuarioNotificado").show();
        }
        
    }

    this.showHideAbasAjax = function () {

        if ($("#flagTipoEnvioAssociado").val() == "ESP") $(".tabAssociadosEspecificos").show();
        if ($("#flagTipoEnvioUsuario").val() == "USU") $(".tabUsuariosEspecificos").show();
        if ($("#flagTipoEnvioUsuario").val() == "PER") $(".tabPerfisEspecificos").show();
    }
    /**
    * Exibir ocultar abas
    */

    this.showHideAbas = function () {

        $(".tabAssociadosEspecificos").hide();
        $(".tabUsuariosEspecificos").hide();
        $(".tabPerfisEspecificos").hide();

        $("#flagTipoEnvioAssociado").on("change", function () {

            if ($(this).val() == "ESP") {
                $(".tabAssociadosEspecificos").show();
                return;
            }

            $(".tabAssociadosEspecificos").hide();
        });

        $("#flagTipoEnvioUsuario").on("change", function () {

                if ($(this).val() == "USU") {
                    $(".tabUsuariosEspecificos").show();
                }
                else { $(".tabUsuariosEspecificos").hide();}

                if ($(this).val() == "PER") {
                    $(".tabPerfisEspecificos").show();
                }
                else { $(".tabPerfisEspecificos").hide();}
            
        });

    }


    /**
	* Autocompletar lista de associados
	*/
    this.autoCompleteNotificacao = function () {

        var element = $("#novoAssociadoEspecifico"); //<--chama o campo texto para torna-lo um auto complete

        // Função é acionada quando selecionamos um item
        var funcSelection = function (data) {
            $("#idAssociadoEspecifico").val(data.id)
            $("#nomeAssociadoEspecifico").val(data.value)
            $("#cnpfAssociadoEspecifico").val(data.cnpf)
        };

        //Função é chamada quando carregamos o HTML
        var funcLoad = function (term) {
        };

        // Chamada quando não encontramos um item na busca
        var callbackNotFound = function (term) {
            return "<a href='#' id='newAssociado'>Nenhum registro encontrado para (" + term + ")</a>";

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

};


var AvisoNotificacao = new ObjAvisoNotificacao();

$(document).ready(function () {
    AvisoNotificacao.init();
});
