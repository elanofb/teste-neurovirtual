function PessoaContaBancariaCadastroClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    };
    
    this.onSuccessForm = function(response) {
        
        if (response.error == false) {

            location.reload();

            return;
        }
        
        $("#boxCadastroConta").replaceWith(response);

        DefaultSistema.reiniciarBotao();
        
    }

};

var PessoaContaBancariaCadastro = new PessoaContaBancariaCadastroClass();

$(document).ready(function () {
    PessoaContaBancariaCadastro.init();
});
