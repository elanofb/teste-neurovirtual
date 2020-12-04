function TituloReceitaParcelamentoClass() {

    var selectorBoxParcelamento = "#box-form-parcelas";

    this.init = function () {

    };

	//Abrir modal para configurar o parcelamento de um titulo
    this.abrirModalParcelamento = function (elemento) {

        $("body").loadingOverlay();

        var url = $(elemento).data("url");

        $.get(url, {

        }, function (response) {

            var Modal = $(response).modal();

            $(Modal).on("shown.bs.modal", function (e) {

                TituloReceitaParcelamento.iniciarPluginsModal();

                $("body").loadingOverlay('remove');

            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });
		
    };

    //Carregar valor das parcelas
    this.carregarParcelas = function (elemento) {

        $(selectorBoxParcelamento).loadingOverlay();

        var url = $(elemento).data("url");

        var qtdeParcelas = $("#qtdeParcelas").val();

        $.get(url, {
            'qtdeParcelas': qtdeParcelas

        }, function (response) {

            console.log(response);
            
            $(selectorBoxParcelamento).html(response);

            $(selectorBoxParcelamento).loadingOverlay('remove');

            TituloReceitaParcelamento.iniciarPluginsModal();

        });


    }

    //Retorno apos enviar dados do parcelamento do pedido
    this.onSuccessParcelamento = function(response) {
        
        if (response.error === false) {

            location.reload(true);

            return;
        }

        TituloReceitaParcelamento.iniciarPluginsModal();
    }

    //Iniciar os plugins 
    this.iniciarPluginsModal = function () {

        iniciarDatePicker();

        $("input:text").setMask();

    }
};

var TituloReceitaParcelamento = new TituloReceitaParcelamentoClass();

$(document).ready(function () {
    TituloReceitaParcelamento.init();
});