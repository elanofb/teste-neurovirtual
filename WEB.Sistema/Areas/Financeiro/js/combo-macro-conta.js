function ObjComboMacroConta() {

    var baseUrlFinanceiro = $("#baseUrlGeral").val() + 'Financeiro/';

	this.init = function() {
	    ComboMacroConta.iniciarComboMacroConta();
        ComboMacroConta.listenerRemoverSelecao();
	};
    
    //Inicia todos os combo de Macro Conta existentes na pagina
    this.iniciarComboMacroConta = function () {
        if (!($(".idMacroConta").length > 0)) { return; }//Verifica se existe algum combo na pagina

        $(".idMacroConta").each(function (index) { ComboMacroConta.comboMacroConta($(this)); });
    }
    
    //Inicia o combo Centro de Custo, recebendo como parametro o proprio select
    this.comboMacroConta = function (combo, groupParam) {

        var group = "";

        if (DefaultSistema.checkNull(groupParam) != "") {
            group = groupParam;
            combo = $(".idMacroConta" + DefaultSistema.getGroupCod(group));

            if (!(combo.length > 0)) {
                return;
            }
        }

        combo.select2().select2('destroy');
        $(combo).off("select2:select");//Remove o evento de onchange do select2

        group = DefaultSistema.checkNull(combo.data("group"));
        var groupCod = DefaultSistema.getGroupCod(group);

        var placeHolder = "Selecione...";

        var linkAdd = function (descricao) {

            return "<a href=\"javascript:;\" class=\"bold pull-right\" onclick=\"ComboMacroConta.modalMacroConta('" + group + "', '" + descricao + "')\"> &nbsp;&nbsp;<i class=\"fa fa-plus-o\"></i> Nova Macro Conta</a>";

        };

        //Verifica se existe um combo de centro de custo vinculado para setar o placeholder e linkAdd de acordo
        var comboCentroCusto = $(".idCentroCusto" + groupCod);
        if (comboCentroCusto.length > 0 && (comboCentroCusto.val() == "" || comboCentroCusto.val() == undefined)) {
            placeHolder = "Selecione o Centro de Custo";
            linkAdd = function () {
                return "";
            };
        }

        combo.select2({
            escapeMarkup: function (markup) { return markup; },
            width: '100%', dropdownCssClass: "bigdrop", allowClear: true, placeholder: placeHolder,
            language: { noResults: function () { return "Sem resultados." + linkAdd(event.target.value); } }


        });

        combo.on("select2:select", function (e) { ComboMacroConta.carregarSubContaMacroConta(group); });
    }

    //Carrega as macro contas relacionadas ao centro de custo selecionado.
    this.carregarSubContaMacroConta = function (group, selected) {

        var groupCod = DefaultSistema.getGroupCod(group);

        //Verifica se existe um campo de de categoria em conjunto com o campo de macro conta
        var combo = $(".idSubConta" + groupCod);
        if (!(combo.length > 0)) {
            return;
        }

        var idMacroConta = $(".idMacroConta" + groupCod).val();

        combo.select2().select2('destroy'); //Destroi o select2 para uma melhor manipulação

        combo.find("option").remove().end().append(new Option("Carregando...", ""));

        $.post(baseUrlFinanceiro + 'SubContaConsulta/listar-ajax', { "idMacroConta": idMacroConta },
            function (data) {
                if (data.length > 0) {

                    combo.find("option").remove().end().append(new Option("Selecione", ""));

                    $.each(data, function (key, item) {
                        combo.append(new Option(item.text, item.value, null, (selected == item.value)));
                    });
                } else {
                    combo.find("option").remove().end().html("<option value=''>N&atilde;o encontrado</option>");
                }

                ComboSubConta.comboSubConta(combo);
            }
        )
    }

    //Abre a modal para adicionar uma nova macro conta
    this.modalMacroConta = function (group, descricao) {

        group = DefaultSistema.checkNull(group);
        var groupCod = DefaultSistema.getGroupCod(group);

        $(".idMacroConta" + groupCod).select2("close");//Fecha o select2 para evitar sobreposição com o modal

        var idCentroCusto = $(".idCentroCusto" + groupCod).val();

        $.get(baseUrlFinanceiro + 'MacroContaCadastro/modal-cadastrar-macro-conta', { descricao : descricao, group: group, idCentroCusto: idCentroCusto, modalId: "modalBoxFormMacroConta" }, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Macro Conta' });

            $(Modal).on("shown.bs.modal", function (e) { DefaultSistema.iniciarPluginsAposAjax($("#boxFormMacroConta")); });

            $(Modal).on("hidden.bs.modal", function (e) { $(this).remove(); });
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

            $('#modalBoxFormMacroConta').modal('hide').data('bs.modal', null);
            ComboMacroConta.carregarSubContaMacroConta(response.group);

            //Caso haja um centro de custo, atualizar os dados do combo dinamicamente
            var comboCentroCusto = $(".idCentroCusto" + groupCod);
            if (comboCentroCusto.length > 0) {
                ComboCentroCusto.carregarCentroCustoMacroConta(response.group, response.id);
                return;
            }

            var combo = $(".idMacroConta" + groupCod);

            combo.select2("destroy");

            $(".idMacroConta").each(function (index) {
                $(this).append($("<option></option>").val(response.id).html(response.descricao));
            });

            combo.val(response.id);

            ComboMacroConta.comboMacroConta(combo);
        }
    }
    
    //Função usada na modal de cadastro, para apresentar os campos de acordo com o tipo de macro conta selecionado
    this.verificarTipoMacroConta = function () {
        var combo = $("#flagReceitaDespesa");
        var tipoMacroConta = $(combo).val();

        if (tipoMacroConta == "D" || tipoMacroConta == "A") {
            $("#boxUsuarioAprovacao").show();
        } else {
            $("#boxUsuarioAprovacao").hide();
        }
    }
    
    /*Remove a seleção do combo em foco e restaura a lista de macro contas*/
    this.listenerRemoverSelecao = function () {
        
        $('.idMacroConta').on('select2:unselect', function () {
            
            var combo = $(this);

            combo.val("");

            group = DefaultSistema.checkNull(combo.data("group"));

            ComboMacroConta.carregarSubContaMacroConta(group);

        });
    }
};

var ComboMacroConta = new ObjComboMacroConta();
$(document).ready(function(){
    ComboMacroConta.init();
});
