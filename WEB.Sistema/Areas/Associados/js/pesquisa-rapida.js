function PesquisaRapidaClass() {

    var campoBuscaRapida = "#campoBuscaRapida";
    var boxResultadoPesquisa = "#boxResultadoPesquisa";
    var boxPesquisaRapidaResultados = "#boxPesquisaRapidaResultados";

    this.init = function () {
        this.pesquisaRapida();
    };

    // Inicar o box do header do sistema que disponibiliza formulário para buscas rápidas
    this.pesquisaRapida = function () {

        $(".consulta-rapida").submit(function (e) {
            e.preventDefault();
            
            PesquisaRapida.enviarPesquisaRapida($("#campoBuscaRapida"));
        })

    }

    //
    this.enviarPesquisaRapida = function (elemento) {

        var textoBusca = $(elemento).val();

        if (textoBusca.length < 1) {
            
            if ($("#boxPesquisaRapida").is(":visible")) {
                $("#boxPesquisaRapida").hide("fast");
            }

            return false;

        }

        if ($("#boxPesquisaRapida").is(":hidden")) {
            $("#boxPesquisaRapida").show("fast");
        }

        $(boxPesquisaRapidaResultados).html("<div class='padding-10 text-center'><i class=\"fa fa-spin fa-sync\"></i> Localizando registros...</div>");

        var url = new String($("#baseUrlGeral").val()).concat("Associados/associadobusca/pesquisa-rapida-listar?valorBusca=").concat(textoBusca);
        $.get(url, {}, function (response) {
            
            $(boxPesquisaRapidaResultados).html(response);

            $(boxResultadoPesquisa).slimScroll({
                height: 300,
                alwaysVisible: false
            });

            DefaultSistema.iniciarPluginsAposAjax($(boxResultadoPesquisa));

        });
    }

};

var PesquisaRapida = new PesquisaRapidaClass();

$(document).ready(function () {
    PesquisaRapida.init();
});