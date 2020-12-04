function ObjComboCentroCusto() {

    var baseUrlFinanceiro = $("#baseUrlGeral").val() + 'Financeiro/';
    
	this.init = function() {
	    ComboCentroCusto.iniciarComboCentroCusto();
        ComboCentroCusto.listenerRemoverSelecao();
	};
    
    /*Inicia todos os combo de Centro de Custo existentes na pagina*/
	this.iniciarComboCentroCusto = function () {
	    
        if (!($(".idCentroCusto").length > 0)) { return; }//Verifica se existe algum combo na pagina

	    $(".idCentroCusto").each(function(index) { ComboCentroCusto.comboCentroCusto($(this)); });
        
        $('.idCentroCusto').on('select2:unselect', function () {
            
            var combo = $(this);
            
            combo.val("");
            
            group = DefaultSistema.checkNull(combo.data("group"));
            
            ComboCentroCusto.carregarCentroCustoMacroConta(group);
            
        });
        
	}
    
    //Inicia o combo Centro de Custo, recebendo como parametro o proprio select
	this.comboCentroCusto = function (combo, groupParam) {
        
	    var group = "";

	    if (DefaultSistema.checkNull(groupParam) != "") {
	        group = groupParam;
	        combo = $(".idCentroCusto" + DefaultSistema.getGroupCod(group));

	        if (!(combo.length > 0)) {
	            return;
	        }
	    }

	    combo.select2().select2('destroy');
	    $(combo).off("select2:select"); //Remove o evento de onchange do combo

	    group = DefaultSistema.checkNull(combo.data("group"));

	    var linkAdd = function (descricao) {
	        return "<a href=\"javascript:;\" class=\"bold pull-right\" onclick=\"ComboCentroCusto.modalCentroCusto('" + group + "', '" + descricao + "')\"> &nbsp;&nbsp;<i class=\"fa fa-plus-o\"></i> Novo Centro de Custo</a>";
	    } 

	    combo.select2({
	        escapeMarkup: function (markup) { return markup; },
            width: '100%', dropdownCssClass: "bigdrop", allowClear: true, placeholder: "Selecione...",
            language: { noResults: function () { return "Sem resultados. " + linkAdd(event.target.value); } }
        });

	    combo.on("select2:select", function (e) { ComboCentroCusto.carregarCentroCustoMacroConta(group); });
    }

    //Carrega as macro contas relacionadas ao centro de custo selecionado.
	this.carregarCentroCustoMacroConta = function (group, selected) {

	    var groupCod = DefaultSistema.getGroupCod(group);
        
        //Verifica se existe um campo de macro conta em conjunto com o campo de centro de custo
        var combo = $(".idMacroConta" + groupCod);
        
        if (!(combo.length > 0)) {
            return;
        }
        
        var comboCentroCusto = $(".idCentroCusto" + groupCod);
        
        var idCentroCusto = comboCentroCusto.select2('val');
        
        var flagReceitaDespesa = comboCentroCusto.data("flagReceitaDespesa");
        
        combo.select2().select2('destroy');
        
        combo.find("option").remove().end().append(new Option("Carregando...", ""));
        
        $.post(baseUrlFinanceiro + 'MacroContaConsulta/listar-ajax', { "idCentroCusto": idCentroCusto, "flagReceitaDespesa": flagReceitaDespesa },
            function (data) {
                if (data.length > 0) {

                    combo.find("option").remove().end().append(new Option("Selecione", ""));

                    $.each(data, function(key, item) {
                        combo.append(new Option(item.text, item.value, null, (selected == item.value)));
                    });
                } else {
                    combo.find("option").remove().end().html("<option value=''>N&atilde;o encontrado</option>");
                }

                ComboMacroConta.comboMacroConta(combo);
            }
        )
    }

    //Abre a modal para adicionar um novo centro de custo
	this.modalCentroCusto = function (group, descricao) {

	    group = DefaultSistema.checkNull(group);
	    var groupCod = DefaultSistema.getGroupCod(group);

	    $(".idCentroCusto" + groupCod).select2("close");//Fecha o select2 para evitar sobreposição com o modal

	    $.get(baseUrlFinanceiro + 'CentroCusto/modal-cadastrar-centro-custo', { descricao : descricao, group: group, modalId: "modalBoxFormCentroConta" }, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Centro de Custo' });

            $(Modal).on("shown.bs.modal", function (e) { DefaultSistema.iniciarPluginsAposAjax($("#boxFormCentroCusto")); });

            $(Modal).on("hidden.bs.modal", function (e) { $(this).remove();});
        });
    }

    //Evento chamado ao submeter o form da modal de cadastrar o centro de custo
    this.onSuccessCentroCusto = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormCentroCusto"));
            return;
        }

        if (response.error === false) {

            var groupCod = DefaultSistema.getGroupCod(response.group);
            var combo = $(".idCentroCusto" + groupCod);

            combo.select2().select2("destroy");

            $(".idCentroCusto").each(function(index) {
                $(this).append($("<option></option>").val(response.id).html(response.descricao));
            });

            combo.val(response.id);

            $('#modalBoxFormCentroConta').modal('hide').data('bs.modal', null);
            ComboCentroCusto.comboCentroCusto(combo);
            ComboCentroCusto.carregarCentroCustoMacroConta(response.group);
        }
    }
    
    /*Remove a seleção do combo em foco e restaura a lista de macro contas*/
    this.listenerRemoverSelecao = function () {
        
        $('.idCentroCusto').on('select2:unselect', function () {
            
            var combo = $(this);

            combo.val("");

            group = DefaultSistema.checkNull(combo.data("group"));

            ComboCentroCusto.carregarCentroCustoMacroConta(group);

        });
    }
};

var ComboCentroCusto = new ObjComboCentroCusto();
$(document).ready(function(){
    ComboCentroCusto.init();
});
