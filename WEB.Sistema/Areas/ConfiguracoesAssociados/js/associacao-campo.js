function AssociadoCampoClass() {

    var BOX_ADD = "add";
    var BOX_EDIT = "edit";
    var BOX_ADM = "adm";
    var BOX_AREA_ASSOCIADO = "area-associado";
    var BOX_HIDDEN = "hidden";
    var BOX_DISABLED = "disabled";

    //Inicializadores
    this.init = function() {

        AssociadoCampo.ativarTabs();

        AssociadoCampo.initMultiSelectView();

        AssociadoCampo.initMultiSelectViewTipoAssociado();

        AssociadoCampo.iniciarConteudoCampos();


        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var target = $(e.target).attr("href") // activated tab
            AssociadoCampo.iniciarConteudoCampos($(target));
        });

        $('body').on("click", ".dropdown-submenu a", function (e) {
            $(this).next('ul').toggle();
            e.stopPropagation();
            e.preventDefault();
        });
    };

    //Ativacao de tabs
    this.ativarTabs = function () {

        $("div.bhoechie-tab-menu>div.list-group>a").click(function (e) {

            e.preventDefault();

            $(this).siblings('a.active').removeClass("active");

            $(this).addClass("active");

            var index = $(this).index();

            $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");

            $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");

            AssociadoCampo.iniciarConteudoCampos($("div.bhoechie-tab>div.bhoechie-tab-content").eq(index));
        });
    }

    //Auto carregamento de campos
    this.iniciarConteudoCampos = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find(".content-load-campos:visible").each(function () {
            DefaultSistema.carregarConteudo($(this));
        });
    }

    //Iniciar modal
    this.initModal = function () {

        AssociadoCampo.initMultiSelectTipoAssociado();

        AssociadoCampo.changeTipoCampo();
    }

    //Iniciar multi select
    this.initMultiSelectView = function () {
        $(".multiSelectViewCampos").multiselect({
            buttonClass: 'btn btn-sm btn-default',
            numberDisplayed: 1,
            buttonWidth: '80%',
            onChange: function () {
                AssociadoCampo.changeViewCampos(this.$select.data("id-tipo-campo-cadastro"));
            }
        });
    }

    //Iniciar Multi select dentro dos modais
    this.initMultiSelectTipoAssociado = function() {
        $(".multiSelecTipoAssociado").multiselect({
            buttonClass: 'btn btn-sm btn-default',
            enableFiltering: true,
            filterBehavior: 'text',
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1
        });
    }

    //Iniciar multi select de tipo de associado na tela de visualizacao
    this.initMultiSelectViewTipoAssociado = function () {

        $(".multiSelectViewCamposTipoAssociado").multiselect({
            buttonClass: 'btn btn-sm btn-default',
            buttonWidth: '80%',
            enableFiltering: true,
            filterBehavior: 'text',
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1,
            onDropdownHide: function (event) {

                var idTipoCampoCadastro = this.$select.data("id-tipo-campo-cadastro");

                AssociadoCampo.atualizarVisualizacaoPorTipoAssociado(idTipoCampoCadastro);

            }
        });
    }

    //Atualizar a visualizacao enviando tipo de associado
    this.atualizarVisualizacaoPorTipoAssociado = function (idTipoCampoCadastro) {

        var comboMulti = $('.multiSelectViewCamposTipoAssociado');

        var itens = $(comboMulti).find("option:selected");

        var idOrganizacao = $(comboMulti).data("id");

        var selected = [];

        $(itens).each(function (index, item) {

            selected.push($(this).val());

        });

        console.log(selected);

        var url = $("#baseUrlGeral").val() + "ConfiguracoesAssociados/ConfiguracaoAssociadoCampos/partial-lista-campos/";

        $.get(url,
            {
                'idsTipoAssociado': selected,
                'idTipoCampoCadastro': idTipoCampoCadastro,
                'idOrganizacao': idOrganizacao
            }, function (response) {

                var boxLista = "#partialListaCampos-" + idTipoCampoCadastro;

                $(boxLista).html(response);

                DefaultSistema.iniciarPluginsAposAjax($(boxLista));

                AssociadoCampo.changeViewCampos(idTipoCampoCadastro);
        });
    }

    //Exibir campos customizáveis para quando o tipo de campo criado for um select
    this.changeTipoCampo = function() {

        var value = $("#idTipoCampo").val();

        if (value == 6 || value == 7) {
            $("#boxDadosCampoSelect").show();

            if (value == 7) {
                $("#boxNameHelper, #boxNameDescription, #boxParamHelper, #boxMethodHelper").show();
            } else {
                $("#boxNameHelper, #boxMethodHelper, #boxParamHelper, #boxNameDescription").hide();
            }
            return;
        }

        $("#boxNameHelper, #boxMethodHelper, #boxDadosCampoSelect, #boxParamHelper, #boxNameDescription").hide();

        AssociadoCampo.initMultiSelectTipoAssociado();
    }

    //Executado após retorno do formulário
    this.onSuccessForm = function (response) {

        if (response.error == false) {

            AssociadoCampo.atualizarCampos(response.idTipoCampoCadastro);

            DefaultSistema.removerModais();

            return;
        }

        AssociadoCampo.initMultiSelectTipoAssociado();
    }

    //remover um grupo
    this.excluir = function (elemento, idTipoCampoCadastro) {

        var fYes = function () {
            $.post($(elemento).data("url"), { 'id': $(elemento).data("id") }, function (response) {
                if (response.flagError != 'undefined' && response.flagError == true) {
                    if (response.listaErros != undefined && response.listaErros.length > 0) {
                        jM.error(response.listaErros.join("<br/>"));
                    } else {
                        jM.error("Erro durante a opera&ccedil;&atilde;o");
                    }
                }
                AssociadoCampo.atualizarCampos(idTipoCampoCadastro)
            });
        };

        jM.confirmation("Confirma a exclus&atilde;o do registro?", fYes, function () {return false;});
    }

    //Buscar campos padrao de sistema
    this.importarDefaultSistema = function(element) {
        
        var funcOk = function() {
            $.post($(element).data("url"), {}, function (response) {

                if (response.flagError != undefined && response.flagError == true) {

                    if (response.listaErros != undefined && response.listaErros.length > 0) {
                        jM.error(response.listaErros.join("<br/>"));
                    } else {
                        jM.error("Erro durante a opera&ccedil;&atilde;o");
                    }
                    return false;
                }
                jM.success("Importa&ccedil;&atilde;o realizada com sucesso", "Sucesso!", function () { location.reload() })
            });
        }
        jM.confirmation("A importação das informa&ccedil;&otilde;es padr&otilde;es do sistema ira apagar todas as que j&aacute; forma cadastradas.<br/>Deseja continuar?", funcOk, function(){});
    }

    //Alterar modo de visualizacao dos campos
    this.changeViewCampos = function (idTipoCampoCadastro) {

        var box = ".box-view"
        var baseClass = box + ".box-";
        var selecteds = $("#listaViewCampos-" + idTipoCampoCadastro).val();
        
        if (selecteds == null || !(selecteds.length > 0)) {
            $(box).show();
            return;
        }

        var existInArray = function (value) { return jQuery.inArray(value, selecteds) != -1 }

        $(box).hide();

        if (existInArray(BOX_ADM)) { $(baseClass + BOX_ADM).show(); }
        if (existInArray(BOX_AREA_ASSOCIADO)) { $(baseClass + BOX_AREA_ASSOCIADO).show(); }
        
        if (existInArray(BOX_ADD) && !existInArray(BOX_EDIT)) { $(baseClass + BOX_EDIT + ":not(.box-" + BOX_ADD + ")").hide(); }
        if (existInArray(BOX_EDIT) && !existInArray(BOX_ADD)) { $(baseClass + BOX_ADD + ":not(.box-" + BOX_EDIT + ")").hide(); }

        if (!existInArray(BOX_HIDDEN)) { $(baseClass + BOX_HIDDEN).hide(); }
        if (!existInArray(BOX_DISABLED)) { $(baseClass + BOX_DISABLED).hide(); }

        if (!existInArray(BOX_AREA_ASSOCIADO) && !existInArray(BOX_ADM)) {

            if (existInArray(BOX_ADD) && !existInArray(BOX_EDIT)) { $(baseClass + BOX_ADD).show(); }
            if (existInArray(BOX_EDIT) && !existInArray(BOX_ADD)) { $(baseClass + BOX_EDIT).show(); }

            if (existInArray(BOX_EDIT) && existInArray(BOX_ADD)) { $(baseClass + BOX_EDIT + ".box-" + BOX_ADD).show(); }

            if (existInArray(BOX_HIDDEN)) { $(baseClass + BOX_HIDDEN).show(); }
            if (existInArray(BOX_DISABLED)) { $(baseClass + BOX_DISABLED).show(); }
        }

        var boxesGrupos = $(".box-grupo");

        $.each(boxesGrupos, function (e, i) {

            var box = $(this);

            box.show();

            var campos = $(this).find(".dropdown:visible");
                        
            if (campos.length == 0) {

                box.hide();

            } else {

                box.show();
            }

        });

    }

    //Atualizar lista de campos configurados
    this.atualizarCampos = function (idTipoCampoCadastro) {

        if (idTipoCampoCadastro > 0) {

            var box = $("#partialListaCampos-" + idTipoCampoCadastro);

            AssociadoCampo.atualizarVisualizacaoPorTipoAssociado(idTipoCampoCadastro)

        } else {
            $(".partial-lista-campos").each(function (e, i) {

                var idTipoCampoCadastro = $(this).attr("id").replace("partial-lista-campos-", "");

                DefaultSistema.carregarConteudo($(this), AssociadoCampo.atualizarVisualizacaoPorTipoAssociado(idTipoCampoCadastro));
            });    
        }
    }


}


var AssociadoCampo = new AssociadoCampoClass();

$(document).ready(function () {
    AssociadoCampo.init();
});