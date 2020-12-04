function ObjTituloCobranca() {

	/**
	*
	*/
	this.init = function () {

    	this.iniciarDatepicker();
    };

	/**
	* Remover produto de um pedido
	*/
    this.iniciarDatepicker = function () {
        $("#dtVencimento").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true, "opens": "left" });
    	$("#dtCompetencia").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true, "opens": "left" });
    };

};

var TituloCobranca = new ObjTituloCobranca();

$(document).ready(function () {
	TituloCobranca.init();
});