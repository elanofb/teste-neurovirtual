function AssociadoEventoClass() {

    var idBoxLista = "#boxLoadListaAssociadoEvento";

    this.init = function () {

        this.carregarLista();

    };

    //Carregamento da lista de documentos existentes
    this.carregarLista = function () {
        var elemento = $(idBoxLista);
        var url = elemento.data("url");

        $.get(url, {},
            function(response) {
                elemento.html(response);

                $(elemento).find("[data-toggle='tooltip']").tooltip();

                DefaultSistema.iniciarLinksAcao($(idBoxLista));
            }
        );
    };

    //Carregamento da lista de documentos existentes
    this.modalInscritos = function (elemento) {

        $(".table-bordered").loadingOverlay();
        var url = $(elemento).data("url");

        $.get(url, {},
            function (response) {
                var Modal = $(response).modal();

                Modal.on("shown.bs.modal", function (e) {

                });

                $(".table-bordered").loadingOverlay('remove');
            }
        );
    };


};

var AssociadoEvento = new AssociadoEventoClass();

$(document).ready(function () {
    AssociadoEvento.init();
});