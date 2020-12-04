function DefaultSistema(){

	this.init = function () {

	    //IniciarMenu
	    Nav.iniciarMenu();
	    
	    //Plugins
        this.iniciarPlugins();

	    //
        this.iniciarLinksAcao();

	    //
        this.iniciarAutoConteudo();

	    //
        this.iniciarTimersConteudo();

        //
        this.iniciarTabs();

        //
        this.iniciarModal();

	    //
        this.iniciarBlocoDetalhe();

        //
        this.iniciarLinhaLink();

	    //
	    this.zerarErros();

	};


    // Iniciar os plugins diversos do sistema usando as bibliotecas
    this.iniciarPlugins = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find('input:text').setMask();

        this.iniciarComplementoMask();

        $(conteudo).find("[data-toggle='tooltip']").tooltip();

        //Marcar todos os checkboxes usando o plugin icheck
        $(conteudo).find('input[type="checkbox"][name="marcarTodos"]').on('ifChecked', function (event) {
            var nomeFilhos = $(this).attr("data-childs");
            $('input[type="checkbox"][name="' + nomeFilhos + '"]').iCheck('check');
        });

        $(conteudo).find('input[type="checkbox"][name="marcarTodos"]').on('ifUnchecked', function (event) {
            var nomeFilhos = $(this).attr("data-childs");
            $('input[type="checkbox"][name="' + nomeFilhos + '"]').iCheck('uncheck');
        });

        DefaultSistema.iniciarBotoes(conteudo);

        //Inicializacao datepicker
        if (typeof iniciarDatePicker !== 'undefined' && $.isFunction(iniciarDatePicker)) {
            iniciarDatePicker();
        }

        this.zerarErros();
    }

    //
    this.iniciarBotoes = function (conteudo) {
        $(conteudo).find(".link-loading").button('reset');
        $(conteudo).find(".link-loading").on('click', function () {
            var btn = $(this).button().data('loading-text', 'Processando...');
            btn.button('loading');
        });
    };

    //Configura��o de a��es padr�o para links pre-configurados no sistema para exclus�es e altera��es de status no sistema.
    this.iniciarLinksAcao = function (conteudo) {
        if (!conteudo) {
            conteudo = $("body");
        }
        //Ouvinte do evento de troca de status de registro
        $(conteudo).find("a.ico-status").on('click', function () {
            DefaultAction.action(this, 'updateStatus');
            return false;
        });

        //Ouvinte do evento de exclus�o dos registros
        $(conteudo).find("a.delete-default").on('click', function () {
            DefaultAction.action(this, 'delete');
            return false;
        });
    };

    //Catpurar elementos configurados para abertura de moal
    this.iniciarModal = function () {
        $('[data-toggle="modal"]').click(function (e) {
            e.preventDefault();
            var url = $(this).attr('data-url');
            DefaultSistema.showModal(url);
        });
    };

    //respons�vel pela requisiao get e abertura do modal
    this.showModal = function (urlContent) {

        $.get(urlContent, function (data) {

            var Modal = $(data).modal();

            $(Modal).on("shown.bs.modal", function (e) {

                $('input:text').setMask();

                if (typeof (iniciarDatePicker) != 'undefined') {
                    iniciarDatePicker();
                }

                $("[data-toggle=tooltip]").tooltip();

                $(Modal).find(".link-loading").button('reset');

                $(Modal).find(".link-loading").on('click', function () {
                    var btn = $(this).button().data('loading-text', 'Processando...');
                    btn.button('loading');
                });

            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }
    
    //
    this.removerModais = function () {
        $(".modal").remove();
        $(".modal-backdrop").remove();
    };

    //Iniciar o plugin de tabs do bootstrap
    this.iniciarTabs = function () {
        $('a[data-toggle="tab"]').on('click', function () {
            //save the latest tab; use cookies if you like 'em better:
            localStorage.setItem('lastTab', $(this).attr('href'));
        });

        //go to the latest tab, if it exists:
        var lastTab = localStorage.getItem('lastTab');
        if (lastTab) {
            $('a[href=' + lastTab + ']').tab('show');
        } else {
            // Set the first tab if cookie do not exist
            $('a[data-toggle="tab"]:first').tab('show');
        }

        //Tratamento para voltar para a primeira aba ap�s usar a p�gina de edi��o.
        if (window.location.pathname.toString().indexOf("listar") != -1) {
            localStorage.setItem('lastTab', false);
        }
    };


    //Carregar conte�do dinamicamente via ajax
    this.iniciarTimersConteudo = function () {
        var secondsTimer = 600;
        var miliTimer = (secondsTimer * 1000);

        DefaultSistema.carregarBlocoDinamico();

        setInterval(function () {
            DefaultSistema.carregarBlocoDinamico();
        }, miliTimer);
    };


    //Carregar conte�do dinamicamente via ajax e que deve ser atualizado automaticamente.
    this.carregarBlocoDinamico = function () {
        $(".box-dinamico").each(function () {
            DefaultSistema.carregarConteudo($(this));
        });
    };


    //Carregar conte�do dinamicamente via ajax apenas no carregamento da p�gina
    this.iniciarAutoConteudo = function (conteudo) {
        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find(".content-load").each(function () {
            DefaultSistema.carregarConteudo($(this));
        });
    };

    //
    this.carregarConteudoElemento = function (idElemento) {
        this.carregarConteudo($("#" + idElemento));
    };

    //
    this.carregarConteudo = function (element, fnCallback) {
        var div = $(element);

        var url = div.attr("data-url");
        var targetId = div.attr("data-target");
        var divAlvo = $("#" + targetId);

        if (typeof (targetId) == 'undefined' || targetId == '') {
            divAlvo = $(div);
        }

        $.ajax({
            url: url, type: "GET", cache: false,
            success: function (response) {
                divAlvo.html(response);
                $(div).removeClass("carregando");
                $(divAlvo).removeClass("carregando");
                DefaultSistema.iniciarPluginsAposAjax(divAlvo);

                if (divAlvo.find(".treeview").length > 0) {
                    $(".sidebar .treeview").tree();
                };

                if (fnCallback != null && $.isFunction(fnCallback)) {
                    fnCallback();
                }

            },
            error: function (response, e, h) {
                div.html(response);
            }
        });
    };

    //Reiniciar os plugins apos um bloco ser carregado via ajax
    this.iniciarPluginsAposAjax = function (conteudo) {
        //console.log(conteudo);

        DefaultSistema.iniciarPlugins(conteudo);

        DefaultSistema.iniciarLinksAcao(conteudo);
    };

    //Limpar os valores dos campos do formul�rio para novas inser��es
    this.limparForm = function (oForm) {
        $(oForm).find("input[type!=button][type!=submit]:not(.nao-limpar)").val('');
        $(oForm).find('select:not(.nao-limpar)').val('');
        $(oForm).find('textarea:not(.nao-limpar)').val('');
    };

	//
    this.checkAll = function () {
    	$('input[type="checkbox"][name="marcarTodos"]').on('ifChecked', function (event) {
    		var nomeFilhos = $(this).attr("data-childs");
    		$('input[type="checkbox"][name="' + nomeFilhos + '"]').iCheck('check');
    	});

    	$('input[type="checkbox"][name="marcarTodos"]').on('ifUnchecked', function (event) {
    		var nomeFilhos = $(this).attr("data-childs");
    		$('input[type="checkbox"][name="' + nomeFilhos + '"]').iCheck('uncheck');
    	});
    };

	//
    this.customValidators = function () {
    	$.validator.methods.date = function (value, element) {
    		return this.optional(element) || Globalize.parseDate(value, 'dd/MM/yyyy') !== null;
    	}
    	$.validator.methods.number = function (value, element) {
    		return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    	};
    };

    //
    this.reiniciarBotao = function () {
        setTimeout(function () {
            $(".link-loading").button('reset');
            $(".link-loading").on('click', function () {
                var btn = $(this).button().data('loading-text', 'Processando...');
                btn.button('loading');
            });
        }, 500);
    }

    //
    this.zerarErros = function () {

        $(".input-validation-error").on("focus", function () {
            $(this).removeClass("input-validation-error");

            $(this).siblings(".field-validation-error").hide();
        });
    };

    //
    this.iniciarComplementoMask = function () {

        $("input[alt=phone]").on('keyup', function () {
            if ($(this).val().length > 14) {
                $(this).setMask("(99) 99999-9999");
            } else {
                $(this).setMask({ mask: "(99) 9999-99999", autoTab: false });
            }
        });
        
        $("input[alt=cnpf]").not(".cnpf-ajustado").each(function () {
            if ($(this).val().length > 11) {
                $(this).setMask({ mask: "99.999.999/9999-99", autoTab: false });
            } else {
                $(this).setMask({ mask: "999.999.999-999", autoTab: false });
            }

            $(this).addClass("cnpf-ajustado");
        });

    };

    //
    this.autoResizeIframe = function (quem) {
        if(navigator.appName.indexOf("Internet Explorer")>-1){
            var func_temp = function () {

                var val_temp = quem.contentWindow.document.body.parentNode.scrollHeight
                quem.style.height = val_temp + "px";
            }

            setTimeout(function() { func_temp() }, 100)
        } else {
            var val = quem.contentWindow.document.body.parentNode.offsetHeight;
            quem.style.height = val + "px";

        }    
    }

    //Ocultar o menu principal
    this.hideMenuPrincipal = function () {

        $("body").addClass("sidebar-collapse");
    };    
};

var DefaultSistema = new DefaultSistema();


$(document).ready(function () {
    
    DefaultSistema.init();
    DefaultSistema.customValidators();
    DefaultSistema.checkAll();
});
