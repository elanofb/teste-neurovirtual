function ObjReceitaCadastro() {

    var iconAlterarFlagTotalParcelamento = "<i class=\"fa fa-credit-card fs-12 pull-right margin-bottom-5\" data-toggle=\"tooltip\" title=\"Clique para informar o valor da parcela\" onclick=\"ReceitaCadastro.changeInputAmountFull()\"></i>";

    var serializeDadosPagamento = "";
    var serializeDadosPagamentoParcelado = "";

    this.init = function () {
        ReceitaCadastro.apresentarCamposRepeticao();
    };

    //Apresenta os campos e box de acordo o tipo de repeti��o selecionada
    this.apresentarCamposRepeticao = function () {

        var flagTipoRepeticao = $("#flagTipoRepeticao").val(); //Pega o tipo de repeti��o

        //Caso a repeti��o seja 'Nenhuma'
        if (flagTipoRepeticao == 1) {
            //Altera os bot�es para deixar vis�vel o que foi selecionado
            $(".btn-repeticao-nenhuma").removeClass("btn-default");
            $(".btn-repeticao-nenhuma").addClass("btn-success");
        }

        //Caso a repeti��o senha 'Parcelamento'
        if (flagTipoRepeticao == 2) {
            
            //Altera os bot�es para deixar vis�vel o que foi selecionado
            $(".btn-repeticao-parcelamento").removeClass("btn-default");
            $(".btn-repeticao-parcelamento").addClass("btn-success");

            //Ativa o slim scroll da listagem das parcelas
            $("#boxListaParcelamentos").slimScroll({ height: 400, alwaysVisible: false });

            //Captura o flag que verifica se o valor informado � de uma parcela ou o valor total
            var flagValorTotalParcelamento = $("#flagValorTotalParcelamento").val();

            //Caso esta selecionado para inserir o valor total do parcelamento apresenta os campos de acordo
            if (flagValorTotalParcelamento == "S") {
                $(".boxValor").show();
                $(".boxValor>label").html("Valor Total " + iconAlterarFlagTotalParcelamento);
                $(".boxValorParcela").hide();
                return;
            }

            //Por padr�o apresenta os campos de acordo para inserir o valor da parcela
            $(".boxValor").hide();
            $(".boxValorParcela").show();
        }
    }

    //Apresenta os campos e box de acordo o tipo de repeti��o selecionada
    this.carregarCamposRepeticao = function () {

        var flagTipoRepeticao = $("#flagTipoRepeticao").val(); //Pega o tipo de repeti��o
        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/ReceitaCadastro/";
        var dadosSerialize = "";

        $("#boxDadosPagamento").loadingOverlay();

        //Caso a repeti��o seja 'Nenhuma'
        if (flagTipoRepeticao == 1) {
            serializeDadosPagamentoParcelado = $("#boxDadosPagamento :input").serialize();
            dadosSerialize = serializeDadosPagamento;

            url += "partial-dados-pagamento/";
        }

        //Caso a repeti��o seja 'Parcelamento'
        if (flagTipoRepeticao == 2) {
            serializeDadosPagamento = $("#boxDadosPagamento :input").serialize();
            dadosSerialize = serializeDadosPagamentoParcelado;

            url += "partial-dados-pagamento-parcelado/";
        }

        $.post(url, dadosSerialize, function (response) {
            $("#boxDadosPagamento").html(response);
            DefaultSistema.iniciarPluginsAposAjax($("#boxDadosPagamento"));

            ReceitaCadastro.apresentarCamposRepeticao();
            $("#boxDadosPagamento").loadingOverlay('remove');
        });
    }

    //Apresenta os campos de acordo com a informa��o que foi escolhida pra ser inserida
    //entre valor da parcela ou valor total do parcelamento
    this.changeInputAmountFull = function () {

        var flagValorTotalParcelamento = $("#flagValorTotalParcelamento").val();

        //Apresenta os campos de acordo para informar o valor da parcela
        if (flagValorTotalParcelamento == "S") {
            $("#flagValorTotalParcelamento").val("N");
            $(".boxValor").hide();
            $(".boxValorParcela").show();
            return;
        }

        //Apresenta os campos de acordo para informar o valor total do parcelamento
        $("#flagValorTotalParcelamento").val("S");
        $(".boxValor").show();
        $(".boxValor label").html("Valor Total " + iconAlterarFlagTotalParcelamento);
        $(".boxValorParcela").hide();
        return;
    }

    //Faz a requisi��o a controller para apresentar as parcelas
    this.gerarParcelas = function () {

        //Captura os valores para gerar as pacelas
        var valorTotal = $("#valorTotal").val();
        var dtPrimeiroVencimento = $("#dtVencimento").val();
        var parcelas = $("#qtdeRepeticao").val();
        var flagValorTotalParcelamento = $("#flagValorTotalParcelamento").val();
        var valorParcelas = $("#valorParcelas").val();
        var flagCompleteDtCompetencia = $("#flagCompleteDtCompetencia").val();
        var dtCompetencia = $("#dtCompetencia").val();

        //Link da action que gera as parcelas
        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/ReceitaCadastro/partial-gerar-receitas-pagamento-form/";

        var box = $("#boxLoadParcelas"); //Pega o box da listagem de parcelamentos

        box.loadingOverlay();//Aplica um efeito de carregamento

        //Faz a requisi��o para regaras as parcelas
        $.post(url,{
            valorTotal: valorTotal,
            dtPrimeiroVencimento: dtPrimeiroVencimento,
            parcelas: parcelas,
            flagValorTotalParcelamento: flagValorTotalParcelamento,
            valorParcelas: valorParcelas,
            flagCompleteDtCompetencia: flagCompleteDtCompetencia,
            dtCompetencia: dtCompetencia
        }, function (response) {
            if (response.error != undefined && response.error == true) {
                jM.error(response.message);
                box.loadingOverlay('remove');
                return;
            }

            box.html(response);
            box.loadingOverlay('remove');
            DefaultSistema.iniciarPluginsAposAjax(box);

            //Ativa o slim scroll da listagem das parcelas
            $("#boxListaParcelamentos").slimScroll({ height: 400, alwaysVisible: false });
        });
    }

    //Alterna os bot�es de repeti��o e executa a fun��o para apresentar os bot�es de acordo
    this.changeBtnRepeticao = function(element) {
        $(".btn-repeticao").removeClass("btn-success");
        $(".btn-repeticao").addClass("btn-default");

        $("#flagTipoRepeticao").val($(element).val());

        ReceitaCadastro.carregarCamposRepeticao();
    }

    this.calcularTotalParcelas = function () {
        var flagValorTotalParcelamento = $("#flagValorTotalParcelamento").val();

        //Apresenta os campos de acordo para informar o valor da parcela
        if (flagValorTotalParcelamento != "S") {
            var valorTotal = 0.00;

            $(".valorParcela").each(function(index) {
                var valorParcela = parseFloat($(this).val().replace('.', '').replace(',', '.'));

                valorTotal = valorTotal + valorParcela;
            });

            var value = valorTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 });

            $(".valorTotalLabel").html("R$"+value)
        }
    }
};

var ReceitaCadastro = new ObjReceitaCadastro();
$(document).ready(function(){
    ReceitaCadastro.init();
});
