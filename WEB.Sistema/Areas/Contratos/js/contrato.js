function ObjContrato() {

    var idCampoOperacaoFinanceira = "#flagOperacaoFinanceira";
    var boxCentroCusto = ".boxFinanceiro";

    var flagOperacaoDebito = "D";
    var flagOperacaoCredito = "C";

    this.init = function () {
        this.verificarOperacaoFinanceira();
        this.listenFlagInserirImpostos();
    }

    //Exibir ou ocultar campos
    this.mostrarCampos = function (box, idMostrar, mostrar) {
        if ($(box).val() == mostrar) {
            $("#" + idMostrar).removeClass("hidden");
        } else {
            $("#" + idMostrar).addClass("hidden");
        }
    }

    this.verificarOperacaoFinanceira = function () {
        var flagOperacaoFinanceira = $(idCampoOperacaoFinanceira).val();

        if (flagOperacaoFinanceira == flagOperacaoDebito) {
            $(boxCentroCusto).removeClass("hide");

            $(".debito").removeClass("hide");
            $(".credito").addClass("hide");
        }

        if (flagOperacaoFinanceira == flagOperacaoCredito) {
            $(boxCentroCusto).addClass("hide");

            $(".debito").addClass("hide");
            $(".credito").removeClass("hide");
        }
    }

    this.exibirMensagemConfirmacao = function () {

        bootbox.dialog({
            message: "Todos os dados do contrato estão corretos? Se não tiver certeza, revise os dados.",
            title: "Confirma&ccedil;&atilde;o",
            buttons: {

                main: {
                    label: "Revisar",
                    className: "btn-default",
                    callback: function () { }
                },
                success: {
                    label: "Sim",
                    className: "btn-primary",
                    callback: function () {
                        $("form#form_contrato").submit();
                    }
                }
            }
        }); 

    }

}

var Contrato = new ObjContrato();

$(document).ready(function () {
    Contrato.init();
});