function ContribuicaoPainelCobrancaClass() {

    var baseUrl;

    //Metodo de inicializacao dos plugins
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        this.listenerComboContribuicoes();

    };

    //Ouvinte do combo de vencimentos da contribuição
    this.listenerComboContribuicoes = function () {

        $("#boxVencimentoVariado").hide();

        var combo = $("#idContribuicao");

        combo.on("change", function() {

            ContribuicaoPainelCobranca.carregarVencimentos($(this).val());

        });

        ContribuicaoPainelCobranca.carregarVencimentos($(combo).val());
    };

    //Carregar as datas de vencimento possíveis para a contribuicão
    this.carregarVencimentos = function (idContribuicao) {

        var url = new String($("#baseUrlGeral").val()).concat("contribuicoes/contribuicaovencimento/buscar-vencimentos");

        var comboVencimento = $("#dtVencimento");

        var selectedValue = $(comboVencimento).data("selected");

        var boxVencimento = $(".boxVencimento");
        var boxVencimentoVariado = $("#boxVencimentoVariado");

        boxVencimento.hide();
        boxVencimentoVariado.hide();

        $.post(url, {

            'idContribuicao': idContribuicao

        }, function (response) {
            console.log(response);

            comboVencimento.find("option").remove();

            if (response.error === true) {

                //jM.error(response.message);

                return;
            }

            if (response.qtdeMeses >= 12 && response.flagVencimentoFixo) {

                boxVencimento.hide();
                boxVencimentoVariado.hide();

                return;
            }
            

            if (response.qtdeMeses >= 12 && response.flagVencimentoFixo === false) {

                boxVencimento.hide();
                boxVencimentoVariado.show();

                return;
            }

            if (response.flagVencimentoFixo === true) {

                boxVencimento.show();
                boxVencimentoVariado.hide();

                $.each(response.listaVencimentos, function (index, value) {

                    $(comboVencimento).append(new Option(value, value));

                });

                $(comboVencimento).val(selectedValue);
            }

        });
    }

    //Exibir o menu de context com as ações de cada bloco
    this.toogleMenuAcoes = function (elemento) {

        var divWrapper = $(elemento).parent(".dropdown-custom");

        var subMenu = $(divWrapper).find("div.dropdown-content");

        var flagIsVisible = subMenu.is(":visible");

        $("div.dropdown-content").removeClass("active-menu");

        if (!flagIsVisible) {

            subMenu.addClass("active-menu");

        }
    }

    // Abrir modal para enviar e-mail de cobrança a todos os associados com a contribuição "Em Aberto"
    this.abrirModalGeracaoEmailCobrancaTodos = function (id) {

        var dados = $("#formFiltro").serialize();

        dados['idCocontribuicao'] = id;
        
        var url = this.baseUrl.concat("AssociadosNotificacoes/AssociadoContribuicaoCobranca/modal-gerar-email-cobranca-todos");

        $.post(url, dados, function(data) {

            var Modal = $(data).modal();

            $(Modal).on("shown.bs.modal", function(e) {

                DefaultSistema.reiniciarBotao();

                $("#boxGeracaoNotificacaoCobranca").find("#emailCobrancaHtml").froalaEditor({
                    height: 250,
                });

            });

            $(Modal).on("hidden.bs.modal", function(e) {

                $(this).remove();

            });

        });

    }

    // Abrir modal para enviar e-mail de cobrança a todos os associados com a contribuição "Em Aberto"
    this.exportarDadosExcel = function () {

        $("#tipoSaida").val('Excel');
        $(".bt-submit").trigger('click');
        $("#tipoSaida").val('');
    }

};

var ContribuicaoPainelCobranca = new ContribuicaoPainelCobrancaClass();


$(document).ready(function () {

    ContribuicaoPainelCobranca.init();

});
