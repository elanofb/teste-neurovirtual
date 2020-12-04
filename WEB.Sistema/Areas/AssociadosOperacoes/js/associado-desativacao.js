function AssociadoDesativacaoClass(){
    
    var urlDesativacao;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlDesativacao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoDesativacao/modal-desativar-associados";

    };
    
    //
    this.desativarAssociado = function (id) {

        var postData = { 'idsAssociados': [id] };

        $.post(this.urlDesativacao, postData, function (response) {

            AssociadoDesativacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.desativarSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }
        
        $.post(this.urlDesativacao, postData, function (response) {

            AssociadoDesativacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.desativarTodos = function () {

        var postData = $(".formFiltro").serialize();
        
        $.post(this.urlDesativacao, postData, function (response) {

            AssociadoDesativacao.onSuccessAberturaModal(response)

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
                height: 225
            });

            $("#boxAssociadosSelecionados").removeClass("hide");

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });

    }

	//Executado ao submeter formulario de admissao do associado
    this.onSuccessFormDesativacao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormDesativacao"));

        $("#boxAssociadosSelecionados").slimScroll({
            height: 225
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            $("#boxFormDesativacao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoDesativacao = new AssociadoDesativacaoClass();

$(document).ready(function(){
    AssociadoDesativacao.init();
});
