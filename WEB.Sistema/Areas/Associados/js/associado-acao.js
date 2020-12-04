function AssociadoAcaoClass(){
    

    //Metodo de inicializacao dos plugins
    this.init = function () {
    };


	//Executado ao submeter formulario de admissao do associado
    this.onSuccessFormAdmissao = function (response) {

        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormAdmissao"));
            }
        } catch (e) {
            console.log(e);
        }
    };

    //
    this.onSuccessFormBloquear = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormBloquear"));
            }
        } catch (e) {
            console.log(e);
        }
    };

    //Reenvio de link para alteração de senha do associado
    this.reenviarSenha = function (idAssociado) {

        var url = $("#baseUrlGeral").val() + "Associados/AssociadoAcao/enviar-link-senha/";

        var funcOk = function () {

            $.post(url, { idAssociado: idAssociado }, function (response) {

                if (response.error != undefined && response.error == true) {
                    jM.error(response.message);
                    return;
                }

                if (response.error != undefined && response.error == false) {
                    jM.success(response.message);
                    return;
                }

                jM.warning("Não foi possível completar a operação.");
            });
        }

        jM.confirmation("Essa opera&ccedil;&atilde;o ir&aacute; enviar um link para que o associado crie uma nova senha. Confirma o reenvio?", funcOk, function () { return false; });
    };

    //
    this.onSuccessFormInadimplencia = function (response) {
        DefaultSistema.reiniciarBotao();

        try {
            if (response.flagSucesso == true) {
                location.reload();
            } else {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormInadimplencia"));
            }
        } catch (e) {
            console.log(e);
        }
    };

    //
    this.alterarTipo = function (comboTipoAssociado) {

        var idTipoAtual = $("#Associado_idTipoAssociado").val();
        var idAssociado = $(comboTipoAssociado).data("id-associado");
        var idTipoAssociado = $(comboTipoAssociado).val();

        var fNo = function () {
            $(comboTipoAssociado).val(idTipoAtual);
            return false;
        };


        var fYes = function () {
            var url = $("#baseUrlGeral").val() + "associados/associadoacao/alterar-tipo-associado";

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

        jM.confirmation("Deseja realmente alterar o tipo do associado?", fYes, fNo);
    };

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

var AssociadoAcao = new AssociadoAcaoClass();

$(document).ready(function(){
    AssociadoAcao.init();
});
