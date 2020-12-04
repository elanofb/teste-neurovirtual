function ObjAssociadoAnuidade() {

    this.init = function () {

        this.autoCompleteAssociado();
    };

	/**
	* Aucompletar lista de cliente para facilitar busca
	*/
    this.autoCompleteCliente = function (element) {

    	var element = $("#nomeCliente"); //<--chama o campo texto para torna-lo um auto complete
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
    				return { results: data  };
    			}
    		},

	   		initSelection: function (obj, callback) {
	   			console.log($(obj));
	   			console.log($("#idPessoa").val());
	   			$.get($(obj).attr("data-url"), { idPessoa: $("#idPessoa").val() },
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

    			$("#idPessoa").val(data.id);
    			$("#cpfCliente").val(data.cnpf).setMask();
    			$("#telPrincipal").val(data.telPrincipal);
    			$("#telSecundario").val(data.telSecundario);
    			$("#emailPrincipal").val(data.emailPrincipal);
    			$("#emailSecundario").val(data.emailSecundario);
    			$("#nome").val(data.value);
    			return data.value;
    		},
    		formatNoMatches: function (term) {
    			return "<a href='#' id='newClient'>Nenhum registro encontrado para (" + term + ")</a>";
    		}
    	});

    };

    /**
     * Carregamento 
     */
    this.registrarPagamento = function (idTituloCobrancaPagamento) {
        //
        var fYes = function () {
            var url = new String($("#baseUrlGeral").val()).concat("titulocobranca/registrar-pagamento-ajax");
            var dtPagamento = $("#data-pagamento-" + idTituloCobrancaPagamento).val();

            var callback = function (response) {
                if (response.error == true) {
                    jM.error(response.message);
                } else {
                    jM.success(response.message, function () {
                        location.reload();
                    });
                }

            };

            var Assync = new ObjAjax();
            Assync.init(url, { "idTituloCobrancaPagamento": idTituloCobrancaPagamento, 'dtPagamento': dtPagamento }, callback, null);

        };

        //
        var fNo = function () {
            return false;
        };

        jM.confirmation("Deseja realmente realizar essa baixa? Essa opera&ccedil;&atilde;o n&atilde;o poder&aacute; ser desfeita.", fYes, fNo);

    };

};

var AssociadoAnuidade = new ObjAssociadoAnuidade();

$(document).ready(function () {
    AssociadoAnuidade.init();
});