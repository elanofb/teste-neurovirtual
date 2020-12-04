function ObjDespesaCadastro() {

    var iconAlterarFlagTotalParcelamento = "<i class=\"fa fa-credit-card fs-12 pull-right margin-bottom-5\" data-toggle=\"tooltip\" title=\"Clique para informar o valor da parcela\" onclick=\"DespesaCadastro.changeInputAmountFull()\"></i>";

    var serializeDadosPagamento = "";
    var serializeDadosPagamentoParcelado = "";

    //Iniciar
    this.init = function () {
        DespesaCadastro.apresentarCamposRepeticao();
        DespesaCadastro.changeCampoModoPagamento();
    };

    //Apresenta os campos e box de acordo o tipo de repetição selecionada
    this.apresentarCamposRepeticao = function () {

        var flagTipoRepeticao = $("#flagTipoRepeticao").val(); //Pega o tipo de repetição

        //Caso a repetição seja 'Nenhuma'
        if (flagTipoRepeticao == 1 || flagTipoRepeticao == "0") {

            $("#flagTipoRepeticao").val(1);
            
            //Altera os botões para deixar visível o que foi selecionado
            
            $(".btn-repeticao-nenhuma").removeClass("btn-default");
            
            $(".btn-repeticao-nenhuma").addClass("btn-success");
        }

        //Caso a repetição senha 'Parcelamento'
        if (flagTipoRepeticao == 2) {

            //Altera os botões para deixar visível o que foi selecionado
            $(".btn-repeticao-parcelamento").removeClass("btn-default");
            $(".btn-repeticao-parcelamento").addClass("btn-success");

            //Ativa o slim scroll da listagem das parcelas
            $("#boxListaParcelamentos").slimScroll({ height: 400, alwaysVisible: false });

            //Captura o flag que verifica se o valor informado é de uma parcela ou o valor total
            var flagValorTotalParcelamento = $("#flagValorTotalParcelamento").val();

            //Caso esta selecionado para inserir o valor total do parcelamento apresenta os campos de acordo
            if (flagValorTotalParcelamento == "S") {
                $(".boxValor").show();
                $(".boxValor>label").html("Valor Total " + iconAlterarFlagTotalParcelamento);
                $(".boxValorParcela").hide();
                return;
            }

            //Por padrão apresenta os campos de acordo para inserir o valor da parcela
            $(".boxValor").hide();
            $(".boxValorParcela").show();
        }
    }

    //Apresenta os campos e box de acordo o tipo de repetição selecionada
    this.carregarCamposRepeticao = function () {

        var flagTipoRepeticao = $("#flagTipoRepeticao").val(); //Pega o tipo de repetição
        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/DespesaCadastro/";
        var dadosSerialize = "";

        $("#boxDadosPagamento").loadingOverlay();

        //Caso a repetição seja 'Nenhuma'
        if (flagTipoRepeticao == 1) {
            serializeDadosPagamentoParcelado = $("#boxDadosPagamento :input").serialize();
            dadosSerialize = serializeDadosPagamento;

            url += "partial-dados-pagamento/";
        }

        //Caso a repetição seja 'Parcelamento'
        if (flagTipoRepeticao == 2) {
            serializeDadosPagamento = $("#boxDadosPagamento :input").serialize();
            dadosSerialize = serializeDadosPagamentoParcelado;

            url += "partial-dados-pagamento-parcelado/";
        }

        $.post(url, dadosSerialize, function(response) {
            $("#boxDadosPagamento").html(response);
            DefaultSistema.iniciarPluginsAposAjax($("#boxDadosPagamento"));

            DespesaCadastro.apresentarCamposRepeticao();
            $("#boxDadosPagamento").loadingOverlay('remove');
        });
    }

    //Apresenta os campos de acordo com a informação que foi escolhida pra ser inserida
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

    //Faz a requisição a controller para apresentar as parcelas
    this.gerarParcelas = function () {

        //Captura os valores para gerar as pacelas
        var valorTotal = $("#valorTotal").val();
        var dtPrimeiroVencimento = $("#dtVencimento").val();
        var parcelas = $("#qtdeRepeticao").val();
        var flagValorTotalParcelamento = $("#flagValorTotalParcelamento").val();
        var valorParcelas = $("#valorParcelas").val();
        var valorMulta = $("#valorMulta").val();
        var valorJuros = $("#valorJuros").val();
        var valorDesconto = $("#valorDesconto").val();
        var flagCompleteDtCompetencia = $("#flagCompleteDtCompetencia").val();
        var dtDespesa = $("#dtDespesa").val();

        //Link da action que gera as parcelas
        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/DespesaCadastro/partial-gerar-despesas-pagamento-form/";

        var box = $("#boxLoadParcelas"); //Pega o box da listagem de parcelamentos

        $("#boxListaParcelamentos").loadingOverlay();//Aplica um efeito de carregamento

        //Faz a requisição para regaras as parcelas
        $.post(url,{
            valorTotal: valorTotal,
            dtPrimeiroVencimento: dtPrimeiroVencimento,
            parcelas: parcelas,
            flagValorTotalParcelamento: flagValorTotalParcelamento,
            valorParcelas: valorParcelas,
            valorMulta: valorMulta,
            valorJuros: valorJuros,
            valorDesconto: valorDesconto,
            flagCompleteDtCompetencia: flagCompleteDtCompetencia,
            dtDespesa : dtDespesa
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

    //Alterna os botões de repetição e executa a função para apresentar os botões de acordo
    this.changeBtnRepeticao = function(element) {
        
        $(".btn-repeticao").removeClass("btn-success");
        
        $(".btn-repeticao").addClass("btn-default");

        $("#flagTipoRepeticao").val($(element).val());

        DespesaCadastro.carregarCamposRepeticao();
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
    
    this.changeCampoModoPagamento = function () {

        var idTipoDespesa  = $("#idTipoDespesa").val();

        var url = $("#baseUrlGeral").val() + "Financeiro/ModoPagamentoDespesaConsulta/listar-modo-pagamento/";

        $.post(url, { idTipoDespesa : idTipoDespesa }, function (response) {

            if (response.error != undefined && response.error == false) {

                var combo = $("#selectModoPagamento");
                var selected = combo.attr("datavalue");
                
                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);

                $.each(response.listaModoPagamento, function (key, item) {
                    if (selected == item.value){
                        combo.append(new Option(item.text, item.value, true));
                    } else{
                        combo.append(new Option(item.text, item.value, false));
                    }
                    
                });

                combo.val(selected);

                DespesaCadastro.verificarModoPagamento();
                
                return;
            }

            jM.error(response.message);

        });
        
    }

    this.verificarModoPagamento = function () {

        var id  = $("#selectModoPagamento").val();
        
        var url = $("#baseUrlGeral").val() + "Financeiro/ModoPagamentoDespesaConsulta/verificar-conta-bancaria/";

        if (id == 9){
            $("#boxNroBoleto").removeClass("hide");
            $("#boxDadosBancarioFavorecido").addClass("hide");
            $("#codigoBoleto").setMask("boleto_bancario");
            return;
        }
        if (id == 12){
            $("#boxNroBoleto").removeClass("hide");
            $("#boxDadosBancarioFavorecido").addClass("hide");
            $("#codigoBoleto").setMask("conta_concessionaria");
            return;
        }

        $("#boxNroBoleto").addClass("hide");

        $.post(url, { id : id }, function (response) {

            if (response == true) {
                $("#boxDadosBancarioFavorecido").removeClass("hide");
                return;
            }

            $("#boxDadosBancarioFavorecido").addClass("hide");

        });

    }
    
};

var DespesaCadastro = new ObjDespesaCadastro();
$(document).ready(function(){
    DespesaCadastro.init();
});
