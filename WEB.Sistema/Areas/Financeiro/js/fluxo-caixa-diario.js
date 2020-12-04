function FluxoCaixaDiarioClass() {

    var baseUrl;

    var formFiltros = "#formFiltros";
    var boxGraficoEvolucao = "#boxGraficoEvolucao";
    var boxMovimentacaoDiaria = "#boxMovimentacaoDiaria";
    var labelDtCadastro = "#dtVencimentoLabel";
    var campoDtCadastroInicio = "#dtInicioPeriodo";
    var campoDtCadastroFim = "#dtFimPeriodo";
    
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();
        this.iniciarRangePicker();
        this.carregarBoxMovimentacaoDiaria();
        this.carregarBoxGraficoEvolucao();
    };

    this.filtrar = function () {
        this.carregarBoxMovimentacaoDiaria();
        this.carregarBoxGraficoEvolucao();
    };

    this.carregarBoxMovimentacaoDiaria = function () {

        var dados = $(formFiltros).serializeObject();
        
        var url = $(boxMovimentacaoDiaria).data("url");

        $(boxMovimentacaoDiaria).loadingOverlay();

        $.post(url, dados, function (response) {

            $(boxMovimentacaoDiaria).html(response);

            $(boxMovimentacaoDiaria).loadingOverlay("remove");

            DefaultSistema.reiniciarBotao();

            $(boxMovimentacaoDiaria).find("[data-toggle=tooltip]").tooltip({
                container: 'body'
            });

            $(boxMovimentacaoDiaria).find("#tableMovimentacaoDiaria").slimScroll({
                height: 550
            })
        })
    };

    this.carregarBoxGraficoEvolucao = function () {

        var url = this.baseUrl + "Financeiro/FluxoCaixaDiario/carregar-evolucao-caixa";

        var dados = $(formFiltros).serialize();

        $(boxGraficoEvolucao).loadingOverlay();

        $.post(url, dados, function (response) {

            $(boxGraficoEvolucao).loadingOverlay("remove");

            DefaultSistema.reiniciarBotao();

            if (response.datas.length === 0) {
                $(boxGraficoEvolucao).html("<div class=\"alert alert-info\">Nenhuma informa&ccedil;&atilde;o foi encontrada para gerar o gr&aacute;fico de evolu&ccedil;&atilde;o.</div>");
            }

            if (response.datas.length > 0) {
                FluxoCaixaDiario.iniciarGraficoEvolucao(response);
            }
        })
    };

    this.iniciarGraficoEvolucao = function (dados) {

        Highcharts.chart('boxGraficoEvolucao', {

            chart: { type: 'column' },
            title:{ text:'' },
            xAxis: { categories: dados.datas },

            yAxis: [{
                title: { text: '' },
                labels: {
                    formatter: function () {
                        return 'R$' + this.axis.defaultLabelFormatter.call(this);
                    }
                }
            }],

            plotOptions: {
                column: { stacking: 'normal' }
            },

            tooltip: {
                valuePrefix: "R$",
                valueDecimals: 2,
                shared: true
            },
            
            series: [ {
                name: 'Receitas',
                data: dados.valoresReceitas,
                stack: 'lancamento',
                color: '#00a65a'
            }, {
                name: 'Despesas',
                data: dados.valoresDespesas,
                stack: 'lancamento',
                color: '#dd4b39'
            }]
            /*,{
                type: 'spline',
                name: 'Saldo Acumulado',
                data: dados.saldosAcumulados,
                marker: {
                    lineWidth: 2
                }
            }]*/
        });
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

var FluxoCaixaDiario = new FluxoCaixaDiarioClass();
$(document).ready(function(){
    FluxoCaixaDiario.init();
});
