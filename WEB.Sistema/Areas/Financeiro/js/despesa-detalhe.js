function ObjDespesaDetalhe() {

    this.init = function () {
        DespesaDetalhe.iniciarBoxInformacoes();
    }

    this.iniciarBoxInformacoes = function () {
        DefaultSistema.carregarConteudo($("#BoxLoadDadosEditar"), function () {
            EditableCustom.listenerEditables();
            EditableCustom.listenerEditableEditor();
        })
    }

    this.onSuccessExclusaoDespesa = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormExcluirDespesa"));
            DefaultSistema.reiniciarBotao();

            return;
        }

        if (response.error == false) {
            jM.success(response.message, function () { window.location = response.urlRetorno });
        }

        if (response.error == true) {
            jM.error(response.message, function () { window.location = response.urlRetorno });
        }
    }
}

var DespesaDetalhe = new ObjDespesaDetalhe();
$(document).ready(function () {
    DespesaDetalhe.init();
});