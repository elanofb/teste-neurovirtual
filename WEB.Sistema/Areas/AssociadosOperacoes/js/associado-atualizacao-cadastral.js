function AssociadoAtualizacaoCadastralClass(){
    
    var urlEnvioLinkAlteracao;

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
        this.urlEnvioLinkAlteracao = $("#baseUrlGeral").val() + "AssociadosOperacoes/AssociadoAtualizacaoCadastral/enviar-link-alteracao";

    };
    
    //
    this.enviarLinkSelecionados = function () {

        var postData = { 'idsAssociados': [] };
        
        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });
            
        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }
        
        $.post(this.urlEnvioLinkAlteracao, postData, function (response) {
        
            if (response.error){

                jM.error(response.message);
                return;
            }

            jM.success(response.message);

        });

    };
    
    //
    this.enviarLinkTodos = function () {
    
        var postData = $(".formFiltro").serialize();
        
        $.post(this.urlEnvioLinkAlteracao, postData, function (response) {
            
            if (response.error){
                
                jM.error(response.message);
                return;
            }
            
            jM.success(response.message);
            
        });

    }

};

var AssociadoAtualizacaoCadastral = new AssociadoAtualizacaoCadastralClass();

$(document).ready(function(){
    AssociadoAtualizacaoCadastral.init();
});
