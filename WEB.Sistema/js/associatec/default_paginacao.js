function DefaultPaginacaoClass() {
    var formFiltro = ".formFiltro";
    var barraProgresso = "#barraProgressoExcel";
    var btDownloadExcel = "#btDownload";

    var nroPaginaAtual = 0;
    var totalPaginas = 0;
    var incrementoBarraProgresso = 0;
    var progressoAtual = 0;

    this.init = function () {
        DefaultPaginacao.iniciarPaginacaoComPost();
    }

    this.iniciarPaginacaoComPost = function () {

        $(".pagination-container .pagination a").on("click", function (e) {

            e.preventDefault();

            if ($(this).parent().hasClass("disabled")) {
                return;
            }

            if ($(this).parent().hasClass("PagedList-skipToNext")) {
                
                var nroPaginaAtual = $(".pagination-container .pagination li.active a").html();

                var novaPagina = parseInt(nroPaginaAtual) + 1;

                $("#nroPagina").val(novaPagina);

                $(formFiltro).submit();

                return;

            }

            if ($(this).parent().hasClass("PagedList-skipToPrevious")) {
                
                var nroPaginaAtual = $(".pagination-container .pagination li.active a").html();

                var novaPagina = parseInt(nroPaginaAtual) - 1;

                $("#nroPagina").val(novaPagina);

                $(formFiltro).submit();

                return false;

            }

            var nroPagina = $(this).html();
            $("#nroPagina").val(nroPagina);

            $(formFiltro).submit();

        });
    }

    //
    this.consultar = function (urlExcel, urlDownloadExcel, idFormFiltro, idBarraProgresso, idBtnDownloadExcel) {
        barraProgresso = (idBarraProgresso ? idBarraProgresso : barraProgresso);
        btDownloadExcel = (idBtnDownloadExcel ? idBtnDownloadExcel : btDownloadExcel);

        formFiltro = (idFormFiltro ? idFormFiltro : formFiltro)

        var form = $(formFiltro);
        var dadosFormulario = $(form).serializeObject();

        $(barraProgresso).addClass("hide");
        $(btDownloadExcel).addClass("hide");

        if (dadosFormulario.tipoSaida == "excel") {
            nroPaginaAtual = 1;
            totalPaginas = 0;
            incrementoBarraProgresso = 0;
            progressoAtual = 0;

            DefaultPaginacao.atualizarBarraProgresso();
            DefaultPaginacao.gerarExcel(urlExcel, urlDownloadExcel, dadosFormulario, nroPaginaAtual, "");

            return;
        }
        form.submit();
    }

    this.gerarExcel = function (urlExcel, urlDownloadExcel, dadosFormulario, nroPagina, nomeArquivo) {

        dadosFormulario.nroPagina = nroPagina;
        dadosFormulario.nomeArquivo = nomeArquivo;

        $.post(urlExcel, dadosFormulario, function (response) {

            if (!response.totalPaginas > 0) {
                jM.info("A consulta n&atilde;o retornou nenhum resultado!");
                DefaultSistema.reiniciarBotao();
                return;
            }

            $(barraProgresso).removeClass("hide");

            nomeArquivo = response.nomeArquivo;
            totalPaginas = response.totalPaginas;
            incrementoBarraProgresso = 100 / totalPaginas;
            progressoAtual += incrementoBarraProgresso;
            progressoAtual = (progressoAtual > 100 ? 100 : progressoAtual);

            DefaultPaginacao.atualizarBarraProgresso();

            if (nroPaginaAtual != totalPaginas) {
                nroPaginaAtual++;
                DefaultPaginacao.gerarExcel(urlExcel, urlDownloadExcel, dadosFormulario, nroPaginaAtual, nomeArquivo);
                return;
            }

            if (nroPaginaAtual == totalPaginas) {
                $(btDownloadExcel).attr("href", urlDownloadExcel.concat("?nomeArquivo=" + nomeArquivo));
                setTimeout(function () { $(".link-loading").button("reset"); $(btDownloadExcel).removeClass("hide"); }, 50);
            }
        });
    }

    this.atualizarBarraProgresso = function () {
        $(barraProgresso).find("div").html(progressoAtual.toFixed(0) + "%");
        $(barraProgresso).find("div").css("width", progressoAtual + "%");
        $(barraProgresso).find("div").attr("aria-valuenow", progressoAtual);
    }
}

var DefaultPaginacao = new DefaultPaginacaoClass();

$(document).ready(function () {

    DefaultPaginacao.init();

});