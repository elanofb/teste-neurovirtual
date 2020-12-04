function IdiomaClass() {

    this.init = function() {
    };

    this.onSuccessForm = function(response) {
        if (response.error != undefined && response.error == false) {
            location.reload();
            return;
        }

        if (response.error != undefined && response.error == true) {
            jM.error(response.message);
            return;
        }

        DefaultSistema.iniciarPluginsAposAjax();
    }
}

var Idioma = new IdiomaClass();
$(document).ready(function () {
    Idioma.init();
});