function ConciliacaoClass() {

    var labelDtCadastro = "#dtPeriodoLabel";

    var campoDtCadastroInicio = "#dtInicio";
    var campoDtCadastroFim = "#dtFim";

    var boxListaLancamentos = "#boxListaLancamentos";
    var boxListaConciliacoes = "#boxListaConciliacoes";

    //
    this.init = function () {
        
        $("#formFiltro").on("submit", function (e) {

            e.preventDefault();

            Conciliacao.carregarLancamentos();
            Conciliacao.carregarConciliacoes();  
        });
        
        this.iniciarRangePicker();
        this.carregarLancamentos();
        this.carregarConciliacoes();
        
    };

    //
    this.iniciarRangePicker = function () {

        function selecionarDatas(dtInicio, dtFim) {

            $(labelDtCadastro).html(dtInicio.format('DD/MM/YYYY') + " - " + dtFim.format('DD/MM/YYYY'));

            $(campoDtCadastroInicio).val(dtInicio.format('DD/MM/YYYY'));

            $(campoDtCadastroFim).val(dtFim.format('DD/MM/YYYY'));

        }

        $("#dtPeriodoRange").daterangepicker({

            locale: {

                customRangeLabel: "Período Customizado",

                format: "DD/MM/YYYY"

            },

            opens: "left", 
            
            autoApply: true,

            //dateLimit: {days: 31},

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

    this.carregarLancamentos = function () {
        
        $(boxListaLancamentos).addClass("carregando");

        var url = $("#baseUrlGeral").val() + "financeirolancamentos/conciliacao/partial-lancamentos";
        var postData = $("#formFiltro").serializeObject();
        
        $.post(url, postData, function (response) {
            $(boxListaLancamentos).removeClass("carregando");
            $(boxListaLancamentos).html(response);
            DefaultSistema.iniciarCheckBoxes();
        });
    }

    this.carregarConciliacoes = function () {

        $(boxListaConciliacoes).addClass("carregando");
        
        var url = $("#baseUrlGeral").val() + "financeirolancamentos/conciliacao/partial-conciliacoes";
        var postData = $("#formFiltro").serializeObject();

        $.post(url, postData, function (response) {
            $(boxListaConciliacoes).removeClass("carregando");
            $(boxListaConciliacoes).html(response);
            DefaultSistema.iniciarCheckBoxes();
        });
    }

}

var Conciliacao = new ConciliacaoClass();
$(document).ready(function(){
    Conciliacao.init();
});
