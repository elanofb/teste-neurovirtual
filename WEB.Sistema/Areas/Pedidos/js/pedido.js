function ObjPedido() {

    this.init = function () {

        idPessoa = $("#idPessoa").val();

        this.carregarPessoa(idPessoa);
    };

    //Buscar dados da pessoa para autopreencher o formulario
    this.carregarPessoa = function (idPessoa) {

        if (idPessoa === '0') {
            idPessoa = $("#idPessoa").val();
        }

        if (idPessoa === '0' || idPessoa == "") {
            return;
        }
        
        //Buscar dados de associado
        var urlAssociado = new String($("#baseUrlGeral").val()).concat("Comprador/carregar-comprador-associado");
        $.get(urlAssociado,
            { idPessoa: idPessoa },
            function (response) {
                if (response.error == true) {
                    return;
                }
                Pedido.preencherDadosPessoa(response);
            }
        );

        //Buscar dados de nao associado
        var urlNaoAssociado = new String($("#baseUrlGeral").val()).concat("Comprador/carregar-comprador-naoassociado");
        $.get(urlNaoAssociado,
            { idPessoa: idPessoa },
            function (response) {
                if (response.error == true) {
                    return;
                }
                Pedido.preencherDadosPessoa(response);
            }
        );
    }

    //Preencher os dados da pessoa apos busca
    this.preencherDadosPessoa = function (response) {

        $("#cpfCliente").val(response.nroDocumento);

        $("#telPrincipal").val(response.telPrincipal);

        $("#telSecundario").val(response.telSecundario);

        $("#emailPrincipal").val(response.emailPrincipal);

        $("#emailSecundario").val(response.emailSecundario);

        $("#descricaoPerfil").val(response.tipo);

        $("#statusAssociado")
            .addClass( (response.ativo == "S"? "bg-green": "bg-red") )
            .val(response.descricaoStatus);

        if (response.tipo == "Associado") {

            $(".dado-associado").show();

            $("#situacaoAssociado")
                .addClass((response.flagSituacaoContribuicao == "AD" ? "bg-green" : "bg-red"))
                .val(response.descricaoSituacao);

        } else {
            $(".dado-associado").hide();
        }


    }

    //
    this.atualizarValoresPedido = function() {

        var url = $("#baseUrlGeral").val() + 'pedidos/pedido/partial-box-valores';

        $.post(url, {
            
        },
            function (response) {
                $("#boxCondicoesPagamento").html(response);
            }
        );
    }

};

var Pedido = new ObjPedido();

$(document).ready(function () {
    Pedido.init();
});