function ClassFornecedor(){
    
    //
	this.init = function () {
	    this.exibirDadosPessoa();
    };
    

    //Exibir dados conforme tipo de pessoa
	this.exibirDadosPessoa = function () {
	    var tipoPessoa = $("#tipoPessoa").val();

	    if (tipoPessoa == "J") {
	        $(".dado-pf").hide();
	        $(".dado-pj").show();
	        $("#Fornecedor_Pessoa_nroDocumento").setMask("cnpj");
	        $(".info-documento").html("CNPJ");
	    } else {
	        $(".dado-pf").show();
	        $(".dado-pj").hide();
	        $("#Fornecedor_Pessoa_nroDocumento").setMask("cpf");
	        $(".info-documento").html("CPF");
	    }

	};

    //Abrir modal para cadastrar novo local de evento
	this.modalNovoFornecedor = function (elemento) {

	    var url = $(elemento).data("url");

	    $.get(url, {}, function (response) {

	        var Modal = $(response).modal({ title: 'Cadastrar Fornecedor' });

	        $(Modal).on("shown.bs.modal", function (e) {

	            DefaultSistema.iniciarPluginsAposAjax();

	            Fornecedor.exibirDadosPessoa();
	        });

	        $(Modal).on("hidden.bs.modal", function (e) {
	            $(this).remove();
	        });

	    });

	}

    //Retorno após submissoes do form no modal
	this.onSuccess = function (response) {

	    if (typeof (response.error) == 'undefined') {

	        DefaultSistema.iniciarPluginsAposAjax();

	        Fornecedor.exibirDadosPessoa();

	        return;
	    }

	    //Se o registro for salvo com sucesso
	    if (response.error === false) {

	        var combo = $("#Produto_idFornecedor");

	        var flagJaExiste = combo.find("option[value=" + response.id + "]").length > 0;

	        if (flagJaExiste === false) {

	            combo.append($("<option></option>").val(response.id).html(response.descricao));

	            combo.val(response.id);

	            DefaultSistema.removerModais();
	        }

	    }
	}
};

var Fornecedor = new ClassFornecedor();

$(document).ready(function(){
    Fornecedor.init();
});
