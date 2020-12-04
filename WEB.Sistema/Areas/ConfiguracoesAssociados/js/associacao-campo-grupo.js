function AssociadoCampoGrupoClass() {

    this.init = function () {

    };

    //Atualizar lista de campos configurados
    this.atualizarGruposLista = function (idTipoCampoCadastro) {

        if (idTipoCampoCadastro > 0) {
            var box = $("#partialListaGrupo-" + idTipoCampoCadastro);

            DefaultSistema.carregarConteudo(box, AssociadoCampoGrupo.alterarOrdem(idTipoCampoCadastro));

        } else {

            $(".partial-lista-grupos").each(function (e, i) {

                var id = $(this).attr("id");

                var idTipoCampoCadastro = id.replace("partialListaGrupo-", "");

                var box = $("#" + id);

                DefaultSistema.carregarConteudo(box, AssociadoCampoGrupo.alterarOrdem(idTipoCampoCadastro));
            });
        }
    }

    //Atualizar lista de campos configurados
    this.atualizarGruposForm = function (idTipoCampoCadastro) {
        if (idTipoCampoCadastro > 0) {
            var box = $("#partialFormGrupo-" + idTipoCampoCadastro);

            DefaultSistema.carregarConteudo(box);

        } else {
            $(".partial-form-grupos").each(function (e, i) {
                var idTipoCampoCadastro = $(this).attr("id").replace("partialFormGrupo-", "");

                DefaultSistema.carregarConteudo($(this));
            });
        }
    }

    //Executado após retorno do formulário
    this.onSucessForm = function (response) {

        if (response.error == false) {

            AssociadoCampoGrupo.atualizarGruposForm(response.idTipoCampoCadastro);

            AssociadoCampoGrupo.atualizarGruposLista(response.idTipoCampoCadastro);

            AssociadoCampo.atualizarCampos(response.idTipoCampoCadastro);

            return;
        }
    }


    //acionar o formulário de edição de um grupo
    this.editar = function (elemento) {
    
        var idTipoCampoCadastro = $(elemento).data("idtipocampocadastro");

        $.get($(elemento).data("url"), {}, function (response) {

            var idBoxForm = new String("partialFormGrupo-").concat(idTipoCampoCadastro);

            $("#" + idBoxForm).html(response);

        });
    }

    //remover um grupo
    this.excluir = function (elemento) {

        var idTipoCampoCadastro = $(elemento).data("idtipocampocadastro");

        var fYes = function () {

            $.post($(elemento).data("url"), { 'id': $(elemento).data("id") }, function (response) {
                console.log(response);

                if (response.error == true) {

                    jM.error(response.message);
                    return;
                }

                $(elemento).closest("tr").fadeOut("slow");

                AssociadoCampo.atualizarCampos(idTipoCampoCadastro);

                AssociadoCampoGrupo.alterarOrdem(idTipoCampoCadastro);
                
            });
        };
        
        jM.confirmation("Confirma a exclus&atilde;o do registro?", fYes, function () { return false; });
    }

    //Reordenar
    this.alterarOrdem = function (idTipoCampoCadastro) {

        var elemento = $("#partialListaGrupo-" + idTipoCampoCadastro);

        $(elemento).find(".sortable-grupos-campos").sortable({

            placeholder: "ui-state-highlight",

            handle: '.handle.fa-arrows',

            stop: function (event, ui) {

                var pos = ui.item.index() + 1;

                var id = ui.item.data("id");

                var url = $("#baseUrlGeral").val() + "ConfiguracoesAssociados/ConfiguracaoAssociadoCampoGrupo/alterar-ordem";

                $.post(url, { id: id, pos: pos, idTipoCampoCadastro: idTipoCampoCadastro }, function (response) {
                    AssociadoCampo.atualizarCampos(idTipoCampoCadastro);
                });
            }
        });
    }
}


var AssociadoCampoGrupo = new AssociadoCampoGrupoClass();

$(document).ready(function () {
    AssociadoCampoGrupo.init();
});