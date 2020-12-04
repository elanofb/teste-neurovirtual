function ObjAvisoNotificacaoAssociado() {

    this.init = function () {
        $(".content-load-associados").each(function () {
            DefaultSistema.carregarConteudo($(this), AvisoNotificacaoAssociado.autoCompleteNotificacao);
        });
    };

    /**
	* Autocompletar lista de associados
	*/
    this.autoCompleteNotificacao = function () {
        
        var element = $("#novoAssociadoEspecifico");
        
        element.select2({
            width: '100%',
            dropdownCssClass: "bigdrop",
            allowClear: true,
            placeholder: element.data("title"),

            language: {
                noResults: function () {
                    return "<a href='javascript:;' id='newAssociado'>Nenhum registro encontrado.</a>";
                },
                inputTooShort: function () {
                    return 'Informe o nome do associado';
                }
            },
            escapeMarkup: function (markup) {
                return markup;
            },
            ajax: {
                url: element.data("url"),
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return {
                        term: params.term, page: params.page
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data.items,
                        pagination: { more: (params.page * 30) < data.total_count }
                    };
                }
            },
            minimumInputLength: 3
        });

        //Evento de onchange do select2
        element.on("select2:select", function (e) {
            $("#idAssociadoEspecifico").val(e.params.id);
            //$("#nomeAssociadoEspecifico").val(e.params.value);
            //$("#cnpfAssociadoEspecifico").val(e.params.nroDocumento);
            //$("#emailAssociadoEspecifico").val(e.params.emailPrincipal);
        });
        
    };

    this.retornoAposAdicionar = function () {
        DefaultSistema.iniciarPluginsAposAjax();
        AvisoNotificacao.verificarExibicaoBlocosEspecificos();
        AvisoNotificacaoAssociado.autoCompleteNotificacao();
    }
};


var AvisoNotificacaoAssociado = new ObjAvisoNotificacaoAssociado();

$(document).ready(function () {
    AvisoNotificacaoAssociado.init();
});
