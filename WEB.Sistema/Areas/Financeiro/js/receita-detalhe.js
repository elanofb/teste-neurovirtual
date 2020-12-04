function ReceitaDetalheClass() {
    
    this.init = function () {
        console.log("ReceitaDetalhe.js successfuly charged!");
        ReceitaDetalhe.iniciarBoxInformacoes();
    };

    this.iniciarBoxInformacoes = function () {
        DefaultSistema.carregarConteudo($("#BoxLoadDadosEditar"), function () {
            ReceitaDetalhe.iniciarEditable();
            EditableCustom.listenerEditableEditor();
        });
    };

    this.iniciarEditable = function () {
        var urlPost = new String($("#baseUrlGeral").val()).concat("financeiro/ReceitaDetalheOperacao/alterar-dados/");
        $(".info-editavel").each(
            function () {
            $(this).editable({
                container: 'body',
                emptytext: 'Nenhum',
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

                        var dataSource = $(this).data("source");
                        console.log(dataSource);

                        console.log(params.value);

                        var itensValue = $.fn.editableutils.itemsByValue(params.value, dataSource);
                        console.log(itensValue);

                        var valorSelect = itensValue[0];

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
                        if (response.listaErros != undefined && response.listaErros.Length > 0) {
                            jM.success(response.listaErros.join("<br/>"));
                        }

                        if ($(this).data("refresh-box") == true) {
                            ReceitaDetalhe.iniciarBoxInformacoes();
                        }
                    }
                }
            });
        });

        $('.info-editavel').on('shown', function (e, editable) {

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

    this.onSuccessExclusaoReceita = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormExcluirReceita"));
            DefaultSistema.reiniciarBotao();
            return;
        }

        if (response.error == false) {
            jM.success(response.message, function () { window.location = response.urlRetorno });
        }

        if (response.error == true) {
            jM.error(response.message, function () { window.location = response.urlRetorno });
        }
    }
};
var ReceitaDetalhe = new ReceitaDetalheClass();
$(document).ready(function(){
    ReceitaDetalhe.init();
});