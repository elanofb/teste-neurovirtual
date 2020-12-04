function AssociadoTituloClass() {

    var idBoxLista = "#boxLoadListaAssociadoTitulo";
    var idBoxForm = "#boxLoadFormAssociadoTitulo";

    this.init = function () {
    };

    //Retorno após submissao do formulario de cadastro
    this.onSuccessForm = function (response) {

        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {
                DefaultSistema.carregarConteudo($(idBoxForm));
                DefaultSistema.carregarConteudo($(idBoxLista));
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAssociadoTitulo"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoTitulo = new AssociadoTituloClass();

$(document).ready(function () {
    AssociadoTitulo.init();
});