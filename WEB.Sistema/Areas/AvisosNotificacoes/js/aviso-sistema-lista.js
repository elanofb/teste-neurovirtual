function AvisoSistemaListaClass() {

    this.init = function () {};

    this.registrarLeitura = function (elemento) {

        var postData = { 'id': [] };
        var id = $(elemento).data("id");;

        if (id > 0) {
            postData["id"].push(id);
        } else {
            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () { postData["id"].push($(this).val()); });
        }

        if (postData["id"].length == 0) {
            jM.info("Informe ao menos um aviso.");
            return false;
        }

        var funcOk = function() {
            var url = $(elemento).data("url");
            $.post(url, postData, function (response) {
                if (response.error != 'undefined' && response.error == true) {
                    jM.error(response.message);
                    return false;
                }

                jM.success("Confirmação de leitura registrado com sucesso.", function () { window.location.reload() });
            });
        }

        jM.confirmation("Deseja confirmar a leitura do(s) aviso(s)?", funcOk, function() { return false; });
    }
}

var AvisoSistemaLista = new AvisoSistemaListaClass();

$(document).ready(function () {
    AvisoSistemaLista.init();
});