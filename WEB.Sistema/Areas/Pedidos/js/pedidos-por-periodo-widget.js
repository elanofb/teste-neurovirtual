function ObjPedidosPorPeriodoWidget() {

    var boxResumoPedidosPorPeriodo = "#boxResumoPedidosPorPeriodo";

    var btnResumoPedidosPeriodo = "#btnResumoPedidosPeriodo";
    
    this.init = function () {
        this.iniciarDaterangerpicker();
    };

    this.iniciarDaterangerpicker = function () {

        var startDate = moment().subtract(6, 'days');
        var endDate = moment();

        var fnCallback = function (start, end) {
            
            $(boxResumoPedidosPorPeriodo).html("");
            $(boxResumoPedidosPorPeriodo).addClass("carregando");

            $(btnResumoPedidosPeriodo).find("span").html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));

            var dataInicio = start.format('DD/MM/YYYY');
            var dataFim = end.format('DD/MM/YYYY');

            var url = $("#baseUrlGeral").val() + 'Pedidos/PedidoPorPeriodoWidget/widget-pedidos-por-periodo';

            $.post(url, { dataInicio: dataInicio, dataFim: dataFim }, function (response) {

                $(boxResumoPedidosPorPeriodo).removeClass("carregando");

                $(boxResumoPedidosPorPeriodo).html(response);

            });

        }

        $(btnResumoPedidosPeriodo).daterangepicker({

            ranges: {
                'Últimos 7 dias': [moment().subtract(6, 'days'), moment()]
            },

            format: 'DD/MM/YYYY',

            startDate: startDate,
            endDate: endDate

        }, fnCallback);

        fnCallback(startDate, endDate);

    }
    
    this.alterarVisaoDiaria = function () {
        
        $(btnResumoPedidosPeriodo).removeClass("hide");

        PedidosPorPeriodoWidget.iniciarDaterangerpicker();
        
    }
    
    this.alterarVisaoSemanal = function () {

        $(btnResumoPedidosPeriodo).addClass("hide");

        var url = $("#baseUrlGeral").val() + 'Pedidos/PedidoPorPeriodoWidget/widget-pedidos-por-periodo';

        var startDate = moment().subtract(48, 'days').format('DD/MM/YYYY');
        var endDate = moment().format('DD/MM/YYYY');

        $.post(url, { dataInicio: startDate, dataFim: endDate, flagSemanal: true }, function (response) {

            $(boxResumoPedidosPorPeriodo).removeClass("carregando");

            $(boxResumoPedidosPorPeriodo).html(response);

        });
        
    }

};

var PedidosPorPeriodoWidget = new ObjPedidosPorPeriodoWidget();
$(document).ready(function () {
    PedidosPorPeriodoWidget.init();
});