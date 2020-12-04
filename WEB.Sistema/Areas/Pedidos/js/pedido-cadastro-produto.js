function PedidoCadastroProdutoClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    };

    //Buscar dados da pessoa para autopreencher o formulario
    this.carregarProduto = function (idProduto) {

        if (idProduto === '0') {
            idProduto = $("#idProduto").val();
        }

        if (idProduto === '0' || idProduto == "") {
            return;
        }
        
        //Buscar dados do produto
        var urlAssociado = this.baseUrl + "Produtos/Produto/buscar";

        $.get(urlAssociado, { idProduto: idProduto }, function (response) {

            if (response.error == true) {
                return;
            }

            PedidoCadastroProduto.preencherDadosProduto(response);

        });

    }

    //Preencher os dados da pessoa apos busca
    this.preencherDadosProduto = function(response) {
        
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

        $("#ganhoDario").val(response.valorGanhoDiario);

        $("#qtdeDiasDuracao").val(response.qtdeDiasDuracao);
        
    };

    //Adicionar um produto ao pedido
    this.adicionarProduto = function () {

        var id = $("#idProduto").val();
        
        var valorProduto = $("#valorProduto").val();

        var qtde = $("#qtde").val();

        var observacoes = $("#observacoes").val();

        var url = this.baseUrl + 'Pedidos/PedidoCadastroProduto/adicionar-produto';

        var dados = { idProduto: id, qtde: qtde, valorProduto: valorProduto, observacoes: observacoes };

        $.post(url, dados, function (response) {

            DefaultSistema.reiniciarBotao();

            if (response.error == false) {

                PedidoCadastroProduto.atualizarBoxProdutos();

                return;

            }

            jM.error(response.message);
            
        });
    };

    //Remover produto de um pedido
    this.removerProduto = function (id) {
        
        var funcYes = function () {

            var url = new String($("#baseUrlGeral").val()).concat("Pedidos/PedidoCadastroProduto/remover-produto");

            $.post(url, { idProduto: id }, function () {

                PedidoCadastroProduto.atualizarBoxProdutos();

            })

        }

        jM.textButtonYes = "Sim";
        jM.textButtonNo = "N&atilde;o";
        jM.confirmation("Voc&ecirc; deseja remover o produto do pedido?", funcYes, null);

    };

    //Atualizar o box de produtos ap�s inclus�o ou exclusao de produtos de um pedido
    this.atualizarBoxProdutos = function () {

        var url = new String($("#baseUrlGeral").val()).concat("Pedidos/PedidoCadastroProduto/partial-produtos");

        $.get(url, {}, function (response) {

            $("#data-produtos").html(response);
            
            DefaultSistema.iniciarPluginsAposAjax($("#data-produtos"));

            $("#boxListaProdutos").slimScroll({
                height: 200
            })

            PedidoCadastroFrete.calcularFrete('entrega');

            PedidoCadastro.atualizarValoresPedido();
        });
    };
};

var PedidoCadastroProduto = new PedidoCadastroProdutoClass();

$(document).ready(function () {
    PedidoCadastroProduto.init();
});