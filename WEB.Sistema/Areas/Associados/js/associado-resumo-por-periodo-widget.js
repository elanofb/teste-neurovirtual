function AssociadoResumoPorPeriodoWidgetClass() {
    
    var boxResumoAssociadosPorPeriodo = "#boxResumoAssociadosPorPeriodo";

    var btnResumoAssociadosPeriodo = "#btnResumoAssociadosPeriodo";

    this.init = function () {

    };

    this.carregarWidget = function () {

        var fnCallback = function () {

            AssociadoResumoPorPeriodoWidget.iniciarGraficosKnob();

            AssociadoResumoPorPeriodoWidget.iniciarDaterangerpicker();

        }

        DefaultSistema.carregarConteudo($(boxResumoAssociadosPorPeriodo), fnCallback);

    }
    
    this.iniciarGraficosKnob = function () {

        $(".dial").knob({ width: "70", height: "95", min: "0", readOnly: "true" });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociadosPorPeriodo).find("#totalAssociadosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociadosPorPeriodo).find("#totalAssociados").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociadosPorPeriodo).find("#totalAtivosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociadosPorPeriodo).find("#totalAtivos").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociadosPorPeriodo).find("#totalEmAdmissaoValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociadosPorPeriodo).find("#totalEmAdmissao").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

        $({ animatedVal: 0 }).animate({ animatedVal: $(boxResumoAssociadosPorPeriodo).find("#totalInativosValue").val() }, {
            duration: 1500, easing: "swing",
            step: function () { $(boxResumoAssociadosPorPeriodo).find("#totalInativos").val(Math.ceil(this.animatedVal)).trigger("change"); }
        });

    }

    this.iniciarDaterangerpicker = function () {

        var startDate = moment().subtract(29, 'days');
        var endDate = moment();
        
        var fnCallback = function (start, end) {

            $(boxResumoAssociadosPorPeriodo).html("");
            $(boxResumoAssociadosPorPeriodo).addClass("carregando");


            $(btnResumoAssociadosPeriodo).find("span").html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));

            var dataInicio = start.format('DD/MM/YYYY');
            var dataFim = end.format('DD/MM/YYYY');

            var url = $("#baseUrlGeral").val() + 'Associados/AssociadoWidget/widget-resumo-associados-por-periodo';

            $.post(url, { dataInicio: dataInicio, dataFim: dataFim }, function (response) {

                $(boxResumoAssociadosPorPeriodo).removeClass("carregando");

                $(boxResumoAssociadosPorPeriodo).html(response);

                AssociadoResumoPorPeriodoWidget.iniciarGraficosKnob();
                    
            });

        }

        $(btnResumoAssociadosPeriodo).daterangepicker({

            ranges: {
                'Últimos 30 dias': [moment().subtract(29, 'days'), moment()],
                'Este mês': [moment().startOf('month'), moment().endOf('month')],
                'Mês passado': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'Este ano': [moment().startOf('year'), moment().endOf('year')],
                'Ano passado': [moment().subtract(1, 'year').startOf('year'), moment().subtract(1, 'year').endOf('year')],
            },

            format: 'DD/MM/YYYY',

            startDate: startDate,
            endDate: endDate

        }, fnCallback);

        fnCallback(startDate, endDate);

    }

};

var AssociadoResumoPorPeriodoWidget = new AssociadoResumoPorPeriodoWidgetClass();

$(document).ready(function () {
    AssociadoResumoPorPeriodoWidget.init();
});