function DefaultCheckoutClass() {

    this.init = function () {

        //Plugins
        this.iniciarPlugins();

        //
        this.esconderBoxesMobile();        
        
    };


    // Iniciar os plugins diversos do sistema usando as bibliotecas
    this.iniciarPlugins = function (conteudo) {

        if (!conteudo) {
            conteudo = $("body");
        }

        $(conteudo).find('input:text').setMask();

        this.iniciarComplementoMask();

        this.zerarErros();
    }

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
    this.isMobile = function () {

        if( /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) ) {
            
            return true;
            
        }

        return false;
        
    }
    
    //
    this.esconderBoxesMobile = function () {
        
        if (!this.isMobile()) {
            return;
        }
        
        $(".nao-exibir-mobile").hide();
        
    }


};

var DefaultCheckout = new DefaultCheckoutClass();


$(document).ready(function () {

    DefaultCheckout.init();
});

var Vocabulary = {
    'project_title': 'SS Reversa Express',

    'confirmUpdateStatus' : 'Confirma a altera&ccedil;&atilde;o do Status?',
    'confirmDelete': 'Tem certeza que deseja efetuar a exclus&atilde;o?<br /> Essa opera&ccedil;&atilde;o n&atilde;o poder&aacute; ser desfeita!',

    'invalid_cep': 'O CEP informado � inv&aacute;lido, tente novamente!',
    'unsuported_cep': 'O CEP informado n&atilde;o &eacute; atendido no momento!',

    'loading' : 'Carregando...',

    'select_state' : 'Por favor, selecione um estado.!',
    'select_newsletter' : 'Por favor, escolha ao menos uma not�cia para criar a newsletter.',
    'select' : 'Selecione...'
};
var ObjAjax = function () {

    this.vars = {
        method: 'post',
        returnType: 'json',
        callback: null,
        alertMessageDefault: null,
        urlAction: '',
        params: {}
    }

    this.init = function (url, params, callback, confirm, method, returnType, alertMessageDefault) {
        this.vars.urlAction = url;
        this.vars.params = params;
        this.vars.callback = (callback == '' || callback == undefined ? null : callback);
        this.vars.alertMessageDefault = (alertMessageDefault === false ? false : true);
        this.vars.method = (method == '' || method == undefined ? 'post' : method);
        this.vars.returnType = (returnType == '' || returnType == undefined ? 'json' : returnType);
        
        if (confirm != null && confirm != 'undefined') {
            fYes = function () { Ajax.send(); };
            fNo = function () { return false; };
            jM.confirmation(confirm, fYes, fNo);
        } else {
            this.send();
        }
    }

    this.send = function () {
        var options = this.vars;
        $.ajax({
            type: this.vars.method,
            data: this.vars.params,
            url: this.vars.urlAction,
            dataType: this.vars.returnType,
            traditional: true,
            success: function (data) {
                //Se n�o houver fun��o callback atribui-se "false"
                if (options.callback == null) {
                    options.callback = function (data) { return false; };
                }
                //alert((options.alertMessageDefault == true));
                if (options.alertMessageDefault === true) {
                    if (data == null) {
                        options.callback(data);
                    } else {
                        if ((data.error != 'undefined' && data.error == true) || (data.flagError != 'undefined' && data.flagError == true)) {

                            if (typeof (data.message) != 'undefined') {
                                jM.error(data.message);
                            } else if (data.listaErros != undefined && data.listaErros.length > 0) {
                                jM.error(data.listaErros.join("<br/>"));
                            } else {
                                jM.error("Erro durante a opera&ccedil;&atilde;o");
                            }
                        } else {
                            //Se houver mensagem de retorno abre-se o dialog e depois executa a fun��o callback, caso contr�rio apenas dispara a fun��o
                            if (typeof (data.message) != 'undefined') {
                                jM.success(data.message);
                            } else if (data.listaErros != undefined && data.listaErros.Length > 0) {
                                jM.success(data.listaErros.join("<br/>"));
                            }
                            options.callback(data);
                        }
                    }
                } else {
                    options.callback(data);
                }
            },
            error: function (err) {
                console.dir(err);
                //alert(err.message);
                //jM.error('ERROR '+err.toString());
            }
        });
    };


    //
    this.updateAjaxList = function (element, url, params) {
        var func = function (data) {
        	$(element).html(data);
        	$(element).find("input:text").setMask();
        	DefaultSistema.iniciarLinksAcao();
        	try {
        		$(element).find(".datepicker").datepicker({
        			format: "dd/mm/yyyy",
        			todayBtn: "linked",
        			language: "pt-BR",
        			orientation: "bottom left",
        			autoclose: true,
        			todayHighlight: true
        		});
        	} catch (e) {
        	}
        };
        var Assync = new ObjAjax();
        Assync.init($("#baseUrlGeral").val() + url, params, func, null, "post", "html", false);
    };


}

var Ajax = new ObjAjax();