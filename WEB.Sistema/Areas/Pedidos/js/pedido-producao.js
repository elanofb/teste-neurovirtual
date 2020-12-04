function PedidoProducaoClass() {
    
    var boxResumoInscricoes = "#boxResumoPedidos";
    var baseUrlGeral;

    this.init = function () {

        this.baseUrlGeral = $("#baseUrlGeral").val();

        this.carregarWidgetResumoPedidos();

        $(".multiSelect").multiselect({
            enableFiltering: true,
            filterBehavior: 'text',
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1
        });

    };

    this.carregarWidgetResumoPedidos = function () {

        var fnCallback = function() {
            
        }

        PedidoProducao.iniciarGraficosKnob();

    }
    
    this.iniciarGraficosKnob = function () {

        $(".dial").knob({ width: "70", height: "95", min: "0", readOnly: "true" });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoInscricoes).find("#totalPedidosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoInscricoes).find("#totalPedidos").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoInscricoes).find("#totalPedidosAtrasadosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoInscricoes).find("#totalPedidosAtrasados").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoInscricoes).find("#totalPedidosHojeValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoInscricoes).find("#totalPedidosHoje").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoInscricoes).find("#totalPedidosAmanhaValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoInscricoes).find("#totalPedidosAmanha").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoInscricoes).find("#totalPedidosSemanaValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoInscricoes).find("#totalPedidosSemana").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });


    }

    this.imprimir = function (elem, idProduto) {
        
        var form = $("#formFiltros");
        
        var urlBusca = form.attr("action");
        var urlImpressao = $(elem).data("url");
        
        form.attr("action", urlImpressao);
        form.attr("target", "_blank");
        
        var inputIdProduto = $("<input>");

        inputIdProduto.attr({
            type: 'hidden',
            name: 'idProduto',
            value: idProduto
        }).appendTo(form);
        
        form.submit();

        inputIdProduto.remove();
        
        form.attr("action", urlBusca);
        form.attr("target", "");

    }
        
}

var PedidoProducao = new PedidoProducaoClass();

$(document).ready(function () {
    PedidoProducao.init();
});