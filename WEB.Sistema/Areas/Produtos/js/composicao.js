function ProdutoComposicaoClass() {

    this.init = function () {
    };

    //Retorno após submissoes do form no modal
    this.onSuccess = function (response) {

        if (typeof (response.error) == 'undefined') {
            DefaultSistema.iniciarPluginsAposAjax();
            return;
        }

        if (response.error === true) {

            jM.error(response.message);

        }

        //Se o registro for salvo com sucesso
        if (response.error === false) {

            jM.success(response.message);
            DefaultSistema.carregarConteudo("#boxLoadAbaComposicao");

        }
    }

};

var ProdutoComposicao = new ProdutoComposicaoClass();

$(document).ready(function () {
    ProdutoComposicao.init();
});
