function AssociadoSituacaoContribuicaoClass() {

    this.init = function () {

    }

    this.onSuccessForm = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            location.reload();

            return;
        }

        DefaultSistema.reiniciarBotao();

    }

}

var AssociadoSituacaoContribuicao = new AssociadoSituacaoContribuicaoClass();
$(document).ready(function () {

    AssociadoSituacaoContribuicao.init();

});