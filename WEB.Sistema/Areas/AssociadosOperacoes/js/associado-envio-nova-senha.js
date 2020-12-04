function AssociadoEnvioNovaSenhaClass(){
    
    var urlEnvioNovaSenha;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlEnvioNovaSenha = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoEnvioNovaSenha/modal-enviar-nova-senha";

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

        $.post(this.urlEnvioNovaSenha, postData, function (response) {

            AssociadoEnvioNovaSenha.onSuccessAberturaModal(response);

        });

    }

    //
    this.enviarParaTodos = function () {

        var postData = $(".formFiltro").serialize();

        $.post(this.urlEnvioNovaSenha, postData, function (response) {

            AssociadoEnvioNovaSenha.onSuccessAberturaModal(response);

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

    //
    this.gerarSenhaAleatoria = function() {

        var url = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoEnvioNovaSenha/gerar-senha-aleatoria";
    
        $.get(url, {}, function(response) {

            $("#formEnvioNovaSenha").find("#novaSenha").val(response.senhaGerada);

        });

    }

};

var AssociadoEnvioNovaSenha = new AssociadoEnvioNovaSenhaClass();

$(document).ready(function(){
    AssociadoEnvioNovaSenha.init();
});
