function LancamentoDespesaClass() {

    this.init = function () {

        Auxiliares.carregarMacroConta('D', $("#idMacroConta").data("val"));

        $(".multiSelectTipoReceita").multiselect({numberDisplayed: 1});        
    };
}

var LancamentoDespesa = new LancamentoDespesaClass();

$(document).ready(function () {
    LancamentoDespesa.init();
});