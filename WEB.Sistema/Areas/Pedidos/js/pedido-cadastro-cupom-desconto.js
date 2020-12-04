function PedidoCadastroCupomDescontoClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    };

    //Utilizar um cupom de desconto em um pedido
    this.adicionarCupomDesconto = function () {

        var cupomDesconto = $("#cupomDesconto").val();

        var url = this.baseUrl + 'Pedidos/PedidoCadastroCupomDesconto/adicionar-cupom-desconto';

        $.post(url, { cupomDesconto: cupomDesconto }, function (response) {
            
            if (response.error == false) {

                PedidoCadastroProduto.atualizarBoxProdutos();

                return;
            } 

            jM.error(response.message);
            
        });
        
    };

    //
    this.removerCupomDesconto = function () {

        var url = this.baseUrl + 'Pedidos/PedidoCadastroCupomDesconto/remover-cupom-desconto';

        $.post(url, {}, function (response) {

            if (response.error == false) {

                PedidoCadastroProduto.atualizarBoxProdutos();

                return;
            } 

            jM.error(response.essage);

        });

    };

};

var PedidoCadastroCupomDesconto = new PedidoCadastroCupomDescontoClass();

$(document).ready(function () {
    PedidoCadastroCupomDesconto.init();
});