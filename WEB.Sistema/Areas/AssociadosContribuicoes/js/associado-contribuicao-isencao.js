function AssociadoContribuicaoIsencaoClass() {

    this.init = function () {

    };

    //Executado ao submenter formulario de isen��o da contribui��o
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
