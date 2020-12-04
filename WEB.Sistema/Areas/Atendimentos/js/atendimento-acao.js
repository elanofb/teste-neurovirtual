function AtendimentoAcaoClass() {

    var boxHistorico = "#boxHistoricoAtendimento"

    var boxBotoes = "#boxBotoesAcao"

    this.init = function () {

    }

    this.onSuccessForm = function (response) {

        if (response.error == false) {

            if (typeof response.urlRedirect != "undefined") {

                location.href = response.urlRedirect;

                return;

            }

            DefaultSistema.removerModais();

            jM.success(response.message);

            DefaultSistema.carregarConteudo($(boxHistorico));

            DefaultSistema.carregarConteudo($(boxBotoes));

            return;

        }

        DefaultSistema.reiniciarBotao();

    }

}

var AtendimentoAcao = new AtendimentoAcaoClass();

$(document).ready(function () {

    AtendimentoAcao.init();

});