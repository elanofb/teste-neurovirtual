function ObjContasAReceber() {

    this.init = function () {
        this.carregarReferencias();
        this.iniciarDatepicker();
    };

    this.iniciarDatepicker = function () {
        $("#dtVencimento").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true, "opens": "left" });
    };

    this.atualizarParcelas = function () {

        var qtdeRepeticao = $("#qtdeRepeticao").val();
        var idRepeticaoReceita = $("#idRepeticaoReceita").val();
        var valorTotal = $("#valorTotal").val();
        var dtEmissao = $("#dtEmissao").val();

        if (dtEmissao == "") {
            jM.error("Informe a data do primeiro recebimento");
            DefaultSistema.reiniciarBotao();
            return false;
        }

        if (valorTotal == "" || valorTotal == 0) {
            jM.error("Informe o valor da receita");
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
            "idPeriodoRepeticao": idRepeticaoReceita
        }

        var callback = function (data) {
            $("#boxListaParcelas").html(data);
            DefaultSistema.reiniciarBotao();
            DefaultSistema.iniciarPlugins();
        }

        var url = $("#baseUrlGeral").val() + 'financeiro/arecebervariado/partial-lista-parcelas';
        var Assync = new ObjAjax();
        Assync.init(url, dados, callback, null, "get", "html");
    }

    this.carregarReferencias = function () {

        var idTipoReceita = $("#idTipoReceita").val();

        var combo = $("#idDespesa");
        var idReferenciaReceitaSelected = combo.val();

        combo.find("option").remove().end().append(new Option("Carregando...", ""));

        var callback = function (data) {

            if (data.length > 0) {

                combo.find("option").remove().end().append(new Option("Selecione", ""));

                $.each(data, function (key, item) {
                    selected = (item.id == idReferenciaReceitaSelected ? true : false);
                    combo.append(new Option(item.nome, item.id, selected));
                });

                combo.find("option:first").html(Vocabulary.select);
                combo.val(idReferenciaReceitaSelected);

            }
        }

        Ajax.init($("#baseUrlGeral").val() + 'financeiro/areceber/carregar-referencias-receitas', { "idTipoReceita": idTipoReceita }, callback, null);
    }
}

var ContasAReceber = new ObjContasAReceber();

$(document).ready(function () {
    ContasAReceber.init();
});