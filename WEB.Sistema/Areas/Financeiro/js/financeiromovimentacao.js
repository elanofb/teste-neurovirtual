function objFinanceiroMovimentacao() {

    this.init = function () {
        this.repeticaoPeriodo();
    };

    this.repeticaoPeriodo = function () {

        alert("oi");

        $("#dtAcabar").hide();
        $("#qtdeParcela").hide();

        $("#ciclo").on("change", function () {
            if ($(this).val() != "") {
                $("#dtAcabar").show();
                return;
            }

            $("#dtAcabar").hide();
        });

        $("input[name='dtFim']").on("click", function () {

            if ($(this).val() == "S") {
                $("#qtdeParcela").show();
                return;
            }
            $("#qtdeParcela").hide();
        });
    };

};

var FinanceiroMovimentacao = new objFinanceiroMovimentoacao();

$(document).ready(function() {
    FinanceiroMovimentacao.init();
});