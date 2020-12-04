function ObjMovimentacao() {

    this.init = function () {
        this.iniciarDatepicker();
    }

    this.iniciarDatepicker = function () {
        $("#dtOperacao").daterangepicker({ format: 'DD/MM/YYYY', singleDatePicker: true, "opens": "left" });
    };
}

var Movimentacao = new ObjMovimentacao();

$(document).ready(function () {
    Movimentacao.init();
});