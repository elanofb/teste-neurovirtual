function ObjAutoComplete() {
    var quantityItems = 3;
    var title = "";
    var url = "";
    var quietmillis = 100;
    var extraParams = "";

	/**
	*
	*/
    this.init = function () {
        this.iniciarSelect2();
    };

    /**
    *
    */
    this.iniciarSelect2 = function (conteudo) {
        if (!conteudo) {
            conteudo = $('body');
        }

        conteudo.find('.select2').each(function () {
            var element = $(this);
            AppAutoComplete.carregarSelect2(element);
        });
    }

    /**
    *
    */
    this.carregarSelect2 = function (elemento) {
        var titulo = $(elemento).data("title");
        var minLenght = $(elemento).data("min-length");

        $(elemento).select2({

            minimumInputLength: minLenght,
            width: '100%',
            dropdownCssClass: "bigdrop",
            allowClear: true,
            placeholder: "Selecione..."
        });
    }

	/**
	*
	*/
    this.loadSelect2 = function (element, callbackSelection, callbackNotFound, callbackLayout, noResults) {

        element.select2({
            width: '100%',
            placeholder: this.title,
            minimumInputLength: this.quantityItems,
            dropdownCssClass: "bigdrop",
            allowClear: true,
            language: {
                noResults: function () { return  DefaultSistema.checkNull(noResults, "Nenhum resultado foi encontrado."); },
                inputTooShort: function () { return 'Informe no m&iacute;nimo ' + AppAutoComplete.quantityItems + ' carateres para realizar a busca.'; }
            },
            ajax: {
                delay: 400,
                url: this.url,
                dataType: 'json',
                quietMillis: 100,
                data: function (params) {
                    return {
                        term: params.term,
                        page: params.page,
                        extra: extraParams,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data,
                        pagination: { more: (params.page * 30) < data.total_count }
                    };
                }
            },

            formatResult: function (data) {
                // <-- Chamado quando buscamos os elementos
                if (typeof callbackLayout == 'function') {
                    return callbackLayout(data);
                } else {
                    var layout = "<table class='list-result'><tr>";
                    if (data.imagem !== undefined && data.imagem !== undefined) {
                        layout += "<td class='list-image'><img src='" + data.imagem + "'/></td>";
                    }
                    layout += "<td class='list-info'><div class='list-title'>" + data.value + "</div></td></tr></table>";
                    return layout;
                }
            },

            formatSelection: function (data) {
                
                if (typeof callbackSelection == 'function') {
                    callbackSelection(data);
                }
                return data.value;
            },

            formatNoMatches: function (term) {
                if (typeof callbackNotFound == 'function') {
                    return callbackNotFound(term);
                } else {
                    return "Nenhum registro foi encontrado.";
                }
            },

            escapeMarkup: function (m) {
                return m;
            }
        });

        //Evento de onchange do select2
        element.on("select2:select", function (e) {  });
    }
}

var AppAutoComplete = new ObjAutoComplete();

$(document).ready(function(){
    AppAutoComplete.init();
});