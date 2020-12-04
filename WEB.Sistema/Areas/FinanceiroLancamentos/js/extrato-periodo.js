function ExtratoPeriodoClass() {

    var labelDtCadastro = "#dtVencimentoLabel";

    var campoDtCadastroInicio = "#dtVencimentoInicio";
    var campoDtCadastroFim = "#dtVencimentoFim";

    //
    this.init = function() {

        this.iniciarRangePicker();

    };

    //
    this.iniciarRangePicker = function() {

        function selecionarDatas (dtInicio, dtFim) {

            $(labelDtCadastro).html(dtInicio.format('DD/MM/YYYY') + " - " + dtFim.format('DD/MM/YYYY'));

            $(campoDtCadastroInicio).val(dtInicio.format('DD/MM/YYYY'));

            $(campoDtCadastroFim).val(dtFim.format('DD/MM/YYYY'));

        }

        $("#dtVencimentoRange").daterangepicker({

            locale: {

                customRangeLabel: "Período Customizado",

                format: "DD/MM/YYYY"

            },

            autoApply: true,

            dateLimit: { days : 31 },

            startDate: $(campoDtCadastroInicio).val(),// moment().startOf('month'),

            endDate: $(campoDtCadastroFim).val(),// moment().endOf('month'),

            ranges: {
                'Hoje': [moment(), moment()],
                'Ontem': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Mês Atual': [moment().startOf('month'), moment().endOf('month')],
                'Mês Anterior': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            }

        }, selecionarDatas);
    }
}

var ExtratoPeriodo = new ExtratoPeriodoClass();
$(document).ready(function(){
    ExtratoPeriodo.init();
});
