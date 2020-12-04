function DefaultSistemaClass() {

	this.init = function () {
	    
	    //IniciarMenu
	    Nav.iniciarMenuLateral();

	    Nav.iniciarMenuTopo();
	    
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
        this.iniciarLinhaLink();

	    //
	    this.iniciarCheckBoxes();

	    //
        this.iniciarDropAutomatic();
	};

    // Iniciar verificação de posição a ser executado o drop
	this.iniciarDropAutomatic = function () {
        $(document).on("shown.bs.dropdown", ".dropdown", function () {

            // calculate the required sizes, spaces
            var $ul = $(this).children(".dropdown-menu");
            var $button = $(this).children(".dropdown-toggle");
            var ulOffset = $ul.offset();
            // how much space would be left on the top if the dropdown opened that direction
            var spaceUp = (ulOffset.top - $button.height() - $ul.height()) - $(window).scrollTop();
            // how much space is left at the bottom
            var spaceDown = $(window).scrollTop() + $(window).height() - (ulOffset.top + $ul.height());
            // switch to dropup only if there is no space at the bottom AND there is space at the top, or there isn't either but it would be still better fit
            if (spaceDown < 0 && (spaceUp >= 0 || spaceUp > spaceDown))
                $(this).addClass("dropup");
        }).on("hidden.bs.dropdown", ".dropdown", function() {
            // always reset after close
            $(this).removeClass("dropup");
        });
    }

    // Iniciar e tratar linhas de tabelas que possuam func��o de link e configura��o da a��o do clique
    this.iniciarLinhaLink = function () {
        $("tr.link td").on("click", function () {
            var action = $(this).parent().attr("data-action");
            location.href = (action);
        });
    };

    //Quando um checkbox com name=marcarTodos for clicado, deve marcar/desmarcar os checkboxes filhos
    this.iniciarCheckBoxes = function() {

        $("#checkMarcarTodos").on("click", function () {
            var status = $(this).is(":checked");

            var selectorFilho = $(this).data("childs");

            $("input[name='" + selectorFilho + "']").each(function () {
                $(this).prop("checked", status);
            });
        });

        $(".checkMarcarTodos").each(function () {

            $(this).on("click", function () {
                var status = $(this).is(":checked");

                var selectorFilho = $(this).data("childs");

                $("input[name='" + selectorFilho + "']").each(function () {
                    $(this).prop("checked", status);
                });
            });

        })
        
    };


    // Iniciar os plugins diversos do sistema usando as bibliotecas
    this.iniciarPlugins = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find('input:text').setMask();

        this.iniciarComplementoMask();

        DefaultSistema.iniciarBotoes(conteudo);

        //Iniciar JQuery.Loading
        if (typeof ($.fn.loadingOverlay) == 'function') {
            $.fn.loadingOverlay.defaults.loadingText = "Carregando";
        }

        //
        if (typeof ($.fn.fileinput) != 'undefined') {
            var $input = $(conteudo).find('input.file[type=file]'), count = $input.attr('type') != null ? $input.length : 0;
            if (count > 0) {
                $input.fileinput();
            }
        }

        if (typeof ($.fn.tooltip) != 'undefined') {
            $(conteudo).find("[data-toggle=tooltip]").not("[data-original-title]").tooltip({
                container: 'body'
            });
        }

        if (typeof (AppAutoComplete) != 'undefined' && typeof (AppAutoComplete.iniciarSelect2) != 'undefined') {
            AppAutoComplete.iniciarSelect2($(conteudo));
        }
        
        if (typeof ($.fn.multiselect) != 'undefined') {
            $(".input-multiselect").multiselect({
                buttonClass: 'btn btn-sm btn-default', numberDisplayed: 1
            });
        }

        if (typeof (iniciarDatePicker) != 'undefined') {
            iniciarDatePicker();
        }

        if (typeof (iniciarDateTimePicker) != 'undefined') {
            iniciarDateTimePicker();
        }

        this.zerarErros();
    };

    //
    this.iniciarBotoes = function (conteudo) {

        $(conteudo).find(".link-loading").button('reset');
        $(conteudo).find(".link-loading").on('click', function () {
            var btn = $(this).button().data('loading-text', 'Processando...');
            btn.button('loading');
        });

        $(conteudo).find(".link-loading-min").button('reset');
        $(conteudo).find(".link-loading-min").on('click', function () {
            var btnMin = $(this).button().data('loading-text', "<i class=\"far fa-spin fa-sync\"></i>");
            btnMin.button('loading');
        });

    };

    //Configuracao de acoes padrao para links pre-configurados no sistema para exclusoes e alteracoes de status no sistema.
    this.iniciarLinksAcao = function (conteudo) {
                
        if (!conteudo) {
            conteudo = $("body");
        }

        DefaultSistema.iniciarLinhaLink();

        //Ouvinte do evento de troca de status de registro
        $(conteudo).find("a.ico-status").on('click', function () {
            DefaultAction.action(this, 'updateStatus');
            return false;
        });

        //Ouvinte do evento de exclusao dos registros
        $(conteudo).find("a.delete-default").on('click', function () {
            DefaultAction.action(this, 'delete');
            return false;
        });
    };

    //responsavel pela requisiao get e abertura do modal
    this.showModal = function (urlContent, funcSuccess) {
        
        $.ajax({

            url: urlContent, 
            type: 'GET',
            cache: false, 
            contentType: false, 
            processData: false,
            success: function (data) {

                DefaultSistema.exibirModal(data, funcSuccess);                
                
            },
            complete: function(xhr, textStatus) {
                
                if (xhr.status == 403) {

                    DefaultSistema.exibirModal(xhr.responseText);
                    
                }
                
            }
            
        })
        
    };
    
    this.exibirModal = function(data, funcSuccess) {

        if ((data.flagErro != "undefined" && data.flagErro == true)) {
            if (data.message) {
                jM.error(data.message);
            } else {
                jM.error("N&atilde;o foi poss&iacute;vel realizar a opera&ccedil;&atilde;o");
            }

            DefaultSistema.reiniciarBotao();
            return false;
        }

        var Modal = $(data).modal();

        $(Modal).on("shown.bs.modal", function (e) {

            DefaultSistema.reiniciarBotao();

            $('input:text').setMask();

            if (typeof (iniciarDatePicker) != 'undefined') {
                iniciarDatePicker();
            }

            if (typeof(AppAutoComplete) != 'undefined' && typeof(AppAutoComplete.iniciarSelect2) != 'undefined') {
                AppAutoComplete.iniciarSelect2(Modal);
            }

            $("[data-toggle=tooltip]").tooltip({
                container: 'body'
            });

            if (typeof ($.fn.fileinput) != 'undefined') {
                var $input = $(Modal).find('input.file[type=file]'), count = $input.attr('type') != null ? $input.length : 0;
                if (count > 0) {
                    $input.fileinput();
                }
            }

            DefaultSistema.iniciarBotoes(Modal);

            if (typeof funcSuccess !== 'undefined' && $.isFunction(funcSuccess)) {
                funcSuccess();
            }

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });
        
    }

    //
    this.removerModais = function () {

        $(".modal").remove();

        $(".modal-backdrop").remove();

    };

    //Iniciar o plugin de tabs do bootstrap
    this.iniciarTabs = function () {

        //save the latest tab; use cookies if you like 'em better:
        $('a[data-toggle="tab"]').on('click', function () {
            localStorage.setItem('lastTab', $(this).attr('href'));
        });

        //Ao abrir um aba carregar as div com conte�do din�mico para exibicao
        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {

            var target = $(e.target).attr("href"); // activated tab
            DefaultSistema.iniciarAutoConteudo($(target));

        });

        //go to the latest tab, if it exists:
        var lastTab = localStorage.getItem('lastTab');
        if (lastTab) {
            $('a[href="' + lastTab + '"]').tab('show');
        } else {
            // Set the first tab if cookie do not exist
            $('a[data-toggle="tab"]:first').tab('show');
        }

        //Tratamento para voltar para a primeira aba ap�s usar a p�gina de edi��o.
        if (window.location.pathname.toString().indexOf("listar") != -1) {
            localStorage.setItem('lastTab', false);
        }


    };


    //Carregar conteudo dinamicamente via ajax
    this.iniciarTimersConteudo = function () {
        var secondsTimer = 600;
        var miliTimer = (secondsTimer * 1000);

        DefaultSistema.carregarBlocoDinamico();

        setInterval(function () {
            DefaultSistema.carregarBlocoDinamico();
        }, miliTimer);
    };


    //Carregar conteudo dinamicamente via ajax e que deve ser atualizado automaticamente.
    this.carregarBlocoDinamico = function () {
        $(".box-dinamico").each(function () {
            DefaultSistema.carregarConteudo($(this));
        });
    };


    //Carregar conteudo dinamicamente via ajax apenas no carregamento da pagina
    this.iniciarAutoConteudo = function (conteudo) {
        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find(".content-load:visible").each(function () {
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

        if (fnCallback == null) {
            fnCallback = div.data("fncallback");
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
                
                if (fnCallback != null) {
                    if ($.isFunction(fnCallback)) {
                        fnCallback();
                    } else {
                        eval(fnCallback);
                    }
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


    //Limpar os valores dos campos do formulario para novas insercoes
    this.limparForm = function (oForm) {
        $(oForm).find("input[type!=button][type!=submit]:not(.nao-limpar)").val('');
        $(oForm).find('select:not(.nao-limpar)').val('');
        $(oForm).find('textarea:not(.nao-limpar)').val('');
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
    this.reiniciarBotao = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        setTimeout(function () {
            DefaultSistema.iniciarBotoes(conteudo);
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

    // Extensao $.serialize
    this.serializeExtensions = function () {
        $.fn.serializeObject = function () {
            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        };
    };

    this.alterarVisaoUnidade = function (element) {
        
        var idUnidade = $(element).val();

        var callback = function (data) {
            location.reload(true);
        };
        
        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + 'Unidades/Unidade/loadVisaoUnidade', { "idVisaoUnidade": idUnidade }, callback, null, 'post', 'json', false);
    };

    //Recarregar o sistema de modo que o usuario logado seja vinculado e uma transportadora
    this.alterarVisaoAssociacao = function (element) {

        var idOrganizacao = $(element).val();

        var callback = function (data) {

            Nav.zerarMenu();

            CarregadorPermissao.carregarPermissoes();

            CarregadorPermissao.carregarPermissoesTopo();

            CarregadorPermissao.redirecionar('#');

        };

        var Assync = new ObjAjax();

        Assync.init($("#baseUrlGeral").val() + 'associacoes/associacao/loadVisaoAssociacao', { "idOrganizacaoVisao": idOrganizacao }, callback, null, 'post', 'json', false);
    };

    this.checkNull = function (value, t) {
        t = !t || typeof t === "undefined" ? "" : t;
        return (!value || typeof value === "undefined" ? t : value);
    };

    this.getGroupCod = function (group) {
        return (!group || typeof group === "undefined" ? "" : "[data-group=" + group + "]");
    };

    this.limparCampos = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find("input[type=text]").val('');

        $(conteudo).find("select").each(function() {
        
            var combo = $(this);
                
            combo.val("").trigger("change");

        })

    };

    //Ocultar o menu principal
    this.hideMenuPrincipal = function () {

        $("body").addClass("sidebar-collapse");
    };
};

var DefaultSistema = new DefaultSistemaClass();


$(document).ready(function () {
    
    DefaultSistema.init();
    DefaultSistema.customValidators();
    DefaultSistema.serializeExtensions();
});
