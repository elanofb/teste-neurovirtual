function UsuarioSistemaClass(){
    
    var idBoxUnidades = "#boxUnidadesVinculadas";

	this.init = function () {

	};


	this.vincularUnidade = function (obj) {

	    var idUnidade = $("#idUnidadeVinculo").val();
	    var idUsuario = $(obj).data("idusuario");
	    var dadosPost = { idUsuario: idUsuario, idUnidade: idUnidade };
	    var url = $(obj).data("url");

	    var funcRetorno = function (data) {
	        $(idBoxUnidades).html(data);
	    };

	    $.post(url, dadosPost, funcRetorno);
	};

	this.excluirUnidadeVinculada = function (obj) {
	    var objeto = obj;
	    var id = $(obj).data("id");
	    var idUsuario = $(obj).data("idusuario");
	    var dadosPost = { id: id, idUsuario: idUsuario };
	    var url = $(obj).data("url");

	    var funcRetorno = function (data) {
	        $(idBoxUnidades).html(data);
	    };

	    jM.confirmation("Tem certeza que deseja desvincular a unidade ?", function () { $.post(url, dadosPost, funcRetorno); });
	};
    
	//
    this.reenviarSenha = function (idUsuarioSistema) {
    	//
    	var fYes = function () {
    		var url = $("#baseUrlGeral").val() + "permissao/usuariosistemaacesso/reenviarSenha";

    		var callback = function (response) {
    			if (response.error == false) {
    			    jM.success(response.message);
    			} else {
    				jM.error(response.message);
    			}
    		};

    		var Assync = new ObjAjax();
    		Assync.init(url, { "idUsuarioSistema": idUsuarioSistema }, callback, null, 'post', 'json', false);
    	};

    	//
    	var fNo = function () { return false; };

    	jM.confirmation("Esse procedimento ir&aacute; criar uma nova senha e enviar para os e-mail do usu&aacute;rio. Tem certeza que deseja continuar?", fYes, fNo);

    };

};

var UsuarioSistema = new UsuarioSistemaClass();

$(document).ready(function(){
	UsuarioSistema.init();
});
