function ObjComboMacroConta() {

    var baseUrlFinanceiro = $("#baseUrlGeral").val() + 'Financeiro/';

	this.init = function() {
	};
    
    //Abre a modal para adicionar uma nova macro conta
    this.modalMacroConta = function (id) {

        $.get(baseUrlFinanceiro + 'MacroContaCadastro/modal-cadastrar-macro-conta', { id : id, descricao : "", group: "", idCentroCusto: 0, modalId: "modalBoxFormComboMacroConta" }, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Macro Conta' });

            $(Modal).on("shown.bs.modal", function (e) { DefaultSistema.iniciarPluginsAposAjax($("#boxFormMacroConta")); });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }

    //Evento chamado ao submeter o form da modal de cadastrar a macro conta
    this.onSuccessMacroConta = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormMacroConta"));
            return;
        }

        if (response.error === false) {

            var groupCod = DefaultSistema.getGroupCod(response.group);
            
            jM.success("Macro conta cadastrada com sucesso!", function () { location.reload() });
        }
    }

    //Abre a modal para excluir uma macro conta
    this.modalExcluirMacroConta = function (id) {

        $.get(baseUrlFinanceiro + 'MacroContaExclusao/modal-excluir-macro-conta', { id: id }, function (response) {

            var Modal = $(response).modal({ title: 'Excluir Macro Conta' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormMacroConta"));

            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }

    //Evento chamado ao submeter o form da modal de excluir a macro conta
    this.onSuccessMacroContaExcluir = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormMacroConta"));

            $("#boxFormMacroConta").html(response);

            return;
        }

        if (response.error === false) {

            jM.success("Sub conta exclu&iacute;da com sucesso!", function () { location.reload() });
        }
    }

};

var ComboMacroConta = new ObjComboMacroConta();
$(document).ready(function(){
    ComboMacroConta.init();
});
