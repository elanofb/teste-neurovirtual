function PedidoAcaoCancelamentoClass() {

    var baseUrl;
    
    //
    this.init = function () {
        
    };
    
    //
    this.onSuccess = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            location.reload();

            return;

        }

        DefaultSistema.iniciarPluginsAposAjax($("#boxModalCancelamento"));
        
        DefaultSistema.reiniciarBotao();

    }

};

var PedidoAcaoCancelamento = new PedidoAcaoCancelamentoClass();

$(document).ready(function () {
    PedidoAcaoCancelamento.init();
});