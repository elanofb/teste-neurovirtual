function ObjTipoGaleriaFoto() {

    this.init = function () {
    };

    //Abrir modal para cadastrar uma novo tipo
    this.modalTipoGaleria = function (elemento) {

        var url = $(elemento).data("url");

        $.get(url, {}, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar uma Nova Categoria' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax();
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });
    }

    //Retorno após submissoes do form no modal
    this.onSuccess = function(response) {

        if (typeof (response.error) == 'undefined') {
            DefaultSistema.iniciarPluginsAposAjax();
            return;
        }

        //Se o registro for salvo com sucesso
        if (response.error === false) {

            var combo = $("#GaleriaFoto_idTipoGaleria");

            var flagJaExiste = combo.find("option[value=" + response.id + "]").length > 0;

            console.log(flagJaExiste);

            if (flagJaExiste === false) {

                combo.append($("<option></option>").val(response.id).html(response.descricao));

                combo.val(response.id);

                DefaultSistema.removerModais();
            }

        }
    }
};

var TipoGaleriaFoto = new ObjTipoGaleriaFoto();

$(document).ready(function () {
    TipoGaleriaFoto.init();
});