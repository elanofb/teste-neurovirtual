function ObjEstoqueSaida(){
    
	this.init = function () {
	};
    
    /**
     * Carregamento as ocorrências
     */
	this.carregarReferencias = function () {

	    var idTipoReferenciaSaida = $("#EstoqueSaida_idTipoReferenciaSaida").val();
	    var combo = $("#EstoqueSaida_idReferencia");
	    combo.find("option").remove().end().append(new Option("Carregando...", ""));

	    var callback = function (data) {

	        if (data.length > 0) {

	            combo.find("option").remove().end().append(new Option("Selecione", ""));

	            $.each(data, function (key, item) {
	                combo.append(new Option(item.nome, item.id));
	            });
	        } else {
	            combo.find("option").remove().end().append(new Option("Não encontrado", ""));
	        }
	    }

	    Ajax.init($("#baseUrlGeral").val() + 'produtos/estoquesaida/carregar-referencias', { "idTipoReferenciaSaida": idTipoReferenciaSaida }, callback, null);
	};
};

var EstoqueSaida = new ObjEstoqueSaida();

$(document).ready(function(){
    EstoqueSaida.init();
});
