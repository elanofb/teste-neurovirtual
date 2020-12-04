function ClassPedidoProduto() {

    this.init = function () {

    };

    //Buscar dados da pessoa para autopreencher o formulario
    this.carregarProduto = function (idProduto) {

        if (idProduto === '0') {
            idProduto = $("#idProduto").val();
        }

        if (idProduto === '0' || idProduto == "") {
            return;
        }
        
        //Buscar dados de associado
        var urlAssociado = new String($("#baseUrlGeral").val()).concat("Produtos/Produto/buscar");
        $.get(urlAssociado,
            { idProduto: idProduto },
            function (response) {
                if (response.error == true) {
                    return;
                }
                console.log(response);
                PedidoProduto.preencherDadosProduto(response);
            }
        );

    }

    //Preencher os dados da pessoa apos busca
    this.preencherDadosProduto = function(response) {
        console.log(response);
        $("#idItem").val(response.id);

        $("#peso").val(response.pesoFormatado);

        $("#valorProduto").val(response.valorFormatado);
        $("#valorProduto").setMask();

        if (response.flagValorConfiguravel == true){
            $("#valorProduto").removeAttr("disabled");
        }

        if (response.flagValorConfiguravel == false) {
            $("#valorProduto").attr("disabled", "disabled");
        }

    };

    //Adicionar um produto ao pedido
    this.adicionarProduto = function () {

        var id = $("#idProduto").val();

        var idPessoa = $("#idPessoa").val();

        var valorProduto = $("#valorProduto").val();

        var qtde = $("#qtde").val();

        var url = $("#baseUrlGeral").val() + 'pedidos/pedidoproduto/adicionar-produto';

        var dados = { idProduto: id, idPessoa: idPessoa, qtde: qtde, valorProduto: valorProduto };

        $.post(url, dados, function (response) {

            if (response.error == false) {

                PedidoProduto.atualizarBoxProdutos();
                return;

            }

            jM.error(response.message);
            
        });
    };

    //Remover produto de um pedido
    this.removerProduto = function (id) {
        var id = id;

        //Função para retorno da resposta.
        var callback = function (response) {
            PedidoProduto.atualizarBoxProdutos();
        };

        var url = new String($("#baseUrlGeral").val()).concat("pedidos/pedidoproduto/remover-produto");

        var Assync = new ObjAjax();

        Assync.init(url, { "idProduto": id }, callback, null);
    };

    //Atualizar o box de produtos após inclusão ou exclusao de produtos de um pedido
    this.atualizarBoxProdutos = function () {
        var url = new String($("#baseUrlGeral").val()).concat("Pedidos/pedidoproduto/partial-produtos");

        $.get(url, {}, function (response) {

            $("#data-produtos").html(response);

            $("#data-produtos").find("input:text").setMask();

            PedidoFrete.calcularFrete('entrega');

            Pedido.atualizarValoresPedido();
        });
    };
};

var PedidoProduto = new ClassPedidoProduto();

$(document).ready(function () {
    PedidoProduto.init();
});