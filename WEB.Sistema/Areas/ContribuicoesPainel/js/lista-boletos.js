function ObjListaBoletos(){
    
	this.init = function () {};

    //
	this.excluirBoletoLote = function (element) {

	    var url = $(element).data("url");

	    var postData = { 'id': [] };
	    $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
	        if ($(this).data("id") != "") {
	            postData["id"].push($(this).data("id"));
	        }
	    });

	    if (postData["id"].length == 0) {
	        jM.info("Informe ao menos uma contribuic&ccedil;&atilde;o.");
	        return false;
	    }

	    var url = $(element).data("url");

	    $.get(url, postData, function (response) {

	        if (response.error != 'undefined' && response.error == true) {
	            jM.error(response.message);
	            return false;
	        }

	        var Modal = $(response).modal();
	        $(Modal).on("shown.bs.modal", function (e) {});

	        $(Modal).on("hidden.bs.modal", function (e) {
	            $(this).remove();
	        });
	    });
	};

    //Executado ao submeter formulario de exclusão
	this.onSuccessFormExclusao = function (response) {

	    var idBox = "#boxFormExclusao";

	    if (response.error == false) {

	        location.reload();
	        
	        return;
	    } 

	    DefaultSistema.iniciarPluginsAposAjax($(idBox));

	    return;

	};

    //Exportar arquivos PDF em pasta compactada
	this.gerarZip = function () {

	    var urlExcel = $("#baseUrlGeral").val().concat("ContribuicoesPainel/ListaBoletos/exportar");

	    var urlDownloadExcel = $("#baseUrlGeral").val().concat("ContribuicoesPainel/ListaBoletos/download-zip");

	    var dadosFormulario = { 'id': [] };
	    $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
	        dadosFormulario["id"].push($(this).data("id"));
	    });

	    if (dadosFormulario["id"].length == 0) {
	        var form = $("#formFiltro");
	        dadosFormulario = $(form).serializeObject();
	    }

	    $.post(urlExcel, dadosFormulario, function (response) {
	        if (response.totalRegistros = 0) {
	            jM.info("A consulta n&atilde;o retornou nenhum resultado!");
	            DefaultSistema.reiniciarBotao();
	            return;
	        }

	        if (response.error == true) {
	            jM.error(response.message);
	            DefaultSistema.reiniciarBotao();
	            return;
	        }

	        if (response.error == false) {
	            window.open(urlDownloadExcel.concat("?nomeArquivo=" + response.nomeArquivo));
	        }
	        DefaultSistema.reiniciarBotao();
	    });
	}
};

var ListaBoletos = new ObjListaBoletos();

$(document).ready(function(){
    ListaBoletos.init();
});
