function PedidoCadastroFreteClass() {

    var baseUrl;

    // Varíavel responsável por armazenar os dados do endereço localizado através da consulta por cep
    var dadosEndereco;

    // Váriável responsável por auxiliar a função de cálculo de frete, se for TRUE, o sistema irá atualizar 
    // o endereço do pedido temporário.
    this.flagAtualizarEndereco;

    //
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        this.dadosEndereco = {};

        this.flagAtualizarEndereco = false;

        this.carregarEndereco('EnderecoEntrega', false);

    };

    //
    this.carregarEndereco = function (sufixo, flagAtualizarEndereco) {

        var campoCep = $("input[rel=cep" + sufixo + "]").val();

        if (!campoCep) {
            return;
        }

        this.flagAtualizarEndereco = flagAtualizarEndereco;

        var fnSuccessEndereco = function (response) {

            PedidoCadastroFrete.dadosEndereco = response;

            var nomeCidadeUF = (PedidoCadastroFrete.dadosEndereco.nomeCidade + "/" + PedidoCadastroFrete.dadosEndereco.siglaEstado);
            $("#cidade-entrega").val(nomeCidadeUF);

            var campoIdEstado = $("input[rel=idEstado" + sufixo + "]");
            campoIdEstado.val(PedidoCadastroFrete.dadosEndereco.idEstado);

        }

        Localizacao.carregarEndereco(sufixo, fnSuccessEndereco);
        
    }

    // 
    this.calcularFrete = function () {
        
        if (typeof (this.dadosEndereco.cepOriginal) == 'undefined') {

            DefaultSistema.reiniciarBotao();

            return;
        }

        if (!this.flagAtualizarEndereco) {

            DefaultSistema.reiniciarBotao();

            PedidoCadastroFrete.buscarFrete("entrega");

            return;

        }
        
        this.dadosEndereco.logradouro = (this.dadosEndereco.tipoLogradouro + " " + this.dadosEndereco.logradouro);

        this.dadosEndereco.cepOrigem = $("#cep-origem-entrega").val();

        this.dadosEndereco.cep = this.dadosEndereco.cepOriginal;

        this.dadosEndereco.bairro = this.dadosEndereco.bairroIni;

        this.dadosEndereco.numero = $("#numero-entrega").val();

        this.dadosEndereco.complemento = $("#complemento-entrega").val();

        var url = this.baseUrl + "Pedidos/PedidoCadastroFrete/salvar-endereco-entrega";

        $.post(url, this.dadosEndereco, function () {

            PedidoCadastroFrete.flagAtualizarEndereco = false;

            DefaultSistema.reiniciarBotao();

            PedidoCadastroFrete.buscarFrete("entrega");

        })

    }

	// Buscar cálculo de CEP
    this.buscarFrete = function (sufixo) {

        var selectorBoxEntrega = "#boxEntrega";

        $(selectorBoxEntrega).loadingOverlay();
    	
        var url = this.baseUrl + "Pedidos/PedidoCadastroFrete/buscarFrete";

        $.post(url, {}, function (response) {
            
            $(selectorBoxEntrega).loadingOverlay('remove');

            if (response.errorSemCep == true) {
                return;
            }

            if (response.error) {

                jM.error(response.message);

                $("#label-total-" + sufixo).html("R$ 0,00");

                PedidoCadastro.atualizarValoresPedido();

                return;

            }

            if (typeof(response.codigoServico) == 'undefined') {
                
                jM.error("A consulta de frete n&atilde;o retornou nenhum servi&ccedil; dispon&iacute;vel para envio.");

                return;
            }

            $("#label-total-" + sufixo).html(response.valorEntregaFormatado);
            
            PedidoCadastro.atualizarValoresPedido();
            
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

var PedidoCadastroFrete = new PedidoCadastroFreteClass();

$(document).ready(function () {
    PedidoCadastroFrete.init();
});