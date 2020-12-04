function ExtratoPessoaClass() {

    var baseUrl;

    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();
        ExtratoPessoa.efeitoBlocoPessoa();
        $(".multiSelectTipoReceita").multiselect({numberDisplayed: 1});

    }
    
    this.modalRegistrarPagamentoReceita = function (id) {

        var postData = { id : id };

        var url = this.baseUrl + "Financeiro/ReceitaBaixa/modal-registrar-pagamento";

        ReceitaBaixa.abrirModalPagamento(url, postData);
        
    }

    this.modalRegistrarPagamentoReceitaParcela = function (id) {

        var postData = { id : id };

        var url = this.baseUrl + "Financeiro/ReceitaDetalhePagamentosOperacao/modal-registrar-pagamento";

        ReceitaRegistrarPagamento.abrirModalPagamento(url, 'lancamentos', postData);

    }

    this.modalRegistrarPagamentoDespesa = function (id) {

        var postData = { id : id };
        
        var url = this.baseUrl + "Financeiro/DespesaDetalhePagamentosOperacao/modal-registrar-pagamento";

        DespesaRegistrarPagamento.abrirModalPagamento(url, 'lancamentos', postData);

    }
    
    this.efeitoBlocoPessoa = function () {
        
        $("[data-widget='collapse-custom']").click(function () {

            var objeto = $(this);
            var idPessoa = objeto.data("id");

            //Find the box parent        
            var box = $("#pessoa-"+idPessoa);

            //Find the body and the footer
            if (!box.hasClass("collapsed-box")) {

                box.addClass("collapsed-box");

                //Convert minus into plus
                $(this).children(".fa-minus").removeClass("fa-minus").addClass("fa-plus");

                box.slideUp();

            } else {
                box.removeClass("collapsed-box");
                //Convert plus into minus
                $(this).children(".fa-plus").removeClass("fa-plus").addClass("fa-minus");
                box.slideDown();
            }
        });
    }
    
}

var ExtratoPessoa = new ExtratoPessoaClass();

$(document).ready(function () {
    ExtratoPessoa.init();
});