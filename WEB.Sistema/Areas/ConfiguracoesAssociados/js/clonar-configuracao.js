function ClonarConfiguracaoClass() {


    //Inicializadores
    this.init = function() {

        
        
    };

    //Executado após retorno do formulário
    this.onSuccessForm = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();
            jM.success("Configuração de campos replicadas com sucesso");

            return;
        }

        ClonarConfiguracao.initMultiSelectTipoAssociadoDestino();
    }

    //Iniciar multi select de tipo de associado na clonagem
    this.initMultiSelectTipoAssociadoDestino = function () {

        $(".multiSelectViewCamposTipoAssociado").multiselect({
            buttonClass: 'btn btn-sm btn-default',
            buttonWidth: '100%',
            enableFiltering: true,
            filterBehavior: 'text',
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1
        });
    }

}


var ClonarConfiguracao = new ClonarConfiguracaoClass();

$(document).ready(function () {
    ClonarConfiguracao.init();
});