function ObjDespesaPagamentoClone() {

    this.init = function () {};
    
    //Abre a modal para adicionar um novo centro de custo
	this.modalClonarPagamento = function (elemento) {

	    var url = $(elemento).data("url");
	    var id = $(elemento).data("id");

	    $.get(url, { id: id }, function (response) {

            if (response.error == true) {
                jM.error(response.message);
                return;
            }

            var Modal = $(response).modal({ title: 'Adiconar Despesa Pagamento' });

            $(Modal).on("shown.bs.modal", function(e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxClonarPagamentoDespesaForm"));
                ComboCentroCusto.comboCentroCusto(null, "modalCloneDespesaPagamento");
                ComboMacroConta.comboMacroConta(null, "modalCloneDespesaPagamento");
                ComboSubConta.comboSubConta(null, "modalCloneDespesaPagamento");
            });

            $(Modal).on("hidden.bs.modal", function (e) { $(this).remove();});
        });
    }

    //Evento chamado ao submeter o form da modal de cadastrar o centro de custo
    this.onSuccessSalvarClone = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxClonarPagamentoDespesaForm"));
            ComboCentroCusto.comboCentroCusto(null, "modalCloneDespesaPagamento");
            ComboMacroConta.comboMacroConta(null, "modalCloneDespesaPagamento");
            ComboSubConta.comboSubConta(null, "modalCloneDespesaPagamento");
            return;
        }

        if (response.error === false) {
            DefaultSistema.removerModais();
            DespesaDetalhePagamento.iniciarBoxPagamentos();
            DespesaDetalhe.iniciarBoxInformacoes();
        }
    }
};

var DespesaPagamentoClone = new ObjDespesaPagamentoClone();
$(document).ready(function(){
    DespesaPagamentoClone.init();
});
