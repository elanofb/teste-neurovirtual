function UltimasTransacoesWidgetClass() {
    
    this.init = function () {

        UltimasTransacoesWidget.iniciarSlimScroll();

    };
  
    this.iniciarSlimScroll = function () {

        $("#boxUltimasTransacoesWidget").slimScroll({
            height: 350,
            alwaysVisible: false
        });

        $("#boxUltimosPagamentosWidget").slimScroll({
            height: 350,
            alwaysVisible: false
        });
    }

};

var UltimasTransacoesWidget = new UltimasTransacoesWidgetClass();

$(document).ready(function () {
    UltimasTransacoesWidget.init();
});