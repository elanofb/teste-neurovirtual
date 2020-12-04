function ClassAssociado(){
    
    var idFormDadosPessoais = "#formDadosPessoais";
    var boxFormDadosPessoais = "#boxFormAssociado";
    var idInputNroDocumento = "#nroDocumento";

    //$(idFormDadosPessoais).loadingOverlay();

    //Metodo de inicializacao dos plugins
    this.init = function () {

		this.configurarPais();

		this.listenerTipoPessoa();

		this.listenerTipoDocumento();

		this.verificarDesativarAdmitir();

        this.iniciarAbas();

    };
    
    //Verififcar se o tipo de pessoa muda e configurar os campos conforme necessário
	this.listenerTipoPessoa = function () {
	    $('select.tipoPessoa').on('change', function (event) {
	        var tipoPessoa = $(this).val();
	        Associado.configurarTipoPessoa(tipoPessoa);
	        Associado.configurarTipoAssociado(tipoPessoa);
	    });

	    var tipoPessoaAtual = $('.tipoPessoa').val();
	    this.configurarTipoPessoa(tipoPessoaAtual);
	    this.configurarTipoAssociado(tipoPessoaAtual);
	};

    //Exibir e ocultar campos de acordo com tipo de pessoa
	this.configurarTipoPessoa = function (tipoPessoa) {

	    var comboTipoDocumento = $("#Associado_Pessoa_idTipoDocumento");

	    if (tipoPessoa == "J") { //Pessoa Juridica
	        $(".info-pf").hide();
	        $(".info-pj").show();
	        $(idInputNroDocumento).setMask("cnpj");

	        $(comboTipoDocumento).val("6");

	    } else {
	        $(".info-pf").show();
	        $(".info-pj").hide();
	        $(idInputNroDocumento).setMask("cpf");
	        this.configurarPais();
	        $(comboTipoDocumento).val("1");
        }

	    //Caso seja o primeiro carregamento da pagina remover a mensagem de carregamento e exibir o formulário. 
	    $(idFormDadosPessoais).removeClass("hidden");
	    $(idFormDadosPessoais).loadingOverlay("remove");
	};

    //Carregar o combo com os tipos de associados
	this.configurarTipoAssociado = function (tipoPessoa) {

	    var url = new String($("#baseUrlGeral").val()).concat("Associados/TipoAssociado/carregar-options");

	    var flagPF = null;
	    var flagPJ = null;

	    if (tipoPessoa == "F") {
	        flagPF = true;
	    }

	    if (tipoPessoa == "J") {
	        flagPJ = true;
	    }

	    var combo = $("#Associado_idTipoAssociado");

	    $.get(url, {
	        'flagPF': flagPF,
	        'flagPJ': flagPJ
	    },
            function (data) {
                if (data.length == 0) {
                    return;
                }

                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);
                combo.find("option:first").removeAttr("selected");

                $.each(data, function (key, item) {
                    combo.append(new Option(item.text, item.value, false));

                });
                combo.find("option:first").html(Vocabulary.select);

            }
        );
	}

    //Ouvinte do combo de tipos de documentos
	this.listenerTipoDocumento = function () {
	    var elemento = $("select.idTipoDocumentoAssociado");

	    elemento.on("change", function () {

	        var selectedOption = $(this).find("option:selected").text();

	        if (selectedOption == "CPF") {

	            $(idInputNroDocumento).setMask("cpf");

	        } else if (selectedOption == "CNPJ") {

	            $(idInputNroDocumento).setMask("cnpj");
	        } else {
	            $(idInputNroDocumento).setMask("*************");
	        }
	    });

	    elemento.change();
	}

    //Evento Pos-Submit do cadastro de associado
	this.onSuccess = function (response) {

	    if (response.error == false) {

	         location.href = (response.urlRedirecionamento);

            return;
        }

	    $("#tab-dados-cadastrais input").setMask();

	    $("#tab-dados-cadastrais").find(".link-loading").on('click', function () {
	        var btn = $(this).button().data('loading-text', 'Processando...');
	        btn.button('loading');
	    });

	    DefaultSistema.zerarErros();

	    Associado.listenerTipoPessoa();

	    Associado.listenerTipoDocumento();
	};

    //Evento Pos-Submit do cadastro de associado
	this.onSuccessEspecifico = function (response) {

	    if (response.error == false) {

	        location.href = (response.urlRedirecionamento);

	        return;
	    }

	    $("#tab-dados-especificos input").setMask();

	    DefaultSistema.iniciarBotoes();

	    DefaultSistema.zerarErros();

	    Associado.listenerTipoPessoa();
	};

    //Configurar País de origem
    this.configurarPais = function () {
    	var idPais = $("#idPaisNascimento").val();

    	if (idPais == "BRA" || idPais == "") {
    		$(".origem-brasil").show();
    		$(".origem-exterior").hide();
		} else {
    	    $(idInputNroDocumento).setMask("*************");
    		$(".origem-exterior").show();
    		$(".origem-brasil").hide();
		}

    	idPais = $("#idPaisEnderecoPrincipal").val();
    	if (idPais == "BRA" || idPais == "") {
    		$(".enderecoPrincipalBrasil").show();
    		$(".enderecoPrincipalExterior").hide();
    	} else {
    		$(".enderecoPrincipalExterior").show();
    		$(".enderecoPrincipalBrasil").hide();
    	}


    	idPais = $("#idPaisEnderecoSecundario").val();
    	if (idPais == "BRA" || idPais == "" || typeof (idPais) == 'undefined') {
    	    $(".enderecoSecundarioBrasil").show();
    	    $(".enderecoSecundarioExterior").hide();
    	} else {
    	    $(".enderecoSecundarioExterior").show();
    	    $(".enderecoSecundarioBrasil").hide();
    	}


    	idPais = $("#idPaisEnderecoHospital").val();
    	if (idPais == "BRA" || idPais == "") {
    		$(".enderecoHospitalBrasil").show();
    		$(".enderecoHospitalExterior").hide();
    	} else {
    		$(".enderecoHospitalExterior").show();
    		$(".enderecoHospitalBrasil").hide();
    	}
    };

	//
    this.verificarOcorrenciaRelacionamento = function () {
    	var objOcorrenciaAdmissao = $("#idOcorrenciaAdmissao");
    	var objOcorrenciaDesativar = $("#idOcorrenciaDesativar");

    	if ($("#idOcorrenciaRelacionamento").val() != '') {
    		if (typeof (objOcorrenciaAdmissao) != 'undefined') objOcorrenciaAdmissao.iCheck('uncheck');
    		if (typeof (objOcorrenciaDesativar) != 'undefined') objOcorrenciaDesativar.iCheck('uncheck');
		}
    }

	//
    this.verificarDesativarAdmitir = function () {
		$("#idOcorrenciaDesativar").on('ifChecked', function () {
			$("#idOcorrenciaRelacionamento").val('');
		});

		$("#idOcorrenciaAdmissao").on('ifChecked', function () {
			$("#idOcorrenciaRelacionamento").val('');
		});

		$("#idOcorrenciaReativar").on('ifChecked', function () {
			$("#idOcorrenciaRelacionamento").val('');
		});

    }


    //Tratamento para quantidade de abas do associado
    this.iniciarAbas = function () {
        $(".nav-tabs-custom").removeClass("nao-exibir");

        $('#tabs').bootstrapResponsiveTabs({
            minTabWidth: 70,
            maxTabWidth: 120
        });

    };

};

var Associado = new ClassAssociado();

$(document).ready(function(){

    Associado.init();

});
