function ConfiguracoesClass() {
    
    //Inicializador de metodos
    this.init = function() {

        this.listenerTipoGeracaoContribuicao();

        this.listenerConfiguracoesBoleto();

        this.listenerConfiguracoesCartaoCredito();

        this.listenerConfiguracoesDepositoBancario();

        this.listenerConfiguracoesPagseguro();

        this.listenerConfiguracoesPaypal();

        this.onChangeCompartilharFacebook();

        this.listenerPopover();

        $(".colorpicker").colorpicker();


    };

    //Iniciar plugin popover
    this.listenerPopover = function() {

        $('.for-popover').each(function () {
            
            var pop = $(this);
            var divConteudo = pop.data("url");
            var titulo = pop.data("title");

            pop.popover({
                title: (titulo),
                html: true,
                container:'body',
                content: function () {
                    return $(divConteudo).html();
                }
            });
            
        });

    };

    //
    this.onChangeCompartilharFacebook = function () {
        var value = $("#flagCompartilharFacebook").val();

        if (value == "S") {
            $("#compartilhar-facebook").show();
        } else {
            $("#compartilhar-facebook").hide();
        }
    };

    //Verificar o tipo de geração de contribuicao no sistema para exibir ou ocultar os campos necessarios
    this.listenerTipoGeracaoContribuicao = function () {
        var boxQtdeDias = $(".box-qtde-dias");
        var elemento = $("#flagTipoGeracaoContribuicao");

        elemento.on("change", function () {
            var value = $(this).val();
            if (value.toString().toLowerCase() == "true") {
                boxQtdeDias.show();
            } else {
                boxQtdeDias.hide();
            }
        });

        elemento.trigger("change");
    };


    // 1 - Verificar se o boleto esta habilitado e exibir as demais configuracoes
    // 2 - Verificar se o combo de configuracao para recebimentos apos o vencimento
    this.listenerConfiguracoesBoleto = function () {

        var boxConfig = $("#boxBoletoConfiguracao");

        var elemento = $("#comboFlagBoleto");

        elemento.on("change", function () {
            var value = $(this).val();

            if (value.toString().toLowerCase() == "true") {
                boxConfig.show();
            } else {
                boxConfig.hide();
            }
        });

        elemento.trigger("change");

        var comboReceberAposVencimento = $("#boletoFlagReceberAposVencimento");
        var seletorCamposReceber = $(".dado-receber-apos-vencimento");
        var seletorCamposNaoReceber = $(".dado-nao-receber-apos-vencimento");

        comboReceberAposVencimento.on("change", function () {
            var value = $(this).val();

            if (value.toString().toLowerCase() == "true") {
                seletorCamposReceber.show();
                seletorCamposNaoReceber.hide();
            } else {
                seletorCamposReceber.hide();
                seletorCamposNaoReceber.show();
            }
        });

        comboReceberAposVencimento.trigger("change");
    };


    //Verificar se o cartao de credito esta habilitado e exibir as demais configuracoes
    this.listenerConfiguracoesCartaoCredito = function () {
        var boxConfig = $("#boxCartaoCreditoConfiguracao");
        var elemento = $("#comboFlagCartaoCredito");

        elemento.on("change", function () {
            var value = $(this).val();
            console.log(value);
            if (value.toString().toLowerCase() == "true") {
                boxConfig.show();
            } else {
                boxConfig.hide();
            }
        });

        elemento.trigger("change");
    };


    //Verificar se o deposito bancario esta habilitado e exibir as demais configuracoes
    this.listenerConfiguracoesDepositoBancario = function () {
        var boxConfig = $("#boxDepositoBancarioConfiguracao");
        var elemento = $("#comboFlagDepositoBancario");

        elemento.on("change", function () {
            var value = $(this).val();
            console.log(value);
            if (value.toString().toLowerCase() == "true") {
                boxConfig.show();
            } else {
                boxConfig.hide();
            }
        });

        elemento.trigger("change");
    };


    //Verificar se o pagseguro esta habilitado e exibir as demais configuracoes
    this.listenerConfiguracoesPagseguro = function () {
        var boxConfig = $("#boxPagseguroConfiguracao");
        var elemento = $("#comboFlagPagseguro");

        elemento.on("change", function () {
            var value = $(this).val();
            console.log(value);
            if (value.toString().toLowerCase() == "true") {
                boxConfig.show();
            } else {
                boxConfig.hide();
            }
        });

        elemento.trigger("change");
    };


    //Verificar se paypal esta habilitado e exibir as demais configuracoes
    this.listenerConfiguracoesPaypal = function () {
        var boxConfig = $("#boxPaypalConfiguracao");
        var elemento = $("#comboFlagPaypal");

        elemento.on("change", function () {
            var value = $(this).val();
            console.log(value);
            if (value.toString().toLowerCase() == "true") {
                boxConfig.show();
            } else {
                boxConfig.hide();
            }
        });

        elemento.trigger("change");
    };

    //Funcao para remover os caches de dados
    this.removerCacheDados = function () {
        var url = new String($("#baseUrlGeral").val()).concat("configuracao/operacoes/limpar-cache-dados");

        $.post(url, {},
            function (response) {
                if (response.error == false) {
                    jM.success(response.message);
                }
            }
        );
    };
}

var Configuracoes = new ConfiguracoesClass();

$(document).ready(function () {

    Configuracoes.init();
});