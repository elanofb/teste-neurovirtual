function ObjRecurso() {

    this.init = function () {

        this.initTree();

        this.initContextMenu();
    };

    //M�todo para inicializa��o e defini��o de plugins do jstree com defini��o de callback para drag on drop
    this.initTree = function () {

        var tree = $(".boxTree").jstree({
            "core": { "check_callback": true },
            "checkbox": { "keep_selected_style": false, real_checkboxes: true, two_state: true },
            "plugins": ["dnd", "checkbox", "ui"]
        });

        $('li[data-checked="false"] a').removeClass("jstree-clicked");

        tree.bind("check_node.jstree", function (e, data) { Recurso.checkNodeListener(e, data); });

        tree.bind("move_node.jstree", function (e, data) { Recurso.moveNodeListener(e, data); });

    };

    //Ouvinte do evento click dos n�s da �rvore de recursos
    this.checkNodeListener = function (e, data) {
        console.log(data);
    };

    //Ouvinte do evento move dos n�s da �rvore de recursos
    this.moveNodeListener = function (e, data) {
        var position = data.position;
        var idRecursoPai = data.parent;
        var idRecurso = data.node.id;
        var idRecursoGrupo = data.node.data.idGrupo;

        console.log(data);
        console.log(idRecursoGrupo);
        var callback = function (response) {
            return false;
        };

        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + 'permissao/inicio/reordenar-recurso', { "idRecurso": idRecurso, "idRecursoPai": idRecursoPai, "idRecursoGrupo": idRecursoGrupo, "position": position }, callback, null);
    };

    //Inicializa��o do menu de contexto
    this.initContextMenu = function () {
        $.contextMenu({
            selector: '.context-menu',
            items: {
                "add": { name: "Novo Item", icon: "add" },
                "edit": { name: "Ver/Editar", icon: "edit" },
                "delete": { name: "Excluir", icon: "delete" },
                "sep1": "---------",
                "quit": { name: "Fechar", icon: "quit" }
            },
            callback: function (key, options) {
                var itemSource = this.context;
                var idPerfilAcesso = $("#idPerfilAcesso").val();
                idPerfilAcesso = (typeof (idPerfilAcesso) == 'undefined' ? 0 : idPerfilAcesso);

                var idRecursoPai = $(itemSource).attr("data-id-pai");
                idRecursoPai = (typeof (idRecursoPai) == 'undefined' ? 0 : idRecursoPai);

                var id = $(itemSource).attr("data-id");
                var idRecursoGrupo = $(itemSource).attr("data-id-grupo");
                var urlContent = $(itemSource).attr("data-url");

                if (key == "delete") {
                    var Assync = new ObjAjax();
                    var url = new String($("#baseUrlGeral").val()).concat("permissao/inicio/excluir-recurso");
                    Assync.init(url, { "id": id }, null, null);
                    Recurso.loadMenus(idRecursoGrupo);
                } else {
                    //Inclus�o e edi��o de recursos
                    if (key == "add") {
                        idRecursoPai = id;
                        id = 0;
                    }

                    var queryString = "?id=" + id + "&idRecursoPai=" + idRecursoPai + "&idRecursoGrupo=" + idRecursoGrupo + '&idPerfilAcesso=' + idPerfilAcesso;
                    var url = new String($("#baseUrlGeral").val()).concat("permissao/inicio/editar-recurso").concat(queryString);

                    $.get(url, function (dataContent) {
                        var modal = $(dataContent).modal({ title: "Recursos do Sistema" });

                        modal.on('shown.bs.modal', function (e) {
                            DefaultSistema.iniciarLinksAcao();
                        });

                        modal.on('hidden.bs.modal', function (e) {
                            $("#myModal").remove();
                        });
                    });
                }
            }
        });
    }

    //Carregamento das �reas do sistema com base no id do grupo de recurso informado
    this.loadMenus = function (idRecursoGrupo, idPerfilAcesso) {

        var baseUrl = new String($("#baseUrlGeral").val()).concat("permissao/inicio/exibir-menus-grupo?idRecursoGrupo=");

        $('div:jstree').each(function () {

            var div = $(this);

            $(div).jstree('destroy');

            var idGrupo = $(div).data("grupo-id");

            var url = baseUrl.concat(idGrupo).concat("&idPerfilAcesso=").concat(idPerfilAcesso);

            $.get(url, function (dataContent) {

                $(div).html(dataContent);

                var tree = $(div).jstree({
                    "core": { "check_callback": true },
                    "checkbox": { "keep_selected_style": false, real_checkboxes: true, two_state: true },
                    "plugins": ["dnd", "checkbox", "ui"]
                });

                tree.bind("check_node.jstree", function (e, data) { Recurso.checkNodeListener(e, data); });

                tree.bind("move_node.jstree", function (e, data) { Recurso.moveNodeListener(e, data); });
            });

        });
        //var url = new String($("#baseUrlGeral").val()).concat("permissao/inicio/exibir-menus-grupo?idRecursoGrupo=").concat(idRecursoGrupo).concat("&idPerfilAcesso=").concat(idPerfilAcesso);

        //$.get(url, function (dataContent) {

        //    var div = $("#recurso-grupo-" + idRecursoGrupo);

        //    $(div).jstree('destroy');

        //    $(div).html(dataContent);

        //    var tree = $(div).jstree({
        //        "core": { "check_callback": true },
        //        "checkbox": { "keep_selected_style": false, real_checkboxes: true, two_state: true },
        //        "plugins": ["dnd", "checkbox", "ui"]
        //    });


        //    tree.bind("check_node.jstree", function (e, data) { Recurso.checkNodeListener(e, data); });

        //    tree.bind("move_node.jstree", function (e, data) { Recurso.moveNodeListener(e, data); });
        //});
    };

    //Carregamento de recursos de acordo com o grupo escolhido
    this.loadCombo = function (sufixElement, selectedItem) {
        var idRecursoGrupo = $("select[rel=idRecursoGrupo" + sufixElement + "]").val();

        var callback = function (data) {

            if (data.length > 0) {
                var combo = $("select[rel=idRecurso" + sufixElement + "]");

                if (typeof (ComboSelect) == 'function' || typeof (ComboSelect) == 'object') {
                    ComboSelect.loadSelectBox(combo, data, selectedItem);
                }
            }
        }

        var Assync = new ObjAjax();
        var url = new String($("#baseUrlGeral").val()).concat("permissao/inicio/carregar-recursos-ajax");
        Assync.init(url, { "idRecursoGrupo": idRecursoGrupo }, callback, null);
    };

    //M�todo para salvar os actions informados atrav�s de modal
    this.saveAction = function () {
        var idPerfilAcesso = $("#idPerfilAcesso").val();
        var idRecursoGrupo = $("#idRecursoGrupo").val();
        var idRecursoPai = $("#idRecursoPaiAcao").val();
        var descricaoAcao = $("#descricaoAcao").val();
        var areaAcao = $("#areaAcao").val();
        var controleAcao = $("#controleAcao").val();
        var nomeAcao = $("#nomeAcao").val();
        var metodoAcao = $("#metodoAcao").val();

        idRecursoGrupo = ($.isNumeric(idRecursoGrupo) ? idRecursoGrupo : 0 );
        idRecursoPai = ($.isNumeric(idRecursoPai) ? idRecursoPai : 0);

        var callback = function (response) {
            if (response.error) {
                jM.error(response.message);
                return false;
            }
            var url = $("#baseUrlGeral").val() + 'permissao/inicio/exibir-actions?idRecurso=' + idRecursoPai + '&idRecursoGrupo=' + idRecursoGrupo + '&idPerfilAcesso=' + idPerfilAcesso;
            $.get(url, function (dataContent) {
                var div = $("#boxActions");
                $(div).html(dataContent);
                DefaultSistema.iniciarLinksAcao();
            });
        };

        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + 'permissao/inicio/editar-action',
                    {
                        "idRecursoGrupo": idRecursoGrupo,
                        "idRecursoPai": idRecursoPai,
                        "idPerfilAcesso": idPerfilAcesso,
                        "descricaoAcao": descricaoAcao,
                        "areaAcao": areaAcao,
                        "controleAcao": controleAcao,
                        "nomeAcao": nomeAcao,
                        "metodoAcao": metodoAcao
                    }, callback, null, 'post', 'json', false);
    };

    //
    this.savePermissoes = function () {
        var itens = new Array;

        //Lista os recursos principais, quando os filhos deste, est�o selecionados parcialmente
        $('i.jstree-undetermined').each(function () {

            var parentElement = $(this).closest("li");

            var id = parentElement.attr("data-id");
            var idGrupo = parentElement.attr("data-id-grupo");
            var recurso = { id: id, idRecursoGrupo: idGrupo, idRecursoPai: 0, nome: '' };
            itens.push(recurso);
        });

        $('a.jstree-clicked').each(function () {
            var parentElement = $(this).parent();
            var id = parentElement.attr("data-id");
            var idGrupo = parentElement.attr("data-id-grupo");
            var recurso = { id: id, idRecursoGrupo: idGrupo, idRecursoPai: 0, nome: '' };
            itens.push(recurso);
        });

        console.log(itens);

        var idPerfilAcesso = $("#idPerfilAcesso").val();
        var dadosPost = {
            idPerfilAcesso: idPerfilAcesso,
            listaRecursos: itens
        };

        var urlPost = $("#baseUrlGeral").val() + "permissao/inicio/salvar-permissoes";

        $.ajax({ type: 'POST', url: urlPost,
            data: dadosPost,
            //contentType: 'application/json',
            success: function (response) {
                if (response.error) {
                    jM.error(response.message);
                    return false;
                } else {
                    jM.success(response.message);
                    DefaultSistema.reiniciarBotao();
                }
            },
            dataType: 'json'
        });
    }

    //
    this.salvarPermissaoAcao = function (elementCheck, idRecursoAcao, idPerfilAcesso) {
        var itens = new Array;
        var flagIncluir = $(elementCheck).is(":checked");
        alert(flagIncluir)
        var urlPost = $("#baseUrlGeral").val() + "permissao/inicio/salvar-permissoes-acao";

        $.post(urlPost,{ idPerfil : idPerfilAcesso, idRecursoAcao:idRecursoAcao, flagIncluir:flagIncluir },
             function (response) {
                if (response.error) {
                    jM.error(response.message);
                } else {
                    jM.success(response.message);
                }
            });
    }

    //
    this.limparFormModal = function (elementForm) {
        var form = $(elementForm);
        $("#boxActions").remove();
        form.find("input[type=text]").val('');
        form.find("input[name=id]").val('');
        form.find("div.validation-summary-errors").remove();
        form.prev("div.alert").remove();
    }
};

var Recurso = new ObjRecurso();

$(document).ready(function () {
    Recurso.init();
});

