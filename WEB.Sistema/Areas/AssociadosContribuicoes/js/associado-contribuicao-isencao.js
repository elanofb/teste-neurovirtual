function AssociadoContribuicaoIsencaoClass() {

    this.init = function () {

    };

    //Executado ao submenter formulario de isenção da contribuição
    this.onSuccessFormIsencao = function (response) {
        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormIsencao"));
            }
        } catch (e) {
            console.log(e);
        }
    };

};

var AssociadoContribuicaoIsencao = new AssociadoContribuicaoIsencaoClass();

$(document).ready(function(){
    AssociadoContribuicaoIsencao.init();
});
