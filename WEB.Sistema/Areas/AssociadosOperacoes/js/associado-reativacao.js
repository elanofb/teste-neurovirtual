function AssociadoReativacaoClass(){
    

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlReativacao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoReativacao/modal-reativar-associados";

    };

    //
    this.reativarAssociado = function (id) {

        var postData = { 'idsAssociados': [ id ] };

        $.post(this.urlReativacao, postData, function (response) {

            AssociadoReativacao.onSuccessAberturaModal(response)

        });

    }
    
    //
    this.reativarSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }

        $.post(this.urlReativacao, postData, function (response) {

            AssociadoReativacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.reativarTodos = function () {

        var postData = $(".formFiltro").serialize();
        
        $.post(this.urlReativacao, postData, function (response) {

            AssociadoReativacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.onSuccessAberturaModal = function (response) {

        if (response.error == true) {

            jM.error(response.message);

            return;

        }

        var Modal = $(response).modal();

        $(Modal).on("shown.bs.modal", function (e) {

            DefaultSistema.reiniciarBotao();

            $('input:text').setMask();

            $("#boxAssociadosSelecionados").slimScroll({
                height: 196
            });

            $("#boxAssociadosSelecionados").removeClass("hide");

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });

    }

	//Executado ao submeter formulario de admissao do associado
    this.onSuccessFormReativacao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormReativacao"));

        $("#boxAssociadosSelecionados").slimScroll({
            height: 196
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            $("#boxFormReativacao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoReativacao = new AssociadoReativacaoClass();

$(document).ready(function(){
    AssociadoReativacao.init();
});
