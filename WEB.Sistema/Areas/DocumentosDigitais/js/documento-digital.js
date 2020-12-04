function DocumentoDigitalClass() {

    this.init = function () {

        this.iniciarEditores();

    }

    //
    this.iniciarEditores = function () {

        var urlUploadImagem = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-foto");
        var urlUploadFile = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-arquivo");

        $('#editor').froalaEditor({
            language: 'pt_br',
            height: '350',
            imageUploadURL: urlUploadImagem,
            fileUploadURL: urlUploadFile
        })
            .on('froalaEditor.file.beforeUpload', function (e, editor, files) {
            })
            .on('froalaEditor.file.uploaded', function (e, editor, response) {
            })
            .on('froalaEditor.file.inserted', function (e, editor, $file, response) {
            })
            .on('froalaEditor.file.error', function (e, editor, error, response) {
                console.log(e);
                console.log(error);
                console.log(response);
                if (error.code === 6) {
                    jM.error("O arquivo informado &eacute; inv&aacute;lido!");
                }
            })
            .on('froalaEditor.image.error', function (e, editor, error) {
                if (error.code === 6) {
                    jM.error("O arquivo informado &eacute; inv&aacute;lido!");
                }
            })
        ;

    };

}

var DocumentoDigital = new DocumentoDigitalClass();

$(document).ready(function () {

    DocumentoDigital.init();

});