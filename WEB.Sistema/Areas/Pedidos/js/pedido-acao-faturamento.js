function PedidoAcaoFaturamentoClass() {

    var baseUrl;
    
    //
    this.init = function () {
        
    };

    //
    this.showModal = function (url) {

        DefaultSistema.showModal(url, PedidoAcaoFaturamento.iniciarPluginsCombos);

    }

    //
    this.iniciarPluginsCombos = function () {

        ComboCentroCusto.iniciarComboCentroCusto();

        ComboMacroConta.iniciarComboMacroConta();

        ComboSubConta.iniciarComboSubConta();

    }
    
    //
    this.onSuccess = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            location.reload();

            return;

        }

        DefaultSistema.iniciarPluginsAposAjax($("#boxModalFaturamento"));

        PedidoAcaoFaturamento.iniciarPluginsCombos();

        DefaultSistema.reiniciarBotao();

    }

};

var PedidoAcaoFaturamento = new PedidoAcaoFaturamentoClass();

$(document).ready(function () {
    PedidoAcaoFaturamento.init();
});