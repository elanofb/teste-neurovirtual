function AssociadoInstituicaoClass() {

    var idBoxLista = "#boxLoadListainstituicao";
    var idBoxForm = "#boxLoadFormInstituicao";

    this.init = function () {
    };

    //Retorno após submissao do formulario de cadastro
    this.onSuccessForm = function (response) {

        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {
                DefaultSistema.carregarConteudo($(idBoxForm));
                DefaultSistema.carregarConteudo($(idBoxLista));
            } else {
                DefaultSistema.iniciarPluginsAposAjax($(idBoxForm));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoInstituicao = new AssociadoInstituicaoClass();

$(document).ready(function () {
    AssociadoInstituicao.init();
});