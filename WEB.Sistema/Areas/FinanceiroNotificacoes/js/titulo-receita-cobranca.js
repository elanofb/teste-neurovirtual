function TituloReceitaCobrancaClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    }

    this.abrirModalGeracaoEmailCobranca = function (idTituloReceita) {

        var dados = { 'idsTitulosReceita': [] };

        if (idTituloReceita > 0) {
            dados["idsTitulosReceita"].push(idTituloReceita);
        }

        if (idTituloReceita == null || idTituloReceita == 'undefined') {

            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
                dados["idsTitulosReceita"].push($(this).data("id"));
            });

        }

        if (dados["idsTitulosReceita"].length == 0) {
            jM.info("Selecione ao menos um item para enviar email de cobran&ccedil;a."); return false;
        }

        var url = this.baseUrl.concat("FinanceiroNotificacoes/TituloReceitaCobranca/modal-gerar-email-cobranca");

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

var TituloReceitaCobranca = new TituloReceitaCobrancaClass();

$(document).ready(function () {
    TituloReceitaCobranca.init();
});