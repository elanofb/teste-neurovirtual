function ObjConta() {

    this.init = function () {
        this.deletar();
    }

    this.deletar = function () {
        $(".excluir-conta").on("click", function (element) {

            var id = $(this).attr("data-id");
            var url = $(this).attr("data-url");

            var fYes = function () {

                $.post(url, { 'id': id }, function (response) {
                    $("#conta-" + id).hide();
                })

            };

            var fNo = function () {
                return false;
            };

            jM.confirmation("Confirma a exclus&atilde;o do registro?", fYes, fNo);

            return false;
        });
    };
}

var Conta = new ObjConta();

$(document).ready(function () {
    Conta.init();
});