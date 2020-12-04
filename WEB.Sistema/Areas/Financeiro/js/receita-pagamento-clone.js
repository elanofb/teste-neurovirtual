function ObjReceitaPagamentoClone() {

    this.init = function () {
	};
    
    //Abre a modal para adicionar um novo centro de custo
	this.modalClonarPagamento = function (elemento) {

	    var url = $(elemento).data("url");
	    var id = $(elemento).data("id");

	    $.get(url, { id: id }, function (response) {

	        if (response.error == true) {
	            jM.error(response.message);
	            return;
	        }

            var Modal = $(response).modal({ title: 'Adiconar Receita Pagamento' });

            $(Modal).on("shown.bs.modal", function(e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxClonarPagamentoReceitaForm"));
                ComboCentroCusto.comboCentroCusto(null, "modalCloneReceitaPagamento");
                ComboMacroConta.comboMacroConta(null, "modalCloneReceitaPagamento");
                ComboSubConta.comboSubConta(null, "modalCloneReceitaPagamento");
            });

            $(Modal).on("hidden.bs.modal", function (e) { $(this).remove();});
        });
    }

    //Evento chamado ao submeter o form da modal de cadastrar o centro de custo
    this.onSuccessSalvarClone = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxClonarPagamentoReceitaForm"));
            ComboCentroCusto.comboCentroCusto(null, "modalCloneReceitaPagamento");
            ComboMacroConta.comboMacroConta(null, "modalCloneReceitaPagamento");
            ComboSubConta.comboSubConta(null, "modalCloneReceitaPagamento");
            return;
        }

        if (response.error === false) {
            DefaultSistema.removerModais();
            ReceitaDetalhePagamento.iniciarBoxPagamentos();
            ReceitaDetalhe.iniciarBoxInformacoes();
        }
    }
};

var ReceitaPagamentoClone = new ObjReceitaPagamentoClone();
$(document).ready(function(){
    ReceitaPagamentoClone.init();
});
