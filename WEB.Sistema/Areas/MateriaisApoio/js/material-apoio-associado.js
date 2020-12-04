function ObjMaterialApoioAssociado() {

    var abaAssociadosEspecificos = "#abaAssociadosEspecificos";
    var comboDisponibilidadeAssociados = "#flagDisponibilidadeAssociado";

    this.init = function () {
        DefaultSistema.carregarConteudo($("#boxAssociadoEspecifico"), MaterialApoioAssociado.autoCompleteMaterialApoio);

        MaterialApoioAssociado.verificarAbaAssociadosEspecificos($(comboDisponibilidadeAssociados));
    };

    /**
	* Autocompletar lista de associados
	*/
    this.autoCompleteMaterialApoio = function () {
        $("#novoAssociadoEspecifico").select2({
            width: 'resolve',
            dropdownCssClass: "bigdrop",
            allowClear: true,
            placeholder: "Selecione...",

            //Apresenta o link para cadastrar um novo devedor quando não encontrar resultados
            
            language: {
                noResults: function () { return "Sem resultados. "; },
                inputTooShort: function () { return 'Informe o nome ou documento do associado'; }
            },

            escapeMarkup: function (markup) { return markup; },

            //Faz o autocomplete funcionar
            ajax: {
                url: $("#novoAssociadoEspecifico").data('url'),
                dataType: 'json',
                delay: 250,
                data: function (params) {
                    return { valorBusca: params.term, page: params.page };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    
                    return {
                        results: data.items,
                        pagination: { more: (params.page * 30) < data.total_count }
                    };
                },
                cache: true
            },
            minimumInputLength: 2
        });


        //Evento de onchange do select2
        $('#novoAssociadoEspecifico').on("select2:select", function (e) {
            MaterialApoioAssociado.carregarInfoAssociado();
        });
    };

    // Função é acionada quando selecionamos um item
    this.carregarInfoAssociado = function () {

        var url = $("#baseUrlGeral").val() + "Associados/Associado/autocomplete-informacoes-associado/"
        
        $.post(url, { id: $("#novoAssociadoEspecifico").select2('val') }, function (response) {
            if (response.error != undefined && response.error == false) {

                $("#idAssociadoEspecifico").val(response.idPessoa);
                $("#nomeAssociadoEspecifico").val(response.value);
                $("#cnpfAssociadoEspecifico").val(response.nroDocumento);

                
                return;
            }

            if (response.error != undefined && response.error == false) {
                jM.error(response.message);
                return;
            }
        });
    };

    this.retornoAposAdicionar = function () {
        DefaultSistema.iniciarPluginsAposAjax($("#novoAssociadoEspecifico"));
        $("#idAssociadoEspecifico").val('');
        $("#nomeAssociadoEspecifico").val('');
        $("#cnpfAssociadoEspecifico").val('');
        MaterialApoioAssociado.autoCompleteMaterialApoio();
    }

    this.verificarAbaAssociadosEspecificos = function (elem) {
    
        var combo = $(elem);

        if (combo.val() == "ESP") {
            $(abaAssociadosEspecificos).removeClass("hide");
        }

        if (combo.val() != "ESP") {
            $(abaAssociadosEspecificos).addClass("hide");
        }

    }

};


var MaterialApoioAssociado = new ObjMaterialApoioAssociado();

$(document).ready(function () {
    MaterialApoioAssociado.init();
});
