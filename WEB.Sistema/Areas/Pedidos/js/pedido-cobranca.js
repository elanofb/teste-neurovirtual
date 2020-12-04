function ClassPedidoCobranca() {

    this.init = function () {

    };

    //Retorno apos enviar dados do parcelamento do pedido
    this.enviarEmailCobranca = function (idPedido) {

        var fYes = function () {

            var url = new String($("#baseUrlGeral").val()).concat("Pedidos/pedidocobranca/enviar-email-cobranca");

            $.post(url, {
                "idPedido": idPedido

            }, function (response) {

                if (response.error == true) {
                    jM.error(response.message);
                    return;
                }

                jM.success(response.message);

            });
        };

        var fNo = function() {
            return false;
        };

        jM.confirmation("Confirma o envio de e-mail com link de pagamento para o comprador?", fYes, fNo);

    }

};

var PedidoCobranca = new ClassPedidoCobranca();

$(document).ready(function () {
    PedidoCobranca.init();
});