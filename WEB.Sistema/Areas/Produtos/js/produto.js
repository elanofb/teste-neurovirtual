function ProdutoClass() {

    this.init = function () {
        Produto.listenFlagCalcularFrete();
        Produto.listenFlagInserirImpostos();
    };

    this.listenFlagCalcularFrete = function () {

        $("#divCalcularFrete").hide();

        $("#flagCalcularFrete").on("change", function () {
            if ($(this).val() == "S") {
                $("#divCalcularFrete").show();
            } else {
                $("#divCalcularFrete").hide();
            }
        });
    }

    this.listenFlagInserirImpostos = function () {

        $("#divImpostos").hide();

        $("#flagInserirImpostos").on("change", function () {
            if ($(this).val() == "S") {
                $("#divImpostos").show();
            } else {
                $("#divImpostos").hide();
            }
        });
    }

    //Exibir ou ocultar campos
    this.mostrarCampos = function (box, idMostrar, mostrar) {
        if ($(box).val() == mostrar) {
            $("#" + idMostrar).removeClass("hidden");
        } else {
            $("#" + idMostrar).addClass("hidden");
        }
    }

    //Exibir ou ocultar campos relacionados ao frete
    this.showCamposFrete = function () {

        var flagCalcularFrete = $("#flagCalcularFrete").val();

        var flagGratis = $("#flagFreteGratis").val();

        console.log(flagCalcularFrete);

        console.log(flagGratis);

        var boxFreteGratis = $(".frete-gratis");

        var boxDimensoes = $(".dimensoes");

        if (flagCalcularFrete == "True") {

            boxFreteGratis.show();

            console.log("teste");

            if (flagGratis == "True") {

                boxDimensoes.hide();

                return;
            }

            boxDimensoes.show();

            return;
        }
        boxFreteGratis.hide();

        boxDimensoes.hide();
    }
};

var Produto = new ProdutoClass();

$(document).ready(function () {
    Produto.init();
});
