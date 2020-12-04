function ObjContaBancaria() {

    var idInputNroDocumento = "#documentoTitular";

    this.init = function () {
        this.listenerTipoDocumento();
    }


    //Ouvinte do combo de tipos de documentos
    this.listenerTipoDocumento = function () {

        $(idInputNroDocumento).keydown(function () {
            try {
                $(idInputNroDocumento).unmask();
            } catch (e) { }

            var tamanho = $(idInputNroDocumento).val().length;
            console.log(tamanho);
            if (tamanho < 11) {
                $(idInputNroDocumento).mask("999.999.999-99");
            } else {
                $(idInputNroDocumento).mask("99.999.999/9999-99");
            }
        });

    }

}

var ContaBancaria = new ObjContaBancaria();

$(document).ready(function () {

    ContaBancaria.init();
});