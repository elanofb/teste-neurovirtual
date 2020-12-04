function TituloReceitaPagamentoExclusaoClass() {

    this.init = function () {
    };


    //Apos formulario de registro de pagamento da anuidade ser enviado
    this.onSuccessExclusaoForm = function (response) {

        if (response.error === false) {

            location.reload(true);

            return;
        }

        var boxPagamento = $("#boxExcluirPagamento");

        DefaultSistema.iniciarBotoes(boxPagamento);

    };


};

var TituloReceitaPagamentoExclusao = new TituloReceitaPagamentoExclusaoClass();

$(document).ready(function(){
    TituloReceitaPagamentoExclusao.init();
});
