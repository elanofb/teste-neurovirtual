function DefaultAction() {
    var element;
    var config = {
        element: null,
        idAffected: null,
        urlAction: ''
    }
    
    /**
     * Submete o formulário de filtros juntamente com informações de paginação.
     */
    this.submitFilterPaginate = function(page){
        $("#searchFilters input[name=page]").val(page);
        $("#searchFilters").submit();
    }
    
    /**
     * Recebe os pedidos de requisição a partir de elementos HTML configurados
     */
    this.action = function (objSource, action) {
        element = $(objSource);
        config.idAffected = element.attr("data-id");
        config.urlAction = element.attr("data-url");

        if (element.attr("data-id") == '' || element.attr("data-id") == undefined) {
            alert('Error. This element does not have attr "cod"!');
            return false;
        }

        switch (action) {
            case 'delete':
                remove();
                break;
            case 'deleteFile':
                removeFile();
                break;
            default:
                updateStatus();
                break;
        }
        return false;
    }

    /**
     * Efetiva a requisição
     */
    this.exec = function(func, confirm){
        Ajax.init(config.urlAction, {
            id: config.idAffected
        }, func, confirm );
    }

    
    //Atualização de Status Padrão
    var updateStatus = function () {
        var func = function (data) {
            console.log(data);
            switch (data.active) {
                case "S":
                    element.removeClass("text-red").addClass("text-green").html("<i class='fa fa-check'></i> Ativo");
                    break;
                case "N":
                    element.removeClass("text-green").addClass("text-red").html("<i class='fa fa-times'></i> Desativado");
                    break;
            }

            var fnCallback = $(element).attr("data-fncallback");

            if (fnCallback != null) {
                eval(fnCallback);
            }
        };

        DefaultAction.exec(func, Vocabulary.confirmUpdateStatus);
    };  // end updateStatus


    /**
     * Remoção de Itens padrão
     */
    var remove = function () {
        var func = function () {
            $(element).parent("td").parent("tr").hide("slow");
            $(element).parent("div").parent("li").hide("slow");

            $(element).closest(".box-info-item").hide("slow");
            $(element).closest(".box-info-item-dotted").hide("slow");

            var fnCallback = $(element).attr("data-fncallback");

            if (fnCallback != null) {
                eval(fnCallback);
            }

        };
        DefaultAction.exec(func, Vocabulary.confirmDelete);
    }; // End remove

	/**
    *
    */
    this.removeAll = function (element) {
    	var postData = { 'id': [] };
    	$("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
    		postData["id"].push($(this).val());
    	});
    	var url = $(element).attr("data-url");
    	if (postData["id"].length == 0) { jM.info("Selecione ao menos um registro."); return false; }

    	var func = function () {
    		location.reload();
    	};
    	Ajax.init(url, postData, func, Vocabulary.confirmDelete);
    };
        
    /**
    * Remoção padrão para Arquivos
    */
    var removeFile = function () {
        var func = function () {
            location.reload();
        };
        DefaultAction.exec(func, Vocabulary.confirmDelete);
    }; // End remove
       
    //
    this.gerarExcel = function () {

        var postData = new Array();

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData.push($(this).val());
        });

        if (postData.length == 0) {
            jM.info("Selecione ao menos um registro.");
            return false;
        }

        $("#idsExcel").val(postData);
        $("#formExcel").submit();

        return false;
    };
}

var DefaultAction = new DefaultAction();