function ArquivosFinanceirosClass(){
    
	var baseUrl;
    var urlGeracaoZip;
    var urlDownloadZip;
	
	this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        this.urlDownloadZip = this.baseUrl.concat("Financeiro/ArquivosFinanceiros/download-zip");
	    
	};

    this.gerarZipSelecionados = function () {

        this.urlGeracaoZip = this.baseUrl.concat("Financeiro/ArquivosFinanceiros/gerar-zip-selecionados");
        
        var postData = { 'idsArquivos': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsArquivos"].push($(this).val());
        });

        if (postData["idsArquivos"].length == 0) {

            jM.info("Selecione ao menos um arquivo para realizar o download.");

            return false;
        }        

        this.gerarZip(postData);
        
    }

    this.gerarZipTodos = function () {

        this.urlGeracaoZip = this.baseUrl.concat("Financeiro/ArquivosFinanceiros/gerar-zip-todos");

        var form = $("#formFiltros");
        
        var dadosForm = form.serialize();

        this.gerarZip(dadosForm);

    }
    
    this.gerarZip = function (postData) {

        $.post(this.urlGeracaoZip, postData, function(response) {

            if (response.totalRegistros == 0) {
                jM.info("A consulta n&atilde;o retornou nenhum resultado!");
                return;
            }

            if (response.error == true) {
                jM.info("Ocorreu um erro ao tentar exportar, tente novamente.");
                return;
            }

            if (response.error == false) {
                window.open(ArquivosFinanceiros.urlDownloadZip.concat("?nomeArquivo=" + response.nomeArquivo));
            }

        });
        
    }

};

var ArquivosFinanceiros = new ArquivosFinanceirosClass();

$(document).ready(function(){
    ArquivosFinanceiros.init();
});
