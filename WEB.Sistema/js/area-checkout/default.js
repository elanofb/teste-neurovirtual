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
