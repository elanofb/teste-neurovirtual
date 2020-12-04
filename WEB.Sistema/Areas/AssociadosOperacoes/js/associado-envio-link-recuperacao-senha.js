function AssociadoEnvioLinkRecuperacaoSenhaClass(){
    
    var urlReenvioSenha;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlEnvioLinkRecuperacaoSenha = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoEnvioLinkRecuperacaoSenha/enviar-link-senha";

    };
      
    //
    this.enviarParaSelecionados = function (id) {

        var postData = { 'idsAssociados': [] };

        if (id > 0) {

            postData["idsAssociados"].push(id);
            
        } else {

            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
                postData["idsAssociados"].push($(this).val());
            });
        }

        if (postData["idsAssociados"].length == 0) {
            
            jM.info("Selecione ao menos um associado.");
            
            return false;
        }
        
        var funcYes = function() {

            $.post(AssociadoEnvioLinkRecuperacaoSenha.urlEnvioLinkRecuperacaoSenha, postData, function (response) {

                if (response.error) {

                    jM.error(response.message);

                    return;

                }

                jM.success(response.message);

                Notificacao.carregarWidgetsTopo();

            });

        }

        jM.confirmation("Voc&ecirc; deseja enviar o link de recupera&ccedil;&atilde;o de senha para os membro(s) selecionado(s)?", funcYes);

    }

    //
    this.enviarParaTodos = function () {

        var funcYes = function() {

            var postData = $(".formFiltro").serialize();

            $.post(AssociadoEnvioLinkRecuperacaoSenha.urlEnvioLinkRecuperacaoSenha, postData, function(response) {

                if (response.error) {

                    jM.error(response.message);

                    return;

                }

                jM.success(response.message);

                Notificacao.carregarWidgetsTopo();

            });

        }

        jM.confirmation("Voc&ecirc; deseja enviar o link de recupera&ccedil;&atilde;o de senha para todos os membro da consulta?", funcYes);

    }
    

};

var AssociadoEnvioLinkRecuperacaoSenha = new AssociadoEnvioLinkRecuperacaoSenhaClass();

$(document).ready(function(){
    AssociadoEnvioLinkRecuperacaoSenha.init();
});
