function ContaBancariaTransferenciaClass() {

    this.init = function () {
        
    }

    this.onSuccessForm = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            location.reload();

            return;

        }

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax("#boxModalTransferencia");

    }
    
} 

var ContaBancariaTransferencia = new ContaBancariaTransferenciaClass();

$(document).ready(function () {
    ContaBancariaTransferencia.init();
});