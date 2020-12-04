function AssociadoCampoOpcaoClass() {

    this.init = function() {};


    this.modalCampoOpcao = function (element) {

        var idTipoCadastro = $(element).data("id-tipo-campo-cadastro");


        $.get($(element).data("url"), {}, function (response) {
            if (response.error != 'undefined' && response.error == true) {
                jM.error(response.message);
                return false;
            }

            var Modal = $(response).modal();
            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax($("#modalCampoOpcao"));
                DefaultSistema.carregarConteudoElemento("partialLoadFormCampoOpcao");
                DefaultSistema.carregarConteudoElemento("partialLoadListaCampoOpcao");
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                AssociadoCampo.atualizarCampos(idTipoCadastro);

                $(this).remove();
            });
        });
    };

    this.onSucessFormOpcao = function (response) {

        if (response.error == false) {

            DefaultSistema.carregarConteudoElemento("partialLoadFormCampoOpcao");

            DefaultSistema.carregarConteudoElemento("partialLoadListaCampoOpcao");

            return;
        }
    }

    //acionar o formulário de edição de um grupo
    this.editarOpcao = function (elemento) {

        var url = $(elemento).data("url");

        $.get(url, {}, function (response) {
            $("#partialLoadFormCampoOpcao").html(response);
        });
    }

    //remover um grupo
    this.excluirOpcao = function (elemento) {

        var id = $(elemento).data("id");
        var url = $(elemento).data("url");

        var fYes = function () {
            $.post(url, { 'id': id }, function (response) {
                console.log(response);

                if (response.error == true) {
                    jM.error(response.message);
                    return;
                }
                $(elemento).closest("tr").fadeOut("slow");

                AssociadoCampo.atualizarCampos(response.idTipoCadastroCampo);
            });
        };

        jM.confirmation("Confirma a exclus&atilde;o do registro?", fYes, function () { return false; });
    }
}


var AssociadoCampoOpcao = new AssociadoCampoOpcaoClass();

$(document).ready(function () {
    AssociadoCampoOpcao.init();
});