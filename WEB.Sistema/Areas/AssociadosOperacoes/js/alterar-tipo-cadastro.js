function AlterarTipoCadastroClass(){
    
    var urlAlteracaoTipoCadastro;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlAlteracaoTipoCadastro = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoAlterarTipoCadastro/modal-alterar-tipo-cadastro";

    };
    
    //
    this.enviarParaSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }

        $.post(this.urlAlteracaoTipoCadastro, postData, function (response) {

            AlterarTipoCadastro.onSuccessAberturaModal(response);

        });

    }

    //
    this.enviarParaTodos = function () {

        var postData = $(".formFiltro").serialize();

        $.post(this.urlAlteracaoTipoCadastro, postData, function (response) {

            AlterarTipoCadastro.onSuccessAberturaModal(response);

        });

    };

    //
    this.onSuccessAberturaModal = function (response) {

        if (response.error === true) {

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

    };

	//Executado ao submeter formulario de admissao do associado
    this.onSuccessForm = function (response) {

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

            $("#boxFormEnvioSenha").addClass("carregando");

            location.reload();

        }

    };

};

var AlterarTipoCadastro = new AlterarTipoCadastroClass();

$(document).ready(function(){
    AlterarTipoCadastro.init();
});
