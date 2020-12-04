function LancamentoDespesaClass() {
    this.init = function () {
        Auxiliares.carregarMacroConta('D', $("#idMacroConta").data("val"));
        $(".multiSelectCredor").multiselect({
            enableFiltering: true,
            filterBehavior: 'text',
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1
        });
    }
}
var LancamentoDespesa = new LancamentoDespesaClass();

$(document).ready(function () {
    LancamentoDespesa.init();
});