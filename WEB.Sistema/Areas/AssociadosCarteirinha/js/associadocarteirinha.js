function AssociadoCarteirinhaClass() {

    var idBoxListaHistorico = "#boxLoadListaHistorico";

    var idBoxForm = "#boxLoadFormHistoricoCarteirinha";
    var idBoxLista = "#boxLoadListaHistoricoCarteirinha";

    this.init = function () {
    };

    //Retorno após submissao do formulario de cadastro
    this.onSuccessForm = function (response) {
        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {
                DefaultSistema.carregarConteudo($(idBoxForm));
                DefaultSistema.carregarConteudo($(idBoxLista));
                DefaultSistema.carregarConteudo($(idBoxListaHistorico));
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormHistoricoCarteirinha"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoCarteirinha = new AssociadoCarteirinhaClass();

$(document).ready(function () {
    AssociadoCarteirinha.init();
});