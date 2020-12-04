function ObjComboSubConta() {

    var baseUrlFinanceiro = $("#baseUrlGeral").val() + 'Financeiro/';

	this.init = function() {
	    ComboSubConta.iniciarComboSubConta();
	};
 
    //Inicia todos os combo de Macro Conta existentes na pagina
    this.iniciarComboSubConta = function () {
        if (!($(".idSubConta").length > 0)) { return; }//Verifica se existe algum combo na pagina

        $(".idSubConta").each(function(index) { ComboSubConta.comboSubConta($(this)); });
    }

    //Inicia o combo Centro de Custo, recebendo como parâmetro o próprio select
    this.comboSubConta = function (combo, groupParam) {

        var group = "";

        if (DefaultSistema.checkNull(groupParam) != "") {
            group = groupParam;
            combo = $(".idSubConta" + DefaultSistema.getGroupCod(group));

            if (!(combo.length > 0)) {
                return;
            }
        }

        combo.select2().select2('destroy');

        group = DefaultSistema.checkNull(combo.data("group"));

        var placeHolder = "Selecione...";
        var linkAdd = function(descricao) { 
            return "<a href=\"javascript:;\" class=\"bold pull-right\" onclick=\"ComboSubConta.modalSubConta('" + group + "', '" + descricao + "')\"> &nbsp;&nbsp;<i class=\"fa fa-plus-o\"></i> Nova Sub-Conta</a>"; //Iniciar o link para adicionar nova sub conta
        } 

        combo.select2({
            escapeMarkup: function (markup) { return markup; },
            width: '100%', dropdownCssClass: "bigdrop", allowClear: true, placeholder: placeHolder,
            language: { noResults: function () { return "Sem resultados." + linkAdd(event.target.value); } }
        });
    }

    //Abre a modal para adicionar uma nova sub conta
    this.modalSubConta = function (group, descricao) {

        group = DefaultSistema.checkNull(group);
        var groupCod = DefaultSistema.getGroupCod(group);

        $(".idSubConta" + groupCod).select2("close");//Fecha o select2 para evitar sobreposição com o modal

        var idMacroConta = $(".idMacroConta" + groupCod).val();

        $.get(baseUrlFinanceiro + 'SubContaCadastro/modal-cadastrar-sub-conta', { descricao : descricao, group: group, idMacroConta: idMacroConta, modalId: "modalBoxFormSubConta" }, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Sub-Conta' });

            $(Modal).on("shown.bs.modal", function(e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormSubConta"));
                
                $('#idMacroContaForm').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true
                });
            });

            $(Modal).on("hidden.bs.modal", function (e) { $(this).remove(); });
        });
    }

    //Evento chamado ao submeter o form da modal de cadastrar a sub conta
    this.onSuccessSubConta = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormSubConta"));

            $('#idMacroContaForm').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });

            return;
        }

        if (response.error === false) {

            var groupCod = DefaultSistema.getGroupCod(response.group);

            $('#modalBoxFormSubConta').modal('hide').data('bs.modal', null);

            //Caso haja uma macro conta, atualizar os dados do combo dinamicamente
            var comboMacroConta = $(".idMacroConta" + groupCod);
            if (comboMacroConta.length > 0 && comboMacroConta.val() > 0) {
                ComboMacroConta.carregarSubContaMacroConta(response.group, response.id);
                return;
            }

            var combo = $(".idSubConta" + groupCod);

            combo.select2().select2("destroy");

            $(".idSubConta").each(function (index) {
                $(this).append($("<option></option>").val(response.id).html(response.descricao));
            });

            combo.val(response.id);

            ComboSubConta.comboSubConta(combo);
        }
    }
};

var ComboSubConta = new ObjComboSubConta();
$(document).ready(function(){
    ComboSubConta.init();
});
