function ClassPedidoPagamento() {

    var selectorBoxPagamento = "#box-dados-pagamento";

    this.init = function () {

    };


    // Buscar c?lculo de CEP
    this.carregarParcelas = function (idPedido) {


        $(selectorBoxPagamento).loadingOverlay();

        var qtdeParcelas = $("#qtdeParcelas").val();

        var url = new String($("#baseUrlGeral").val()).concat("Pedidos/pedidopagamento/partial-carregar-parcelas");

        $.post(url, {
            "idPedido": idPedido,
            "qtdeParcelas": qtdeParcelas

        }, function (response) {

            console.log(response);

            if (response.error == true) {
                jM.error(response.message);
                return;
            }

            $(selectorBoxPagamento).loadingOverlay('remove');

            $(selectorBoxPagamento).html(response);

            $(selectorBoxPagamento).find("input").setMask();

            iniciarDatePicker();


        });

    };

    //Retorno apos enviar dados do parcelamento do pedido
    this.onSuccessParcelamento = function (response) {

        if (response.error === false) {
            location.reload(true);
            return;
        }

        $(selectorBoxPagamento).find("input").setMask();

        iniciarDatePicker();
    }

};

var PedidoPagamento = new ClassPedidoPagamento();

$(document).ready(function () {
    PedidoPagamento.init();
});