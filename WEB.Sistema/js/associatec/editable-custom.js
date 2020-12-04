function EditableCustomClass() {

    this.init = function () {

    };

    //Ouviente dos editables
    this.listenerEditables = function () {
        
        $(".info-editavel-default").each(function () {

            var infoElemento = $(this);

            EditableCustom.iniciarEditable(infoElemento);

        });

        $('.info-editavel-default').on('shown', function (e, editable) {

            var elementoEditable = $(this);

            EditableCustom.listenerShowEditables(elementoEditable, editable);

        });
    };

    //Ouvinte para editables especificos para editores de textos
    this.listenerEditableEditor = function () {
        
        $(".editable-texteditor").on("click", function (e) {

            e.stopPropagation();

            e.preventDefault();

            var elemento = $(this);

            var targetBox = elemento.data("target");

            var targetEditable = $(targetBox);

            var conteudoAtual = targetEditable.html();

            var textoPadrao = targetEditable.data("default-text");

            var urlPost = targetEditable.data("url");

            var pk = targetEditable.data("pk");

            var value = targetEditable.data("value");

            var name = targetEditable.data("name");

            var nomeCampoDisplay = targetEditable.data("title");


            $.post(urlPost, {
                'pk': pk,
                'value': conteudoAtual,
                'name': name,
                'nomeCampoDisplay': nomeCampoDisplay,
                'targetBox': targetBox

            }, function (response) {

                var Modal = $(response).modal();

                $(Modal).on("shown.bs.modal", function (e) {

                    FroalaCustom.listenerEditores();

                    $(Modal).find(".link-loading").button('reset');

                    $(Modal).find(".link-loading").on('click', function () {

                        var btn = $(this).button().data('loading-text', 'Processando...');

                        btn.button('loading');
                    });

                });

                $(Modal).on("hidden.bs.modal", function (e) {
                    $(this).remove();
                });
            });

        });
    }

    //Iniciar links com opção de edicao
    this.iniciarEditable = function (infoElemento) {

        var textoPadrao = infoElemento.data("default-text");

        var urlPost = infoElemento.data("url");

        var alt = infoElemento.data("alt");

        var tipoCampo = infoElemento.data("type");

        var dataPlacement = infoElemento.data("placement");

        var dataSourceCache = infoElemento.data("cache");

        var dataCallbackSuccess = infoElemento.data("callback-success");

        if (textoPadrao == '' || typeof(textoPadrao) == 'undefined') {

            textoPadrao = ".....";

        }

        if (dataPlacement == '' || typeof (dataPlacement) == 'undefined') {

            dataPlacement = "top";

        }

        if (dataSourceCache == '' || typeof (dataSourceCache) == 'undefined') {

            dataSourceCache = false;

        }

        $(infoElemento).editable({
            container: 'body',
            emptytext: textoPadrao,
            sourceCache: dataSourceCache,
            url: urlPost,
            placement: dataPlacement,
            display: function (value, sourceData) {

                if (alt == "decimal") {

                    var valor = parseFloat(String(value).replace('.', '').replace(',', '.'));

                    if (valor > 0) {

                        $(this).text(valor.toLocaleString('pt-BR', { minimumFractionDigits: 2 }));

                    }

                } else if (tipoCampo == "select") {

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
                data['nomeCampoDisplay'] = infoElemento.data("title");


                return data;
            },
            success: function (response, newValue) {

                console.log(response);

                infoElemento.data("value", newValue);

                if (dataCallbackSuccess != '' && typeof (dataCallbackSuccess) != 'undefined') {
                    eval(dataCallbackSuccess);
                }

                if (response.error != 'undefined' && response.error == true) {

                    toastr.options.positionClass = "toast-top-right";

                    toastr.error(response.message, 'Erro!', { timeOut: "8000" });

                    return "";
                }

                toastr.options.positionClass = "toast-top-right";

                toastr.success(response.message, 'Sucesso!', { timeOut: "8000" });


            }
        });

    };

    //Acoes no ato da abertura do editable
    this.listenerShowEditables = function (elementoEditable, editable) {

        var alt = $(elementoEditable).data("alt");

        var widthCampo = $(elementoEditable).data("width");

        var maxlengthCampo = $(elementoEditable).data("maxlength");

        console.log(alt);

        if (typeof alt !== 'undefined') {

            editable.input.$input.setMask(alt);

            if (alt == "date") {

                var options = new Array();

                options['language'] = 'pt-BR';

                options['format'] = 'dd/mm/yyyy';

                $('.popover-content input').datepicker(options);
            }

            if (alt == "phone") {

                editable.input.$input.on('keyup', function () {

                    if ($(this).val().length > 14) {
                        $(this).setMask("(99) 99999-9999");
                    } else {
                        $(this).setMask({ mask: "(99) 9999-99999", autoTab: false });
                    }

                });

            }

        }

        if (typeof widthCampo !== 'undefined' && widthCampo != "") {

            editable.input.$input.css("width", (widthCampo + "px"));

        }

        if (typeof maxlengthCampo !== 'undefined' && maxlengthCampo != "") {

            editable.input.$input.attr("maxlength", maxlengthCampo);

        }
    }

    //Retorno da postagem do form
    this.onSuccessForm = function (response) {

        console.log(response);

        if (response.error === true) {

            toastr.options.positionClass = "toast-top-right";
            
            toastr.error(response.message, 'Erro!', { timeOut: "8000" });

            return;
        }
        
        if (response.error === false) {

            DefaultSistema.removerModais();

            toastr.options.positionClass = "toast-top-right";

            toastr.success(response.message, 'Sucesso!', { timeOut: "8000" });

            var targetBox = response.targetBox;

            if (targetBox != '' && targetBox != 'undefined') {

                $(targetBox).html(response.value);
                $(targetBox).attr("data-value", response.value);

            }

            return;
        }

        FroalaCustom.listenerEditores();

    }
};

var EditableCustom = new EditableCustomClass();

$(document).ready(function () {
    EditableCustom.init();
});
