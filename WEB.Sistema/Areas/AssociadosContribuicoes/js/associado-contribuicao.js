function AssociadoContribuicaoClass() {

    var idBoxLista = "#boxLoadListaCobranca";
    var idBoxForm = "#boxLoadFormCobranca";
    var idBoxOpcaoVencimento = "#boxOpcoesVencimento";

    this.init = function () {

        this.carregarLista();
        
        this.iniciarPlugins();
    };

    //
    this.iniciarPlugins = function () {

        var elemento = $(idBoxForm);

        if (elemento.length > 0) {

            $(elemento).find("input:text").setMask();

            AssociadoContribuicao.listenerCampoVencimento();
        }
    }

    //Ouvinte do campo de vencimentos
    this.listenerCampoVencimento = function () {

        var options = new Array();
        options['language'] = 'pt-BR';
        options['format'] = 'dd/mm/yyyy';
        options['yearRange'] = '2010:2022';
        options['changeMonth'] = true;
        options['changeYear'] = true;
        options['autoclose'] = true;
        options['minDate'] = '01/01/2010';

        var inputDtVencimento = $("#dtVencimentoOriginal");

        var inputDtNovoVencimento = $("#dtVencimentoAtual");

        if (inputDtVencimento.val() === '01/01/0001') {

            inputDtVencimento.val('');

            inputDtNovoVencimento.val('');
        }

        var dtVencimentoPicker = inputDtVencimento.datepicker(options);

        var dtNovoVencimentoPicker = inputDtNovoVencimento.datepicker(options);

        dtVencimentoPicker.on("hide", function () {

            AssociadoContribuicao.validarVencimento();

        });

    };

    //Carregamento via AJax do formulario
    this.carregarForm = function () {
        var elemento = $(idBoxForm);
        var url = elemento.data("url");

        elemento.addClass("carregando");

        $.get(url, {},
            function (response) {

                elemento.html(response);

                elemento.removeClass("carregando");

                DefaultSistema.reiniciarBotao();

                $("input:text").setMask();

                AssociadoContribuicao.listenerCampoVencimento();
            }
        )
    };

    //Carregamento da lista de documentos existentes
    this.carregarLista = function () {
        var elemento = $(idBoxLista);
        var url = elemento.data("url");

        elemento.addClass("carregando");

        $.get(url, {},
            function(response) {
                elemento.html(response);

                elemento.removeClass("carregando");

                $(elemento).find("[data-toggle='tooltip']").tooltip();

                DefaultSistema.iniciarLinksAcao($(idBoxLista));
            }
        );
    };


    //Pre-carregar valores para o associado
    this.carregarPreco = function (idContribuicao, idAssociado) {

        var url = new String($("#baseUrlGeral").val()).concat("associadoscontribuicoes/associadocontribuicaopreco/buscar-preco");

        $(idBoxOpcaoVencimento).hide();

        if (idContribuicao === "") {
            $("#valorContribuicao").val("");
            $("#dtInicioVigencia").val("");
            $("#dtFimVigencia").val("");
            $("#dtVencimentoOriginal").val("");
            $("#dtVencimentoAtual").val("");

            return;
        }

        $.post(url, {
            'idContribuicao': idContribuicao,
            'idAssociado': idAssociado
        }, function (response) {

            console.log(response);
            
            if (response.error === true) {

                jM.error(response.message);

                return;
            }


            var inputValor = $("#valorContribuicao");

            inputValor.val(response.valor);

            if (response.flagVencimentoFixo) {
                AssociadoContribuicao.carregarVencimentos(idContribuicao);
            } else {
                $("#dtInicioVigencia").val(response.dtInicioVigencia);
                $("#dtFimVigencia").val(response.dtFimVigencia);
                $("#dtVencimentoOriginal").val(response.dtVencimento);
                $("#dtVencimentoAtual").val(response.dtVencimento);
            }

            if (response.flagIsento == true) {
                $("#boxAssociadoIsento").show();
                $("#AssociadoContribuicao_flagIsento").val("True");
            } else {
                $("#boxAssociadoIsento").hide();
                $("#AssociadoContribuicao_flagIsento").val('False');
            }
        });
    }

    //

    //Carregar as datas de vencimento possíveis para a contribuicão
    this.carregarVencimentos = function (idContribuicao) {

        var url = new String($("#baseUrlGeral").val()).concat("contribuicoes/contribuicaovencimento/buscar-vencimentos");

        $(idBoxOpcaoVencimento).hide();

        $.post(url, {

            'idContribuicao': idContribuicao

        }, function (response) {

            if (response.error === true) {

                jM.error(response.message);

                return;
            }

            if (response.flagVencimentoFixo === true) {

                $(idBoxOpcaoVencimento).find("ul.list-group").html('');

                $.each(response.listaVencimentos, function (index, value) {

                    $(idBoxOpcaoVencimento).find("ul.list-group").append("<li class='list-group-item text-center'>" + value + "</li>");

                });

                $(idBoxOpcaoVencimento).show();
            }

        });
    }

    //Carregar as datas de vencimento possíveis para a contribuicão
    this.validarVencimento = function () {

        var elemento = $("#dtVencimentoOriginal");

        var dtVencimento = $(elemento).val();

        var idContribuicao = $("#AssociadoContribuicao_idContribuicao").val();

        if (dtVencimento.length < 10 || idContribuicao == '' || idContribuicao == '0') {
            return;
        }

        var url = new String($("#baseUrlGeral").val()).concat("contribuicoes/contribuicaovencimento/validar-vencimento");

        $.post(url, {

            'idContribuicao': idContribuicao,
            'dtVencimento': dtVencimento

        }, function (response) {

            console.log(response);
            if (response.error === true) {

                jM.error(response.message);

                return;
            }

            var inputInicioVigencia = $("#dtInicioVigencia");

            var inputFimVigencia = $("#dtFimVigencia");

            inputInicioVigencia.val(response.dtInicioVigencia);

            inputFimVigencia.val(response.dtFimVigencia);

        });
    }

    //Retorno após submissao do formulario de cadastro
    this.onSuccessForm = function (response) {

        try {
            if (response.flagSucesso == true || response.flagSucesso == false) {

                AssociadoContribuicao.carregarForm();

                AssociadoContribuicao.carregarLista();

                return;
            }

            AssociadoContribuicao.listenerCampoVencimento();

            DefaultSistema.iniciarPluginsAposAjax($(idBoxForm));

        } catch (e) {
            console.log(e);
        }
    };


};

var AssociadoContribuicao = new AssociadoContribuicaoClass();

$(document).ready(function(){
    AssociadoContribuicao.init();
});
