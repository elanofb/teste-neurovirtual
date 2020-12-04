function PedidoCadastroClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        idPessoa = $("#idPessoa").val();

        this.iniciarAutoCompleteAssociado(idPessoa);

        if (idPessoa > 0){
            this.carregarPessoa(idPessoa);
        }

        $("#boxListaProdutos").slimScroll({
            height: 200
        })

    };

    //
    this.iniciarAutoCompleteAssociado = function () {

        AppAutoComplete.title = "Selecione...";
        AppAutoComplete.url = PedidoCadastro.baseUrl + "Pessoas/PessoaAutoComplete/listar-json-associados-nao-associados";
        AppAutoComplete.quantityItems = 3;

        var combo = $("#idPessoa");

        AppAutoComplete.loadSelect2(combo);

    }

    //Buscar dados da pessoa para autopreencher o formulario
    this.carregarPessoa = function (idPessoa) {
        
        if (idPessoa === '0') {
            idPessoa = $("#idPessoa").val();
        }

        if (idPessoa === '0' || idPessoa == "") {
            return;
        }

        $("#boxDadosComprador").loadingOverlay();
        
        //Buscar dados de associado
        var urlAssociado = this.baseUrl + "Pedidos/Comprador/carregar-comprador-associado";

        $.get(urlAssociado, { idPessoa: idPessoa }, function (response) {

            if (response.error == false) {
                
                PedidoCadastro.preencherDadosPessoa(response);

                $("#boxDadosComprador").loadingOverlay("remove");

                return;

            }

        });

        //Buscar dados de nao associado
        var urlNaoAssociado = this.baseUrl + "Pedidos/Comprador/carregar-comprador-naoassociado";

        $.get(urlNaoAssociado, { idPessoa: idPessoa }, function (response) {

            if (response.error == false) {
                
                PedidoCadastro.preencherDadosPessoa(response);

                $("#boxDadosComprador").loadingOverlay("remove");

            }

        });

    }

    //Preencher os dados da pessoa apos busca
    this.preencherDadosPessoa = function (response) {

        $("#idAssociado").val(response.idAssociado);

        $("#idNaoAssociado").val(response.idNaoAssociado);
        
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

        var url = $("#baseUrlGeral").val() + 'Pedidos/PedidoCadastro/partial-box-valores';

        $.post(url, {}, function (response) {

            $("#boxCondicoesPagamento").html(response);

            DefaultSistema.reiniciarBotao($("#boxCondicoesPagamento"));

        });
    }

    //
    this.onChangeFlagFaturamento = function (elem) {

        var checkbox = $(elem);

        if (!checkbox.is(':checked')) {

            DefaultSistema.limparCampos($("#boxFormFinanceiro"));

        }

    }

};

var PedidoCadastro = new PedidoCadastroClass();

$(document).ready(function () {

    PedidoCadastro.init();

});