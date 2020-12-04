function AssociadoAreaAtuacaoClass() {

    var idBoxLista = "#boxLoadListaAreaAtuacao";
    var idBoxForm = "#boxLoadFormAreaAtuacao";

    this.init = function () {
    };

    //Retorno após submissao do formulario de cadastro
    this.onSuccessForm = function (response) {

        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {
                DefaultSistema.carregarConteudo($(idBoxForm));
                DefaultSistema.carregarConteudo($(idBoxLista));
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAssociadoAreaAtuacao"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoAreaAtuacao = new AssociadoAreaAtuacaoClass();

$(document).ready(function () {
    AssociadoAreaAtuacao.init();
});