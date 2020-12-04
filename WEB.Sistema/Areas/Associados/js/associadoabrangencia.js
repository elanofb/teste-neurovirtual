function AssociadoAbrangenciaClass() {

    var idBoxLista = "#boxLoadListaAssociadoAbrangencia";
    var idBoxForm = "#boxLoadFormAssociadoAbrangencia";

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
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAssociadoAbrangencia"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoAbrangencia = new AssociadoAbrangenciaClass();

$(document).ready(function () {
    AssociadoAbrangencia.init();
});