function ConciliacaoAcaoClass() {

    //
    this.init = function () {
    };

    //
    this.detalheConciliacao = function (idConciliacao) {

        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/ConciliacaoAcao/modal-lista-detalhe/" + idConciliacao;

        $.post(url, {}, function (response) {

            if (response.error == true) {

                jM.error(response.message);
                return;
            }

            var Modal = $(response).modal();

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.reiniciarBotao();
                DefaultSistema.iniciarPluginsAposAjax($("#listaDetalheConciliacao"));
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }

    //
    this.realizarConciliacao = function () {

        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/ConciliacaoAcao/modal-conciliar";
        var postData = {'idsLancamentos': [], 'tiposLancamentos': []};

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsLancamentos"].push($(this).val());
            postData["tiposLancamentos"].push($(this).data("tipo"));
        });

        if (postData["idsLancamentos"].length == 0) {
            jM.info("Selecione ao menos um lançamento.");
            return false;
        }

        $.post(url, postData, function (response) {

            if (response.error == true) {

                jM.error(response.message);
                return;
            }

            var Modal = $(response).modal();

            $(Modal).on("shown.bs.modal", function (e) {

                DefaultSistema.reiniciarBotao();
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormConciliacao"));

                $("#boxLancamentosSelecionados").slimScroll({
                    height: 196
                });

                $("#boxLancamentosSelecionados").removeClass("hide");
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }

    //
    this.excluirConciliacao = function () {

        var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/ConciliacaoAcao/excluir-conciliacao";
        var postData = {'idsConciliacoes': []};

        $("input[type=checkbox][name='checkRegistroConciliacao[]']:checked").each(function () {
            postData["idsConciliacoes"].push($(this).val());
        });

        if (postData["idsConciliacoes"].length == 0) {
            jM.info("Selecione ao menos uma conciliação.");
            return false;
        }

        var fYes = function () {

            $.post(url, postData, function (response) {

                if (response.error == true) {
                    jM.error(response.message);
                    return;
                }else{
                    jM.success(response.message);
                    Conciliacao.carregarLancamentos();
                    Conciliacao.carregarConciliacoes();
                }
            });
        };
        
        var fNo = function () {
            return false;
        };

        jM.confirmation("Você tem certeza que deseja excluir esta conciliação? Está ação é irreversível.", fYes, fNo);
    }

    this.onSuccessFormConciliacaoFinanceira = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormConciliacao"));

        $("#boxLancamentosSelecionados").slimScroll({
            height: 225
        });

        $("#boxLancamentosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            
            $("#boxFormConciliacao").addClass("carregando");
            jM.success(response.message);
                        
            Conciliacao.carregarLancamentos();
            Conciliacao.carregarConciliacoes();
            $("#myModal").remove();
            $(".modal-backdrop").remove();
            
        }
    };
}

var ConciliacaoAcao = new ConciliacaoAcaoClass();
$(document).ready(function () {
    ConciliacaoAcao.init();
});
