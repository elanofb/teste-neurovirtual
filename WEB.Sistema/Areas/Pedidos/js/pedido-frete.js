function ClassPedidoFrete() {
    var cepAtual = "";
    var selectorBoxEntrega = "#boxEntrega";

    this.init = function () {

        this.calcularFrete('entrega');

    };


	// Buscar cálculo de CEP
    this.calcularFrete = function (sufixo) {

        $(selectorBoxEntrega).loadingOverlay();
    	var cep = $("#cep-" + sufixo).val();

    	if (cep.length != 9) {

            PedidoFrete.zerarFrete(sufixo);

            $(selectorBoxEntrega).loadingOverlay('remove');

            return;
        }

    	var url = new String($("#baseUrlGeral").val()).concat("Pedidos/pedidofrete/buscarFrete");

        $.post(url, {
            "cepDestino": cep

        }, function (response) {

            console.log(response);

            if (typeof (response.codigoServico) == 'undefined') {

                PedidoFrete.zerarFrete(sufixo);

                $(selectorBoxEntrega).loadingOverlay('remove');

                return;
            }

            var nomeLogradouro = (response.tipoLogradouro + " " + response.logradouro);
            var nomeCidadeUF = (response.nomeCidade + "/" + response.siglaEstado);
            $("#logradouro-" + sufixo).val(nomeLogradouro);
            $("#bairro-" + sufixo).val(response.bairro);
            $("#cidade-" + sufixo).val(nomeCidadeUF);
            $("#PedidoEntrega_idEstado").val(response.idEstado);
            $("#PedidoEntrega_idCidade").val(response.idCidade);
            $("#PedidoEntrega_nomeCidade").val(nomeCidadeUF);
            $("#label-total-" + sufixo).html(response.valorEntregaFormatado);

            if (PedidoFrete.cepAtual != response.cepOriginal) {
                $("#numero-" + sufixo).val('');
                $("#complemento-" + sufixo).val('');
            }

            Pedido.atualizarValoresPedido();

            $(selectorBoxEntrega).loadingOverlay('remove');
        });
		
    };

    //
    this.zerarFrete = function(sufixo) {
        $("#logradouro-" + sufixo).val('');
        $("#bairro-" + sufixo).val('');
        $("#cidade-" + sufixo).val('');
        $("#label-total-" + sufixo).html('0.00');
    }

};

var PedidoFrete = new ClassPedidoFrete();

$(document).ready(function () {
    PedidoFrete.init();
});