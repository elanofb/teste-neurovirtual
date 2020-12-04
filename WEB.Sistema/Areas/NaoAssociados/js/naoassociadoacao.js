function NaoAssociadoAcaoClass(){
    

    //Metodo de inicializacao dos plugins
    this.init = function () {

        this.listenerDesejaAdmitir();

    };

    //Verififcar se o tipo de pessoa muda e configurar os campos conforme necess�rio
    this.listenerDesejaAdmitir = function () {
        $('#flagDesejaAdmitir').on('change', function (event) {
            var flagDesejaAdmitir = $(this).val();
            NaoAssociadoAcao.configurarAdmissao(flagDesejaAdmitir);
        });

        this.configurarAdmissao($('#flagDesejaAdmitir').val());
    };

    //Exibir e ocultar campos de acordo com a flagDesejaAdmitir 
    this.configurarAdmissao = function (flagDesejaAdmitir) {

        if (flagDesejaAdmitir == "S") {
            $(".info-admissao").show();
        } else {
            $(".info-admissao").hide();
        }
    };

    //respons�vel pela abertura de modal de tornar associado
    this.showModalTornarAssociado = function (urlContent) {
        $.get(urlContent, function (data) {

            var Modal = $(data).modal();

            $(Modal).on("shown.bs.modal", function (e) {

                $('input:text').setMask();
                $("[data-toggle=tooltip]").tooltip();

                $(Modal).find(".link-loading").button('reset');
                $(Modal).find(".link-loading").on('click', function () {
                    var btn = $(this).button().data('loading-text', 'Processando...');
                    btn.button('loading');
                });

                NaoAssociadoAcao.listenerDesejaAdmitir();
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }

    //Executado ao submeter formulario de desativacao
    this.onSuccessFormDesativacao = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormDesativacao"));
            }
        } catch (e) {
            console.log(e);
        }
    };

    //Executado ao submeter formulario de reativacao
    this.onSuccessFormReativacao = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormReativacao"));
            }
        } catch (e) {
            console.log(e);
        }
    };


    //Executado ao submeter formulario de exclus�o do n�o associado
    this.onSuccessFormExclusao = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {

                var box = $("#boxFormExclusao");

                DefaultSistema.iniciarPluginsAposAjax(box);

                DefaultSistema.reiniciarBotao();
            }
        } catch (e) {

            console.log(e);
        }
    };

    //Executado ao submeter formulario de tornar associado
    this.onSuccessFormTornarAssociado = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.href = $("#baseUrlGeral").val() + "associados/associado/detalhes/" + response.id;
            } else {

                var box = $("#boxFormTornarAssociado");

                DefaultSistema.iniciarPluginsAposAjax(box);

                DefaultSistema.reiniciarBotao();

                NaoAssociadoAcao.listenerDesejaAdmitir();
            }
        } catch (e) {

            console.log(e);
        }
    };

    //
    this.alterarTipo = function (comboTipoAssociado) {

        var idTipoAtual = $("#NaoAssociado_idTipoAssociado").val();
        var idAssociado = $(comboTipoAssociado).data("id-associado");
        var idTipoAssociado = $(comboTipoAssociado).val();

        var fNo = function () {
           $(comboTipoAssociado).val(idTipoAtual);
            return false;
        };

        var fYes = function () {
            var url = $("#baseUrlGeral").val() + "naoassociados/naoassociadoacao/alterar-tipo-nao-associado";

            $.post(url,
                { 'idAssociado': idAssociado, 'idTipoAssociado': idTipoAssociado },
                function (response) {
                    
                    if (response.error == true) {
                        jM.error(response.message);
                        $(comboTipoAssociado).val(idTipoAtual);
                        return;
                    }

                    location.reload(true);
                }
            );

        };

        jM.confirmation("Deseja realmente alterar o tipo do n;&atilde;o associado?", fYes, fNo);
    }

    //
    this.onSuccessFormEnvioFichaCadastralPoEmail = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagError == false) {
                $(".modal").modal('toggle');
                DefaultSistema.removerModais();

                location.reload();

            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormEnvioCadastro"));
            }
        } catch (e) {
            console.log(e);
        }
    };
};

var NaoAssociadoAcao = new NaoAssociadoAcaoClass();

$(document).ready(function(){
    NaoAssociadoAcao.init();
});
