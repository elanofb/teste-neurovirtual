function ObjReceitaDetalhePagamento() {

    this.init = function () {
        this.iniciarBoxPagamentos();
    }

    this.iniciarBoxPagamentos = function () {

        var callback = function () {

            ReceitaDetalhePagamento.iniciarEditable();

            EditableCustom.listenerEditables();

            DefaultSistema.iniciarCheckBoxes();

        }

        DefaultSistema.carregarConteudo("#BoxLoadListPagamentos", callback)
    }

    this.iniciarBoxPagamentosExcluidos = function () {

        DefaultSistema.carregarConteudo("#BoxLoadListPagamentosExcluidos");
    }

    this.showPagamentos = function (element, status) {

        var box = $(element).closest(".box-tools")

        var selected = box.data("status-show");

        $(box).find(".btn-box-tool").css("font-weight", "normal");
        $(box).find(".btn-box-tool").css("font-size", "12px");

        if (status == selected) {
            console.log(selected + "a");
            $("#boxListaPagamentos").find("div[data-status=PG]").show();
            $("#boxListaPagamentos").find("div[data-status=EA]").show();
            box.data("status-show", "ALL");

            return;
        }

        $(element).css("font-weight", "bold");
        $(element).css("font-size", "14px");
        box.data("status-show", status);

        $("#boxListaPagamentos").find("div[data-status=PG]").hide();
        $("#boxListaPagamentos").find("div[data-status=EA]").hide();
        $("#boxListaPagamentos").find("div[data-status=" + status + "]").show();
    }

    this.iniciarEditable = function () {

        var urlPost = new String($("#baseUrlGeral").val()).concat("Financeiro/ReceitaDetalhePagamentosOperacao/alterar-dados-pagamentos/");

        $(".info-editavel-pagamento").each(function () {

            $(this).editable({
                container: 'body',
                emptytext: 'N&atilde;o Informado',
                url: urlPost,
                display: function (value, sourceData) {
                    if ($(this).data("alt") == "decimal") {

                        var valor = parseFloat(String(value).replace('.', '').replace(',', '.'));
                        if (valor > 0) {
                            $(this).text(valor.toLocaleString('pt-BR', { minimumFractionDigits: 2 }));
                        }
                    } else if ($(this).data("type") == "select") {

                        var valorSelect = $.fn.editableutils.itemsByValue(value, sourceData)[0];

                        if (valorSelect) {
                            $(this).text(valorSelect.text);
                        }
                    } else {
                        $(this).text(value);
                    }
                },
                params: function (params) {
                    var data = {};
                    data['pk'] = params.pk;
                    data['value'] = params.value;
                    data['name'] = params.name;
                    data['nomeCampoDisplay'] = $(this).data("title");

                    if ($(this).data("type") == "select") {
                        var valorSelect = $.fn.editableutils.itemsByValue(params.value, $(this).data("source"))[0];
                        data['oldValue'] = $(this).text();
                        data['newValue'] = valorSelect.text;
                    }

                    return data;
                },
                success: function (response, newValue) {
                    if ((response.flagError != 'undefined' && response.flagError == true)) {
                        if (response.listaErros != undefined && response.listaErros.length > 0) {
                            return response.listaErros.join("<br/>");
                        } else {
                            return "Erro durante a opera&ccedil;&atilde;o";
                        }
                    } else {
                        
                        if ($(this).data("editar-outros") != true) {
                            ReceitaDetalhe.iniciarBoxInformacoes();

                            if ($(this).data("refresh-box") == true) {
                                ReceitaDetalhePagamento.iniciarBoxPagamentos();
                            }
                            return;
                        }

                        ReceitaDetalhePagamento.confirmAlterarOutrosPagamentos(
                            newValue,
                            $(this).data("pk"),
                            $(this).data("name"),
                            $(this).data("refresh-box"),
                            response);
                    }
                },
            });
        });

        $('.info-editavel-pagamento').on('shown', function (e, editable) {
            var alt = $(this).attr("data-alt");
            if (typeof alt !== 'undefined') {
                editable.input.$input.setMask(alt);
                if (alt == "date") {
                    var options = new Array();
                    options['language'] = 'pt-BR';
                    options['format'] = 'dd/mm/yyyy';
                    $('.popover-content input').datepicker(options);
                }
            }
        });
    };


    this.confirmAlterarOutrosPagamentos = function (value, id, campo, atualizarBox, data) {

        if (data.flagHabilitarAtualizarTodos != true && data.flagHabilitarAtualizarProximos != true) {
            jM.success("Registro alterado com sucesso!");
            return;
        }

        var buttons = {
            botao3: {
                label: "somente a este",
                className: "btn-default",
                callback: function () {
                    ReceitaDetalhe.iniciarBoxInformacoes();
                    if (atualizarBox == true) {
                        ReceitaDetalhePagamento.iniciarBoxPagamentos();
                    }
                }
            }
        };

        if (data.flagHabilitarAtualizarTodos == true) {
            buttons.botao1 = {
                label: "em todos os pagamentos",
                className: "btn-primary",
                callback: function () { ReceitaDetalhePagamento.alterarOutrosPagamentos("ALL", id, campo, value, data) }
            };
        }

        if (data.flagHabilitarAtualizarProximos == true) {
            buttons.botao2 = {
                label: "a este e nos pr&oacute;ximos pagamentos",
                className: "btn-primary",
                callback: function () { ReceitaDetalhePagamento.alterarOutrosPagamentos("NEXT", id, campo, value, data) }
            };
        }

        bootbox.dialog({
            message: "Registro alterado com sucesso.<br/>Deseja aplicar a alteração...",
            title: "Confirma&ccedil;&atilde;o",
            buttons: buttons
        });
    };

    this.alterarOutrosPagamentos = function (tipoEdit, id, campo, value, data) {
        var urlPost = new String($("#baseUrlGeral").val()).concat("Financeiro/ReceitaDetalhePagamentosOperacao/alterar-dados-outros-pagamentos/");

        $.post(urlPost,
            {
                tipoEdit: tipoEdit,
                id: id,
                value: value, campo: campo,
                nomeCampoDisplay: data.nomeCampoDisplay,
                oldValue: data.oldValue,
                newValue: data.newValue
            },
            function (response) {
                if ((response.flagError != 'undefined' && response.flagError == true)) {

                    if (response.listaErros != undefined && response.listaErros.length > 0) {
                        jM.error(response.listaErros.join("<br/>"));
                    } else {
                        jM.error("Erro durante a opera&ccedil;&atilde;o");
                    }
                } else {
                    if (response.listaErros != undefined && response.listaErros.Length > 0) {
                        jM.success(response.listaErros.join("<br/>"));
                    } else {
                        jM.success("Registros alterados com sucesso");
                    }
                }

                ReceitaDetalhePagamento.iniciarBoxPagamentos();
                ReceitaDetalhe.iniciarBoxInformacoes();
            }
        )
    }

    this.modalCancelarPagamento = function(element, pagina) {

        var id = $(element).data("id");
        var url = $(element).data("url");

        var funcOk = function() {
            $.post(url, { "id": id }, function (response) {

                if (response.flagError == true) {
                    if (response.listaErros != undefined && response.listaErros.length > 0) {
                        jM.error(response.listaErros.join("<br/>"));
                    } else {
                        jM.error("Erro durante a opera&ccedil;&atilde;o");
                    }
                } else {
                    if (response.listaErros != undefined && response.listaErros.Length > 0) {
                        jM.success(response.listaErros.join("<br/>"));
                    } else {
                        jM.success("Baixa do pagamento cancelamento com sucesso.");
                    }

                    if (pagina == "editar") {
                        ReceitaDetalhe.iniciarBoxInformacoes();
                        ReceitaDetalhePagamento.iniciarBoxPagamentos();
                        ReceitaDetalhePagamento.iniciarBoxPagamentosExcluidos();
                    } else {
                        ReceitaDetalheListar.atualizarTabelaItens(id);
                    }
                }
            });
        }

        jM.confirmation("Tem certeza que deseja cancelar o pagamento?", funcOk, function(){return false});
    }


    this.conciliar = function (element) {

        var url = $(element).data("url");

        $.post(url, {}, function (data) {

            if (!data.error) {

                jM.success(data.message);

                ReceitaDetalhe.iniciarBoxInformacoes();
                ReceitaDetalhePagamento.iniciarBoxPagamentos();
            }
        })
    }

    this.onSuccessExclusaoReceitaPagamento = function(response) {
        
        if (response.flagError == undefined || response.flagError == true) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormExcluirReceitaPagamento"));
            DefaultSistema.reiniciarBotao();
            return;
        } else {
            if (response.listaErros != undefined && response.listaErros.Length > 0) {
                jM.success(response.listaErros.join("<br/>"), function () { location.reload(); });
            } else {
                jM.success("Pagamento removido com sucesso.", function () {
                    if ($("#BoxLoadListPagamentos").length > 0) {
                        ReceitaDetalhe.iniciarBoxInformacoes();
                        ReceitaDetalhePagamento.iniciarBoxPagamentos();
                        ReceitaDetalhePagamento.iniciarBoxPagamentosExcluidos();
                        DefaultSistema.removerModais();
                    } else {
                        window.location.reload();
                    }
                });
            }
        }
    }


    ////
    //this.modelCancelarPagamento = function (element, pagina) {

    //    var id = $(element).attr("data-id");
    //    var url = $(element).attr("data-url");

    //    var dados = { "id": id }

    //    var func = function (data) {

    //        if (!data.error) {

    //            jM.success(data.message);

    //            if (pagina == "listar") {
    //                AReceberListar.atualizarTabelaItens(id);
    //            } else {
    //                DefaultSistema.carregarConteudo($("#BoxLoadDadosEditar"));
    //                DefaultSistema.carregarConteudo($("#BoxLoadListPagamentos"));
    //                DefaultSistema.reiniciarBotao();
    //                DefaultSistema.iniciarPlugins();
    //            }
    //        }
    //    };

    //    var confirmCancelamentoPagamento = "Tem certeza que deseja cancelar o pagamento?";

    //    Ajax.init(url, dados, func, confirmCancelamentoPagamento);
    //}

    ////
    //this.onSuccessForm = function (response) {

    //    try {
    //        DefaultSistema.iniciarPluginsAposAjax($("#BoxLoadListPagamentos"));
    //        if (response.flagSucesso == true) {
    //            DefaultSistema.carregarConteudo($("#BoxLoadListPagamentos"));
    //            DefaultSistema.carregarConteudo($("#BoxLoadDadosEditar"));

    //        }
    //    } catch (e) {
    //        console.log(e);
    //    }
    //};

    ////
    //this.excluirTitulo = function (id, urlRetorno) {
    //    var url = new String($("#baseUrlGeral").val()).concat("financeiro/areceber/excluir/");

    //    var func = function (retorno) {
    //        if (retorno) {

    //            var acao = function () {
    //                location.href = urlRetorno;
    //            }

    //            jM.success("Receita excluída com sucesso!", acao, "AVISO");
    //        }
    //    };

    //    Ajax.init(url, { "id": id }, func, Vocabulary.confirmDelete);
    //};

    ////
    //this.confirmaEAtualiza = function (func, msg) {

    //    bootbox.dialog({
    //        message: msg,
    //        title: "Confirma&ccedil;&atilde;o",
    //        buttons: {

    //            botao1: {
    //                label: "TODAS RECEITAS",
    //                className: "btn-primary bt-todos",
    //                callback: function () {
    //                    return func('T');
    //                }
    //            },
    //            botao3: {
    //                label: "ESTA E AS PRÓXIMAS",
    //                className: "btn-primary bt-proximos",
    //                callback: function () {
    //                    return func('P');
    //                }
    //            },
    //            botao2: {
    //                label: "SOMENTE ESTA",
    //                className: "btn-danger bt-apenas-este",
    //                callback: function () {
    //                    return func('U');
    //                }
    //            }
    //        }
    //    });

    //};

    ////
    //this.modelConciliar = function (element) {

    //    var func = function (data) {

    //        if (!data.error) {

    //            jM.success(data.message);

    //            DefaultSistema.carregarConteudo($("#BoxLoadDadosEditar"));
    //            DefaultSistema.carregarConteudo($("#BoxLoadListPagamentos"));
    //            DefaultSistema.reiniciarBotao();
    //            DefaultSistema.iniciarPlugins();
    //        }
    //    };

    //    var url = $(element).attr("data-url");

    //    Ajax.init(url, {}, func);
    //}



    //this.iniciarBoxPagamentos = function () {

    //    var fnCallback = function () {

    //        $("#boxListaPagamentos").slimScroll({
    //            height: 400,
    //        })

    //    }

    //    DefaultSistema.carregarConteudo("#BoxLoadListPagamentos", fnCallback)

    //}
}

var ReceitaDetalhePagamento = new ObjReceitaDetalhePagamento();

$(document).ready(function () {
    ReceitaDetalhePagamento.init();
});