function AssociadoResumoWidgetClass() {
    
    var boxResumoAssociados = "#boxResumoAssociados";

    this.init = function () {

        this.carregarWidgetResumoAssociados();

    };

    this.carregarWidgetResumoAssociados = function () {

        var fnCallback = function() {
            AssociadoResumoWidget.iniciarGraficosKnob();
        }

        DefaultSistema.carregarConteudo($(boxResumoAssociados), fnCallback);

    }
    
    this.iniciarGraficosKnob = function () {

        $(".dial").knob({ width: "70", height: "95", min: "0", readOnly: "true" });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociados).find("#totalAtivosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociados).find("#totalAtivos").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociados).find("#totalInativosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociados).find("#totalInativos").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociados).find("#totalAdimplentesValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociados).find("#totalAdimplentes").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociados).find("#totalInadimplentesValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociados).find("#totalInadimplentes").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

    }

};

var AssociadoResumoWidget = new AssociadoResumoWidgetClass();

$(document).ready(function () {
    AssociadoResumoWidget.init();
});