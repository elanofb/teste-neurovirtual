function ClassDevedor(){
    
    //
	this.init = function () {
	    Devedor.iniciarCombosDevedor();
	};

    //Exibir dados conforme tipo de pessoa
	this.exibirDadosPessoa = function () {
	    var tipoPessoa = $("#tipoPessoa").val();

	    if (tipoPessoa == "J") {
	        $(".dado-pf").hide();
	        $(".dado-pj").show();
	        $("#Devedor_Pessoa_nroDocumento").setMask("cnpj");
	        $(".info-documento").html("CNPJ");
	    } else {
	        $(".dado-pf").show();
	        $(".dado-pj").hide();
	        $("#Devedor_Pessoa_nroDocumento").setMask("cpf");
	        $(".info-documento").html("CPF");
	    }
	};

    //Abrir modal para cadastrar novo local de evento
	this.modalNovoDevedor = function (group) {

	    group = DefaultSistema.checkNull(group);
	    var groupCod = DefaultSistema.getGroupCod(group);

	    $(".idDevedor" + groupCod).select2("close");

	    var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/Devedor/modal-cadastrar-devedor";

	    $.get(url, {group : group}, function (response) {

	        var Modal = $(response).modal({ title: 'Cadastrar Devedor' });

	        $(Modal).on("shown.bs.modal", function (e) {
	            DefaultSistema.iniciarPluginsAposAjax($("#boxFormDevedor"));
	            Devedor.exibirDadosPessoa();
	        });

	        $(Modal).on("hidden.bs.modal", function (e) {
	            $(this).remove();
	        });

	    });
	}
	
    //Retorno apos submissoes do form no modal
	this.onSuccess = function (response) {

	    if (typeof (response.error) == 'undefined') {

	        DefaultSistema.iniciarPluginsAposAjax($("#boxFormDevedor"));

	        Devedor.exibirDadosPessoa();

	        return;
	    }

	    //Se o registro for salvo com sucesso
	    if (response.error === false) {

	        var groupCod = DefaultSistema.getGroupCod(response.group);

	        var combo = $(".idDevedor" + groupCod);
	        combo.select2("destroy");

	        $(".idDevedor").each(function (index) {
	            var flagJaExiste = $(this).find("option[value='" + response.id + "']").length > 0;

	            if (flagJaExiste === false) {
	                $(this).append($("<option></option>").val(response.id).html(response.descricao));
	            }
	        });

	        combo.val(response.id);

	        DefaultSistema.removerModais();

            Devedor.comboDevedor(combo);

	        var inputDocumento = $(".nroDocumentoDevedor" + groupCod);
	        var inputTelefone = $(".nroTelefoneDevedor" + groupCod);

	        if (!(inputDocumento.length) > 0 && !(inputTelefone.length > 0)) { return; }

	        inputDocumento.closest("div").show();
	        inputDocumento.val(response.nroDocumento);

	        inputTelefone.closest("div").show();
	        inputTelefone.val(response.nroTelefone);
	    }
	}

	this.iniciarCombosDevedor = function () {

	    if (!($(".idDevedor").length > 0)) { return; }

	    $(".idDevedor").each(function (index) {
	        Devedor.comboDevedor($(this));
	        Devedor.carregarInfoDevedor($(this));
	    });
	}

	this.comboDevedor = function (combo) {

	    combo.select2().select2('destroy');
	    $(combo).off("select2:select");

	    var group = DefaultSistema.checkNull($(combo).data("group"));
	    var groupCod = DefaultSistema.getGroupCod(group);

	    var linkAddDevedor = "<a href='javascript:;' class='bold pull-right' onclick='Devedor.modalNovoDevedor(" + group + ")'> &nbsp;&nbsp;<i class='fa fa-plus-o'></i> Adicionar Devedor</a>";

	    combo.select2({
	        width: 'resolve',
	        dropdownCssClass: "bigdrop",
	        allowClear: true,
	        placeholder: "Selecione...",
	        escapeMarkup: function (markup) { return markup; },
	        language: {
	            noResults: function () { return "Sem resultados. " + linkAddDevedor; },
	            inputTooShort: function () { return 'Informe o nome do devedor'; }
	        },

	        //Faz o autocomplete funcionar
	        ajax: {
	            url: $("#baseUrlGeral").val() + "FinanceiroLancamentos/Devedor/auto-complete-devedor/",
	            dataType: 'json',
	            delay: 250,
	            data: function (params) {
	                return { q: params.term, page: params.page };
	            },
	            processResults: function (data, params) {
	                params.page = params.page || 1;
	                return {
	                    results: data.items,
	                    pagination: { more: (params.page * 30) < data.total_count }
	                };
	            },
	            cache: true
	        },
	        minimumInputLength: 2
	    });

	    //Evento de onchange do select2
	    combo.on("select2:select", function (e) { Devedor.carregarInfoDevedor($(this)); });
	}

	this.carregarInfoDevedor = function (element) {

	    var group = DefaultSistema.checkNull($(element).data("group"));
	    var groupCod = DefaultSistema.getGroupCod(group);

	    var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/Devedor/autocomplete-informacoes-devedor/"
	    var id = $(element).select2('val');

	    var inputDocumento = $(".nroDocumentoDevedor" + groupCod);
	    var inputTelefone = $(".nroTelefoneDevedor" + groupCod);

	    if (!(inputDocumento.length) > 0 && !(inputTelefone.length > 0) || DefaultSistema.checkNull(id) == "") {
	        return;
	    }

	    inputDocumento.closest("div").hide();
	    inputDocumento.closest("div").hide();

	    $.post(url, { id : id }, function (response) {
	        if (response.error != undefined && response.error == false) {
	            inputDocumento.closest("div").show();
	            inputTelefone.closest("div").show();

	            inputDocumento.val(response.nroDocumento);
	            inputTelefone.val(response.nroTelefone);

	            $('body').find('input:text').setMask();

	            return;
	        }

	        if (response.error != undefined && response.error == false) {
	            jM.error(response.message);
	            return;
	        }
	    });
	}
};

var Devedor = new ClassDevedor();
$(document).ready(function(){
    Devedor.init();
});
