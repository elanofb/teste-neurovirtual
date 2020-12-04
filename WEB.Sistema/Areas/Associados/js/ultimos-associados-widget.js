function UltimosAssociadosWidgetClass() {
    
    this.init = function () {

        UltimosAssociadosWidget.iniciarSlimScroll();

    };
  
    this.iniciarSlimScroll = function () {

        $("#boxUltimosAssociadosWidget").slimScroll({
            height: 350,
            alwaysVisible: false
        });

    }

};

var UltimosAssociadosWidget = new UltimosAssociadosWidgetClass();

$(document).ready(function () {
    UltimosAssociadosWidget.init();
});