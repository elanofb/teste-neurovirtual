function AssociadoConsultaClass() {

    var formFiltro = "#fmFiltro";

    this.init = function () {

        $(".input-multiselect").multiselect({
            buttonClass: "form-control input-sm",
            numberDisplayed: 1
        });
        
        this.iniciarPaginacaoComPost();

    }

    this.iniciarPaginacaoComPost = function () {

        $(".pagination-container .pagination a").on("click", function (e) {
            e.preventDefault();

            var nroPagina = $(this).html();
            $("#nroPagina").val(nroPagina);

            $(formFiltro).submit();
        });

    }

    //Exportar arquivos em pasta compactada
    this.gerarZip = function () {

        var urlExcel = $("#baseUrlGeral").val().concat("AssociadosConsultas/AssociadoDocumento/exportar");

        var urlDownloadExcel = $("#baseUrlGeral").val().concat("AssociadosConsultas/AssociadoDocumento/download-zip");

        var dadosFormulario = { 'id': [] };
        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            dadosFormulario["id"].push($(this).val());
        });

        if (dadosFormulario["id"].length == 0) {
            var form = $("#formFiltro");
            dadosFormulario = $(form).serializeObject();
        }

        $.post(urlExcel, dadosFormulario, function (response) {
            if (response.totalRegistros = 0) {
                jM.info("A consulta n&atilde;o retornou nenhum resultado!");
                DefaultSistema.reiniciarBotao();
                return;
            }

            if (response.error == true) {
                jM.error(response.message);
                DefaultSistema.reiniciarBotao();
                return;
            }

            if (response.error == false) {
                window.open(urlDownloadExcel.concat("?nomeArquivo=" + response.nomeArquivo));
            }
            DefaultSistema.reiniciarBotao();
        });
    }

}

var AssociadoConsulta = new AssociadoConsultaClass();

$(document).ready(function () {

    AssociadoConsulta.init();

});