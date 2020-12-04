function AssociadoContribuicaoExclusaoClass() {


    this.init = function () {

        this.urlExclusaoLote = $("#baseUrlGeral").val() + "AssociadosContribuicoes/AssociadoContribuicaoExclusao/partial-excluir-contribuicao-lote";
        
    };


    //Executado ao submeter formulario de exclus�o
    this.onSuccessFormExclusao = function (response) {

        var idBox = "#boxFormExclusao";

        try {

            if (response.flagSucesso == true) {
                location.reload();
                return;
            }

            DefaultSistema.iniciarPluginsAposAjax($(idBox));

            DefaultSistema.reiniciarBotao();

        } catch (e) {
            console.log(e);
        }
    };

    this.excluirSelecionados = function () {

        var postData = { 'ids': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["ids"].push($(this).attr('data-id'));
        });

        if (postData["ids"].length == 0) {
            jM.info("Selecione ao menos uma cobrança.");
            return false;
        }

        $.post(this.urlExclusaoLote, postData, function (response) {

            AssociadoContribuicaoExclusao.onSuccessAberturaModal(response)

        });

    }

    //
    this.onSuccessAberturaModal = function (response) {

        if (response.error == true) {

            jM.error(response.message);

            return;

        }

        var Modal = $(response).modal();

        $(Modal).on("shown.bs.modal", function (e) {

            DefaultSistema.reiniciarBotao();

            $('input:text').setMask();

            $("#boxCobrancasSelecionadas").slimScroll({
                height: 196
            });

            $("#boxCobrancasSelecionadas").removeClass("hide");

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });

    }

};

var AssociadoContribuicaoExclusao = new AssociadoContribuicaoExclusaoClass();

$(document).ready(function(){
    AssociadoContribuicaoExclusao.init();
});
