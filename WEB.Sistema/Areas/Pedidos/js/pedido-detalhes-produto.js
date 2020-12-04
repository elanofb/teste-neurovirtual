function PedidoDetalhesProdutoClass() {

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

            PedidoDetalhesProduto.preencherDadosProduto(response);

        });

    }

    //Preencher os dados da pessoa apos busca
    this.preencherDadosProduto = function(response) {
        
        $("#idItem").val(response.id);

        $("#peso").val(response.pesoFormatado);

        $("#valorProduto").val(response.valorFormatado);
        $("#valorProduto").setMask();

        if (response.flagValorConfiguravel == true){
            $("#valorProduto").removeAttr("readonly");
        }

        if (response.flagValorConfiguravel == false) {
            $("#valorProduto").attr("readonly", "readonly");
        }

    };

    //Adicionar um produto ao pedido
    this.onSuccess = function (response) {
        
        if (response.error == false) {
            
            DefaultSistema.removerModais();

            location.reload();

            return;

        }

        DefaultSistema.iniciarPluginsAposAjax($("#boxModalAdicionarProduto"))
        
    };

    //Remover produto de um pedido
    this.removerProduto = function (id) {
        
        var funcYes = function () {

            var url = new String($("#baseUrlGeral").val()).concat("Pedidos/PedidoDetalhesProduto/remover-produto");

            $.post(url, { id : id }, function (response) {

                if (response.error == false) {

                    location.reload();

                    return;

                }

                jM.error(response.message);

            })

        }

        jM.textButtonYes = "Sim";
        jM.textButtonNo = "N&atilde;o";
        jM.confirmation("Voc&ecirc; deseja remover o produto do pedido?", funcYes, null);

    };

};

var PedidoDetalhesProduto = new PedidoDetalhesProdutoClass();

$(document).ready(function () {
    PedidoDetalhesProduto.init();
});