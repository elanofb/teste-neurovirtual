function ConsultasImediatas() {

    this.init = function () {

    };
    
    /**
    *
    */
	this.exportarParaExcel = function () {
	    $("#flagGerarExcel").val("S");
	    $("#formConsulta").submit();

    };
    
    /**
    *
    */
	this.gerarEtiquetas = function () {
	    $("#flagEtiqueta").val("S");
	    $("#nroEtiqueta").val($("#txtNroEtiqueta").val());
	    $("#formConsulta").submit();

	};

};

var ConsultaImediata = new ConsultasImediatas();

$(document).ready(function(){
    ConsultaImediata.init();
});
