function FroalaCustomClass() {

    var idBoxArquivoLista = "#boxArquivosListar";

    var baseUrl = $("#baseUrlGeral").val();

    this.init = function () {

        this.listenerEditores();

    };

    //Ouvinte para iniciar editores
    this.listenerEditores = function () {

        $(".froala-editor").each(function () {

            var elemento = $(this);

            FroalaCustom.iniciarEditor(elemento);
        });

    }

    //Inicializacao do plugin de edicao do campo textarea
    this.iniciarEditor = function (elementoFroala) {

        var urlUploadImagem = new String(baseUrl).concat("Arquivos/froalaupload/salvar-foto");

        var urlUploadFile = new String(baseUrl).concat("Arquivos/froalaupload/salvar-arquivo");

        $(elementoFroala).froalaEditor({

            language: 'pt_br',

            height: '350',

            imageUploadURL: urlUploadImagem,

            fileUploadURL: urlUploadFile,

            toolbarButtons: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],

            toolbarButtonsXS: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],

            toolbarButtonsSM: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],

            toolbarButtonsMD: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink']

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

    }


};

var FroalaCustom = new FroalaCustomClass();

$(document).ready(function () {
    FroalaCustom.init();
});
