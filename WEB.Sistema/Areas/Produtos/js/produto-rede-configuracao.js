function ProdutoRedeConfiguracaoClass() {

    this.init = function () {
        
    };


    //Retorno após submissoes do form no modal
    this.onSuccessForm = function (response) {

        if (typeof (response.error) === 'undefined') {
            
            DefaultSistema.iniciarPluginsAposAjax();
            
            return;
        }

        //Se o registro for salvo com sucesso
        if (response.error === false) {
            
            toastr.success("A configuração foi realizada com sucesso!", "Operação concluída!");
            
            DefaultSistema.carregarConteudoElemento("boxListaRede");

            DefaultSistema.carregarConteudoElemento("boxFormRede");

        }
    }
};

var ProdutoRedeConfiguracao = new ProdutoRedeConfiguracaoClass();

$(document).ready(function () {
    ProdutoRedeConfiguracao.init();
});
