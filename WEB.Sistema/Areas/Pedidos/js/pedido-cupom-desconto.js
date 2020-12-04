function ClassPedidoCupomDesconto() {

    this.init = function () {

    };

    //Utilizar um cupom de desconto em um pedido
    this.adicionarCupomDesconto = function () {

        var idPessoa = $("#idPessoa").val();

        var cupomDesconto = $("#cupomDesconto").val();

        //Função para retorno da resposta.
        var callback = function (response) {

            if (response.error == false) {

                PedidoProduto.atualizarBoxProdutos();

            } else {
                jM.error(response.message);
            }

        };

        var url = $("#baseUrlGeral").val() + 'pedidos/pedidocupomdesconto/adicionar-cupom-desconto';

        var Assync = new ObjAjax();
        
        Assync.init(url, {"idPessoa": idPessoa, "cupomDesconto": cupomDesconto }, callback, null);
    };

    //
    this.removerCupomDesconto = function () {

        //Função para retorno da resposta.
        var callback = function (response) {

            if (response.error == false) {
                PedidoProduto.atualizarBoxProdutos();
            } else {
                jM.error(response.essage);
            }
        };
        var url = $("#baseUrlGeral").val() + 'pedidos/pedidocupomdesconto/remover-cupom-desconto';

        $.post(url, {}, callback);
    };
};

var PedidoCupomDesconto = new ClassPedidoCupomDesconto();

$(document).ready(function () {
    PedidoCupomDesconto.init();
});