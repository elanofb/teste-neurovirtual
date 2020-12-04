function AssociadoTipoAlteracaoClass(){
    
    var urlTipoAlteracao;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlTipoAlteracao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoTipoAlteracao/modal-alterar-tipo";

    };
    
    //
    this.alterarTipoAssociado = function (id) {

        var postData = { 'idsAssociados': [id] };

        $.post(this.urlTipoAlteracao, postData, function (response) {

            AssociadoReativacao.onSuccessAberturaModal(response);

        });

    }

    //
    this.alterarTipoSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }
        
        $.post(this.urlTipoAlteracao, postData, function (response) {

            AssociadoTipoAlteracao.onSuccessAberturaModal(response);

        });

    }

    //
    this.alterarTipoTodos = function () {

        var postData = $(".formFiltro").serialize();
        
        $.post(this.urlTipoAlteracao, postData, function (response) {

            AssociadoTipoAlteracao.onSuccessAberturaModal(response);

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

            DefaultSistema.iniciarPluginsAposAjax($("#boxFormTipoAlteracao"));
            
            $("#boxAssociadosSelecionados").slimScroll({
                height: 225
            });

            $("#boxAssociadosSelecionados").removeClass("hide");

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });

    }

	//Executado ao submeter formulario de Tipo do associado
    this.onSuccessFormTipoAlteracao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormTipoAlteracao"));

        $("#boxAssociadosSelecionados").slimScroll({
            height: 225
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            $("#boxFormTipoAlteracao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoTipoAlteracao = new AssociadoTipoAlteracaoClass();

$(document).ready(function(){
    AssociadoTipoAlteracao.init();
});
