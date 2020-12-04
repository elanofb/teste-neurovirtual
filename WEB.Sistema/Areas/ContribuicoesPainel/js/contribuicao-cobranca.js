function ContribuicaoCobrancaClass() {

    var idFormFiltro = "#formFiltro";
    var idInputAcao = "#flagAcao";

    //Metodo de inicializacao dos plugins
    this.init = function () {

    };

    //Gerar a cobrança para os associados ainda não relacionados
    this.gerarCobrancas = function (elemento, flagTodos) {

        var fYes = function () {

            $(".content").loadingOverlay();

            var url = $(elemento).data("url");

            var dados = $(idFormFiltro).serialize();

            if (flagTodos == false) {

                dados = new String(dados);

                var idsAssociados = [];

                $("input[name='checkRegistro[]']:checked").each(function () {

                    var idAssociado = $(this).val();

                    idsAssociados.push(idAssociado);

                    dados = dados.concat("&idsAssociados=").concat(idAssociado);
                    
                });

                if (idsAssociados.length == 0) {

                    jM.error("Selecione ao menos um associado.");

                    $(".content").loadingOverlay('remove');

                    return false;
                }

            }
            
            $.post(url, dados, function (response) {

                console.log(response);

                if (response.error == true) {

                    jM.error(response.message);

                    $(".content").loadingOverlay('remove');

                    return;
                }

                jM.success(response.message);

                $(".content").loadingOverlay('remove');

                location.reload(true);

            });
        };

        var fNo = function() {
            return false;
        }

        jM.confirmation('Deseja realmente gerar cobran&ccedil;a para esses associados?', fYes, fNo);
    }


    //Enviar e-mail de cobrança para todos da lista
    this.enviarCobranca = function (elemento) {

        var url = $(elemento).data("url");

        var dados = $(idFormFiltro).serialize();

        $.get(url, dados, function (response) {

            var Modal = $(response).modal();

            Modal.on("shown.bs.modal", function (e) {

            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });

    }

    //
    this.onSuccessForm = function(response) {
        
    }
};

var ContribuicaoCobranca = new ContribuicaoCobrancaClass();


$(document).ready(function () {

    ContribuicaoCobranca.init();

});
