function AssociadoContribuicaoCobrancaClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        this.iniciarPaginacaoComPost();

    }

    this.iniciarPaginacaoComPost = function () {

        $(".pagination-container .pagination a").on("click", function (e) {
            e.preventDefault();

            var url = $(this).attr("href");

            window.location.replace(url);
        });

    }

    this.abrirModalGeracaoEmailCobranca = function (id, idAssociadoContribuicao) {

        var dados = { 'idsAssociadoContribuicoes': [], 'idContribuicao': id };

        if (idAssociadoContribuicao > 0) {
            dados["idsAssociadoContribuicoes"].push(idAssociadoContribuicao);
        }

        if (idAssociadoContribuicao == null || idAssociadoContribuicao == 'undefined') {

            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
                dados["idsAssociadoContribuicoes"].push($(this).data("id"));
            });

        }

        if (dados["idsAssociadoContribuicoes"].length == 0) {
            jM.info("Selecione ao menos um associado para enviar email de cobran&ccedil;a."); return false;
        }

        var url = this.baseUrl.concat("AssociadosNotificacoes/AssociadoContribuicaoCobranca/modal-gerar-email-cobranca");

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

var AssociadoContribuicaoCobranca = new AssociadoContribuicaoCobrancaClass();

$(document).ready(function () {
    AssociadoContribuicaoCobranca.init();
});