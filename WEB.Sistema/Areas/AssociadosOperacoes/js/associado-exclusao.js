function AssociadoExclusaoClass(){
    
    var urlExclusao;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlExclusao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoExclusao/modal-excluir-associados";

    };
    
    //
    this.excluirAssociado = function (id) {

        var postData = { 'idsAssociados': [id] };

        $.post(this.urlExclusao, postData, function (response) {

            AssociadoReativacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.excluirSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }

        $.post(this.urlExclusao, postData, function (response) {

            AssociadoExclusao.onSuccessAberturaModal(response)

        });

    }

    //
    this.excluirTodos = function () {

        var postData = $(".formFiltro").serialize();

        $.post(this.urlExclusao, postData, function (response) {

            AssociadoExclusao.onSuccessAberturaModal(response)

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
    this.onSuccessFormExclusao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormExclusao"));

        $("#boxAssociadosSelecionados").slimScroll({
            height: 225
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            $("#boxFormExclusao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoExclusao = new AssociadoExclusaoClass();

$(document).ready(function(){
    AssociadoExclusao.init();
});
