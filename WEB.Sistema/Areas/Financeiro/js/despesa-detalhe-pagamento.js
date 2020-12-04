function ObjDespesaDetalhePagamento() {

    this.init = function () {
        this.iniciarBoxPagamentos();
    }

    this.iniciarBoxPagamentos = function () {

        var callback = function () { DespesaDetalhePagamento.iniciarEditable(); DefaultSistema.iniciarCheckBoxes(); }

        DefaultSistema.carregarConteudo("#BoxLoadListPagamentos", callback);
    }

    this.iniciarBoxPagamentosExcluidos = function () {

        DefaultSistema.carregarConteudo("#BoxLoadListPagamentosExcluidos");
    }

    this.showPagamentos = function (element, status) {

        var box = $(element).closest(".box-tools")

        $(box).find(".btn-box-tool").css("font-weight", "normal");
        $(box).find(".btn-box-tool").css("font-size", "12px");

        if (status == box.data("status-show")) {
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

        $(".info-editavel-pagamento").each(function () {

            var urlPost = new String($("#baseUrlGeral").val()).concat("financeiro/DespesaDetalhePagamentosOperacao/alterar-dados-pagamentos/");
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

                }, params: function (params) {
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
                    if (response.flagError == true) {
                        if (response.listaErros != undefined && response.listaErros.length > 0) {
                            return response.listaErros.join("<br/>");
                        } else {
                            return "Erro durante a opera&ccedil;&atilde;o";
                        }
                    } else {
                        
                        if ($(this).data("editar-outros") != true) {
                            DespesaDetalhe.iniciarBoxInformacoes();

                            if ($(this).data("refresh-box") == true) {
                                DespesaDetalhePagamento.iniciarBoxPagamentos();
                            }
                            return;
                        }

                        DespesaDetalhePagamento.confirmAlterarOutrosPagamentos(
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
            botao1: {
                label: "somente a este",
                className: "btn-default",
                callback: function () {
                    DespesaDetalhe.iniciarBoxInformacoes();
                    if (atualizarBox == true) {
                        DespesaDetalhePagamento.iniciarBoxPagamentos();
                    }
                }
            }
        };

        if (data.flagHabilitarAtualizarTodos == true) {
            buttons.botao2 = {
                label: "em todos os pagamentos",
                className: "btn-primary",
                callback: function () { DespesaDetalhePagamento.alterarOutrosPagamentos("ALL", id, campo, value, data) }
            };
        }

        if (data.flagHabilitarAtualizarProximos == true) {
            buttons.botao3 = {
                label: "a este e nos pr&oacute;ximos pagamentos",
                className: "btn-primary",
                callback: function () { DespesaDetalhePagamento.alterarOutrosPagamentos("NEXT", id, campo, value, data) }
            };
        }

        bootbox.dialog({
            message: "Registro alterado com sucesso.<br/>Deseja aplicar a alteração...",
            title: "Confirma&ccedil;&atilde;o",
            buttons: buttons
        });
    };

    this.alterarOutrosPagamentos = function (tipoEdit, id, campo, value, data) {
        var urlPost = new String($("#baseUrlGeral").val()).concat("Financeiro/DespesaDetalhePagamentosOperacao/alterar-dados-outros-pagamentos/");

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
                        jM.success("Registros alterados com sucesso");
                    }
                }

                DespesaDetalhePagamento.iniciarBoxPagamentos();
                DespesaDetalhe.iniciarBoxInformacoes();
            }
        )
    }

    this.modalCancelarPagamento = function(element) {

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

                    DespesaDetalhe.iniciarBoxInformacoes();
                    DespesaDetalhePagamento.iniciarBoxPagamentos();
                    DespesaDetalhePagamento.iniciarBoxPagamentosExcluidos();
                }
            });
        }

        jM.confirmation("Tem certeza que deseja cancelar a baixa do pagamento?", funcOk, function(){return false});
    }


    this.conciliar = function (element) {

        var url = $(element).data("url");

        $.post(url, {}, function (data) {

            if (!data.error) {

                jM.success(data.message);

                DespesaDetalhe.iniciarBoxInformacoes();
                DespesaDetalhePagamento.iniciarBoxPagamentos();
            }
        })
    }

    this.onSuccessExclusaoDespesaPagamento = function (response) {

        if (response.flagError == undefined || response.flagError == true) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormExcluirDespesaPagamento"));
            DefaultSistema.reiniciarBotao();
            return;
        } else {
            if (response.listaErros != undefined && response.listaErros.Length > 0) {
                jM.success(response.listaErros.join("<br/>"), function () { location.reload(); });
            } else {
                jM.success("Pagamento removido com sucesso.", function () {
                    if ($("#BoxLoadListPagamentos").length > 0) {
                        DespesaDetalhe.iniciarBoxInformacoes();
                        DespesaDetalhePagamento.iniciarBoxPagamentos();
                        DespesaDetalhePagamento.iniciarBoxPagamentosExcluidos();
                        DefaultSistema.removerModais();
                    } else {
                        window.location.reload();    
                    }
                });
            }
        }
    }
}

var DespesaDetalhePagamento = new ObjDespesaDetalhePagamento();

$(document).ready(function () {
    DespesaDetalhePagamento.init();
});