function ObjAutoComplete() {

    var quantityItems = 3;
    var title       = "";
    var url = "";
    var quietmillis = 100;
    var extraParams = "";

	/**
	*
	*/
    this.init = function () {

    };    

	/**
	*
	*/
    this.loadSelect2 = function (element, callbackSelection, callbackNotFound, callbackLayout, callbackInit) {

        element.select2({
            placeholder: this.title,
            minimumInputLength: this.quantityItems,
            width:'copy',
            dropdownCssClass: "bigdrop",
            allowClear: true,
            ajax: {
                url: this.url,
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) { 
                    return {
                    	term: term,
                    	extra: extraParams
                    };
                },
                results: function (data, page) {
                    return { results: data };
                }
            },

            initSelection: function (element, callback) {
                // Quando carrega o componente
                if (typeof callbackInit == 'function')
                    callback(callbackInit());

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

    }
};

var AppAutoComplete = new ObjAutoComplete();

$(document).ready(function(){
    AppAutoComplete.init();
});
