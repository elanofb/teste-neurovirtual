function ObjAnuidade(){
    
	this.init = function () {

    };
    

    /**
     * Carregamento 
     */
	this.processarAnuidades = function (element) {

		//
		var fYes = function () {
			var url = $(element).attr("data-url");
			var idAnuidade = $(element).attr("data-idanuidade");

			var callback = function (response) {
				//jM.success("Processamento conclu&iacute;do com sucesso.");
			};

			var Assync = new ObjAjax();
			Assync.init(url, { "idAnuidade": idAnuidade }, callback, null);

			jM.success("O processamento foi iniciado, voc&ecirc; ser&aacute; notificado quando a opera&ccedil;&atilde;o estiver conclu&iacute;da.", function () {
				location.reload();
			});
			
		};
		
		//
		var fNo = function () {
			return false;
		};

		jM.confirmation("Ao iniciar o processamento das anuidades, todos os associados n&atilde;o isentos receber&atilde;o mensagens de cobran&ccedil;a com os dados para pagamento.", fYes, fNo);
	};


	/**
     *  
     */
	this.reenviarEmailAnuidade = function (element) {

		//
		var fYes = function () {
			var url = $(element).attr("data-url");
			var idPessoaAnuidade = $(element).attr("data-idpessoaanuidade");

			var callback = function (response) {
				console.log(response);
				if (response.error == true) {
					jM.error(response.message);
				} else {
					jM.success(response.message);
				}
			};

			var Assync = new ObjAjax();
			Assync.init(url, { "idPessoaAnuidade": idPessoaAnuidade }, callback, null, 'post', 'json', false);
		};

		//
		var fNo = function () {
			return false;
		};

		jM.confirmation("Deseja reenviar o e-mail da anuidade?", fYes, fNo);
	};

    /**
    * Envio de e-mails em lote para anuidades
    */
	this.reenviarEmailAnuidadeLote = function (element) {
	    var postData = { 'id': [] };
	    $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
	        postData["id"].push($(this).val());
	    });
	    postData["idAnuidade"] = $("#idAnuidade").val();

	    var url = $(element).attr("data-url");
	    if (postData["id"].length == 0) { jM.info("Selecione ao menos um registro."); return false; }

	    var func = function (response) {
	        jM.success(response.message);
	        //location.reload();
	    };

	    Ajax.init(url, postData, func, Vocabulary.confirmDelete);
	};

	/**
     *  
     */
	this.carregaDadosAnuidade = function (idAnuidade) {

		if (idAnuidade != "") {
			var url = ($("#baseUrlGeral").val() + "anuidade/carregar");

			var callback = function (response) {
				console.log(response);
				$("#valorAnuidade").val(response.valor);
				$("#dtVencimentoAnuidade").val(response.dtVencimento);
				$("#valorAnuidade").setMask();
				$("#dtVencimentoAnuidade").setMask();
			};

			var Assync = new ObjAjax();
			Assync.init(url, { "idAnuidade": idAnuidade }, callback, null, 'post', 'json', false);

		} else {
			$("#valorAnuidade").val('0.00');
			$("#dtVencimentoAnuidade").val('');
		}

	};

    /**
	*
	*/
	this.criarTarefaEnviarBoleto = function (element) {
	    var idAnuidade = $(element).attr("data-id-anuidade");
	    var url = $(element).attr("data-url");

	    var fYes = function () {

	        var callback = function (response) {
	            if (response.error == true) {
	                jM.error(response.message);
	            } else {
	                jM.success(response.message);
	                DefaultSistema.autoLoadBlocoDinamico();
	            }
	        };

	        var Assync = new ObjAjax();
	        Assync.init(url, { "idAnuidade": idAnuidade }, callback, null, 'post', 'json', false);
	    };

	    var fNo = function () {
	        return false;
	    };

	    jM.confirmation("Essa tarefa ir&aacute; enviar o e-mail com link para pagamento para todos os associados que ainda n&atilde;o quitaram a anuidade. Confirma essa opera&ccedil;&atilde;o?", fYes, fNo);
	}

};

var Anuidade = new ObjAnuidade();

$(document).ready(function(){
	Anuidade.init();
});
