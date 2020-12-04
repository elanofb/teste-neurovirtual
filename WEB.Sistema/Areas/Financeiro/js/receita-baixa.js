function ReceitaBaixaClass() {

    var idMeioPagamentoBoleto = "1";
    var idMeioPagamentoDinheiro = "4";
    var idMeioPagamentoCheque = "3";
    var idMeioPagamentoDeposito = "2";
    var idMeioPagamentoDebito = "5";
    var idMeioPagamentoCredito = "6";
    var idMeioPagamentoTransferencia = "7";
    var idMeioPagamentoPagSeguro = "8";
    var idMeioPagamentoGuia = "100";
    var idMeioPagamentoDebitoConta = "101";

    var classeBoxBoleto = ".dados-boleto-bancario";
    var classeBoxCartao = ".dados-cartao";
    var classeBoxCheque = ".dados-cheque";
    var classeBoxDocumento = ".dados-documento";
    var classeLabelDocumento = ".label-documento";

    this.init = function () {
    };

    //Carregamento da lista de documentos existentes
    this.modalRegistrarPagamento = function (elemento, pagina) {

        var postData = { 'id': [] };
        var id = $(elemento).data("id");;

        if (id > 0) {
            postData["id"].push(id);
        } else {
            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () { postData["id"].push($(this).val()); });
        }

        if (postData["id"].length == 0) {
            jM.info("Informe ao menos um pagamento.");
            return false;
        }

        var url = $(elemento).data("url");

        this.abrirModalPagamento(url, postData);
    };

    this.abrirModalPagamento = function (url, postData) {

        $.post(url, postData, function(response) {
            
            var Modal = $(response).modal();

            Modal.on("shown.bs.modal", function (e) {
                DefaultSistema.reiniciarBotao();
                DefaultSistema.iniciarPlugins($("#boxRegistrarPagamentoForm"));
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();

                window.location.reload();
            });
            
        });
        
    }
    
    //Apos formulario de registro de pagamento da anuidade ser enviado
    this.onSuccessPagamentoForm = function (response) {

        var boxPagamento = $("#boxRegistrarPagamentoForm");

        boxPagamento.find("input:text").setMask();

        DefaultSistema.iniciarBotoes(boxPagamento);

        DefaultSistema.zerarErros();

        DefaultSistema.iniciarPluginsAposAjax();

        ReceitaBaixa.showCamposAdicionais($("#idMeioPagamento"));
    };



    //Exibir esconder campos adicionais conforme op��o de pagamento escolhida
    // Uso 1: Na funcao "listenerOnChangeFormaPagamento" desse mesmo arquivo
    this.showCamposAdicionais = function (element) {

        var idMeioPagamento = $(element).val();

        $(classeBoxBoleto).hide();
        $(classeBoxCartao).hide();
        $(classeBoxCheque).hide();
        $(classeBoxDocumento).hide();

        if (idMeioPagamento === idMeioPagamentoBoleto) {
            $(classeBoxBoleto).show();
            $(classeLabelDocumento).html("<i class=\"fa fa-asterisk label-required\"></i> N&ordm; Boleto");
            $(classeBoxDocumento).show();
            return;
        }

        if (idMeioPagamento === idMeioPagamentoCheque) {
            $(classeBoxCheque).show();
            $(classeLabelDocumento).html("<i class=\"fa fa-asterisk label-required\"></i> N&ordm; Cheque");
            $(classeBoxDocumento).show();
            return;
        }

        if (idMeioPagamento === idMeioPagamentoDeposito || idMeioPagamento === idMeioPagamentoPagSeguro || idMeioPagamento === idMeioPagamentoDinheiro || idMeioPagamento === idMeioPagamentoTransferencia || idMeioPagamento === idMeioPagamentoGuia || idMeioPagamento === idMeioPagamentoDebitoConta) {
            return;
        }

        if (idMeioPagamento > 0) {

            this.carregarFormasPagamento(idMeioPagamento);

            $(classeBoxCartao).show();

            return;
        }

    };

    //Carregamento do combo com as formas de pagamento de acordo com o meio de pagamento
    this.carregarFormasPagamento = function (idMeioPagamento) {

        var comboForma = $("#idFormaPagamento");

        var url = $("#baseUrlGeral").val() + 'financeiro/formapagamento/buscar-formas-pagamentos';

        $.post(url, {
            idMeioPagamento: idMeioPagamento
        }, function (response) {

            if (response.error == true) {
                jM.error("Nenhuma forma de pagamento foi localizada.");
                return false;
            }

            comboForma.find("option").not("option:first").remove();
            comboForma.find("option:first").html(Vocabulary.loading);
            comboForma.find("option:first").removeAttr("selected");

            $.each(response.listaResultados, function (key, item) {
                selected = false;
                comboForma.append(new Option(item.descricao, item.id, selected));

            });
            comboForma.find("option:first").html("Selecione...");
        });
    };



};

var ReceitaBaixa = new ReceitaBaixaClass();

$(document).ready(function(){
    ReceitaBaixa.init();
});
