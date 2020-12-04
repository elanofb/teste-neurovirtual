function PessoaContatoClass() {

    var idBoxListaContatos = "#boxLoadListaContatos";
    var idBoxFormContatos = "#boxLoadFormContatos";

    this.init = function () {

       

    };

    /**
	* Retorno após submissao do formulario de cadastro
	*/
    this.onSuccessForm = function (response) {

        DefaultSistema.reiniciarBotao();

        PessoaContato.iniciarPlugins();

        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {
                DefaultSistema.carregarConteudo($(idBoxFormContatos));
                DefaultSistema.carregarConteudo($(idBoxListaContatos));
            }
        } catch (e) {
            
            console.log(e);
        }
    };

    //Iniciar plugins ao carregar blocos
    this.iniciarPlugins = function () {

        $(idBoxFormContatos).find("input:text").setMask();

    }

};

var PessoaContato = new PessoaContatoClass();

$(document).ready(function () {
    PessoaContato.init();
});