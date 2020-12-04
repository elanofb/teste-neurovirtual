function AssociadoRepresentanteClass() {

    var idBoxLista = "#boxLoadListaAssociadoRepresentante";
    var idBoxForm = "#boxLoadFormAssociadoRepresentante";

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
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAssociadoRepresentante"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoRepresentante = new AssociadoRepresentanteClass();

$(document).ready(function () {
    AssociadoRepresentante.init();
});