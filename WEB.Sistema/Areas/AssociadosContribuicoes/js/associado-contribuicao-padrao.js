function AssociadoContribuicaoPadraoClass() {

    this.init = function () {

    };

	//Listenter Formulario
    this.onSuccessForm = function (response) {

        if (response.error === false) {
            location.reload(true);
            return;
        }

        DefaultSistema.reiniciarBotao();
		
    };


};

var AssociadoContribuicaoPadrao = new AssociadoContribuicaoPadraoClass();

$(document).ready(function () {
    AssociadoContribuicaoPadrao.init();
});