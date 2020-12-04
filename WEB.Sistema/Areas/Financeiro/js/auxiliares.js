function ObjAuxiliares(){
    
	this.init = function () {
	};
    
    /** Carregamento as ocorrencias */
	this.carregarCategorias = function () {

	    var idMacroConta = $("#idMacroConta").val();
	    var combo = $("#idCategoria");
	    combo.find("option").remove().end().append(new Option("Carregando...", ""));

	    var callback = function (data) {

	        if (data.length > 0) {

	            combo.find("option").remove().end().append(new Option("Selecione", ""));

	            $.each(data, function (key, item) {
	                combo.append(new Option(item.text, item.value));
	            });
	        } else {
	            combo.find("option").remove().end().html("<option value=''>N&atilde;o encontrado</option>");
	        }
	    }

	    Ajax.init($("#baseUrlGeral").val() + 'financeiro/categoriatitulo/listar-ajax', { "idMacroConta": idMacroConta }, callback, null);
	};

    /** Carregamento das macros contas */
	this.carregarMacroConta = function (flagReceitaDespesa, selectedValue) {

	    var idCentroCusto = $("#idCentroCusto").val();

        if (idCentroCusto < 1) {
            return;
        }

	    var combo = $("#idMacroConta");
	    combo.find("option").remove().end().append(new Option("Carregando...", ""));

	    var callback = function (data) {

	        if (data.length > 0) {

	            combo.find("option").remove().end().append(new Option("Selecione", ""));

	            $.each(data, function (key, item) {
	                combo.append(new Option(item.text, item.value));
	            });

	            if (selectedValue > 0) {
	                combo.val(selectedValue);
	            }
	        } else {
	            combo.find("option").remove().end().html("<option value=''>N&atilde;o encontrado</option>");
	        }
	    }

	    Ajax.init($("#baseUrlGeral").val() + 'financeiro/macrocontaconsulta/listar-ajax', { "idCentroCusto": idCentroCusto, "flagReceitaDespesa": flagReceitaDespesa }, callback, null);
	};

    /**
    * Carregamento as ocorrencias
    */
	this.carregarTipos = function () {

	    var idCategoria = $("#idCategoria").val();
	    var combo = $("#idTipoCategoria");
	    combo.find("option").remove().end().append(new Option("Carregando...", ""));

	    var callback = function (data) {

	        if (data.length > 0) {

	            combo.find("option").remove().end().append(new Option("Selecione", ""));

	            $.each(data, function (key, item) {
	                combo.append(new Option(item.text, item.value));
	            });
	        } else {
	            combo.find("option").remove().end().html("<option value=''>N&atilde;o encontrado</option>");
	        }
	    }

	    Ajax.init($("#baseUrlGeral").val() + 'financeiro/tipocategoria/listar-ajax', { "idCategoria": idCategoria }, callback, null);
	};

    /**
     * Carregamento as ocorrencias
     */
	this.carregarDetalheTipos = function () {

	    var idTipoCategoria = $("#idTipoCategoria").val();
	    var combo = $("#idDetalheTipoCategoria");
	    combo.find("option").remove().end().append(new Option("Carregando...", ""));

	    var callback = function (data) {

	        if (data.length > 0) {

	            combo.find("option").remove().end().append(new Option("Selecione", ""));

	            $.each(data, function (key, item) {
	                combo.append(new Option(item.text, item.value));
	            });
	        } else {
	            combo.find("option").remove().end().html("<option value=''>N&atilde;o encontrado</option>");
	        }
	    }

	    Ajax.init($("#baseUrlGeral").val() + 'financeiro/detalhetipocategoria/listar-ajax', { "idTipoCategoria": idTipoCategoria }, callback, null);
	};


};

var Auxiliares = new ObjAuxiliares();

$(document).ready(function(){
    Auxiliares.init();
});
