function ClassPedidoAcoes() {

    this.init = function () {

    };

    //Retorno apos enviar dados do parcelamento do pedido
    this.onSuccessAcompanhamento = function(response) {
        console.log(response);
        if (response.error === false) {
            location.reload(true);
            return;
        }

        $(selectorBoxPagamento).find("input").setMask();

        DefaultSistema.reiniciarBotao();
    }

};

var PedidoAcoes = new ClassPedidoAcoes();

$(document).ready(function () {
    PedidoAcoes.init();
});