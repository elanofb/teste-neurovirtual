function PedidoAcaoClass() {

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

        DefaultSistema.iniciarPluginsAposAjax($("#boxModalAcao"));
        
        DefaultSistema.reiniciarBotao();

    }

};

var PedidoAcao = new PedidoAcaoClass();

$(document).ready(function () {
    PedidoAcao.init();
});