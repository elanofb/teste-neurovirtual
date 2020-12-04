function ConfiguracaoTextoClass() {

    this.init = function() {
    };

    this.modalAdd = function(element) {
        
        var url = $(element).data("url");
        $.get(url, {}, function (response) {
            if (response.error != 'undefined' && response.error == true) {
                jM.error(response.message);
                return false;
            }

            var Modal = $(response).modal();
            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxAdicionarConfiguracaoTexto"));
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }

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

var ConfiguracaoTexto = new ConfiguracaoTextoClass();
$(document).ready(function () {
    ConfiguracaoTexto.init();
});