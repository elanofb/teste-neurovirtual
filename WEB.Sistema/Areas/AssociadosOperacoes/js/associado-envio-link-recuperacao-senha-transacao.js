function AssociadoEnvioLinkRecuperacaoSenhaTransacaoClass(){
    
    var urlReenvioSenha;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlEnvioLinkRecuperacaoSenhaTransacao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoEnvioLinkRecuperacaoSenhaTransacao/enviar-link-senha-transacao";

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
            
            jM.info("Selecione ao menos um membro.");
            
            return false;
        }
        
        var funcYes = function() {

            $.post(AssociadoEnvioLinkRecuperacaoSenhaTransacao.urlEnvioLinkRecuperacaoSenhaTransacao, postData, function (response) {

                if (response.error) {

                    jM.error(response.message);

                    return;

                }

                jM.success(response.message);

                Notificacao.carregarWidgetsTopo();

            });

        }

        jM.confirmation("Voc&ecirc; deseja enviar o link de recupera&ccedil;&atilde;o de senha para transa&ccedil;&otilde;es para os membro(s) selecionado(s)?", funcYes);

    }

    //
    this.enviarParaTodos = function () {

        var funcYes = function() {

            var postData = $(".formFiltro").serialize();

            $.post(AssociadoEnvioLinkRecuperacaoSenhaTransacao.urlEnvioLinkRecuperacaoSenhaTransacao, postData, function(response) {

                if (response.error) {

                    jM.error(response.message);

                    return;

                }

                jM.success(response.message);

                Notificacao.carregarWidgetsTopo();

            });

        }

        jM.confirmation("Voc&ecirc; deseja enviar o link de recupera&ccedil;&atilde;o de senha para transa&ccedil;&otilde;es para todos os membros da consulta?", funcYes);

    }
    

};

var AssociadoEnvioLinkRecuperacaoSenhaTransacao = new AssociadoEnvioLinkRecuperacaoSenhaTransacaoClass();

$(document).ready(function(){
    AssociadoEnvioLinkRecuperacaoSenhaTransacao.init();
});
