function RelacionamentoConsultaClass() {

    var baseUrl;

    //
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        this.iniciarAutoCompleteAssociado();
        
    }
    
    //
    this.iniciarAutoCompleteAssociado = function () {

        AppAutoComplete.title = "Selecione...";

        AppAutoComplete.url = RelacionamentoConsulta.baseUrl + "Pessoas/PessoaAutoComplete/listar-json-associados-nao-associados";

        AppAutoComplete.quantityItems = 2;

        var combo = $("#idPessoa");

        AppAutoComplete.loadSelect2(combo);
        
    }    
    
}

var RelacionamentoConsulta = new RelacionamentoConsultaClass();

$(document).ready(function () {
    RelacionamentoConsulta.init();
});
