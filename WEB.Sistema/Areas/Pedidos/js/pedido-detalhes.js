function PedidoDetalhesClass() {
    
    this.init = function () {

        this.iniciarBoxItensPedido();

        this.iniciarBoxHistoricoPedido();

    };

    this.iniciarBoxItensPedido = function () {

        DefaultSistema.carregarConteudo($("#boxItensPedido"), function () {

            $("#boxListaProdutosPedido").slimScroll({

                height: 320,

            })

        });

    }

    this.iniciarBoxHistoricoPedido = function () {

        DefaultSistema.carregarConteudo($("#boxHistoricoPedido"), function () {

            $("#boxListaHistoricoPedido").slimScroll({

                height: 320,

            })

        });

    }

};

var PedidoDetalhes = new PedidoDetalhesClass();

$(document).ready(function () {
    PedidoDetalhes.init();
});