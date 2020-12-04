function ObjComboSubConta() {

    var baseUrlFinanceiro = $("#baseUrlGeral").val() + 'Financeiro/';

	this.init = function() {
	};
 

    //Abre a modal para adicionar uma nova sub conta
    this.modalSubConta = function (id, idCategoriaPai) {

        $.get(baseUrlFinanceiro + 'SubContaCadastro/modal-cadastrar-sub-conta', { id : id, descricao : "", group: "", idMacroConta: 0, modalId: "modalBoxFormSubConta" }, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Sub-Conta' });

            $(Modal).on("shown.bs.modal", function(e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormSubConta"));
                PlanoContas.carregarSubContaPai(idCategoriaPai,id);
            });

            $(Modal).on("hidden.bs.modal", function(e) {
                $(this).remove();
            });
        });
    }

    //Abre a modal para excluir uma sub conta
    this.modalExcluirSubConta = function (id) {

        $.get(baseUrlFinanceiro + 'SubContaExclusao/modal-excluir-sub-conta', { id: id }, function (response) {

            var Modal = $(response).modal({ title: 'Excluir Sub-Conta' });

            $(Modal).on("shown.bs.modal", function(e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormSubConta"));
                PlanoContas.carregarSubConta(id);
            });

            $(Modal).on("hidden.bs.modal", function(e) {
                $(this).remove();
            });
        });
    }

    //Evento chamado ao submeter o form da modal de excluir a sub conta
    this.onSuccessSubContaExcluir = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormSubConta"));

            $("#boxFormSubConta").html(response);

            return;
        }

        if (response.error === false) {

            jM.success("Sub conta exclu&iacute;da com sucesso!", function () { location.reload() });
        }
    }

    //Evento chamado ao submeter o form da modal de cadastrar a sub conta
    this.onSuccessSubConta = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormSubConta"));

            return;
        }

        if (response.error === false) {

            var groupCod = DefaultSistema.getGroupCod(response.group);
            
            jM.success("Sub conta cadastrada com sucesso!", function () { location.reload() });
        }
    }

};

var ComboSubConta = new ObjComboSubConta();
$(document).ready(function(){
    ComboSubConta.init();
});
