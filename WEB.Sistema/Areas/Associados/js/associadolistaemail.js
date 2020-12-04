function AssociadoListaEmailClass() {

    var idBoxLista = "#boxLoadListaAssociadoListaEmail";
    var idBoxForm = "#boxLoadFormAssociadoListaEmail";

    this.init = function () {
    };

    /**
	* Retorno após submissao do formulario de cadastro
	*/
    this.onSuccessForm = function (response) {

        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {
                DefaultSistema.carregarConteudo($(idBoxForm));
                DefaultSistema.carregarConteudo($(idBoxLista));
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAssociadoListaEmail"));
            }
        } catch (e) {
            console.log(e);
        }
    };

    /**
	* Retorno após submissao do formulario de busca
	*/
    this.onSuccessBusca = function () {
        DefaultSistema.iniciarPluginsAposAjax($("#boxLoadListaAssociadoListaEmail"));
    };
};

var AssociadoListaEmail = new AssociadoListaEmailClass();

$(document).ready(function () {
    AssociadoListaEmail.init();
});
