function AssociadoCargoClass() {

    var idBoxLista = "#boxLoadListaAssociadoCargo";
    var idBoxForm = "#boxLoadFormAssociadoCargo";

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
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAssociadoCargo"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoCargo = new AssociadoCargoClass();

$(document).ready(function () {
    AssociadoCargo.init();
});