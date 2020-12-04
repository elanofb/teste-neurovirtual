function ObjTipoMaterialApoio() {

    this.init = function () {

    }

    //Abrir modal para cadastrar novo local de evento
    this.modalCategoria = function (elemento) {

        $("select[rel=idCategoria]").select2("close");

        var url = $("#baseUrlGeral").val() + "MateriaisApoio/tipomaterialapoio/modal-cadastrar-categoria";

        $.get(url, {}, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Categoria' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormCategoria"));
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });
    }

    //Retorno após submissões do form no modal
    this.onSuccess = function (response) {

        if (typeof (response.error) == 'undefined') {

            DefaultSistema.iniciarPluginsAposAjax($("#boxFormCategoria"));

            return;
        }

        //Se o registro for salvo com sucesso
        if (response.error === false) {

            var combo = $("select[id=idCategoria]");
            combo.select2("destroy");

            var flagJaExiste = combo.find("option[value='" + response.id + "']").length > 0;

            if (flagJaExiste === false) {

                combo.append($("<option></option>").val(response.id).html(response.descricao));

                combo.val(response.id);

                DefaultSistema.removerModais();

                MaterialApoio.select2();
            }
        }
    }
}

var TipoMaterialApoio = new ObjTipoMaterialApoio();

$(document).ready(function () {

    TipoMaterialApoio.init();
});