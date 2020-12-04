function PessoaRelacionamentoClass() {

    var baseUrl;
    
    //
    this.init = function () {
        this.baseUrl = $("#baseUrlGeral").val();
    };

    //
    this.iniciarAutoCompleteAssociado = function () {

        AppAutoComplete.title = "Selecione...";

        AppAutoComplete.url = PessoaRelacionamento.baseUrl + "Pessoas/PessoaAutoComplete/listar-json-associados-nao-associados";

        AppAutoComplete.quantityItems = 2;

        var combo = $("#idPessoaOcorrencia");

        AppAutoComplete.loadSelect2(combo);

    }

    //Retorno após submissao do formulario de cadastro
    this.onSuccessForm = function (response) {

        DefaultSistema.reiniciarBotao();

        if (response.error == false) {
            
            DefaultSistema.removerModais();

            if (response.flagRecarregar == true) {
                location.reload();
            }

            if (response.flagRecarregar == false){
                DefaultSistema.carregarConteudo($("#boxLoadListaHistorico"));
            }              

            return;            
        } 

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormHistorico"));

        PessoaRelacionamento.iniciarAutoCompleteAssociado();

    };
    
    this.exportar = function () {
        
        var dadosForm = $("#formPessoaRelacionamento").serialize();
        
        location.href = this.baseUrl + "Pessoas/PessoaRelacionamentoExportacao/exportar?" + dadosForm;
        
    }

}

var PessoaRelacionamento = new PessoaRelacionamentoClass();

$(document).ready(function () {
    PessoaRelacionamento.init();
});