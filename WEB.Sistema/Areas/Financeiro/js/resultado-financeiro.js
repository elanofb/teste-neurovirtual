function ResultadoFinanceiroClass() {

    var baseUrl;

    var formFiltros = "#formFiltros";

    var boxResultados = "#boxResultados";

    var boxListaReceitas = "#boxListaReceitas";

    var boxListaDespesas = "#boxListaDespesas";

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    };

    this.filtrar = function () {

        $(boxResultados).loadingOverlay();

        var dadosForm = $(formFiltros).serializeObject();

        var url = $("#baseUrlGeral").val() + "Financeiro/ResultadoFinanceiro/partial-resultados";

        $.post(url, dadosForm, function (response) {

            $(boxResultados).loadingOverlay('remove');

            $(boxResultados).html(response);

            DefaultSistema.reiniciarBotao();

            ResultadoFinanceiro.carregarBoxReceitas("tipo");

            ResultadoFinanceiro.carregarBoxDespesas("cc");

        });

    }

    this.carregarBoxReceitas = function (tipoResultado) {

        $(boxListaReceitas).addClass("carregando");

        var url = this.baseUrl.concat("Financeiro/ResultadoFinanceiroPagamentos/partial-carregar-pagamentos");

        var dadosForm = $(formFiltros).serializeObject();
        dadosForm["flagTipoTitulo"] = "R";
        dadosForm["tipoResultado"] = tipoResultado;

        $.post(url, dadosForm, function (response) {

            $(boxListaReceitas).removeClass("carregando");

            $(boxListaReceitas).html(response);

        })

    }

    this.carregarBoxDespesas = function (tipoResultado) {

        $(boxListaDespesas).addClass("carregando");

        var url = this.baseUrl.concat("Financeiro/ResultadoFinanceiroPagamentos/partial-carregar-pagamentos");

        var dadosForm = $(formFiltros).serializeObject();
        dadosForm["flagTipoTitulo"] = "D";
        dadosForm["tipoResultado"] = tipoResultado;

        $.post(url, dadosForm, function (response) {

            $(boxListaDespesas).removeClass("carregando");

            $(boxListaDespesas).html(response);

        })

    }

};

var ResultadoFinanceiro = new ResultadoFinanceiroClass();

$(document).ready(function(){
    ResultadoFinanceiro.init();
});
