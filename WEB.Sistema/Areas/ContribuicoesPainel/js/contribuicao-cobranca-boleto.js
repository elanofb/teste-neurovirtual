function ContribuicaoCobrancaBoletoClass() {

    var idFormFiltro = "#formFiltro";
    var idInputAcao = "#flagAcao";

    //Metodo de inicializacao dos plugins
    this.init = function () {

    };


    //Gerar a cobrança para os associados ainda não relacionados
    this.gerarBoletos = function (elemento, flagTodos) {

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

                jM.success(response.message, function () { location.reload(true);});

                $(".content").loadingOverlay('remove');

            });

        };

        var fNo = function() {
            return false;
        }

        jM.confirmation('Deseja realmente gerar boletos para esses associados?', fYes, fNo);
    }


    //
    this.onSuccessForm = function(response) {
        
    }
};

var ContribuicaoCobrancaBoleto = new ContribuicaoCobrancaBoletoClass();


$(document).ready(function () {

    ContribuicaoCobrancaBoleto.init();

});
