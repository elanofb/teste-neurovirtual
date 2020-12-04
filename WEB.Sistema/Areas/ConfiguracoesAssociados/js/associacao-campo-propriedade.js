function AssociadoCampoPropriedadeClass() {

    this.init = function() {};

    //Abrir janela modal para alteração de status
    this.modalCampoPropriedade = function (element) {

        var idTipoCadastro = $(element).data("id-tipo-campo-cadastro");

        $.get($(element).data("url"), {}, function (response) {
            if (response.error != 'undefined' && response.error == true) {
                jM.error(response.message);
                return false;
            }

            var Modal = $(response).modal();
            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax($("#modalCampoPropriedade"));
                DefaultSistema.carregarConteudoElemento("partialLoadFormCampoPropriedade");
                DefaultSistema.carregarConteudoElemento("partialLoadListaCampoPropriedade");
            });

            $(Modal).on("hidden.bs.modal", function (e) {

                AssociadoCampo.atualizarCampos(idTipoCadastro);

                $(this).remove();
            });
        });
    };

    this.onSucessFormPropriedade = function (response) {

        if (response.error == false) {

            DefaultSistema.carregarConteudoElemento("partialLoadFormCampoPropriedade");

            DefaultSistema.carregarConteudoElemento("partialLoadListaCampoPropriedade");

            return;
        }
    }

    //acionar o formulário de edição de um grupo
    this.editarPropriedade = function (elemento) {

        var url = $(elemento).data("url");

        $.get(url, {}, function (response) {
            $("#partialLoadFormCampoPropriedade").html(response);
        });
    }

    //remover um grupo
    this.excluirPropriedade = function (elemento) {

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


var AssociadoCampoPropriedade = new AssociadoCampoPropriedadeClass();

$(document).ready(function () {
    AssociadoCampoPropriedade.init();
});