function TituloReceitaPagamentoCobrancaClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    }

    this.abrirModalGeracaoEmailCobranca = function (idPagamento) {

        var dados = { 'idsPagamentos': [] };

        if (idPagamento > 0) {
            dados["idsPagamentos"].push(idPagamento);
        }

        if (idPagamento == null || idPagamento == 'undefined') {

            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
                dados["idsPagamentos"].push($(this).data("id"));
            });

        }

        if (dados["idsPagamentos"].length == 0) {
            jM.info("Selecione ao menos um item para enviar email de cobran&ccedil;a."); return false;
        }

        var url = this.baseUrl.concat("FinanceiroNotificacoes/TituloReceitaPagamentoCobranca/modal-gerar-email-cobranca");

        $.post(url, dados, function (data) {

            var Modal = $(data).modal();

            $(Modal).on("shown.bs.modal", function (e) {

                DefaultSistema.reiniciarBotao();
                
                $("#boxGeracaoNotificacaoCobranca").find("#emailCobrancaHtml").froalaEditor({
                    height: 250,
                });

            });

            $(Modal).on("hidden.bs.modal", function (e) {

                $(this).remove();

            });

        })

    }

    this.onSucess = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            jM.success(response.message);

            return;

        }
        
        $("#boxGeracaoNotificacaoCobranca").find("#emailCobrancaHtml").froalaEditor({
            height: 250,
        });

        DefaultSistema.reiniciarBotao();

    }

}

var TituloReceitaPagamentoCobranca = new TituloReceitaPagamentoCobrancaClass();

$(document).ready(function () {
    TituloReceitaPagamentoCobranca.init();
});