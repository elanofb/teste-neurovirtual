function ClassCredor(){
    
    //
	this.init = function() {
	    Credor.iniciarCombosCredor();
	};

    //Exibir dados conforme tipo de pessoa
	this.exibirDadosPessoa = function () {
	    var tipoPessoa = $("#tipoPessoa").val();

	    if (tipoPessoa == "J") {
	        $(".dado-pf").hide();
	        $(".dado-pj").show();
	        $("#Credor_Pessoa_nroDocumento").setMask("cnpj");
	        $(".info-documento").html("CNPJ");
	    } else {
	        $(".dado-pf").show();
	        $(".dado-pj").hide();
	        $("#Credor_Pessoa_nroDocumento").setMask("cpf");
	        $(".info-documento").html("CPF");
	    }
	};

    //Abrir modal para cadastrar novo local de evento
	this.modalNovoCredor = function (group) {

	    group = DefaultSistema.checkNull(group);
	    var groupCod = DefaultSistema.getGroupCod(group);

        $(".idCredor" + groupCod).select2("close");

	    var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/CredorCadastro/modal-cadastrar-credor";

	    $.get(url, {group : group}, function (response) {

	        var Modal = $(response).modal({ title: 'Cadastrar Credor' });

	        $(Modal).on("shown.bs.modal", function (e) {
	            DefaultSistema.iniciarPluginsAposAjax($("#boxFormCredor"));
	            Credor.exibirDadosPessoa();
	        });

	        $(Modal).on("hidden.bs.modal", function (e) {
	            $(this).remove();
	        });

	    });
	}

    //Retorno apos submissoes do form no modal
	this.onSuccess = function (response) {

	    if (typeof (response.error) == 'undefined') {

	        DefaultSistema.iniciarPluginsAposAjax($("#boxFormCredor"));

	        Credor.exibirDadosPessoa();

	        return;
	    }

	    //Se o registro for salvo com sucesso
	    if (response.error === false) {

	        var groupCod = DefaultSistema.getGroupCod(response.group);

	        var combo = $(".idCredor" + groupCod);
	        combo.select2("destroy");

	        $(".idCredor").each(function (index) {
	            var flagJaExiste = $(this).find("option[value='" + response.id + "']").length > 0;

	            if (flagJaExiste === false) {
	                $(this).append($("<option></option>").val(response.id).html(response.descricao));
	            }
	        });

	        combo.val(response.id);

	        DefaultSistema.removerModais();

	        Credor.comboCredor(combo);

	        var inputDocumento = $(".nroDocumentoCredor" + groupCod);
	        var inputTelefone = $(".nroTelefoneCredor" + groupCod);

	        if (!(inputDocumento.length) > 0 && !(inputTelefone.length > 0)) { return; }

	        inputDocumento.closest("div").show();
	        inputDocumento.val(response.nroDocumento);

	        inputTelefone.closest("div").show();
	        inputTelefone.val(response.nroTelefone);

            DadosBancariosCombo.carregarComboDadoBancario(response.idPessoa);
	    }
	}

	this.iniciarCombosCredor = function () {

	    if (!($(".idCredor").length > 0)) { return; }

	    $(".idCredor").each(function(index) {
	        Credor.comboCredor($(this));
	        Credor.carregarInfoCredor($(this));
	    });
	    
	}

	this.comboCredor = function (combo) {

        combo.select2().select2('destroy');
        $(combo).off("select2:select");

        var group = DefaultSistema.checkNull($(combo).data("group"));
        var groupCod = DefaultSistema.getGroupCod(group);

        var linkAddCredor = "<a href='javascript:;' class='bold pull-right' onclick='Credor.modalNovoCredor(" + group + ")'> &nbsp;&nbsp;<i class='fa fa-plus-o'></i> Adicionar Credor</a>";
		
        combo.select2({
            width: 'resolve',
            dropdownCssClass: "bigdrop",
            allowClear: true,
            placeholder: "Selecione...",
            escapeMarkup: function (markup) { return markup; },
            language: {
                noResults: function () { return "Sem resultados. " + linkAddCredor; },
                inputTooShort: function () { return 'Informe o nome do credor'; }
            },
			
            //Faz o autocomplete funcionar
            ajax: {
                url: $("#baseUrlGeral").val() + "FinanceiroLancamentos/CredorLista/auto-complete-credor/",
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
        combo.on("select2:select", function (e) { Credor.carregarInfoCredor($(this)); });
	}
	
    //Carregas as informa��es do credor de acordo com o valor selecionado no combo de credor
	this.carregarInfoCredor = function (element) {

	    var group = DefaultSistema.checkNull($(element).data("group"));
	    var groupCod = DefaultSistema.getGroupCod(group);


	    var url = $("#baseUrlGeral").val() + "FinanceiroLancamentos/CredorLista/autocomplete-informacoes-credor/";
	    var id = $(element).select2('val');
	    
	    if (id == ""){
	    	id = $(element).attr('datavalue');
		} 

	    var inputDocumento = $(".nroDocumentoCredor" + groupCod);
	    var inputTelefone = $(".nroTelefoneCredor" + groupCod);

	    if (!(inputDocumento.length) > 0 && !(inputTelefone.length > 0) || DefaultSistema.checkNull(id) == "") {
	        return;
	    }

	    inputDocumento.closest("div").hide();
	    inputTelefone.closest("div").hide();

	    $.post(url, { id : id }, function (response) {
	        if (response.error != undefined && response.error == false) {
	            inputDocumento.closest("div").show();
	            inputTelefone.closest("div").show();

                var newOption = new Option(response.text, response.id, true);
                $(element).append(newOption);

                $(element).val(id);

                // $(element).select2();
                
	            inputDocumento.val(response.nroDocumento);
	            inputTelefone.val(response.nroTelefone);
	            
	            DadosBancariosCombo.carregarComboDadoBancario(response.idPessoa);

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

var Credor = new ClassCredor();
$(document).ready(function(){
    Credor.init();
});
