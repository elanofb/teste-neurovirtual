function ContribuicaoCadastroClass() {

    var idCampoID = "#id";
    var idCampoPost = "#flagIsPost";
    var idComboVencimento = "#idTipoVencimento";
    var idComboPeriodo = "#idPeriodoContribuicao";
    var idComboGeracaoAutomatica = "#flagGerarCobrancaAutomatica";
    var idBoxVencimentos = "#boxVencimentos";
    var idComboflagCobrancaProRata = "#flagCobrancaProRata";
    
    var classBoxGeracaoAutomatica = ".box-geracao-automatica";
    var classBoxVencimentoFixo = ".box-vencimento-fixo";
    

    //Metodo de inicializacao dos plugins
    this.init = function () {

        this.iniciarEditorEmailCobranca();

        this.listenerPopover();

        this.onChangeComboPeriodo();

        this.onChangeComboVencimento();

        this.onChangeComboGeracaoAutomatica();
    };

    //Iniciar editor de conteudo
    this.iniciarEditorEmailCobranca = function () {

        $('#emailCobranca').froalaEditor({
            language: 'pt_br',
            height: 200,
            toolbarButtons: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsXS: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsSM: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsMD: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink']
        });

        $('#emailPagamento').froalaEditor({
            language: 'pt_br',
            height: 200,
            toolbarButtons: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsXS: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsSM: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsMD: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink']
        });
    }

    //Iniciar plugin popover
    this.listenerPopover = function () {

        $('#email-cobranca').popover({
            container: 'body',
            placement: 'top',
            html: true,
            content: function () {
                return $("#hashtags-email-cobranca").html();
            }
        });

        $('#email-pagamento').popover({
            container: 'body',
            placement: 'top',
            html: true,
            content: function () {
                return $("#hashtags-email-pagamento").html();
            }
        });

        //$('.for-popover').each(function () {
        //    var pop = $(this);
        //    var divConteudo = pop.data("url");
        //    var titulo = pop.data("title");
        //    pop.popover({
        //        title: (titulo),
        //        html: true,
        //        container: 'body',
        //        content: function () {
        //            return $(divConteudo).html();
        //        }
        //    });
        //});

    };

    //Listener
    this.onChangeComboPeriodo = function () {

        $(idComboPeriodo).on("change", function () {
            
            ContribuicaoCadastro.showBoxVencimentos();

        });

    }

    //Listener
    this.onChangeComboVencimento = function () {

        $(idComboVencimento).on("change", function () {

            ContribuicaoCadastro.showBoxVencimentos();
        });

        $(idComboVencimento).trigger("change");
    }

    //Listener
    this.onChangeComboGeracaoAutomatica = function () {

        $(idComboGeracaoAutomatica).on("change", function () {
            
            var flagGeracao = $(this).val();
            
            if (flagGeracao == "True") {
                $(classBoxGeracaoAutomatica).show();
                return;
            }

            $(classBoxGeracaoAutomatica).hide();
        });

        $(idComboGeracaoAutomatica).trigger("change");
    }


    //Exibir ou ocultar o box das datas de vencimentos
    this.showBoxVencimentos = function () {

        var flagPost = $("#flagIsPost").val();

        var id = $("#id").val();

        if (flagPost == "True" && id == 0) {

            return id;
        }

        var idTipoVencimento = $(idComboVencimento).val();

        var idPeriodoVencimento = $(idComboPeriodo).val();

        var boxVencimentos = $(idBoxVencimentos);

        var boxVencimentoFixo = $(classBoxVencimentoFixo);

        //idTipoVencimento = 1 Por admissao
        //idTipoVencimento = 4 Por ultima contribuicao
        //Regra para impedir que o usuário selecione vencimentos para outro periodo que não seja anual.
        if ((idTipoVencimento == "1" || idTipoVencimento == "4")
            && idTipoVencimento != "" && idPeriodoVencimento != 7 && idPeriodoVencimento != "") {

            $(idComboVencimento).val("");
            jM.error("Vencimento por admiss&atilde;o ou por &uacuteltima contribui&ccedil;&atilde;o dispon&iacute;vel somente para o periodo de cobran&ccedil;a anual");
            return;
        }

        //Pela ultima contribuição ou pela data de admissão
        if (idTipoVencimento == "1" || idTipoVencimento == "4") {

            $(idComboflagCobrancaProRata).val("False");
            boxVencimentos.hide();
            boxVencimentos.html("");
            boxVencimentoFixo.hide();
            return;
        }

        var id = $(idCampoID).val();

        if (id > 0) {
            return;
        }

       var url = new String($("#baseUrlGeral").val()).concat("contribuicoes/contribuicaovencimento/partial-box-vencimentos");

        $.get(url,
            {
                'idPeriodoContribuicao': idPeriodoVencimento
            },
            function (response) {
                console.log(response);
                boxVencimentos.html(response);

                boxVencimentos.show();

                boxVencimentoFixo.show();


                $("input:text").setMask()

                iniciarDatePicker();
            });
    }

    this.listenFlagInserirImpostos = function () {
        
        var flag = $("#flagConfigurarImpostos").val();

        if (flag == "True") {
            $("#boxImpostos").removeClass("hide");
        }

        if (flag == "False") {
            $("#boxImpostos").addClass("hide");
        }
    }
};

var ContribuicaoCadastro = new ContribuicaoCadastroClass();

$(document).ready(function(){
    ContribuicaoCadastro.init();
});
