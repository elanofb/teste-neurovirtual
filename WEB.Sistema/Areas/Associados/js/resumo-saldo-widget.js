function ResumoSaldoWidgetClass() {
    
    var boxResumoSaldo = "#boxResumoSaldo";

    this.init = function () {

        this.carregarWidgetResumo();

    };

    //
    this.carregarWidgetResumo = function () {

        var fnCallback = function() {
            ResumoSaldoWidget.iniciarGraficosKnob();
        };

        DefaultSistema.carregarConteudo($(boxResumoSaldo), fnCallback);

    }
    
    this.iniciarGraficosKnob = function () {

        $(".dial").knob({ width: "70", height: "95", min: "0", readOnly: "true" });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoSaldo).find("#saldoTotalValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoSaldo).find("#saldoTotal").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoSaldo).find("#saldoLinkeyValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoSaldo).find("#saldoLinkey").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoSaldo).find("#saldoConsumidoresValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoSaldo).find("#saldoConsumidores").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoSaldo).find("#saldoEstabelecimentosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoSaldo).find("#saldoEstabelecimentos").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

    }

};

var ResumoSaldoWidget = new ResumoSaldoWidgetClass();

$(document).ready(function () {
    ResumoSaldoWidget.init();
});