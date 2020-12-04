function ObjExtrato() {

    var idFormBusca = "#formBusca";

    this.init = function () {
    }

    this.alterarConciliacao = function (id, tipoMovimentacao, element) {

        var url = new String($("#baseUrlGeral").val()).concat("financeiro/extrato/alterar-conciliacao/");
        var dados = { "id": id, "tipoMovimentacao" : tipoMovimentacao, "flagConciliado": $(element).val() }

        var func = function () {
        };

        Ajax.init(url, dados, func);
    };
    
    this.carregarExcel = function () {

        var dados = $(idFormBusca).serialize();

        var url = $("#baseUrlGeral").val().concat("financeiro/extrato/exportar-excel?" + dados);

        location.href = (url);
    }

    this.carregarImpressao = function () {

        var dados = $(idFormBusca).serialize();

        var url = $("#baseUrlGeral").val().concat("financeiro/extratoimpressao/imprimir?" + dados);

        window.open(url);
    }
}

var Extrato = new ObjExtrato();

$(document).ready(function () {
    Extrato.init();
});