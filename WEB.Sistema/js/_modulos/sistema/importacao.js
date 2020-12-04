function ObjImportacao() {

    this.init = function () {
        this.setInputFile();
    }

    this.setInputFile = function () {
        $('#FileUpload').fileinput({
            showPreview: false
        });
    };
}

var Importacao = new ObjImportacao();

$(document).ready(function () {

    Importacao.init();
});