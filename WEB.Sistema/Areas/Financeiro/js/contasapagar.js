function ObjContasAPagar() {

    this.init = function () {
        this.carregarReferencias();
        this.iniciarDatepicker();
    };

    this.iniciarDatepicker = function () {
        $("#dtVencimento").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true, "opens": "left" });
    };

    this.atualizarParcelas = function () {

        var qtdeRepeticao = $("#qtdeRepeticao").val();
        var idRepeticaoDespesa = $("#idRepeticaoDespesa").val();
        var valorTotal = $("#valorTotal").val();
        var dtEmissao = $("#dtEmissao").val();

        if (dtEmissao == "") {
            jM.error("Informe a data do primeiro pagamento");
            DefaultSistema.reiniciarBotao();
            return false;
        }

        if (valorTotal == "" || valorTotal == 0) {
            jM.error("Informe o valor da despesa");
            DefaultSistema.reiniciarBotao();
            return false;
        }

        if (qtdeRepeticao == "") {
            jM.error("Informe a quantidade de parcelas");
            DefaultSistema.reiniciarBotao();
            return false;
        }

        var dados = {
            "dtEmissao": dtEmissao,
            "valorTotal": valorTotal,
            "qtdParcelas": qtdeRepeticao,
            "idPeriodoRepeticao": idRepeticaoDespesa
        }

        var callback = function (data) {
            $("#boxListaParcelas").html(data);
            DefaultSistema.reiniciarBotao();
            DefaultSistema.iniciarPlugins();
        }

        var url = $("#baseUrlGeral").val() + 'financeiro/apagarvariado/partial-lista-parcelas';
        var Assync = new ObjAjax();
        Assync.init(url, dados, callback, null, "get", "html");
    }
    
    this.carregarReferencias = function () {

        var idMacroConta = $("#idMacroConta").val();
        
        $("#idBoxReferenciaDespesa").show();

        var combo = $("#idReferenciaDespesa");
        var idReferenciaDespesaSelected = combo.val();

        combo.find("option").remove().end().append(new Option("Carregando...", ""));

        var callback = function (data) {

            if (data.length > 0) {

                combo.find("option").remove().end().append(new Option("Selecione", ""));

                $.each(data, function (key, item) {
                    selected = (item.id == idReferenciaDespesaSelected ? true : false);
                    combo.append(new Option(item.nome, item.id, selected));
                });

                combo.find("option:first").html(Vocabulary.select);
                combo.val(idReferenciaDespesaSelected);
                $("#idBoxReferenciaDespesa").show();

            } else {
                $("#idBoxReferenciaDespesa").hide();
            }
        }

        Ajax.init($("#baseUrlGeral").val() + 'financeiro/relatorioapagar/carregar-referencias-despesas', { "idMacroConta": idMacroConta }, callback, null);
    }
}

var ContasAPagar = new ObjContasAPagar();

$(document).ready(function () {
    ContasAPagar.init();
});