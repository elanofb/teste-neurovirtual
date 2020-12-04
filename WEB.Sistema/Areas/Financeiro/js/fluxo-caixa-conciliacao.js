function FluxoCaixaConciliacaoClass() {

    var baseUrl;
    
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    };

    this.conciliarPagamentos = function () {

        var postData = { "listaPagamentos" : [] };

        $("input[type=checkbox][name='checkPagamento[]']:checked").each(function () {
            postData["listaPagamentos"].push({ id : $(this).val(), flagTipoPagamento : $(this).data("tppgto") });
        });

        if (postData["listaPagamentos"].length == 0) {
            jM.info("Selecione ao menos um pagamento para conciliar.");
            return false;
        }

        var funcYes = function () { 

            var url = FluxoCaixaConciliacao.baseUrl + "Financeiro/FluxoCaixaOperacao/conciliar-pagamentos";

            $.post(url, postData, function () {

                FluxoCaixa.carregarBoxMovimentacaoDiaria();

                jM.success("A concilia&ccedil;&atilde;o foi realizada com sucesso.")

            });

        }

        jM.confirmation("Voc&ecirc; deseja conciliar os pagamentos selecionados?", funcYes, null);

    }

    this.cancelarConciliacao = function (idPagamento, flagTipoPagamento) {
        
        var funcYes = function () {

            var url = FluxoCaixaConciliacao.baseUrl + "Financeiro/FluxoCaixaOperacao/cancelar-conciliacao";

            $.post(url, { id: idPagamento, flagTipoPagamento: flagTipoPagamento }, function () {

                FluxoCaixa.carregarBoxMovimentacaoDiaria();

                jM.success("A concilia&ccedil;&atilde;o foi cancelada com sucesso.")

            });

        }

        jM.confirmation("Voc&ecirc; deseja cancelar a concilia&ccedil;&atilde;o do pagamento selecionado?", funcYes, null);

    }

};

var FluxoCaixaConciliacao = new FluxoCaixaConciliacaoClass();

$(document).ready(function(){
    FluxoCaixaConciliacao.init();
});
