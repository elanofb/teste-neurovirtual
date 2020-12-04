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
    	$("#dtPagamento").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true });

    	$("#dtCredito").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true });
    };

};

var TituloCobranca = new ObjTituloCobranca();

$(document).ready(function () {
	TituloCobranca.init();
});