function ObjContasAPagar() {

    this.init = function () {
        this.autoCompleteCliente();
        this.iniciarDatepicker();
    }

    this.iniciarDatepicker = function () {
        $("#dtEmissao").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true });

        //$("#competencia").daterangepicker({ format: 'MM/YYYY', singleDatePicker: true });
    };

    this.autoCompleteCliente = function (element) {

        var element = $("#nomeFornecedor"); //<--chama o campo texto para torna-lo um auto complete
        element.select2({
            placeholder: element.attr("data-title"), minimumInputLength: 3,
            width: 'copy', dropdownCssClass: "bigdrop", allowClear: true,
            ajax: {
                url: element.attr("data-url"),
                dataType: 'json',
                quietMillis: 100,
                data: function (term, page) {
                    return { term: term, idPessoa: 0 };
                },
                results: function (data, page) {
                    return { results: data };
                }
            },

            initSelection: function (obj, callback) {
                console.log($(obj));
                console.log($("#idFornecedor").val());
                $.get($(obj).attr("data-url"), { idFornecedor: $("#idFornecedor").val() },
					function (response) {
					    console.log(response[0]);
					    callback(response[0]);
					}
				);

            },
            formatResult: function (data) {
                var layout = "<table class='list-result'><tr>";
                if (data.imagem !== undefined && data.imagem !== undefined) {
                    layout += "<td class='list-image'><img src='" + data.imagem + "'/></td>";
                }
                layout += "<td class='list-info'><div class='list-title'>" + data.value + "</div></td></tr></table>";
                return layout;
            },
            formatSelection: function (data) {

                $("#idFornecedor").val(data.id);
                $("#nomePessoa").val(data.value);
                $("#documentoPessoa").val(data.cnpf);

                return data.value;
            },
            formatNoMatches: function (term) {
                return "<a href='#' id='newClient'>Nenhum registro encontrado para (" + term + ")</a>";
            }
        });

    };

    
}

var ContasAPagar = new ObjContasAPagar();

$(document).ready(function () {

    ContasAPagar.init();
});