function ObjPlano() {

    this.init = function () {

        $('#idPessoa').val($("#idContratante").val()).change();

        CKEDITOR.replace('editor',
            {
                toolbar: 'Custom',
                height: 700
            }
        );    };

};

var Plano = new ObjPlano();

$(document).ready(function () {
    Plano.init();
});
