function AssociadoEmailConsultaClass() {

    var formFiltro = "#fmFiltro";

    this.init = function () {

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

}

var AssociadoEmailConsulta = new AssociadoEmailConsultaClass();

$(document).ready(function () {

    AssociadoEmailConsulta.init();

});