function ObjCupomDesconto(){
    
	this.init = function () {
    };

	//
    this.enviarCupom = function (idCupomDesconto) {
    	//
    	var fYes = function () {
    	    var url = $("#baseUrlGeral").val() + "cuponsdesconto/cupomdesconto/enviarCupom";

    		var callback = function (response) {
    			if (response.error == false) {
    				jM.success("O cupom de desconto foi enviado para os e-mails deste cadastro.");
    			} else {
    				jM.error(response.message);
    			}
    		};

    		var Assync = new ObjAjax();
    		Assync.init(url, { "idCupomDesconto": idCupomDesconto }, callback, null, 'post', 'json', false);
    	};

    	//
    	var fNo = function () { return false; };

    	jM.confirmation("Esse procedimento vai enviar o cupom de desconto para os e-mail deste cadastro. Tem certeza que deseja continuar?", fYes, fNo);
    };
};

var CupomDesconto = new ObjCupomDesconto();

$(document).ready(function(){
	CupomDesconto.init();
});
