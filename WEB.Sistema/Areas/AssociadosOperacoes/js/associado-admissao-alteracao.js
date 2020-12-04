function AssociadoAdmissaoAlteracaoClass(){
    
    var urlAdmissaoAlteracao;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlAdmissaoAlteracao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoAdmissaoAlteracao/modal-alterar-admissao";

    };
    
    //
    this.alterarAdmissaoAssociado = function (id) {

        var postData = { 'idsAssociados': [id] };

        $.post(this.urlAdmissaoAlteracao, postData, function (response) {

            AssociadoReativacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.alterarAdmissaoSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }
        
        $.post(this.urlAdmissaoAlteracao, postData, function (response) {

            AssociadoAdmissaoAlteracao.onSuccessAberturaModal(response)

        });

    }

    //
    this.alterarAdmissaoTodos = function () {

        var postData = $(".formFiltro").serialize();
        
        $.post(this.urlAdmissaoAlteracao, postData, function (response) {

            AssociadoAdmissaoAlteracao.onSuccessAberturaModal(response)

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

            DefaultSistema.iniciarPluginsAposAjax($("#boxFormAdmissaoAlteracao"));
            
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
    this.onSuccessFormAdmissaoAlteracao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormAdmissaoAlteracao"));

        $("#boxAssociadosSelecionados").slimScroll({
            height: 225
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            $("#boxFormAdmissaoAlteracao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoAdmissaoAlteracao = new AssociadoAdmissaoAlteracaoClass();

$(document).ready(function(){
    AssociadoAdmissaoAlteracao.init();
});
